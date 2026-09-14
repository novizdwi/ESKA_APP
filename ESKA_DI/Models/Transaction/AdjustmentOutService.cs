using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;

using Models._Sap;
using SAPbobsCOM;

namespace Models.Transaction
{
    #region Models

    public class AdjustmentOutModel
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

        public string TransType { get; set; }

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? PostingDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public string DocNum_ { get; set; }

        public string Status { get; set; }

        public string CheckNeedApproval_ { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public List<AdjustmentOut_DetailModel> ListDetail_ = new List<AdjustmentOut_DetailModel>();

        public AdjustmentOut_Detail Details_ { get; set; }
    }

    public class AdjustmentOut_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentOut_DetailModel> insertedRowValues { get; set; }
        public List<AdjustmentOut_DetailModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentOut_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentOut_ApprovalModel> insertedRowValues { get; set; }
        public List<AdjustmentOut_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentOut_ApprovalModel
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

    public class AdjustmentOut_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int? RowNo { get; set; }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string AcctCode { get; set; }

        public string AcctName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOnHandSAP_ { get; set; }

        // OITM.ManBtchNum: batch hanya dikirim ke SAP untuk item yang batch-managed.
        public string ManBtchNum_ { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public decimal? UnitPriceTc { get; set; }

        public decimal? LineTotal { get; set; }

        public string FreeText { get; set; }

        public List<AdjustmentOutBatchModel> ListItemBatch_ = new List<AdjustmentOutBatchModel>();
    }

    public class AdjustmentOutApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<AdjustmentOut_ApprovalModel> ApprovalStepList__ = new List<AdjustmentOut_ApprovalModel>();

