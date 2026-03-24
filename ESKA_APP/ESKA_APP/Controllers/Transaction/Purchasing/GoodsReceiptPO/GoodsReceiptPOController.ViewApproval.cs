using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models;
using System.Net;
using Models._Utils;
using Models.Transaction.Purchasing;

namespace Controllers.Transaction.Purchasing
{
    public partial class GoodsReceiptPOController : BaseController
    {

        string VIEW_APPROVAL_PANEL_PARTIAL = "Partial/ViewApproval/ViewApproval_Panel_Partial";
        string VIEW_APPROVAL_FORM_PARTIAL = "Partial/ViewApproval/ViewApproval_Form_Partial";
        string VIEW_APPROVAL_CONTENT = "Partial/ViewApproval/ViewApproval_TabContent_List_Partial";

        public ActionResult ViewApproval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            goodsReceiptPOService = new GoodsReceiptPOService();
            GoodsReceiptPOApprovalView___ model = new GoodsReceiptPOApprovalView___();

            if (Id != 0)
            {
                model = goodsReceiptPOService.GetViewApproval(Id);
            }

            return PartialView(VIEW_APPROVAL_PANEL_PARTIAL, model);
        }

        public ActionResult ViewApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            goodsReceiptPOService = new GoodsReceiptPOService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<GoodsReceiptPO_ApprovalModel> modelList = goodsReceiptPOService.GetGoodsReceiptPO_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_CONTENT, modelList);
        }

        public ActionResult PopupViewApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new GoodsReceiptPOApprovalView___();

            return PartialView(VIEW_APPROVAL_FORM_PARTIAL, model);
        }
    }

}