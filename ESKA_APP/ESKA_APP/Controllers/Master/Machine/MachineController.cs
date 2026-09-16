using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Machine;

namespace Controllers.Master
{
    public partial class MachineController : BaseController
    {
        string VIEW_DETAIL = "Machine";
        string VIEW_FORM_PARTIAL = "Partial/Machine_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Machine_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Machine_Panel_List_Partial";


        MachineService machineService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            machineService = new MachineService();
            MachineModel machineModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                machineModel = machineService.GetNewModel(userId);
                machineModel.IsActive = "Y";
                machineModel._FormMode = FormModeEnum.New;
            }
            else
            {
                machineService = new MachineService();
                machineModel = machineService.GetById(userId, Id);
                machineModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, machineModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            MachineModel machineModel;

            machineService = new MachineService();
            if (Id == 0)
            {
                machineModel = machineService.GetNewModel(userId);
                machineModel._FormMode = FormModeEnum.New;
            }
            else
            {
                machineModel = machineService.GetById(userId, Id);
                if (machineModel != null)
                {
                    machineModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    machineModel = machineService.GetNewModel(userId);
                    machineModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, machineModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  MachineModel MachineModel)
        {
            int userId = (int)Session["userId"];

            MachineModel._UserId = (int)Session["userId"];
            machineService = new MachineService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = machineService.Add(MachineModel);
                MachineModel = machineService.GetById(userId, Id);
                MachineModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, MachineModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  MachineModel machineModel)
        {
            int userId = (int)Session["userId"];

            machineModel._UserId = (int)Session["userId"];
            machineService = new MachineService();
            machineModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            machineService.Update(machineModel);
            machineModel = machineService.GetById(userId, machineModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, machineModel);
        }

    }
}