        public AdjustmentOut_Approval ApprovalStep__ { get; set; }
    }

    public class AdjustmentOutBatchView___
    {
        public int? RowNo { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public List<AdjustmentOutBatchModel> AdjustmentOutBatchModel___ { get; set; }

        public AdjustmentOut_DetailBatch DetailBatchs_ { get; set; }
    }

    public class AdjustmentOut_DetailBatch
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentOutBatchModel> insertedRowValues { get; set; }
        public List<AdjustmentOutBatchModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentOutBatchModel
    {
        public int _UserId { get; set; }

        public int? RowNo { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public long? DetDetId { get; set; }

        [Required(ErrorMessage = "required")]
        public string Batch { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? AdmissionDate { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Netto { get; set; }
    }
    #endregion

    #region Services

    public class AdjustmentOutService
    {

        public AdjustmentOutModel GetNewModel(int userId)
        {
            AdjustmentOutModel model = new AdjustmentOutModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public AdjustmentOutModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public AdjustmentOutModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            AdjustmentOutModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_AdjustmentOut"" T0
                            WHERE T0.""Id"" = :p0
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutModel>(ssql, id).SingleOrDefault();
                if (model == null)
                {
                    return null;
                }

                model.ListDetail_ = this.AdjustmentOut_Details(CONTEXT, id, method);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'AdjustmentOut', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
                            FROM ""Tx_AdjustmentOut"" T0
                            INNER JOIN  ""Tx_AdjustmentOut_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
                            WHERE T0.""Id"" = :p0
                            AND T1.""UserId"" = :p1
                        ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }

        public List<AdjustmentOut_DetailModel> AdjustmentOut_Details(long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentOut_Details(CONTEXT, id, method);
            }

        }

        public List<AdjustmentOut_DetailModel> AdjustmentOut_Details(HANA_APP CONTEXT, long id = 0, string method = "")
        {

            string ssql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetId"") AS ""RowNo"",
                T0.*,
                T2.""OnHand"" AS ""QuantityOnHandSAP_"",
                T3.""ManBtchNum"" AS ""ManBtchNum_""
            FROM ""Tx_AdjustmentOut_Item"" T0
            INNER JOIN ""Tx_AdjustmentOut"" T1 ON T0.""Id"" = T1.""Id""
            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITW"" T2 ON T0.""ItemCode"" = T2.""ItemCode"" AND T1.""WhsCode"" = T2.""WhsCode""
            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITM"" T3 ON T0.""ItemCode"" = T3.""ItemCode""
            WHERE T0.""Id"" =:p0
            ORDER BY T0.""DetId"" ASC
            ";
            var adjustmentOut = CONTEXT.Database.SqlQuery<AdjustmentOut_DetailModel>(ssql, id).ToList();

            if (method == "Post" && adjustmentOut.Count != 0)
            {
                string ssqlBatch = @"
                    SELECT *
                    FROM ""Tx_AdjustmentOut_Item_Batch""
                    WHERE ""Id"" = :p0
                    ORDER BY ""DetId"", ""DetDetId""
                ";

                var itemBatch = CONTEXT.Database.SqlQuery<AdjustmentOutBatchModel>(ssqlBatch, id).ToList();
                var batchLookup = itemBatch.ToLookup(x => x.DetId);
                foreach (var item in adjustmentOut)
                {
                    item.ListItemBatch_ = batchLookup[item.DetId].ToList();
                }
            }

            return adjustmentOut;
        }

        public List<AdjustmentOut_ApprovalModel> GetAdjustmentOut_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentOut_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<AdjustmentOut_ApprovalModel> GetAdjustmentOut_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_AdjustmentOut_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<AdjustmentOut_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public AdjustmentOutModel NavFirst(int userId)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public AdjustmentOutModel NavPrevious(int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public AdjustmentOutModel NavNext(int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public AdjustmentOutModel NavLast(int userId)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(AdjustmentOutModel model)
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
                            Tx_AdjustmentOut tx_AdjustmentOut = new Tx_AdjustmentOut();
                            CopyProperty.CopyProperties(model, tx_AdjustmentOut, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentOut.TransType = "AdjustmentOut";
                            tx_AdjustmentOut.CreatedDate = dtModified;
                            tx_AdjustmentOut.CreatedUser = model._UserId;
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'AdjustmentOut','" + dateX + "','') ").SingleOrDefault();
                            tx_AdjustmentOut.TransNo = transNo;

                            CONTEXT.Tx_AdjustmentOut.Add(tx_AdjustmentOut);
                            CONTEXT.SaveChanges();
                            Id = tx_AdjustmentOut.Id;

                            String keyValue;
                            keyValue = tx_AdjustmentOut.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "AdjustmentOut", "add", "Id", keyValue);

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
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }
            }

            return Id;

        }

        public void Update(AdjustmentOutModel model, string method = "")
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

                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "before", "AdjustmentOut", "update", "Id", keyValue);

                            Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(model.Id);
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            if (tx_AdjustmentOut != null)
                            {
                                var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                CopyProperty.CopyProperties(model, tx_AdjustmentOut, false, exceptColumns);

                                tx_AdjustmentOut.ModifiedDate = dtModified;
                                tx_AdjustmentOut.ModifiedUser = model._UserId;

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
                                            AdjustmentOut_DetailModel detailModel = new AdjustmentOut_DetailModel();
                                            detailModel.DetId = detId;
                                            Detail_Delete(CONTEXT, detailModel);
                                        }
                                    }
                                }

                                CONTEXT.SaveChanges();

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "AdjustmentOut", "update", "Id", keyValue);

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
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }
            }
        }

        public bool ChooseItem(int UserId, long Id, string[] data, string sorting)
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
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "before", "AdjustmentOut", "ChooseItem", "Id", keyValue);

                            string sqlWhere;
                            if (data.Length == 0)
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

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_ChooseItem\"(:p0,:p1,:p2,:p3)", UserId, Id, sqlWhere, sorting ?? "");

                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "AdjustmentOut", "ChooseItem", "Id", keyValue);

                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMessage;
                            if (ex.Message.StartsWith("[VALIDATION]"))
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

        public long Detail_Add(HANA_APP CONTEXT, AdjustmentOut_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_AdjustmentOut_Item tx_AdjustmentOut_Item = new Tx_AdjustmentOut_Item();

                CopyProperty.CopyProperties(model, tx_AdjustmentOut_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_AdjustmentOut_Item.Id = Id;
                tx_AdjustmentOut_Item.CreatedDate = dtModified;
                tx_AdjustmentOut_Item.CreatedUser = UserId;
                tx_AdjustmentOut_Item.ModifiedDate = dtModified;
                tx_AdjustmentOut_Item.ModifiedUser = UserId;

                CONTEXT.Tx_AdjustmentOut_Item.Add(tx_AdjustmentOut_Item);
                CONTEXT.SaveChanges();
                DetId = tx_AdjustmentOut_Item.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, AdjustmentOut_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_AdjustmentOut_Item tx_AdjustmentOut_Item = CONTEXT.Tx_AdjustmentOut_Item.Find(model.DetId);

                if (tx_AdjustmentOut_Item != null)
                {
                    // Quantity & LineTotal dihitung SpAdjustmentOut_UpdateItemQuantity dari
                    // popup batch -- jangan sampai tertimpa nilai grid saat edit Free Text.
                    var exceptColumns = new string[] { "DetId", "Id", "Quantity", "LineTotal" };
                    CopyProperty.CopyProperties(model, tx_AdjustmentOut_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tx_AdjustmentOut_Item.ModifiedDate = dtModified;
                    tx_AdjustmentOut_Item.ModifiedUser = UserId;
                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, AdjustmentOut_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {
                    // Batch & scale milik item ikut dihapus, supaya tidak jadi baris yatim.
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentOut_Item_Batch_Scale\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentOut_Item_Batch\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentOut_Item\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.SaveChanges();
                }
            }

        }

        public void Post(int userId, AdjustmentOutModel adjustmentOutModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var statusCheck = CONTEXT.Database.SqlQuery<StatusCheckModel>(@"
                    SELECT ""Status"", ""ApprovalStatus"", ""IsApproval""
                    FROM ""Tx_AdjustmentOut""
                    WHERE ""Id"" = :p0
                ", adjustmentOutModel.Id).FirstOrDefault();

                if (statusCheck == null)
                    throw new Exception("[VALIDATION] Transaction not found.");

                // Mencegah Goods Issue dobel di SAP kalau Post terkirim dua kali.
                if (statusCheck.Status != "Draft")
                    throw new Exception("[VALIDATION] Cannot post. Only Draft transaction can be posted.");

                if (statusCheck.ApprovalStatus == "Rejected")
                    throw new Exception("[VALIDATION] Cannot post. Transaction has been Rejected.");

                if (statusCheck.ApprovalStatus == "Waiting")
                    throw new Exception("[VALIDATION] Cannot post. Transaction is still waiting for Approval.");
            }

            Update(adjustmentOutModel, "Post");
            PostSAP(userId, adjustmentOutModel.Id);
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
                        String keyValue;
                        keyValue = id.ToString();

                        AdjustmentOutModel syncAdjustmentOut = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "post", "Id", keyValue);

                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(id);
                        if (tx_AdjustmentOut == null)
                        {
                            throw new Exception("[VALIDATION] Transaction not found.");
                        }

                        oCompany = SAPCachedCompany.GetCompany();

                        // Goods Issue dan update status web harus jadi satu kesatuan:
                        // kalau hook "after" gagal, dokumen SAP ikut di-rollback.
                        oCompany.StartTransaction();

                        string docNum;
                        int docEntry = AddGoodsIssue(oCompany, syncAdjustmentOut, out docNum);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_AdjustmentOut.PostingDate = dtModified;
                        tx_AdjustmentOut.DocEntry = docEntry;
                        tx_AdjustmentOut.DocNum = docNum;

                        tx_AdjustmentOut.Status = "Posted";
                        tx_AdjustmentOut.IsAfterPosted = "Y";
                        tx_AdjustmentOut.ModifiedDate = dtModified;
                        tx_AdjustmentOut.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "post", "Id", keyValue);

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

                        string errorMassage;
                        if (ex.Message.StartsWith("[VALIDATION]"))
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
                        if (oCompany != null)
                        {
                            SAPCachedCompany.Release(oCompany);
                        }
                    }
                }
            }

        }

        // OIGE: satu baris per item yang Quantity-nya > 0.
        private int AddGoodsIssue(SAPbobsCOM.Company oCompany, AdjustmentOutModel model, out string docNum)
        {
            docNum = "";

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

            try
            {
                oDoc.DocDate = DateTime.Now;

                if (!string.IsNullOrWhiteSpace(model.Comments))
                {
                    oDoc.Comments = model.Comments;
                    // JrnlMemo di SAP maksimal 50 karakter.
                    oDoc.JournalMemo = model.Comments.Length > 50 ? model.Comments.Substring(0, 50) : model.Comments;
                }

                oDoc.UserFields.Fields.Item("U_IDU_WebId").Value = model.Id.ToString();
                oDoc.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo ?? "";

                bool firstLine = true;

                foreach (var item in model.ListDetail_)
                {
                    if ((item.Quantity ?? 0) <= 0)
                    {
                        continue;
                    }

                    if (!firstLine)
                    {
                        oDoc.Lines.Add();
                    }
                    firstLine = false;

                    oDoc.Lines.ItemCode = item.ItemCode;
                    oDoc.Lines.WarehouseCode = item.WhsCode;
                    oDoc.Lines.Quantity = (double)item.Quantity;

                    // Goods Issue dinilai SAP dengan harga pokok item, UnitPrice tidak dikirim.

                    // Kosong = SAP pakai akun offset default dari item group / warehouse.
                    if (!string.IsNullOrWhiteSpace(item.AcctCode))
                    {
                        oDoc.Lines.AccountCode = item.AcctCode;
                    }

                    if (!string.IsNullOrWhiteSpace(item.FreeText))
                    {
                        oDoc.Lines.FreeText = item.FreeText;
                    }

                    oDoc.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = (item.Id ?? 0).ToString();
                    oDoc.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = (item.DetId ?? 0).ToString();

                    if (item.ManBtchNum_ == "Y" && item.ListItemBatch_ != null)
                    {
                        int batchIndex = 0;

                        foreach (var itemBatch in item.ListItemBatch_)
                        {
                            if (string.IsNullOrWhiteSpace(itemBatch.Batch) || (itemBatch.Quantity ?? 0) <= 0)
                            {
                                continue;
                            }

                            if (batchIndex > 0)
                            {
                                oDoc.Lines.BatchNumbers.Add();
                            }

                            oDoc.Lines.BatchNumbers.BatchNumber = itemBatch.Batch;

                            // WAJIB Quantity, bukan Netto: total BatchNumbers.Quantity harus sama
                            // dengan Lines.Quantity (= SUM batch Quantity dari
                            // SpAdjustmentOut_UpdateItemQuantity), kalau tidak SAP menolak -4014.
                            oDoc.Lines.BatchNumbers.Quantity = (double)itemBatch.Quantity;

                            batchIndex++;
                        }
                    }
                }

                if (firstLine)
                {
                    throw new Exception("[VALIDATION] - No item with quantity greater than zero");
                }

                if (oDoc.Add() != 0)
                {
                    int nErr = oCompany.GetLastErrorCode();
                    string errMsg = oCompany.GetLastErrorDescription();

                    throw new Exception(string.Format("[VALIDATION] - Add Goods Issue : {0}|{1}", nErr, errMsg));
                }

                int docEntry = Convert.ToInt32(oCompany.GetNewObjectKey());

                // DocNum dibaca lewat DI API (koneksi yang sama), karena dokumennya belum
                // di-commit sehingga belum terlihat dari koneksi HANA_APP.
                if (oDoc.GetByKey(docEntry))
                {
                    docNum = oDoc.DocNum.ToString();
                }

                return docEntry;
            }
            finally
            {
                SapCompany.CleanUp(oDoc);
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

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "cancel", "Id", keyValue);

                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(Id);
                        if (tx_AdjustmentOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentOut.Status = "Cancel";
                            tx_AdjustmentOut.ApprovalStatus = "Rejected";
                            tx_AdjustmentOut.CancelReason = cancelReason;
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "cancel", "Id", keyValue);


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
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "requestApproval", "Id", keyValue);

                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(id);
                        if (tx_AdjustmentOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentOut.IsApproval = "Y";
                            tx_AdjustmentOut.ApprovalMessages = approvalMessages;
                            tx_AdjustmentOut.ApprovalStatus = "Waiting";
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'AdjustmentOut',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "requestApproval", "Id", keyValue);
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
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                }
            }

        }

        public void Approve(int userId, long id, string approvalMessage)
        {
            string approvalStatus = Authorize(userId, id, "Approve", approvalMessage);

            // Approval terakhir langsung memposting dokumen ke SAP.
            if (approvalStatus == "Approved")
            {
                this.PostSAP(userId, id);
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

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'AdjustmentOut', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus""
                            FROM ""Tx_AdjustmentOut"" T0
                            WHERE T0.""Id"" = :p0
                        ";

                        approvalStatus = CONTEXT.Database.SqlQuery<string>(strApprovalStatus, id).FirstOrDefault();
                        return approvalStatus;
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
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                }
            }

        }

        public AdjustmentOutApprovalView___ GetViewApproval(long id)
        {
            AdjustmentOutApprovalView___ model = new AdjustmentOutApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_AdjustmentOut"" T0
                    LEFT JOIN ""Tx_AdjustmentOut_Approval"" T1 ON T0.""Id"" = T1.""Id""
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0
                ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetAdjustmentOut_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

        public AdjustmentOutBatchView___ GetAdjustmentOut_Batch(long id, long detId)
        {
            string sql = null;
            AdjustmentOutBatchView___ model = new AdjustmentOutBatchView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName"", T1.""WhsCode"", T1.""WhsName""
                        FROM ""Tx_AdjustmentOut_Item"" T0
                        INNER JOIN ""Tx_AdjustmentOut"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutBatchView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_AdjustmentOut_Item_Batch"" T0
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.AdjustmentOutBatchModel___ = CONTEXT.Database.SqlQuery<AdjustmentOutBatchModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public List<AdjustmentOutBatchModel> GetAdjustmentOut_ItemBatchList(long id, long detId)
        {
            string sql = null;
            List<AdjustmentOutBatchModel> model = new List<AdjustmentOutBatchModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_AdjustmentOut_Item_Batch"" T0
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutBatchModel>(sql, id, detId).ToList();
            }
            return model;
        }


        public long AdjustmentOut_AddNewItemBatch(AdjustmentOutBatchModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_AdjustmentOut_Item_Batch tx_AdjustmentOut_Item_Batch = new Tx_AdjustmentOut_Item_Batch();
                        CopyProperty.CopyProperties(model, tx_AdjustmentOut_Item_Batch, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_AdjustmentOut_Item_Batch.CreatedDate = dtModified;
                        tx_AdjustmentOut_Item_Batch.CreatedUser = model._UserId;
                        tx_AdjustmentOut_Item_Batch.ModifiedDate = dtModified;
                        tx_AdjustmentOut_Item_Batch.ModifiedUser = model._UserId;

                        CONTEXT.Tx_AdjustmentOut_Item_Batch.Add(tx_AdjustmentOut_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_AdjustmentOut_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_AdjustmentOut_Item_Batch.Id.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_UpdateItemQuantity\"(:p0, 'Tx_AdjustmentOut_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                        SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "AdjustmentOut", "addItemBatch", "Id", keyValue);

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
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }

                }
            }

            return detDetId;
        }

        public void AdjustmentOut_UpdateItemBatch(AdjustmentOutBatchModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "before", "AdjustmentOut", "updateItemBatch", "Id", keyValue);

                        Tx_AdjustmentOut_Item_Batch tx_AdjustmentOut_Item_Batch = CONTEXT.Tx_AdjustmentOut_Item_Batch.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_AdjustmentOut_Item_Batch != null)
                        {
                            var exceptColumns = new string[] { "Id", "DetId", "DetDetId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_AdjustmentOut_Item_Batch, false, exceptColumns);

                            tx_AdjustmentOut_Item_Batch.ModifiedDate = dtModified;
                            tx_AdjustmentOut_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_UpdateItemQuantity\"(:p0, 'Tx_AdjustmentOut_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "AdjustmentOut", "updateItemBatch", "Id", keyValue);

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
                            errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                        }

                        throw new Exception(errorMassage);
                    }
                }
            }
        }

        public void AdjustmentOut_DeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetId != 0)
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(_userId, "AdjustmentOut", CONTEXT, "before", "AdjustmentOut", "deleteItemBatch", "Id", Id.ToString());

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentOut_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentOut_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_UpdateItemQuantity\"(:p0, 'Tx_AdjustmentOut_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }

                }
            }
        }

    }


    #endregion

}
