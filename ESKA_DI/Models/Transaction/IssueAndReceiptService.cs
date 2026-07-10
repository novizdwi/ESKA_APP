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

    public class IssueAndReceiptModel
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

        // Production Order
        public long? BaseEntry { get; set; }
        public string BaseDocNum { get; set; }

        // Process Card
        public long? BaseProcessCardId { get; set; }
        public string BaseProcessCardTransNo { get; set; }
        public int? BaseProcessCardSort { get; set; }
        public string BaseProcessCardRoutingCode { get; set; }
        public string BaseProcessCardRoutingName { get; set; }
        public int? BaseProcessCardOperatorId { get; set; }
        public string BaseProcessCardOperatorName { get; set; }

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
        public decimal? Netto { get; set; }
        public string Uom { get; set; }
        public string WhsCode { get; set; }
        public string MsnPrd { get; set; }
        public string Cost { get; set; }
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

    public class IssueAndReceiptBatchReceiptView___
    {
        public long Id { get; set; }

        public string BaseDocNum { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public int? Quantity { get; set; }

        public List<IssueAndReceiptBatchReceiptModel> IssueAndReceiptBatchReceiptModel___ { get; set; }
    }

    public class IssueAndReceiptBatchReceiptModel
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

    public class IssueAndReceiptBatchIssueView___
    {
        public long Id { get; set; }

        public string BaseDocNum { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string Whse { get; set; }

        public int? TotalNeeded { get; set; }

        public int? TotalCreated { get; set; }

        public List<IssueAndReceiptBatchIssueModel> IssueAndReceiptBatchIssueModel___ { get; set; }
    }

    public class IssueAndReceiptBatchIssueModel
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



    #endregion

    #region Services

    public class IssueAndReceiptService
    {

        public IssueAndReceiptModel GetNewModel(int userId)
        {
            IssueAndReceiptModel model = new IssueAndReceiptModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public IssueAndReceiptModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public IssueAndReceiptModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            IssueAndReceiptModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_IssueAndReceipt"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<IssueAndReceiptModel>(ssql, id).Single();

                model.ListIssueItem_ = this.IssueAndReceipt_IssueItemDetails(CONTEXT, id);

                model.ListIssueItem_ = this.IssueAndReceipt_IssueBatchDetails(CONTEXT, model.ListIssueItem_);

                model.ListReceiptItem_ = this.IssueAndReceipt_ReceiptItemDetails(CONTEXT, id);

                model.ListReceiptItem_ = this.IssueAndReceipt_ReceiptBatchDetails(CONTEXT, model.ListReceiptItem_);




                if (model.Status == "Draft")
                {
                    int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'IssueAndReceipt', :p1) ", userId, model.Id).FirstOrDefault();
                }
            }

            return model;
        }

        public List<IssueReceipt_IssueItemModel> IssueAndReceipt_IssueItemDetails(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return IssueAndReceipt_IssueItemDetails(CONTEXT, id);
            }

        }

        public List<IssueReceipt_IssueItemModel> IssueAndReceipt_IssueItemDetails(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_IssueAndReceipt_Issue_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var IssueAndReceipt_IssueItemDetails = CONTEXT.Database.SqlQuery<IssueReceipt_IssueItemModel>(ssql, id).ToList();
            return IssueAndReceipt_IssueItemDetails;
        }

        public List<IssueReceipt_IssueItemModel> IssueAndReceipt_IssueBatchDetails(HANA_APP CONTEXT,List<IssueReceipt_IssueItemModel> items)
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


        public List<IssueReceipt_ReceiptItemModel> IssueAndReceipt_ReceiptItemDetails(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return IssueAndReceipt_ReceiptItemDetails(CONTEXT, id);
            }

        }

        public List<IssueReceipt_ReceiptItemModel> IssueAndReceipt_ReceiptItemDetails(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_IssueAndReceipt_Receipt_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var IssueAndReceipt_ReceiptItemDetails = CONTEXT.Database.SqlQuery<IssueReceipt_ReceiptItemModel>(ssql, id).ToList();
            return IssueAndReceipt_ReceiptItemDetails;
        }

        public List<IssueReceipt_ReceiptItemModel> IssueAndReceipt_ReceiptBatchDetails(HANA_APP CONTEXT, List<IssueReceipt_ReceiptItemModel> items)
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
        public IssueAndReceiptModel NavFirst(int userId)
        {
            IssueAndReceiptModel model = null;
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
        public IssueAndReceiptModel NavPrevious(int userId, long id = 0)
        {
            IssueAndReceiptModel model = null;
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

        public IssueAndReceiptModel NavNext(int userId, long id = 0)
        {
            IssueAndReceiptModel model = null;
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

        public IssueAndReceiptModel NavLast(int userId)
        {
            IssueAndReceiptModel model = null;
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

        public IssueAndReceiptBatchReceiptView___ GetReceiptBatch(long id, long detId)
        {
            string sql = null;
            IssueAndReceiptBatchReceiptView___ model = new IssueAndReceiptBatchReceiptView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", 
                                T0.""BaseDocNum"",
                                T1.""DetId"", 
                                T1.""ItemCode"", 
                                T1.""ItemName"",
                                T1.""WhsCode"",
                                T1.""Quantity""
                                FROM ""Tx_IssueAndReceipt"" T0   
                                LEFT JOIN ""Tx_IssueAndReceipt_Receipt_Item"" T1 ON T0.""Id"" = T1.""Id"" 
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchReceiptView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_IssueAndReceipt_Receipt_Item_Batch"" T0   
                            WHERE ""DetId"" = :p0 ";

                model.IssueAndReceiptBatchReceiptModel___ = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchReceiptModel>(sql, detId).ToList();
            }

            return model;
        }

        public IssueAndReceiptBatchIssueView___ GetIssueBatch(long id, long detId)
        {
            string sql = null;
            IssueAndReceiptBatchIssueView___ model = new IssueAndReceiptBatchIssueView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", 
                                T0.""BaseDocNum"",
                                T1.""DetId"", 
                                T1.""ItemCode"", 
                                T1.""ItemName"",
                                T1.""WhsCode"",
                                T1.""Quantity""
                                FROM ""Tx_IssueAndReceipt"" T0   
                                LEFT JOIN ""Tx_IssueAndReceipt_Issue_Item"" T1 ON T0.""Id"" = T1.""Id"" 
                                WHERE T0.""Id""=:p0 AND T1.""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchIssueView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_IssueAndReceipt_Issue_Item_Batch"" T0   
                            WHERE ""DetId"" = :p0 ";

                model.IssueAndReceiptBatchIssueModel___ = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchIssueModel>(sql, detId).ToList();
            }

            return model;
        }

        public List<IssueAndReceiptBatchReceiptModel> IssueAndReceipt__ReceiptItemBatchList(long detId)
        {
            string sql = null;
            List<IssueAndReceiptBatchReceiptModel> model = new List<IssueAndReceiptBatchReceiptModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_IssueAndReceipt_Receipt_Item_Batch"" T0   
                            WHERE ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchReceiptModel>(sql, detId).ToList();
            }
            return model;
        }

        public List<IssueAndReceiptBatchReceiptModel> IssueAndReceipt__IssueItemBatchList(long detId)
        {
            string sql = null;
            List<IssueAndReceiptBatchReceiptModel> model = new List<IssueAndReceiptBatchReceiptModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_IssueAndReceipt_Issue_Item_Batch"" T0   
                            WHERE ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchReceiptModel>(sql, detId).ToList();
            }
            return model;
        }

        public long IssueAndReceipt__ReceiptAddNewItemBatch(IssueAndReceiptBatchReceiptModel model)
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

                        CONTEXT.Tx_IssueAndReceipt_Receipt_Item_Batch.Add(tx_IssueAndReceipt_Receipt_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_IssueAndReceipt_Receipt_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_IssueAndReceipt_Receipt_Item_Batch.DetId.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'Tx_IssueAndReceipt_Issue_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
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

        public long IssueAndReceipt__IssueAddNewItemBatch(IssueAndReceiptBatchIssueModel model)
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

                        CONTEXT.Tx_IssueAndReceipt_Issue_Item_Batch.Add(tx_IssueAndReceipt_issue_Item_Batch);
                        CONTEXT.SaveChanges();
                        detDetId = tx_IssueAndReceipt_issue_Item_Batch.DetDetId;

                        String keyValue;
                        keyValue = tx_IssueAndReceipt_issue_Item_Batch.DetId.ToString();

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'Tx_IssueAndReceipt_Issue_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
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


        public void IssueAndReceipt_ReceiptUpdateItemBatch(IssueAndReceiptBatchReceiptModel model)
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
                            var exceptColumns = new string[] { "DetId", "DetDetId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_IssueAndReceipt_Receipt_Item_Batch, false, exceptColumns);

                            tx_IssueAndReceipt_Receipt_Item_Batch.ModifiedDate = dtModified;
                            tx_IssueAndReceipt_Receipt_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'tx_IssueAndReceipt_Receipt_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);

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

        public void IssueAndReceipt_IssueUpdateItemBatch(IssueAndReceiptBatchIssueModel model)
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
                            var exceptColumns = new string[] { "DetId", "DetDetId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_IssueAndReceipt_Issue_Item_Batch, false, exceptColumns);

                            tx_IssueAndReceipt_Issue_Item_Batch.ModifiedDate = dtModified;
                            tx_IssueAndReceipt_Issue_Item_Batch.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'tx_IssueAndReceipt_Issue_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);

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


        public void IssueAndReceipt_ReceiptDeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
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

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'Tx_IssueAndReceipt_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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

        public void IssueAndReceipt_IssueDeleteItemBatch(int _userId, long Id, long DetId, long DetDetId)
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

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_UpdateReceiptItemQuantity\"(:p0, 'Tx_IssueAndReceipt_Issue_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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


    #endregion

}