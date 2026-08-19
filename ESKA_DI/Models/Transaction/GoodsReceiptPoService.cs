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

        [Required(ErrorMessage = "required")]
        public string VendorCode { get; set; }

        [Required(ErrorMessage = "required")]

        public string VendorName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; } //DocEntry Grpo Dari SAP

        public string DocNum { get; set; } //DocNum Grpo Dari SAP

        public DateTime? DocDate { get; set; } //DocDate Grpo Dari SAP

        public long? BaseEntry { get; set; } //DocEntry PO Dari SAP

        public string BaseDocNum { get; set; } //DocNum PO Dari SAP

        public DateTime? PostingDate { get; set; } //Adalah Date Saat GRPO masuk ke SAP

        public string IsAfterPosted { get; set; } //NULL / Y

        public string CancelReason { get; set; }

        public string RefNo { get; set; }

        public string Status { get; set; } //Draft/Posted/Cancel

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
        public List<GoodsReceiptPoItem> insertedRowValues { get; set; }
        public List<GoodsReceiptPoItem> modifiedRowValues { get; set; }
    }

    public class GoodsReceiptPoItem
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }
        public int? RowNo { get; set; }

        public int _UserId { get; set; }

        public long DetId { get; set; }

        public long? Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public int? Quantity { get; set; }

        public int? QuantityCreated { get; set; }

        public string Uom { get; set; }

        public string UomEntry { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Whse { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? Netto { get; set; }

        public string Department { get; set; }

        public int? LineNum { get; set; }

        public string LineStatus { get; set; }

        public long? BaseEntry { get; set; } //DocEntry PO Dari SAP

        public int? BaseLine { get; set; } //DocNum PO Dari SAP

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public List<GoodsReceiptPoItemBatch> ListItemBatch_ { get; set; } = new List<GoodsReceiptPoItemBatch>();

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

    public class GoodsReceiptPoBatchView___
    {
        public long Id { get; set; }

        public string BaseDocNum { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string Whse { get; set; }

        public int? TotalNeeded { get; set; }

        public int? TotalCreated { get; set; }

        public List<GoodsReceiptPoBatchModel> GoodsReceiptPoBatchModel___ { get; set; }
    }

    public class GoodsReceiptPoBatchModel
    {
        public int? RowNo { get; set; }

        public int _UserId { get; set; }

        public long? DetId { get; set; }

        public long? DetDetId { get; set; }

        public string Batch { get; set; }

        public int? Quantity { get; set; }

        public DateTime? AdmissionDate { get; set; }

        public decimal? Netto { get; set; }

        public long? LineNum { get; set; }

        public string LineStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class GoodsReceiptPoScaleView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public long DetDetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string Uom { get; set; }

        public int? LineNum { get; set; }

        // Nomor batch (kolom Batch) untuk header popup Scale.
        public string Batch { get; set; }

        // LineNum batch dalam bentuk string — nested TextBox tidak me-render numerik.
        public string LineNumText { get; set; }

        // Id batch dalam bentuk string (tidak dipakai lagi di form; disimpan untuk kompatibilitas).
        public string DetDetIdText { get; set; }

        public List<GoodsReceiptPoScaleModel> GoodsReceiptPoScaleModel___ { get; set; }
    }

    public class GoodsReceiptPoScaleModel
    {
        public int? RowNo { get; set; }

        public int _UserId { get; set; }

        public long? DetDetId { get; set; }

        public long? DetDetDetId { get; set; }

        public int? Quantity { get; set; }

        public string Uom { get; set; }

        public decimal? Netto { get; set; }

        public int? LineNum { get; set; }

        public string LineStatus { get; set; }

        // Status permintaan timbang dari Tp_ScaleStaging (Waiting/Processed/Error). Null bila belum pernah diklik.
        public string StagingStatus { get; set; }

        // RequestId permintaan timbang dari Tp_ScaleStaging (baris staging terkini).
        public string StagingRequestId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class GRPOAddResultModel
    {
        public long? DocEntry { get; set; }
        public long? DocNum { get; set; }
        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    public class GoodsReceiptResultModel
    {
        public long DetId { get; set; }

        public long? GoodsReceiptId { get; set; }
    }

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

                model.ListDetails_ = this.GoodsReceiptPo_Details(CONTEXT, id);

                model.ListDetails_ = this.GoodsReceiptPo_BatchDetails(CONTEXT, model.ListDetails_);


                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'GoodsReceiptPo', :p1) ", userId, model.Id).FirstOrDefault();
                }
            }

            return model;
        }

        public List<GoodsReceiptPoItem> GoodsReceiptPo_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GoodsReceiptPo_Details(CONTEXT, id);
            }

        }

        public List<GoodsReceiptPoItem> GoodsReceiptPo_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetId"" ASC) AS ""RowNo"", T0.*
                FROM ""Tx_GoodsReceiptPO_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var GoodsReceiptPo = CONTEXT.Database.SqlQuery<GoodsReceiptPoItem>(ssql, id).ToList();
            return GoodsReceiptPo;
        }

        public List<GoodsReceiptPoItem> GoodsReceiptPo_BatchDetails(HANA_APP CONTEXT,List<GoodsReceiptPoItem> items)
        {
            if (items == null || !items.Any())
                return items;

            var detIds = items
                .Select(x => x.DetId)
                .Distinct()
                .ToList();

            string ssql = $@"
                            SELECT T0.*
                            FROM ""Tx_GoodsReceiptPO_Item_Batch"" T0
                            WHERE T0.""DetId"" IN ({string.Join(",", detIds)})
                            ORDER BY T0.""DetDetId"" ASC
                        ";

            var batches = CONTEXT.Database
                .SqlQuery<GoodsReceiptPoItemBatch>(ssql)
                .ToList();

            foreach (var item in items)
            {
                item.ListItemBatch_ = batches
                    .Where(x => x.DetId == item.DetId)
                    .ToList();
            }

            return items;
        }

        //    public List<GoodsReceiptPoItem> GoodsReceiptPo_BatchDetails(
        //        HANA_APP CONTEXT,
        //        List<GoodsReceiptPoItem> items)
        //    {
        //        if (!items.Any())
        //            return items;

        //        string ssql = @"
        //    SELECT T0.*
        //    FROM ""Tx_GoodsReceiptPO_Item_Batch"" T0 WHERE detdetid = 
        //";

        //        var batches = CONTEXT.Database
        //            .SqlQuery<GoodsReceiptPoItemBatch>(ssql)
        //            .ToList();

        //        foreach (var item in items)
        //        {
        //            item.ListItemBatch_ = batches
        //                .Where(x => x.DetId == item.DetId)
        //                .ToList();
        //        }

        //        return items;
        //    }

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

                            //SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "after", "GoodsReceiptPo", "add", "Id", keyValue);
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_AddItemDetail\"(:p0,:p1,'Add')", model._UserId, Id);

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

                                Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                if (tx_GoodsReceiptPO != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };

                                    foreach (var sourceProp in model.GetType().GetProperties())
                                    {
                                        if (exceptColumns.Contains(sourceProp.Name))
                                            continue;

                                        var value = sourceProp.GetValue(model, null);

                                        // Skip jika nilai baru NULL
                                        if (value == null)
                                            continue;

                                        var targetProp = tx_GoodsReceiptPO.GetType().GetProperty(sourceProp.Name);

                                        if (targetProp != null && targetProp.CanWrite)
                                        {
                                            targetProp.SetValue(tx_GoodsReceiptPO, value, null);
                                        }
                                    }

                                    //Tx_StockOpname.ApprovalStatus = isApprovalActive == "Y" && Tx_StockOpname.ApprovalStatus == "" ? "Waiting" : "Approved";
                                    tx_GoodsReceiptPO.ModifiedDate = dtModified;
                                    tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                                    if (method == "Post")
                                    {
                                        //CONTEXT.Database.ExecuteSqlCommand("CALL \"GoodsReceiptPo_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
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
                                                GoodsReceiptPoItem detailModel = new GoodsReceiptPoItem();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
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

                                if (ex.Message.StartsWith("[VALIDATION]"))
                                {
                                    errorMassage = ex.Message;
                                }
                                else
                                {
                                    errorMassage = string.Format("[VALIDATION] {0}", ex.Message);
                                }

                                throw new Exception(errorMassage);
                            }
                        }
                    }
                }
            }
        }

        //public void Update(GoodsReceiptPoModel model, string method = "")
        //{
        //    if (model != null)
        //    {
        //        if (model != null)
        //        {
        //            using (var CONTEXT = new HANA_APP())
        //            {
        //                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
        //                {
        //                    try
        //                    {
        //                        String keyValue;
        //                        keyValue = model.Id.ToString();

        //                        SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "before", "GoodsReceiptPo", "update", "Id", keyValue);

        //                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(model.Id);
        //                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //                        if (tx_GoodsReceiptPO != null)
        //                        {
        //                            var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
        //                            CopyProperty.CopyProperties(model, tx_GoodsReceiptPO, false, exceptColumns);

        //                            //Tx_StockOpname.ApprovalStatus = isApprovalActive == "Y" && Tx_StockOpname.ApprovalStatus == "" ? "Waiting" : "Approved";
        //                            tx_GoodsReceiptPO.ModifiedDate = dtModified;
        //                            tx_GoodsReceiptPO.ModifiedUser = model._UserId;

        //                            if (method == "Post")
        //                            {
        //                                //CONTEXT.Database.ExecuteSqlCommand("CALL \"GoodsReceiptPo_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
        //                            }

        //                            if (model.Details_ != null)
        //                            {
        //                                if (model.Details_.insertedRowValues != null)
        //                                {
        //                                    foreach (var detail in model.Details_.insertedRowValues)
        //                                    {
        //                                        Detail_Add(CONTEXT, detail, model.Id, model._UserId);
        //                                    }
        //                                }

        //                                if (model.Details_.modifiedRowValues != null)
        //                                {
        //                                    foreach (var detail in model.Details_.modifiedRowValues)
        //                                    {
        //                                        Detail_Update(CONTEXT, detail, model._UserId);
        //                                    }
        //                                }

        //                                if (model.Details_.deletedRowKeys != null)
        //                                {
        //                                    foreach (var detId in model.Details_.deletedRowKeys)
        //                                    {
        //                                        GoodsReceiptPoItem detailModel = new GoodsReceiptPoItem();
        //                                        detailModel.DetId = detId;
        //                                        Detail_Delete(CONTEXT, detailModel);
        //                                    }
        //                                }
        //                            }

        //                           CONTEXT.SaveChanges();

        //                            SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "after", "GoodsReceiptPo", "update", "Id", keyValue);

        //                        }

        //                        CONTEXT_TRANS.Commit();
        //                    }

        //                    catch (Exception ex)
        //                    {
        //                        CONTEXT_TRANS.Rollback();

        //                        string errorMassage;
        //                        if (ex.Message.Substring(12) == "[VALIDATION]")
        //                        {
        //                            errorMassage = ex.Message;
        //                        }
        //                        else
        //                        {
        //                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
        //                        }

        //                        throw new Exception(errorMassage);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}

        // Wrapper publik: buka context + transaksi sendiri, dipakai action ChooseItem (tambah 1 item ke detail GRPO)
        public long Detail_Add(long Id, GoodsReceiptPoItem model, int UserId)
        {
            long detId = 0;

            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    if (model.Netto == null) model.Netto = 0; // kolom Netto Required / NOT NULL

                    if (string.IsNullOrEmpty(model.LineStatus)) model.LineStatus = "O"; // default Open

                    // LineNum lanjutkan dari baris yang sudah ada di GRPO ini (max + 1, mulai 0)
                    if (model.LineNum == null)
                    {
                        int? maxLine = CONTEXT.Tx_GoodsReceiptPO_Item
                            .Where(x => x.Id == Id)
                            .Select(x => x.LineNum)
                            .Max();
                        model.LineNum = (maxLine ?? -1) + 1;
                    }

                    // UomEntry di model bertipe string, tetapi di tabel (entity) bertipe int?.
                    // Detail_Add memakai CopyProperties (reflection SetValue tanpa konversi), sehingga
                    // string non-null ke int? akan error. Kosongkan dulu, lalu set setelah insert.
                    string uomEntryStr = model.UomEntry;
                    model.UomEntry = null;

                    detId = Detail_Add(CONTEXT, model, Id, UserId);

                    int uomEntryInt;
                    if (int.TryParse(uomEntryStr, out uomEntryInt))
                    {
                        var ent = CONTEXT.Tx_GoodsReceiptPO_Item.Find(detId);
                        if (ent != null)
                        {
                            ent.UomEntry = uomEntryInt;
                            CONTEXT.SaveChanges();
                        }
                    }

                    CONTEXT_TRANS.Commit();
                }
                catch (Exception)
                {
                    CONTEXT_TRANS.Rollback();
                    throw;
                }
            }

            return detId;
        }

        public long Detail_Add(HANA_APP CONTEXT, GoodsReceiptPoItem model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = new Tx_GoodsReceiptPO_Item();

                CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_GoodsReceiptPO_Item.Id = Id;
                Tx_GoodsReceiptPO_Item.CreatedDate = dtModified;
                Tx_GoodsReceiptPO_Item.CreatedUser = UserId;
                Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
                Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;

                CONTEXT.Tx_GoodsReceiptPO_Item.Add(Tx_GoodsReceiptPO_Item);
                CONTEXT.SaveChanges();
                DetId = Tx_GoodsReceiptPO_Item.DetId;

            }

            return DetId;

        }

        public void Detail_Delete(HANA_APP CONTEXT, GoodsReceiptPoItem model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {
                    long detId = model.DetId;

                    // Hapus berjenjang (relasi): scale -> batch -> item, agar tidak ada baris orphan.
                    // Ambil semua DetDetId batch milik item ini, lalu hapus scale per batch (pola sama dgn GoodsReceiptPo_DeleteItemBatch).
                    var detDetIds = CONTEXT.Database.SqlQuery<long>("SELECT \"DetDetId\" FROM \"Tx_GoodsReceiptPO_Item_Batch\" WHERE \"DetId\"=:p0", detId).ToList();

                    foreach (var detDetId in detDetIds)
                    {
                        CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", detDetId);
                    }

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetId\"=:p0 AND \"TransType\"='GoodsReceiptPo'", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item_Batch\"  WHERE \"DetId\"=:p0", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item\"  WHERE \"DetId\"=:p0", detId);
                    CONTEXT.SaveChanges();


                }
            }

        }

        // Wrapper publik: buka context + transaksi sendiri, dipakai action DeleteItem (hapus 1 baris detail GRPO)
        public void Detail_Delete(long DetId)
        {
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    Detail_Delete(CONTEXT, new GoodsReceiptPoItem { DetId = DetId });
                    CONTEXT_TRANS.Commit();
                }
                catch (Exception)
                {
                    CONTEXT_TRANS.Rollback();
                    throw;
                }
            }
        }

        // Ambil status header GRPO (dipakai grid detail untuk menampilkan/menyembunyikan tombol Delete)
        public string GetStatus(long id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CONTEXT.Database.SqlQuery<string>("SELECT \"Status\" FROM \"Tx_GoodsReceiptPO\" WHERE \"Id\"=:p0", id).FirstOrDefault();
            }
        }

        public void Detail_Update(HANA_APP CONTEXT, GoodsReceiptPoItem model, int UserId)
        {
            if (model != null)
            {

                Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = CONTEXT.Tx_GoodsReceiptPO_Item.Find(model.DetId);

                if (Tx_GoodsReceiptPO_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
                    Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;
                    //CONTEXT.SaveChanges();
                }


            }

        }

        public GoodsReceiptPoBatchView___ GetBatch(long id, long detId)
        {
            string sql = null;
            GoodsReceiptPoBatchView___ model = new GoodsReceiptPoBatchView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", 
                                T0.""BaseDocNum"",
                                T1.""DetId"", 
                                T1.""ItemCode"", 
                                T1.""ItemName"",
                                T1.""Whse"",
                                T1.""Quantity"" AS ""TotalNeeded"",
                                T1.""QuantityCreated"" AS ""TotalCreated""
                                FROM ""Tx_GoodsReceiptPO"" T0   
                                LEFT JOIN ""Tx_GoodsReceiptPO_Item"" T1 ON T0.""Id"" = T1.""Id"" 
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPoBatchView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_GoodsReceiptPO_Item_Batch"" T0   
                            WHERE ""DetId"" = :p0 ";

                model.GoodsReceiptPoBatchModel___ = CONTEXT.Database.SqlQuery<GoodsReceiptPoBatchModel>(sql, detId).ToList();
            }

            return model;
        }

       

        public void Post(int userId, GoodsReceiptPoModel GoodsReceiptPoModel)
        {
            try
            {
                //Update(GoodsReceiptPoModel, "Post");
                PostSAP(userId, GoodsReceiptPoModel.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Susun pesan [VALIDATION] yang panjangnya dijamin aman.
        //
        // BaseController (file template, tidak diubah) menaruh pesan exception ke
        // HttpResponse.Status. Reason phrase HTTP dibatasi 512 karakter; pesan yang lebih
        // panjang membuat setter-nya melempar ArgumentOutOfRangeException, sehingga pesan
        // validasi asli HILANG dan user hanya melihat "Specified argument was out of the
        // range of valid values" -- error yang sama sekali tidak menjelaskan masalahnya.
        //
        // Dokumen ber-item banyak mudah melewati batas itu karena detailnya satu baris per
        // item/batch bermasalah. Karena itu detail dibatasi MAX_DETAIL baris dan sisanya
        // diringkas jadi jumlah saja. Kembar dengan ReProcessService.BuildValidationMessage.
        private static string BuildValidationMessage(string header, List<string> details)
        {
            const int MAX_DETAIL = 5;
            const int SAFE_MAX = 460; // di bawah 512, sisakan ruang untuk prefix "500 "

            List<string> shown = details.Take(MAX_DETAIL).ToList();
            string msg = "[VALIDATION] - " + header + ":\n" + string.Join("\n", shown);

            if (details.Count > shown.Count)
            {
                msg += string.Format("\n...dan {0} lainnya.", details.Count - shown.Count);
            }

            // Jaring pengaman: ItemCode/Batch yang panjang bisa tetap melewati batas.
            if (msg.Length > SAFE_MAX)
            {
                msg = msg.Substring(0, SAFE_MAX) + "...";
            }

            return msg;
        }

        //public void PostSAP(int userId, long id)
        //{
        //    SAPbobsCOM.Company oCompany = null;
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                oCompany = SAPCachedCompany.GetCompany();

        //                String keyValue;
        //                keyValue = id.ToString();

        //                GoodsReceiptPoModel syncGoodsReceiptPo = GetById(userId, id, "Post");

        //                SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

        //                Tx_GoodsReceiptPO Tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
        //                if (Tx_GoodsReceiptPO != null)
        //                {

        //                    int docEntry_ = AddInventoryPosting(oCompany, userId, id, syncGoodsReceiptPo);
        //                    if (docEntry_ <= 0)
        //                    {
        //                        throw new Exception($"[VALIDATION] - No inventory posting created");
        //                    }
        //                    string ssql = @"SELECT ""DocNum"" 
        //                                FROM """ + DbProvider.dbSap_Name + @""".""OIQR"" T0
        //                                WHERE T0.""DocEntry"" = " + docEntry_ + @" 
        //                                ";

        //                    string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();

        //                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //                    // Tx_GoodsReceiptPO.PostingDate = dtModified;
        //                    Tx_GoodsReceiptPO.DocEntry = Convert.ToInt32(docEntry_);
        //                    Tx_GoodsReceiptPO.DocNum = docNum;
        //                    //Tx_GoodsReceiptPO.PostingDate = dtModified;

        //                    Tx_GoodsReceiptPO.Status = "Posted";
        //                    // Tx_GoodsReceiptPO.IsAfterPosted = "Y";
        //                    Tx_GoodsReceiptPO.ModifiedDate = dtModified;
        //                    Tx_GoodsReceiptPO.ModifiedUser = userId;

        //                    CONTEXT.SaveChanges();

        //                    SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

        //                    if (oCompany.InTransaction)
        //                    {
        //                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
        //                    }

        //                    CONTEXT_TRANS.Commit();
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                if (oCompany.InTransaction)
        //                {
        //                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
        //                }

        //                CONTEXT_TRANS.Rollback();

        //                string errorMassage;
        //                if (ex.Message.Substring(12) == "[VALIDATION]")
        //                {
        //                    errorMassage = ex.Message;
        //                }
        //                else
        //                {
        //                    errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
        //                }

        //                throw new Exception(errorMassage);
        //            }
        //            finally
        //            {
        //                SAPCachedCompany.Release(oCompany);
        //            }
        //        }
        //    }

        //}

        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;
            GoodsReceiptPoModel syncGRPO = GetById(userId, id);

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


                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
                        if (tx_GoodsReceiptPO == null)
                        {
                            throw new Exception($"[VALIDATION] - GRPO Data not found");
                        }

                        if (syncGRPO.ListDetails_.All(q => q.Quantity == 0))
                        {
                            throw new Exception($"[VALIDATION] - No record created");
                        }

                        // Validasi: Total Created (QuantityCreated) harus SAMA dengan Total Needed (Quantity) tiap item.
                        // Kurang -> tampilkan kekurangannya; lebih -> tidak boleh.
                        var qtyErrors = new List<string>();
                        foreach (var item in syncGRPO.ListDetails_.Where(x => (x.Quantity ?? 0) > 0))
                        {
                            int needed = item.Quantity ?? 0;
                            int created = item.QuantityCreated ?? 0;

                            if (created < needed)
                                qtyErrors.Add(string.Format("{0}: Total Created {1} < Total Needed {2} (kurang {3})", item.ItemCode, created, needed, needed - created));
                            else if (created > needed)
                                qtyErrors.Add(string.Format("{0}: Total Created {1} > Total Needed {2} (lebih {3}, tidak boleh)", item.ItemCode, created, needed, created - needed));
                        }
                        if (qtyErrors.Any())
                        {
                            throw new Exception(BuildValidationMessage("Total Created harus sama dengan Total Needed", qtyErrors));
                        }

                        // Validasi Scale/Netto: tiap batch yang dikirim ke SAP harus SUDAH punya baris scale
                        // dan tidak sedang menunggu hasil timbang (staging Status='Waiting').
                        var scaleErrors = new List<string>();
                        if (syncGRPO.ListDetails_ != null)
                        {
                            foreach (var item in syncGRPO.ListDetails_.Where(x => (x.Quantity ?? 0) > 0))
                            {
                                if (item.ListItemBatch_ == null) continue;
                                foreach (var b in item.ListItemBatch_)
                                {
                                    long detDetId = b.DetDetId;
                                    if (detDetId == 0) continue;
                                    long scaleCount = CONTEXT.Database.SqlQuery<long>(
                                        "SELECT COUNT(*) AS IDU FROM \"Tx_GoodsReceiptPO_Item_Batch_Scale\" WHERE \"DetDetId\"=:p0", detDetId).FirstOrDefault();
                                    if (scaleCount == 0)
                                    {
                                        // Teks per baris dijaga pendek; instruksinya ada di header pesan
                                        // (lihat BuildValidationMessage) agar tidak berulang tiap baris.
                                        scaleErrors.Add(string.Format("Batch {0}: belum ada data Scale", b.Batch));
                                        continue;
                                    }
                                    // WAJIB difilter TransType: "DetDetId" adalah identity per-tabel batch, dan
                                    // GRPO/Issue/Receipt menulis ke satu Tp_ScaleStaging -> nilainya bertabrakan
                                    // antar dokumen. Tanpa filter ini, baris Waiting milik dokumen lain ikut terhitung.
                                    // Hanya baris staging TERKINI per baris scale yang dinilai (pola MAX(StagingId)
                                    // sama dengan grid), sebab SaveFromScale menambah baris staging baru saat
                                    // percobaan sebelumnya berstatus Error.
                                    int waitingCount = CONTEXT.Database.SqlQuery<int>(
                                        "SELECT COUNT(*) AS IDU " +
                                        "FROM \"Tx_GoodsReceiptPO_Item_Batch_Scale\" SC " +
                                        "INNER JOIN \"Tp_ScaleStaging\" ST ON ST.\"DetDetDetId\" = SC.\"DetDetDetId\" AND ST.\"TransType\" = 'GoodsReceiptPo' " +
                                        "WHERE SC.\"DetDetId\" = :p0 AND ST.\"Status\" = 'Waiting' " +
                                        "  AND ST.\"StagingId\" = (SELECT MAX(ST2.\"StagingId\") FROM \"Tp_ScaleStaging\" ST2 " +
                                        "                          WHERE ST2.\"DetDetDetId\" = SC.\"DetDetDetId\" AND ST2.\"TransType\" = 'GoodsReceiptPo')",
                                        detDetId).FirstOrDefault();
                                    if (waitingCount > 0)
                                    {
                                        scaleErrors.Add(string.Format("Batch {0}: menunggu hasil timbang", b.Batch));
                                    }
                                }
                            }
                        }
                        if (scaleErrors.Any())
                        {
                            throw new Exception(BuildValidationMessage(
                                "Data Scale belum siap untuk di-Post (tunggu hasil timbang selesai, atau hapus baris scale yang menunggu)",
                                scaleErrors));
                        }

                        GRPOAddResultModel GRPOResult = AddGoodsReceiptPO(oCompany, userId, id, syncGRPO);
                        //if (GRPOResult != null)
                        //{
                        //    //insert RFID items to pending
                        //    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_InsertItemTag\"(:p0,:p1, 'GoodsReceiptPO','A')", userId, id);
                        //}
                        //List<GoodsReceiptResultModel> GRResult = new List<GoodsReceiptResultModel>();
                        //if (syncGRPO.ListDetails_.Any(q => q.IdPDO.GetValueOrDefault() != 0 && (q.QuantityValid > 0 && q.QuantityValid.HasValue)))
                        //{
                        //    GRResult = AddReceiveFromProduction(oCompany, userId, id, syncGRPO);
                        //}

                        //string ssql = @"SELECT ""DocNum"" 
                        //    FROM """ + DbProvider.dbSap_Name + @""".""OPDN"" T0
                        //    WHERE T0.""DocEntry"" = " + GRPOResult.DocEntry + @" 
                        //";

                        //string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();


                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_GoodsReceiptPO.PostingDate = dtModified;
                        tx_GoodsReceiptPO.DocEntry = Convert.ToInt64(GRPOResult.DocEntry);
                        //tx_GoodsReceiptPO.DocEntry = Convert.ToInt64(GRPOResult.doc);
                        //tx_GoodsReceiptPO.DocNum = docNum;

                        tx_GoodsReceiptPO.Status = "Posted";
                        tx_GoodsReceiptPO.IsAfterPosted = "Y";
                        tx_GoodsReceiptPO.ModifiedDate = dtModified;
                        tx_GoodsReceiptPO.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                        //var caseStatements = string.Join(" ",
                        //    GRPOResult.LineMapping.Select(kv => $"WHEN T0.\"DetId\" = {kv.Key} THEN {kv.Value}"));

                        //string sqlLine = $@"
                        //    UPDATE ""Tx_GoodsReceiptPO_Item"" T0 
                        //    SET ""LineNum"" = CASE {caseStatements} END,
                        //        ""DocEntry"" = {GRPOResult.DocEntry}
                        //    ";
                        //var whereIn = string.Join(", ", GRPOResult.LineMapping.Keys);

                        // Update Header
                    string sqlHeader = $@"
                        UPDATE ""Tx_GoodsReceiptPO""
                        SET ""DocEntry"" = {GRPOResult.DocEntry},
                        ""DocNum""   = {GRPOResult.DocNum}
                        WHERE ""Id"" = {id}";

                        CONTEXT.Database.ExecuteSqlCommand(sqlHeader);

                        // Update Detail
                        var caseStatements = string.Join(" ",
                            GRPOResult.LineMapping.Select(x =>
                                $"WHEN T0.\"DetId\" = {x.Key} THEN {x.Value}"));

                        var whereIn = string.Join(", ",
                            GRPOResult.LineMapping.Keys);

                        string sqlLine = $@"
                            UPDATE ""Tx_GoodsReceiptPO_Item"" T0
                            SET ""LineNum"" = CASE
                                {caseStatements}
                            END
                            WHERE T0.""DetId"" IN ({whereIn})
                        ";

                        CONTEXT.Database.ExecuteSqlCommand(sqlLine);



                        //if (GRResult.Count > 0)
                        //{
                        //    var GRStatements = string.Join(" ",
                        //        GRResult.Select(kv => $"WHEN T0.\"DetId\" = {kv.DetId} THEN {kv.GoodsReceiptId}"));
                        //    sqlLine += ",";
                        //    sqlLine += @"
                        //            ""GoodsReceiptDocEntry"" = CASE " + GRStatements + " END ";
                        //    whereIn = string.Join(", ", GRPOResult.LineMapping.Keys.Union(GRResult.Select(x => x.DetId)).ToList());
                        //}

                        //sqlLine += @" WHERE T0.""DetId"" IN (" + whereIn + ")";

                        //CONTEXT.Database.ExecuteSqlCommand(sqlLine);


                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", "post", "Id", keyValue);
                        //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_UpdatePOStatus\"(:p0,:p1,'post')", userId, id);

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

        private GRPOAddResultModel AddGoodsReceiptPO(Company oCompany, int userId, long id, GoodsReceiptPoModel model)
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

            //if (model.Comments != null)
            //{
            //    oDocument.Comments = model.Comments;
            //}

            if (model.Address != null)
            {
                oDocument.Address = model.Address;
            }

            //oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            //oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;

            int i = 0;
            Dictionary<long, int> InsertedLine = new Dictionary<long, int>();
            foreach (var item in model.ListDetails_.Where(x => x.Quantity > 0))
            {
                oDocument.Lines.BaseType = 22;
                oDocument.Lines.BaseEntry = Convert.ToInt32(item.BaseEntry);
                oDocument.Lines.BaseLine = Convert.ToInt32(item.BaseLine);
                oDocument.Lines.Quantity = (double)item.Quantity;

                if (item.UomEntry != null)
                {
                    oDocument.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                }

                // Isi Batch
                if (item.ListItemBatch_ != null && item.ListItemBatch_.Any())
                {
                    int batchIndex = 0;

                    foreach (var batch in item.ListItemBatch_)
                    {
                        if (batchIndex > 0)
                        {
                            oDocument.Lines.BatchNumbers.Add();
                        }

                        oDocument.Lines.BatchNumbers.BatchNumber = batch.Batch;
                        oDocument.Lines.BatchNumbers.Quantity = (double)batch.Quantity;

                        batchIndex++;
                    }
                }

                oDocument.Lines.Add();

                InsertedLine.Add(Convert.ToInt64(item.DetId), i);
                i++;
            }

            int docAdd = oDocument.Add();
            if (docAdd != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Add Goods Receipt PO : " + nErr.ToString() + "|" + errMsg);
            }

            int docEntry = Convert.ToInt32(oCompany.GetNewObjectKey());
            Documents oGRPO = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

            if (oGRPO.GetByKey(docEntry))
            {
                result.DocEntry = docEntry;
                result.DocNum = oGRPO.DocNum;
            }

            result.LineMapping = InsertedLine;

            SapCompany.CleanUp(oDocument);
            return result;
        }

        public static void CancelSAP(SAPbobsCOM.Company oCompany, int docEntry)
        {
            SAPbobsCOM.Documents oGRPO = null;

            try
            {
                if (!oCompany.InTransaction)
                    oCompany.StartTransaction();

                //oGRPO = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(
                //    SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                //if (!oGRPO.GetByKey(docEntry))
                //    throw new Exception($"GRPO DocEntry [{docEntry}] tidak ditemukan.");

                //if (oGRPO.Cancelled == SAPbobsCOM.BoYesNoEnum.tYES)
                //    throw new Exception($"GRPO DocEntry [{docEntry}] sudah dicancel.");

                //int ret = oGRPO.Cancel();

                //if (ret != 0)
                //{
                //    oCompany.GetLastError(out int errCode, out string errMsg);
                //    throw new Exception($"Cancel GRPO gagal : {errCode} - {errMsg}");
                //}
                oGRPO = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                if (!oGRPO.GetByKey(docEntry))
                    throw new Exception("GRPO tidak ditemukan.");
                var test = oGRPO.DocObjectCode;

                SAPbobsCOM.Documents oCancellation = (SAPbobsCOM.Documents)oGRPO.CreateCancellationDocument();

                if (oCancellation == null)
                {
                    oCompany.GetLastError(out int errCode, out string errMsg);

                    throw new Exception(
                        $"CreateCancellationDocument Error : {errCode} - {errMsg}");
                }

                int ret = oCancellation.Add();


                if (ret != 0)
                {
                    oCompany.GetLastError(out int errCode, out string errMsg);

                    throw new Exception(
                        $"Cancel GRPO gagal : {errCode} - {errMsg}");
                }

                if (oCompany.InTransaction)
                    oCompany.EndTransaction(
                        SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }
            catch (Exception)
            {
                if (oCompany.InTransaction)
                    oCompany.EndTransaction(
                        SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                throw;
            }
            finally
            {
                if (oGRPO != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oGRPO);
                    oGRPO = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
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

                       // SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);

                        Tx_GoodsReceiptPO Tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(Id);
                        if (Tx_GoodsReceiptPO != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_GoodsReceiptPO.Status = "Cancel";
                            // Tx_GoodsReceiptPO.ApprovalStatus = "Rejected";
                            Tx_GoodsReceiptPO.CancelReason = cancelReason;
                            Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            Tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();

                            // ==========================
                            // CANCEL SAP B1
                            // ==========================

                            var oCompany = SAPCachedCompany.GetCompany();

                            CancelSAP(oCompany, Convert.ToInt32(Tx_GoodsReceiptPO.DocEntry));
                        }

                        //SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);


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

        public List<GoodsReceiptPoBatchModel> GoodsReceiptPo__ItemBatchList(long detId)
        {
            string sql = null;
            List<GoodsReceiptPoBatchModel> model = new List<GoodsReceiptPoBatchModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_GoodsReceiptPO_Item_Batch"" T0   
                            WHERE ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPoBatchModel>(sql,detId).ToList();
            }
            return model;
        }

        public long GoodsReceiptPo_AddNewItemBatch(GoodsReceiptPoBatchModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_GoodsReceiptPO_Item_Batch tx_GoodsReceiptPO_Item_Batch = new Tx_GoodsReceiptPO_Item_Batch();
                        CopyProperty.CopyProperties(model, tx_GoodsReceiptPO_Item_Batch, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        // Nomor urut per item: lanjut dari LineNum terakhir pada DetId ini (mulai 1).
                        // Di-set ke entity (bukan model) karena model.LineNum bertipe long? sedangkan
                        // entity int? -- CopyProperties memakai reflection tanpa konversi tipe.
                        int? maxLineNum = CONTEXT.Database.SqlQuery<int?>("SELECT MAX(\"LineNum\") AS IDU FROM \"Tx_GoodsReceiptPO_Item_Batch\" WHERE \"DetId\"=:p0", model.DetId).FirstOrDefault();
                        tx_GoodsReceiptPO_Item_Batch.LineNum = (maxLineNum ?? 0) + 1;

                        if (string.IsNullOrEmpty(tx_GoodsReceiptPO_Item_Batch.LineStatus))
                        {
                            tx_GoodsReceiptPO_Item_Batch.LineStatus = "O";
                        }

                        tx_GoodsReceiptPO_Item_Batch.CreatedDate = dtModified;
                        tx_GoodsReceiptPO_Item_Batch.CreatedUser = model._UserId;
                        tx_GoodsReceiptPO_Item_Batch.ModifiedDate = dtModified;
                        tx_GoodsReceiptPO_Item_Batch.ModifiedUser = model._UserId;

                        CONTEXT.Tx_GoodsReceiptPO_Item_Batch.Add(tx_GoodsReceiptPO_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_GoodsReceiptPO_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_GoodsReceiptPO_Item_Batch.DetId.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_UpdateItemQuantity\"(:p0, 'Tx_GoodsReceiptPO_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                        SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "after", "GoodsReceiptPo", "addItemBatch", "Id", keyValue);

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

            return detDetId;
        }

        public void GoodsReceiptPo_UpdateItemBatch(GoodsReceiptPoBatchModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.DetId.ToString();

                        // SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "before", "GoodsReceiptPo", "updateItemBatch", "Id", keyValue);

                        Tx_GoodsReceiptPO_Item_Batch tx_GoodsReceiptPO_Item_Batch = CONTEXT.Tx_GoodsReceiptPO_Item_Batch.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_GoodsReceiptPO_Item_Batch != null)
                        {
                            // LineNum & LineStatus di-generate server-side dan tidak dikirim grid.
                            // Tanpa dikecualikan, CopyProperties akan menimpanya dengan null tiap kali edit.
                            // Netto juga: nilainya dihitung dari SUM(Netto baris scale) via
                            // UpdateBatchNettoFromScale, dan kolomnya read-only di grid (terkirim null).
                            var exceptColumns = new string[] { "DetId", "DetDetId", "LineNum", "LineStatus", "Netto", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_GoodsReceiptPO_Item_Batch, false, exceptColumns);

                            tx_GoodsReceiptPO_Item_Batch.ModifiedDate = dtModified;
                            tx_GoodsReceiptPO_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_UpdateItemQuantity\"(:p0, 'Tx_GoodsReceiptPO_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPo", CONTEXT, "after", "GoodsReceiptPo", "updateItemBatch", "Id", keyValue);

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

        public void GoodsReceiptPo_DeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetId != 0)
                    {
                        try
                        {
                            //SpNotif.SpSysControllerTransNotif(_userId, "StockOpname", CONTEXT, "before", "StockOpname", "deleteItemBatch", "Id", Id.ToString());

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetDetId\"=:p0 AND \"TransType\"='GoodsReceiptPo'", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_UpdateItemQuantity\"(:p0, 'Tx_GoodsReceiptPO_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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

        #region Scale (Timbangan)

        public GoodsReceiptPoScaleView___ GetScale(long id, long detId, long detDetId)
        {
            string sql = null;
            GoodsReceiptPoScaleView___ model = new GoodsReceiptPoScaleView___();

            using (var CONTEXT = new HANA_APP())
            {
                // "LineNum" di-COALESCE dengan nomor urut batch (berdasarkan DetDetId, sama seperti
                // penomoran saat batch dibuat), agar header tetap tampil pada data lama ber-LineNum NULL.
                sql = @"SELECT T0.""Id"",
                                T1.""DetId"",
                                T2.""DetDetId"",
                                T1.""ItemCode"",
                                T1.""ItemName"",
                                T1.""Uom"",
                                CAST(COALESCE(T2.""LineNum"", T3.""LineNumUrut"") AS INTEGER) AS ""LineNum"",
                                T2.""Batch""
                                FROM ""Tx_GoodsReceiptPO"" T0
                                LEFT JOIN ""Tx_GoodsReceiptPO_Item"" T1 ON T0.""Id"" = T1.""Id""
                                LEFT JOIN ""Tx_GoodsReceiptPO_Item_Batch"" T2 ON T1.""DetId"" = T2.""DetId""
                                LEFT JOIN (SELECT ""DetDetId"", ""DetId"",
                                                  ROW_NUMBER() OVER (PARTITION BY ""DetId"" ORDER BY ""DetDetId"") AS ""LineNumUrut""
                                             FROM ""Tx_GoodsReceiptPO_Item_Batch"") T3 ON T3.""DetDetId"" = T2.""DetDetId""
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 AND T2.""DetDetId"" = :p2 ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPoScaleView___>(sql, id, detId, detDetId).FirstOrDefault();

                if (model == null)
                {
                    model = new GoodsReceiptPoScaleView___();
                    model.Id = id;
                    model.DetId = detId;
                    model.DetDetId = detDetId;
                }

                // Status permintaan timbang terbaru per baris scale dari Tp_ScaleStaging.
                // Ambil status baris ber-StagingId terbesar (HANA tak izinkan ORDER BY/LIMIT di subquery korelasi).
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""LineNum"", ""DetDetDetId"") AS ""RowNo"", T0.*,
                            (SELECT ST.""Status"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = 'GoodsReceiptPo'
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = 'GoodsReceiptPo')
                            ) AS ""StagingStatus"",
                            (SELECT ST.""RequestId"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = 'GoodsReceiptPo'
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = 'GoodsReceiptPo')
                            ) AS ""StagingRequestId""
                            FROM ""Tx_GoodsReceiptPO_Item_Batch_Scale"" T0
                            WHERE T0.""DetDetId"" = :p0 ";

                model.GoodsReceiptPoScaleModel___ = CONTEXT.Database.SqlQuery<GoodsReceiptPoScaleModel>(sql, detDetId).ToList();
            }

            // "Line Num" = nilai LineNum batch, disajikan string (nested TextBox tak me-render numerik).
            model.LineNumText = model.LineNum.HasValue ? model.LineNum.Value.ToString() : "";
            model.DetDetIdText = model.DetDetId.ToString();

            return model;
        }

        public List<GoodsReceiptPoScaleModel> GoodsReceiptPo__ItemBatchScaleList(long detDetId)
        {
            string sql = null;
            List<GoodsReceiptPoScaleModel> model = new List<GoodsReceiptPoScaleModel>();

            using (var CONTEXT = new HANA_APP())
            {
                // Sertakan status permintaan timbang terbaru per baris scale dari Tp_ScaleStaging.
                // Ambil status baris ber-StagingId terbesar (HANA tak izinkan ORDER BY/LIMIT di subquery korelasi).
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""LineNum"", ""DetDetDetId"") AS ""RowNo"", T0.*,
                            (SELECT ST.""Status"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = 'GoodsReceiptPo'
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = 'GoodsReceiptPo')
                            ) AS ""StagingStatus"",
                            (SELECT ST.""RequestId"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = 'GoodsReceiptPo'
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = 'GoodsReceiptPo')
                            ) AS ""StagingRequestId""
                            FROM ""Tx_GoodsReceiptPO_Item_Batch_Scale"" T0
                            WHERE T0.""DetDetId"" = :p0 ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPoScaleModel>(sql, detDetId).ToList();
            }
            return model;
        }

        public long GoodsReceiptPo_AddNewItemBatchScale(GoodsReceiptPoScaleModel model)
        {
            long detDetDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_GoodsReceiptPO_Item_Batch_Scale tx_Scale = new Tx_GoodsReceiptPO_Item_Batch_Scale();

                        // DetDetDetId = PK identity, jangan disalin dari model.
                        var exceptColumns = new string[] { "DetDetDetId" };
                        CopyProperty.CopyProperties(model, tx_Scale, false, exceptColumns);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        // Sequence otomatis: lanjut dari LineNum terakhir pada batch ini (mulai dari 1).
                        int? maxLineNum = CONTEXT.Database.SqlQuery<int?>("SELECT MAX(\"LineNum\") AS IDU FROM \"Tx_GoodsReceiptPO_Item_Batch_Scale\" WHERE \"DetDetId\"=:p0", model.DetDetId).FirstOrDefault();
                        tx_Scale.LineNum = (maxLineNum ?? 0) + 1;

                        // Uom baris scale SELALU mengikuti Uom item induk, bukan input user
                        // (kolomnya read-only di grid).
                        tx_Scale.Uom = CONTEXT.Database.SqlQuery<string>(@"SELECT T1.""Uom"" AS IDU
                                    FROM ""Tx_GoodsReceiptPO_Item_Batch"" T0
                                    INNER JOIN ""Tx_GoodsReceiptPO_Item"" T1 ON T0.""DetId"" = T1.""DetId""
                                    WHERE T0.""DetDetId""=:p0 ", model.DetDetId).FirstOrDefault();

                        if (string.IsNullOrEmpty(tx_Scale.LineStatus))
                        {
                            tx_Scale.LineStatus = "O";
                        }

                        tx_Scale.CreatedDate = dtModified;
                        tx_Scale.CreatedUser = model._UserId;
                        tx_Scale.ModifiedDate = dtModified;
                        tx_Scale.ModifiedUser = model._UserId;

                        CONTEXT.Tx_GoodsReceiptPO_Item_Batch_Scale.Add(tx_Scale);
                        CONTEXT.SaveChanges();
                        detDetDetId = tx_Scale.DetDetDetId;

                        UpdateBatchNettoFromScale(CONTEXT, model._UserId, model.DetDetId ?? 0);

                        CONTEXT_TRANS.Commit();
                    }
                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();
                        throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                    }
                }
            }

            return detDetDetId;
        }

        public void GoodsReceiptPo_UpdateItemBatchScale(GoodsReceiptPoScaleModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_GoodsReceiptPO_Item_Batch_Scale tx_Scale = CONTEXT.Tx_GoodsReceiptPO_Item_Batch_Scale.Find(model.DetDetDetId ?? 0);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_Scale != null)
                        {
                            // LineNum (Sequence) di-generate server-side, jangan ditimpa dari grid.
                            // Uom juga dikecualikan: nilainya selalu diturunkan dari Uom item induk, dan
                            // karena kolomnya read-only DevExpress mengirimnya null -- bila disalin, Uom jadi NULL.
                            var exceptColumns = new string[] { "DetDetId", "DetDetDetId", "LineNum", "CreatedUser", "CreatedDate", "Uom" };
                            CopyProperty.CopyProperties(model, tx_Scale, false, exceptColumns);

                            tx_Scale.ModifiedDate = dtModified;
                            tx_Scale.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();

                            UpdateBatchNettoFromScale(CONTEXT, model._UserId, tx_Scale.DetDetId ?? 0);
                        }

                        CONTEXT_TRANS.Commit();
                    }
                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();
                        throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                    }
                }
            }
        }

        public void GoodsReceiptPo_DeleteItemBatchScale(int _userId, long DetDetId, long DetDetDetId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetDetId != 0)
                    {
                        try
                        {
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item_Batch_Scale\"  WHERE \"DetDetDetId\"=:p0", DetDetDetId);
                            // Hapus juga permintaan timbang terkait di staging.
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetDetDetId\"=:p0 AND \"TransType\"='GoodsReceiptPo'", DetDetDetId);
                            CONTEXT.SaveChanges();

                            UpdateBatchNettoFromScale(CONTEXT, _userId, DetDetId);

                            CONTEXT_TRANS.Commit();
                        }
                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();
                            throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                        }
                    }
                }
            }
        }

        // Netto pada baris Batch = jumlah Netto seluruh baris Scale miliknya.
        private void UpdateBatchNettoFromScale(HANA_APP CONTEXT, int _userId, long detDetId)
        {
            if (detDetId == 0)
            {
                return;
            }

            CONTEXT.Database.ExecuteSqlCommand(
                @"UPDATE ""Tx_GoodsReceiptPO_Item_Batch""
                    SET ""Netto"" = COALESCE((SELECT SUM(""Netto"") FROM ""Tx_GoodsReceiptPO_Item_Batch_Scale"" WHERE ""DetDetId""=:p0), 0)
                    WHERE ""DetDetId""=:p1 ", detDetId, detDetId);

            // Segarkan total di level item, pola sama dengan CRUD batch.
            long? detId = CONTEXT.Database.SqlQuery<long?>("SELECT \"DetId\" AS IDU FROM \"Tx_GoodsReceiptPO_Item_Batch\" WHERE \"DetDetId\"=:p0", detDetId).FirstOrDefault();
            if (detId.HasValue)
            {
                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPo_UpdateItemQuantity\"(:p0, 'Tx_GoodsReceiptPO_Item_Batch',:p1, :p2)", _userId, detId.Value, 0);
            }
        }

        #endregion

    }


    #endregion

}