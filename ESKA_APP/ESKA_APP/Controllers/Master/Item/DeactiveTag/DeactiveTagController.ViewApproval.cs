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
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class DeactiveTagController : BaseController
    {

        string VIEW_APPROVAL_PANEL_PARTIAL = "Partial/ViewApproval/ViewApproval_Panel_Partial";
        string VIEW_APPROVAL_FORM_PARTIAL = "Partial/ViewApproval/ViewApproval_Form_Partial";
        string VIEW_APPROVAL_CONTENT = "Partial/ViewApproval/ViewApproval_TabContent_List_Partial";

        public ActionResult ViewApproval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            deactiveTagService = new DeactiveTagService();
            DeactiveTagApprovalView___ model = new DeactiveTagApprovalView___();

            if (Id != 0)
            {
                model = deactiveTagService.GetViewApproval(Id);
            }

            return PartialView(VIEW_APPROVAL_PANEL_PARTIAL, model);
        }

        public ActionResult ViewApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<DeactiveTag_ApprovalModel> modelList = deactiveTagService.GetDeactiveTag_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_CONTENT, modelList);
        }

        public ActionResult PopupViewApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new DeactiveTagApprovalView___();

            return PartialView(VIEW_APPROVAL_FORM_PARTIAL, model);
        }
    }

}