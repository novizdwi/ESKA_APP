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

    public class ReProcessModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return _FormModeEnum; }
            set { _FormModeEnum = value; }
        }

        public int _UserId { get; set; }
        public int? CreatedUser { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public long Id { get; set; }
        public string TransType { get; set; }
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? PostingDate { get; set; }

        // Standalone: field Production Order / Process Card (BaseEntry, BaseDocNum, BaseProcessCard*)
        // dihapus — modul tidak lagi memakai Production Order. Kolom DB & entity EF tetap ada.

        // SAP
        public long? IssueDocEntry { get; set; }
        public string IssueDocNum { get; set; }
        public DateTime? IssueDocDate { get; set; }

        public long? ReceiptDocEntry { get; set; }
        public string ReceiptDocNum { get; set; }
        public DateTime? ReceiptDocDate { get; set; }

        public string Status { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalMessages { get; set; }
        public string CheckNeedApproval_ { get; set; }
        public string IsApproval { get; set; }
        public string IsAfterPosted { get; set; }
        public string CancelReason { get; set; }
        public string RefNo { get; set; }
        public string CreatedDate_ { get; set; }
        public string ModifiedDate_ { get; set; }

        public List<IssueReceipt_IssueItemModel> ListIssueItem_ = new List<IssueReceipt_IssueItemModel>();
        public List<IssueReceipt_ReceiptItemModel> ListReceiptItem_ = new List<IssueReceipt_ReceiptItemModel>();

        public IssueReceipt_IssueDetail IssueDetails_ { get; set; }
        public IssueReceipt_ReceiptDetail ReceiptDetails_ { get; set; }
    }

    public class IssueReceipt_IssueDetail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<IssueReceipt_IssueItemModel> insertedRowValues { get; set; }
        public List<IssueReceipt_IssueItemModel> modifiedRowValues { get; set; }
    }

    public class IssueReceipt_ReceiptDetail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<IssueReceipt_ReceiptItemModel> insertedRowValues { get; set; }
        public List<IssueReceipt_ReceiptItemModel> modifiedRowValues { get; set; }
    }

    public class IssueReceipt_IssueItemModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;
        public FormModeEnum _FormMode { get => _FormModeEnum; set => _FormModeEnum = value; }

        public int? RowNo { get; set; }
        public int _UserId { get; set; }
        public long? Id { get; set; }
        public long? DetId { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public int? QuantityCreated { get; set; }   // Total Created = SUM(Quantity batch) per item
        public decimal? Netto { get; set; }
        public decimal? Value { get; set; }          // Receipt: total Netto per line = SUM(Netto batch)
        public string Uom { get; set; }
        public string WhsCode { get; set; }
        public string MsnPrd { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public string Department { get; set; }
        public long? DocEntry { get; set; }
        public int? LineNum { get; set; }
        public string LineStatus { get; set; }

        public List<IssueReceipt_IssueBatchModel> ListBatch_ = new List<IssueReceipt_IssueBatchModel>();
    }

    public class IssueReceipt_IssueBatchModel
    {
        public int _UserId { get; set; }
        public int? RowNo { get; set; }
        public long? DetId { get; set; }
        public long? DetDetId { get; set; }

        [Required(ErrorMessage = "required")]
        public string Batch { get; set; }

        public DateTime? AdmissionDate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Netto { get; set; }

        public List<IssueReceipt_IssueScaleModel> ListScale_ = new List<IssueReceipt_IssueScaleModel>();
    }

    public class IssueReceipt_IssueScaleModel
    {
        public int _UserId { get; set; }
        public int? RowNo { get; set; }
        public long? DetDetId { get; set; }
        public long? DetDetDetId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Netto { get; set; }
        public string Uom { get; set; }
    }

    public class IssueReceipt_ReceiptItemModel : IssueReceipt_IssueItemModel
    {
        public new List<IssueReceipt_ReceiptBatchModel> ListBatch_ = new List<IssueReceipt_ReceiptBatchModel>();
    }

    public class IssueReceipt_ReceiptBatchModel
    {
        public int _UserId { get; set; }
        public int? RowNo { get; set; }
        public long? DetId { get; set; }
        public long? DetDetId { get; set; }

        [Required(ErrorMessage = "required")]
        public string Batch { get; set; }

        public DateTime? AdmissionDate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Netto { get; set; }

        public List<IssueReceipt_ReceiptScaleModel> ListScale_ = new List<IssueReceipt_ReceiptScaleModel>();
    }

    public class IssueReceipt_ReceiptScaleModel
    {
        public int _UserId { get; set; }
        public int? RowNo { get; set; }
        public long? DetDetId { get; set; }
        public long? DetDetDetId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Netto { get; set; }
        public string Uom { get; set; }
    }

    public class IssueReceipt_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<IssueReceipt_ApprovalModel> insertedRowValues { get; set; }
        public List<IssueReceipt_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class IssueReceipt_ApprovalModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;
        public FormModeEnum _FormMode { get => _FormModeEnum; set => _FormModeEnum = value; }

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

    public class IssueReceiptAddResultModel
    {
        public string IssueDocEntry { get; set; }
        public string ReceiptDocEntry { get; set; }
        public Dictionary<long, int> LineMapping { get; set; } 
    }

    public class ReProcessBatchReceiptView___
    {
        public long Id { get; set; }

        public string BaseDocNum { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public int? Quantity { get; set; }

        public int? TotalNeeded { get; set; }

        public int? TotalCreated { get; set; }

        public List<ReProcessBatchReceiptModel> ReProcessBatchReceiptModel___ { get; set; }
    }

    public class ReProcessBatchReceiptModel
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

    public class ReProcessBatchIssueView___
    {
        public long Id { get; set; }

        public string BaseDocNum { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public int? Quantity { get; set; }

        public int? TotalNeeded { get; set; }

        public int? TotalCreated { get; set; }

        public List<ReProcessBatchIssueModel> ReProcessBatchIssueModel___ { get; set; }
    }

    public class ReProcessBatchIssueModel
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



    // Proyeksi seragam item/batch untuk validasi Post berjenjang (Item -> Batch -> Scale).
    // Model Issue & Receipt punya tipe batch berbeda (dan ListBatch_ Receipt menyembunyikan
    // milik base class), jadi keduanya dipetakan ke bentuk ini supaya aturan validasinya
    // ditulis sekali dan berlaku sama untuk kedua sisi.
    public class ReProcess_ValidateItem
    {
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public List<ReProcess_ValidateBatch> Batches { get; set; }
    }

    public class ReProcess_ValidateBatch
    {
        public string Batch { get; set; }
        public decimal Quantity { get; set; }
        public long DetDetId { get; set; }
    }

    // Baris hasil query cost SAP (OINM) per baris Goods Issue.
    public class ReProcess_IssueCostRow
    {
        public int? LineNum { get; set; }
        public string ItemCode { get; set; }
        public decimal? IssueCost { get; set; }   // unit cost = TransValue/OutQty
        public decimal? IssueValue { get; set; }  // total cost = TransValue (kolom Value)
    }

    // Baris ringkas item (Issue/Receipt) untuk perhitungan costing.
    public class ReProcess_CostItemRow
    {
        public long DetId { get; set; }
        public string ItemCode { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Netto { get; set; }
    }

    // Header popup Timbangan (Scale) — dipakai bersama Issue & Receipt (dibedakan `side`).
    public class ReProcessScaleView___
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

        public List<ReProcessScaleModel> ReProcessScaleModel___ { get; set; }
    }

    public class ReProcessScaleModel
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

    #endregion

    #region Services

    public class ReProcessService
    {

        public ReProcessModel GetNewModel(int userId)
        {
            ReProcessModel model = new ReProcessModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public long Add(ReProcessModel model)
        {
            long Id = 0;

            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_IssueAndReceipt ent = new Tx_IssueAndReceipt();
                        CopyProperty.CopyProperties(model, ent, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        ent.TransType = "IssueAndReceipt";
                        ent.CreatedDate = dtModified;
                        ent.CreatedUser = model._UserId;
                        ent.ModifiedDate = dtModified;
                        ent.ModifiedUser = model._UserId;

                        string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                        ent.TransNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'IssueAndReceipt','" + dateX + "','') ").SingleOrDefault();

                        CONTEXT.Tx_IssueAndReceipt.Add(ent);
                        CONTEXT.SaveChanges();
                        Id = ent.Id;

                        // Standalone: dokumen baru mulai KOSONG. Item ditambahkan user via CFL (ChooseItem),
                        // bukan di-seed dari Production Order / Process Card.

                        CONTEXT_TRANS.Commit();
                    }
                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();
                        throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                    }
                }
            }

            return Id;
        }

        // Salin sebuah dokumen ReProcess menjadi dokumen Draft BARU, lengkap dengan
        // Issue/Receipt Item -> Batch -> Scale. Mengembalikan Id dokumen baru.
        //
        // Dipakai untuk membuat dokumen uji/berulang tanpa entri ulang. Semua PK adalah
        // identity, jadi penyalinan dilakukan berjenjang: simpan induk -> ambil PK barunya
        // -> pakai sebagai FK anak.
        public long Duplicate(int userId, long sourceId)
        {
            long newId = 0;

            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    Tx_IssueAndReceipt src = CONTEXT.Tx_IssueAndReceipt.Find(sourceId);
                    if (src == null)
                    {
                        throw new Exception("[VALIDATION] - Dokumen sumber tidak ditemukan");
                    }

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    // Header dibuat manual (bukan CopyProperties) supaya jejak posting dokumen
                    // sumber -- DocEntry/DocNum/PostingDate SAP -- TIDAK ikut terbawa; dokumen
                    // baru harus tampak murni Draft yang belum pernah menyentuh SAP.
                    Tx_IssueAndReceipt ent = new Tx_IssueAndReceipt();
                    ent.TransType = "IssueAndReceipt";
                    ent.TransDate = dtModified;
                    ent.Status = "Draft";
                    ent.RefNo = src.RefNo;
                    ent.BaseEntry = src.BaseEntry;
                    ent.BaseDocNum = src.BaseDocNum;
                    ent.BaseProcessCardTransNo = src.BaseProcessCardTransNo;
                    ent.BaseProcessCardId = src.BaseProcessCardId;
                    ent.BaseProcessCardSort = src.BaseProcessCardSort;
                    ent.BaseProcessCardRoutingCode = src.BaseProcessCardRoutingCode;
                    ent.BaseProcessCardRoutingName = src.BaseProcessCardRoutingName;
                    ent.BaseProcessCardOperatorId = src.BaseProcessCardOperatorId;
                    ent.BaseProcessCardOperatorName = src.BaseProcessCardOperatorName;
                    ent.CreatedDate = dtModified;
                    ent.CreatedUser = userId;
                    ent.ModifiedDate = dtModified;
                    ent.ModifiedUser = userId;

                    string dateX = dtModified.ToString("yyyy-MM-dd");
                    ent.TransNo = CONTEXT.Database.SqlQuery<string>(
                        "CALL \"SpSysGetNumbering\" (" + userId.ToString() + ",'IssueAndReceipt','" + dateX + "','') ").SingleOrDefault();

                    CONTEXT.Tx_IssueAndReceipt.Add(ent);
                    CONTEXT.SaveChanges();
                    newId = ent.Id;

                    // ---- Sisi ISSUE: Item -> Batch -> Scale ----
                    var srcIssueItems = CONTEXT.Tx_IssueAndReceipt_Issue_Item
                        .Where(x => x.Id == sourceId).OrderBy(x => x.DetId).ToList();

                    foreach (var si in srcIssueItems)
                    {
                        var ni = new Tx_IssueAndReceipt_Issue_Item();
                        ni.Id = newId;
                        ni.ItemCode = si.ItemCode;
                        ni.ItemName = si.ItemName;
                        ni.Quantity = si.Quantity;
                        ni.Uom = si.Uom;
                        ni.WhsCode = si.WhsCode;
                        ni.MsnPrd = si.MsnPrd;
                        ni.Department = si.Department;
                        ni.LineNum = si.LineNum;
                        ni.LineStatus = si.LineStatus;
                        // Netto & QuantityCreated dihitung ulang dari batch/scale di akhir.
                        ni.Netto = 0;
                        ni.QuantityCreated = 0;
                        // Cost/Value = hasil FillCosting saat Post, dan DocEntry = nomor dokumen SAP.
                        // Sengaja dikosongkan supaya dokumen baru menghitungnya sendiri saat di-Post.
                        ni.CreatedDate = dtModified;
                        ni.CreatedUser = userId;
                        ni.ModifiedDate = dtModified;
                        ni.ModifiedUser = userId;
                        CONTEXT.Tx_IssueAndReceipt_Issue_Item.Add(ni);
                        CONTEXT.SaveChanges();

                        var srcBatches = CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch
                            .Where(x => x.DetId == si.DetId).OrderBy(x => x.DetDetId).ToList();

                        foreach (var sb in srcBatches)
                        {
                            var nb = new Tx_IssueAndReceipt_Issue_Item_Batch();
                            nb.DetId = ni.DetId;
                            nb.Batch = sb.Batch;
                            nb.Quantity = sb.Quantity;
                            nb.AdmissionDate = sb.AdmissionDate;
                            nb.LineNum = sb.LineNum;
                            nb.LineStatus = sb.LineStatus;
                            nb.Netto = 0; // dihitung ulang dari scale
                            nb.CreatedDate = dtModified;
                            nb.CreatedUser = userId;
                            nb.ModifiedDate = dtModified;
                            nb.ModifiedUser = userId;
                            CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch.Add(nb);
                            CONTEXT.SaveChanges();

                            var srcScales = CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch_Scale
                                .Where(x => x.DetDetId == sb.DetDetId).OrderBy(x => x.DetDetDetId).ToList();

                            foreach (var ss in srcScales)
                            {
                                var ns = new Tx_IssueAndReceipt_Issue_Item_Batch_Scale();
                                ns.DetDetId = nb.DetDetId;
                                ns.Quantity = ss.Quantity;
                                ns.Uom = ss.Uom;
                                ns.LineNum = ss.LineNum;
                                ns.LineStatus = ss.LineStatus;
                                // Netto IKUT disalin. Normalnya hanya diisi API hasil timbang, tapi
                                // tanpa Netto dokumen hasil salin akan ditolak validasi Post ("Netto
                                // belum terisi") dan costing-nya jadi 0 -- tidak bisa dipakai sama sekali.
                                ns.Netto = ss.Netto;
                                ns.CreatedDate = dtModified;
                                ns.CreatedUser = userId;
                                ns.ModifiedDate = dtModified;
                                ns.ModifiedUser = userId;
                                CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch_Scale.Add(ns);
                            }
                            CONTEXT.SaveChanges();
                        }
                    }

                    // ---- Sisi RECEIPT: Item -> Batch -> Scale ----
                    var srcReceiptItems = CONTEXT.Tx_IssueAndReceipt_Receipt_Item
                        .Where(x => x.Id == sourceId).OrderBy(x => x.DetId).ToList();

                    foreach (var si in srcReceiptItems)
                    {
                        var ni = new Tx_IssueAndReceipt_Receipt_Item();
                        ni.Id = newId;
                        ni.ItemCode = si.ItemCode;
                        ni.ItemName = si.ItemName;
                        ni.Quantity = si.Quantity;
                        ni.Uom = si.Uom;
                        ni.WhsCode = si.WhsCode;
                        ni.MsnPrd = si.MsnPrd;
                        ni.Department = si.Department;
                        ni.LineNum = si.LineNum;
                        ni.LineStatus = si.LineStatus;
                        ni.Netto = 0;
                        // Price (dan kolom Value/QuantityCreated yang hanya ada di DB) sengaja
                        // dibiarkan kosong -- diisi FillCosting saat Post.
                        ni.CreatedDate = dtModified;
                        ni.CreatedUser = userId;
                        ni.ModifiedDate = dtModified;
                        ni.ModifiedUser = userId;
                        CONTEXT.Tx_IssueAndReceipt_Receipt_Item.Add(ni);
                        CONTEXT.SaveChanges();

                        var srcBatches = CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch
                            .Where(x => x.DetId == si.DetId).OrderBy(x => x.DetDetId).ToList();

                        foreach (var sb in srcBatches)
                        {
                            var nb = new Tx_IssueAndReceipt_Receipt_Item_Batch();
                            nb.DetId = ni.DetId;
                            nb.Batch = sb.Batch;
                            nb.Quantity = sb.Quantity;
                            nb.AdmissionDate = sb.AdmissionDate;
                            nb.LineNum = sb.LineNum;
                            nb.LineStatus = sb.LineStatus;
                            nb.Netto = 0;
                            nb.CreatedDate = dtModified;
                            nb.CreatedUser = userId;
                            nb.ModifiedDate = dtModified;
                            nb.ModifiedUser = userId;
                            CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch.Add(nb);
                            CONTEXT.SaveChanges();

                            var srcScales = CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch_Scale
                                .Where(x => x.DetDetId == sb.DetDetId).OrderBy(x => x.DetDetDetId).ToList();

                            foreach (var ss in srcScales)
                            {
                                var ns = new Tx_IssueAndReceipt_Receipt_Item_Batch_Scale();
                                ns.DetDetId = nb.DetDetId;
                                ns.Quantity = ss.Quantity;
                                ns.Uom = ss.Uom;
                                ns.LineNum = ss.LineNum;
                                ns.LineStatus = ss.LineStatus;
                                ns.Netto = ss.Netto;
                                ns.CreatedDate = dtModified;
                                ns.CreatedUser = userId;
                                ns.ModifiedDate = dtModified;
                                ns.ModifiedUser = userId;
                                CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch_Scale.Add(ns);
                            }
                            CONTEXT.SaveChanges();
                        }
                    }

                    // ---- Rollup: hitung ulang Netto & QuantityCreated dari data yang tersalin ----
                    // Dipakai raw SQL karena kolom "QuantityCreated"/"Value" pada tabel Receipt ada di
                    // DB tapi tidak dipetakan di model EF.
                    CONTEXT.Database.ExecuteSqlCommand(
                        "UPDATE \"Tx_IssueAndReceipt_Issue_Item_Batch\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch_Scale\" WHERE \"DetDetId\" = \"Tx_IssueAndReceipt_Issue_Item_Batch\".\"DetDetId\"), 0) " +
                        "WHERE \"DetId\" IN (SELECT \"DetId\" FROM \"Tx_IssueAndReceipt_Issue_Item\" WHERE \"Id\"=:p0)", newId);
                    CONTEXT.Database.ExecuteSqlCommand(
                        "UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET " +
                        "\"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\" = \"Tx_IssueAndReceipt_Issue_Item\".\"DetId\"), 0), " +
                        "\"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\" = \"Tx_IssueAndReceipt_Issue_Item\".\"DetId\"), 0) " +
                        "WHERE \"Id\"=:p0", newId);

                    CONTEXT.Database.ExecuteSqlCommand(
                        "UPDATE \"Tx_IssueAndReceipt_Receipt_Item_Batch\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch_Scale\" WHERE \"DetDetId\" = \"Tx_IssueAndReceipt_Receipt_Item_Batch\".\"DetDetId\"), 0) " +
                        "WHERE \"DetId\" IN (SELECT \"DetId\" FROM \"Tx_IssueAndReceipt_Receipt_Item\" WHERE \"Id\"=:p0)", newId);
                    CONTEXT.Database.ExecuteSqlCommand(
                        "UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET " +
                        "\"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\" = \"Tx_IssueAndReceipt_Receipt_Item\".\"DetId\"), 0), " +
                        "\"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\" = \"Tx_IssueAndReceipt_Receipt_Item\".\"DetId\"), 0) " +
                        "WHERE \"Id\"=:p0", newId);

                    CONTEXT_TRANS.Commit();
                }
                catch (Exception ex)
                {
                    CONTEXT_TRANS.Rollback();
                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
            }

            return newId;
        }

        public void Update(ReProcessModel model, string method = "")
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_IssueAndReceipt ent = CONTEXT.Tx_IssueAndReceipt.Find(model.Id);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (ent != null)
                        {
                            var exceptColumns = new string[] { "Id", "TransNo", "TransType", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, ent, false, exceptColumns);

                            ent.ModifiedDate = dtModified;
                            ent.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT_TRANS.Commit();
                    }
                    catch (Exception ex)
                    {
                        CONTEXT_TRANS.Rollback();
                        throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                    }
                }

                // Simpan perubahan grid detail (edit/tambah/hapus) yang dikirim bersama form Update.
                ApplyIssueDetailChanges(model);
                ApplyReceiptDetailChanges(model);
            }
        }

        // Terapkan perubahan batch grid Issue: modified -> update, inserted -> add, deleted -> delete.
        private void ApplyIssueDetailChanges(ReProcessModel model)
        {
            var d = model.IssueDetails_;
            if (d == null) return;
            if (d.modifiedRowValues != null)
                foreach (var it in d.modifiedRowValues) { it._UserId = model._UserId; IssueItem_UpdateQuantity(it); }
            if (d.insertedRowValues != null)
                foreach (var it in d.insertedRowValues) { it._UserId = model._UserId; it.Id = model.Id; IssueItem_Add(it); }
            if (d.deletedRowKeys != null)
                foreach (var detId in d.deletedRowKeys) IssueItem_Delete(model._UserId, detId);
        }

        private void ApplyReceiptDetailChanges(ReProcessModel model)
        {
            var d = model.ReceiptDetails_;
            if (d == null) return;
            if (d.modifiedRowValues != null)
                foreach (var it in d.modifiedRowValues) { it._UserId = model._UserId; ReceiptItem_UpdateQuantity(it); }
            if (d.insertedRowValues != null)
                foreach (var it in d.insertedRowValues) { it._UserId = model._UserId; it.Id = model.Id; ReceiptItem_Add(it); }
            if (d.deletedRowKeys != null)
                foreach (var detId in d.deletedRowKeys) ReceiptItem_Delete(model._UserId, detId);
        }

        public ReProcessModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public ReProcessModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            ReProcessModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_IssueAndReceipt"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<ReProcessModel>(ssql, id).Single();

                model.ListIssueItem_ = this.ReProcess_IssueItemDetails(CONTEXT, id);

                model.ListIssueItem_ = this.ReProcess_IssueBatchDetails(CONTEXT, model.ListIssueItem_);

                model.ListReceiptItem_ = this.ReProcess_ReceiptItemDetails(CONTEXT, id);

                model.ListReceiptItem_ = this.ReProcess_ReceiptBatchDetails(CONTEXT, model.ListReceiptItem_);




                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'IssueAndReceipt', :p1) ", userId, model.Id).FirstOrDefault();
                }
            }

            return model;
        }

        public List<IssueReceipt_IssueItemModel> ReProcess_IssueItemDetails(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ReProcess_IssueItemDetails(CONTEXT, id);
            }

        }

        public List<IssueReceipt_IssueItemModel> ReProcess_IssueItemDetails(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetId"" ASC) AS ""RowNo"", T0.*
                FROM ""Tx_IssueAndReceipt_Issue_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var ReProcess_IssueItemDetails = CONTEXT.Database.SqlQuery<IssueReceipt_IssueItemModel>(ssql, id).ToList();
            return ReProcess_IssueItemDetails;
        }

        public List<IssueReceipt_IssueItemModel> ReProcess_IssueBatchDetails(HANA_APP CONTEXT,List<IssueReceipt_IssueItemModel> items)
        {
            if (items == null || !items.Any())
                return items;

            var detIds = items
                .Select(x => x.DetId)
                .Distinct()
                .ToList();

            string ssql = $@"
                            SELECT T0.*
                            FROM ""Tx_IssueAndReceipt_Issue_Item_Batch"" T0
                            WHERE T0.""DetId"" IN ({string.Join(",", detIds)})
                            ORDER BY T0.""DetDetId"" ASC
                        ";

            var batches = CONTEXT.Database
                .SqlQuery<IssueReceipt_IssueBatchModel>(ssql)
                .ToList();

            foreach (var item in items)
            {
                item.ListBatch_ = batches
                    .Where(x => x.DetId == item.DetId)
                    .ToList();
            }

            return items;
        }


        public List<IssueReceipt_ReceiptItemModel> ReProcess_ReceiptItemDetails(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ReProcess_ReceiptItemDetails(CONTEXT, id);
            }

        }

        public List<IssueReceipt_ReceiptItemModel> ReProcess_ReceiptItemDetails(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetId"" ASC) AS ""RowNo"", T0.*
                FROM ""Tx_IssueAndReceipt_Receipt_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var ReProcess_ReceiptItemDetails = CONTEXT.Database.SqlQuery<IssueReceipt_ReceiptItemModel>(ssql, id).ToList();
            return ReProcess_ReceiptItemDetails;
        }

        public List<IssueReceipt_ReceiptItemModel> ReProcess_ReceiptBatchDetails(HANA_APP CONTEXT, List<IssueReceipt_ReceiptItemModel> items)
        {
            if (items == null || !items.Any())
                return items;

            var detIds = items
                .Select(x => x.DetId)
                .Distinct()
                .ToList();

            string ssql = $@"
                            SELECT T0.*
                            FROM ""Tx_IssueAndReceipt_Receipt_Item_Batch"" T0
                            WHERE T0.""DetId"" IN ({string.Join(",", detIds)})
                            ORDER BY T0.""DetDetId"" ASC
                        ";

            var batches = CONTEXT.Database
                .SqlQuery<IssueReceipt_ReceiptBatchModel>(ssql)
                .ToList();

            foreach (var item in items)
            {
                item.ListBatch_ = batches
                    .Where(x => x.DetId == item.DetId)
                    .ToList();
            }

            return items;
        }
        public ReProcessModel NavFirst(int userId)
        {
            ReProcessModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "IssueAndReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_IssueAndReceipt\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public ReProcessModel NavPrevious(int userId, long id = 0)
        {
            ReProcessModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "IssueAndReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_IssueAndReceipt\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public ReProcessModel NavNext(int userId, long id = 0)
        {
            ReProcessModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "IssueAndReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_IssueAndReceipt\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public ReProcessModel NavLast(int userId)
        {
            ReProcessModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "IssueAndReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_IssueAndReceipt\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public ReProcessBatchReceiptView___ GetReceiptBatch(long id, long detId)
        {
            string sql = null;
            ReProcessBatchReceiptView___ model = new ReProcessBatchReceiptView___();

            using (var CONTEXT = new HANA_APP())
            {
                // Standalone: "No" pada popup batch memakai TransNo dokumen (BaseDocNum lama = No Production Order, kini kosong).
                sql = @"SELECT T0.""Id"",
                                T0.""TransNo"" AS ""BaseDocNum"",
                                T1.""DetId"",
                                T1.""ItemCode"",
                                T1.""ItemName"",
                                T1.""WhsCode"",
                                T1.""Quantity"",
                                T1.""Quantity"" AS ""TotalNeeded"",
                                T1.""QuantityCreated"" AS ""TotalCreated""
                                FROM ""Tx_IssueAndReceipt"" T0
                                LEFT JOIN ""Tx_IssueAndReceipt_Receipt_Item"" T1 ON T0.""Id"" = T1.""Id""
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<ReProcessBatchReceiptView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_IssueAndReceipt_Receipt_Item_Batch"" T0   
                            WHERE ""DetId"" = :p0 ";

                model.ReProcessBatchReceiptModel___ = CONTEXT.Database.SqlQuery<ReProcessBatchReceiptModel>(sql, detId).ToList();
            }

            return model;
        }

        public ReProcessBatchIssueView___ GetIssueBatch(long id, long detId)
        {
            string sql = null;
            ReProcessBatchIssueView___ model = new ReProcessBatchIssueView___();

            using (var CONTEXT = new HANA_APP())
            {
                // Standalone: "No" pada popup batch memakai TransNo dokumen (BaseDocNum lama = No Production Order, kini kosong).
                sql = @"SELECT T0.""Id"",
                                T0.""TransNo"" AS ""BaseDocNum"",
                                T1.""DetId"",
                                T1.""ItemCode"",
                                T1.""ItemName"",
                                T1.""WhsCode"",
                                T1.""Quantity"",
                                T1.""Quantity"" AS ""TotalNeeded"",
                                T1.""QuantityCreated"" AS ""TotalCreated""
                                FROM ""Tx_IssueAndReceipt"" T0
                                LEFT JOIN ""Tx_IssueAndReceipt_Issue_Item"" T1 ON T0.""Id"" = T1.""Id""
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<ReProcessBatchIssueView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_IssueAndReceipt_Issue_Item_Batch"" T0   
                            WHERE ""DetId"" = :p0 ";

                model.ReProcessBatchIssueModel___ = CONTEXT.Database.SqlQuery<ReProcessBatchIssueModel>(sql, detId).ToList();
            }

            return model;
        }

        public List<ReProcessBatchReceiptModel> ReProcess__ReceiptItemBatchList(long detId)
        {
            string sql = null;
            List<ReProcessBatchReceiptModel> model = new List<ReProcessBatchReceiptModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_IssueAndReceipt_Receipt_Item_Batch"" T0
                            WHERE ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<ReProcessBatchReceiptModel>(sql, detId).ToList();
            }
            return model;
        }

        // ItemCode dari baris item Issue berdasarkan DetId — dipakai grid batch popup agar tetap tahu
        // item-nya walau render via callback (tanpa bergantung ViewBag).
        public string GetIssueItemCode(long detId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CONTEXT.Database.SqlQuery<string>(
                    "SELECT T0.\"ItemCode\" FROM \"Tx_IssueAndReceipt_Issue_Item\" T0 WHERE T0.\"DetId\"=:p0", detId).FirstOrDefault();
            }
        }

        // Baris batch OBTN + stok per gudang (OBTQ) untuk ComboBox kolom Batch.
        private class ReProcess_ObtnBatchRow
        {
            public string DistNumber { get; set; }
            public decimal? Quantity { get; set; }
        }

        // ItemCode + WhsCode baris item Issue — WhsCode dipakai memfilter stok batch per gudang.
        public string GetIssueItemWhsCode(long detId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CONTEXT.Database.SqlQuery<string>(
                    "SELECT T0.\"WhsCode\" FROM \"Tx_IssueAndReceipt_Issue_Item\" T0 WHERE T0.\"DetId\"=:p0", detId).FirstOrDefault();
            }
        }

        // Daftar batch SAP (OBTN) untuk sebuah item — dipakai ComboBox kolom Batch pada batch popup.
        // Hanya batch yang BELUM dipilih (belum ada di Tx_IssueAndReceipt_Issue_Item_Batch utk DetId ini).
        //
        // Mengembalikan DataTable 2 kolom:
        //   "Value"   -> DistNumber murni; INI yang tersimpan ke kolom Batch (ValueField).
        //   "Display" -> "DistNumber - Qty" untuk ditampilkan di dropdown (TextField).
        // Dipisah supaya teks tampilan TIDAK ikut tersimpan sebagai nomor batch.
        //
        // Qty diambil dari OBTQ (stok batch per gudang) difilter ItemCode + WhsCode + DistNumber.
        // LEFT JOIN dipakai agar batch yang tidak punya baris stok di gudang itu tetap muncul
        // dengan qty 0 (bukan hilang dari daftar).
        public System.Data.DataTable ReProcess_OBTNBatches(string itemCode, long detId, string whsCode)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Display", typeof(string));

            if (string.IsNullOrEmpty(itemCode))
            {
                return dt;
            }

            using (var CONTEXT = new HANA_APP())
            {
                string dbSap = DbProvider.dbSap_Name;
                string ssql =
                    "SELECT T0.\"DistNumber\" AS \"DistNumber\", " +
                    "       IFNULL(SUM(T2.\"Quantity\"), 0) AS \"Quantity\" " +
                    "FROM \"" + dbSap + "\".\"OBTN\" T0 " +
                    "LEFT JOIN \"" + dbSap + "\".\"OBTQ\" T2 " +
                    "       ON T2.\"SysNumber\" = T0.\"SysNumber\" " +
                    "      AND T2.\"ItemCode\"  = T0.\"ItemCode\" " +
                    "      AND T2.\"WhsCode\"   = :p2 " +
                    "WHERE T0.\"ItemCode\"=:p0 AND IFNULL(T0.\"Status\",'0')='0' " +
                    "AND T0.\"DistNumber\" NOT IN (" +
                    "  SELECT T1.\"Batch\" FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" T1 " +
                    "  WHERE T1.\"DetId\"=:p1 AND T1.\"Batch\" IS NOT NULL) " +
                    "GROUP BY T0.\"DistNumber\" ORDER BY T0.\"DistNumber\"";

                var rows = CONTEXT.Database.SqlQuery<ReProcess_ObtnBatchRow>(ssql, itemCode, detId, whsCode ?? "").ToList();
                foreach (var r in rows)
                {
                    decimal qty = r.Quantity ?? 0;
                    // Qty batch berupa bilangan bulat di layar (mengikuti format kolom Quantity grid).
                    dt.Rows.Add(r.DistNumber, string.Format("{0} - {1}", r.DistNumber, qty.ToString("0.##")));
                }
            }
            return dt;
        }

        public List<ReProcessBatchIssueModel> ReProcess__IssueItemBatchList(long detId)
        {
            string sql = null;
            List<ReProcessBatchIssueModel> model = new List<ReProcessBatchIssueModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_IssueAndReceipt_Issue_Item_Batch"" T0
                            WHERE ""DetId"" = :p0 ";

                model = CONTEXT.Database.SqlQuery<ReProcessBatchIssueModel>(sql, detId).ToList();
            }
            return model;
        }

        public long ReProcess__ReceiptAddNewItemBatch(ReProcessBatchReceiptModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_IssueAndReceipt_Receipt_Item_Batch tx_IssueAndReceipt_Receipt_Item_Batch = new Tx_IssueAndReceipt_Receipt_Item_Batch();
                        CopyProperty.CopyProperties(model, tx_IssueAndReceipt_Receipt_Item_Batch, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_IssueAndReceipt_Receipt_Item_Batch.CreatedDate = dtModified;
                        tx_IssueAndReceipt_Receipt_Item_Batch.CreatedUser = model._UserId;
                        tx_IssueAndReceipt_Receipt_Item_Batch.ModifiedDate = dtModified;
                        tx_IssueAndReceipt_Receipt_Item_Batch.ModifiedUser = model._UserId;

                        // LineNum batch: lanjut dari batch terakhir pada item ini (mulai 1).
                        // Di-set ke entity (bukan model) karena model.LineNum bertipe long? sedangkan
                        // kolom entity int? -- CopyProperties tidak mengonversi tipe.
                        int? maxLineNum = CONTEXT.Database.SqlQuery<int?>("SELECT MAX(\"LineNum\") AS IDU FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0", model.DetId).FirstOrDefault();
                        tx_IssueAndReceipt_Receipt_Item_Batch.LineNum = (maxLineNum ?? 0) + 1;

                        CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch.Add(tx_IssueAndReceipt_Receipt_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_IssueAndReceipt_Receipt_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_IssueAndReceipt_Receipt_Item_Batch.DetId.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'Tx_IssueAndReceipt_Issue_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                        // Netto/Value/QuantityCreated item RECEIPT dihitung ulang dari batch-nya.
                        CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0), 0), \"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p1), 0) WHERE \"DetId\"=:p2", model.DetId, model.DetId, model.DetId);
                       // SpNotif.SpSysControllerTransNotif(model._UserId, "IssueAndReceipt", CONTEXT, "after", "IssueAndReceipt", "addItemBatch", "Id", keyValue);

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

        public long ReProcess__IssueAddNewItemBatch(ReProcessBatchIssueModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_IssueAndReceipt_Issue_Item_Batch tx_IssueAndReceipt_issue_Item_Batch = new Tx_IssueAndReceipt_Issue_Item_Batch();
                        CopyProperty.CopyProperties(model, tx_IssueAndReceipt_issue_Item_Batch, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_IssueAndReceipt_issue_Item_Batch.CreatedDate = dtModified;
                        tx_IssueAndReceipt_issue_Item_Batch.CreatedUser = model._UserId;
                        tx_IssueAndReceipt_issue_Item_Batch.ModifiedDate = dtModified;
                        tx_IssueAndReceipt_issue_Item_Batch.ModifiedUser = model._UserId;

                        // LineNum batch: lanjut dari batch terakhir pada item ini (mulai 1).
                        // Di-set ke entity (bukan model) karena model.LineNum bertipe long? sedangkan
                        // kolom entity int? -- CopyProperties tidak mengonversi tipe.
                        int? maxLineNum = CONTEXT.Database.SqlQuery<int?>("SELECT MAX(\"LineNum\") AS IDU FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0", model.DetId).FirstOrDefault();
                        tx_IssueAndReceipt_issue_Item_Batch.LineNum = (maxLineNum ?? 0) + 1;

                        CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch.Add(tx_IssueAndReceipt_issue_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_IssueAndReceipt_issue_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_IssueAndReceipt_issue_Item_Batch.DetId.ToString();

                        // Hitung ulang Netto item ISSUE dari batch-nya.
                        // (SP SpIssueAndReceipt_UpdateReceiptItemQuantity hanya melayani sisi Receipt --
                        //  bila dipakai utk Issue, ia malah menimpa Netto item Receipt ber-DetId sama.)
                        CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0), 0), \"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p1), 0) WHERE \"DetId\"=:p2", model.DetId, model.DetId, model.DetId);
                        // SpNotif.SpSysControllerTransNotif(model._UserId, "IssueAndReceipt", CONTEXT, "after", "IssueAndReceipt", "addItemBatch", "Id", keyValue);

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


        public void ReProcess_ReceiptUpdateItemBatch(ReProcessBatchReceiptModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.DetId.ToString();

                        // SpNotif.SpSysControllerTransNotif(model._UserId, "IssueAndReceipt", CONTEXT, "before", "IssueAndReceipt", "updateItemBatch", "Id", keyValue);

                        Tx_IssueAndReceipt_Receipt_Item_Batch tx_IssueAndReceipt_Receipt_Item_Batch = CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_IssueAndReceipt_Receipt_Item_Batch != null)
                        {
                            // LineNum & Netto TIDAK boleh disalin dari grid:
                            //  - LineNum di-generate server-side saat Add dan tidak ada di grid batch,
                            //    jadi DevExpress mengirimnya null -> bila disalin, LineNum jadi NULL dan
                            //    header "Line Num" pada popup Scale kehilangan nilainya.
                            //  - Netto batch dihitung dari SUM(Netto baris scale) via UpdateBatchNettoFromScale;
                            //    kolomnya read-only di grid sehingga juga terkirim null.
                            var exceptColumns = new string[] { "DetId", "DetDetId", "LineNum", "Netto", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_IssueAndReceipt_Receipt_Item_Batch, false, exceptColumns);

                            tx_IssueAndReceipt_Receipt_Item_Batch.ModifiedDate = dtModified;
                            tx_IssueAndReceipt_Receipt_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'tx_IssueAndReceipt_Receipt_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                            // Netto/Value/QuantityCreated item RECEIPT dihitung ulang dari batch-nya.
                            CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0), 0), \"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p1), 0) WHERE \"DetId\"=:p2", model.DetId, model.DetId, model.DetId);

                            //SpNotif.SpSysControllerTransNotif(model._UserId, "IssueAndReceipt", CONTEXT, "after", "IssueAndReceipt", "updateItemBatch", "Id", keyValue);

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

        public void ReProcess_IssueUpdateItemBatch(ReProcessBatchIssueModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.DetId.ToString();

                        // SpNotif.SpSysControllerTransNotif(model._UserId, "IssueAndReceipt", CONTEXT, "before", "IssueAndReceipt", "updateItemBatch", "Id", keyValue);

                        Tx_IssueAndReceipt_Issue_Item_Batch tx_IssueAndReceipt_Issue_Item_Batch = CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_IssueAndReceipt_Issue_Item_Batch != null)
                        {
                            // LineNum & Netto TIDAK boleh disalin dari grid:
                            //  - LineNum di-generate server-side saat Add dan tidak ada di grid batch,
                            //    jadi DevExpress mengirimnya null -> bila disalin, LineNum jadi NULL dan
                            //    header "Line Num" pada popup Scale kehilangan nilainya.
                            //  - Netto batch dihitung dari SUM(Netto baris scale) via UpdateBatchNettoFromScale;
                            //    kolomnya read-only di grid sehingga juga terkirim null.
                            var exceptColumns = new string[] { "DetId", "DetDetId", "LineNum", "Netto", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_IssueAndReceipt_Issue_Item_Batch, false, exceptColumns);

                            tx_IssueAndReceipt_Issue_Item_Batch.ModifiedDate = dtModified;
                            tx_IssueAndReceipt_Issue_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            // Hitung ulang Netto item ISSUE dari batch-nya.
                        // (SP SpIssueAndReceipt_UpdateReceiptItemQuantity hanya melayani sisi Receipt --
                        //  bila dipakai utk Issue, ia malah menimpa Netto item Receipt ber-DetId sama.)
                        CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0), 0), \"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p1), 0) WHERE \"DetId\"=:p2", model.DetId, model.DetId, model.DetId);

                            //SpNotif.SpSysControllerTransNotif(model._UserId, "IssueAndReceipt", CONTEXT, "after", "IssueAndReceipt", "updateItemBatch", "Id", keyValue);

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


        public void ReProcess_ReceiptDeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
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

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetDetId\"=:p0 AND \"TransType\"='IssueAndReceiptReceipt'", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            // Hitung ulang Netto item RECEIPT dari batch tersisa.
                            // (Sebelumnya memanggil SP dgn nama tabel yang tidak ada: 'Tx_IssueAndReceipt_Item_Batch'.)
                            CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0), 0), \"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p1), 0) WHERE \"DetId\"=:p2", DetId, DetId, DetId);
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

        public void ReProcess_IssueDeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
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

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetDetId\"=:p0 AND \"TransType\"='IssueAndReceiptIssue'", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            // Hitung ulang Netto item ISSUE dari batch tersisa.
                            // (SP "SpIssueAndReceipt_UpdateIssueItemQuantity" TIDAK ADA di database --
                            //  inilah penyebab error saat delete. SP yang ada hanya versi Receipt.)
                            CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0), 0), \"QuantityCreated\" = COALESCE((SELECT SUM(\"Quantity\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p1), 0) WHERE \"DetId\"=:p2", DetId, DetId, DetId);
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

        #region Scale (Timbangan) — dipakai bersama Issue & Receipt via parameter `side`

        // Sanitasi `side` ke {"Issue","Receipt"} — dipakai untuk membangun nama tabel (aman dari injeksi).
        private static string ScaleSide(string side)
        {
            return (side == "Receipt") ? "Receipt" : "Issue";
        }

        public ReProcessScaleView___ GetScale(long id, long detId, long detDetId, string side)
        {
            string s = ScaleSide(side);
            string tblItem = "Tx_IssueAndReceipt_" + s + "_Item";
            string tblBatch = "Tx_IssueAndReceipt_" + s + "_Item_Batch";
            string tblScale = "Tx_IssueAndReceipt_" + s + "_Item_Batch_Scale";

            string sql = null;
            ReProcessScaleView___ model = new ReProcessScaleView___();

            using (var CONTEXT = new HANA_APP())
            {
                // "LineNum" di-COALESCE dengan nomor urut batch (berdasarkan DetDetId, sama seperti
                // penomoran saat batch dibuat). Sebagian data lama ber-LineNum NULL karena dulu
                // tertimpa saat baris batch diedit, dan header popup ini jadi kosong -- fallback
                // membuatnya tetap tampil tanpa menunggu perbaikan data.
                sql = string.Format(@"SELECT T0.""Id"",
                                T1.""DetId"",
                                T2.""DetDetId"",
                                T1.""ItemCode"",
                                T1.""ItemName"",
                                T1.""Uom"",
                                CAST(COALESCE(T2.""LineNum"", T3.""LineNumUrut"") AS INTEGER) AS ""LineNum"",
                                T2.""Batch""
                                FROM ""Tx_IssueAndReceipt"" T0
                                LEFT JOIN ""{0}"" T1 ON T0.""Id"" = T1.""Id""
                                LEFT JOIN ""{1}"" T2 ON T1.""DetId"" = T2.""DetId""
                                LEFT JOIN (SELECT ""DetDetId"", ""DetId"",
                                                  ROW_NUMBER() OVER (PARTITION BY ""DetId"" ORDER BY ""DetDetId"") AS ""LineNumUrut""
                                             FROM ""{1}"") T3 ON T3.""DetDetId"" = T2.""DetDetId""
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 AND T2.""DetDetId"" = :p2 ", tblItem, tblBatch);

                model = CONTEXT.Database.SqlQuery<ReProcessScaleView___>(sql, id, detId, detDetId).FirstOrDefault();

                if (model == null)
                {
                    model = new ReProcessScaleView___();
                    model.Id = id;
                    model.DetId = detId;
                    model.DetDetId = detDetId;
                }

                // Status permintaan timbang terbaru per baris scale dari Tp_ScaleStaging (per TransType/side).
                // Ambil status baris ber-StagingId terbesar (HANA tak izinkan ORDER BY/LIMIT di subquery korelasi).
                string transType = (s == "Receipt") ? "IssueAndReceiptReceipt" : "IssueAndReceiptIssue";
                sql = string.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY ""LineNum"", ""DetDetDetId"") AS ""RowNo"", T0.*,
                            (SELECT ST.""Status"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = :p0
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = :p1)
                            ) AS ""StagingStatus"",
                            (SELECT ST.""RequestId"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = :p3
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = :p4)
                            ) AS ""StagingRequestId""
                            FROM ""{0}"" T0
                            WHERE T0.""DetDetId"" = :p2 ", tblScale);

                model.ReProcessScaleModel___ = CONTEXT.Database.SqlQuery<ReProcessScaleModel>(sql, transType, transType, detDetId, transType, transType).ToList();
            }

            // "Line Num" = nilai LineNum batch, disajikan string (nested TextBox tak me-render numerik).
            model.LineNumText = model.LineNum.HasValue ? model.LineNum.Value.ToString() : "";
            model.DetDetIdText = model.DetDetId.ToString();

            return model;
        }

        public List<ReProcessScaleModel> ReProcess__ItemBatchScaleList(long detDetId, string side)
        {
            string s = ScaleSide(side);
            string tblScale = "Tx_IssueAndReceipt_" + s + "_Item_Batch_Scale";
            string transType = (s == "Receipt") ? "IssueAndReceiptReceipt" : "IssueAndReceiptIssue";

            string sql = null;
            List<ReProcessScaleModel> model = new List<ReProcessScaleModel>();

            using (var CONTEXT = new HANA_APP())
            {
                // Sertakan status permintaan timbang terbaru per baris scale dari Tp_ScaleStaging.
                // Ambil status baris ber-StagingId terbesar (HANA tak izinkan ORDER BY/LIMIT di subquery korelasi).
                sql = string.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY ""LineNum"", ""DetDetDetId"") AS ""RowNo"", T0.*,
                            (SELECT ST.""Status"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = :p0
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = :p1)
                            ) AS ""StagingStatus"",
                            (SELECT ST.""RequestId"" FROM ""Tp_ScaleStaging"" ST
                             WHERE ST.""DetDetDetId"" = T0.""DetDetDetId"" AND ST.""TransType"" = :p3
                               AND ST.""StagingId"" = (SELECT MAX(ST2.""StagingId"") FROM ""Tp_ScaleStaging"" ST2
                                                       WHERE ST2.""DetDetDetId"" = T0.""DetDetDetId"" AND ST2.""TransType"" = :p4)
                            ) AS ""StagingRequestId""
                            FROM ""{0}"" T0
                            WHERE T0.""DetDetId"" = :p2 ", tblScale);

                model = CONTEXT.Database.SqlQuery<ReProcessScaleModel>(sql, transType, transType, detDetId, transType, transType).ToList();
            }
            return model;
        }

        public long ReProcess_AddNewItemBatchScale(ReProcessScaleModel model, string side)
        {
            string s = ScaleSide(side);
            string tblBatch = "Tx_IssueAndReceipt_" + s + "_Item_Batch";
            string tblItem = "Tx_IssueAndReceipt_" + s + "_Item";
            string tblScale = "Tx_IssueAndReceipt_" + s + "_Item_Batch_Scale";

            long detDetDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        // Sequence otomatis: lanjut dari LineNum terakhir pada batch ini (mulai dari 1).
                        int? maxLineNum = CONTEXT.Database.SqlQuery<int?>(string.Format("SELECT MAX(\"LineNum\") AS IDU FROM \"{0}\" WHERE \"DetDetId\"=:p0", tblScale), model.DetDetId).FirstOrDefault();
                        int nextLineNum = (maxLineNum ?? 0) + 1;

                        // Uom baris scale SELALU mengikuti Uom item induk, bukan input user
                        // (kolomnya read-only di grid).
                        string uom = CONTEXT.Database.SqlQuery<string>(string.Format(@"SELECT T1.""Uom"" AS IDU
                                    FROM ""{0}"" T0
                                    INNER JOIN ""{1}"" T1 ON T0.""DetId"" = T1.""DetId""
                                    WHERE T0.""DetDetId""=:p0 ", tblBatch, tblItem), model.DetDetId).FirstOrDefault();

                        string lineStatus = string.IsNullOrEmpty(model.LineStatus) ? "O" : model.LineStatus;

                        // Branch side untuk entity yang tepat (PK DetDetDetId identity — tidak disalin).
                        var exceptColumns = new string[] { "DetDetDetId" };
                        if (s == "Receipt")
                        {
                            Tx_IssueAndReceipt_Receipt_Item_Batch_Scale tx_Scale = new Tx_IssueAndReceipt_Receipt_Item_Batch_Scale();
                            CopyProperty.CopyProperties(model, tx_Scale, false, exceptColumns);
                            tx_Scale.LineNum = nextLineNum;
                            tx_Scale.Uom = uom;
                            tx_Scale.LineStatus = lineStatus;
                            tx_Scale.CreatedDate = dtModified;
                            tx_Scale.CreatedUser = model._UserId;
                            tx_Scale.ModifiedDate = dtModified;
                            tx_Scale.ModifiedUser = model._UserId;
                            CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch_Scale.Add(tx_Scale);
                            CONTEXT.SaveChanges();
                            detDetDetId = tx_Scale.DetDetDetId;
                        }
                        else
                        {
                            Tx_IssueAndReceipt_Issue_Item_Batch_Scale tx_Scale = new Tx_IssueAndReceipt_Issue_Item_Batch_Scale();
                            CopyProperty.CopyProperties(model, tx_Scale, false, exceptColumns);
                            tx_Scale.LineNum = nextLineNum;
                            tx_Scale.Uom = uom;
                            tx_Scale.LineStatus = lineStatus;
                            tx_Scale.CreatedDate = dtModified;
                            tx_Scale.CreatedUser = model._UserId;
                            tx_Scale.ModifiedDate = dtModified;
                            tx_Scale.ModifiedUser = model._UserId;
                            CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch_Scale.Add(tx_Scale);
                            CONTEXT.SaveChanges();
                            detDetDetId = tx_Scale.DetDetDetId;
                        }

                        UpdateBatchNettoFromScale(CONTEXT, model._UserId, model.DetDetId ?? 0, s);

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

        public void ReProcess_UpdateItemBatchScale(ReProcessScaleModel model, string side)
        {
            string s = ScaleSide(side);

            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        // LineNum (Sequence) di-generate server-side, jangan ditimpa dari grid.
                        // Uom juga dikecualikan: nilainya selalu diturunkan dari Uom item induk, dan
                        // karena kolomnya read-only DevExpress mengirimnya null -- bila disalin, Uom jadi NULL.
                        var exceptColumns = new string[] { "DetDetId", "DetDetDetId", "LineNum", "CreatedUser", "CreatedDate", "Uom" };
                        long? detDetId = null;

                        if (s == "Receipt")
                        {
                            Tx_IssueAndReceipt_Receipt_Item_Batch_Scale tx_Scale = CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch_Scale.Find(model.DetDetDetId ?? 0);
                            if (tx_Scale != null)
                            {
                                CopyProperty.CopyProperties(model, tx_Scale, false, exceptColumns);
                                tx_Scale.ModifiedDate = dtModified;
                                tx_Scale.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();
                                detDetId = tx_Scale.DetDetId;
                            }
                        }
                        else
                        {
                            Tx_IssueAndReceipt_Issue_Item_Batch_Scale tx_Scale = CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch_Scale.Find(model.DetDetDetId ?? 0);
                            if (tx_Scale != null)
                            {
                                CopyProperty.CopyProperties(model, tx_Scale, false, exceptColumns);
                                tx_Scale.ModifiedDate = dtModified;
                                tx_Scale.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();
                                detDetId = tx_Scale.DetDetId;
                            }
                        }

                        if (detDetId.HasValue)
                        {
                            UpdateBatchNettoFromScale(CONTEXT, model._UserId, detDetId.Value, s);
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

        public void ReProcess_DeleteItemBatchScale(int _userId, long DetDetId, long DetDetDetId, string side)
        {
            string s = ScaleSide(side);
            string tblScale = "Tx_IssueAndReceipt_" + s + "_Item_Batch_Scale";

            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetDetId != 0)
                    {
                        try
                        {
                            CONTEXT.Database.ExecuteSqlCommand(string.Format("DELETE FROM \"{0}\"  WHERE \"DetDetDetId\"=:p0", tblScale), DetDetDetId);
                            // Hapus juga permintaan timbang terkait di staging (per TransType/side).
                            string transType = (s == "Receipt") ? "IssueAndReceiptReceipt" : "IssueAndReceiptIssue";
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetDetDetId\"=:p0 AND \"TransType\"=:p1", DetDetDetId, transType);
                            CONTEXT.SaveChanges();

                            UpdateBatchNettoFromScale(CONTEXT, _userId, DetDetId, s);

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

        // Netto batch = SUM(Netto scale) miliknya; lalu Netto item = SUM(Netto batch).
        // QuantityCreated TIDAK diubah oleh scale (hanya dipengaruhi Quantity batch).
        private void UpdateBatchNettoFromScale(HANA_APP CONTEXT, int _userId, long detDetId, string side)
        {
            if (detDetId == 0)
            {
                return;
            }

            string s = ScaleSide(side);
            string tblBatch = "Tx_IssueAndReceipt_" + s + "_Item_Batch";
            string tblItem = "Tx_IssueAndReceipt_" + s + "_Item";
            string tblScale = "Tx_IssueAndReceipt_" + s + "_Item_Batch_Scale";

            CONTEXT.Database.ExecuteSqlCommand(
                string.Format(@"UPDATE ""{0}""
                    SET ""Netto"" = COALESCE((SELECT SUM(""Netto"") FROM ""{1}"" WHERE ""DetDetId""=:p0), 0)
                    WHERE ""DetDetId""=:p1 ", tblBatch, tblScale), detDetId, detDetId);

            // Segarkan Netto di level item (QuantityCreated dibiarkan apa adanya).
            long? detId = CONTEXT.Database.SqlQuery<long?>(string.Format("SELECT \"DetId\" AS IDU FROM \"{0}\" WHERE \"DetDetId\"=:p0", tblBatch), detDetId).FirstOrDefault();
            if (detId.HasValue)
            {
                CONTEXT.Database.ExecuteSqlCommand(
                    string.Format(@"UPDATE ""{0}"" SET ""Netto"" = COALESCE((SELECT SUM(""Netto"") FROM ""{1}"" WHERE ""DetId""=:p0), 0) WHERE ""DetId""=:p1", tblItem, tblBatch),
                    detId.Value, detId.Value);
            }
        }

        #endregion

        #region CRUD baris ITEM (standalone: item ditambah user via CFL)

        // Tambah baris item ISSUE dari CFL (item master + stok). Netto=0, batch diisi menyusul.
        public long IssueItem_Add(IssueReceipt_IssueItemModel model)
        {
            long detId = 0;
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    int nextLine = (CONTEXT.Database.SqlQuery<int?>("SELECT MAX(\"LineNum\") AS IDU FROM \"Tx_IssueAndReceipt_Issue_Item\" WHERE \"Id\"=:p0", model.Id).FirstOrDefault() ?? -1) + 1;

                    var ent = new Tx_IssueAndReceipt_Issue_Item();
                    ent.Id = (long)model.Id;
                    ent.ItemCode = model.ItemCode;
                    ent.ItemName = model.ItemName;
                    ent.Quantity = (int)(model.Quantity ?? 0);
                    ent.Uom = model.Uom;
                    ent.WhsCode = model.WhsCode;
                    ent.Netto = 0;
                    ent.LineNum = nextLine;
                    ent.LineStatus = "O";
                    ent.CreatedDate = dtModified;
                    ent.CreatedUser = model._UserId;
                    ent.ModifiedDate = dtModified;
                    ent.ModifiedUser = model._UserId;

                    CONTEXT.Tx_IssueAndReceipt_Issue_Item.Add(ent);
                    CONTEXT.SaveChanges();
                    detId = ent.DetId;

                    CONTEXT_TRANS.Commit();
                }
                catch (Exception ex)
                {
                    CONTEXT_TRANS.Rollback();
                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
            }
            return detId;
        }

        // Tambah baris item RECEIPT dari CFL (barang jadi + gudang tujuan).
        public long ReceiptItem_Add(IssueReceipt_ReceiptItemModel model)
        {
            long detId = 0;
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    int nextLine = (CONTEXT.Database.SqlQuery<int?>("SELECT MAX(\"LineNum\") AS IDU FROM \"Tx_IssueAndReceipt_Receipt_Item\" WHERE \"Id\"=:p0", model.Id).FirstOrDefault() ?? -1) + 1;

                    var ent = new Tx_IssueAndReceipt_Receipt_Item();
                    ent.Id = model.Id;
                    ent.ItemCode = model.ItemCode;
                    ent.ItemName = model.ItemName;
                    ent.Quantity = (int)(model.Quantity ?? 0);
                    ent.Uom = model.Uom;
                    ent.WhsCode = model.WhsCode;
                    ent.Netto = 0;
                    ent.LineNum = nextLine;
                    ent.LineStatus = "O";
                    ent.CreatedDate = dtModified;
                    ent.CreatedUser = model._UserId;
                    ent.ModifiedDate = dtModified;
                    ent.ModifiedUser = model._UserId;

                    CONTEXT.Tx_IssueAndReceipt_Receipt_Item.Add(ent);
                    CONTEXT.SaveChanges();
                    detId = ent.DetId;

                    CONTEXT_TRANS.Commit();
                }
                catch (Exception ex)
                {
                    CONTEXT_TRANS.Rollback();
                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
            }
            return detId;
        }

        // Update Quantity (dan Uom/WhsCode) baris item — dipanggil saat user edit grid.
        public void IssueItem_UpdateQuantity(IssueReceipt_IssueItemModel model)
        {
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    var ent = CONTEXT.Tx_IssueAndReceipt_Issue_Item.Find(model.DetId ?? 0);
                    if (ent != null)
                    {
                        ent.Quantity = (int)(model.Quantity ?? 0);
                        if (!string.IsNullOrEmpty(model.WhsCode)) ent.WhsCode = model.WhsCode;
                        ent.MsnPrd = model.MsnPrd;
                        ent.Department = model.Department;
                        ent.Cost = model.Cost;
                        ent.ModifiedDate = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        ent.ModifiedUser = model._UserId;
                        CONTEXT.SaveChanges();
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

        public void ReceiptItem_UpdateQuantity(IssueReceipt_ReceiptItemModel model)
        {
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    var ent = CONTEXT.Tx_IssueAndReceipt_Receipt_Item.Find(model.DetId ?? 0);
                    if (ent != null)
                    {
                        ent.Quantity = (int)(model.Quantity ?? 0);
                        if (!string.IsNullOrEmpty(model.WhsCode)) ent.WhsCode = model.WhsCode;
                        ent.MsnPrd = model.MsnPrd;
                        ent.Department = model.Department;
                        ent.Price = model.Price;
                        ent.ModifiedDate = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        ent.ModifiedUser = model._UserId;
                        CONTEXT.SaveChanges();
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

        // Hapus baris item ISSUE beserta batch & scale-nya (cascade).
        public void IssueItem_Delete(int userId, long detId)
        {
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetId\"=:p0 AND \"TransType\"='IssueAndReceiptIssue'", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item_Batch_Scale\" WHERE \"DetDetId\" IN (SELECT \"DetDetId\" FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0)", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item\" WHERE \"DetId\"=:p0", detId);
                    CONTEXT_TRANS.Commit();
                }
                catch (Exception ex)
                {
                    CONTEXT_TRANS.Rollback();
                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
            }
        }

        // Hapus baris item RECEIPT beserta batch & scale-nya (cascade).
        public void ReceiptItem_Delete(int userId, long detId)
        {
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tp_ScaleStaging\" WHERE \"DetId\"=:p0 AND \"TransType\"='IssueAndReceiptReceipt'", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch_Scale\" WHERE \"DetDetId\" IN (SELECT \"DetDetId\" FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0)", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0", detId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item\" WHERE \"DetId\"=:p0", detId);
                    CONTEXT_TRANS.Commit();
                }
                catch (Exception ex)
                {
                    CONTEXT_TRANS.Rollback();
                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
            }
        }

        #endregion

        #region Post / Cancel ke SAP

        // Hasil Add satu dokumen SAP.
        public class IssueReceipt_PostResult
        {
            public long? DocEntry { get; set; }
            public string DocNum { get; set; }
        }

        public void Post(int userId, ReProcessModel model)
        {
            PostSAP(userId, model.Id);
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
        // diringkas jadi jumlah saja.
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

        public void PostSAP(int userId, long id)
        {
            ReProcessModel sync = GetById(userId, id);

            // ---- Fase 1: Goods Issue (SAP transaksi #1 + DB transaksi #1) ----
            // Dilewati bila dokumen sudah "IssuePosted" (retry Fase 2 setelah Fase 2 gagal sebelumnya).
            IssueReceipt_PostResult issRes;
            using (var CONTEXT = new HANA_APP())
            {
                Tx_IssueAndReceipt txPeek = CONTEXT.Tx_IssueAndReceipt.Find(id);
                if (txPeek != null && txPeek.Status == "IssuePosted")
                {
                    issRes = new IssueReceipt_PostResult { DocEntry = txPeek.IssueDocEntry, DocNum = txPeek.IssueDocNum };
                }
                else
                {
                    issRes = PostSAP_Phase1_Issue(userId, id, sync);
                }
            }

            // ---- Fase 2: Costing + Goods Receipt (SAP transaksi #2 + DB transaksi #2) ----
            PostSAP_Phase2_Receipt(userId, id, sync, issRes);
        }

        // Fase 1: validasi + AddGoodsIssue dalam satu transaksi SAP dan satu transaksi DB.
        // Sukses -> tx.Status = "IssuePosted" (durable), commit SAP + commit DB bersamaan.
        // Gagal -> rollback SAP + rollback DB, tx.Status tetap "Draft" (sama seperti sebelum perubahan ini).
        private IssueReceipt_PostResult PostSAP_Phase1_Issue(int userId, long id, ReProcessModel sync)
        {
            SAPbobsCOM.Company oCompany = null;

            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    oCompany = SAPCachedCompany.GetCompany();
                    oCompany.StartTransaction();

                    Tx_IssueAndReceipt tx = CONTEXT.Tx_IssueAndReceipt.Find(id);
                    if (tx == null)
                    {
                        throw new Exception("[VALIDATION] - ReProcess tidak ditemukan");
                    }
                    if (tx.Status != "Draft")
                    {
                        throw new Exception("[VALIDATION] - Hanya dokumen Draft yang bisa di-Post");
                    }
                    bool anyIssue = sync.ListIssueItem_ != null && sync.ListIssueItem_.Any(x => (x.Quantity ?? 0) > 0);
                    bool anyReceipt = sync.ListReceiptItem_ != null && sync.ListReceiptItem_.Any(x => (x.Quantity ?? 0) > 0);
                    if (!anyIssue && !anyReceipt)
                    {
                        throw new Exception("[VALIDATION] - Tidak ada item Issue maupun Receipt untuk di-Post");
                    }

                    // Validasi Whse: baris tanpa gudang bikin SAP gagal (-5002 Inventory account not defined).
                    // Divalidasi di sini agar pesannya jelas (item mana), bukan error SAP mentah.
                    var whsErrors = new List<string>();
                    if (sync.ListIssueItem_ != null)
                    {
                        foreach (var it in sync.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0 && string.IsNullOrWhiteSpace(x.WhsCode)))
                        {
                            whsErrors.Add("Issue " + it.ItemCode);
                        }
                    }
                    if (sync.ListReceiptItem_ != null)
                    {
                        foreach (var it in sync.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0 && string.IsNullOrWhiteSpace(x.WhsCode)))
                        {
                            whsErrors.Add("Receipt " + it.ItemCode);
                        }
                    }
                    if (whsErrors.Any())
                    {
                        throw new Exception(BuildValidationMessage("Whse belum dipilih", whsErrors));
                    }

                    // ---- Validasi berjenjang Item -> Batch -> Scale, untuk sisi Issue DAN Receipt ----
                    // Dokumen hanya boleh di-Post bila seluruh jenjang di KEDUA sisi sudah selesai dan
                    // tidak ada quantity outstanding. Tiap jenjang dievaluasi untuk kedua sisi lalu
                    // dilaporkan bersama; bila ada error, Post berhenti di jenjang itu (tidak lanjut ke
                    // jenjang berikutnya) supaya user memperbaiki dari hulu ke hilir.
                    //
                    // Kedua sisi diproyeksikan ke bentuk seragam agar aturannya ditulis SEKALI saja.
                    // ListBatch_ pada model Receipt "menyembunyikan" milik base class (field `new`),
                    // jadi proyeksi dilakukan per sisi dengan tipe konkretnya masing-masing.
                    var sides = new List<Tuple<string, List<ReProcess_ValidateItem>>>();
                    sides.Add(Tuple.Create("Issue", sync.ListIssueItem_ == null
                        ? new List<ReProcess_ValidateItem>()
                        : sync.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0).Select(x => new ReProcess_ValidateItem
                        {
                            ItemCode = x.ItemCode,
                            Quantity = x.Quantity ?? 0,
                            Batches = (x.ListBatch_ ?? new List<IssueReceipt_IssueBatchModel>())
                                .Select(b => new ReProcess_ValidateBatch { Batch = b.Batch, Quantity = b.Quantity ?? 0, DetDetId = b.DetDetId ?? 0 })
                                .ToList()
                        }).ToList()));
                    sides.Add(Tuple.Create("Receipt", sync.ListReceiptItem_ == null
                        ? new List<ReProcess_ValidateItem>()
                        : sync.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0).Select(x => new ReProcess_ValidateItem
                        {
                            ItemCode = x.ItemCode,
                            Quantity = x.Quantity ?? 0,
                            Batches = (x.ListBatch_ ?? new List<IssueReceipt_ReceiptBatchModel>())
                                .Select(b => new ReProcess_ValidateBatch { Batch = b.Batch, Quantity = b.Quantity ?? 0, DetDetId = b.DetDetId ?? 0 })
                                .ToList()
                        }).ToList()));

                    // --- Jenjang 1: Item. Total Created (= SUM qty batch) harus SAMA dengan Total Needed.
                    // Dihitung langsung dari batch, BUKAN dari kolom "QuantityCreated": kolom itu
                    // di-refresh manual di banyak tempat sehingga bisa stale -- bila stale, dokumen yang
                    // batch-nya sudah benar malah tertolak (atau yang belum benar malah lolos).
                    var qtyErrors = new List<string>();
                    foreach (var side in sides)
                    {
                        foreach (var it in side.Item2)
                        {
                            decimal needed = it.Quantity;
                            decimal created = it.Batches.Sum(b => b.Quantity);
                            if (created < needed)
                            {
                                qtyErrors.Add(string.Format("{0} {1}: Created {2} < Needed {3} (outstanding {4})",
                                    side.Item1, it.ItemCode, created, needed, needed - created));
                            }
                            else if (created > needed)
                            {
                                qtyErrors.Add(string.Format("{0} {1}: Created {2} > Needed {3} (lebih {4})",
                                    side.Item1, it.ItemCode, created, needed, created - needed));
                            }
                        }
                    }
                    if (qtyErrors.Any())
                    {
                        throw new Exception(BuildValidationMessage("Total Created harus sama dengan Total Needed", qtyErrors));
                    }

                    // --- Jenjang 2: Batch. Tiap item wajib punya baris batch (semua item di ReProcess
                    // dikelola batch), dan batch wajib sudah tersimpan (DetDetId terisi) karena
                    // AddGoodsIssue/AddGoodsReceipt mengirim seluruh ListBatch_ ke SAP apa adanya.
                    var batchErrors = new List<string>();
                    foreach (var side in sides)
                    {
                        foreach (var it in side.Item2)
                        {
                            if (!it.Batches.Any())
                            {
                                batchErrors.Add(string.Format("{0} {1}: belum ada Batch", side.Item1, it.ItemCode));
                                continue;
                            }
                            foreach (var b in it.Batches.Where(x => x.DetDetId == 0))
                            {
                                batchErrors.Add(string.Format("{0} {1}: Batch {2} belum tersimpan", side.Item1, it.ItemCode, b.Batch));
                            }
                        }
                    }
                    if (batchErrors.Any())
                    {
                        throw new Exception(BuildValidationMessage("Batch belum lengkap", batchErrors));
                    }

                    // --- Jenjang 3: Scale. Tiap batch wajib punya minimal satu baris scale, hasil
                    // timbangnya sudah final (tidak ada staging 'Waiting'), dan Netto-nya sudah terisi.
                    var scaleErrors = new List<string>();
                    Action<string, string, long> addScaleError = (label, batch, detDetId) =>
                    {
                        string tblScale = "Tx_IssueAndReceipt_" + label + "_Item_Batch_Scale";

                        long scaleCount = CONTEXT.Database.SqlQuery<long>(
                            "SELECT COUNT(*) AS IDU FROM \"" + tblScale + "\" WHERE \"DetDetId\"=:p0", detDetId).FirstOrDefault();
                        // tidak ada baris scale sama sekali pada batch ini
                        if (scaleCount == 0)
                        {
                            // Teks per baris dijaga pendek; instruksinya ada di header pesan
                            // (lihat BuildValidationMessage) agar tidak berulang tiap baris.
                            scaleErrors.Add(string.Format("{0} Batch {1}: belum ada data Scale", label, batch));
                            return;
                        }

                        // ada baris scale yang masih Waiting di staging -> hasil timbang belum final.
                        // WAJIB difilter TransType: "DetDetId" adalah identity per-tabel batch, dan
                        // GRPO/Issue/Receipt menulis ke satu Tp_ScaleStaging -> nilainya bertabrakan
                        // antar dokumen. Tanpa filter ini, baris Waiting milik dokumen lain ikut terhitung.
                        // Hanya baris staging TERKINI per baris scale yang dinilai (pola MAX(StagingId)
                        // sama dengan grid di ReProcess__ItemBatchScaleList), sebab SaveFromScale
                        // menambah baris staging baru saat percobaan sebelumnya berstatus Error.
                        string transType = (label == "Receipt") ? "IssueAndReceiptReceipt" : "IssueAndReceiptIssue";
                        int waitingCount = CONTEXT.Database.SqlQuery<int>(
                            "SELECT COUNT(*) AS IDU " +
                            "FROM \"" + tblScale + "\" SC " +
                            "INNER JOIN \"Tp_ScaleStaging\" ST ON ST.\"DetDetDetId\" = SC.\"DetDetDetId\" AND ST.\"TransType\" = :p1 " +
                            "WHERE SC.\"DetDetId\" = :p0 AND ST.\"Status\" = 'Waiting' " +
                            "  AND ST.\"StagingId\" = (SELECT MAX(ST2.\"StagingId\") FROM \"Tp_ScaleStaging\" ST2 " +
                            "                          WHERE ST2.\"DetDetDetId\" = SC.\"DetDetDetId\" AND ST2.\"TransType\" = :p2)",
                            detDetId, transType, transType).FirstOrDefault();
                        if (waitingCount > 0)
                        {
                            scaleErrors.Add(string.Format("{0} Batch {1}: menunggu hasil timbang", label, batch));
                            return;
                        }

                        // Netto hanya diisi API (callback hasil timbang). Netto kosong berarti hasil
                        // timbang belum masuk -- termasuk kasus vendor GAGAL menimbang, yang membuat
                        // staging jadi Status='Error' + Netto=NULL sehingga TIDAK tertangkap cek
                        // 'Waiting' di atas. Tanpa cek ini dokumen bisa ter-post dengan Netto kosong,
                        // lalu FillCosting menghitung totalNettoReceipt=0 -> Price/Value Receipt jadi 0.
                        int nettoKosong = CONTEXT.Database.SqlQuery<int>(
                            "SELECT COUNT(*) AS IDU FROM \"" + tblScale + "\" " +
                            "WHERE \"DetDetId\"=:p0 AND (\"Netto\" IS NULL OR \"Netto\" <= 0)", detDetId).FirstOrDefault();
                        if (nettoKosong > 0)
                        {
                            scaleErrors.Add(string.Format("{0} Batch {1}: Netto belum terisi", label, batch));
                        }
                    };

                    foreach (var side in sides)
                    {
                        foreach (var b in side.Item2.SelectMany(x => x.Batches))
                        {
                            addScaleError(side.Item1, b.Batch, b.DetDetId);
                        }
                    }

                    if (scaleErrors.Any())
                    {
                        throw new Exception(BuildValidationMessage(
                            "Data Scale belum siap untuk di-Post (tunggu hasil timbang selesai, atau hapus baris scale yang menunggu)",
                            scaleErrors));
                    }

                    // Fase 1 hanya membuat Goods Issue. Goods Receipt (dengan UnitPrice benar) dibuat
                    // di Fase 2, SETELAH Goods Issue ini commit ke SAP (OINM baru terbaca setelah commit).
                    IssueReceipt_PostResult issRes = AddGoodsIssue(CONTEXT, oCompany, sync);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    if (issRes != null)
                    {
                        tx.IssueDocEntry = issRes.DocEntry;
                        tx.IssueDocNum = issRes.DocNum;
                        tx.IssueDocDate = dtModified;

                        // Simpan DocEntry SAP ke tiap baris item Issue.
                        if (issRes.DocEntry.HasValue)
                        {
                            CONTEXT.Database.ExecuteSqlCommand(
                                "UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"DocEntry\"=:p0 WHERE \"Id\"=:p1",
                                issRes.DocEntry.Value, id);
                        }
                    }

                    // Status antara: Issue sudah di SAP, Receipt menyusul di Fase 2. Bila Fase 2 gagal,
                    // status ini tetap tersimpan (bukan Draft, bukan Posted) sehingga Post bisa di-retry
                    // (lanjut Fase 2 saja, tanpa membuat Goods Issue dobel) atau dokumen di-Cancel.
                    tx.Status = "IssuePosted";
                    tx.ModifiedDate = dtModified;
                    tx.ModifiedUser = userId;

                    CONTEXT.SaveChanges();

                    if (oCompany.InTransaction)
                    {
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    }

                    CONTEXT_TRANS.Commit();

                    return issRes;
                }
                catch (Exception ex)
                {
                    if (oCompany != null && oCompany.InTransaction)
                    {
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    CONTEXT_TRANS.Rollback();

                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
                finally
                {
                    SAPCachedCompany.Release(oCompany);
                }
            }
        }

        // Fase 2: costing (OINM sudah terbaca karena Goods Issue Fase 1 sudah commit) + AddGoodsReceipt
        // dalam transaksi SAP #2 dan transaksi DB #2 sendiri, terpisah dari Fase 1.
        // Gagal -> rollback SAP #2 (Goods Receipt belum tersentuh SAP) + rollback DB #2 (Status tx TIDAK
        // ikut rollback -- tetap "IssuePosted" dari Fase 1 yang sudah commit sebelumnya) -> retry via Post lagi.
        private void PostSAP_Phase2_Receipt(int userId, long id, ReProcessModel sync, IssueReceipt_PostResult issRes)
        {
            SAPbobsCOM.Company oCompany = null;

            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    Tx_IssueAndReceipt tx = CONTEXT.Tx_IssueAndReceipt.Find(id);
                    if (tx == null)
                    {
                        throw new Exception("[VALIDATION] - ReProcess tidak ditemukan");
                    }

                    if (issRes != null && issRes.DocEntry.HasValue && sync.ListReceiptItem_ != null && sync.ListReceiptItem_.Any(x => (x.Quantity ?? 0) > 0))
                    {
                        FillCosting(CONTEXT, id, (int)issRes.DocEntry.Value, sync);
                    }

                    oCompany = SAPCachedCompany.GetCompany();
                    oCompany.StartTransaction();

                    IssueReceipt_PostResult recRes = AddGoodsReceipt(CONTEXT, oCompany, sync);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    if (recRes != null)
                    {
                        tx.ReceiptDocEntry = recRes.DocEntry;
                        tx.ReceiptDocNum = recRes.DocNum;
                        tx.ReceiptDocDate = dtModified;

                        // Simpan DocEntry SAP ke tiap baris item Receipt.
                        if (recRes.DocEntry.HasValue)
                        {
                            CONTEXT.Database.ExecuteSqlCommand(
                                "UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET \"DocEntry\"=:p0 WHERE \"Id\"=:p1",
                                recRes.DocEntry.Value, id);
                        }
                    }

                    tx.PostingDate = dtModified;
                    tx.Status = "Posted";
                    tx.IsAfterPosted = "Y";
                    tx.ModifiedDate = dtModified;
                    tx.ModifiedUser = userId;

                    CONTEXT.SaveChanges();

                    if (oCompany.InTransaction)
                    {
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    }

                    CONTEXT_TRANS.Commit();
                }
                catch (Exception ex)
                {
                    if (oCompany != null && oCompany.InTransaction)
                    {
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    CONTEXT_TRANS.Rollback();

                    string issueInfo = (issRes != null && issRes.DocEntry.HasValue)
                        ? string.Format(" (Goods Issue sudah ter-post, DocEntry {0} -- klik Post lagi untuk melanjutkan Goods Receipt)", issRes.DocEntry.Value)
                        : "";
                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message + issueInfo : string.Format("[VALIDATION] {0}{1} ", ex.Message, issueInfo));
                }
                finally
                {
                    SAPCachedCompany.Release(oCompany);
                }
            }
        }

        // Costing: isi Cost item Issue dari OINM (Goods Issue, TransType 60), lalu hitung Value & Price
        // item Receipt sebagai alokasi biaya issue per proporsi Netto.
        //   Value  = (Netto / TotalNettoReceipt) * TotalCostIssue
        //   Price  = Value / Quantity
        // Dipanggil di Fase 2, setelah Goods Issue Fase 1 sudah commit (OINM baru bisa dibaca setelah commit).
        // Menulis hasil ke DB DAN ke `sync` in-memory (dipakai AddGoodsReceipt utk isi UnitPrice SAP).
        private void FillCosting(HANA_APP CONTEXT, long id, int issueDocEntry, ReProcessModel sync)
        {
            string dbSap = DbProvider.dbSap_Name;

            // Issue Cost per baris dari OINM, urut LineNum.
            // Cost  = unit cost = ABS(TransValue)/OutQty
            // Value = total cost = ABS(TransValue)
            string sqlCost =
                "SELECT T1.\"LineNum\" AS \"LineNum\", T1.\"ItemCode\" AS \"ItemCode\", " +
                "(SELECT DISTINCT ABS(TT0.\"TransValue\") / NULLIF(TT0.\"OutQty\", 0) FROM \"" + dbSap + "\".\"OINM\" TT0 " +
                " WHERE TT0.\"CreatedBy\" = T0.\"DocEntry\" AND TT0.\"BASE_REF\" = T0.\"DocNum\" " +
                "   AND TT0.\"DocLineNum\" = T1.\"LineNum\" AND TT0.\"ItemCode\" = T1.\"ItemCode\" AND TT0.\"TransType\" = 60) AS \"IssueCost\", " +
                "(SELECT DISTINCT ABS(TT0.\"TransValue\") FROM \"" + dbSap + "\".\"OINM\" TT0 " +
                " WHERE TT0.\"CreatedBy\" = T0.\"DocEntry\" AND TT0.\"BASE_REF\" = T0.\"DocNum\" " +
                "   AND TT0.\"DocLineNum\" = T1.\"LineNum\" AND TT0.\"ItemCode\" = T1.\"ItemCode\" AND TT0.\"TransType\" = 60) AS \"IssueValue\" " +
                "FROM \"" + dbSap + "\".\"OIGE\" T0 " +
                "JOIN \"" + dbSap + "\".\"IGE1\" T1 ON T1.\"DocEntry\" = T0.\"DocEntry\" " +
                "WHERE T0.\"DocEntry\" = :p0 ORDER BY T1.\"LineNum\" ";
            var costRows = CONTEXT.Database.SqlQuery<ReProcess_IssueCostRow>(sqlCost, issueDocEntry).ToList();

            // Item Issue dikirim ke SAP hanya qty>0 -> LineNum 0-based, sesuai urutan AddGoodsIssue.
            var issueItems = sync.ListIssueItem_ == null
                ? new List<IssueReceipt_IssueItemModel>()
                : sync.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0).ToList();

            decimal totalCostIssue = 0m;
            int lineIdx = 0;
            foreach (var it in issueItems)
            {
                var row = costRows.FirstOrDefault(r => r.LineNum == lineIdx && r.ItemCode == it.ItemCode)
                          ?? costRows.FirstOrDefault(r => r.LineNum == lineIdx);
                if (row != null && (row.IssueCost.HasValue || row.IssueValue.HasValue))
                {
                    // Cost  = biaya per UNIT  (ABS(TransValue)/OutQty) -> kolom "Cost" item Issue.
                    // Value = biaya TOTAL baris (ABS(TransValue), sudah dikali qty) -> kolom "Value".
                    decimal unitCost = row.IssueCost ?? 0m;
                    decimal lineValue = row.IssueValue ?? (unitCost * (it.Quantity ?? 0));

                    // Yang diakumulasi untuk dialokasikan ke Receipt adalah biaya TOTAL, bukan biaya
                    // per unit -- menjumlahkan harga satuan antar item berbeda tidak bermakna dan
                    // membuat Value/Price Receipt jauh lebih kecil dari semestinya.
                    totalCostIssue += lineValue;

                    it.Cost = Math.Round(unitCost, 2);
                    CONTEXT.Database.ExecuteSqlCommand(
                        "UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Cost\" = :p0, \"Value\" = :p1 WHERE \"DetId\" = :p2",
                        Math.Round(unitCost, 2), Math.Round(lineValue, 2), it.DetId);
                }
                lineIdx++;
            }

            // Alokasi ke Receipt: Value = (Netto/TotalNettoReceipt)*TotalCostIssue; Price = Value/Quantity.
            var receiptItems = sync.ListReceiptItem_ == null
                ? new List<IssueReceipt_ReceiptItemModel>()
                : sync.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0).ToList();
            decimal totalNettoReceipt = receiptItems.Sum(r => r.Netto ?? 0m);
            foreach (var it in receiptItems)
            {
                decimal netto = it.Netto ?? 0m;
                decimal value = totalNettoReceipt > 0 ? (netto / totalNettoReceipt) * totalCostIssue : 0m;
                decimal qty = it.Quantity ?? 0m;
                decimal price = qty > 0 ? value / qty : 0m;
                it.Value = Math.Round(value, 2);
                it.Price = Math.Round(price, 2);
                CONTEXT.Database.ExecuteSqlCommand(
                    "UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET \"Value\" = :p0, \"Price\" = :p1 WHERE \"DetId\" = :p2",
                    Math.Round(value, 2), Math.Round(price, 2), it.DetId);
            }
        }

        // Issue = Goods Issue standalone (oInventoryGenExit / ObjType 60), tanpa tautan Production Order.
        private IssueReceipt_PostResult AddGoodsIssue(HANA_APP CONTEXT, SAPbobsCOM.Company oCompany, ReProcessModel model)
        {
            if (model.ListIssueItem_ == null || !model.ListIssueItem_.Any(x => (x.Quantity ?? 0) > 0))
            {
                return null;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            oDoc.DocDate = DateTime.Now;
            oDoc.TaxDate = DateTime.Now;

            foreach (var item in model.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0))
            {
                // Standalone: baris TIDAK tertaut Production Order — item ditentukan langsung dari ItemCode.
                oDoc.Lines.ItemCode = item.ItemCode;
                if (!string.IsNullOrEmpty(item.WhsCode))
                {
                    oDoc.Lines.WarehouseCode = item.WhsCode;
                }
                oDoc.Lines.Quantity = (double)(item.Quantity ?? 0);

                if (item.ListBatch_ != null && item.ListBatch_.Any())
                {
                    int batchIndex = 0;
                    foreach (var batch in item.ListBatch_)
                    {
                        if (batchIndex > 0)
                        {
                            oDoc.Lines.BatchNumbers.Add();
                        }
                        oDoc.Lines.BatchNumbers.BatchNumber = batch.Batch;
                        oDoc.Lines.BatchNumbers.Quantity = (double)(batch.Quantity ?? 0);
                        batchIndex++;
                    }
                }

                oDoc.Lines.Add();
            }

            int ret = oDoc.Add();
            if (ret != 0)
            {
                int nErr = oCompany.GetLastErrorCode();
                string errMsg = oCompany.GetLastErrorDescription();
                SapCompany.CleanUp(oDoc);
                throw new Exception("[VALIDATION] - Goods Issue : " + nErr + "|" + errMsg);
            }

            var result = new IssueReceipt_PostResult();
            int docEntry = Convert.ToInt32(oCompany.GetNewObjectKey());
            SAPbobsCOM.Documents oNew = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            if (oNew.GetByKey(docEntry))
            {
                result.DocEntry = docEntry;
                result.DocNum = oNew.DocNum.ToString();
            }
            SapCompany.CleanUp(oNew);
            SapCompany.CleanUp(oDoc);
            return result;
        }

        // Receipt = Goods Receipt standalone (oInventoryGenEntry / ObjType 59), tanpa tautan Production Order.
        private IssueReceipt_PostResult AddGoodsReceipt(HANA_APP CONTEXT, SAPbobsCOM.Company oCompany, ReProcessModel model)
        {
            if (model.ListReceiptItem_ == null || !model.ListReceiptItem_.Any(x => (x.Quantity ?? 0) > 0))
            {
                return null;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
            oDoc.DocDate = DateTime.Now;
            oDoc.TaxDate = DateTime.Now;

            foreach (var item in model.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0))
            {
                // Standalone: baris TIDAK tertaut Production Order — item ditentukan langsung dari ItemCode.
                oDoc.Lines.ItemCode = item.ItemCode;
                if (!string.IsNullOrEmpty(item.WhsCode))
                {
                    oDoc.Lines.WarehouseCode = item.WhsCode;
                }
                oDoc.Lines.Quantity = (double)(item.Quantity ?? 0);

                // UnitPrice diisi dari kolom Price pada Receipt Item (bila sudah terisi).
                if (item.Price.HasValue)
                {
                    oDoc.Lines.UnitPrice = (double)item.Price.Value;
                }

                if (item.ListBatch_ != null && item.ListBatch_.Any())
                {
                    int batchIndex = 0;
                    foreach (var batch in item.ListBatch_)
                    {
                        if (batchIndex > 0)
                        {
                            oDoc.Lines.BatchNumbers.Add();
                        }
                        oDoc.Lines.BatchNumbers.BatchNumber = batch.Batch;
                        oDoc.Lines.BatchNumbers.Quantity = (double)(batch.Quantity ?? 0);
                        batchIndex++;
                    }
                }

                oDoc.Lines.Add();
            }

            int ret = oDoc.Add();
            if (ret != 0)
            {
                int nErr = oCompany.GetLastErrorCode();
                string errMsg = oCompany.GetLastErrorDescription();
                SapCompany.CleanUp(oDoc);
                throw new Exception("[VALIDATION] - Goods Receipt : " + nErr + "|" + errMsg);
            }

            var result = new IssueReceipt_PostResult();
            int docEntry = Convert.ToInt32(oCompany.GetNewObjectKey());
            SAPbobsCOM.Documents oNew = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
            if (oNew.GetByKey(docEntry))
            {
                result.DocEntry = docEntry;
                result.DocNum = oNew.DocNum.ToString();
            }
            SapCompany.CleanUp(oNew);
            SapCompany.CleanUp(oDoc);
            return result;
        }

        public void Cancel(int userId, long Id, string cancelReason)
        {
            using (var CONTEXT = new HANA_APP())
            using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
            {
                try
                {
                    Tx_IssueAndReceipt tx = CONTEXT.Tx_IssueAndReceipt.Find(Id);
                    if (tx != null)
                    {
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        tx.Status = "Cancel";
                        tx.CancelReason = cancelReason;
                        tx.ModifiedDate = dtModified;
                        tx.ModifiedUser = userId;
                        CONTEXT.SaveChanges();

                        // Batalkan di SAP: urutan kebalikan (Receipt dulu, lalu Issue).
                        var oCompany = SAPCachedCompany.GetCompany();
                        try
                        {
                            if ((tx.ReceiptDocEntry ?? 0) != 0)
                            {
                                CancelSAP(oCompany, SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, Convert.ToInt32(tx.ReceiptDocEntry));
                            }
                            if ((tx.IssueDocEntry ?? 0) != 0)
                            {
                                CancelSAP(oCompany, SAPbobsCOM.BoObjectTypes.oInventoryGenExit, Convert.ToInt32(tx.IssueDocEntry));
                            }
                        }
                        finally
                        {
                            SAPCachedCompany.Release(oCompany);
                        }
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

        // Batalkan satu dokumen SAP via CreateCancellationDocument.
        private static void CancelSAP(SAPbobsCOM.Company oCompany, SAPbobsCOM.BoObjectTypes objType, int docEntry)
        {
            SAPbobsCOM.Documents oDoc = null;
            try
            {
                if (!oCompany.InTransaction)
                    oCompany.StartTransaction();

                oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(objType);
                if (!oDoc.GetByKey(docEntry))
                    throw new Exception("[VALIDATION] - Dokumen SAP tidak ditemukan (DocEntry " + docEntry + ")");

                SAPbobsCOM.Documents oCancellation = (SAPbobsCOM.Documents)oDoc.CreateCancellationDocument();
                if (oCancellation == null)
                {
                    oCompany.GetLastError(out int e1, out string m1);
                    throw new Exception("[VALIDATION] - CreateCancellationDocument : " + e1 + "|" + m1);
                }

                int ret = oCancellation.Add();
                if (ret != 0)
                {
                    oCompany.GetLastError(out int e2, out string m2);
                    throw new Exception("[VALIDATION] - Cancel gagal : " + e2 + "|" + m2);
                }

                if (oCompany.InTransaction)
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }
            catch (Exception)
            {
                if (oCompany.InTransaction)
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                throw;
            }
            finally
            {
                if (oDoc != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDoc);
                    oDoc = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #endregion

    }


    #endregion

}