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
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class StockOpnameController : BaseController
    {

        string VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL = "Partial/Approval/Approval_Panel_Partial";
        string VIEW_APPROVAL_PROGRESS_FORM_PARTIAL = "Partial/Approval/Approval_Form_Partial";
        string VIEW_APPROVAL_PROGRESS_CONTENT = "Partial/Approval/Approval_Form_TabApproval_Partial";

        public ActionResult Approval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            stockOpnameService = new StockOpnameService();
            StockOpnameApprovalView___ model = new StockOpnameApprovalView___();
            if (Id != 0)
            {
                model = stockOpnameService.GetViewApproval(Id);
            }
            return PartialView(VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL, model);
        }

        public ActionResult TabApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            stockOpnameService = new StockOpnameService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<StockOpname_ApprovalModel> modelList = stockOpnameService.GetStockOpname_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_PROGRESS_CONTENT, modelList);
        }


        public ActionResult PopupApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new StockOpnameModel();

            return PartialView(VIEW_APPROVAL_PROGRESS_FORM_PARTIAL, model);
        }
    }
}