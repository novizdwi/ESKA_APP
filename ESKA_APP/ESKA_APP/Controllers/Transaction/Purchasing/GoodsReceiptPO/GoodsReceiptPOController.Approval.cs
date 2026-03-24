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
using Models.Transaction.Purchasing;


namespace Controllers.Transaction.Purchasing
{
    public partial class GoodsReceiptPOController : BaseController
    {

        string VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL = "Partial/Approval/Approval_Panel_Partial";
        string VIEW_APPROVAL_PROGRESS_FORM_PARTIAL = "Partial/Approval/Approval_Form_Partial";
        string VIEW_APPROVAL_PROGRESS_CONTENT = "Partial/Approval/Approval_Form_TabApproval_Partial";

        public ActionResult Approval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            goodsReceiptPOService = new GoodsReceiptPOService();
            GoodsReceiptPOApprovalView___ model = new GoodsReceiptPOApprovalView___();
            if (Id != 0)
            {
                model = goodsReceiptPOService.GetViewApproval(Id);
            }
            return PartialView(VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL, model);
        }

        public ActionResult TabApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            goodsReceiptPOService = new GoodsReceiptPOService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<GoodsReceiptPO_ApprovalModel> modelList = goodsReceiptPOService.GetGoodsReceiptPO_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_PROGRESS_CONTENT, modelList);
        }


        public ActionResult PopupApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new GoodsReceiptPOModel();

            return PartialView(VIEW_APPROVAL_PROGRESS_FORM_PARTIAL, model);
        }
    }
}