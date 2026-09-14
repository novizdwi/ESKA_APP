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

    public class AdjustmentInModel
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

        public List<AdjustmentIn_DetailModel> ListDetail_ = new List<AdjustmentIn_DetailModel>();

        public AdjustmentIn_Detail Details_ { get; set; }
    }

    public class AdjustmentIn_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentIn_DetailModel> insertedRowValues { get; set; }
        public List<AdjustmentIn_DetailModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentIn_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentIn_ApprovalModel> insertedRowValues { get; set; }
        public List<AdjustmentIn_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentIn_ApprovalModel
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

    public class AdjustmentIn_DetailModel
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

        public List<AdjustmentInBatchModel> ListItemBatch_ = new List<AdjustmentInBatchModel>();
    }

    public class AdjustmentInApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<AdjustmentIn_ApprovalModel> ApprovalStepList__ = new List<AdjustmentIn_ApprovalModel>();

        public AdjustmentIn_Approval ApprovalStep__ { get; set; }
    }

    public class AdjustmentInBatchView___
    {
        public int? RowNo { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public List<AdjustmentInBatchModel> AdjustmentInBatchModel___ { get; set; }

        public AdjustmentIn_DetailBatch DetailBatchs_ { get; set; }
    }

    public class AdjustmentIn_DetailBatch
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentInBatchModel> insertedRowValues { get; set; }
        public List<AdjustmentInBatchModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentInBatchModel
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

    public class AdjustmentInService
    {

        public AdjustmentInModel GetNewModel(int userId)
        {
            AdjustmentInModel model = new AdjustmentInModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public AdjustmentInModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public AdjustmentInModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            AdjustmentInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_AdjustmentIn"" T0
                            WHERE T0.""Id"" = :p0
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<AdjustmentInModel>(ssql, id).SingleOrDefault();
                if (model == null)
                {
                    return null;
                }

                model.ListDetail_ = this.AdjustmentIn_Details(CONTEXT, id, method);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'AdjustmentIn', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
                            FROM ""Tx_AdjustmentIn"" T0
                            INNER JOIN  ""Tx_AdjustmentIn_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
                            WHERE T0.""Id"" = :p0
                            AND T1.""UserId"" = :p1
                        ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }

        public List<AdjustmentIn_DetailModel> AdjustmentIn_Details(long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentIn_Details(CONTEXT, id, method);
            }

        }

        public List<AdjustmentIn_DetailModel> AdjustmentIn_Details(HANA_APP CONTEXT, long id = 0, string method = "")
        {

            string ssql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetId"") AS ""RowNo"",
                T0.*,
                T2.""OnHand"" AS ""QuantityOnHandSAP_"",
                T3.""ManBtchNum"" AS ""ManBtchNum_""
            FROM ""Tx_AdjustmentIn_Item"" T0
            INNER JOIN ""Tx_AdjustmentIn"" T1 ON T0.""Id"" = T1.""Id""
            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITW"" T2 ON T0.""ItemCode"" = T2.""ItemCode"" AND T1.""WhsCode"" = T2.""WhsCode""
            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITM"" T3 ON T0.""ItemCode"" = T3.""ItemCode""
            WHERE T0.""Id"" =:p0
            ORDER BY T0.""DetId"" ASC
            ";
            var adjustmentIn = CONTEXT.Database.SqlQuery<AdjustmentIn_DetailModel>(ssql, id).ToList();

            if (method == "Post" && adjustmentIn.Count != 0)
            {
                string ssqlBatch = @"
                    SELECT *
                    FROM ""Tx_AdjustmentIn_Item_Batch""
                    WHERE ""Id"" = :p0
                    ORDER BY ""DetId"", ""DetDetId""
                ";

                var itemBatch = CONTEXT.Database.SqlQuery<AdjustmentInBatchModel>(ssqlBatch, id).ToList();
                var batchLookup = itemBatch.ToLookup(x => x.DetId);
                foreach (var item in adjustmentIn)
                {
                    item.ListItemBatch_ = batchLookup[item.DetId].ToList();
                }
            }

            return adjustmentIn;
        }

        public List<AdjustmentIn_ApprovalModel> GetAdjustmentIn_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentIn_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<AdjustmentIn_ApprovalModel> GetAdjustmentIn_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_AdjustmentIn_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<AdjustmentIn_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public AdjustmentInModel NavFirst(int userId)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public AdjustmentInModel NavPrevious(int userId, long id = 0)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public AdjustmentInModel NavNext(int userId, long id = 0)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public AdjustmentInModel NavLast(int userId)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(AdjustmentInModel model)
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
                            Tx_AdjustmentIn tx_AdjustmentIn = new Tx_AdjustmentIn();
                            CopyProperty.CopyProperties(model, tx_AdjustmentIn, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentIn.TransType = "AdjustmentIn";
                            tx_AdjustmentIn.CreatedDate = dtModified;
                            tx_AdjustmentIn.CreatedUser = model._UserId;
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'AdjustmentIn','" + dateX + "','') ").SingleOrDefault();
                            tx_AdjustmentIn.TransNo = transNo;

                            CONTEXT.Tx_AdjustmentIn.Add(tx_AdjustmentIn);
                            CONTEXT.SaveChanges();
                            Id = tx_AdjustmentIn.Id;

                            String keyValue;
                            keyValue = tx_AdjustmentIn.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn", "add", "Id", keyValue);

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

        public void Update(AdjustmentInModel model, string method = "")
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

                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "before", "AdjustmentIn", "update", "Id", keyValue);

                            Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(model.Id);
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            if (tx_AdjustmentIn != null)
                            {
                                var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                CopyProperty.CopyProperties(model, tx_AdjustmentIn, false, exceptColumns);

                                tx_AdjustmentIn.ModifiedDate = dtModified;
                                tx_AdjustmentIn.ModifiedUser = model._UserId;

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
                                            AdjustmentIn_DetailModel detailModel = new AdjustmentIn_DetailModel();
                                            detailModel.DetId = detId;
                                            Detail_Delete(CONTEXT, detailModel);
                                        }
                                    }
                                }

                                CONTEXT.SaveChanges();

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn", "update", "Id", keyValue);

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
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "before", "AdjustmentIn", "ChooseItem", "Id", keyValue);

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

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_ChooseItem\"(:p0,:p1,:p2,:p3)", UserId, Id, sqlWhere, sorting ?? "");

                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "AdjustmentIn", "ChooseItem", "Id", keyValue);

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

        public long Detail_Add(HANA_APP CONTEXT, AdjustmentIn_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_AdjustmentIn_Item tx_AdjustmentIn_Item = new Tx_AdjustmentIn_Item();

                CopyProperty.CopyProperties(model, tx_AdjustmentIn_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_AdjustmentIn_Item.Id = Id;
                tx_AdjustmentIn_Item.CreatedDate = dtModified;
                tx_AdjustmentIn_Item.CreatedUser = UserId;
                tx_AdjustmentIn_Item.ModifiedDate = dtModified;
                tx_AdjustmentIn_Item.ModifiedUser = UserId;

                CONTEXT.Tx_AdjustmentIn_Item.Add(tx_AdjustmentIn_Item);
                CONTEXT.SaveChanges();
                DetId = tx_AdjustmentIn_Item.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, AdjustmentIn_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_AdjustmentIn_Item tx_AdjustmentIn_Item = CONTEXT.Tx_AdjustmentIn_Item.Find(model.DetId);

                if (tx_AdjustmentIn_Item != null)
                {
                    // Quantity & LineTotal dihitung SpAdjustmentIn_UpdateItemQuantity dari
                    // popup batch -- jangan sampai tertimpa nilai grid saat edit Free Text.
                    var exceptColumns = new string[] { "DetId", "Id", "Quantity", "LineTotal" };
                    CopyProperty.CopyProperties(model, tx_AdjustmentIn_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tx_AdjustmentIn_Item.ModifiedDate = dtModified;
                    tx_AdjustmentIn_Item.ModifiedUser = UserId;
                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, AdjustmentIn_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {
                    // Batch & scale milik item ikut dihapus, supaya tidak jadi baris yatim.
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Item_Batch_Scale\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Item_Batch\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Item\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.SaveChanges();
                }
            }

        }

        public void Post(int userId, AdjustmentInModel adjustmentInModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var statusCheck = CONTEXT.Database.SqlQuery<StatusCheckModel>(@"
                    SELECT ""Status"", ""ApprovalStatus"", ""IsApproval""
                    FROM ""Tx_AdjustmentIn""
                    WHERE ""Id"" = :p0
                ", adjustmentInModel.Id).FirstOrDefault();

                if (statusCheck == null)
                    throw new Exception("[VALIDATION] Transaction not found.");

                // Mencegah Goods Receipt dobel di SAP kalau Post terkirim dua kali.
                if (statusCheck.Status != "Draft")
                    throw new Exception("[VALIDATION] Cannot post. Only Draft transaction can be posted.");

                if (statusCheck.ApprovalStatus == "Rejected")
                    throw new Exception("[VALIDATION] Cannot post. Transaction has been Rejected.");

                if (statusCheck.ApprovalStatus == "Waiting")
                    throw new Exception("[VALIDATION] Cannot post. Transaction is still waiting for Approval.");
            }

            Update(adjustmentInModel, "Post");
            PostSAP(userId, adjustmentInModel.Id);
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

                        AdjustmentInModel syncAdjustmentIn = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "post", "Id", keyValue);

                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(id);
                        if (tx_AdjustmentIn == null)
                        {
                            throw new Exception("[VALIDATION] Transaction not found.");
                        }

                        oCompany = SAPCachedCompany.GetCompany();

                        // Goods Receipt dan update status web harus jadi satu kesatuan:
                        // kalau hook "after" gagal, dokumen SAP ikut di-rollback.
                        oCompany.StartTransaction();

                        string docNum;
                        int docEntry = AddGoodsReceipt(oCompany, syncAdjustmentIn, out docNum);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_AdjustmentIn.PostingDate = dtModified;
                        tx_AdjustmentIn.DocEntry = docEntry;
                        tx_AdjustmentIn.DocNum = docNum;

                        tx_AdjustmentIn.Status = "Posted";
                        tx_AdjustmentIn.IsAfterPosted = "Y";
                        tx_AdjustmentIn.ModifiedDate = dtModified;
                        tx_AdjustmentIn.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "post", "Id", keyValue);

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

        // OIGN: satu baris per item yang Quantity-nya > 0.
        private int AddGoodsReceipt(SAPbobsCOM.Company oCompany, AdjustmentInModel model, out string docNum)
        {
            docNum = "";

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

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

                    if ((item.UnitPriceTc ?? 0) > 0)
                    {
                        oDoc.Lines.UnitPrice = (double)item.UnitPriceTc;
                    }

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
                            // SpAdjustmentIn_UpdateItemQuantity), kalau tidak SAP menolak -4014.
                            oDoc.Lines.BatchNumbers.Quantity = (double)itemBatch.Quantity;

                            if (itemBatch.AdmissionDate.HasValue)
                            {
                                oDoc.Lines.BatchNumbers.AddmisionDate = itemBatch.AdmissionDate.Value;
                            }

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

                    throw new Exception(string.Format("[VALIDATION] - Add Goods Receipt : {0}|{1}", nErr, errMsg));
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

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "cancel", "Id", keyValue);

                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(Id);
                        if (tx_AdjustmentIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentIn.Status = "Cancel";
                            tx_AdjustmentIn.ApprovalStatus = "Rejected";
                            tx_AdjustmentIn.CancelReason = cancelReason;
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "cancel", "Id", keyValue);


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

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "requestApproval", "Id", keyValue);

                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(id);
                        if (tx_AdjustmentIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentIn.IsApproval = "Y";
                            tx_AdjustmentIn.ApprovalMessages = approvalMessages;
                            tx_AdjustmentIn.ApprovalStatus = "Waiting";
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'AdjustmentIn',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "requestApproval", "Id", keyValue);
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

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'AdjustmentIn', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus""
                            FROM ""Tx_AdjustmentIn"" T0
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

        public AdjustmentInApprovalView___ GetViewApproval(long id)
        {
            AdjustmentInApprovalView___ model = new AdjustmentInApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_AdjustmentIn"" T0
                    LEFT JOIN ""Tx_AdjustmentIn_Approval"" T1 ON T0.""Id"" = T1.""Id""
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0
                ";

                model = CONTEXT.Database.SqlQuery<AdjustmentInApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetAdjustmentIn_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

        public AdjustmentInBatchView___ GetAdjustmentIn_Batch(long id, long detId)
        {
            string sql = null;
            AdjustmentInBatchView___ model = new AdjustmentInBatchView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName"", T1.""WhsCode"", T1.""WhsName""
                        FROM ""Tx_AdjustmentIn_Item"" T0
                        INNER JOIN ""Tx_AdjustmentIn"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentInBatchView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_AdjustmentIn_Item_Batch"" T0
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.AdjustmentInBatchModel___ = CONTEXT.Database.SqlQuery<AdjustmentInBatchModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public List<AdjustmentInBatchModel> GetAdjustmentIn_ItemBatchList(long id, long detId)
        {
            string sql = null;
            List<AdjustmentInBatchModel> model = new List<AdjustmentInBatchModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_AdjustmentIn_Item_Batch"" T0
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentInBatchModel>(sql, id, detId).ToList();
            }
            return model;
        }


        public long AdjustmentIn_AddNewItemBatch(AdjustmentInBatchModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_AdjustmentIn_Item_Batch tx_AdjustmentIn_Item_Batch = new Tx_AdjustmentIn_Item_Batch();
                        CopyProperty.CopyProperties(model, tx_AdjustmentIn_Item_Batch, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_AdjustmentIn_Item_Batch.CreatedDate = dtModified;
                        tx_AdjustmentIn_Item_Batch.CreatedUser = model._UserId;
                        tx_AdjustmentIn_Item_Batch.ModifiedDate = dtModified;
                        tx_AdjustmentIn_Item_Batch.ModifiedUser = model._UserId;

                        CONTEXT.Tx_AdjustmentIn_Item_Batch.Add(tx_AdjustmentIn_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_AdjustmentIn_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_AdjustmentIn_Item_Batch.Id.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_UpdateItemQuantity\"(:p0, 'Tx_AdjustmentIn_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                        SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn", "addItemBatch", "Id", keyValue);

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

        public void AdjustmentIn_UpdateItemBatch(AdjustmentInBatchModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "before", "AdjustmentIn", "updateItemBatch", "Id", keyValue);

                        Tx_AdjustmentIn_Item_Batch tx_AdjustmentIn_Item_Batch = CONTEXT.Tx_AdjustmentIn_Item_Batch.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_AdjustmentIn_Item_Batch != null)
                        {
                            var exceptColumns = new string[] { "Id", "DetId", "DetDetId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_AdjustmentIn_Item_Batch, false, exceptColumns);

                            tx_AdjustmentIn_Item_Batch.ModifiedDate = dtModified;
                            tx_AdjustmentIn_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_UpdateItemQuantity\"(:p0, 'Tx_AdjustmentIn_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn", "updateItemBatch", "Id", keyValue);

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

        public void AdjustmentIn_DeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetId != 0)
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(_userId, "AdjustmentIn", CONTEXT, "before", "AdjustmentIn", "deleteItemBatch", "Id", Id.ToString());

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_UpdateItemQuantity\"(:p0, 'Tx_AdjustmentIn_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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
