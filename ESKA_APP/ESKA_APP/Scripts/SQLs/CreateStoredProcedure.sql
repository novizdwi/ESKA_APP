CREATE PROCEDURE "SpApproval_Authorize" 
(
	UserId INT, -- id Approver
	ObjectCode NVARCHAR(200), -- AdjustmentIn, AdjustmentOut, TransferSummaryOut, etc
	BaseId BIGINT, --Id of object code
	ActionMethod NVARCHAR(50), -- Approve, Reject
	ApprovalMessage NVARCHAR(500) --action Remarks
)
AS
BEGIN 
	DECLARE flag int; 
	  	
	DECLARE error  INT	;			-- Result (0 for no error)
	DECLARE error_message nvarchar (200) ;		-- Error string to be displayed
	DECLARE SapDb NVARCHAR(50);
	DECLARE SSQL NVARCHAR(5000);
	DECLARE MainTable NVARCHAR(200);
	DECLARE ApprovalTable NVARCHAR(200);
	
	DECLARE TblStatus NVARCHAR(50);
	
	DECLARE StageId INT;
	DECLARE DetId BIGINT;	
	DECLARE Step INT;
    
	DECLARE IsLastStage CHAR(1);
	
	DetId   = NULL;
    StageId = NULL;
    Step    = NULL;
    
	MainTable := 'Tx_' || :ObjectCode;	
	ApprovalTable := 'Tx_' || :ObjectCode || '_Approval';
	
	SELECT "SpSysGetSapDb"() INTO SapDb FROM DUMMY;	
	CREATE LOCAL TEMPORARY TABLE "#TempTx_Approval"( 
		"Id" BIGINT,
		"DetId" BIGINT,
		"StageId" INT,
		"Step" INT,
		"Status" NVARCHAR(200)		
	); 
	TblStatus := CASE WHEN ActionMethod = 'Approve' THEN 'Approved' ELSE 'Rejected' END ;
	
	--get det id, template id, stage id and step from approval table	
	SSQL := '
	    SELECT 
	        T0."DetId", 	  	   
	        T0."StageId", 
	        T0."Step"
	    FROM "Tx_' || :ObjectCode || '_Approval" T0
	    WHERE  T0."Id" = ?
	      AND T0."UserId" = ?
	';

	EXECUTE IMMEDIATE :SSQL
	INTO DetId, StageId, Step
	USING BaseId, UserId
	;
	
	--update approval table
	SSQL := '
	    UPDATE "' || :ApprovalTable || '" SET
	        "Status" = ?,
	        "Comments" = ?,
	        "ActionDate" = CURRENT_TIMESTAMP,
	        "ModifiedDate" = CURRENT_TIMESTAMP,
	        "ModifiedUser" = ?
	    WHERE "Status" = ''Waiting''
	    AND "Id" = ?
	    AND "DetId" = ?	
	    AND "UserId" = ?
		';
		
	EXECUTE IMMEDIATE :SSQL USING TblStatus, ApprovalMessage, UserId, BaseId, DetId, UserId;
	
	SSQL = '
		INSERT INTO "#TempTx_Approval"(	
			"Id",
			"DetId",
			"StageId",
			"Step",
			"Status"
		)
		SELECT "Id", "DetId", "StageId", "Step", "Status"
		FROM "'|| :ApprovalTable ||'" T0
		WHERE "Id"  = ?
		;	
	';
	EXECUTE IMMEDIATE :SSQL USING :BaseId;
	
	IF :ActionMethod = 'Approve' THEN
		IF EXISTS(
			SELECT 1
			FROM "#TempTx_Approval" T0
			INNER JOIN "Tm_ApprovalStage" T1 ON T0."StageId" = T1."Id"
			WHERE T0."Status" = 'Approved' 
				AND T0."Step" = :Step 
				AND T0."Id" = :BaseId
			GROUP BY T1."MinApprove"
			HAVING COUNT(*) >= T1."MinApprove"
		) THEN
		/*
			SSQL := '
			    DELETE FROM "' || :ApprovalTable || '" T0
			    WHERE T0."Status" = ''Waiting''
			      AND T0."Id" = ?
			      AND T0."Step" = ?';
			
			EXECUTE IMMEDIATE :SSQL USING BaseId, Step;
		*/
		
		SSQL := '
		    UPDATE "' || :ApprovalTable || '" SET
		        "Status" = ''Skipped'',
		        "ModifiedDate" = CURRENT_TIMESTAMP,
		        "ModifiedUser" = ?
		    WHERE "Status" = ''Waiting''
		    AND "Id" = ?
		    AND "Step" =  ? 
		    
		';
		EXECUTE IMMEDIATE :SSQL USING UserId, BaseId, Step;
	
			IF EXISTS(
				SELECT 1
				FROM "#TempTx_Approval" T0
				WHERE "Step" > :Step
			)THEN
				SSQL := '
				    UPDATE "' || :ApprovalTable || '" SET
				        "Status" = ''Waiting''
				    WHERE "Id" = ?
				    AND "Step" = (
				    	SELECT MIN("Step") 
				    	FROM "#TempTx_Approval" Tx 
				    	WHERE Tx."Step" > ?
			    	)
			    ';			
				EXECUTE IMMEDIATE :SSQL USING BaseId, Step;						
			ELSE
				IsLastStage = 'Y';
			END IF;

			
		END IF
		;
	ELSEIF :ActionMethod = 'Reject' THEN
		IF EXISTS(
			SELECT 1
			FROM "#TempTx_Approval" T0
			INNER JOIN "Tm_ApprovalStage" T1 ON T0."StageId" = T1."Id"
			WHERE T0."Status" = 'Rejected' 
				AND T0."Step" = :Step 
				AND T0."Id" = :BaseId
			GROUP BY T1."MinReject"
			HAVING COUNT(*) >= T1."MinReject"
		) THEN
			SSQL := '
			    UPDATE "' || :MainTable || '" SET
			        "ApprovalStatus" = ?,
			        "Status" = ''Cancel'',
			        "ModifiedDate" = CURRENT_TIMESTAMP,
			        "ModifiedUser" = ?
			    WHERE "Id" = ?';
			
			EXECUTE IMMEDIATE :SSQL USING TblStatus, UserId, BaseId;

		SSQL := '
		    UPDATE "' || :ApprovalTable || '" SET
		        "Status" = ''Skipped'',
		        "ModifiedDate" = CURRENT_TIMESTAMP,
		        "ModifiedUser" = ?
		    WHERE "Status" = ''Waiting''
		    AND "Id" = ?
		    AND "Step" =  ?
		    
		';
		EXECUTE IMMEDIATE :SSQL USING UserId, BaseId, Step;
		END IF;

	END IF;
	
	IF IsLastStage = 'Y' THEN
		SSQL = '
			UPDATE "' || :MainTable || '" SET 
				"ApprovalStatus" = '''|| :TblStatus ||''',
				"ModifiedDate" = CURRENT_TIMESTAMP,
				"ModifiedUser" = '||:UserId||'
			WHERE "Id" = '||:BaseId||'
		'
		;
		EXEC (:SSQL);
		COMMIT;
	END IF; 
	
	DROP TABLE "#TempTx_Approval";
