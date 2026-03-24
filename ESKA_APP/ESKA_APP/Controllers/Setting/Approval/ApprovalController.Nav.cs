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
using Models.Setting.Approval;

namespace Controllers.Setting
{
    public partial class ApprovalController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            ApprovalModel approvalModel;
            approvalService = new ApprovalService();

            approvalModel = approvalService.NavFirst();
            if (approvalModel != null)
            {
                approvalModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalModel == null)
            {
                //approvalModel = approvalService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            ApprovalModel approvalModel;
            approvalService = new ApprovalService();

            approvalModel = approvalService.NavPrevious(Id);
            if (approvalModel != null)
            {
                approvalModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalModel == null)
            {
                //approvalModel = approvalService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            ApprovalModel approvalModel;
            approvalService = new ApprovalService();

            approvalModel = approvalService.NavNext(Id);
            if (approvalModel != null)
            {

                approvalModel._FormMode = FormModeEnum.Edit;

            }

            if (approvalModel == null)
            {
                //approvalModel = approvalService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            ApprovalModel approvalModel;
            approvalService = new ApprovalService();

            approvalModel = approvalService.NavLast();
            if (approvalModel != null)
            {
                approvalModel._FormMode = FormModeEnum.Edit; 
            }

            if (approvalModel == null)
            {
                //approvalModel = approvalService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }



    }
}