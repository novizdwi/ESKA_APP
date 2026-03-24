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
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class DeactiveTagController : BaseController
    {

        string VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL = "Partial/Approval/Approval_Panel_Partial";
        string VIEW_APPROVAL_PROGRESS_FORM_PARTIAL = "Partial/Approval/Approval_Form_Partial";
        string VIEW_APPROVAL_PROGRESS_CONTENT = "Partial/Approval/Approval_Form_TabApproval_Partial";

        public ActionResult Approval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];
            deactiveTagService = new DeactiveTagService();
            DeactiveTagApprovalView___ model = new DeactiveTagApprovalView___();
            if (Id != 0)
            {
                model = deactiveTagService.GetViewApproval(Id);
            }
            return PartialView(VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL, model);
        }

        public ActionResult TabApprovalListPartial()
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<DeactiveTag_ApprovalModel> modelList = deactiveTagService.GetDeactiveTag_ApprovalSteps(Id);

            return PartialView(VIEW_APPROVAL_PROGRESS_CONTENT, modelList);
        }

        
        public ActionResult PopupApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new DeactiveTagModel();

            return PartialView(VIEW_APPROVAL_PROGRESS_FORM_PARTIAL, model);
        }
    }
}