END 
;

CREATE PROCEDURE "SpApproval_CheckNeedApproval"
(
    UserId     INT,
    ObjectCode NVARCHAR(100),
    BaseId     BIGINT
)
LANGUAGE SQLSCRIPT
AS
BEGIN

    DECLARE SapDb NVARCHAR(50);
    DECLARE SQL_STMT  NVARCHAR(5000);
    DECLARE Result NVARCHAR(1) := 'N';
	DECLARE TemplateId INT;
	
    DECLARE i INT := 1;
    DECLARE MaxRow INT := 0;
    DECLARE TempIdLooping INT;

    -- Ambil SAP DB
    SELECT "SpSysGetSapDb"() INTO SapDb FROM DUMMY;

    -- Temp table (pengganti table variable)
    CREATE LOCAL TEMPORARY TABLE "#TempApprovalTemplate"
    (
        "RowNo"      INT,
        "TemplateId" INT,
        "Sqls"       NVARCHAR(5000)
    );

    INSERT INTO "#TempApprovalTemplate"
    SELECT 
        ROW_NUMBER() OVER (ORDER BY Tx."Id") AS "RowNo",
        Tx."Id",
        Tx."Sql"
    FROM
    (
        SELECT T0."Id", T0."Sql"
        FROM "Tm_ApprovalTemplate" T0
        INNER JOIN "Tm_ApprovalTemplate_User" T1 
            ON T0."Id" = T1."Id"
        WHERE T1."UserId" = :UserId
          AND T0."ObjectCode" = :ObjectCode
          AND IFNULL(T0."IsActive",'Y') = 'Y'
          AND EXISTS(SELECT 1 FROM "Tm_ApprovalTemplate_Stage" Tx WHERE Tx."Id" = T0."Id")
    ) Tx;

    SELECT MAX("RowNo") INTO MaxRow FROM"#TempApprovalTemplate";

    TemplateId := -1;

    WHILE :i <= 10 AND :i <= :MaxRow DO

        SELECT 
            "TemplateId",
            "Sqls"
        INTO
            TempIdLooping,
            SQL_STMT
        FROM "#TempApprovalTemplate"
        WHERE "RowNo" = :i;

        -- Replace parameter
        SQL_STMT := REPLACE(:SQL_STMT, '{@Id}', TO_NVARCHAR(:BaseId));
        SQL_STMT := REPLACE(:SQL_STMT, '{@UserId}', TO_NVARCHAR(:UserId));
        SQL_STMT := REPLACE(:SQL_STMT, '{DbSap}', :SapDb);

        -- Execute dynamic SQL
        EXECUTE IMMEDIATE :SQL_STMT INTO Result;

        IF :Result = 'Y' THEN
            TemplateId := :TempIdLooping;
            BREAK;
        END IF;

        i := :i + 1;

    END WHILE;

    DROP TABLE "#TempApprovalTemplate";
	SELECT TemplateId FROM DUMMY;
	
