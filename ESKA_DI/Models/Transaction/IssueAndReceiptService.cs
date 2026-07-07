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
                FROM ""Tx_IssueAndReceipt_Issue_Item"" T0
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


    }


    #endregion

}