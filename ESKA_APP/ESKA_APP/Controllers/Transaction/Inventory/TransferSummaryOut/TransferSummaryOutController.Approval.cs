using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;

using System.Net;

using Models;
using Models.Transaction.Inventory;


namespace Controllers.Transaction.Inventory
{
    public partial class TransferSummaryOutController : BaseController
    {

        string VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL = "Partial/Approval/Approval_Panel_Partial";
        string VIEW_APPROVAL_PROGRESS_FORM_PARTIAL = "Partial/Approval/Approval_Form_Partial";
        string VIEW_APPROVAL_PROGRESS_CONTENT = "Partial/Approval/Approval_Form_TabApproval_Partial";

        public ActionResult Approval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            transferSummaryOutService = new TransferSummaryOutService();
            TransferSummaryOutApprovalView___ model = new TransferSummaryOutApprovalView___();
            if (Id != 0)
            {
                model = transferSummaryOutService.GetViewApproval(Id);
            }
            return PartialView(VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL, model);
        }

        public ActionResult TabApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            transferSummaryOutService = new TransferSummaryOutService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<TransferSummaryOut_ApprovalModel> modelList = transferSummaryOutService.GetTransferSummaryOut_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_PROGRESS_CONTENT, modelList);
        }


        public ActionResult PopupApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new TransferSummaryOutModel();

            return PartialView(VIEW_APPROVAL_PROGRESS_FORM_PARTIAL, model);
        }
    }
}