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

        public string WhsCode { get; set; }

        public int? Quantity { get; set; }

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

        public long Add(IssueAndReceiptModel model)
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

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpIssueAndReceipt_AddItemDetail\"(:p0,:p1,:p2, :p3,'Add')", model._UserId, Id, model.BaseProcessCardId, model.BaseProcessCardSort);


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

        public void Update(IssueAndReceiptModel model, string method = "")
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
            }
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

        public List<IssueAndReceiptBatchIssueModel> IssueAndReceipt__IssueItemBatchList(long detId)
        {
            string sql = null;
            List<IssueAndReceiptBatchIssueModel> model = new List<IssueAndReceiptBatchIssueModel>();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.*
                            FROM ""Tx_IssueAndReceipt_Issue_Item_Batch"" T0
                            WHERE ""DetId"" = :p0 ";

                model = CONTEXT.Database.SqlQuery<IssueAndReceiptBatchIssueModel>(sql, detId).ToList();
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

                        // Hitung ulang Netto item ISSUE dari batch-nya.
                        // (SP SpIssueAndReceipt_UpdateReceiptItemQuantity hanya melayani sisi Receipt --
                        //  bila dipakai utk Issue, ia malah menimpa Netto item Receipt ber-DetId sama.)
                        CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0), 0) WHERE \"DetId\"=:p1", model.DetId, model.DetId);
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
                            // Hitung ulang Netto item ISSUE dari batch-nya.
                        // (SP SpIssueAndReceipt_UpdateReceiptItemQuantity hanya melayani sisi Receipt --
                        //  bila dipakai utk Issue, ia malah menimpa Netto item Receipt ber-DetId sama.)
                        CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0), 0) WHERE \"DetId\"=:p1", model.DetId, model.DetId);

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

                            // Hitung ulang Netto item RECEIPT dari batch tersisa.
                            // (Sebelumnya memanggil SP dgn nama tabel yang tidak ada: 'Tx_IssueAndReceipt_Item_Batch'.)
                            CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Receipt_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Receipt_Item_Batch\" WHERE \"DetId\"=:p0), 0) WHERE \"DetId\"=:p1", DetId, DetId);
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

                            // Hitung ulang Netto item ISSUE dari batch tersisa.
                            // (SP "SpIssueAndReceipt_UpdateIssueItemQuantity" TIDAK ADA di database --
                            //  inilah penyebab error saat delete. SP yang ada hanya versi Receipt.)
                            CONTEXT.Database.ExecuteSqlCommand("UPDATE \"Tx_IssueAndReceipt_Issue_Item\" SET \"Netto\" = COALESCE((SELECT SUM(\"Netto\") FROM \"Tx_IssueAndReceipt_Issue_Item_Batch\" WHERE \"DetId\"=:p0), 0) WHERE \"DetId\"=:p1", DetId, DetId);
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

        #region Post / Cancel ke SAP

        // Hasil Add satu dokumen SAP.
        public class IssueReceipt_PostResult
        {
            public long? DocEntry { get; set; }
            public string DocNum { get; set; }
        }

        public void Post(int userId, IssueAndReceiptModel model)
        {
            PostSAP(userId, model.Id);
        }

        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;
            IssueAndReceiptModel sync = GetById(userId, id);

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
                        throw new Exception("[VALIDATION] - IssueAndReceipt tidak ditemukan");
                    }
                    if (tx.Status != "Draft")
                    {
                        throw new Exception("[VALIDATION] - Hanya dokumen Draft yang bisa di-Post");
                    }
                    if ((sync.BaseEntry ?? 0) == 0)
                    {
                        throw new Exception("[VALIDATION] - Work Order (Production Order) belum dipilih");
                    }

                    bool anyIssue = sync.ListIssueItem_ != null && sync.ListIssueItem_.Any(x => (x.Quantity ?? 0) > 0);
                    bool anyReceipt = sync.ListReceiptItem_ != null && sync.ListReceiptItem_.Any(x => (x.Quantity ?? 0) > 0);
                    if (!anyIssue && !anyReceipt)
                    {
                        throw new Exception("[VALIDATION] - Tidak ada item Issue maupun Receipt untuk di-Post");
                    }

                    // Validasi kelengkapan batch: untuk item yang dikelola batch (OITM.ManBtchNum='Y'),
                    // SAP menuntut total qty batch per baris = qty baris (error -4014 bila tidak).
                    // Divalidasi di sini agar pesannya menunjuk item & angkanya, bukan -4014 mentah.
                    var allItemCodes = new List<string>();
                    if (sync.ListIssueItem_ != null)
                    {
                        allItemCodes.AddRange(sync.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0 && !string.IsNullOrEmpty(x.ItemCode)).Select(x => x.ItemCode));
                    }
                    if (sync.ListReceiptItem_ != null)
                    {
                        allItemCodes.AddRange(sync.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0 && !string.IsNullOrEmpty(x.ItemCode)).Select(x => x.ItemCode));
                    }
                    allItemCodes = allItemCodes.Distinct().ToList();

                    var batchManagedItems = new List<string>();
                    if (allItemCodes.Any())
                    {
                        string inList = string.Join(",", allItemCodes.Select(c => "'" + c.Replace("'", "''") + "'"));
                        batchManagedItems = CONTEXT.Database.SqlQuery<string>(
                            "SELECT \"ItemCode\" FROM \"" + DbProvider.dbSap_Name + "\".\"OITM\" WHERE \"ManBtchNum\"='Y' AND \"ItemCode\" IN (" + inList + ")").ToList();
                    }

                    var batchErrors = new List<string>();
                    if (sync.ListIssueItem_ != null)
                    {
                        foreach (var it in sync.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0))
                        {
                            if (!batchManagedItems.Contains(it.ItemCode)) continue;
                            decimal sumBatch = it.ListBatch_ == null ? 0 : it.ListBatch_.Sum(b => b.Quantity ?? 0);
                            if (sumBatch != (it.Quantity ?? 0))
                            {
                                batchErrors.Add(string.Format("Issue {0}: total batch {1} <> qty item {2}", it.ItemCode, sumBatch, it.Quantity ?? 0));
                            }
                        }
                    }
                    if (sync.ListReceiptItem_ != null)
                    {
                        foreach (var it in sync.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0))
                        {
                            if (!batchManagedItems.Contains(it.ItemCode)) continue;
                            decimal sumBatch = it.ListBatch_ == null ? 0 : it.ListBatch_.Sum(b => b.Quantity ?? 0);
                            if (sumBatch != (it.Quantity ?? 0))
                            {
                                batchErrors.Add(string.Format("Receipt {0}: total batch {1} <> qty item {2}", it.ItemCode, sumBatch, it.Quantity ?? 0));
                            }
                        }
                    }
                    if (batchErrors.Any())
                    {
                        throw new Exception("[VALIDATION] - Batch belum lengkap (total batch harus = qty item):\n" + string.Join("\n", batchErrors));
                    }

                    // 2 dokumen dalam SATU transaksi SAP: bila salah satu gagal, keduanya rollback.
                    IssueReceipt_PostResult issRes = AddIssueForProduction(CONTEXT, oCompany, sync);
                    IssueReceipt_PostResult recRes = AddReceiptFromProduction(CONTEXT, oCompany, sync);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    if (issRes != null)
                    {
                        tx.IssueDocEntry = issRes.DocEntry;
                        tx.IssueDocNum = issRes.DocNum;
                        tx.IssueDocDate = dtModified;
                    }
                    if (recRes != null)
                    {
                        tx.ReceiptDocEntry = recRes.DocEntry;
                        tx.ReceiptDocNum = recRes.DocNum;
                        tx.ReceiptDocDate = dtModified;
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

                    throw new Exception(ex.Message.StartsWith("[VALIDATION]") ? ex.Message : string.Format("[VALIDATION] {0} ", ex.Message));
                }
                finally
                {
                    SAPCachedCompany.Release(oCompany);
                }
            }
        }

        // BaseLine komponen di Production Order (WOR1) berdasarkan ItemCode. -1 bila tidak ketemu.
        private int GetWor1BaseLine(HANA_APP CONTEXT, long baseEntry, string itemCode)
        {
            int? line = CONTEXT.Database.SqlQuery<int?>(
                "SELECT \"LineNum\" AS IDU FROM \"" + DbProvider.dbSap_Name + "\".\"WOR1\" WHERE \"DocEntry\"=:p0 AND \"ItemCode\"=:p1",
                baseEntry, itemCode ?? "").FirstOrDefault();
            return line ?? -1;
        }

        // Issue for Production = Goods Issue (oInventoryGenExit / ObjType 60), baris tertaut Production Order (BaseType 202).
        private IssueReceipt_PostResult AddIssueForProduction(HANA_APP CONTEXT, SAPbobsCOM.Company oCompany, IssueAndReceiptModel model)
        {
            if (model.ListIssueItem_ == null || !model.ListIssueItem_.Any(x => (x.Quantity ?? 0) > 0))
            {
                return null;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            oDoc.DocDate = DateTime.Now;
            // TaxDate TIDAK boleh diisi utk dokumen bertaut Production Order (SAP -5002 [OIGE.TaxDate]).

            foreach (var item in model.ListIssueItem_.Where(x => (x.Quantity ?? 0) > 0))
            {
                oDoc.Lines.BaseType = 202; // Production Order
                oDoc.Lines.BaseEntry = Convert.ToInt32(model.BaseEntry);
                int baseLine = GetWor1BaseLine(CONTEXT, model.BaseEntry ?? 0, item.ItemCode);
                if (baseLine >= 0)
                {
                    oDoc.Lines.BaseLine = baseLine;
                }

                //oDoc.Lines.ItemCode = item.ItemCode;
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
                throw new Exception("[VALIDATION] - Issue for Production : " + nErr + "|" + errMsg);
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

        // Receipt from Production = Goods Receipt (oInventoryGenEntry / ObjType 59), baris tertaut Production Order (BaseType 202).
        private IssueReceipt_PostResult AddReceiptFromProduction(HANA_APP CONTEXT, SAPbobsCOM.Company oCompany, IssueAndReceiptModel model)
        {
            if (model.ListReceiptItem_ == null || !model.ListReceiptItem_.Any(x => (x.Quantity ?? 0) > 0))
            {
                return null;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
            oDoc.DocDate = DateTime.Now;
            // TaxDate TIDAK boleh diisi utk dokumen bertaut Production Order (SAP -5002 [OIGN.TaxDate]).

            foreach (var item in model.ListReceiptItem_.Where(x => (x.Quantity ?? 0) > 0))
            {
                oDoc.Lines.BaseType = 202; // Production Order
                oDoc.Lines.BaseEntry = Convert.ToInt32(model.BaseEntry);
                int baseLine = GetWor1BaseLine(CONTEXT, model.BaseEntry ?? 0, item.ItemCode);
                if (baseLine >= 0)
                {
                    oDoc.Lines.BaseLine = baseLine;
                }

                //oDoc.Lines.ItemCode = item.ItemCode;
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
                throw new Exception("[VALIDATION] - Receipt from Production : " + nErr + "|" + errMsg);
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