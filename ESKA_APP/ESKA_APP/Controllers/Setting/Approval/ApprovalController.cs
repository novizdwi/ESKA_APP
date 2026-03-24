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

        string VIEW_DETAIL = "Approval";
        string VIEW_FORM_PARTIAL = "Partial/Approval_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Approval_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Approval_Panel_List_Partial";

        ApprovalService approvalService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int Id =0)
        {

            approvalService = new ApprovalService();
            ApprovalModel approvalModel;
            if (Id == 0)
            {

                approvalModel = approvalService.GetNewModel();
                approvalModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalService = new ApprovalService();
                approvalModel = approvalService.GetById(Id);
                if (approvalModel != null)
                {
                    approvalModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, approvalModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            ApprovalModel approvalModel;

            approvalService = new ApprovalService();

            if (Id == 0)
            {
                approvalModel = approvalService.GetNewModel();
                approvalModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalModel = approvalService.GetById(Id);
                if (approvalModel != null)
                {
                    approvalModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }

 

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalModel approvalModel)
        {

            approvalModel._UserId = (int)Session["userId"];

            approvalService = new ApprovalService();
 

            if (ModelState.IsValid)
            {
                approvalService.Add(approvalModel);
                approvalModel = approvalService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            
            approvalModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalModel approvalModel)
        {

            approvalModel._UserId = (int)Session["userId"];

            approvalService = new ApprovalService();
            approvalModel._FormMode = FormModeEnum.Edit;

           
          
            if (ModelState.IsValid)
            {
                approvalService.Update(approvalModel);
                approvalModel = approvalService.GetById(approvalModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            approvalService = new ApprovalService();

            if (Id != 0)
            {
                approvalService = new ApprovalService();
                approvalService.DeleteById(Id);

            }

            ApprovalModel approvalModel = new ApprovalModel();
            return PartialView(VIEW_FORM_PARTIAL, approvalModel);
        } 

    }
}