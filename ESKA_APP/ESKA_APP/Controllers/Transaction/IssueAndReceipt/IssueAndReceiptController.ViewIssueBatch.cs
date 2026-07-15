using Models;
using Models.Transaction;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction
{
    public partial class IssueAndReceiptController : BaseController
    {

        string VIEW_ISSUE_ITEMTAG_PANEL_PARTIAL = "Partial/BatchIssue/BatchIssue_Panel_Partial";
        string VIEW_ISSUE_ITEMTAG_FORM_PARTIAL = "Partial/BatchIssue/BatchIssue_Form_Partial";
        string VIEW_ISSUE_TAB_BATCH = "Partial/BatchIssue/BatchIssue_TabBatchList_List_Partial";

        public ActionResult ViewIssueBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;

            issueAndReceiptService = new IssueAndReceiptService();
            var model = new IssueAndReceiptBatchIssueView___();
            if (id != 0 && detId != 0)
            {
                model = issueAndReceiptService.GetIssueBatch(id, detId);
            }
            return PartialView(VIEW_ISSUE_ITEMTAG_PANEL_PARTIAL, model);
        }

        public ActionResult TabIssueBatchListPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            issueAndReceiptService = new IssueAndReceiptService();

            var modelList = issueAndReceiptService.IssueAndReceipt__IssueItemBatchList(DetId);

            return PartialView(VIEW_ISSUE_TAB_BATCH, modelList);
        }

        public ActionResult PopupIssueItemTagLoadOnDemandPartial()
        {

            int userId = (int)Session["userId"];
            var model = new IssueAndReceiptModel();

            return PartialView(VIEW_ISSUE_ITEMTAG_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabIssueBatchListAddNewRow([Bind] IssueAndReceiptBatchIssueModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;

            issueAndReceiptService = new IssueAndReceiptService();

            if (ModelState.IsValid)
            {
                //model.Id = Id;
                model.DetDetId = DetId;
                model._UserId = (int)Session["userId"];
                issueAndReceiptService.IssueAndReceipt__IssueAddNewItemBatch(model);
            }

            return TabIssueBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabIssueBatchListUpdateRow([Bind] IssueAndReceiptBatchIssueModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;

            issueAndReceiptService = new IssueAndReceiptService();

            if (ModelState.IsValid)
            {
                model._UserId = (int)Session["userId"];
                issueAndReceiptService.IssueAndReceipt_IssueUpdateItemBatch(model);
            }

            return TabIssueBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabIssueBatchListDeleteRow(long DetDetId, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            int userId = (int)Session["userId"];
            issueAndReceiptService = new IssueAndReceiptService();

            if (ModelState.IsValid)
            {
                issueAndReceiptService.IssueAndReceipt_IssueDeleteItemBatch(userId, Id, DetId, DetDetId);
            }

            return TabIssueBatchListPartial(id, detId);
        }

    }

}