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
using Models.Setting.Alert;

namespace Controllers.Setting
{
    public partial class AlertController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            AlertModel alertModel;
            alertService = new AlertService();

            alertModel = alertService.NavFirst();
            if (alertModel != null)
            {
                alertModel._FormMode = FormModeEnum.Edit;
            }

            if (alertModel == null)
            {
                //alertModel = alertService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            AlertModel alertModel;
            alertService = new AlertService();

            alertModel = alertService.NavPrevious(Id);
            if (alertModel != null)
            {
                alertModel._FormMode = FormModeEnum.Edit;
            }

            if (alertModel == null)
            {
                //alertModel = alertService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            AlertModel alertModel;
            alertService = new AlertService();

            alertModel = alertService.NavNext(Id);
            if (alertModel != null)
            {

                alertModel._FormMode = FormModeEnum.Edit;

            }

            if (alertModel == null)
            {
                //alertModel = alertService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            AlertModel alertModel;
            alertService = new AlertService();

            alertModel = alertService.NavLast();
            if (alertModel != null)
            {
                alertModel._FormMode = FormModeEnum.Edit; 
            }

            if (alertModel == null)
            {
                //alertModel = alertService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }



    }
}