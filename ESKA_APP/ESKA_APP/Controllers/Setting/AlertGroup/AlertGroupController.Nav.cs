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
using Models.Setting.AlertGroup;

namespace Controllers.Setting
{
    public partial class AlertGroupController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            AlertGroupModel alertGroupModel;
            alertGroupService = new AlertGroupService();

            alertGroupModel = alertGroupService.NavFirst();
            if (alertGroupModel != null)
            {
                alertGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (alertGroupModel == null)
            {
                //alertGroupModel = alertGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            AlertGroupModel alertGroupModel;
            alertGroupService = new AlertGroupService();

            alertGroupModel = alertGroupService.NavPrevious(Id);
            if (alertGroupModel != null)
            {
                alertGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (alertGroupModel == null)
            {
                //alertGroupModel = alertGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            AlertGroupModel alertGroupModel;
            alertGroupService = new AlertGroupService();

            alertGroupModel = alertGroupService.NavNext(Id);
            if (alertGroupModel != null)
            {

                alertGroupModel._FormMode = FormModeEnum.Edit;

            }

            if (alertGroupModel == null)
            {
                //alertGroupModel = alertGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            AlertGroupModel alertGroupModel;
            alertGroupService = new AlertGroupService();

            alertGroupModel = alertGroupService.NavLast();
            if (alertGroupModel != null)
            {
                alertGroupModel._FormMode = FormModeEnum.Edit; 
            }

            if (alertGroupModel == null)
            {
                //alertGroupModel = alertGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }



    }
}