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

        public DateTime? DueDate { get; set; }

        public DateTime? PostingDate { get; set; }
        
        public string CardCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string CardName{ get; set; }

        public string ContractNo { get; set; }

        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public string ItemCode { get; set; }
        
        public string ItemName { get; set; }
        
        public decimal? Quantity { get; set; }

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

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string RoutingCode { get; set; }

        public string RoutingName { get; set; }

        public string RoutingStatus { get; set; }

        public string LineStatus { get; set; }

        public int?  OperatorId { get; set; }

        public string OperatorName { get; set; }

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

    #endregion

    #region Services

    public class ProcessCardService
    {

        public ProcessCardModel GetNewModel(int userId)
        {
            ProcessCardModel model = new ProcessCardModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            model.DueDate = DateTime.Now.AddMonths(1);
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
                SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY T0.""DetId"") AS ""RowNo"",
                    T0.*
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
                            
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProcessCard_AddDetail\"(:p0,:p1)", Id, model._UserId);
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
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                             
                                if (tx_ProcessCard != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_ProcessCard, false, exceptColumns);

                                    //Tx_ProcessCard.ApprovalStatus = isApprovalActive == "Y" && Tx_ProcessCard.ApprovalStatus == "" ? "Waiting" : "Approved";
                                    tx_ProcessCard.ModifiedDate = dtModified;
                                    tx_ProcessCard.ModifiedUser = model._UserId;

                                    if (method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"ProcessCard_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
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
                                                ProcessCard_DetailModel detailModel = new ProcessCard_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }
                                    
                                    CONTEXT.SaveChanges();

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
                Update(ProcessCardModel, "Post");
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

                        String keyValue;
                        keyValue = id.ToString();

                        ProcessCardModel syncProcessCard = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "before", "Tx_ProcessCard", "post", "Id", keyValue);

                        Tx_ProcessCard tx_ProcessCard = CONTEXT.Tx_ProcessCard.Find(id);
                        if (tx_ProcessCard != null)
                        {

                            int docEntry_ = AddInventoryPosting(oCompany, userId, id, syncProcessCard);
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

                            tx_ProcessCard.PostingDate = dtModified;
                            tx_ProcessCard.DocEntry = Convert.ToInt32(docEntry_);
                            tx_ProcessCard.DocNum = docNum;
                            tx_ProcessCard.PostingDate = dtModified;

                            tx_ProcessCard.Status = "Posted";
                            tx_ProcessCard.IsAfterPosted = "Y";
                            tx_ProcessCard.ModifiedDate = dtModified;
                            tx_ProcessCard.ModifiedUser = userId;

                            CONTEXT.SaveChanges();

                            SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "after", "Tx_ProcessCard", "post", "Id", keyValue);

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

        private int AddInventoryPosting(Company oCompany, int userId, long id, ProcessCardModel model)
        {
            int newDocEntry = -1;
            int nErr;
            string errMsg;

            //SAPbobsCOM.CompanyService oCS = (SAPbobsCOM.CompanyService)oCompany.GetCompanyService();
            //SAPbobsCOM.InventoryPostingsService oInventoryPostingsService = oCS.GetBusinessService(SAPbobsCOM.ServiceTypes.InventoryPostingsService);
            //SAPbobsCOM.InventoryPosting oDocument = oInventoryPostingsService.GetDataInterface(SAPbobsCOM.InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

            //oDocument.PostingDate = DateTime.Now;

            //if (!string.IsNullOrWhiteSpace(model.Comments))
            //{
            //    oDocument.Remarks = model.Comments;
            //    oDocument.JournalRemark = model.Comments;
            //}

            //oDocument.UserFields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            //oDocument.UserFields.Item("U_IDU_WebTransNo").Value = model.TransNo;
            //if (model.ListDetail_.Count > 0)
            //{
            //    foreach (var item in model.ListDetail_)
            //    {
            //        if (item.QtyVariance > 0)
            //        {
            //            InventoryPostingLine line = oDocument.InventoryPostingLines.Add();
            //            line.ItemCode = item.ItemCode;
            //            line.WarehouseCode = item.WhsCode;
            //            line.CountedQuantity = Convert.ToDouble(item.QtyVariance);
            //            line.UoMCode = item.Uom ?? "";

            //            line.Price = (double)item.UnitPriceTc;

            //            line.InventoryOffsetIncreaseAccount = item.AcctCode;
            //            line.InventoryOffsetDecreaseAccount = item.AcctCode;

            //            //line.CostingCode = item.PillarsCode;
            //            //line.CostingCode2 = item.ClassCode;
            //            //line.CostingCode3 = item.SubClass1Code;
            //            //line.CostingCode4 = item.SubClass2Code;
            //            //line.ProjectCode = item.ProjectCode;

            //            line.UserFields.Item("U_IDU_WebId").Value = Convert.ToInt32(item.Id);
            //            line.UserFields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);
            //        }
            //    }
            //}

            //InventoryPostingParams oParams = oInventoryPostingsService.Add(oDocument);
            //newDocEntry = oParams.DocumentEntry;

            //if (newDocEntry <= 0)
            //{
            //    nErr = oCompany.GetLastErrorCode();
            //    errMsg = oCompany.GetLastErrorDescription();

            //    SapCompany.CleanUp(oDocument);

            //    throw new Exception("[VALIDATION] - Inventory Posting : " + nErr.ToString() + "|" + errMsg);
            //}

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

                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "before", "Tx_ProcessCard", "cancel", "Id", keyValue);

                        Tx_ProcessCard tx_ProcessCard = CONTEXT.Tx_ProcessCard.Find(Id);
                        if (tx_ProcessCard != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_ProcessCard.Status = "Cancel";
                            tx_ProcessCard.ApprovalStatus = "Rejected";
                            tx_ProcessCard.CancelReason = cancelReason;
                            tx_ProcessCard.ModifiedDate = dtModified;
                            tx_ProcessCard.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "ProcessCard", CONTEXT, "after", "Tx_ProcessCard", "cancel", "Id", keyValue);

                        
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
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProcessCard_UpdateItem\"(:p0,:p1, 'before')", userId, id);
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