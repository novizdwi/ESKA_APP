using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction;
using System.Linq;
using Newtonsoft.Json;

namespace Controllers.Transaction
{
    public partial class ReProcessController : BaseController
    {
        string VIEW_DETAIL = "ReProcess";
        string VIEW_FORM_PARTIAL = "Partial/ReProcess_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ReProcess_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ReProcess_Panel_List_Partial";


        ReProcessService reProcessService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            reProcessService = new ReProcessService();
            ReProcessModel ReProcessModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                ReProcessModel = reProcessService.GetNewModel(userId);
                ReProcessModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reProcessService = new ReProcessService();
                ReProcessModel = reProcessService.GetById(userId, Id);
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, ReProcessModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            ReProcessModel ReProcessModel;

            reProcessService = new ReProcessService();
            if (Id == 0)
            {
                ReProcessModel = reProcessService.GetNewModel(userId);
                ReProcessModel._FormMode = FormModeEnum.New;
            }
            else
            {
                ReProcessModel = reProcessService.GetById(userId, Id);
                if (ReProcessModel != null)
                {
                    ReProcessModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    ReProcessModel = reProcessService.GetNewModel(userId);
                    ReProcessModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))] ReProcessModel ReProcessModel)
        {
            int userId = (int)Session["userId"];

            ReProcessModel._UserId = userId;
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                long Id = reProcessService.Add(ReProcessModel);
                ReProcessModel = reProcessService.GetById(userId, Id);
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))] ReProcessModel ReProcessModel)
        {
            int userId = (int)Session["userId"];

            ReProcessModel._UserId = userId;
            reProcessService = new ReProcessService();
            ReProcessModel._FormMode = FormModeEnum.Edit;

            reProcessService.Update(ReProcessModel);
            ReProcessModel = reProcessService.GetById(userId, ReProcessModel.Id);

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))] ReProcessModel ReProcessModel)
        {
            int userId = (int)Session["userId"];

            ReProcessModel._UserId = userId;
            reProcessService = new ReProcessService();
            ReProcessModel._FormMode = FormModeEnum.Edit;

            reProcessService.Post(userId, ReProcessModel);
            ReProcessModel = reProcessService.GetById(userId, ReProcessModel.Id);

            if (ReProcessModel != null)
            {
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                ReProcessModel = reProcessService.GetNewModel(userId);
                ReProcessModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            ReProcessModel ReProcessModel;

            reProcessService = new ReProcessService();
            reProcessService.Cancel(userId, Id, CancelReason);

            ReProcessModel = reProcessService.GetById(userId, Id);
            if (ReProcessModel != null)
            {
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                ReProcessModel = reProcessService.GetNewModel(userId);
                ReProcessModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }

    }
}