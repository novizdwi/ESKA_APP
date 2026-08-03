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

                        // 4. Simpan via EF (seragam pola Add). StagingId identity -> terisi otomatis.
                        DateTime dtNow = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        Tp_ScaleStaging st = new Tp_ScaleStaging
                        {
                            TransType = transType,
                            Id = row.Id,
                            DetId = row.DetId,
                            DetDetId = row.DetDetId,
                            DetDetDetId = row.DetDetDetId,
                            TransNo = row.TransNo,
                            DocNum = row.DocNum,
                            ItemCode = row.ItemCode,
                            ItemName = row.ItemName,
                            WhsCode = row.WhsCode,
                            Batch = row.Batch,
                            BatchLineNum = row.BatchLineNum,
                            ScaleLineNum = row.ScaleLineNum,
                            Quantity = row.Quantity,
                            Uom = row.Uom,
                            Status = "Waiting",
                            RequestId = requestId,
                            RetryCount = 0,
                            RequestPayload = requestPayload,
                            CreatedDate = dtNow,
                            CreatedUser = userId,
                            ModifiedDate = dtNow,
                            ModifiedUser = userId
                        };

                        CONTEXT.Tp_ScaleStaging.Add(st);
                        CONTEXT.SaveChanges();
                        stagingId = st.StagingId;

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
