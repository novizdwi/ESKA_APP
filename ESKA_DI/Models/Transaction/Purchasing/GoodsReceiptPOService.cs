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

namespace Models.Transaction.Purchasing
{
    #region Models

    public class GoodsReceiptPOModel
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

        public long? BaseId { get; set; }

        public long? BaseDetId { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? PostingDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string VendorCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string VendorName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public string DocNum_ { get; set; }

        public long? BaseEntry { get; set; }

        public string BaseDocNum { get; set; }

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

        public List<GoodsReceiptPO_DetailModel> ListDetails_ = new List<GoodsReceiptPO_DetailModel>();

        public List<GoodsReceiptPO_RefModel> ListRef_ = new List<GoodsReceiptPO_RefModel>();

        public GoodsReceiptPO_Detail Details_ { get; set; }

    }

    public class GoodsReceiptPO_ApprovalModel
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


    public class GoodsReceiptPO_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<GoodsReceiptPO_ApprovalModel> insertedRowValues { get; set; }
        public List<GoodsReceiptPO_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class GoodsReceiptPO_RefModel
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

    public class GoodsReceiptPO_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<GoodsReceiptPO_DetailModel> insertedRowValues { get; set; }
        public List<GoodsReceiptPO_DetailModel> modifiedRowValues { get; set; }
    }

    public class GoodsReceiptPO_DetailModel
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

        public int? IdPDO { get; set; }

        public string PDODocNum_ { get; set; }

        public int? GoodsReceiptDocEntry { get; set; }

        public string GRDocNum_ { get; set; }

        public string Bagian { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOpen { get; set; }

        public decimal? QuantityScan { get; set; }

        public decimal? QuantityValid { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public long? DocEntry { get; set; }

        public long? LineNum { get; set; }

        public long? BaseEntry { get; set; }

        public int? BaseLine { get; set; }

        public string LineStatus { get; set; }
    }

    public class GoodsReceiptPOItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<GoodsReceiptPOItemTagModel> GoodsReceiptPOItemTagModel___ { get; set; }
    }

    public class GoodsReceiptPOItemTagModel
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

    public class GRPOAddResultModel
    {
        public string DocEntry { get; set; }

        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    public class GoodsReceiptResultModel
    {
        public long DetId { get; set; }

        public long? GoodsReceiptId { get; set; }
    }

    public class GoodsReceiptPOApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<GoodsReceiptPO_ApprovalModel> ApprovalStepList__ = new List<GoodsReceiptPO_ApprovalModel>();

        public GoodsReceiptPO_Approval ApprovalStep__ { get; set; }
    }

    #endregion

    #region Services

    public class GoodsReceiptPOService
    {

        public GoodsReceiptPOModel GetNewModel(int userId)
        {
            GoodsReceiptPOModel model = new GoodsReceiptPOModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public GoodsReceiptPOModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public GoodsReceiptPOModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            GoodsReceiptPOModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_GoodsReceiptPO"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";
                model = CONTEXT.Database.SqlQuery<GoodsReceiptPOModel>(ssql, id).Single();

                if (model.DocEntry != null)
                {
                    string getDocNum = @"SELECT T1.""DocNum""
                    FROM ""Tx_GoodsReceiptPO"" T0
                    INNER JOIN """ + DbProvider.dbSap_Name + @""".""OPDN"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                    WHERE T0.""Id"" = :p0 
                    ORDER BY T0.""Id"" ASC
                ";
                    model.DocNum_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id).FirstOrDefault();
                }
                
                model.ListRef_ = this.GoodsReceiptPO_Refs(CONTEXT, id);
                model.ListDetails_ = this.GoodsReceiptPO_Details(CONTEXT, id);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'GoodsReceiptPO', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
			            FROM ""Tx_GoodsReceiptPO"" T0
			            INNER JOIN  ""Tx_GoodsReceiptPO_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
			            WHERE T0.""Id"" = :p0 
			            AND T1.""UserId"" = :p1
		            ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }
        public List<GoodsReceiptPO_DetailModel> GoodsReceiptPO_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GoodsReceiptPO_Details(CONTEXT, id);
            }

        }

        public List<GoodsReceiptPO_DetailModel> GoodsReceiptPO_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""DocNum"" AS ""PDODocNum_"", T2.""DocNum"" AS ""GRDocNum_""
                FROM ""Tx_GoodsReceiptPO_Item"" T0
                LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OWOR"" T1 ON T0.""IdPDO"" = T1.""DocEntry""
                LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OIGN"" T2 ON T0.""GoodsReceiptDocEntry"" = T2.""DocEntry""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var goodsReceiptPO = CONTEXT.Database.SqlQuery<GoodsReceiptPO_DetailModel>(ssql, id).ToList();
            return goodsReceiptPO;
        }

        public List<GoodsReceiptPO_RefModel> GoodsReceiptPO_Refs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GoodsReceiptPO_Refs(CONTEXT, id);
            }

        }

        public List<GoodsReceiptPO_RefModel> GoodsReceiptPO_Refs(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""TransDate"", T2.""FirstName"" AS ""BaseCreatedUser_""
                FROM ""Tx_GoodsReceiptPO_Ref"" T0
                INNER JOIN ""Tx_PurchaseOrder"" T1 ON T0.""BaseId"" = T1.""Id""
                LEFT JOIN ""Tm_User"" T2 ON T0.""BaseCreatedUser"" = T2.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var goodsReceiptPO = CONTEXT.Database.SqlQuery<GoodsReceiptPO_RefModel>(ssql, id).ToList();
            return goodsReceiptPO;
        }

        public List<GoodsReceiptPO_ApprovalModel> GetGoodsReceiptPO_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetGoodsReceiptPO_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<GoodsReceiptPO_ApprovalModel> GetGoodsReceiptPO_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_GoodsReceiptPO_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<GoodsReceiptPO_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public GoodsReceiptPOModel NavFirst(int userId)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public GoodsReceiptPOModel NavPrevious(int userId, long id = 0)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public GoodsReceiptPOModel NavNext(int userId, long id = 0)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public GoodsReceiptPOModel NavLast(int userId)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

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
                    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_AddItemDetail\"(:p0,:p1,'Refresh')", userId, id);
                }
            }
            return ret;
        }

        public long Add(GoodsReceiptPOModel model)
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
                            Tx_GoodsReceiptPO tx_GoodsReceiptPO = new Tx_GoodsReceiptPO();
                            CopyProperty.CopyProperties(model, tx_GoodsReceiptPO, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();                            

                            tx_GoodsReceiptPO.TransType = "GoodsReceiptPO";
                            tx_GoodsReceiptPO.CreatedDate = dtModified;
                            tx_GoodsReceiptPO.CreatedUser = model._UserId;
                            tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'GoodsReceiptPO','" + dateX + "','') ").SingleOrDefault();
                            tx_GoodsReceiptPO.TransNo = transNo;

                            CONTEXT.Tx_GoodsReceiptPO.Add(tx_GoodsReceiptPO);
                            CONTEXT.SaveChanges();
                            Id = tx_GoodsReceiptPO.Id;

                            String keyValue;
                            keyValue = tx_GoodsReceiptPO.Id.ToString();
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPO", CONTEXT, "after", "GoodsReceiptPO", "add", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_AddItemDetail\"(:p0,:p1,'Add')", model._UserId, Id);

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

        public void Update(GoodsReceiptPOModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPO", CONTEXT, "before", "GoodsReceiptPO", "update", "Id", keyValue);


                                Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                
                                if (tx_GoodsReceiptPO != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_GoodsReceiptPO, false, exceptColumns);

                                    var isApprovalActive = _Utils.GeneralGetList.GetApprovalActive("GoodsReceiptPO");

                                    tx_GoodsReceiptPO.IsApproval = !string.IsNullOrEmpty(isApprovalActive) ? isApprovalActive : "N";
                                    tx_GoodsReceiptPO.ModifiedDate = dtModified;
                                    tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                                    //if (model.StartDate != null)
                                    //{
                                    //    Tx_GoodsReceiptPO.Status2 = "On Progress";
                                    //}
                                    //else
                                    //{
                                    //    Tx_GoodsReceiptPO.Status2 = "Open";
                                    //}

                                    //if (model.EndDate != null)
                                    //{
                                    //    Tx_GoodsReceiptPO.Status2 = "Close";
                                    //}
                                    CONTEXT.SaveChanges();

                                    //if (model.Details_ != null)
                                    //{
                                    //    if (model.Details_.insertedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.insertedRowValues)
                                    //        {
                                    //            Detail_Add(CONTEXT, detail, model.Id, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.Details_.modifiedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.modifiedRowValues)
                                    //        {
                                    //            Detail_Update(CONTEXT, detail, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.Details_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.Details_.deletedRowKeys)
                                    //        {
                                    //            GoodsReceiptPO_DetailModel detailModel = new GoodsReceiptPO_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPO", CONTEXT, "after", "GoodsReceiptPO", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, GoodsReceiptPO_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = new Tx_GoodsReceiptPO_Item();

        //        CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_GoodsReceiptPO_Item.Id = Id;
        //        Tx_GoodsReceiptPO_Item.CreatedDate = dtModified;
        //        Tx_GoodsReceiptPO_Item.CreatedUser = UserId;
        //        Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
        //        Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_GoodsReceiptPO_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_GoodsReceiptPO_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_GoodsReceiptPO_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_GoodsReceiptPO_Item.Add(Tx_GoodsReceiptPO_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_GoodsReceiptPO_Item.DetId;

        //    }

        //    return DetId;

        //}

        //public void Detail_Update(HANA_APP CONTEXT, GoodsReceiptPO_DetailModel model, int UserId)
        //{
        //    if (model != null)
        //    {

        //        Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = CONTEXT.Tx_GoodsReceiptPO_Item.Find(model.DetId);

        //        if (Tx_GoodsReceiptPO_Item != null)
        //        {
        //            var exceptColumns = new string[] { "DetId", "Id" };
        //            CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false, exceptColumns);


        //            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //            Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
        //            Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;
        //            if (model.StartDate != null && model.EndDate == null)
        //            {
        //                Tx_GoodsReceiptPO_Item.Status = "On Progress";
        //            }
        //            else if (model.StartDate != null && model.EndDate != null)
        //            {
        //                Tx_GoodsReceiptPO_Item.Status = "Close";
        //            }
        //            else
        //            {
        //                Tx_GoodsReceiptPO_Item.Status = "Open";
        //            }
        //            CONTEXT.SaveChanges();

        //        }


        //    }

        //}

        //public void Detail_Delete(HANA_APP CONTEXT, GoodsReceiptPO_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item\"  WHERE \"DetId\"=:p0", model.DetId);

        //            CONTEXT.SaveChanges();


        //        }
        //    }

        //}

        public void Post(int userId, GoodsReceiptPOModel goodsReceiptPOModel)
        {
            try
            {
                Update(goodsReceiptPOModel);
                PostSAP(userId, goodsReceiptPOModel.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;
            GoodsReceiptPOModel syncGRPO = GetById(userId, id);

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        oCompany = SAPCachedCompany.GetCompany();
                        oCompany.StartTransaction();

                        String keyValue;
                        keyValue = id.ToString();


                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "before", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
                        if (tx_GoodsReceiptPO == null)
                        {
                            throw new Exception($"[VALIDATION] - GRPO Data not found");
                        }

                        GRPOAddResultModel GRPOResult = AddGoodsReceiptPO(oCompany, userId, id, syncGRPO);
                        if(GRPOResult != null)
                        {
                            //insert RFID items to pending
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_InsertItemTag\"(:p0,:p1, 'GoodsReceiptPO','A')", userId, id);
                        }
                        List<GoodsReceiptResultModel> GRResult = new List<GoodsReceiptResultModel>();
                        if (syncGRPO.ListDetails_.Any(q => q.IdPDO.GetValueOrDefault() != 0 && (q.QuantityValid > 0 && q.QuantityValid.HasValue )))
                        {
                            GRResult = AddReceiveFromProduction(oCompany, userId, id, syncGRPO);
                        }

                        //string ssql = @"SELECT ""DocNum"" 
                        //    FROM """ + DbProvider.dbSap_Name + @""".""OPDN"" T0
                        //    WHERE T0.""DocEntry"" = " + GRPOResult.DocEntry + @" 
                        //";

                        //string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();


                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_GoodsReceiptPO.PostingDate = dtModified;
                        tx_GoodsReceiptPO.DocEntry = Convert.ToInt64(GRPOResult.DocEntry);
                        //tx_GoodsReceiptPO.DocNum = docNum;

                        tx_GoodsReceiptPO.Status = "Posted";
                        tx_GoodsReceiptPO.IsAfterPosted = "Y";
                        tx_GoodsReceiptPO.ModifiedDate = dtModified;
                        tx_GoodsReceiptPO.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                        var caseStatements = string.Join(" ",
                            GRPOResult.LineMapping.Select(kv => $"WHEN T0.\"DetId\" = {kv.Key} THEN {kv.Value}"));

                        string sqlLine = $@"
                            UPDATE ""Tx_GoodsReceiptPO_Item"" T0 
                            SET ""LineNum"" = CASE {caseStatements} END,
                                ""DocEntry"" = {GRPOResult.DocEntry}
                            ";
                        var whereIn = string.Join(", ", GRPOResult.LineMapping.Keys);

                        if (GRResult.Count > 0) {
                        var GRStatements = string.Join(" ",
                            GRResult.Select(kv => $"WHEN T0.\"DetId\" = {kv.DetId} THEN {kv.GoodsReceiptId}"));
                            sqlLine += ",";
                            sqlLine += @"
                                    ""GoodsReceiptDocEntry"" = CASE " + GRStatements + " END ";
                            whereIn = string.Join(", ", GRPOResult.LineMapping.Keys.Union(GRResult.Select(x => x.DetId)).ToList());
                        }

                        sqlLine += @" WHERE T0.""DetId"" IN ("+ whereIn + ")";

                        CONTEXT.Database.ExecuteSqlCommand(sqlLine);


                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "after", "Tx_GoodsReceiptPO", "post", "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_UpdatePOStatus\"(:p0,:p1,'post')", userId, id);

                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        }

                        CONTEXT_TRANS.Commit();
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


        private GRPOAddResultModel AddGoodsReceiptPO(Company oCompany, int userId, long id, GoodsReceiptPOModel model)
        {
            GRPOAddResultModel result = new GRPOAddResultModel();

            int nErr;
            string errMsg;
            //SAPbobsCOM.Recordset rsDetailSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

            oDocument.DocDate = DateTime.Now;
            oDocument.DocDueDate = DateTime.Now;
            oDocument.TaxDate = DateTime.Now;

            oDocument.CardCode = model.VendorCode;
            oDocument.CardName = model.VendorCode;

            if (model.RefNo != null)
            {
                oDocument.NumAtCard = model.RefNo;
            }

            if (model.Comments != null)
            {
                oDocument.Comments = model.Comments;
            }

            if (model.Address != null)
            {
                oDocument.Address = model.Address;
            }

            oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;
            
            int i = 0;
            Dictionary<long, int> InsertedLine = new Dictionary<long, int>();
            if (model.ListDetails_.Count > 0)
            {
                foreach (var item in model.ListDetails_)
                {
                    oDocument.Lines.BaseType = 22;
                    oDocument.Lines.BaseEntry = Convert.ToInt32(item.BaseEntry);
                    oDocument.Lines.BaseLine = Convert.ToInt32(item.BaseLine);

                    //oDocument.Lines.ItemCode = item.ItemCode;
                    //oDocument.Lines.WarehouseCode = item.WhsCode;
                    oDocument.Lines.Quantity = (double)item.QuantityValid;

                    if (item.UomEntry != null)
                    {
                        oDocument.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                    }

                    //if (item.FreeText != null)
                    //{
                    //    oDocument.Lines.FreeText = item.FreeText;
                    //}

                    oDocument.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                    oDocument.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);

                    oDocument.Lines.Add();
                    InsertedLine.Add(Convert.ToInt64(item.DetId) , i);
                    i ++;
                }
            }

            int docAdd = oDocument.Add();
            if (docAdd != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Add Goods Receipt PO : " + nErr.ToString() + "|" + errMsg);
            }

            result.DocEntry = oCompany.GetNewObjectKey();
            result.LineMapping = InsertedLine;

            SapCompany.CleanUp(oDocument);
            return result;
        }

        

        private List<GoodsReceiptResultModel> AddReceiveFromProduction(Company oCompany, int userId, long id, GoodsReceiptPOModel model)
        {
            List<GoodsReceiptResultModel> result = new List<GoodsReceiptResultModel>();
            int nErr;
            string errMsg;
            
            if (model.ListDetails_.Count > 0)
            {
                foreach (var item in model.ListDetails_)
                {
                    if(item.IdPDO != 0)
                    {
                        SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
                        oDocument.DocDate = model.TransDate?? DateTime.Now;

                        oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                        oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;
                        oDocument.Comments = model.Comments;

                        oDocument.Lines.BaseType = 202;
                        oDocument.Lines.BaseEntry = item.IdPDO ?? 0;
                        oDocument.Lines.Quantity = double.Parse(item.QuantityValid.ToString());

                        oDocument.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                        oDocument.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);

                        oDocument.Lines.Add();

                        if (oDocument.Add() != 0)
                        {

                            nErr = oCompany.GetLastErrorCode();
                            errMsg = oCompany.GetLastErrorDescription();

                            SapCompany.CleanUp(oDocument);

                            throw new Exception("[VALIDATION] - Add ReceiptProduction | " + nErr.ToString() + "|" + errMsg);

                        }
                        string docEntry =  oCompany.GetNewObjectKey();
                        result.Add( 
                            new GoodsReceiptResultModel
                            {
                                DetId = Convert.ToInt64(item.DetId),
                                GoodsReceiptId = Convert.ToInt32(docEntry)
                            }
                        );

                    }

                }
            }

            return result;
        }

        public void Cancel(int userId, long Id, string cancelReason)
        {
            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        oCompany.StartTransaction();

                        String keyValue;
                        keyValue = Id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "before", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);

                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(Id);
                        if (tx_GoodsReceiptPO != null)
                        {
                            if (tx_GoodsReceiptPO.IsAfterPosted == "Y")
                            {
                                Cancel_GoodReceiptPo(oCompany, Convert.ToInt32(tx_GoodsReceiptPO.DocEntry));
                                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_InsertItemTag\"(:p0,:p1, 'GoodsReceiptPO','I')", userId, Id);
                            }

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_GoodsReceiptPO.Status = "Cancel";
                            tx_GoodsReceiptPO.CancelReason = cancelReason;
                            tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "after", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_UpdatePOStatus\"(:p0,:p1,'cancel')", userId, Id);
                        oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

                        CONTEXT_TRANS.Commit();
                    }

                    catch (Exception ex)
                    {
                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }

                        CONTEXT_TRANS.Rollback();

                        string errorMessage;
                        if (ex.Message.StartsWith("[VALIDATION]"))
                        {
                            errorMessage = ex.Message;
                        }
                        else
                        {
                            errorMessage = $"[VALIDATION] {ex.Message}";
                        }

                        throw new Exception(errorMessage);
                    }
                    finally
                    {
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }

        public bool Cancel_GoodReceiptPo(SAPbobsCOM.Company oCompany, int id)
        {
            int nErr;
            string errMsg;

            if (id != 0)
            {
                SAPbobsCOM.Documents oDocument = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                SAPbobsCOM.Documents oCancelDocument = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                if (oDocument.GetByKey(id))
                {
                    oCancelDocument = oDocument.CreateCancellationDocument();
                    if (oCancelDocument.Add() != 0)
                    {
                        nErr = oCompany.GetLastErrorCode();
                        errMsg = oCompany.GetLastErrorDescription();

                        SapCompany.CleanUp(oDocument);

                        throw new Exception("[VALIDATION] - Cancel GRPO |" + nErr.ToString() + "|" + errMsg);
                    }

                }
                
                SapCompany.CleanUp(oDocument);
            }

            return true;
        }

        public void RequestApproval(int userId, long id, int templateId, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {
                GoodsReceiptPOModel GoodsReceiptPOModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "before", "Tx_GoodsReceiptPO", "requestApproval", "Id", keyValue);

                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
                        if (tx_GoodsReceiptPO != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_GoodsReceiptPO.IsApproval = "Y";
                            tx_GoodsReceiptPO.ApprovalMessages = approvalMessages;
                            tx_GoodsReceiptPO.ApprovalStatus = "Waiting";
                            tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'GoodsReceiptPO',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "after", "Tx_GoodsReceiptPO", "requestApproval", "Id", keyValue);
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
                    //GoodsReceiptPOModel GoodsReceiptPOModel = GetById(userId, id);
                    //this.Update(GoodsReceiptPOModel);
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

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "before", "Tx_GoodsReceiptPO", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'GoodsReceiptPO', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "after", "Tx_GoodsReceiptPO", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_GoodsReceiptPO"" T0
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

        public GoodsReceiptPOItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;
            GoodsReceiptPOItemTagView___ model = new GoodsReceiptPOItemTagView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_GoodsReceiptPO_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPOItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_GoodsReceiptPO_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.GoodsReceiptPOItemTagModel___ = CONTEXT.Database.SqlQuery<GoodsReceiptPOItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public GoodsReceiptPOApprovalView___ GetViewApproval(long id)
        {
            GoodsReceiptPOApprovalView___ model = new GoodsReceiptPOApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_GoodsReceiptPO"" T0 
                    LEFT JOIN ""Tx_GoodsReceiptPO_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPOApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetGoodsReceiptPO_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }
    }


    #endregion

}