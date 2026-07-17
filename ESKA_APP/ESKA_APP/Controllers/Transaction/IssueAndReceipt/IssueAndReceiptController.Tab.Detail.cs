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


    }
}