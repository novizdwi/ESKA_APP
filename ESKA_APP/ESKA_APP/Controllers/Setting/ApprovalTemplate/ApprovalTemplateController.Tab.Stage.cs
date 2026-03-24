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

using Models.Setting.ApprovalTemplate;
using Models._Utils;

namespace Controllers.Setting
{
    public partial class ApprovalTemplateController : BaseController
    {

        string VIEW_TAB_CONTENT = "Partial/ApprovalTemplate_Form_TabStage_List_Partial";
        
        public ActionResult TabStagesListPartial()
        {
            int userId = (int)Session["userId"];
            
            approvalTemplateService = new ApprovalTemplateService();

            var Id = Convert.ToInt32(Request["cbId"]);

            var modelList = approvalTemplateService.GetApprovalTemplate_Stages(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

      



    }
}