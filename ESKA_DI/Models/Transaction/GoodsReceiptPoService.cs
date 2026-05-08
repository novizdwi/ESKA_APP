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

    public class GoodsReceiptPoModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        public string TransType { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public DateTime? DocDate { get; set; }

        public string RefNo { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

        public List<GoodsReceiptPoItem> ListDetails_ = new List<GoodsReceiptPoItem>();

        public GoodsReceiptPoItem_Detail Details_ { get; set; }
    }


    public class GoodsReceiptPoItem_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockOpname_DetailModel> insertedRowValues { get; set; }
        public List<StockOpname_DetailModel> modifiedRowValues { get; set; }
    }

    public class GoodsReceiptPoItem
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long DetId { get; set; }

        public long? Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public int? Quantity { get; set; }

        public string Uom { get; set; }

        public string Whse { get; set; }

        public decimal? Netto { get; set; }

        public string Department { get; set; }

        public int? LineNum { get; set; }

        public string LineStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public List<GoodsReceiptPoItem> ListDetails_ { get; set; } = new List<GoodsReceiptPoItem>();

    }

    public class GoodsReceiptPoItemBatch
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long DetDetId { get; set; }

        public long? DetId { get; set; }

        public string Batch { get; set; }

        public int? Quantity { get; set; }

        public DateTime? AdmissionDate { get; set; }

        public decimal? Netto { get; set; }

        public int? LineNum { get; set; }

        public string LineStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class GoodsReceiptPoItemBatchScale
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long DetDetDetId { get; set; }

        public long? DetDetId { get; set; }

        public int? Quantity { get; set; }

        public string Uom { get; set; }

        public decimal? Netto { get; set; }

        public int? LineNum { get; set; }

        public string LineStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

    }

    //public class GoodsReceiptPo_Approval
    //{
    //    public List<long> deletedRowKeys { get; set; }
    //    public List<GoodsReceiptPo_ApprovalModel> insertedRowValues { get; set; }
    //    public List<GoodsReceiptPo_ApprovalModel> modifiedRowValues { get; set; }
    //}

    //public class GoodsReceiptPo_RefModel
    //{
    //    private FormModeEnum _FormModeEnum = FormModeEnum.New;

    //    public FormModeEnum _FormMode
    //    {
    //        get { return this._FormModeEnum; }
    //        set { this._FormModeEnum = value; }
    //    }

    //    public int _UserId { get; set; }

    //    public long? Id { get; set; }

    //    public long? DetId { get; set; }

    //    public long? BaseId { get; set; }

    //    public string BaseNo { get; set; }

    //    public DateTime? BaseCreatedDate { get; set; }

    //    public string ScanDeviceId { get; set; }

    //    public string Status { get; set; }

    //    public string Comments { get; set; }

    //    public string BaseCreatedUser_ { get; set; }
    //}

    //public class GoodsReceiptPo_ApprovalModel
    //{
    //    private FormModeEnum _FormModeEnum = FormModeEnum.New;

    //    public FormModeEnum _FormMode
    //    {
    //        get { return this._FormModeEnum; }
    //        set { this._FormModeEnum = value; }
    //    }

    //    public int _UserId { get; set; }

    //    public int? Id { get; set; }

    //    public int? DetId { get; set; }

    //    public int? StageId { get; set; }

    //    public int? UserId { get; set; }

    //    public string Username { get; set; }

    //    public int? Step { get; set; }

    //    public string Status { get; set; }

    //    public string Comments { get; set; }

    //    public DateTime? ActionDate { get; set; }
    //}

    //public class GoodsReceiptPo_DetailModel
    //{

    //    private FormModeEnum _FormModeEnum = FormModeEnum.New;

    //    public FormModeEnum _FormMode
    //    {
    //        get { return this._FormModeEnum; }
    //        set { this._FormModeEnum = value; }
    //    }

    //    public int? RowNo { get; set; }

    //    public int _UserId { get; set; }

    //    public long? Id { get; set; }

    //    public long? DetId { get; set; }

    //    public string ItemCode { get; set; }

    //    public string ItemName { get; set; }

    //    public string WhsCode { get; set; }

    //    public string AcctCode { get; set; }

    //    public string AcctName { get; set; }

    //    public decimal? Quantity { get; set; }

    //    public decimal? QtyVariance { get; set; }

    //    public decimal? QtyVariance_ { get; set; }

    //    public decimal? QuantityOnHandSAP_ { get; set; }

    //    public int? UomEntry { get; set; }

    //    public string Uom { get; set; }

    //    public decimal? UnitPriceTc { get; set; }

    //    public decimal? LineTotal { get; set; }

    //    public string FreeText { get; set; }

    //}

    //public class GoodsReceiptPoAddResultModel
    //{
    //    public string DocEntry { get; set; }
    //    public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    //}

    //public class GoodsReceiptPoApprovalView___
    //{
    //    public long Id { get; set; }

    //    public string FirstName { get; set; }

    //    public string Status { get; set; }

    //    public string RequestMassage { get; set; }

    //    public string ApprovalMessages { get; set; }

    //    public DateTime? CreatedDate { get; set; }

    //    public List<GoodsReceiptPo_ApprovalModel> ApprovalStepList__ = new List<GoodsReceiptPo_ApprovalModel>();

    //    public GoodsReceiptPo_Approval ApprovalStep__ { get; set; }
    //}

    #endregion

    #region Services

    public class GoodsReceiptPoService
    {

        public GoodsReceiptPoModel GetNewModel(int userId)
        {
            GoodsReceiptPoModel model = new GoodsReceiptPoModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public GoodsReceiptPoModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public GoodsReceiptPoModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            GoodsReceiptPoModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_GoodsReceiptPO"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPoModel>(ssql, id).Single();

               // model.ListDetails_ = this.GoodsReceiptPo_Details(CONTEXT, id);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'GoodsReceiptPo', :p1) ", userId, model.Id).FirstOrDefault();
                    //model.ApprovalTemplateId_ = approvalId;
                }
                //  if (method == "Post")
                //  {
                //      ssql = @"SELECT TOP 1 'Y'
                //          FROM ""Tx_GoodsReceiptPO_Item_Tag"" T0
                //          INNER JOIN ""Tm_Item_Warehouse_Tag"" T1 ON T0.""TagId"" = T1.""TagId""
                //          WHERE T1.""Status"" = 'I'
                //          AND T0.""Id"" = :p0
                //      ";
                //      string checkDeactive = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();
                //  } 
                //  if (model.ApprovalStatus == "Waiting")
                //  {
                //      string getDocNum = @"SELECT 'Y'
                // FROM ""Tx_GoodsReceiptPO"" T0
                // INNER JOIN  ""Tx_GoodsReceiptPO_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
                // WHERE T0.""Id"" = :p0 
                // AND T1.""UserId"" = :p1
                //";
                //      model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                //  }

            }

            return model;
        }

        //public List<GoodsReceiptPo_RefModel> GoodsReceiptPo_Refs(long id = 0)
        //{
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        return GoodsReceiptPo_Refs(CONTEXT, id);
        //    }

        //}

        //public List<GoodsReceiptPo_RefModel> GoodsReceiptPo_Refs(HANA_APP CONTEXT, long id = 0)
        //{
        //    string ssql = @"SELECT T0.*, T1.""TransDate"", T2.""FirstName"" AS ""BaseCreatedUser_""
        //        FROM ""Tx_GoodsReceiptPO_Ref"" T0
        //        INNER JOIN ""Tx_TransferOut"" T1 ON T0.""BaseId"" = T1.""Id""
        //        LEFT JOIN ""Tm_User"" T2 ON T0.""BaseCreatedUser"" = T2.""Id""
        //        WHERE T0.""Id"" =:p0
        //        ORDER BY T0.""DetId"" ASC
        //    ";
        //    var result = CONTEXT.Database.SqlQuery<GoodsReceiptPo_RefModel>(ssql, id).ToList();
        //    return result;
        //}

        public List<GoodsReceiptPoItem> GoodsReceiptPo_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GoodsReceiptPo_Details(CONTEXT, id);
            }

        }

        public List<GoodsReceiptPoItem> GoodsReceiptPo_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_GoodsReceiptPO_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var GoodsReceiptPo = CONTEXT.Database.SqlQuery<GoodsReceiptPoItem>(ssql, id).ToList();
            return GoodsReceiptPo;
        }

        //public List<GoodsReceiptPoItem> GetGoodsReceiptPo_ApprovalSteps(long id = 0)
        //{
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        return GetGoodsReceiptPo_ApprovalSteps(CONTEXT, id);
        //    }

        //}

        //public List<GoodsReceiptPo_ApprovalModel> GetGoodsReceiptPo_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        //{
        //    string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
        //        FROM ""Tx_GoodsReceiptPO_Approval"" T0
        //        LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
        //        WHERE T0.""Id"" =:p0
        //        ORDER BY T0.""Step"" ASC
        //    ";
        //    var listData = CONTEXT.Database.SqlQuery<GoodsReceiptPo_ApprovalModel>(ssql, id).ToList();
        //    return listData;
        //}

        public GoodsReceiptPoModel NavFirst(int userId)
        {
            GoodsReceiptPoModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPo");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public GoodsReceiptPoModel NavPrevious(int userId, long id = 0)
        {
            GoodsReceiptPoModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPo");
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

        public GoodsReceiptPoModel NavNext(int userId, long id = 0)
        {
            GoodsReceiptPoModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPo");
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

        public GoodsReceiptPoModel NavLast(int userId)
        {
            GoodsReceiptPoModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPo");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(GoodsReceiptPoModel model)
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
                            Tx_GoodsReceiptPO Tx_GoodsReceiptPO = new Tx_GoodsReceiptPO();
                            CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            Tx_GoodsReceiptPO.TransType = "GoodsReceiptPo";
                            Tx_GoodsReceiptPO.CreatedDate = dtModified;
                            Tx_GoodsReceiptPO.CreatedUser = model._UserId;
                            Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            Tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'GoodsReceiptPo','" + dateX + "','') ").SingleOrDefault();
                            Tx_GoodsReceiptPO.TransNo = transNo;

                            CONTEXT.Tx_GoodsReceiptPO.Add(Tx_GoodsReceiptPO);
                            CONTEXT.SaveChanges();
                            Id = Tx_GoodsReceiptPO.Id;

                            String keyValue;
                            keyValue = Tx_GoodsReceiptPO.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "after", "GoodsReceiptPo", "add", "Id", keyValue);

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

        public void Update(GoodsReceiptPoModel model, string method = "")
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "before", "GoodsReceiptPo", "update", "Id", keyValue);


                                Tx_GoodsReceiptPO Tx_GoodsReceiptPo = CONTEXT.Tx_GoodsReceiptPO.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                if (Tx_GoodsReceiptPo != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_GoodsReceiptPo, false, exceptColumns);

                                    //Tx_GoodsReceiptPO.ApprovalStatus = isApprovalActive == "Y" && Tx_GoodsReceiptPO.ApprovalStatus == "" ? "Waiting" : "Approved";
                                    Tx_GoodsReceiptPo.ModifiedDate = dtModified;
                                    Tx_GoodsReceiptPo.ModifiedUser = model._UserId;

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
                                    //            GoodsReceiptPo_DetailModel detailModel = new GoodsReceiptPo_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}

                                    if (method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_UpdateItem\"(:p0,:p1, 'before')", model._UserId, model.Id);
                                    }
                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "after", "GoodsReceiptPo", "update", "Id", keyValue);

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

        //public long Detail_Add(HANA_APP CONTEXT, GoodsReceiptPo_DetailModel model, long Id, int UserId)
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

        //        CONTEXT.Tx_GoodsReceiptPO_Item.Add(Tx_GoodsReceiptPO_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_GoodsReceiptPO_Item.DetId;

        //    }

        //    return DetId;

        //}

        //public void Detail_Update(HANA_APP CONTEXT, GoodsReceiptPo_DetailModel model, int UserId)
        //{
        //    if (model != null)
        //    {

        //        Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = CONTEXT.Tx_GoodsReceiptPO_Item.Find(model.DetId);

        //        if (Tx_GoodsReceiptPO_Item != null)
        //        {
        //            var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
        //            CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false, exceptColumns);


        //            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //            Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
        //            Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;
        //            //CONTEXT.SaveChanges();
        //        }


        //    }

        //}

        //public void Detail_Delete(HANA_APP CONTEXT, GoodsReceiptPo_DetailModel model)
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

        public void Post(int userId, GoodsReceiptPoModel GoodsReceiptPoModel)
        {
            try
            {
                Update(GoodsReceiptPoModel, "Post");
                PostSAP(userId, GoodsReceiptPoModel.Id);

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

                        GoodsReceiptPoModel syncGoodsReceiptPo = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

                        Tx_GoodsReceiptPO Tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
                        if (Tx_GoodsReceiptPO != null)
                        {

                            int docEntry_ = AddInventoryPosting(oCompany, userId, id, syncGoodsReceiptPo);
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

                            // Tx_GoodsReceiptPO.PostingDate = dtModified;
                            Tx_GoodsReceiptPO.DocEntry = Convert.ToInt32(docEntry_);
                            Tx_GoodsReceiptPO.DocNum = docNum;
                            //Tx_GoodsReceiptPO.PostingDate = dtModified;

                            Tx_GoodsReceiptPO.Status = "Posted";
                            // Tx_GoodsReceiptPO.IsAfterPosted = "Y";
                            Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            Tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();

                            SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

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

        private int AddInventoryPosting(Company oCompany, int userId, long id, GoodsReceiptPoModel model)
        {
            int newDocEntry = -1;
            int nErr;
            string errMsg;

            SAPbobsCOM.CompanyService oCS = (SAPbobsCOM.CompanyService)oCompany.GetCompanyService();
            SAPbobsCOM.InventoryPostingsService oInventoryPostingsService = oCS.GetBusinessService(SAPbobsCOM.ServiceTypes.InventoryPostingsService);
            SAPbobsCOM.InventoryPosting oDocument = oInventoryPostingsService.GetDataInterface(SAPbobsCOM.InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

            oDocument.PostingDate = DateTime.Now;

            //if (!string.IsNullOrWhiteSpace(model.Comments))
            //{
            //    oDocument.Remarks = model.Comments;
            //    oDocument.JournalRemark = model.Comments;
            //}

            oDocument.UserFields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            oDocument.UserFields.Item("U_IDU_WebTransNo").Value = model.TransNo;
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

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);

                        Tx_GoodsReceiptPO Tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(Id);
                        if (Tx_GoodsReceiptPO != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_GoodsReceiptPO.Status = "Cancel";
                            // Tx_GoodsReceiptPO.ApprovalStatus = "Rejected";
                            // Tx_GoodsReceiptPO.CancelReason = cancelReason;
                            Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            Tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);


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
                GoodsReceiptPoModel GoodsReceiptPoModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "requestApproval", "Id", keyValue);

                        Tx_GoodsReceiptPO Tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
                        if (Tx_GoodsReceiptPO != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            // Tx_GoodsReceiptPO.IsApproval = "Y";
                            // Tx_GoodsReceiptPO.ApprovalMessages = approvalMessages;
                            // Tx_GoodsReceiptPO.ApprovalStatus = "Waiting";
                            Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            Tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'GoodsReceiptPo',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", "requestApproval", "Id", keyValue);
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
                    //GoodsReceiptPoModel GoodsReceiptPoModel = GetById(userId, id);
                    //this.Update(GoodsReceiptPoModel, "Post");
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_UpdateItem\"(:p0,:p1, 'before')", userId, id);
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

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'GoodsReceiptPo', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", action.ToLower(), "Id", keyValue);

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

        //public GoodsReceiptPoApprovalView___ GetViewApproval(long id)
        //{
        //    GoodsReceiptPoApprovalView___ model = new GoodsReceiptPoApprovalView___();
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        string sql = @"
        //            SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
        //            FROM ""Tx_GoodsReceiptPO"" T0 
        //            LEFT JOIN ""Tx_GoodsReceiptPO_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
        //            LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
        //            WHERE T0.""Id""=:p0 
        //        ";

        //        model = CONTEXT.Database.SqlQuery<GoodsReceiptPoApprovalView___>(sql, id).FirstOrDefault();

        //        model.ApprovalStepList__ = GetGoodsReceiptPo_ApprovalSteps(CONTEXT, id);

        //    }
        //    return model;
        //}

    }


    #endregion

}