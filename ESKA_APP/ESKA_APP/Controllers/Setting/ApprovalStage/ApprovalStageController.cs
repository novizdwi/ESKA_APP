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
using Models.Setting.ApprovalStage;
using Models;

namespace Controllers.Setting
{
    public partial class ApprovalStageController : BaseController
    {

        string VIEW_DETAIL = "ApprovalStage";
        string VIEW_FORM_PARTIAL = "Partial/ApprovalStage_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ApprovalStage_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ApprovalStage_Panel_List_Partial";

        ApprovalStageService approvalStageService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(int Id = 0)
        {
            int UserId = (int)Session["userId"];
            approvalStageService = new ApprovalStageService();
            ApprovalStageModel approvalStageModel;
            if (Id == 0)
            {

                approvalStageModel = approvalStageService.GetNewModel();
                approvalStageModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalStageService = new ApprovalStageService();
                approvalStageModel = approvalStageService.GetById(UserId, Id);
                if (approvalStageModel != null)
                {
                    approvalStageModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, approvalStageModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];

            ApprovalStageModel approvalStageModel;

            approvalStageService = new ApprovalStageService();

            if (Id == 0)
            {
                approvalStageModel = approvalStageService.GetNewModel();
                approvalStageModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalStageModel = approvalStageService.GetById(userId, Id);
                if (approvalStageModel != null)
                {
                    approvalStageModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalStageModel approvalStageModel)
        {

            approvalStageModel._UserId = (int)Session["userId"];


            approvalStageService = new ApprovalStageService();

            approvalStageModel._FormMode = Models.FormModeEnum.New;
            int Id = 0;
            if (ModelState.IsValid)
            {
                Id = approvalStageService.Add(approvalStageModel);
                if (Id != 0)
                {
                    approvalStageModel = approvalStageService.GetById(approvalStageModel._UserId, Id);
                    approvalStageModel._FormMode = FormModeEnum.Edit;
                }
                else 
                {
                    approvalStageModel = approvalStageService.GetNewModel();
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }



            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalStageModel approvalStageModel)
        {

            approvalStageModel._UserId = (int)Session["userId"];

            approvalStageService = new ApprovalStageService();
            approvalStageModel._FormMode = FormModeEnum.Edit;



            if (ModelState.IsValid)
            {
                approvalStageService.Update(approvalStageModel);
                approvalStageModel = approvalStageService.GetById(approvalStageModel._UserId, approvalStageModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            approvalStageService = new ApprovalStageService();

            if (Id != 0)
            {
                approvalStageService = new ApprovalStageService();
                approvalStageService.DeleteById((int)Session["userId"], Id);

            }

            ApprovalStageModel approvalStageModel = new ApprovalStageModel();
            return PartialView(VIEW_FORM_PARTIAL, approvalStageModel);
        }




    }



}