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

    public class TransferSummaryOutModel
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

        public DateTime? PostingDate { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public long? RequestId { get; set; }

        public string RequestNo { get; set; }

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

        public string TransitWhsCode { get; set; }

        public string TransitWhsName { get; set; }

        public string Address { get; set; }

        public string RefNo { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public string IsAnyDeactiveTag_ { get; set; }

        public List<TransferSummaryOut_DetailModel> ListDetails_ = new List<TransferSummaryOut_DetailModel>();

        public List<TransferSummaryOut_RefModel> ListRef_ = new List<TransferSummaryOut_RefModel>();

        public TransferSummaryOut_Detail Details_ { get; set; }
    }
        
    public class TransferSummaryOut_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferSummaryOut_DetailModel> insertedRowValues { get; set; }
        public List<TransferSummaryOut_DetailModel> modifiedRowValues { get; set; }
    }

    public class TransferSummaryOut_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferSummaryOut_ApprovalModel> insertedRowValues { get; set; }
        public List<TransferSummaryOut_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class TransferSummaryOut_RefModel
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

    public class TransferSummaryOut_ApprovalModel
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

    public class TransferSummaryOut_DetailModel
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

        public string Comments { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public string TransitWhsCode { get; set; }

        public string TransitWhsName { get; set; }

        public decimal? QuantityRequest { get; set; }

        public decimal? QuantityOpen { get; set; }

        public decimal? QuantityOpen_ { get; set; }

        public decimal? QuantityScan { get; set; }

        public decimal? QuantityValid { get; set; }

        public decimal? QuantityPosted { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public long? DocEntry { get; set; }

        public long? LineNum { get; set; }

        public long? RequestId { get; set; }

        public int? RequestRequestDetId { get; set; }

        public string LineStatus { get; set; }
    }

    public class TransferSummaryOutItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<TransferSummaryOutItemTagModel> TransferSummaryOutItemTagModel___ { get; set; }
    }

    public class TransferSummaryOutItemTagModel
    {
        public int RowNo { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public long DetDetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string TagId { get; set; }

        public decimal? Quantity { get; set; }

        public string EventType { get; set; }

        public string Status { get; set; }
    }

    public class TransferSummaryOutAddResultModel
    {
        public string DocEntry { get; set; }
        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    public class TransferSummaryOutApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<TransferSummaryOut_ApprovalModel> ApprovalStepList__ = new List<TransferSummaryOut_ApprovalModel>();

        public TransferSummaryOut_Approval ApprovalStep__ { get; set; }
    }

    #endregion

    #region Services

    public class TransferSummaryOutService
    {

        public TransferSummaryOutModel GetNewModel(int userId)
        {
            TransferSummaryOutModel model = new TransferSummaryOutModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public TransferSummaryOutModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public TransferSummaryOutModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            TransferSummaryOutModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_TransferSummaryOut"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<TransferSummaryOutModel>(ssql, id).Single();

                model.ListRef_ = this.TransferSummaryOut_Refs(CONTEXT, id);
                model.ListDetails_ = this.TransferSummaryOut_Details(CONTEXT, id);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'TransferSummaryOut', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }
                if (method == "Post")
                {
                    ssql = @"SELECT TOP 1 'Y'
                        FROM ""Tx_TransferSummaryOut_Item_Tag"" T0
                        INNER JOIN ""Tm_Item_Warehouse_Tag"" T1 ON T0.""TagId"" = T1.""TagId""
                        WHERE T1.""Status"" = 'I'
                        AND T0.""Id"" = :p0
                    ";
                    string checkDeactive = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();
                    model.IsAnyDeactiveTag_ = checkDeactive;
                } 
                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
			            FROM ""Tx_TransferSummaryOut"" T0
			            INNER JOIN  ""Tx_TransferSummaryOut_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
			            WHERE T0.""Id"" = :p0 
			            AND T1.""UserId"" = :p1
		            ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }

        public List<TransferSummaryOut_RefModel> TransferSummaryOut_Refs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferSummaryOut_Refs(CONTEXT, id);
            }

        }

        public List<TransferSummaryOut_RefModel> TransferSummaryOut_Refs(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""TransDate"", T2.""FirstName"" AS ""BaseCreatedUser_""
                FROM ""Tx_TransferSummaryOut_Ref"" T0
                INNER JOIN ""Tx_TransferOut"" T1 ON T0.""BaseId"" = T1.""Id""
                LEFT JOIN ""Tm_User"" T2 ON T0.""BaseCreatedUser"" = T2.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var result = CONTEXT.Database.SqlQuery<TransferSummaryOut_RefModel>(ssql, id).ToList();
            return result;
        }

        public List<TransferSummaryOut_DetailModel> TransferSummaryOut_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferSummaryOut_Details(CONTEXT, id);
            }

        }

        public List<TransferSummaryOut_DetailModel> TransferSummaryOut_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""QuantityOpen"" AS ""QuantityOpen_""
                FROM ""Tx_TransferSummaryOut_Item"" T0
                LEFT JOIN ""Tx_TransferRequest_Item"" T1 ON T0.""RequestDetId"" = T1.""DetId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var transferSummaryOut = CONTEXT.Database.SqlQuery<TransferSummaryOut_DetailModel>(ssql, id).ToList();
            return transferSummaryOut;
        }

        public List<TransferSummaryOut_ApprovalModel> GetTransferSummaryOut_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetTransferSummaryOut_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<TransferSummaryOut_ApprovalModel> GetTransferSummaryOut_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_TransferSummaryOut_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<TransferSummaryOut_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public TransferSummaryOutModel NavFirst(int userId)
        {
            TransferSummaryOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryOut\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public TransferSummaryOutModel NavPrevious(int userId, long id = 0)
        {
            TransferSummaryOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryOut\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TransferSummaryOutModel NavNext(int userId, long id = 0)
        {
            TransferSummaryOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryOut\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TransferSummaryOutModel NavLast(int userId)
        {
            TransferSummaryOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryOut\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public bool RefreshItem(int userId, long id)
        {
            bool ret = true;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryOut_AddItemDetail\"(:p0,:p1,'Refresh')", userId, id);
                }
            }
            return ret;
        }

        public long Add(TransferSummaryOutModel model)
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
                            Tx_TransferSummaryOut tx_TransferSummaryOut = new Tx_TransferSummaryOut();
                            CopyProperty.CopyProperties(model, tx_TransferSummaryOut, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            
                            tx_TransferSummaryOut.TransType = "TransferSummaryOut";
                            tx_TransferSummaryOut.CreatedDate = dtModified;
                            tx_TransferSummaryOut.CreatedUser = model._UserId;
                            tx_TransferSummaryOut.ModifiedDate = dtModified;
                            tx_TransferSummaryOut.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TransferSummaryOut','" + dateX + "','') ").SingleOrDefault();
                            tx_TransferSummaryOut.TransNo = transNo;

                            CONTEXT.Tx_TransferSummaryOut.Add(tx_TransferSummaryOut);
                            CONTEXT.SaveChanges();
                            Id = tx_TransferSummaryOut.Id;

                            String keyValue;
                            keyValue = tx_TransferSummaryOut.Id.ToString();
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "TransferSummaryOut", CONTEXT, "after", "TransferSummaryOut", "add", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryOut_AddItemDetail\"(:p0,:p1,'Add')", model._UserId, Id);

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

        public void Update(TransferSummaryOutModel model, string method ="")
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "TransferSummaryOut", CONTEXT, "before", "TransferSummaryOut", "update", "Id", keyValue);


                                Tx_TransferSummaryOut tx_TransferSummaryOut = CONTEXT.Tx_TransferSummaryOut.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                             
                                if (tx_TransferSummaryOut != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_TransferSummaryOut, false, exceptColumns);

                                    //Tx_TransferSummaryOut.ApprovalStatus = isApprovalActive == "Y" && Tx_TransferSummaryOut.ApprovalStatus == "" ? "Waiting" : "Approved";
                                    tx_TransferSummaryOut.ModifiedDate = dtModified;
                                    tx_TransferSummaryOut.ModifiedUser = model._UserId;

                                    //if (model.StartDate != null)
                                    //{
                                    //    Tx_TransferSummaryOut.Status2 = "On Progress";
                                    //}
                                    //else
                                    //{
                                    //    Tx_TransferSummaryOut.Status2 = "Open";
                                    //}

                                    //if (model.EndDate != null)
                                    //{
                                    //    Tx_TransferSummaryOut.Status2 = "Close";
                                    //}

                                    if (model.Details_ != null)
                                    {
                                    //    if (model.Details_.insertedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.insertedRowValues)
                                    //        {
                                    //            Detail_Add(CONTEXT, detail, model.Id, model._UserId);
                                    //        }
                                    //    }

                                    //if (model.Details_.modifiedRowValues != null)
                                    //{
                                    //    foreach (var detail in model.Details_.modifiedRowValues)
                                    //    {
                                    //        Detail_Update(CONTEXT, detail, model._UserId);
                                    //    }
                                    //}

                                    //    if (model.Details_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.Details_.deletedRowKeys)
                                    //        {
                                    //            TransferSummaryOut_DetailModel detailModel = new TransferSummaryOut_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    }

                                    if(method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryOut_UpdateItem\"(:p0,:p1, 'before')", model._UserId, model.Id);
                                    }
                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TransferSummaryOut", CONTEXT, "after", "TransferSummaryOut", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, TransferSummaryOut_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_TransferSummaryOut_Item Tx_TransferSummaryOut_Item = new Tx_TransferSummaryOut_Item();

        //        CopyProperty.CopyProperties(model, Tx_TransferSummaryOut_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_TransferSummaryOut_Item.Id = Id;
        //        Tx_TransferSummaryOut_Item.CreatedDate = dtModified;
        //        Tx_TransferSummaryOut_Item.CreatedUser = UserId;
        //        Tx_TransferSummaryOut_Item.ModifiedDate = dtModified;
        //        Tx_TransferSummaryOut_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_TransferSummaryOut_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_TransferSummaryOut_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_TransferSummaryOut_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_TransferSummaryOut_Item.Add(Tx_TransferSummaryOut_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_TransferSummaryOut_Item.DetId;

        //    }

        //    return DetId;

        //}

        public void Detail_Update(HANA_APP CONTEXT, TransferSummaryOut_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_TransferSummaryOut_Item Tx_TransferSummaryOut_Item = CONTEXT.Tx_TransferSummaryOut_Item.Find(model.DetId);

                if (Tx_TransferSummaryOut_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_TransferSummaryOut_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_TransferSummaryOut_Item.ModifiedDate = dtModified;
                    Tx_TransferSummaryOut_Item.ModifiedUser = UserId;
                    //CONTEXT.SaveChanges();
                }


            }

        }

        //public void Detail_Delete(HANA_APP CONTEXT, TransferSummaryOut_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_TransferSummaryOut_Item\"  WHERE \"DetId\"=:p0", model.DetId);

        //            CONTEXT.SaveChanges();


        //        }
        //    }

        //}
        public void Post(int userId, TransferSummaryOutModel transferSummaryOutModel)
        {
            try
            {
                Update(transferSummaryOutModel, "Post");
                PostSAP(userId, transferSummaryOutModel.Id);

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
                        //oCompany.StartTransaction();

                        String keyValue;
                        keyValue = id.ToString();

                        TransferSummaryOutModel syncTransferSummaryOut = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "before", "Tx_TransferSummaryOut", "post", "Id", keyValue);
                        if (syncTransferSummaryOut.ListDetails_.All(q => !q.QuantityPosted.HasValue || q.QuantityPosted == 0 ) )
                        {
                            throw new Exception("One or more RFID is already posted in another transaction");
                        }

                        if (syncTransferSummaryOut.IsAnyDeactiveTag_ == "Y")
                        {
                            throw new Exception("One or more RFID statuses has already changed to Inactive");
                        }

                        Tx_TransferSummaryOut tx_TransferSummaryOut = CONTEXT.Tx_TransferSummaryOut.Find(id);
                        if (tx_TransferSummaryOut != null)
                        {
                            //TransferSummaryOutAddResultModel TransferSummaryOutResult = new TransferSummaryOutAddResultModel();
                            //string docEntry_ = 4382.ToString();

                            string docEntry_ = AddTransferSummaryOut(oCompany, userId, id, syncTransferSummaryOut);

                            if (!string.IsNullOrEmpty(docEntry_))
                            {
                                string ssql = @"SELECT ""DocNum"" 
                                            FROM """ + DbProvider.dbSap_Name + @""".""OWTR"" T0
                                            WHERE T0.""DocEntry"" = " + docEntry_ + @" 
                                            ";

                                string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tx_TransferSummaryOut.PostingDate = dtModified;
                                tx_TransferSummaryOut.DocEntry = Convert.ToInt64(docEntry_);
                                tx_TransferSummaryOut.DocNum = docNum;
                                tx_TransferSummaryOut.PostingDate = dtModified;

                                tx_TransferSummaryOut.Status = "Posted";
                                tx_TransferSummaryOut.IsAfterPosted = "Y";
                                tx_TransferSummaryOut.ModifiedDate = dtModified;
                                tx_TransferSummaryOut.ModifiedUser = userId;

                                CONTEXT.SaveChanges();

                                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_TransferItemTag\"(:p0,:p1, 'TransferSummaryOut', 'A')", userId, id);
                                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryOut_UpdateItem\"(:p0,:p1, 'after')", userId, id);
                                SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "after", "Tx_TransferSummaryOut", "post", "Id", keyValue);
                             

                                CONTEXT_TRANS.Commit();
                            }                            
                        }

                    }

                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();

                        //if (oCompany.InTransaction)
                        //{
                        //    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                        //}

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
                        SapCompany.CleanUpGCCollect();
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }

        private string AddTransferSummaryOut(Company oCompany, int userId, long id, TransferSummaryOutModel model)
        {
            string result ;

            int nErr;
            string errMsg;
            string newEntry_ = string.Empty;
            //SAPbobsCOM.Recordset rsDetailSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            SAPbobsCOM.StockTransfer oInventoryTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

            oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebId").Value = model.Id.ToString();
            oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;

            oInventoryTransfer.DocDate = DateTime.Now;
            oInventoryTransfer.DueDate = DateTime.Now;
            oInventoryTransfer.TaxDate = DateTime.Now;

            oInventoryTransfer.FromWarehouse = model.FromWhsCode;
            oInventoryTransfer.ToWarehouse = model.TransitWhsCode;

            //oDocument.CardCode = model.VendorCode;
            //oDocument.CardName = model.VendorCode;

            if (model.Comments != null)
            {
                oInventoryTransfer.Comments = model.Comments;
            }

            if (model.Address != null)
            {
                oInventoryTransfer.Address = model.Address;
            }

            var insertedLineIds = new Dictionary<long, int>();
            int i = 0;
            if (model.ListDetails_.Count > 0)
            {
                foreach (var item in model.ListDetails_.Where(x => x.QuantityPosted.HasValue && x.QuantityPosted > 0) )
                {
                    //oDocument.Lines.BaseType = InvBaseDocTypeEnum.InventoryTransferRequest;
                    //oDocument.Lines.BaseEntry = Convert.ToInt32(model.BaseEntry);
                    //oDocument.Lines.BaseLine = Convert.ToInt32(item.BaseLine);

                    oInventoryTransfer.Lines.ItemCode = item.ItemCode;
                    oInventoryTransfer.Lines.FromWarehouseCode = item.FromWhsCode;
                    oInventoryTransfer.Lines.WarehouseCode = item.TransitWhsCode;
                    oInventoryTransfer.Lines.Quantity = (double)item.QuantityPosted;

                    if (item.UomEntry != null)
                    {
                        oInventoryTransfer.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                    }

                    //if (item.FreeText != null)
                    //{
                    //    oInventoryTransfer.Lines.UserFields.Fields.Item("U_H_KET").Value = item.FreeText;
                    //}

                    oInventoryTransfer.Lines.Add();
                    insertedLineIds.Add(Convert.ToInt64(item.DetId), i);
                    i += 1;
                }
            }

            if (oInventoryTransfer.Add() != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oInventoryTransfer);

                throw new Exception("[VALIDATION] - Add Transfer Summary Out : " + nErr.ToString() + "|" + errMsg);
            }
            result = oCompany.GetNewObjectKey();

            return result;
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

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "before", "Tx_TransferSummaryOut", "cancel", "Id", keyValue);

                        Tx_TransferSummaryOut tx_TransferSummaryOut = CONTEXT.Tx_TransferSummaryOut.Find(Id);
                        if (tx_TransferSummaryOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferSummaryOut.Status = "Cancel";
                            tx_TransferSummaryOut.ApprovalStatus = "Rejected";
                            tx_TransferSummaryOut.CancelReason = cancelReason;
                            tx_TransferSummaryOut.ModifiedDate = dtModified;
                            tx_TransferSummaryOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "after", "Tx_TransferSummaryOut", "cancel", "Id", keyValue);

                        
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
                TransferSummaryOutModel TransferSummaryOutModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "before", "Tx_TransferSummaryOut", "requestApproval", "Id", keyValue);

                        Tx_TransferSummaryOut tx_TransferSummaryOut = CONTEXT.Tx_TransferSummaryOut.Find(id);
                        if (tx_TransferSummaryOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_TransferSummaryOut.IsApproval = "Y";
                            tx_TransferSummaryOut.ApprovalMessages = approvalMessages;
                            tx_TransferSummaryOut.ApprovalStatus = "Waiting";
                            tx_TransferSummaryOut.ModifiedDate = dtModified;
                            tx_TransferSummaryOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'TransferSummaryOut',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "after", "Tx_TransferSummaryOut", "requestApproval", "Id", keyValue);
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
                    //TransferSummaryOutModel TransferSummaryOutModel = GetById(userId, id);
                    //this.Update(TransferSummaryOutModel, "Post");
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryOut_UpdateItem\"(:p0,:p1, 'before')", userId, id);
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

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "before", "Tx_TransferSummaryOut", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'TransferSummaryOut', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryOut", CONTEXT, "after", "Tx_TransferSummaryOut", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_TransferSummaryOut"" T0
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

        public TransferSummaryOutItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;
            TransferSummaryOutItemTagView___ model = new TransferSummaryOutItemTagView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_TransferSummaryOut_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<TransferSummaryOutItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_TransferSummaryOut_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.TransferSummaryOutItemTagModel___ = CONTEXT.Database.SqlQuery<TransferSummaryOutItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public TransferSummaryOutApprovalView___ GetViewApproval(long id)
        {
            TransferSummaryOutApprovalView___ model = new TransferSummaryOutApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_TransferSummaryOut"" T0 
                    LEFT JOIN ""Tx_TransferSummaryOut_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<TransferSummaryOutApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetTransferSummaryOut_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

    }


    #endregion

}