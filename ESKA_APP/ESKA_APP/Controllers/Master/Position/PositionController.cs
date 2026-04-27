using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Position;

namespace Controllers.Master
{
    public partial class PositionController : BaseController
    {
        string VIEW_DETAIL = "Position";
        string VIEW_FORM_PARTIAL = "Partial/Position_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Position_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Position_Panel_List_Partial";


        PositionService positionService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            positionService = new PositionService();
            PositionModel positionModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                positionModel = positionService.GetNewModel(userId);
                positionModel.IsActive = "Y";
                positionModel._FormMode = FormModeEnum.New;
            }
            else
            {
                positionService = new PositionService();
                positionModel = positionService.GetById(userId, Id);
                positionModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, positionModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            PositionModel positionModel;

            positionService = new PositionService();
            if (Id == 0)
            {
                positionModel = positionService.GetNewModel(userId);
                positionModel._FormMode = FormModeEnum.New;
            }
            else
            {
                positionModel = positionService.GetById(userId, Id);
                if (positionModel != null)
                {
                    positionModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    positionModel = positionService.GetNewModel(userId);
                    positionModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, positionModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  PositionModel PositionModel)
        {
            int userId = (int)Session["userId"];

            PositionModel._UserId = (int)Session["userId"];
            positionService = new PositionService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = positionService.Add(PositionModel);
                PositionModel = positionService.GetById(userId, Id);
                PositionModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, PositionModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  PositionModel positionModel)
        {
            int userId = (int)Session["userId"];

            positionModel._UserId = (int)Session["userId"];
            positionService = new PositionService();
            positionModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            positionService.Update(positionModel);
            positionModel = positionService.GetById(userId, positionModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, positionModel);
        }

    }
}