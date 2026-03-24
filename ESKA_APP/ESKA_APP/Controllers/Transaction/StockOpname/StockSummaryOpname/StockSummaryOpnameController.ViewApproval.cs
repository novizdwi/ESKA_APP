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
using Models.Transaction.StockOpname;

namespace Controllers.Transaction.StockOpname
{
    public partial class StockSummaryOpnameController : BaseController
    {

        string VIEW_APPROVAL_PANEL_PARTIAL = "Partial/ViewApproval/ViewApproval_Panel_Partial";
        string VIEW_APPROVAL_FORM_PARTIAL = "Partial/ViewApproval/ViewApproval_Form_Partial";
        string VIEW_APPROVAL_CONTENT = "Partial/ViewApproval/ViewApproval_TabContent_List_Partial";

        public ActionResult ViewApproval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            stockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameApprovalView___ model = new StockSummaryOpnameApprovalView___();

            if (Id != 0)
            {
                model = stockSummaryOpnameService.GetViewApproval(Id);
            }

            return PartialView(VIEW_APPROVAL_PANEL_PARTIAL, model);
        }

        public ActionResult ViewApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            stockSummaryOpnameService = new StockSummaryOpnameService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<StockSummaryOpname_ApprovalModel> modelList = stockSummaryOpnameService.GetStockSummaryOpname_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_CONTENT, modelList);
        }

        public ActionResult PopupViewApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new StockSummaryOpnameApprovalView___();

            return PartialView(VIEW_APPROVAL_FORM_PARTIAL, model);
        }
    }

}