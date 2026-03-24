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

namespace Models.Transaction.Inventory
{
    #region Models

    public class TransferRequestModel
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

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public string RefNo { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public string FileTemplateName_ { get; set; }

        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public List<TransferRequest_DetailModel> ListDetails_ = new List<TransferRequest_DetailModel>();

        public TransferRequest_Detail Details_ { get; set; }
    }

    public class TransferRequest_ApprovalModel
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


    public class TransferRequest_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferRequest_ApprovalModel> insertedRowValues { get; set; }
        public List<TransferRequest_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class TransferRequest_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferRequest_DetailModel> insertedRowValues { get; set; }
        public List<TransferRequest_DetailModel> modifiedRowValues { get; set; }
    }
    public class TransferRequest_DetailModel
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

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOnHand_ { get; set; }

        public decimal? QuantityOpen { get; set; }

        public decimal? QuantityScan { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }
               
        public string Comments { get; set; }
    }

    public class TransferRequestItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        //public List<TransferRequestItemTagModel> TransferRequestItemTagModel___ { get; set; }
    }

    //public class TransferRequestItemTagModel
    //{
    //    public int RowNo { get; set; }

    //    public long Id { get; set; }

    //    public long DetId { get; set; }

    //    public long DetDetId { get; set; }

    //    public string ItemCode { get; set; }

    //    public string ItemName { get; set; }

    //    public string TagId { get; set; }

    //    public decimal? Quantity { get; set; }

    //    public string EventType { get; set; }

    //    public string Status { get; set; }
    //}


    public class TransferRequestTemplateHeader
    {
        public DateTime TransDate { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public string Comments { get; set; }

        public List<TransferRequestTemplateDetail> Detail_ { get; set; }
    }


    public class TransferRequestTemplateDetail
    {
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public decimal Quantity { get; set; }

        public decimal? QuantityOpen { get; set; }

        public string Comments { get; set; }
    }

    public class TransferRequestApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<TransferRequest_ApprovalModel> ApprovalStepList__ = new List<TransferRequest_ApprovalModel>();

        public TransferRequest_Approval ApprovalStep__ { get; set; }
    }

    #endregion

    #region Services

    public class TransferRequestService
    {

        public TransferRequestModel GetNewModel(int userId)
        {
            TransferRequestModel model = new TransferRequestModel();
            model.Status = "Draft";
            return model;
        }
        public TransferRequestModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public TransferRequestModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            TransferRequestModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_TransferRequest"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<TransferRequestModel>(ssql, id).Single();

                model.ListDetails_ = this.TransferRequest_Details(CONTEXT, id);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'TransferRequest', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
			            FROM ""Tx_TransferRequest"" T0
			            INNER JOIN  ""Tx_TransferRequest_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
			            WHERE T0.""Id"" = :p0 
			            AND T1.""UserId"" = :p1
		            ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }
        public List<TransferRequest_DetailModel> TransferRequest_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferRequest_Details(CONTEXT, id);
            }

        }

        public List<TransferRequest_DetailModel> TransferRequest_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.* ,
                COALESCE(T1.""OnHand"",0) AS ""QuantityOnHand_""
                FROM ""Tx_TransferRequest_Item"" T0
                LEFT JOIN  """ + DbProvider.dbSap_Name + @""".""OITW"" T1 ON T0.""ItemCode"" = T1.""ItemCode"" AND T0.""FromWhsCode"" = T1.""WhsCode""
                WHERE ""Id"" =:p0
                ORDER BY ""DetId"" ASC
            ";
            var purchaseOrder = CONTEXT.Database.SqlQuery<TransferRequest_DetailModel>(ssql, id).ToList();
            return purchaseOrder;
        }

        public List<TransferRequest_ApprovalModel> GetTransferRequest_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetTransferRequest_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<TransferRequest_ApprovalModel> GetTransferRequest_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_TransferRequest_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<TransferRequest_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public TransferRequestModel NavFirst(int userId)
        {
            TransferRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferRequest\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public TransferRequestModel NavPrevious(int userId, long id = 0)
        {
            TransferRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferRequest\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TransferRequestModel NavNext(int userId, long id = 0)
        {
            TransferRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferRequest\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TransferRequestModel NavLast(int userId)
        {
            TransferRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferRequest\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(TransferRequestModel model)
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

                            Tx_TransferRequest tx_TransferRequest = new Tx_TransferRequest();
                            CopyProperty.CopyProperties(model, tx_TransferRequest, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                                        
                            tx_TransferRequest.TransType = "TransferRequest";
                            tx_TransferRequest.CreatedDate = dtModified;
                            tx_TransferRequest.CreatedUser = model._UserId;
                            tx_TransferRequest.ModifiedDate = dtModified;
                            tx_TransferRequest.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TransferRequest','" + dateX + "','') ").SingleOrDefault();
                            tx_TransferRequest.TransNo = transNo;

                            CONTEXT.Tx_TransferRequest.Add(tx_TransferRequest);
                            CONTEXT.SaveChanges();
                            Id = tx_TransferRequest.Id;

                            String keyValue;
                            keyValue = tx_TransferRequest.Id.ToString();

                            if (model.Details_ != null)
                            {
                                if (model.Details_.insertedRowValues != null)
                                {
                                    foreach (var detail in model.Details_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, detail, Convert.ToInt64(keyValue), model._UserId);
                                    }
                                }
                            }

                            //    CONTEXT.Database.ExecuteSqlCommand(
                            //    "CALL \"SpGoodsReceiptPO_AddItemDetail\" ({0}, {1}, {2})",
                            //    model._UserId,
                            //    keyValue
                            //);
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "TransferRequest", CONTEXT, "after", "TransferRequest", "add", "Id", keyValue);

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

        public void Update(TransferRequestModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "TransferRequest", CONTEXT, "before", "TransferRequest", "update", "Id", keyValue);

                                Tx_TransferRequest tx_TransferRequest = CONTEXT.Tx_TransferRequest.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            
                                if (tx_TransferRequest != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_TransferRequest, false, exceptColumns);

                                    tx_TransferRequest.ModifiedDate = dtModified;
                                    tx_TransferRequest.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

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
                                                TransferRequest_DetailModel detailModel = new TransferRequest_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TransferRequest", CONTEXT, "after", "TransferRequest", "update", "Id", keyValue);
                                    
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

        public long Detail_Add(HANA_APP CONTEXT, TransferRequest_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_TransferRequest_Item Tx_TransferRequest_Item = new Tx_TransferRequest_Item();

                CopyProperty.CopyProperties(model, Tx_TransferRequest_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_TransferRequest_Item.Id = Id;
                Tx_TransferRequest_Item.CreatedDate = dtModified;
                Tx_TransferRequest_Item.CreatedUser = UserId;
                Tx_TransferRequest_Item.ModifiedDate = dtModified;
                Tx_TransferRequest_Item.ModifiedUser = UserId;               


                CONTEXT.Tx_TransferRequest_Item.Add(Tx_TransferRequest_Item);
                CONTEXT.SaveChanges();
                DetId = Tx_TransferRequest_Item.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, TransferRequest_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_TransferRequest_Item Tx_TransferRequest_Item = CONTEXT.Tx_TransferRequest_Item.Find(model.DetId);

                if (Tx_TransferRequest_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_TransferRequest_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    
                    Tx_TransferRequest_Item.Quantity = model.Quantity;
                    Tx_TransferRequest_Item.ModifiedDate = dtModified;
                    Tx_TransferRequest_Item.ModifiedUser = UserId;
                   
                    CONTEXT.SaveChanges();

                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, TransferRequest_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_TransferRequest_Item\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public bool ChooseItem(int UserId, long Id, string[] data)
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
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "before", "TransferRequest", "ChooseItem", "Id", keyValue);

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


                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferRequest_ChooseItem\"(:p0,:p1,:p2)", UserId, Id, sqlWhere);


                            keyValue = Id.ToString();
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "TransferRequest", "ChooseItem", "Id", keyValue);


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

        public void Post(int userId, long id)
        {

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        Tx_TransferRequest tx_TransferRequest = CONTEXT.Tx_TransferRequest.Find(id);
                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "before", "TransferRequest", "post", "Id", keyValue); 

                        if (tx_TransferRequest != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferRequest.Status = "Posted";
                            tx_TransferRequest.IsAfterPosted = "Y";
                            tx_TransferRequest.ModifiedDate = dtModified;
                            tx_TransferRequest.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "after", "Tx_TransferRequest", "post", "Id", keyValue);


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

                        Tx_TransferRequest tx_TransferRequest = CONTEXT.Tx_TransferRequest.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "before", "Tx_TransferRequest", "cancel", "Id", keyValue);
                        if (tx_TransferRequest != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferRequest.Status = "Cancel";
                            tx_TransferRequest.CancelReason = cancelReason;
                            tx_TransferRequest.ModifiedDate = dtModified;
                            tx_TransferRequest.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "after", "Tx_TransferRequest", "cancel", "Id", keyValue);


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
                TransferRequestModel TransferRequestModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "before", "Tx_TransferRequest", "requestApproval", "Id", keyValue);

                        Tx_TransferRequest tx_TransferRequest = CONTEXT.Tx_TransferRequest.Find(id);
                        if (tx_TransferRequest != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_TransferRequest.IsApproval = "Y";
                            tx_TransferRequest.ApprovalMessages = approvalMessages;
                            tx_TransferRequest.ApprovalStatus = "Waiting";
                            tx_TransferRequest.ModifiedDate = dtModified;
                            tx_TransferRequest.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'TransferRequest',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "after", "Tx_TransferRequest", "requestApproval", "Id", keyValue);
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
                    this.Post(userId, id);
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

                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "before", "Tx_TransferRequest", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'TransferRequest', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "TransferRequest", CONTEXT, "after", "Tx_TransferRequest", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();

                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_TransferRequest"" T0
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

        public TransferRequestItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            TransferRequestItemTagView___ model = new TransferRequestItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_TransferRequest_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<TransferRequestItemTagView___>(sql, id, detId).FirstOrDefault();

                //sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                //            FROM ""Tx_TransferRequest_Item_Tag"" T0   
                //            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                //model.TransferRequestItemTagModel___ = CONTEXT.Database.SqlQuery<TransferRequestItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public TransferRequestApprovalView___ GetViewApproval(long id)
        {
            TransferRequestApprovalView___ model = new TransferRequestApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_TransferRequest"" T0 
                    LEFT JOIN ""Tx_TransferRequest_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<TransferRequestApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetTransferRequest_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

    }


    #endregion

}