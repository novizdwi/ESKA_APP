using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class IssueAndReceiptController : BaseController
    {

        string VIEW_TAB_CONTENT_Issue = "Partial/IssueAndReceipt_Form_TabIssueDetail_List_Partial";
        string VIEW_TAB_CONTENT_Receipt = "Partial/IssueAndReceipt_Form_TabReceiptDetail_List_Partial";

        public ActionResult TabIssueDetailListPartial()
        {
            int userId = (int)Session["userId"];

            issueAndReceiptService = new IssueAndReceiptService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<IssueReceipt_IssueItemModel> modelList = issueAndReceiptService.IssueAndReceipt_IssueItemDetails(Id);

            var header = issueAndReceiptService.GetById(userId, Id);
            ViewBag.ReadOnly = header == null || header.Status != "Draft";

            return PartialView(VIEW_TAB_CONTENT_Issue, modelList);
        }

        public ActionResult TabReceiptDetailListPartial()
        {
            int userId = (int)Session["userId"];

            issueAndReceiptService = new IssueAndReceiptService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<IssueReceipt_ReceiptItemModel> modelList = issueAndReceiptService.IssueAndReceipt_ReceiptItemDetails(Id);

            var header = issueAndReceiptService.GetById(userId, Id);
            ViewBag.ReadOnly = header == null || header.Status != "Draft";

            return PartialView(VIEW_TAB_CONTENT_Receipt, modelList);
        }


        // ===== Standalone: tambah/hapus/ubah baris item via CFL =====

        [HttpPost, ValidateInput(false)]
        public ContentResult ChooseItemIssue(long Id, string ItemCode, string ItemName, string Uom, string WhsCode, decimal? Quantity)
        {
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();

            var model = new IssueReceipt_IssueItemModel
            {
                _UserId = userId,
                Id = Id,
                ItemCode = ItemCode,
                ItemName = ItemName,
                Uom = Uom,
                WhsCode = WhsCode,
                Quantity = Quantity ?? 0
            };
            long detId = issueAndReceiptService.IssueItem_Add(model);
            return Content(detId.ToString());
        }

        [HttpPost, ValidateInput(false)]
        public ContentResult ChooseItemReceipt(long Id, string ItemCode, string ItemName, string Uom, string WhsCode, decimal? Quantity)
        {
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();

            var model = new IssueReceipt_ReceiptItemModel
            {
                _UserId = userId,
                Id = Id,
                ItemCode = ItemCode,
                ItemName = ItemName,
                Uom = Uom,
                WhsCode = WhsCode,
                Quantity = Quantity ?? 0
            };
            long detId = issueAndReceiptService.ReceiptItem_Add(model);
            return Content(detId.ToString());
        }

        [HttpPost, ValidateInput(false)]
        public ContentResult UpdateQtyIssue(long DetId, decimal? Quantity, string WhsCode, string MsnPrd, string Department, string Cost)
        {
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();
            issueAndReceiptService.IssueItem_UpdateQuantity(new IssueReceipt_IssueItemModel { _UserId = userId, DetId = DetId, Quantity = Quantity ?? 0, WhsCode = WhsCode, MsnPrd = MsnPrd, Department = Department, Cost = Cost });
            return Content("OK");
        }

        [HttpPost, ValidateInput(false)]
        public ContentResult UpdateQtyReceipt(long DetId, decimal? Quantity, string WhsCode, string MsnPrd, string Department, decimal? Price)
        {
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();
            issueAndReceiptService.ReceiptItem_UpdateQuantity(new IssueReceipt_ReceiptItemModel { _UserId = userId, DetId = DetId, Quantity = Quantity ?? 0, WhsCode = WhsCode, MsnPrd = MsnPrd, Department = Department, Price = Price });
            return Content("OK");
        }

        [HttpPost, ValidateInput(false)]
        public ContentResult DeleteItemIssue(long Id, long DetId)
        {
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();
            issueAndReceiptService.IssueItem_Delete(userId, DetId);
            return Content("OK");
        }

        [HttpPost, ValidateInput(false)]
        public ContentResult DeleteItemReceipt(long Id, long DetId)
        {
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();
            issueAndReceiptService.ReceiptItem_Delete(userId, DetId);
            return Content("OK");
        }


    }
}