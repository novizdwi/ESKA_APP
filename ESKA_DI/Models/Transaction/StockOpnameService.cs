using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;

using Models._Sap;
using SAPbobsCOM;

namespace Models.Transaction
{
    #region Models

    public class StockOpnameModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string UserName { get; set; }

        public string TransType { get; set; }

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? PostingDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public string DocNum_ { get; set; }

        public string Status { get; set; }

        public string CheckNeedApproval_ { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public List<StockOpname_DetailModel> ListDetail_ = new List<StockOpname_DetailModel>();

        public StockOpname_Detail Details_ { get; set; }
    }
        
    public class StockOpname_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockOpname_DetailModel> insertedRowValues { get; set; }
        public List<StockOpname_DetailModel> modifiedRowValues { get; set; }
    }

    public class StockOpname_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockOpname_ApprovalModel> insertedRowValues { get; set; }
        public List<StockOpname_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class StockOpname_RefModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public long? BaseId { get; set; }

        public string BaseNo { get; set; }

        public DateTime? BaseCreatedDate { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public string BaseCreatedUser_ { get; set; }
    }

    public class StockOpname_ApprovalModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? Id { get; set; }

        public int? DetId { get; set; }

        public int? StageId { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; }

        public int? Step { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public DateTime? ActionDate { get; set; }
    }

    public class StockOpname_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int? RowNo { get; set; }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string AcctCode { get; set; }

        public string AcctName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QtyVariance { get; set; }

        public decimal? QtyVariance_ { get; set; }

        public decimal? QuantityOnHandSAP_ { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public decimal? UnitPriceTc { get; set; }

        public decimal? LineTotal { get; set; }

        public string FreeText { get; set; }

    }

    public class StockOpnameAddResultModel
    {
        public string DocEntry { get; set; }
        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    public class StockOpnameApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<StockOpname_ApprovalModel> ApprovalStepList__ = new List<StockOpname_ApprovalModel>();

        public StockOpname_Approval ApprovalStep__ { get; set; }
    }

    #endregion

    #region Services

    public class StockOpnameService
    {

        public StockOpnameModel GetNewModel(int userId)
        {
            StockOpnameModel model = new StockOpnameModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public StockOpnameModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public StockOpnameModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            StockOpnameModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_StockOpname"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<StockOpnameModel>(ssql, id).Single();
                
                model.ListDetail_ = this.StockOpname_Details(CONTEXT, id);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'StockOpname', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
			            FROM ""Tx_StockOpname"" T0
			            INNER JOIN  ""Tx_StockOpname_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
			            WHERE T0.""Id"" = :p0 
			            AND T1.""UserId"" = :p1
		            ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }

        public List<StockOpname_RefModel> StockOpname_Refs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return StockOpname_Refs(CONTEXT, id);
            }

        }

        public List<StockOpname_RefModel> StockOpname_Refs(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""TransDate"", T2.""FirstName"" AS ""BaseCreatedUser_""
                FROM ""Tx_StockOpname_Ref"" T0
                INNER JOIN ""Tx_TransferOut"" T1 ON T0.""BaseId"" = T1.""Id""
                LEFT JOIN ""Tm_User"" T2 ON T0.""BaseCreatedUser"" = T2.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var result = CONTEXT.Database.SqlQuery<StockOpname_RefModel>(ssql, id).ToList();
            return result;
        }

        public List<StockOpname_DetailModel> StockOpname_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return StockOpname_Details(CONTEXT, id);
            }

        }

        public List<StockOpname_DetailModel> StockOpname_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"
            SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY T0.""DetId"") AS ""RowNo"",
                T0.*,
                T2.""OnHand"" AS ""QuantityOnHandSAP_"",
                CASE WHEN T1.""Status"" IN ('Draft') THEN COALESCE(T2.""OnHand"", 0) - COALESCE(T0.""Quantity"", 0) ELSE NULL END AS ""QtyVariance_""
            FROM ""Tx_StockOpname_Item"" T0
            INNER JOIN ""Tx_StockOpname"" T1 ON T0.""Id"" = T1.""Id""
            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITW"" T2 ON T0.""ItemCode"" = T2.""ItemCode"" AND T1.""WhsCode"" = T2.""WhsCode""
            WHERE T0.""Id"" =:p0
            ORDER BY T0.""DetId"" ASC
            ";
            var StockOpname = CONTEXT.Database.SqlQuery<StockOpname_DetailModel>(ssql, id).ToList();
            return StockOpname;
        }

        public List<StockOpname_ApprovalModel> GetStockOpname_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetStockOpname_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<StockOpname_ApprovalModel> GetStockOpname_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_StockOpname_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<StockOpname_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public StockOpnameModel NavFirst(int userId)
        {
            StockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public StockOpnameModel NavPrevious(int userId, long id = 0)
        {
            StockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }


            return model;
        }

        public StockOpnameModel NavNext(int userId, long id = 0)
        {
            StockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }

            return model;
        }

        public StockOpnameModel NavLast(int userId)
        {
            StockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(StockOpnameModel model)
        {
            long Id = 0;

            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            Tx_StockOpname tx_StockOpname = new Tx_StockOpname();
                            CopyProperty.CopyProperties(model, tx_StockOpname, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            
                            tx_StockOpname.TransType = "StockOpname";
                            tx_StockOpname.CreatedDate = dtModified;
                            tx_StockOpname.CreatedUser = model._UserId;
                            tx_StockOpname.ModifiedDate = dtModified;
                            tx_StockOpname.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'StockOpname','" + dateX + "','') ").SingleOrDefault();
                            tx_StockOpname.TransNo = transNo;

                            CONTEXT.Tx_StockOpname.Add(tx_StockOpname);
                            CONTEXT.SaveChanges();
                            Id = tx_StockOpname.Id;

                            String keyValue;
                            keyValue = tx_StockOpname.Id.ToString();
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "StockOpname", CONTEXT, "after", "StockOpname", "add", "Id", keyValue);

                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMassage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMassage = ex.Message;
                            }
                            else
                            {
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }
            }

            return Id;

        }

        public void Update(StockOpnameModel model, string method ="")
        {
            if (model != null)
            {
                if (model != null)
                {
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            try
                            {
                                String keyValue;
                                keyValue = model.Id.ToString();
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "StockOpname", CONTEXT, "before", "StockOpname", "update", "Id", keyValue);


                                Tx_StockOpname tx_StockOpname = CONTEXT.Tx_StockOpname.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                             
                                if (tx_StockOpname != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_StockOpname, false, exceptColumns);

                                    //Tx_StockOpname.ApprovalStatus = isApprovalActive == "Y" && Tx_StockOpname.ApprovalStatus == "" ? "Waiting" : "Approved";
                                    tx_StockOpname.ModifiedDate = dtModified;
                                    tx_StockOpname.ModifiedUser = model._UserId;

                                    if (method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"StockOpname_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
                                    }

                                    if (model.Details_ != null)
                                    {
                                        if (model.Details_.insertedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.insertedRowValues)
                                            {
                                                Detail_Add(CONTEXT, detail, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Details_.modifiedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.modifiedRowValues)
                                            {
                                                Detail_Update(CONTEXT, detail, model._UserId);
                                            }
                                        }

                                        if (model.Details_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Details_.deletedRowKeys)
                                            {
                                                StockOpname_DetailModel detailModel = new StockOpname_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }
                                    
                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "StockOpname", CONTEXT, "after", "StockOpname", "update", "Id", keyValue);
                                    
                                }

                                CONTEXT_TRANS.Commit();
                            }

                            catch (Exception ex)
                            {
                                CONTEXT_TRANS.Rollback();

                                string errorMassage;
                                if (ex.Message.Substring(12) == "[VALIDATION]")
                                {
                                    errorMassage = ex.Message;
                                }
                                else
                                {
                                    errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                                }

                                throw new Exception(errorMassage);
                            }
                        }
                    }
                }

            }


        }

        public bool ChooseItem(int UserId, long Id, string[] data, string sorting)
        {
            if (data != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            String keyValue;
                            keyValue = Id.ToString();
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "before", "StockOpname", "ChooseItem", "Id", keyValue);

                            string sqlWhere;
                            if (data == null)
                            {
                                sqlWhere = "";
                            }
                            else if (data.Length == 0)
                            {
                                sqlWhere = "";
                            }
                            else
                            {
                                for (var i = 0; i < data.Length; i++)
                                {
                                    data[i] = "'" + data[i].Replace("'", "''") + "'";
                                }

                                sqlWhere = string.Join(",", data);
                            }


                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpStockOpname_ChooseItem\"(:p0,:p1,:p2,:p3)", UserId, Id, sqlWhere, sorting);


                            keyValue = Id.ToString();
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "StockOpname", "ChooseItem", "Id", keyValue);


                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMessage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMessage = ex.Message;
                            }
                            else
                            {
                                errorMessage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMessage);
                        }
                    }
                }


            }
            return true;
        }

        public long Detail_Add(HANA_APP CONTEXT, StockOpname_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_StockOpname_Item Tx_StockOpname_Item = new Tx_StockOpname_Item();

                CopyProperty.CopyProperties(model, Tx_StockOpname_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_StockOpname_Item.Id = Id;
                Tx_StockOpname_Item.CreatedDate = dtModified;
                Tx_StockOpname_Item.CreatedUser = UserId;
                Tx_StockOpname_Item.ModifiedDate = dtModified;
                Tx_StockOpname_Item.ModifiedUser = UserId;

                CONTEXT.Tx_StockOpname_Item.Add(Tx_StockOpname_Item);
                CONTEXT.SaveChanges();
                DetId = Tx_StockOpname_Item.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, StockOpname_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_StockOpname_Item Tx_StockOpname_Item = CONTEXT.Tx_StockOpname_Item.Find(model.DetId);

                if (Tx_StockOpname_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_StockOpname_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_StockOpname_Item.ModifiedDate = dtModified;
                    Tx_StockOpname_Item.ModifiedUser = UserId;
                    //CONTEXT.SaveChanges();
                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, StockOpname_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_StockOpname_Item\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.SaveChanges();


                }
            }

        }

        public void Post(int userId, StockOpnameModel StockOpnameModel)
        {
            try
            {
                Update(StockOpnameModel, "Post");
                PostSAP(userId, StockOpnameModel.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        oCompany = SAPCachedCompany.GetCompany();

                        String keyValue;
                        keyValue = id.ToString();

                        StockOpnameModel syncStockOpname = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "before", "Tx_StockOpname", "post", "Id", keyValue);

                        Tx_StockOpname tx_StockOpname = CONTEXT.Tx_StockOpname.Find(id);
                        if (tx_StockOpname != null)
                        {

                            int docEntry_ = AddInventoryPosting(oCompany, userId, id, syncStockOpname);
                            if (docEntry_ <= 0)
                            {
                                throw new Exception($"[VALIDATION] - No inventory posting created");
                            }
                            string ssql = @"SELECT ""DocNum"" 
                                        FROM """ + DbProvider.dbSap_Name + @""".""OIQR"" T0
                                        WHERE T0.""DocEntry"" = " + docEntry_ + @" 
                                        ";

                            string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_StockOpname.PostingDate = dtModified;
                            tx_StockOpname.DocEntry = Convert.ToInt32(docEntry_);
                            tx_StockOpname.DocNum = docNum;
                            tx_StockOpname.PostingDate = dtModified;

                            tx_StockOpname.Status = "Posted";
                            tx_StockOpname.IsAfterPosted = "Y";
                            tx_StockOpname.ModifiedDate = dtModified;
                            tx_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();

                            SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "after", "Tx_StockOpname", "post", "Id", keyValue);

                            if (oCompany.InTransaction)
                            {
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                            }

                            CONTEXT_TRANS.Commit();            
                        }

                    }
                    catch (Exception ex)
                    {
                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }

                        CONTEXT_TRANS.Rollback();

                        string errorMassage;
                        if (ex.Message.Substring(12) == "[VALIDATION]")
                        {
                            errorMassage = ex.Message;
                        }
                        else
                        {
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                    finally
                    {
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }

        private int AddInventoryPosting(Company oCompany, int userId, long id, StockOpnameModel model)
        {
            int newDocEntry = -1;
            int nErr;
            string errMsg;

            SAPbobsCOM.CompanyService oCS = (SAPbobsCOM.CompanyService)oCompany.GetCompanyService();
            SAPbobsCOM.InventoryPostingsService oInventoryPostingsService = oCS.GetBusinessService(SAPbobsCOM.ServiceTypes.InventoryPostingsService);
            SAPbobsCOM.InventoryPosting oDocument = oInventoryPostingsService.GetDataInterface(SAPbobsCOM.InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

            oDocument.PostingDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(model.Comments))
            {
                oDocument.Remarks = model.Comments;
                oDocument.JournalRemark = model.Comments;
            }

            oDocument.UserFields.Item("U_IDU_WebId").Value = model.Id.ToString();
            oDocument.UserFields.Item("U_IDU_WebTransNo").Value = model.TransNo;
            if (model.ListDetail_.Count > 0)
            {
                foreach (var item in model.ListDetail_)
                {
                    if (item.QtyVariance > 0)
                    {
                        InventoryPostingLine line = oDocument.InventoryPostingLines.Add();
                        line.ItemCode = item.ItemCode;
                        line.WarehouseCode = item.WhsCode;
                        line.CountedQuantity = Convert.ToDouble(item.QtyVariance);
                        line.UoMCode = item.Uom ?? "";

                        line.Price = (double)item.UnitPriceTc;

                        line.InventoryOffsetIncreaseAccount = item.AcctCode;
                        line.InventoryOffsetDecreaseAccount = item.AcctCode;

                        //line.CostingCode = item.PillarsCode;
                        //line.CostingCode2 = item.ClassCode;
                        //line.CostingCode3 = item.SubClass1Code;
                        //line.CostingCode4 = item.SubClass2Code;
                        //line.ProjectCode = item.ProjectCode;

                        line.UserFields.Item("U_IDU_WebId").Value = item.Id.ToString();
                        line.UserFields.Item("U_IDU_DetId").Value = item.DetId.ToString();
                    }
                }
            }

            InventoryPostingParams oParams = oInventoryPostingsService.Add(oDocument);
            newDocEntry = oParams.DocumentEntry;

            if (newDocEntry <= 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Inventory Posting : " + nErr.ToString() + "|" + errMsg);
            }

            return newDocEntry;
        }

        public void Cancel(int userId, long Id, string cancelReason)
        {
            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = Id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "before", "Tx_StockOpname", "cancel", "Id", keyValue);

                        Tx_StockOpname tx_StockOpname = CONTEXT.Tx_StockOpname.Find(Id);
                        if (tx_StockOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_StockOpname.Status = "Cancel";
                            tx_StockOpname.ApprovalStatus = "Rejected";
                            tx_StockOpname.CancelReason = cancelReason;
                            tx_StockOpname.ModifiedDate = dtModified;
                            tx_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "after", "Tx_StockOpname", "cancel", "Id", keyValue);

                        
                        CONTEXT_TRANS.Commit();
                    }

                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();

                        string errorMassage;
                        if (ex.Message.Substring(12) == "[VALIDATION]")
                        {
                            errorMassage = ex.Message;
                        }
                        else
                        {
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                }
            }

        }

        public void RequestApproval(int userId, long id, int templateId, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {
                StockOpnameModel StockOpnameModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "before", "Tx_StockOpname", "requestApproval", "Id", keyValue);

                        Tx_StockOpname tx_StockOpname = CONTEXT.Tx_StockOpname.Find(id);
                        if (tx_StockOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_StockOpname.IsApproval = "Y";
                            tx_StockOpname.ApprovalMessages = approvalMessages;
                            tx_StockOpname.ApprovalStatus = "Waiting";
                            tx_StockOpname.ModifiedDate = dtModified;
                            tx_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'StockOpname',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "after", "Tx_StockOpname", "requestApproval", "Id", keyValue);
                        CONTEXT_TRANS.Commit();

                    }

                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();

                        string errorMassage;
                        if (ex.Message.Substring(12) == "[VALIDATION]")
                        {
                            errorMassage = ex.Message;
                        }
                        else
                        {
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                }
            }

        }

        public void Approve(int userId, long id, string approvalMessage)
        {
            string approvalStatus = string.Empty;
            try
            {
                approvalStatus = Authorize(userId, id, "Approve", approvalMessage);

                if (approvalStatus == "Approved")
                {
                    //StockOpnameModel StockOpnameModel = GetById(userId, id);
                    //this.Update(StockOpnameModel, "Post");
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpStockOpname_UpdateItem\"(:p0,:p1, 'before')", userId, id);
                            CONTEXT.SaveChanges();
                        }
                    }

                    this.PostSAP(userId, id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Authorize(int userId, long id, string action, string approvalMessage)
        {
            string approvalStatus = string.Empty;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "before", "Tx_StockOpname", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'StockOpname', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "StockOpname", CONTEXT, "after", "Tx_StockOpname", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_StockOpname"" T0
                            WHERE T0.""Id"" = :p0 
                        ";

                        approvalStatus = CONTEXT.Database.SqlQuery<string>(strApprovalStatus, id).FirstOrDefault();
                        return approvalStatus;
                    }

                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();

                        string errorMassage;
                        if (ex.Message.Substring(12) == "[VALIDATION]")
                        {
                            errorMassage = ex.Message;
                        }
                        else
                        {
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                }
            }

        }

        public StockOpnameApprovalView___ GetViewApproval(long id)
        {
            StockOpnameApprovalView___ model = new StockOpnameApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_StockOpname"" T0 
                    LEFT JOIN ""Tx_StockOpname_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<StockOpnameApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetStockOpname_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

    }


    #endregion

}