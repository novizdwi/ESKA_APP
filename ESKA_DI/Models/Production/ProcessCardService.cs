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

namespace Models.Production
{
    #region Models

    public class ProcessCardModel
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

        [Required(ErrorMessage = "required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? DueDate { get; set; }

        public DateTime? PostingDate { get; set; }

        public string CardCode { get; set; }

        public string CardName { get; set; }

        public string ContractNo { get; set; }

        [Required(ErrorMessage = "required")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public decimal? Quantity { get; set; } 

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

        public string SerialNumber_ { get; set; }

        public List<ProcessCard_DetailModel> ListDetail_ = new List<ProcessCard_DetailModel>();

        public List<ProcessCard_DetailModel> ListDetails_ = new List<ProcessCard_DetailModel>();

        public ProcessCard_Detail Details_ { get; set; }

    }

    public class ProcessCard_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<ProcessCard_DetailModel> insertedRowValues { get; set; }
        public List<ProcessCard_DetailModel> modifiedRowValues { get; set; }
    }

    public class ProcessCard_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<ProcessCard_ApprovalModel> insertedRowValues { get; set; }
        public List<ProcessCard_ApprovalModel> modifiedRowValues { get; set; }
    }
    
    public class ProcessCard_ApprovalModel
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

    public class ProcessCard_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }


        public int? RowNo { get; set; }
        
        public int? Sort { get; set; }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string RoutingCode { get; set; }

        public string DocNum { get; set; }
        
        public string RoutingName { get; set; }

        public string RoutingStatus { get; set; }

        public string LineStatus { get; set; }

        public int?  OperatorId { get; set; }

        public string  OperatorName { get; set; }

        public DateTime? ProcessingDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MachineId { get; set; }

        public string MachineName { get; set; }

        public int? PracticeHours { get; set; }

        public int? ActualHours { get; set; }

        public string Comments { get; set; }

    }

    public class ProcessCardApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<ProcessCard_ApprovalModel> ApprovalStepList__ = new List<ProcessCard_ApprovalModel>();

        public ProcessCard_Approval ApprovalStep__ { get; set; }
    }

    public class ProductionOrder_ReturnModel
    {
        public int? Id { get; set; }
        public long? DetId { get; set; }
        public int? DocEntry { get; set; }
        public string DocNum { get; set; }
    }


    public class BomPropagationModel
    {
        public string FG { get; set; }

        public string Parent { get; set; }

        public string Component { get; set; }

        public int Level { get; set; }

        public int RoutingLevel { get; set; }

        public string RoutingName { get; set; }

        public string RoutingStage { get; set; }

        public string TreeType { get; set; }

        public string ComponentDesc { get; set; }

        public string IssueMethod { get; set; }

        public string UoM { get; set; }

        public decimal QtyPer { get; set; }

        public decimal QtyOrder { get; set; }

        public decimal TotalQty { get; set; }
    }

    public class ProductionOrderModel
    {

        public long? Id { get; set; }   // FK ke Tx_ProcessCard

        public long? DetId { get; set; }   // FK ke Tx_ProcessCard_Detail

        public string TransNo { get; set; }   // Nomor transaksi web

        public int? OperatorId { get; set; }
        
        public string OperatorName { get; set; }

        public string FG { get; set; }   // Root Finished Good

        public string Parent { get; set; }   // Item yang diproduksi (ItemNo OWOR)

        public string TreeType { get; set; }   // OITT.TreeType (P/S/A)

        public string RoutingStage { get; set; }   // OITT.U_IDU_RoutingStage

        public int RoutingLevel { get; set; }   // Urutan bottom-up → UDF U_IDU_Level

        public string RoutingName { get; set; }   // Nama routing   → UDF U_IDU_RoutingStage

        public decimal QtyOrder { get; set; }   // Qty FG yang dipesan (konstan)

        public decimal PlannedQty { get; set; }   // Qty yang diproduksi pada PO ini

        public string DocEntry { get; set; }   // DocEntry OWOR setelah Add()

        public string SapStatus { get; set; }   // null / "Posted" / "Error"

        public List<ProductionOrder_DetailModel> ListDetails_ { get; set; } = new List<ProductionOrder_DetailModel>();
    }
    
    public class ProductionOrder_DetailModel
    {

        public long? DetId { get; set; }   // FK ke Tx_ProcessCard_Detail

        public string FG { get; set; }   // Root Finished Good

        public string Parent { get; set; }   // Item induk (= Master.Parent)

        public string Component { get; set; }   // ITT1.Code  → Lines.ItemNo

        public string ComponentDesc { get; set; }   // OITM.ItemName

        public string IssueMethod { get; set; }   // ITT1.IssueMthd (M=Manual, B=Backflush)

        public string UoM { get; set; }   // ITT1.Uom

        public int Level { get; set; }   // Kedalaman BOM (1 = langsung di bawah FG)

        public int RoutingLevel { get; set; }   // Routing level komponen ini

        public decimal QtyPer { get; set; }   // Qty per 1 unit parent (ITT1.Quantity)

        public decimal QtyOrder { get; set; }   // Qty FG yang dipesan (konstan)

        public decimal TotalQty { get; set; }   // Qty total → Lines.PlannedQuantity

    }


    #endregion

    #region Services

    public class ProcessCardService
    {

        public ProcessCardModel GetNewModel(int userId)
        {
            ProcessCardModel model = new ProcessCardModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            model.StartDate = DateTime.Now;
            model.DueDate = DateTime.Now.AddMonths(1);
            model.Quantity = 1;
            return model;
        }

        public ProcessCardModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public ProcessCardModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            ProcessCardModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_ProcessCard"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<ProcessCardModel>(ssql, id).Single();
                model.ListDetails_ = this.ProcessCard_Details(CONTEXT, id);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'ProcessCard', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
			            FROM ""Tx_ProcessCard"" T0
			            INNER JOIN  ""Tx_ProcessCard_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
			            WHERE T0.""Id"" = :p0 
			            AND T1.""UserId"" = :p1
		            ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }
        

        public List<ProcessCard_DetailModel> ProcessCard_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProcessCard_Details(CONTEXT, id);
            }

        }

        public List<ProcessCard_DetailModel> ProcessCard_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"
                SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY T0.""DetId"") AS ""RowNo"", T0.* 
                FROM ""Tx_ProcessCard_Detail"" T0  
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            
            var ProcessCard = CONTEXT.Database.SqlQuery<ProcessCard_DetailModel>(ssql, id).ToList();
            return ProcessCard;
        }

        public List<ProcessCard_ApprovalModel> GetProcessCard_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetProcessCard_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<ProcessCard_ApprovalModel> GetProcessCard_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_ProcessCard_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<ProcessCard_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public ProcessCardModel NavFirst(int userId)
        {
            ProcessCardModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ProcessCard");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ProcessCard\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public ProcessCardModel NavPrevious(int userId, long id = 0)
        {
            ProcessCardModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ProcessCard");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ProcessCard\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public ProcessCardModel NavNext(int userId, long id = 0)
        {
            ProcessCardModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ProcessCard");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ProcessCard\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public ProcessCardModel NavLast(int userId)
        {
            ProcessCardModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ProcessCard");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ProcessCard\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(ProcessCardModel model)
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
                            Tx_ProcessCard tx_ProcessCard = new Tx_ProcessCard();
                            CopyProperty.CopyProperties(model, tx_ProcessCard, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            
                            tx_ProcessCard.TransType = "ProcessCard";
                            tx_ProcessCard.CreatedDate = dtModified;
                            tx_ProcessCard.CreatedUser = model._UserId;
                            tx_ProcessCard.ModifiedDate = dtModified;
                            tx_ProcessCard.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'ProcessCard','" + dateX + "','') ").SingleOrDefault();
                            tx_ProcessCard.TransNo = transNo;

                            CONTEXT.Tx_ProcessCard.Add(tx_ProcessCard);
                            CONTEXT.SaveChanges();
                            Id = tx_ProcessCard.Id;

                            String keyValue;
                            keyValue = tx_ProcessCard.Id.ToString();
                            
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProcessCard_AddDetail\"(:p0,:p1, 'add')", model._UserId, Id );
                            SpNotif.SpSysControllerTransNotif(model._UserId, "ProcessCard", CONTEXT, "after", "ProcessCard", "add", "Id", keyValue);
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

        public void Update(ProcessCardModel model, string method ="")
        {
            int changeItem = 0;
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "ProcessCard", CONTEXT, "before", "ProcessCard", "update", "Id", keyValue);

                                Tx_ProcessCard tx_ProcessCard = CONTEXT.Tx_ProcessCard.Find(model.Id);
                                changeItem = model.ItemCode != tx_ProcessCard.ItemCode ? 1 : 0;
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                             
                                if (tx_ProcessCard != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_ProcessCard, false, exceptColumns);

                                    //Tx_ProcessCard.ApprovalStatus = isApprovalActive == "Y" && Tx_ProcessCard.ApprovalStatus == "" ? "Waiting" : "Approved";
                                    tx_ProcessCard.ModifiedDate = dtModified;
                                    tx_ProcessCard.ModifiedUser = model._UserId;

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
                                                ProcessCard_DetailModel detailModel = new ProcessCard_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }
                                    
                                    CONTEXT.SaveChanges();
                                    //if change item then repopulate detail
                                    if (changeItem == 1)
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProcessCard_AddDetail\"(:p0,:p1, 'update')", model._UserId, model.Id );
                                    }
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "ProcessCard", CONTEXT, "after", "ProcessCard", "update", "Id", keyValue);
                                    
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
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "before", "ProcessCard", "ChooseItem", "Id", keyValue);

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


                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProcessCard_ChooseItem\"(:p0,:p1,:p2,:p3)", UserId, Id, sqlWhere, sorting);


                            keyValue = Id.ToString();
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "ProcessCard", "ChooseItem", "Id", keyValue);


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

        public long Detail_Add(HANA_APP CONTEXT, ProcessCard_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_ProcessCard_Detail Tx_ProcessCard_Detail = new Tx_ProcessCard_Detail();

                CopyProperty.CopyProperties(model, Tx_ProcessCard_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_ProcessCard_Detail.Id = Id;
                Tx_ProcessCard_Detail.CreatedDate = dtModified;
                Tx_ProcessCard_Detail.CreatedUser = UserId;
                Tx_ProcessCard_Detail.ModifiedDate = dtModified;
                Tx_ProcessCard_Detail.ModifiedUser = UserId;

                CONTEXT.Tx_ProcessCard_Detail.Add(Tx_ProcessCard_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_ProcessCard_Detail.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, ProcessCard_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_ProcessCard_Detail Tx_ProcessCard_Detail = CONTEXT.Tx_ProcessCard_Detail.Find(model.DetId);

                if (Tx_ProcessCard_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_ProcessCard_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_ProcessCard_Detail.ModifiedDate = dtModified;
                    Tx_ProcessCard_Detail.ModifiedUser = UserId;
                    //CONTEXT.SaveChanges();
                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, ProcessCard_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ProcessCard_Detail\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.SaveChanges();


                }
            }

        }

        public void Post(int userId, ProcessCardModel ProcessCardModel)
        {
            try
            {
                using (var CONTEXT = new HANA_APP())
                {
                    var statusCheck = CONTEXT.Database.SqlQuery<StatusCheckModel>(@"
                        SELECT ""Status"", ""ApprovalStatus"", ""IsApproval""
                        FROM ""Tx_ProcessCard""
                        WHERE ""Id"" = :p0
                    ", ProcessCardModel.Id).FirstOrDefault();

                    if (statusCheck == null)
                        throw new Exception("[VALIDATION] Transaction not found.");

                    if (statusCheck.ApprovalStatus == "Rejected")
                        throw new Exception("[VALIDATION] Cannot post. Transaction has been Rejected.");

                    if (statusCheck.ApprovalStatus == "Waiting")
                        throw new Exception("[VALIDATION] Cannot post. Transaction is still waiting for Approval.");
                }

                //Update(ProcessCardModel, "Post");
                PostSAP(userId, ProcessCardModel.Id);

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
                        oCompany.StartTransaction();

                        string keyValue = id.ToString();
                        ProcessCardModel syncProcessCard = GetById(userId, id, "Post");
                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "before", "Tx_ProcessCard", "post", "Id", keyValue);

                        Tx_ProcessCard tx_ProcessCard = CONTEXT.Tx_ProcessCard.Find(id);
                        if (tx_ProcessCard != null)
                        {
                            List<ProductionOrder_ReturnModel> poResults = AddProductionOrder(oCompany, userId, id, syncProcessCard);

                            if (poResults.Count <= 0)
                                throw new Exception("[VALIDATION] - No Production Order Created");

                            UpdateProductionOrderDetail(CONTEXT, userId, id, poResults);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_ProcessCard.PostingDate = dtModified;
                            tx_ProcessCard.Status = "Posted";
                            tx_ProcessCard.IsAfterPosted = "Y";
                            tx_ProcessCard.ModifiedDate = dtModified;
                            tx_ProcessCard.ModifiedUser = userId;

                            CONTEXT.SaveChanges();

                            SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "after", "Tx_ProcessCard", "post", "Id", keyValue);

                            if (oCompany.InTransaction)
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                            CONTEXT_TRANS.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (oCompany != null && oCompany.InTransaction)
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                        CONTEXT_TRANS.Rollback();

                        string errorMessage = ex.Message.Length >= 12 && ex.Message.Substring(0, 12) == "[VALIDATION]"
                            ? ex.Message
                            : string.Format("[VALIDATION] {0}", ex.Message);

                        throw new Exception(errorMessage);
                    }
                    finally
                    {
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }
        }

        private List<ProductionOrder_ReturnModel> AddProductionOrder(Company oCompany, int userId, long id, ProcessCardModel model)
        {
            int nErr;
            string errMsg;
            List<ProductionOrder_ReturnModel> ret = new List<ProductionOrder_ReturnModel>();

            // ----------------------------------------------------------
            // 1. Tentukan StartRoutingLevel dari ListDetails_
            //    Rule: Sort pertama yang Active setelah ada Inactive
            // ----------------------------------------------------------
            int startRoutingLevel = 0;

            bool foundInactive = false;
            foreach (var det in model.ListDetails_.OrderBy(x => x.Sort))
            {
                if (det.RoutingStatus == "Inactive")
                {
                    foundInactive = true;
                }
                else if (det.RoutingStatus == "Active" && foundInactive)
                {
                    startRoutingLevel = det.Sort ?? 0;
                    break;
                }
            }

            // ----------------------------------------------------------
            // 2. Panggil Stored Procedure → ambil seluruh BOM rows
            // ----------------------------------------------------------
            List<BomPropagationModel> bomRows;

            using (var CONTEXT = new HANA_APP())
            {
                string sql = string.Format(
                    "CALL \"{0}\".\"__IDU_ProductionOrder\"(:p0, :p1, :p2)",
                    DbProvider.dbSap_Name);

                bomRows = CONTEXT.Database
                    .SqlQuery<BomPropagationModel>(sql, model.ItemCode, model.Quantity, startRoutingLevel)
                    .ToList();
            }

            if (bomRows == null || bomRows.Count == 0)
                throw new Exception("[VALIDATION] - Add Production Order : BOM tidak ditemukan untuk item " + model.ItemCode);

            // ----------------------------------------------------------
            // 3. Mapping BOM rows → Master Detail
            //    Group by (RoutingLevel, Parent) → 1 group = 1 Production Order
            //    DetId dicari dari ListDetails_ berdasarkan Sort == RoutingLevel
            // ----------------------------------------------------------
            DateTime startDate = model.StartDate ?? DateTime.Now;
            DateTime dueDate = model.DueDate ?? DateTime.Now;

            List<ProductionOrderModel> poMasters = bomRows
                .GroupBy(r => new { r.RoutingLevel, r.Parent })
                .OrderBy(g => g.Key.RoutingLevel)
                .Select(g => new ProductionOrderModel
                {
                    Id = (int?)id,
                    TransNo = model.TransNo,
                    FG = g.First().FG,
                    Parent = g.Key.Parent,
                    OperatorId = model.ListDetails_
                                       .Where(d => d.Sort == g.Key.RoutingLevel)
                                       .Select(d => d.OperatorId)
                                       .FirstOrDefault(),
                    OperatorName = model.ListDetails_
                                       .Where(d => d.Sort == g.Key.RoutingLevel)
                                       .Select(d => d.OperatorName)
                                       .FirstOrDefault(),
                    RoutingLevel = g.Key.RoutingLevel,
                    RoutingName = g.First().RoutingName,
                    RoutingStage = g.First().RoutingStage,
                    TreeType = g.First().TreeType,
                    QtyOrder = g.First().QtyOrder,
                    PlannedQty = g.First().TotalQty,
                    DetId = model.ListDetails_
                                       .Where(d => d.Sort == g.Key.RoutingLevel)
                                       .Select(d => d.DetId)
                                       .FirstOrDefault(),
                    ListDetails_ = g.Select(r => new ProductionOrder_DetailModel
                    {
                        FG = r.FG,
                        Parent = r.Parent,
                        Component = r.Component,
                        ComponentDesc = r.ComponentDesc,
                        IssueMethod = r.IssueMethod,
                        UoM = r.UoM,
                        Level = r.Level,
                        RoutingLevel = r.RoutingLevel,
                        QtyPer = r.QtyPer,
                        QtyOrder = r.QtyOrder,
                        TotalQty = r.TotalQty
                    }).ToList()
                }).ToList();

            // ----------------------------------------------------------
            // 4. Loop setiap Production Order → Post ke SAP (bottom-up)
            //    Jika satu level error → throw → SAP rollback semua
            // ----------------------------------------------------------
            foreach (var po in poMasters)
            {
                SAPbobsCOM.ProductionOrders oPO = (SAPbobsCOM.ProductionOrders)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                try
                {
                    // ---- Header (OWOR) ----
                    oPO.ItemNo = po.Parent;
                    oPO.PlannedQuantity = (double)po.PlannedQty;
                    oPO.DueDate = dueDate;
                    oPO.StartDate = startDate;
                    oPO.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotStandard; 
                    oPO.Remarks = string.Format( "Routing: {0} | Level: {1} | FG: {2} | Ref: {3}", po.RoutingName, po.RoutingLevel, po.FG, po.TransNo);

                    oPO.UserFields.Fields.Item("U_IDU_WebId").Value = id.ToString();
                    oPO.UserFields.Fields.Item("U_IDU_WebTransNo").Value = po.TransNo;
                    oPO.UserFields.Fields.Item("U_IDU_RoutingStage").Value = po.RoutingName;
                    oPO.UserFields.Fields.Item("U_IDU_RoutingLevel").Value = Convert.ToInt32(po.RoutingLevel);
                    oPO.UserFields.Fields.Item("U_IDU_HNcode").Value = model.SerialNumber;
                    oPO.UserFields.Fields.Item("U_IDU_OperatorId").Value = Convert.ToInt32(po.RoutingLevel);
                    oPO.UserFields.Fields.Item("U_IDU_OperatorName").Value = po.OperatorName;

                    // ---- Lines (WOR1) ----
                    bool firstLine = true;
                    foreach (var det in po.ListDetails_)
                    {
                        if (det.TotalQty <= 0) continue;

                        if (!firstLine) oPO.Lines.Add();

                        oPO.Lines.ItemNo = det.Component;
                        oPO.Lines.ProductionOrderIssueType = BoIssueMethod.im_Backflush;
                        oPO.Lines.PlannedQuantity = (double)det.TotalQty;

                        firstLine = false;
                    }

                    // ---- Add ke SAP ----
                    int docAdd = oPO.Add();
                    if (docAdd != 0)
                    {
                        nErr = oCompany.GetLastErrorCode();
                        errMsg = oCompany.GetLastErrorDescription();

                        SapCompany.CleanUp(oPO);

                        throw new Exception(string.Format( "[VALIDATION] - Add Production Order [{0}] RoutingLevel [{1}] : {2}|{3}", po.Parent, po.RoutingLevel, nErr, errMsg));
                    }

                    // ---- Ambil DocEntry & DocNum ----
                    string newDocEntry = oCompany.GetNewObjectKey();
                    string newDocNum = string.Empty;
                    SAPbobsCOM.Recordset rsDocNum = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    rsDocNum.DoQuery($@"SELECT ""DocNum"" FROM ""{DbProvider.dbSap_Name}"".""OWOR"" WHERE ""DocEntry"" = {newDocEntry}");
                    if (!rsDocNum.EoF)
                    {
                        newDocNum = rsDocNum.Fields.Item("DocNum").Value?.ToString();
                    }

                    SapCompany.CleanUp(oPO);

                    ret.Add(new ProductionOrder_ReturnModel
                    {
                        Id = (int?)id,
                        DetId = po.DetId,
                        DocEntry = Convert.ToInt32(newDocEntry),
                        DocNum = newDocNum
                    });

                }
                catch
                {
                    SapCompany.CleanUp(oPO);
                    throw;
                }
            }

            return ret;
        }

        private void UpdateProductionOrderDetail( HANA_APP CONTEXT, int userId, long id, List<ProductionOrder_ReturnModel> poResults)
        {
            if (poResults == null || poResults.Count == 0) return;

            DateTime dtModified = CONTEXT.Database
                .SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY")
                .FirstOrDefault();

            foreach (var po in poResults)
            {
                if (po.DetId == null) continue;
                Tx_ProcessCard_Detail txDetail = CONTEXT.Tx_ProcessCard_Detail.Find(po.DetId);
                if (txDetail == null) continue;

                txDetail.DocEntry = po.DocEntry; 
                txDetail.DocNum = po.DocNum;
                txDetail.RoutingStatus = "Ready";
                txDetail.ModifiedDate = dtModified;
                txDetail.ModifiedUser = userId;
            }

            CONTEXT.SaveChanges();
        }

        public void RequestApproval(int userId, long id, int templateId, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {
                ProcessCardModel ProcessCardModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "before", "Tx_ProcessCard", "requestApproval", "Id", keyValue);

                        Tx_ProcessCard tx_ProcessCard = CONTEXT.Tx_ProcessCard.Find(id);
                        if (tx_ProcessCard != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_ProcessCard.IsApproval = "Y";
                            tx_ProcessCard.ApprovalMessages = approvalMessages;
                            tx_ProcessCard.ApprovalStatus = "Waiting";
                            tx_ProcessCard.ModifiedDate = dtModified;
                            tx_ProcessCard.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'ProcessCard',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "after", "Tx_ProcessCard", "requestApproval", "Id", keyValue);
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
                    //ProcessCardModel ProcessCardModel = GetById(userId, id);
                    //this.Update(ProcessCardModel, "Post");

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

                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "before", "Tx_ProcessCard", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'ProcessCard', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "after", "Tx_ProcessCard", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_ProcessCard"" T0
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

        public ProcessCardApprovalView___ GetViewApproval(long id)
        {
            ProcessCardApprovalView___ model = new ProcessCardApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_ProcessCard"" T0 
                    LEFT JOIN ""Tx_ProcessCard_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<ProcessCardApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetProcessCard_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

    }


    #endregion

}