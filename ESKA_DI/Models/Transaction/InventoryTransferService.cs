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

    public class InventoryTransferModel
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

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsName { get; set; }

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

        public List<InventoryTransfer_DetailModel> ListDetail_ = new List<InventoryTransfer_DetailModel>();

        public InventoryTransfer_Detail Details_ { get; set; }
    }

    public class InventoryTransfer_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<InventoryTransfer_DetailModel> insertedRowValues { get; set; }
        public List<InventoryTransfer_DetailModel> modifiedRowValues { get; set; }
    }

    public class InventoryTransfer_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<InventoryTransfer_ApprovalModel> insertedRowValues { get; set; }
        public List<InventoryTransfer_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class InventoryTransfer_ApprovalModel
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

    public class InventoryTransfer_DetailModel
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

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Netto { get; set; }

        // OITM.ManBtchNum: batch hanya dikirim ke SAP untuk item yang batch-managed.
        public string ManBtchNum_ { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public string FreeText { get; set; }

        public List<InventoryTransferBatchModel> ListItemBatch_ = new List<InventoryTransferBatchModel>();
    }

    public class InventoryTransferApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<InventoryTransfer_ApprovalModel> ApprovalStepList__ = new List<InventoryTransfer_ApprovalModel>();

        public InventoryTransfer_Approval ApprovalStep__ { get; set; }
    }

    public class InventoryTransferBatchView___
    {
        public int? RowNo { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public List<InventoryTransferBatchModel> InventoryTransferBatchModel___ { get; set; }

        public InventoryTransfer_DetailBatch DetailBatchs_ { get; set; }
    }

    public class InventoryTransfer_DetailBatch
    {
        public List<long> deletedRowKeys { get; set; }
        public List<InventoryTransferBatchModel> insertedRowValues { get; set; }
        public List<InventoryTransferBatchModel> modifiedRowValues { get; set; }
    }

    public class InventoryTransferBatchModel
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

    public class InventoryTransferService
    {

        public InventoryTransferModel GetNewModel(int userId)
        {
            InventoryTransferModel model = new InventoryTransferModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public InventoryTransferModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public InventoryTransferModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            InventoryTransferModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_InventoryTransfer"" T0
                            WHERE T0.""Id"" = :p0
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<InventoryTransferModel>(ssql, id).SingleOrDefault();
                if (model == null)
                {
                    return null;
                }

                model.ListDetail_ = this.InventoryTransfer_Details(CONTEXT, id, method);

                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'InventoryTransfer', :p1) ", userId, model.Id).FirstOrDefault();
                    model.ApprovalTemplateId_ = approvalId;
                }

                if (model.ApprovalStatus == "Waiting")
                {
                    string getDocNum = @"SELECT 'Y'
                            FROM ""Tx_InventoryTransfer"" T0
                            INNER JOIN  ""Tx_InventoryTransfer_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
                            WHERE T0.""Id"" = :p0
                            AND T1.""UserId"" = :p1
                        ";
                    model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                }

            }

            return model;
        }

        public List<InventoryTransfer_DetailModel> InventoryTransfer_Details(long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return InventoryTransfer_Details(CONTEXT, id, method);
            }

        }

        public List<InventoryTransfer_DetailModel> InventoryTransfer_Details(HANA_APP CONTEXT, long id = 0, string method = "")
        {

            string ssql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetId"") AS ""RowNo"",
                T0.*, 
                T3.""ManBtchNum"" AS ""ManBtchNum_""
            FROM ""Tx_InventoryTransfer_Item"" T0
            INNER JOIN ""Tx_InventoryTransfer"" T1 ON T0.""Id"" = T1.""Id"" 
            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITM"" T3 ON T0.""ItemCode"" = T3.""ItemCode""
            WHERE T0.""Id"" =:p0
            ORDER BY T0.""DetId"" ASC
            ";
            var inventoryTransfer = CONTEXT.Database.SqlQuery<InventoryTransfer_DetailModel>(ssql, id).ToList();

            if (method == "Post" && inventoryTransfer.Count != 0)
            {
                string ssqlBatch = @"
                    SELECT *
                    FROM ""Tx_InventoryTransfer_Item_Batch""
                    WHERE ""Id"" = :p0
                    ORDER BY ""DetId"", ""DetDetId""
                ";

                var itemBatch = CONTEXT.Database.SqlQuery<InventoryTransferBatchModel>(ssqlBatch, id).ToList();
                var batchLookup = itemBatch.ToLookup(x => x.DetId);
                foreach (var item in inventoryTransfer)
                {
                    item.ListItemBatch_ = batchLookup[item.DetId].ToList();
                }
            }

            return inventoryTransfer;
        }

        public List<InventoryTransfer_ApprovalModel> GetInventoryTransfer_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetInventoryTransfer_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<InventoryTransfer_ApprovalModel> GetInventoryTransfer_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_InventoryTransfer_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<InventoryTransfer_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public InventoryTransferModel NavFirst(int userId)
        {
            InventoryTransferModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryTransfer");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryTransfer\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public InventoryTransferModel NavPrevious(int userId, long id = 0)
        {
            InventoryTransferModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryTransfer");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryTransfer\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public InventoryTransferModel NavNext(int userId, long id = 0)
        {
            InventoryTransferModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryTransfer");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryTransfer\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public InventoryTransferModel NavLast(int userId)
        {
            InventoryTransferModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryTransfer");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryTransfer\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(InventoryTransferModel model)
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
                            Tx_InventoryTransfer tx_InventoryTransfer = new Tx_InventoryTransfer();
                            CopyProperty.CopyProperties(model, tx_InventoryTransfer, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_InventoryTransfer.TransType = "InventoryTransfer";
                            tx_InventoryTransfer.CreatedDate = dtModified;
                            tx_InventoryTransfer.CreatedUser = model._UserId;
                            tx_InventoryTransfer.ModifiedDate = dtModified;
                            tx_InventoryTransfer.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'InventoryTransfer','" + dateX + "','') ").SingleOrDefault();
                            tx_InventoryTransfer.TransNo = transNo;

                            CONTEXT.Tx_InventoryTransfer.Add(tx_InventoryTransfer);
                            CONTEXT.SaveChanges();
                            Id = tx_InventoryTransfer.Id;

                            String keyValue;
                            keyValue = tx_InventoryTransfer.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "InventoryTransfer", CONTEXT, "after", "InventoryTransfer", "add", "Id", keyValue);

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

        public void Update(InventoryTransferModel model, string method = "")
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

                            SpNotif.SpSysControllerTransNotif(model._UserId, "InventoryTransfer", CONTEXT, "before", "InventoryTransfer", "update", "Id", keyValue);

                            Tx_InventoryTransfer tx_InventoryTransfer = CONTEXT.Tx_InventoryTransfer.Find(model.Id);
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            if (tx_InventoryTransfer != null)
                            {
                                var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                CopyProperty.CopyProperties(model, tx_InventoryTransfer, false, exceptColumns);

                                tx_InventoryTransfer.ModifiedDate = dtModified;
                                tx_InventoryTransfer.ModifiedUser = model._UserId;

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
                                            InventoryTransfer_DetailModel detailModel = new InventoryTransfer_DetailModel();
                                            detailModel.DetId = detId;
                                            Detail_Delete(CONTEXT, detailModel);
                                        }
                                    }
                                }

                                CONTEXT.SaveChanges();

                                // Gudang asal & tujuan di baris selalu mengikuti header.
                                CONTEXT.Database.ExecuteSqlCommand(@"
                                    UPDATE T0 SET
                                        ""FromWhsCode"" = (SELECT H.""FromWhsCode"" FROM ""Tx_InventoryTransfer"" H WHERE H.""Id"" = T0.""Id""),
                                        ""FromWhsName"" = (SELECT H.""FromWhsName"" FROM ""Tx_InventoryTransfer"" H WHERE H.""Id"" = T0.""Id""),
                                        ""ToWhsCode"" = (SELECT H.""ToWhsCode"" FROM ""Tx_InventoryTransfer"" H WHERE H.""Id"" = T0.""Id""),
                                        ""ToWhsName"" = (SELECT H.""ToWhsName"" FROM ""Tx_InventoryTransfer"" H WHERE H.""Id"" = T0.""Id"")
                                    FROM ""Tx_InventoryTransfer_Item"" T0
                                    WHERE T0.""Id"" = :p0", model.Id);

                                SpNotif.SpSysControllerTransNotif(model._UserId, "InventoryTransfer", CONTEXT, "after", "InventoryTransfer", "update", "Id", keyValue);

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
                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "before", "InventoryTransfer", "ChooseItem", "Id", keyValue);

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

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpInventoryTransfer_ChooseItem\"(:p0,:p1,:p2,:p3)", UserId, Id, sqlWhere, sorting ?? "");

                            SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "InventoryTransfer", "ChooseItem", "Id", keyValue);

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

        public long Detail_Add(HANA_APP CONTEXT, InventoryTransfer_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_InventoryTransfer_Item tx_InventoryTransfer_Item = new Tx_InventoryTransfer_Item();

                CopyProperty.CopyProperties(model, tx_InventoryTransfer_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_InventoryTransfer_Item.Id = Id;
                tx_InventoryTransfer_Item.CreatedDate = dtModified;
                tx_InventoryTransfer_Item.CreatedUser = UserId;
                tx_InventoryTransfer_Item.ModifiedDate = dtModified;
                tx_InventoryTransfer_Item.ModifiedUser = UserId;

                CONTEXT.Tx_InventoryTransfer_Item.Add(tx_InventoryTransfer_Item);
                CONTEXT.SaveChanges();
                DetId = tx_InventoryTransfer_Item.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, InventoryTransfer_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_InventoryTransfer_Item tx_InventoryTransfer_Item = CONTEXT.Tx_InventoryTransfer_Item.Find(model.DetId);

                if (tx_InventoryTransfer_Item != null)
                {
                    // Quantity dihitung SpInventoryTransfer_UpdateItemQuantity dari popup batch dan
                    // gudang baris selalu disalin dari header -- jangan tertimpa nilai grid.
                    var exceptColumns = new string[] { "DetId", "Id", "Quantity", "FromWhsCode", "FromWhsName", "ToWhsCode", "ToWhsName" };
                    CopyProperty.CopyProperties(model, tx_InventoryTransfer_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tx_InventoryTransfer_Item.ModifiedDate = dtModified;
                    tx_InventoryTransfer_Item.ModifiedUser = UserId;
                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, InventoryTransfer_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {
                    // Batch & scale milik item ikut dihapus, supaya tidak jadi baris yatim.
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryTransfer_Item_Batch_Scale\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryTransfer_Item_Batch\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryTransfer_Item\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.SaveChanges();
                }
            }

        }

        public void Post(int userId, InventoryTransferModel inventoryTransferModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var statusCheck = CONTEXT.Database.SqlQuery<StatusCheckModel>(@"
                    SELECT ""Status"", ""ApprovalStatus"", ""IsApproval""
                    FROM ""Tx_InventoryTransfer""
                    WHERE ""Id"" = :p0
                ", inventoryTransferModel.Id).FirstOrDefault();

                if (statusCheck == null)
                    throw new Exception("[VALIDATION] Transaction not found.");

                // Mencegah Inventory Transfer dobel di SAP kalau Post terkirim dua kali.
                if (statusCheck.Status != "Draft")
                    throw new Exception("[VALIDATION] Cannot post. Only Draft transaction can be posted.");

                if (statusCheck.ApprovalStatus == "Rejected")
                    throw new Exception("[VALIDATION] Cannot post. Transaction has been Rejected.");

                if (statusCheck.ApprovalStatus == "Waiting")
                    throw new Exception("[VALIDATION] Cannot post. Transaction is still waiting for Approval.");
            }

            Update(inventoryTransferModel, "Post");
            PostSAP(userId, inventoryTransferModel.Id);
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

                        InventoryTransferModel syncInventoryTransfer = GetById(userId, id, "Post");

                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "before", "Tx_InventoryTransfer", "post", "Id", keyValue);

                        Tx_InventoryTransfer tx_InventoryTransfer = CONTEXT.Tx_InventoryTransfer.Find(id);
                        if (tx_InventoryTransfer == null)
                        {
                            throw new Exception("[VALIDATION] Transaction not found.");
                        }

                        oCompany = SAPCachedCompany.GetCompany();

                        // Inventory Transfer dan update status web harus jadi satu kesatuan:
                        // kalau hook "after" gagal, dokumen SAP ikut di-rollback.
                        oCompany.StartTransaction();

                        string docNum;
                        int docEntry = AddInventoryTransfer(oCompany, syncInventoryTransfer, out docNum);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_InventoryTransfer.PostingDate = dtModified;
                        tx_InventoryTransfer.DocEntry = docEntry;
                        tx_InventoryTransfer.DocNum = docNum;

                        tx_InventoryTransfer.Status = "Posted";
                        tx_InventoryTransfer.IsAfterPosted = "Y";
                        tx_InventoryTransfer.ModifiedDate = dtModified;
                        tx_InventoryTransfer.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "after", "Tx_InventoryTransfer", "post", "Id", keyValue);

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

        // OWTR: satu baris per item yang Quantity-nya > 0. Gudang asal & tujuan baris
        // selalu disamakan dengan header.
        private int AddInventoryTransfer(SAPbobsCOM.Company oCompany, InventoryTransferModel model, out string docNum)
        {
            docNum = "";

            SAPbobsCOM.StockTransfer oInventoryTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

            try
            {
                oInventoryTransfer.DocDate = DateTime.Now;
                oInventoryTransfer.FromWarehouse = model.FromWhsCode;
                oInventoryTransfer.ToWarehouse = model.ToWhsCode;

                if (!string.IsNullOrWhiteSpace(model.Comments))
                {
                    oInventoryTransfer.Comments = model.Comments;
                    // JrnlMemo di SAP maksimal 50 karakter.
                    oInventoryTransfer.JournalMemo = model.Comments.Length > 50 ? model.Comments.Substring(0, 50) : model.Comments;
                }

                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebId").Value = model.Id.ToString();
                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo ?? "";

                bool firstLine = true;

                foreach (var item in model.ListDetail_)
                {
                    if ((item.Quantity ?? 0) <= 0)
                    {
                        continue;
                    }

                    if (!firstLine)
                    {
                        oInventoryTransfer.Lines.Add();
                    }
                    firstLine = false;

                    oInventoryTransfer.Lines.ItemCode = item.ItemCode;
                    oInventoryTransfer.Lines.FromWarehouseCode = model.FromWhsCode;
                    oInventoryTransfer.Lines.WarehouseCode = model.ToWhsCode;
                    oInventoryTransfer.Lines.Quantity = (double)item.Quantity;

                    oInventoryTransfer.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = (item.Id ?? 0).ToString();
                    oInventoryTransfer.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = (item.DetId ?? 0).ToString();

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
                                oInventoryTransfer.Lines.BatchNumbers.Add();
                            }

                            oInventoryTransfer.Lines.BatchNumbers.BatchNumber = itemBatch.Batch;

                            // WAJIB Quantity, bukan Netto: total BatchNumbers.Quantity harus sama
                            // dengan Lines.Quantity (= SUM batch Quantity), kalau tidak SAP menolak -4014.
                            oInventoryTransfer.Lines.BatchNumbers.Quantity = (double)itemBatch.Quantity;

                            batchIndex++;
                        }
                    }
                }

                if (firstLine)
                {
                    throw new Exception("[VALIDATION] - No item with quantity greater than zero");
                }

                if (oInventoryTransfer.Add() != 0)
                {
                    int nErr = oCompany.GetLastErrorCode();
                    string errMsg = oCompany.GetLastErrorDescription();

                    throw new Exception(string.Format("[VALIDATION] - Add Inventory Transfer : {0}|{1}", nErr, errMsg));
                }

                int docEntry = Convert.ToInt32(oCompany.GetNewObjectKey());

                // DocNum dibaca lewat DI API (koneksi yang sama), karena dokumennya belum
                // di-commit sehingga belum terlihat dari koneksi HANA_APP.
                if (oInventoryTransfer.GetByKey(docEntry))
                {
                    docNum = oInventoryTransfer.DocNum.ToString();
                }

                return docEntry;
            }
            finally
            {
                SapCompany.CleanUp(oInventoryTransfer);
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

                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "before", "Tx_InventoryTransfer", "cancel", "Id", keyValue);

                        Tx_InventoryTransfer tx_InventoryTransfer = CONTEXT.Tx_InventoryTransfer.Find(Id);
                        if (tx_InventoryTransfer != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_InventoryTransfer.Status = "Cancel";
                            tx_InventoryTransfer.ApprovalStatus = "Rejected";
                            tx_InventoryTransfer.CancelReason = cancelReason;
                            tx_InventoryTransfer.ModifiedDate = dtModified;
                            tx_InventoryTransfer.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "after", "Tx_InventoryTransfer", "cancel", "Id", keyValue);


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

                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "before", "Tx_InventoryTransfer", "requestApproval", "Id", keyValue);

                        Tx_InventoryTransfer tx_InventoryTransfer = CONTEXT.Tx_InventoryTransfer.Find(id);
                        if (tx_InventoryTransfer != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_InventoryTransfer.IsApproval = "Y";
                            tx_InventoryTransfer.ApprovalMessages = approvalMessages;
                            tx_InventoryTransfer.ApprovalStatus = "Waiting";
                            tx_InventoryTransfer.ModifiedDate = dtModified;
                            tx_InventoryTransfer.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'InventoryTransfer',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "after", "Tx_InventoryTransfer", "requestApproval", "Id", keyValue);
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

                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "before", "Tx_InventoryTransfer", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'InventoryTransfer', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "InventoryTransfer", CONTEXT, "after", "Tx_InventoryTransfer", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus""
                            FROM ""Tx_InventoryTransfer"" T0
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

        public InventoryTransferApprovalView___ GetViewApproval(long id)
        {
            InventoryTransferApprovalView___ model = new InventoryTransferApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_InventoryTransfer"" T0
                    LEFT JOIN ""Tx_InventoryTransfer_Approval"" T1 ON T0.""Id"" = T1.""Id""
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0
                ";

                model = CONTEXT.Database.SqlQuery<InventoryTransferApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetInventoryTransfer_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

        public InventoryTransferBatchView___ GetInventoryTransfer_Batch(long id, long detId)
        {
            string sql = null;
            InventoryTransferBatchView___ model = new InventoryTransferBatchView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName"", T1.""FromWhsCode"" AS ""WhsCode"", T1.""FromWhsName"" AS ""WhsName""
                        FROM ""Tx_InventoryTransfer_Item"" T0
                        INNER JOIN ""Tx_InventoryTransfer"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<InventoryTransferBatchView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_InventoryTransfer_Item_Batch"" T0
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.InventoryTransferBatchModel___ = CONTEXT.Database.SqlQuery<InventoryTransferBatchModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public List<InventoryTransferBatchModel> GetInventoryTransfer_ItemBatchList(long id, long detId)
        {
            string sql = null;
            List<InventoryTransferBatchModel> model = new List<InventoryTransferBatchModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_InventoryTransfer_Item_Batch"" T0
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<InventoryTransferBatchModel>(sql, id, detId).ToList();
            }
            return model;
        }


        public long InventoryTransfer_AddNewItemBatch(InventoryTransferBatchModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_InventoryTransfer_Item_Batch tx_InventoryTransfer_Item_Batch = new Tx_InventoryTransfer_Item_Batch();
                        CopyProperty.CopyProperties(model, tx_InventoryTransfer_Item_Batch, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_InventoryTransfer_Item_Batch.CreatedDate = dtModified;
                        tx_InventoryTransfer_Item_Batch.CreatedUser = model._UserId;
                        tx_InventoryTransfer_Item_Batch.ModifiedDate = dtModified;
                        tx_InventoryTransfer_Item_Batch.ModifiedUser = model._UserId;

                        CONTEXT.Tx_InventoryTransfer_Item_Batch.Add(tx_InventoryTransfer_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_InventoryTransfer_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_InventoryTransfer_Item_Batch.Id.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpInventoryTransfer_UpdateItemQuantity\"(:p0, 'Tx_InventoryTransfer_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                        SpNotif.SpSysControllerTransNotif(model._UserId, "InventoryTransfer", CONTEXT, "after", "InventoryTransfer", "addItemBatch", "Id", keyValue);

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

        public void InventoryTransfer_UpdateItemBatch(InventoryTransferBatchModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "InventoryTransfer", CONTEXT, "before", "InventoryTransfer", "updateItemBatch", "Id", keyValue);

                        Tx_InventoryTransfer_Item_Batch tx_InventoryTransfer_Item_Batch = CONTEXT.Tx_InventoryTransfer_Item_Batch.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_InventoryTransfer_Item_Batch != null)
                        {
                            var exceptColumns = new string[] { "Id", "DetId", "DetDetId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_InventoryTransfer_Item_Batch, false, exceptColumns);

                            tx_InventoryTransfer_Item_Batch.ModifiedDate = dtModified;
                            tx_InventoryTransfer_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpInventoryTransfer_UpdateItemQuantity\"(:p0, 'Tx_InventoryTransfer_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                            SpNotif.SpSysControllerTransNotif(model._UserId, "InventoryTransfer", CONTEXT, "after", "InventoryTransfer", "updateItemBatch", "Id", keyValue);

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

        public void InventoryTransfer_DeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetId != 0)
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(_userId, "InventoryTransfer", CONTEXT, "before", "InventoryTransfer", "deleteItemBatch", "Id", Id.ToString());

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryTransfer_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryTransfer_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpInventoryTransfer_UpdateItemQuantity\"(:p0, 'Tx_InventoryTransfer_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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
