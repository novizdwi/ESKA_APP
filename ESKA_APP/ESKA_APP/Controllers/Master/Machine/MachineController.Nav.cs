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
using Models.Master.Machine;

namespace Controllers.Master
{
    public partial class MachineController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            MachineModel machineModel;
            machineService = new MachineService();

            machineModel = machineService.NavFirst(userId);
            if (machineModel != null)
            {
                machineModel._FormMode = FormModeEnum.Edit;
            }

            if (machineModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, machineModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            MachineModel machineModel;
            machineService = new MachineService();

            machineModel = machineService.NavPrevious(userId, Id);
            if (machineModel != null)
            {
                machineModel._FormMode = FormModeEnum.Edit;
            }

            if (machineModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, machineModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            MachineModel machineModel;
            machineService = new MachineService();

            machineModel = machineService.NavNext(userId, Id);
            if (machineModel != null)
            {

                machineModel._FormMode = FormModeEnum.Edit;

            }

            if (machineModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, machineModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            MachineModel machineModel;
            machineService = new MachineService();

            machineModel = machineService.NavLast(userId);
            if (machineModel != null)
            {
                machineModel._FormMode = FormModeEnum.Edit;
            }

            if (machineModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, machineModel);
        }



    }
}