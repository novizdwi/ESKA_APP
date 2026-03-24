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
using Models.Setting.ApprovalTemplate;
using Models.Setting.ApprovalStage;

namespace Controllers.Setting
{
    public partial class ApprovalTemplateController : BaseController
    {

        string VIEW_DETAIL = "ApprovalTemplate";
        string VIEW_FORM_PARTIAL = "Partial/ApprovalTemplate_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ApprovalTemplate_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ApprovalTemplate_Panel_List_Partial";

        ApprovalTemplateService approvalTemplateService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(int Id = 0)
        {
            ViewBag.Id = Id;
            approvalTemplateService = new ApprovalTemplateService();
            ApprovalTemplateModel approvalTemplateModel;
            if (Id == 0)
            {

                approvalTemplateModel = approvalTemplateService.GetNewModel();
                approvalTemplateModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalTemplateService = new ApprovalTemplateService();
                approvalTemplateModel = approvalTemplateService.GetById(Id);
                if (approvalTemplateModel != null)
                {
                    ViewBag.ObjectCode = approvalTemplateModel.ObjectCode;
                    approvalTemplateModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, approvalTemplateModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            ApprovalTemplateModel approvalTemplateModel;

            approvalTemplateService = new ApprovalTemplateService();

            if (Id == 0)
            {
                approvalTemplateModel = approvalTemplateService.GetNewModel();
                approvalTemplateModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalTemplateModel = approvalTemplateService.GetById(Id);
                if (approvalTemplateModel != null)
                {
                    approvalTemplateModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalTemplateModel approvalTemplateModel)
        {

            approvalTemplateModel._UserId = (int)Session["userId"];


            approvalTemplateService = new ApprovalTemplateService();

            int Id = 0;
            approvalTemplateModel._FormMode = Models.FormModeEnum.New;
            if (ModelState.IsValid)
            {
                Id = approvalTemplateService.Add(approvalTemplateModel);
                if (Id != 0)
                {
                    approvalTemplateModel = approvalTemplateService.GetById(Id);
                    approvalTemplateModel._FormMode = FormModeEnum.Edit;

                }
                else
                {
                    approvalTemplateModel = approvalTemplateService.GetNewModel();
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }



            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalTemplateModel approvalTemplateModel)
        {

            approvalTemplateModel._UserId = (int)Session["userId"];

            approvalTemplateService = new ApprovalTemplateService();
            approvalTemplateModel._FormMode = FormModeEnum.Edit;



            if (ModelState.IsValid)
            {
                approvalTemplateService.Update(approvalTemplateModel);
                approvalTemplateModel = approvalTemplateService.GetById(approvalTemplateModel.Id);
                ViewBag.ObjectCode = approvalTemplateModel.ObjectCode;
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            approvalTemplateService = new ApprovalTemplateService();

            if (Id != 0)
            {
                approvalTemplateService = new ApprovalTemplateService();
                approvalTemplateService.DeleteById((int)Session["userId"], Id);

            }

            ApprovalTemplateModel approvalTemplateModel = new ApprovalTemplateModel();
            approvalTemplateModel = approvalTemplateService.GetNewModel();
            return PartialView(VIEW_FORM_PARTIAL, approvalTemplateModel);
        }




    }



}