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

        string VIEW_TAB_CONTENT = "Partial/IssueAndReceipt_Form_TabDetail_List_Partial";

        public ActionResult TabIssueDetailListPartial()
        {
            int userId = (int)Session["userId"];

            issueAndReceiptService = new IssueAndReceiptService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<IssueReceipt_IssueItemModel> modelList = issueAndReceiptService.IssueAndReceipt_IssueItemDetails(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

        public ActionResult TabReceiptDetailListPartial()
        {
            int userId = (int)Session["userId"];

            issueAndReceiptService = new IssueAndReceiptService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<IssueReceipt_ReceiptItemModel> modelList = issueAndReceiptService.IssueAndReceipt_ReceiptItemDetails(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }


    }
}