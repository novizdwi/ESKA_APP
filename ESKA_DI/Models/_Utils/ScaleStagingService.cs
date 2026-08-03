using System;
using System.Linq;
using System.Web.Script.Serialization;

using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models._Utils
{
    // Baris hasil gather Scale -> Batch -> Item -> Header untuk disimpan ke Tp_ScaleStaging.
    public class ScaleStagingRow
    {
        public long Id { get; set; }
        public long? DetId { get; set; }
        public long? DetDetId { get; set; }
        public long? DetDetDetId { get; set; }
        public string TransNo { get; set; }
        public string DocNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string WhsCode { get; set; }
        public string Batch { get; set; }
        public int? BatchLineNum { get; set; }
        public int? ScaleLineNum { get; set; }
        public decimal? Quantity { get; set; }
        public string Uom { get; set; }
    }

    // Konfigurasi tabel per jenis dokumen (TransType).
    internal class ScaleStagingTableSet
    {
        public string Header;
        public string Item;
        public string Batch;
        public string Scale;
        public string WhseCol;   // "Whse" (GRPO) atau "WhsCode" (Re-Process)
        public bool HasDocNum;   // header punya kolom DocNum?
    }

    public class ScaleStagingService
    {
        // Petakan TransType -> set tabel sumber. transType yang tidak dikenal ditolak.
        private static ScaleStagingTableSet ResolveTables(string transType)
        {
            switch (transType)
            {
                case "GoodsReceiptPo":
                    return new ScaleStagingTableSet
                    {
                        Header = "Tx_GoodsReceiptPO",
                        Item = "Tx_GoodsReceiptPO_Item",
                        Batch = "Tx_GoodsReceiptPO_Item_Batch",
                        Scale = "Tx_GoodsReceiptPO_Item_Batch_Scale",
                        WhseCol = "Whse",
                        HasDocNum = true
                    };
                case "IssueAndReceiptIssue":
                    return new ScaleStagingTableSet
                    {
                        Header = "Tx_IssueAndReceipt",
                        Item = "Tx_IssueAndReceipt_Issue_Item",
                        Batch = "Tx_IssueAndReceipt_Issue_Item_Batch",
                        Scale = "Tx_IssueAndReceipt_Issue_Item_Batch_Scale",
                        WhseCol = "WhsCode",
                        HasDocNum = false
                    };
                case "IssueAndReceiptReceipt":
                    return new ScaleStagingTableSet
                    {
                        Header = "Tx_IssueAndReceipt",
                        Item = "Tx_IssueAndReceipt_Receipt_Item",
                        Batch = "Tx_IssueAndReceipt_Receipt_Item_Batch",
                        Scale = "Tx_IssueAndReceipt_Receipt_Item_Batch_Scale",
                        WhseCol = "WhsCode",
                        HasDocNum = false
                    };
                default:
                    throw new Exception(string.Format("[VALIDATION] TransType tidak dikenal: {0} ", transType));
            }
        }

        // Simpan satu baris permintaan timbang ke Tp_ScaleStaging (Status='Waiting').
        // Mengembalikan StagingId yang terbentuk.
        public long SaveFromScale(int userId, string transType, long id, long detId, long detDetId, long detDetDetId)
        {
            ScaleStagingTableSet t = ResolveTables(transType);
            long stagingId = 0;

            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Gather data dari baris Scale ke atas.
                        string docNumExpr = t.HasDocNum ? "T0.\"DocNum\"" : "CAST(NULL AS NVARCHAR(50))";
                        string sqlGather = string.Format(@"SELECT T0.""Id"",
                                    T1.""DetId"",
                                    T2.""DetDetId"",
                                    T3.""DetDetDetId"",
                                    T0.""TransNo"",
                                    {0} AS ""DocNum"",
                                    T1.""ItemCode"",
                                    T1.""ItemName"",
                                    T1.""{1}"" AS ""WhsCode"",
                                    T2.""Batch"",
                                    T2.""LineNum"" AS ""BatchLineNum"",
                                    T3.""LineNum"" AS ""ScaleLineNum"",
                                    T3.""Quantity"",
                                    T3.""Uom""
                                FROM ""{2}"" T0
                                INNER JOIN ""{3}"" T1 ON T0.""Id"" = T1.""Id""
                                INNER JOIN ""{4}"" T2 ON T1.""DetId"" = T2.""DetId""
                                INNER JOIN ""{5}"" T3 ON T2.""DetDetId"" = T3.""DetDetId""
                                WHERE T3.""DetDetDetId"" = :p0 ",
                                docNumExpr, t.WhseCol, t.Header, t.Item, t.Batch, t.Scale);

                        ScaleStagingRow row = CONTEXT.Database.SqlQuery<ScaleStagingRow>(sqlGather, detDetDetId).FirstOrDefault();
                        if (row == null)
                        {
                            throw new Exception("[VALIDATION] Baris scale tidak ditemukan. ");
                        }

                        // 2. Anti-duplikat: satu 'Waiting' per baris scale (per TransType).
                        int waiting = CONTEXT.Database.SqlQuery<int>(
                            "SELECT COUNT(*) AS IDU FROM \"Tp_ScaleStaging\" WHERE \"DetDetDetId\"=:p0 AND \"TransType\"=:p1 AND \"Status\"='Waiting'",
                            detDetDetId, transType).FirstOrDefault();
                        if (waiting > 0)
                        {
                            throw new Exception("[VALIDATION] Permintaan timbang untuk baris ini sudah dalam antrean. ");
                        }

                        // 3. RequestId = UUID 32 char (kolom NVARCHAR(32)); RequestPayload minimal.
                        string requestId = Guid.NewGuid().ToString("N");
                        string requestPayload = new JavaScriptSerializer().Serialize(new
                        {
                            RequestId = requestId,
                            row.ItemCode,
                            row.Batch,
                            row.Quantity,
                            row.Uom
                        });

                        // 4. INSERT (StagingId identity -> tidak diisi). Semua placeholder distinct.
                        CONTEXT.Database.ExecuteSqlCommand(
                            @"INSERT INTO ""Tp_ScaleStaging""
                                (""TransType"",""Id"",""DetId"",""DetDetId"",""DetDetDetId"",
                                 ""TransNo"",""DocNum"",""ItemCode"",""ItemName"",""WhsCode"",
                                 ""Batch"",""BatchLineNum"",""ScaleLineNum"",""Quantity"",""Uom"",
                                 ""Status"",""RequestId"",""RetryCount"",""RequestPayload"",
                                 ""CreatedDate"",""CreatedUser"",""ModifiedDate"",""ModifiedUser"")
                              VALUES
                                (:p0,:p1,:p2,:p3,:p4,
                                 :p5,:p6,:p7,:p8,:p9,
                                 :p10,:p11,:p12,:p13,:p14,
                                 'Waiting',:p15,0,:p16,
                                 CURRENT_TIMESTAMP,:p17,CURRENT_TIMESTAMP,:p18) ",
                            transType,
                            row.Id,
                            (object)row.DetId ?? DBNull.Value,
                            (object)row.DetDetId ?? DBNull.Value,
                            (object)row.DetDetDetId ?? DBNull.Value,
                            (object)row.TransNo ?? DBNull.Value,
                            (object)row.DocNum ?? DBNull.Value,
                            (object)row.ItemCode ?? DBNull.Value,
                            (object)row.ItemName ?? DBNull.Value,
                            (object)row.WhsCode ?? DBNull.Value,
                            (object)row.Batch ?? DBNull.Value,
                            (object)row.BatchLineNum ?? DBNull.Value,
                            (object)row.ScaleLineNum ?? DBNull.Value,
                            (object)row.Quantity ?? DBNull.Value,
                            (object)row.Uom ?? DBNull.Value,
                            requestId,
                            requestPayload,
                            userId,
                            userId);

                        stagingId = CONTEXT.Database.SqlQuery<long>(
                            "SELECT MAX(\"StagingId\") AS IDU FROM \"Tp_ScaleStaging\" WHERE \"RequestId\"=:p0", requestId).FirstOrDefault();

                        CONTEXT_TRANS.Commit();
                    }
                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();
                        throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                    }
                }
            }

            return stagingId;
        }
    }
}
