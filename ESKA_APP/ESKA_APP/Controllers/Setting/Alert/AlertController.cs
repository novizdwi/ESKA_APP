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

        string VIEW_DETAIL = "Alert";
        string VIEW_FORM_PARTIAL = "Partial/Alert_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Alert_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Alert_Panel_List_Partial";

        AlertService alertService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int Id =0)
        {

            alertService = new AlertService();
            AlertModel alertModel;
            if (Id == 0)
            {

                alertModel = alertService.GetNewModel();
                alertModel._FormMode = FormModeEnum.New;
            }
            else
            {
                alertService = new AlertService();
                alertModel = alertService.GetById(Id);
                if (alertModel != null)
                {
                    alertModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, alertModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            AlertModel alertModel;

            alertService = new AlertService();

            if (Id == 0)
            {
                alertModel = alertService.GetNewModel();
                alertModel._FormMode = FormModeEnum.New;
            }
            else
            {
                alertModel = alertService.GetById(Id);
                if (alertModel != null)
                {
                    alertModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }

 

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  AlertModel alertModel)
        {

            alertModel._UserId = (int)Session["userId"];

            alertService = new AlertService();
 

            if (ModelState.IsValid)
            {
                alertService.Add(alertModel);
                alertModel = alertService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            
            alertModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  AlertModel alertModel)
        {

            alertModel._UserId = (int)Session["userId"];

            alertService = new AlertService();
            alertModel._FormMode = FormModeEnum.Edit;

           
          
            if (ModelState.IsValid)
            {
                alertService.Update(alertModel);
                alertModel = alertService.GetById(alertModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            alertService = new AlertService();

            if (Id != 0)
            {
                alertService = new AlertService();
                alertService.DeleteById(Id);

            }

            AlertModel alertModel = new AlertModel();
            return PartialView(VIEW_FORM_PARTIAL, alertModel);
        } 

    }
}