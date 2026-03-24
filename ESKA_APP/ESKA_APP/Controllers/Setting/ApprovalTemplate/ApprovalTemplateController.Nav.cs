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
using Models.Setting.ApprovalTemplate;

namespace Controllers.Setting
{
    public partial class ApprovalTemplateController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            ApprovalTemplateModel approvalTemplateModel;
            approvalTemplateService = new ApprovalTemplateService();

            approvalTemplateModel = approvalTemplateService.NavFirst();
            if (approvalTemplateModel != null)
            {
                approvalTemplateModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalTemplateModel == null)
            {
                //approvalTemplateModel = approvalTemplateService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            ApprovalTemplateModel approvalTemplateModel;
            approvalTemplateService = new ApprovalTemplateService();

            approvalTemplateModel = approvalTemplateService.NavPrevious(Id);
            if (approvalTemplateModel != null)
            {
                approvalTemplateModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalTemplateModel == null)
            {
                //approvalTemplateModel = approvalTemplateService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            ApprovalTemplateModel approvalTemplateModel;
            approvalTemplateService = new ApprovalTemplateService();

            approvalTemplateModel = approvalTemplateService.NavNext(Id);
            if (approvalTemplateModel != null)
            {

                approvalTemplateModel._FormMode = FormModeEnum.Edit;

            }

            if (approvalTemplateModel == null)
            {
                //approvalTemplateModel = approvalTemplateService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            ApprovalTemplateModel approvalTemplateModel;
            approvalTemplateService = new ApprovalTemplateService();

            approvalTemplateModel = approvalTemplateService.NavLast();
            if (approvalTemplateModel != null)
            {
                approvalTemplateModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalTemplateModel == null)
            {
                //approvalTemplateModel = approvalTemplateService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }



    }
}