END;

CREATE PROCEDURE "SpApproval_Insert"
(
    UserId     INT,
    ObjectCode NVARCHAR(100),
    BaseId     BIGINT,
    TemplateId INT
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    DECLARE SapDb NVARCHAR(50);
    DECLARE SSQL NVARCHAR(5000);
    DECLARE MinStage INT = 1;
    
    SELECT "SpSysGetSapDb"() INTO SapDb FROM DUMMY;
    SELECT MIN("SortCode") INTO MinStage FROM "Tm_ApprovalTemplate_Stage" WHERE "Id" = :TemplateId;
    
    SSQL = '
	INSERT INTO "Tx_'||:ObjectCode||'_Approval"(
		"Id",
		"StageId",
		"UserId",
		"Step",
		"Status",
		"CreatedDate",
		"CreatedUser"
	)
	SELECT
		'|| :BaseId ||',
		T2."Id",
		T3."UserId",
		T1."SortCode",
		CASE WHEN T1."SortCode" = '||:MinStage||' THEN  ''Waiting'' END,
		CURRENT_TIMESTAMP,
		'||:UserId||'
	FROM "Tm_ApprovalTemplate" T0
	INNER JOIN "Tm_ApprovalTemplate_Stage" T1 ON T0."Id" = T1."Id"
	INNER JOIN "Tm_ApprovalStage" T2 ON T1."StageId" = T2."Id"
	INNER JOIN "Tm_ApprovalStage_User" T3 ON T2."Id" = T3."Id" AND COALESCE(T3."IsTick",''N'') = ''Y''
	WHERE T0."Id" = '|| :TemplateId ||'
	AND NOT EXISTS(
		SELECT 1
		FROM "Tx_'||:ObjectCode||'_Approval" Tx
		WHERE Tx."Id" = '||:BaseId||'
	)'
	;
	EXEC (:SSQL);
END
;

CREATE PROCEDURE "SpApprovalStage__TransNotif" 
(
	  UserId INT, --[User Login]
	  Category NVARCHAR(100) , --[before], [after]
	  ObjCode NVARCHAR(100) ,  --TableName
	  TransType NVARCHAR(100) ,-- [add], [update], [delete] ,[post], [cancel], ;--add: tidak ada before	
	  FieldKeys NVARCHAR(255) ,
	  FieldValues NVARCHAR(255) ,
	  FieldParentValues NVARCHAR(255) 			 
)
AS
BEGIN 
	DECLARE flag int; 
	  	
	DECLARE error  INT	;			-- Result (0 for no error)
	DECLARE error_message nvarchar (200) ;		-- Error string to be displayed
	DECLARE SapDb NVARCHAR(50);
	DECLARE transNo NVARCHAR(10000);
		
	 error = 0;
	 error_message = 'Ok' ;
	 
	SELECT :error AS "error", :error_message AS "error_message" FROM DUMMY;	
	
END;

CREATE PROCEDURE "SpApprovalTemplate__TransNotif" 
(
	  UserId INT, --[User Login]
	  Category NVARCHAR(100) , --[before], [after]
	  ObjCode NVARCHAR(100) ,  --TableName
	  TransType NVARCHAR(100) ,-- [add], [update], [delete] ,[post], [cancel], ;--add: tidak ada before	
	  FieldKeys NVARCHAR(255) ,
	  FieldValues NVARCHAR(255) ,
	  FieldParentValues NVARCHAR(255) 			 
)
AS
BEGIN 
	DECLARE flag int; 
	  	
	DECLARE error  INT	;			-- Result (0 for no error)
	DECLARE error_message nvarchar (200) ;		-- Error string to be displayed
	DECLARE SapDb NVARCHAR(50);
	DECLARE transNo NVARCHAR(10000);
		
	 error = 0;
	 error_message = 'Ok' ;
	IF Category = 'after' THEN
	
		IF TransType IN('add', 'update') THEN
			IF EXISTS(
				SELECT "Id", "SortCode"
				FROM "Tm_ApprovalTemplate_Stage" T0
				WHERE "Id" = :FieldValues
				GROUP BY "Id", "SortCode"  
				HAVING COUNT("SortCode") > 1				
			)THEN
 				error = -1;
				error_message = 'Duplicate step level ' ;	
			END IF;
			
			IF EXISTS(
				SELECT "Id", "StageId"
				FROM "Tm_ApprovalTemplate_Stage" T0
				WHERE "Id" = :FieldValues
				GROUP BY "Id", "StageId"  
				HAVING COUNT("StageId") > 1				
			)THEN
 				error = -1;
				error_message = 'Duplicate stage ' ;	
			END IF;
			
		END IF;

	END IF;	
	SELECT :error AS "error", :error_message AS "error_message" FROM DUMMY;	
	
END;
