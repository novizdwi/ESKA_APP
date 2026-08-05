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

    public class ProductionTaskActivityModel
    {
        public int UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string TransNo { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string DocNum { get; set; }

        public string RoutingName { get; set; }

        public decimal? QuantityPlanned { get; set; }

        public decimal? QuantityActual { get; set; }

        public decimal? QuantityRemain { get; set; }

        public long? EstimatedHours { get; set; }

        public long? ActualHours { get; set; }

        public DateTime? ActivityDate { get; set; }

        public string ActivityStatus { get; set; }

    }

    public class ProductionActivityPauseModel
    {
        public long? Id { get; set; }

        public long? DetId { get; set; }

        [Required(ErrorMessage = "required")]
        public string PauseReason { get; set; }

        public string PauseComments { get; set; }
    }

    public class ProductionActivityFinishModel
    {
        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string FinishTransNo { get; set; }

        public string FinishDocNum { get; set; }

        public string FinsihItemCode { get; set; }

        public string FinishItemName { get; set; }

        [Required(ErrorMessage = "required")]
        public string FinishBatch { get; set; }

        [Required(ErrorMessage = "required")]
        public Decimal? Quantity { get; set; }
        public Decimal? QuantityPlanned { get; set; }
        public Decimal? QuantityActual { get; set; }
        public Decimal? QuantityRemain { get; set; }
        public string Comments { get; set; }

        public List<ProductionTaskDetailItemModel> ListItem_ = new List<ProductionTaskDetailItemModel>();
        public ProductionTaskDetailItemModel Items_ { get; set; }
    }

    public class ProductionTaskDetailItemModel
    {
        public int _UserId { get; set; }

        // Id = Tx_ProductionTask, DetId = kunci baris Tx_ProductionTask_Item (level 2).
        public long? Id { get; set; }
        public long? DetId { get; set; }
        public int? LineNum { get; set; }

        [Required(ErrorMessage = "required")]
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsCode { get; set; }
        public string WhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string Direction { get; set; }
        public int? UomEntry { get; set; }
        public string Uom { get; set; }
        public string Batch { get; set; }
        public Decimal? QuantityPlanned { get; set; }
        public Decimal? QuantityActual { get; set; }
        public string Comments { get; set; }

    }

    // Data header task yang dipakai saat membuat OIGN / OIGE.
    public class ProductionTaskSapHeaderModel
    {
        public long Id { get; set; }

        public string TransNo { get; set; }

        public int? DocEntry { get; set; }

        // ItemCode produk jadi (baris utama Receipt from Production).
        public string ItemCode { get; set; }

        // "Id" pada Tx_ProductionTask_Activity -> UDF baris produk jadi.
        public long ActivityId { get; set; }

        public decimal? ActivityQuantity { get; set; }
    }

    // Hasil insert baris WOR1: LineNum dari SAP untuk ditulis balik ke Tx_ProductionTask_Item.
    public class ProductionTaskActivity_Wor1Line
    {
        public long DetId { get; set; }

        public int LineNum { get; set; }
    }

    public class ProductionTaskActivityBatchView___
    {
        public int? RowNo { get; set; }

        public long Id { get; set; }

        // Kunci baris item (Tx_ProductionTask_Item) yang batch nya sedang dibuka.
        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public List<ProductionTaskActivityBatchModel> ProductionTaskActivityBatchModel___ { get; set; }

    }

    public class ProductionTaskActivityBatchModel
    {
        public int _UserId { get; set; }

        public int? RowNo { get; set; }

        public long? Id { get; set; }

        // Kunci baris Tx_ProductionTask_Item pemilik batch ini.
        public long? DetId { get; set; }

        // PK Tx_ProductionTask_Item_Batch (identity) -> KeyFieldName grid.
        public long BatchId { get; set; }

        [Required(ErrorMessage = "required")]
        public string Batch { get; set; }

        // Read only: "QuantityPlanned" milik item induk, tidak disimpan di tabel batch.
        public decimal? Quantity { get; set; }

        // Default hari ini saat baris baru dibuat (lihat InitNewRow di grid).
        [Required(ErrorMessage = "required")]
        public DateTime? AdmissionDate { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? Netto { get; set; }
    }

    #endregion

    #region Services

    public class ProductionActivityService
    {
        private static string SqlSelect = @"
        SELECT
	        T0.""Id"",
	        T1.""DetId"",
	        T0.""TransNo"",
	        T0.""ItemCode"",
	        T0.""ItemName"",
	        T0.""DocNum"",
	        T2.""RoutingName"",
	        T0.""QuantityPlanned"",
	        T0.""QuantityActual"",
	        COALESCE(T0.""QuantityPlanned"", 0 ) - COALESCE(T0.""QuantityActual"", 0) AS ""QuantityRemain"",
	        T0.""EstimatedHours"",
	        T0.""ActualHours"",
	        T0.""CreatedDate"" AS ""ActivityDate"",
            T1.""Status"" AS ""ActivityStatus""
        FROM ""Tx_ProductionTask"" T0
        INNER JOIN ""Tx_ProductionTask_Activity"" T1 ON T0.""Id"" = T1.""Id""
        INNER JOIN ""Tx_ProcessCard_Detail"" T2 ON T0.""BaseId"" = T2.""Id"" AND T0.""BaseDetId"" = T2.""DetId""
        WHERE T1.""Status"" NOT IN ('Finished')
        AND T0.""Id"" = :p0
        ";

        public ProductionTaskActivityModel GetNewModel(int userId, long id)
        {
            ProductionTaskActivityModel model = this.GetById(userId, id);

            model.UserId = userId;
            return model;
        }

        public void PauseActivity(int userId, ProductionActivityPauseModel model)
        {
            SetStatus(userId, model, "pause");
        }

        public void StartActivity(int userId, ProductionActivityPauseModel model)
        {
            SetStatus(userId, model, "start");
        }

        public void SetStatus(int userId, ProductionActivityPauseModel model, string status)
        {
            string activityStatus = status == "pause" ? "Paused" : "OnProgress";
            string detailType = status == "pause" ? "Paused" : "Production";
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", status, "Id", model.DetId.ToString());

                            DateTime dtModified = DateTime.Now;
                            Tx_ProductionTask_Activity tx_ProductionTask_Activity = CONTEXT.Tx_ProductionTask_Activity.FirstOrDefault(x => x.DetId == model.DetId);
                            if (tx_ProductionTask_Activity != null)
                            {
                                tx_ProductionTask_Activity.Status = activityStatus;
                                tx_ProductionTask_Activity.ModifiedDate = dtModified;
                                tx_ProductionTask_Activity.ModifiedUser = userId;

                                var lastDetail = CONTEXT.Tx_ProductionTask_Activity_Log
                                    .Where(x => x.DetId == model.DetId)
                                    .OrderByDescending(x => x.DetDetId)
                                    .FirstOrDefault();
                                if (lastDetail != null)
                                {
                                    lastDetail.EndTime = dtModified;
                                    lastDetail.ModifiedDate = dtModified;
                                    lastDetail.ModifiedUser = userId;
                                }

                                Tx_ProductionTask_Activity_Log tx_ProductionTask_Activity_log = new Tx_ProductionTask_Activity_Log
                                {
                                    Id = model.Id,
                                    DetId = model.DetId,
                                    DetailType = detailType,
                                    PauseType = model.PauseReason,
                                    Comments = model.PauseComments,
                                    StartTime = dtModified,
                                    CreatedDate = dtModified,
                                    CreatedUser = userId,
                                    ModifiedDate = dtModified,
                                    ModifiedUser = userId
                                };
                                CONTEXT.Tx_ProductionTask_Activity_Log.Add(tx_ProductionTask_Activity_log);

                            }

                            CONTEXT.SaveChanges();
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", status, "Id", model.DetId.ToString());
                            CONTEXT_TRANS.Commit();
                        }
                        catch
                        {
                            CONTEXT_TRANS.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public ProductionTaskActivityModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public ProductionActivityFinishModel GetFinishModel(long id = 0, long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string SqlSelect = @"
                    SELECT
	                    T0.""Id"",
                        T1.""DetId"",
                        T2.""SerialNumber"" AS ""FinishBatch"",
	                    T0.""TransNo"" AS ""FinishTransNo"",
	                    T0.""DocEntry"",
	                    T0.""DocNum"" AS ""FinishDocNum"",
	                    T0.""ItemCode"" AS ""FinsihItemCode"",
	                    T0.""ItemName"" AS ""FinishItemName"",
	                    T0.""QuantityPlanned"",
	                    T0.""QuantityActual"",
	                    COALESCE(T0.""QuantityPlanned"", 0) - COALESCE(T0.""QuantityActual"", 0) AS ""QuantityRemain""
                    FROM ""Tx_ProductionTask"" T0
                    INNER JOIN  ""Tx_ProductionTask_Activity"" T1 ON T0.""Id"" = T1.""Id""
                    INNER JOIN ""Tx_ProcessCard"" T2 ON  T0.""BaseId"" = T2.""Id""
                    WHERE T1.""DetId"" = :p0
                ";
                ProductionActivityFinishModel model = CONTEXT.Database.SqlQuery<ProductionActivityFinishModel>(SqlSelect, detId).SingleOrDefault();

                // Item menempel pada TASK, jadi diambil pakai Id task -- bukan DetId activity.
                model.ListItem_ = this.GetProductionTaskDetailItems(model != null ? model.Id : null);

                return model;
            }
        }

        // Item milik Tx_ProductionTask (level 2), kuncinya "Id" task.
        public List<ProductionTaskDetailItemModel> GetProductionTaskDetailItems(long? id)
        {
            List<ProductionTaskDetailItemModel> ret = new List<ProductionTaskDetailItemModel>();
            using (var CONTEXT = new HANA_APP())
            {
                string sqls = @"
                    SELECT *
                    FROM ""Tx_ProductionTask_Item"" T0
                    WHERE T0.""Id"" = :p0
                    ORDER BY T0.""DetId""
                ";

                ret = CONTEXT.Database.SqlQuery<ProductionTaskDetailItemModel>(sqls, id ?? 0).ToList();
            }

            return ret;
        }

        public ProductionActivityPauseModel GetPauseModel(long id, long detId)
        {
            ProductionActivityPauseModel model = new ProductionActivityPauseModel()
            {
                Id = id,
                DetId = detId
            };

            return model;
        }
        public void FinishActivity(int userId, ProductionActivityFinishModel model)
        {
            if (model != null)
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

                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "finish", "Id", model.Id.ToString());

                            DateTime dtModified = DateTime.Now;
                            Tx_ProductionTask_Activity tx_ProductionTask_Activity = CONTEXT.Tx_ProductionTask_Activity.FirstOrDefault(x => x.DetId == model.DetId);
                            if (tx_ProductionTask_Activity != null)
                            {
                                tx_ProductionTask_Activity.Status = "Finished";

                                tx_ProductionTask_Activity.Quantity = model.Quantity;
                                tx_ProductionTask_Activity.Batch = model.FinishBatch;
                                tx_ProductionTask_Activity.Comments = model.Comments;

                                tx_ProductionTask_Activity.Quantity = model.Quantity;
                                tx_ProductionTask_Activity.ModifiedDate = dtModified;
                                tx_ProductionTask_Activity.ModifiedUser = userId;

                                var lastDetail = CONTEXT.Tx_ProductionTask_Activity_Log
                                    .Where(x => x.DetId == model.DetId)
                                    .OrderByDescending(x => x.DetDetId)
                                    .FirstOrDefault();
                                if (lastDetail != null)
                                {
                                    lastDetail.EndTime = dtModified;
                                    lastDetail.ModifiedDate = dtModified;
                                    lastDetail.ModifiedUser = userId;
                                }

                                Tx_ProductionTask_Activity_Log tx_ProductionTask_Activity_log = new Tx_ProductionTask_Activity_Log
                                {
                                    Id = model.Id,
                                    DetId = model.DetId,
                                    DetailType = "Finish",
                                    Comments = model.Comments,
                                    StartTime = dtModified,
                                    CreatedDate = dtModified,
                                    CreatedUser = userId,
                                    ModifiedDate = dtModified,
                                    ModifiedUser = userId
                                };
                                CONTEXT.Tx_ProductionTask_Activity_Log.Add(tx_ProductionTask_Activity_log);

                            }

                            UpdateItemBatchOut(CONTEXT, userId, model.DetId ?? 0);
                            CONTEXT.SaveChanges();

                            // Validasi bisnis (SpProductionTaskActivity__TransNotif) dijalankan lebih dulu,
                            // supaya SAP tidak disentuh kalau datanya belum valid.
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "finish", "Id", model.Id.ToString());
                            // Sinkronkan komponen activity ke baris WOR1 Production Order di SAP,
                            // lalu tulis balik LineNum hasil insert nya.
                            var wor1Lines = SyncProductionOrderLines(CONTEXT, oCompany, model);
                            UpdateActivityItemLineNum(CONTEXT, userId, wor1Lines);

                            // Sesudah OWOR ter-update: Receipt from Production (OIGN)
                            // dan Issue for Production (OIGE).
                            PostInventoryDocuments(CONTEXT, oCompany, model);

                            // QuantityActual Tx_ProductionTask + IsRunningTask = 'N' diurus di SP ini.
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProductionTaskActivity_UpdateTask\"(:p0,:p1)", userId, model.DetId);

                            if (oCompany.InTransaction)
                            {
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                            }

                            CONTEXT_TRANS.Commit();
                        }
                        catch
                        {
                            if ((oCompany != null) && (oCompany.InTransaction))
                            {
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                            }

                            CONTEXT_TRANS.Rollback();
                            throw;
                        }
                        finally
                        {
                            // Wajib: GetCompany() menahan TransactionLock, Release() yang melepasnya.
                            if (oCompany != null)
                            {
                                SAPCachedCompany.Release(oCompany);
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // Sinkronisasi komponen activity -> baris WOR1 Production Order SAP
        //----------------------------------------------------------------------

        // Menyamakan Tx_ProductionTask_Item dengan baris WOR1 milik Production Order
        // (OWOR) yang DocEntry nya tersimpan di header Tx_ProductionTask.
        //   LineNum = -1        -> insert baris baru di WOR1
        //   LineNum != -1       -> hanya ItemCode yang disamakan kalau berbeda
        //   Direction = 'In'    -> PlannedQuantity dibalik jadi negatif
        //   Direction = 'Out'   -> PlannedQuantity normal
        // Mengembalikan pasangan DetId -> LineNum untuk baris yang baru di-insert.
        private List<ProductionTaskActivity_Wor1Line> SyncProductionOrderLines(HANA_APP CONTEXT, SAPbobsCOM.Company oCompany, ProductionActivityFinishModel model)
        {
            var ret = new List<ProductionTaskActivity_Wor1Line>();

            // DocEntry Production Order diambil dari header (Tx_ProductionTask), bukan dari activity.
            int docEntry = CONTEXT.Database.SqlQuery<int?>(
                @"SELECT TOP 1 T0.""DocEntry""
                  FROM ""Tx_ProductionTask"" T0
                  INNER JOIN ""Tx_ProductionTask_Activity"" T1 ON T0.""Id"" = T1.""Id""
                  WHERE T1.""DetId"" = :p0", model.DetId).FirstOrDefault() ?? 0;

            if (docEntry == 0)
            {
                throw new Exception("[VALIDATION] - Production Order belum punya DocEntry di SAP");
            }

            // Item milik task, jadi difilter pakai Id task.
            long taskId = CONTEXT.Database.SqlQuery<long?>(
                @"SELECT TOP 1 T0.""Id"" FROM ""Tx_ProductionTask_Activity"" T0 WHERE T0.""DetId"" = :p0",
                model.DetId).FirstOrDefault() ?? 0;

            var items = CONTEXT.Database.SqlQuery<ProductionTaskDetailItemModel>(
                @"SELECT
                      T0.""Id"", T0.""DetId"", T0.""LineNum"",
                      T0.""ItemCode"", T0.""ItemName"", T0.""WhsCode"", T0.""Direction"",
                      T0.""QuantityActual""
                  FROM ""Tx_ProductionTask_Item"" T0
                  WHERE T0.""Id"" = :p0
                  ORDER BY T0.""DetId""", taskId).ToList();

            if (items.Count == 0)
            {
                return ret;
            }

            SAPbobsCOM.ProductionOrders oPO = (SAPbobsCOM.ProductionOrders)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

            try
            {
                if (!oPO.GetByKey(docEntry))
                {
                    throw new Exception(string.Format("[VALIDATION] - Production Order DocEntry [{0}] tidak ditemukan di SAP (OWOR)", docEntry));
                }

                // Posisi baris baru dicatat, LineNum nya baru bisa dibaca sesudah Update().
                var newLinePos = new List<KeyValuePair<long, int>>();

                int initialCount = oPO.Lines.Count;
                bool firstNewLine = true;

                foreach (var item in items)
                {
                    double plannedQuantity = (double)(item.QuantityActual ?? 0);

                    // Direction 'In' -> komponen masuk, kuantitas dibalik negatif.
                    if (string.Equals(item.Direction, "In", StringComparison.OrdinalIgnoreCase))
                    {
                        plannedQuantity = plannedQuantity * -1;
                    }

                    if ((item.LineNum ?? -1) == -1)
                    {
                        // Dokumen hasil GetByKey sudah punya baris, jadi baris baru selalu lewat Add().
                        // Kalau kebetulan belum ada baris sama sekali, baris kosong bawaan dipakai.
                        if (!(firstNewLine && initialCount == 0))
                        {
                            oPO.Lines.Add();
                        }
                        firstNewLine = false;

                        oPO.Lines.ItemNo = item.ItemCode;
                        oPO.Lines.PlannedQuantity = plannedQuantity;
                        oPO.Lines.ProductionOrderIssueType = SAPbobsCOM.BoIssueMethod.im_Manual;

                        newLinePos.Add(new KeyValuePair<long, int>(item.DetId ?? 0, oPO.Lines.Count - 1));
                    }
                    else
                    {
                        if (!SetCurrentLineByLineNumber(oPO, item.LineNum.Value))
                        {
                            throw new Exception(string.Format("[VALIDATION] - Baris WOR1 LineNum [{0}] tidak ditemukan pada Production Order DocEntry [{1}]", item.LineNum.Value, docEntry));
                        }

                        // Baris lama: hanya ItemCode yang disamakan.
                        if (oPO.Lines.ItemNo != item.ItemCode)
                        {
                            oPO.Lines.ItemNo = item.ItemCode;
                        }
                    }
                }

                int updateResult = oPO.Update();
                if (updateResult != 0)
                {
                    int nErr = oCompany.GetLastErrorCode();
                    string errMsg = oCompany.GetLastErrorDescription();

                    throw new Exception(string.Format("[VALIDATION] - Update Production Order DocEntry [{0}] : {1}|{2}", docEntry, nErr, errMsg));
                }

                // Baca balik LineNum baris yang baru di-insert.
                if (newLinePos.Count > 0)
                {
                    SAPbobsCOM.ProductionOrders oPORead = (SAPbobsCOM.ProductionOrders)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                    try
                    {
                        if (!oPORead.GetByKey(docEntry))
                        {
                            throw new Exception(string.Format("[VALIDATION] - Production Order DocEntry [{0}] tidak bisa dibaca ulang sesudah update", docEntry));
                        }

                        foreach (var pos in newLinePos)
                        {
                            oPORead.Lines.SetCurrentLine(pos.Value);

                            ret.Add(new ProductionTaskActivity_Wor1Line
                            {
                                DetId = pos.Key,
                                LineNum = oPORead.Lines.LineNumber
                            });
                        }
                    }
                    finally
                    {
                        SapCompany.CleanUp(oPORead);
                    }
                }
            }
            finally
            {
                SapCompany.CleanUp(oPO);
            }

            return ret;
        }

        // WOR1 LineNum tidak selalu sama dengan posisi baris, jadi dicari satu per satu.
        private bool SetCurrentLineByLineNumber(SAPbobsCOM.ProductionOrders oPO, int lineNum)
        {
            for (int i = 0; i < oPO.Lines.Count; i++)
            {
                oPO.Lines.SetCurrentLine(i);
                if (oPO.Lines.LineNumber == lineNum)
                {
                    return true;
                }
            }

            return false;
        }

        //----------------------------------------------------------------------
        // Receipt from Production (OIGN) & Issue for Production (OIGE)
        //----------------------------------------------------------------------

        // BaseType dokumen Production Order di SAP.
        private const int BASETYPE_PRODUCTION_ORDER = 202;

        private void PostInventoryDocuments(HANA_APP CONTEXT, SAPbobsCOM.Company oCompany, ProductionActivityFinishModel model)
        {
            var task = CONTEXT.Database.SqlQuery<ProductionTaskSapHeaderModel>(
                @"SELECT TOP 1
                      T0.""Id"", T0.""TransNo"", T0.""DocEntry"", T0.""ItemCode"",
                      T1.""Id"" AS ""ActivityId"", T1.""Quantity"" AS ""ActivityQuantity""
                  FROM ""Tx_ProductionTask"" T0
                  INNER JOIN ""Tx_ProductionTask_Activity"" T1 ON T0.""Id"" = T1.""Id""
                  WHERE T1.""DetId"" = :p0", model.DetId).FirstOrDefault();

            if (task == null)
            {
                throw new Exception("[VALIDATION] - Data Production Task tidak ditemukan");
            }

            if ((task.DocEntry ?? 0) == 0)
            {
                throw new Exception("[VALIDATION] - Production Order belum punya DocEntry di SAP");
            }

            var items = CONTEXT.Database.SqlQuery<ProductionTaskDetailItemModel>(
                @"SELECT
                      T0.""Id"", T0.""DetId"", T0.""LineNum"",
                      T0.""ItemCode"", T0.""ItemName"", T0.""WhsCode"",
                      T0.""Direction"", T0.""QuantityActual""
                  FROM ""Tx_ProductionTask_Item"" T0
                  WHERE T0.""Id"" = :p0
                  ORDER BY T0.""DetId""", task.Id).ToList();

            AddReceiptFromProduction(oCompany, task, items);
            AddIssueForProduction(oCompany, task, items);
        }

        // OIGN: baris produk jadi (dari Tx_ProductionTask) + item ber-Direction 'In'.
        private void AddReceiptFromProduction(SAPbobsCOM.Company oCompany, ProductionTaskSapHeaderModel task, List<ProductionTaskDetailItemModel> items)
        {
            var inItems = items
                .Where(x => string.Equals(x.Direction, "In", StringComparison.OrdinalIgnoreCase)
                            && ((x.QuantityActual ?? 0) > 0))
                .ToList();

            bool hasParentLine = (task.ActivityQuantity ?? 0) > 0;

            if (!hasParentLine && (inItems.Count == 0))
            {
                return;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

            try
            {
                oDoc.UserFields.Fields.Item("U_IDU_WebTransNo").Value = task.TransNo ?? "";
                oDoc.UserFields.Fields.Item("U_IDU_WebId").Value = task.Id.ToString();

                bool firstLine = true;

                // Produk jadi: ItemCode dari Tx_ProductionTask, qty dari Tx_ProductionTask_Activity.
                if (hasParentLine)
                {
                    oDoc.Lines.ItemCode = task.ItemCode;
                    oDoc.Lines.Quantity = (double)(task.ActivityQuantity ?? 0);
                    oDoc.Lines.BaseType = BASETYPE_PRODUCTION_ORDER;
                    oDoc.Lines.BaseEntry = task.DocEntry ?? 0;
                    oDoc.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = task.ActivityId.ToString();

                    firstLine = false;
                }

                foreach (var item in inItems)
                {
                    if (!firstLine)
                    {
                        oDoc.Lines.Add();
                    }
                    firstLine = false;

                    oDoc.Lines.ItemCode = item.ItemCode;
                    oDoc.Lines.Quantity = (double)(item.QuantityActual ?? 0);
                    oDoc.Lines.BaseType = BASETYPE_PRODUCTION_ORDER;
                    oDoc.Lines.BaseEntry = task.DocEntry ?? 0;
                    oDoc.Lines.BaseLine = item.LineNum ?? 0;

                    oDoc.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = (item.Id ?? 0).ToString();
                    oDoc.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = (item.DetId ?? 0).ToString();
                }

                int result = oDoc.Add();
                if (result != 0)
                {
                    int nErr = oCompany.GetLastErrorCode();
                    string errMsg = oCompany.GetLastErrorDescription();

                    throw new Exception(string.Format("[VALIDATION] - Add Receipt from Production : {0}|{1}", nErr, errMsg));
                }
            }
            finally
            {
                SapCompany.CleanUp(oDoc);
            }
        }

        // OIGE: item ber-Direction 'Out'.
        private void AddIssueForProduction(SAPbobsCOM.Company oCompany, ProductionTaskSapHeaderModel task, List<ProductionTaskDetailItemModel> items)
        {
            var outItems = items
                .Where(x => string.Equals(x.Direction, "Out", StringComparison.OrdinalIgnoreCase)
                            && ((x.QuantityActual ?? 0) > 0))
                .ToList();

            if (outItems.Count == 0)
            {
                return;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

            try
            {
                oDoc.UserFields.Fields.Item("U_IDU_WebTransNo").Value = task.TransNo ?? "";
                oDoc.UserFields.Fields.Item("U_IDU_WebId").Value = task.Id.ToString();

                bool firstLine = true;

                foreach (var item in outItems)
                {
                    if (!firstLine)
                    {
                        oDoc.Lines.Add();
                    }
                    firstLine = false;

                    oDoc.Lines.ItemCode = item.ItemCode;
                    oDoc.Lines.Quantity = (double)(item.QuantityActual ?? 0);
                    oDoc.Lines.BaseType = BASETYPE_PRODUCTION_ORDER;
                    oDoc.Lines.BaseEntry = task.DocEntry ?? 0;
                    oDoc.Lines.BaseLine = item.LineNum ?? 0;

                    oDoc.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = (item.Id ?? 0).ToString();
                    oDoc.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = (item.DetId ?? 0).ToString();
                }

                int result = oDoc.Add();
                if (result != 0)
                {
                    int nErr = oCompany.GetLastErrorCode();
                    string errMsg = oCompany.GetLastErrorDescription();

                    throw new Exception(string.Format("[VALIDATION] - Add Issue for Production : {0}|{1}", nErr, errMsg));
                }
            }
            finally
            {
                SapCompany.CleanUp(oDoc);
            }
        }

        private void UpdateActivityItemLineNum(HANA_APP CONTEXT, int userId, List<ProductionTaskActivity_Wor1Line> lines)
        {
            if ((lines == null) || (lines.Count == 0))
            {
                return;
            }

            foreach (var line in lines)
            {
                CONTEXT.Database.ExecuteSqlCommand(
                    @"UPDATE ""Tx_ProductionTask_Item""
                      SET ""LineNum"" = :p0,
                          ""ModifiedDate"" = CURRENT_TIMESTAMP,
                          ""ModifiedUser"" = :p1
                      WHERE ""DetId"" = :p2", line.LineNum, userId, line.DetId);
            }
        }

        public ProductionTaskActivityModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            long currentTaskId = id;
            if (currentTaskId == 0)
            {
                string ssql = @" SELECT ""Id"" AS ""CurrentTaskId""
                    FROM ""Tx_ProductionTask"" 
                    WHERE ""Status"" = 'Open'
                    AND ""IsRunningTask"" = 'Y' 
                    AND  ""OperatorId"" = :p0 
                ";
                currentTaskId = CONTEXT.Database.SqlQuery<long>(ssql, userId).FirstOrDefault();

            }

            ProductionTaskActivityModel model = new ProductionTaskActivityModel();

            if (currentTaskId != 0)
            {
                model = CONTEXT.Database.SqlQuery<ProductionTaskActivityModel>(SqlSelect, currentTaskId).SingleOrDefault();
            }
            if (model == null)
            {
                model = new ProductionTaskActivityModel { Id = 0 };
            }

            return model;
        }

        public long ProductionTaskActivity_AddNewItem(ProductionTaskDetailItemModel model)
        {
            long detId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_ProductionTask_Item tx_ProductionTask_Item = new Tx_ProductionTask_Item();
                        CopyProperty.CopyProperties(model, tx_ProductionTask_Item, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_ProductionTask_Item.CreatedDate = dtModified;
                        tx_ProductionTask_Item.CreatedUser = model._UserId;
                        tx_ProductionTask_Item.ModifiedDate = dtModified;
                        tx_ProductionTask_Item.ModifiedUser = model._UserId;

                        CONTEXT.Tx_ProductionTask_Item.Add(tx_ProductionTask_Item);
                        CONTEXT.SaveChanges();
                        detId = tx_ProductionTask_Item.DetId;

                        String keyValue;
                        keyValue = tx_ProductionTask_Item.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "addItem", "Id", model.Id.ToString() );

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

            return detId;
        }

        public void ProductionTaskActivity_UpdateItem(ProductionTaskDetailItemModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "updateItem", "Id", keyValue);

                        Tx_ProductionTask_Item tx_ProductionTask_Item = CONTEXT.Tx_ProductionTask_Item.Find(model.DetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_ProductionTask_Item != null)
                        {
                            var exceptColumns = new string[] { "Id", "DetId", "DetId", "QuantityPlanned", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_ProductionTask_Item, false, exceptColumns);

                            tx_ProductionTask_Item.ModifiedDate = dtModified;
                            tx_ProductionTask_Item.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProductionTaskActivity_UpdateItemQuantity\"(:p0, 'Tx_ProductionTaskActivity_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                            SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "updateItem", "Id", keyValue);

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

        public void ProductionTaskActivity_DeleteItem(int _userId, long Id, long DetId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetId != 0)
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(_userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "deleteItem", "Id", Id.ToString());

                            //CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ProductionTaskActivity_Item_Batch_Scale\"  WHERE \"DetId\"=:p0", DetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ProductionTask_Item\"  WHERE \"DetId\"=:p0", DetId);
                            CONTEXT.SaveChanges();

                            //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProductionTaskActivity_UpdateItemQuantity\"(:p0, 'Tx_ProductionTaskActivity_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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

        #region item batch

        // Batch dilihat per baris item: kuncinya "DetId" milik Tx_ProductionTask_Item.
        // "Quantity" tidak disimpan di tabel batch, diambil dari QuantityPlanned item induk.
        private const string SqlSelectItemBatch = @"
            SELECT
                ROW_NUMBER() OVER (ORDER BY T0.""BatchId"") AS ""RowNo"",
                T0.""Id"",
                T0.""DetId"",
                T0.""BatchId"",
                T0.""Batch"",
                T1.""QuantityPlanned"" AS ""Quantity"",
                T0.""AdmissionDate"",
                T0.""Netto""
            FROM ""Tx_ProductionTask_Item_Batch"" T0
            INNER JOIN ""Tx_ProductionTask_Item"" T1 ON T0.""DetId"" = T1.""DetId""
            WHERE T0.""DetId"" = :p0
            ORDER BY T0.""BatchId""
        ";

        public ProductionTaskActivityBatchView___ GetProductionTaskActivity_Batch(long id, long detId)
        {
            string sql = null;
            ProductionTaskActivityBatchView___ model = new ProductionTaskActivityBatchView___();

            using (var CONTEXT = new HANA_APP())
            {
                // ItemCode/ItemName/WhsCode/WhsName semuanya ada di tabel item.
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName"", T0.""WhsCode"", T0.""WhsName""
                        FROM ""Tx_ProductionTask_Item"" T0
                        WHERE T0.""DetId"" = :p0 ";

                model = CONTEXT.Database.SqlQuery<ProductionTaskActivityBatchView___>(sql, detId).FirstOrDefault();

                if (model == null)
                {
                    model = new ProductionTaskActivityBatchView___ { Id = id, DetId = detId };
                }

                model.ProductionTaskActivityBatchModel___ = CONTEXT.Database
                    .SqlQuery<ProductionTaskActivityBatchModel>(SqlSelectItemBatch, detId).ToList();
            }

            return model;
        }

        // detId = kunci Tx_ProductionTask_Item.
        public List<ProductionTaskActivityBatchModel> GetProductionTaskActivity_ItemBatchList(long id, long detId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CONTEXT.Database.SqlQuery<ProductionTaskActivityBatchModel>(SqlSelectItemBatch, detId).ToList();
            }
        }

        public void UpdateItemBatchOut(HANA_APP CONTEXT, int userId, long detId)
        {
            string sql = @"
                UPDATE T1 SET
                    T1.""Batch"" = T0.""Batch"",
                    T1.""ModifiedDate"" = CURRENT_TIMESTAMP,
                    T1.""ModifiedUser"" = :p1
                FROM ""Tx_ProductionTask_Activity"" T0
                -- Item menempel pada TASK, jadi join cukup lewat ""Id"".
                -- ""DetId"" milik item adalah PK nya sendiri, bukan kunci activity.
                INNER JOIN ""Tx_ProductionTask_Item"" T1 ON T0.""Id"" = T1.""Id""
                WHERE T0.""DetId"" = :p0
                AND T1.""Direction"" = 'In'
 
            ";

            CONTEXT.Database.ExecuteSqlCommand(sql, detId, userId);
        }

        // Netto item induk = jumlah Netto seluruh batch nya.
        private void UpdateItemQuantityActual(HANA_APP CONTEXT, int userId, long detId)
        {
            string sql = @"
                UPDATE ""Tx_ProductionTask_Item""
                SET ""QuantityActual"" = COALESCE((
                        SELECT SUM(""Netto"")
                        FROM ""Tx_ProductionTask_Item_Batch""
                        WHERE ""DetId"" = :p0
                    ), 0),
                    ""ModifiedDate"" = CURRENT_TIMESTAMP,
                    ""ModifiedUser"" = :p1
                WHERE ""DetId"" = :p2
            ";

            CONTEXT.Database.ExecuteSqlCommand(sql, detId, userId, detId);
        }

        public long ProductionTaskActivity_AddNewItemBatch(ProductionTaskActivityBatchModel model)
        {
            long batchId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_ProductionTask_Item_Batch tx_ProductionTask_Item_Batch = new Tx_ProductionTask_Item_Batch();

                        // BatchId identity -> jangan ikut disalin dari model.
                        var exceptColumnsAdd = new string[] { "BatchId" };
                        CopyProperty.CopyProperties(model, tx_ProductionTask_Item_Batch, false, exceptColumnsAdd);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_ProductionTask_Item_Batch.CreatedDate = dtModified;
                        tx_ProductionTask_Item_Batch.CreatedUser = model._UserId;
                        tx_ProductionTask_Item_Batch.ModifiedDate = dtModified;
                        tx_ProductionTask_Item_Batch.ModifiedUser = model._UserId;

                        CONTEXT.Tx_ProductionTask_Item_Batch.Add(tx_ProductionTask_Item_Batch);
                        CONTEXT.SaveChanges();
                        batchId = tx_ProductionTask_Item_Batch.BatchId;

                        String keyValue;
                        keyValue = (model.DetId ?? 0).ToString();

                        UpdateItemQuantityActual(CONTEXT, model._UserId, model.DetId ?? 0);
                        SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "addItemBatch", "Id", model.Id.ToString() );

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

            return batchId;
        }

        public void ProductionTaskActivity_UpdateItemBatch(ProductionTaskActivityBatchModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = (model.DetId ?? 0).ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "updateItemBatch", "Id", keyValue);

                        // PK tabel batch adalah BatchId, bukan DetId.
                        Tx_ProductionTask_Item_Batch tx_ProductionTask_Item_Batch = CONTEXT.Tx_ProductionTask_Item_Batch.Find(model.BatchId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_ProductionTask_Item_Batch != null)
                        {
                            var exceptColumns = new string[] { "Id", "DetId", "DetId", "BatchId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_ProductionTask_Item_Batch, false, exceptColumns);

                            tx_ProductionTask_Item_Batch.ModifiedDate = dtModified;
                            tx_ProductionTask_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();

                            long detId_ = tx_ProductionTask_Item_Batch.DetId ?? 0;
                            UpdateItemQuantityActual(CONTEXT, model._UserId, detId_);
                            SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "updateItemBatch", "Id", model.Id.ToString());

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

        public void ProductionTaskActivity_DeleteItemBatch(int _userId, long Id, long DetId, long BatchId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (BatchId != 0)
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(_userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "deleteItemBatch", "Id", Id.ToString());

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ProductionTask_Item_Batch\"  WHERE \"BatchId\"=:p0", BatchId);
                            CONTEXT.SaveChanges();

                            UpdateItemQuantityActual(CONTEXT, _userId, DetId);
                            SpNotif.SpSysControllerTransNotif(_userId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "deleteItemBatch", "Id", Id.ToString());
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

        #endregion
    }

    #endregion

}