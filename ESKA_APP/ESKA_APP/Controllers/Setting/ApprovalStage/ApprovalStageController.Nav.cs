using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;


using System.Net;

using Models;
using Models.Setting.ApprovalStage;

namespace Controllers.Setting
{
    public partial class ApprovalStageController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ApprovalStageModel approvalStageModel;
            approvalStageService = new ApprovalStageService();

            approvalStageModel = approvalStageService.NavFirst(userId);
            if (approvalStageModel != null)
            {
                approvalStageModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalStageModel == null)
            {
                //approvalStageModel = approvalStageService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];

            ApprovalStageModel approvalStageModel;
            approvalStageService = new ApprovalStageService();

            approvalStageModel = approvalStageService.NavPrevious(userId, Id);
            if (approvalStageModel != null)
            {
                approvalStageModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalStageModel == null)
            {
                //approvalStageModel = approvalStageService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];

            ApprovalStageModel approvalStageModel;
            approvalStageService = new ApprovalStageService();

            approvalStageModel = approvalStageService.NavNext(userId, Id);
            if (approvalStageModel != null)
            {

                approvalStageModel._FormMode = FormModeEnum.Edit;

            }

            if (approvalStageModel == null)
            {
                //approvalStageModel = approvalStageService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ApprovalStageModel approvalStageModel;
            approvalStageService = new ApprovalStageService();

            approvalStageModel = approvalStageService.NavLast(userId);
            if (approvalStageModel != null)
            {
                approvalStageModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalStageModel == null)
            {
                //approvalStageModel = approvalStageService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }



    }
}