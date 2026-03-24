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

    public class PurchaseOrderScanModel
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

        [Required(ErrorMessage = "required")]
        public string VendorCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string VendorName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public string RefNo { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public long? SummaryId_ { get; set; }

        public string SummaryTransNo_ { get; set; }

        public List<PurchaseOrderScan_ItemModel> ListDetails_ = new List<PurchaseOrderScan_ItemModel>();

        public PurchaseOrderScan_Item Details_ { get; set; }
    }

    public class PurchaseOrderScan_Item
    {
        public List<long> deletedRowKeys { get; set; }
        public List<PurchaseOrderScan_ItemModel> insertedRowValues { get; set; }
        public List<PurchaseOrderScan_ItemModel> modifiedRowValues { get; set; }
    }

    public class PurchaseOrderScan_ItemModel
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

        public string Bagian { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOpen { get; set; }

        public decimal? QuantityScan { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public long? DocEntry { get; set; }

        public long? LineNum { get; set; }

        public string LineStatus { get; set; }

        public int? IdPDO { get; set; }

        public string PDODocNum_ { get; set; }
    }

    public class PurchaseOrderScanItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<PurchaseOrderScanItemTagModel> PurchaseOrderScan_Item_TagModel___ { get; set; }
    }

    public class PurchaseOrderScanItemTagModel
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

    #endregion

    #region Services

    public class PurchaseOrderScanService
    {

        public PurchaseOrderScanModel GetNewModel(int userId)
        {
            PurchaseOrderScanModel model = new PurchaseOrderScanModel();
            model.Status = "Draft";
            return model;
        }
        public PurchaseOrderScanModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public PurchaseOrderScanModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            PurchaseOrderScanModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_PurchaseOrder"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<PurchaseOrderScanModel>(ssql, id).Single();

                model.ListDetails_ = this.PurchaseOrder_Items(CONTEXT, id);

                if (model.Status != "Draft")
                {
                    string sSummaryId = @"
                        SELECT TOP 1 T1.""Id""
                        FROM ""Tx_GoodsReceiptPO_Ref"" T0
                        INNER JOIN ""Tx_GoodsReceiptPO"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                        ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryId_ = CONTEXT.Database.SqlQuery<long?>(sSummaryId, id).SingleOrDefault();

                    string sSummaryNo = @"
                        SELECT TOP 1 T1.""TransNo""
                        FROM ""Tx_GoodsReceiptPO_Ref"" T0
                        INNER JOIN ""Tx_GoodsReceiptPO"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                        ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryTransNo_ = CONTEXT.Database.SqlQuery<string>(sSummaryNo, id).SingleOrDefault();

                }

            }

            return model;
        }

        public List<PurchaseOrderScan_ItemModel> PurchaseOrder_Items(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return PurchaseOrder_Items(CONTEXT, id);
            }

        }

        public List<PurchaseOrderScan_ItemModel> PurchaseOrder_Items(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""DocNum"" AS ""PDODocNum_""
                FROM ""Tx_PurchaseOrder_Item"" T0
                LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OWOR"" T1 ON T0.""IdPDO"" = T1.""DocEntry""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var purchaseOrder = CONTEXT.Database.SqlQuery<PurchaseOrderScan_ItemModel>(ssql, id).ToList();
            return purchaseOrder;
        }

        public PurchaseOrderScanModel NavFirst(int userId)
        {
            PurchaseOrderScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "PurchaseOrderScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_PurchaseOrder\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public PurchaseOrderScanModel NavPrevious(int userId, long id = 0)
        {
            PurchaseOrderScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "PurchaseOrderScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_PurchaseOrder\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public PurchaseOrderScanModel NavNext(int userId, long id = 0)
        {
            PurchaseOrderScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "PurchaseOrderScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_PurchaseOrder\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public PurchaseOrderScanModel NavLast(int userId)
        {
            PurchaseOrderScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "PurchaseOrderScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_PurchaseOrder\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(PurchaseOrderScanModel model)
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

                            Tx_PurchaseOrder Tx_PurchaseOrder = new Tx_PurchaseOrder();
                            CopyProperty.CopyProperties(model, Tx_PurchaseOrder, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_PurchaseOrder.TransType = "PurchaseOrder";
                            Tx_PurchaseOrder.CreatedDate = dtModified;
                            Tx_PurchaseOrder.CreatedUser = model._UserId;
                            Tx_PurchaseOrder.ModifiedDate = dtModified;
                            Tx_PurchaseOrder.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'PurchaseOrder','" + dateX + "','') ").SingleOrDefault();
                            Tx_PurchaseOrder.TransNo = transNo;

                            CONTEXT.Tx_PurchaseOrder.Add(Tx_PurchaseOrder);
                            CONTEXT.SaveChanges();
                            Id = Tx_PurchaseOrder.Id;

                            String keyValue;
                            keyValue = Tx_PurchaseOrder.Id.ToString();

                            CONTEXT.Database.ExecuteSqlCommand(
                            "CALL \"SpGoodsReceiptPO_AddItemDetail\" ({0}, {1}, {2})",
                            model._UserId,
                            keyValue
                        );

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_PurchaseOrder", "add", "Id", keyValue);


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

        public void Update(PurchaseOrderScanModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "PurchaseOrder", CONTEXT, "before", "PurchaseOrder", "update", "Id", keyValue);


                                Tx_PurchaseOrder Tx_PurchaseOrder = CONTEXT.Tx_PurchaseOrder.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_PurchaseOrder.ModifiedDate = dtModified;
                                Tx_PurchaseOrder.ModifiedUser = model._UserId;

                                if (Tx_PurchaseOrder != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_PurchaseOrder, false, exceptColumns);
                                    Tx_PurchaseOrder.ModifiedDate = dtModified;
                                    Tx_PurchaseOrder.ModifiedUser = model._UserId;

                                    //if (model.StartDate != null)
                                    //{
                                    //    Tx_PurchaseOrder.Status2 = "On Progress";
                                    //}
                                    //else
                                    //{
                                    //    Tx_PurchaseOrder.Status2 = "Open";
                                    //}

                                    //if (model.EndDate != null)
                                    //{
                                    //    Tx_PurchaseOrder.Status2 = "Close";
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
                                    //            PurchaseOrder_DetailModel detailModel = new PurchaseOrder_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "PurchaseOrder", CONTEXT, "after", "PurchaseOrder", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, PurchaseOrder_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_PurchaseOrder_Item Tx_PurchaseOrder_Item = new Tx_PurchaseOrder_Item();

        //        CopyProperty.CopyProperties(model, Tx_PurchaseOrder_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_PurchaseOrder_Item.Id = Id;
        //        Tx_PurchaseOrder_Item.CreatedDate = dtModified;
        //        Tx_PurchaseOrder_Item.CreatedUser = UserId;
        //        Tx_PurchaseOrder_Item.ModifiedDate = dtModified;
        //        Tx_PurchaseOrder_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_PurchaseOrder_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_PurchaseOrder_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_PurchaseOrder_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_PurchaseOrder_Item.Add(Tx_PurchaseOrder_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_PurchaseOrder_Item.DetId;

        //    }

        //    return DetId;

        //}

        //public void Detail_Update(HANA_APP CONTEXT, PurchaseOrder_DetailModel model, int UserId)
        //{
        //    if (model != null)
        //    {

        //        Tx_PurchaseOrder_Item Tx_PurchaseOrder_Item = CONTEXT.Tx_PurchaseOrder_Item.Find(model.DetId);

        //        if (Tx_PurchaseOrder_Item != null)
        //        {
        //            var exceptColumns = new string[] { "DetId", "Id" };
        //            CopyProperty.CopyProperties(model, Tx_PurchaseOrder_Item, false, exceptColumns);


        //            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //            Tx_PurchaseOrder_Item.ModifiedDate = dtModified;
        //            Tx_PurchaseOrder_Item.ModifiedUser = UserId;
        //            if (model.StartDate != null && model.EndDate == null)
        //            {
        //                Tx_PurchaseOrder_Item.Status = "On Progress";
        //            }
        //            else if (model.StartDate != null && model.EndDate != null)
        //            {
        //                Tx_PurchaseOrder_Item.Status = "Close";
        //            }
        //            else
        //            {
        //                Tx_PurchaseOrder_Item.Status = "Open";
        //            }
        //            CONTEXT.SaveChanges();

        //        }


        //    }

        //}

        //public void Detail_Delete(HANA_APP CONTEXT, PurchaseOrder_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_PurchaseOrder_Item\"  WHERE \"DetId\"=:p0", model.DetId);

        //            CONTEXT.SaveChanges();


        //        }
        //    }

        //}

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

                        Tx_PurchaseOrder tx_PurchaseOrder = CONTEXT.Tx_PurchaseOrder.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "PurchaseOrder", CONTEXT, "before", "Tx_PurchaseOrder", "post", "Id", keyValue);

                        if (tx_PurchaseOrder != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_PurchaseOrder.Status = "Posted";
                            tx_PurchaseOrder.IsAfterPosted = "Y";
                            tx_PurchaseOrder.ModifiedDate = dtModified;
                            tx_PurchaseOrder.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "PurchaseOrder", CONTEXT, "after", "Tx_PurchaseOrder", "post", "Id", keyValue);


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

                        Tx_PurchaseOrder tx_PurchaseOrder = CONTEXT.Tx_PurchaseOrder.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "PurchaseOrder", CONTEXT, "before", "Tx_PurchaseOrder", "cancel", "Id", keyValue);
                        if (tx_PurchaseOrder != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_PurchaseOrder.Status = "Cancel";
                            tx_PurchaseOrder.CancelReason = cancelReason;
                            tx_PurchaseOrder.ModifiedDate = dtModified;
                            tx_PurchaseOrder.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "PurchaseOrder", CONTEXT, "after", "Tx_PurchaseOrder", "cancel", "Id", keyValue);


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


        public PurchaseOrderScanItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            PurchaseOrderScanItemTagView___ model = new PurchaseOrderScanItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_PurchaseOrder_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<PurchaseOrderScanItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_PurchaseOrder_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.PurchaseOrderScan_Item_TagModel___ = CONTEXT.Database.SqlQuery<PurchaseOrderScanItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

    }


    #endregion

}