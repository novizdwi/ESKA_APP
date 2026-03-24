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
using Models.Setting.AlertGroup;

namespace Controllers.Setting
{
    public partial class AlertGroupController : BaseController
    {

        string VIEW_DETAIL = "AlertGroup";
        string VIEW_FORM_PARTIAL = "Partial/AlertGroup_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/AlertGroup_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/AlertGroup_Panel_List_Partial";

        AlertGroupService alertGroupService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int Id =0)
        {

            alertGroupService = new AlertGroupService();
            AlertGroupModel alertGroupModel;
            if (Id == 0)
            {

                alertGroupModel = alertGroupService.GetNewModel();
                alertGroupModel._FormMode = FormModeEnum.New;
            }
            else
            {
                alertGroupService = new AlertGroupService();
                alertGroupModel = alertGroupService.GetById(Id);
                if (alertGroupModel != null)
                {
                    alertGroupModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, alertGroupModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            AlertGroupModel alertGroupModel;

            alertGroupService = new AlertGroupService();

            if (Id == 0)
            {
                alertGroupModel = alertGroupService.GetNewModel();
                alertGroupModel._FormMode = FormModeEnum.New;
            }
            else
            {
                alertGroupModel = alertGroupService.GetById(Id);
                if (alertGroupModel != null)
                {
                    alertGroupModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }

 

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  AlertGroupModel alertGroupModel)
        {

            alertGroupModel._UserId = (int)Session["userId"];

            
            alertGroupService = new AlertGroupService();
 

            if (ModelState.IsValid)
            { 
                alertGroupService.Add(alertGroupModel);
                alertGroupModel = alertGroupService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            
            alertGroupModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  AlertGroupModel alertGroupModel)
        {

            alertGroupModel._UserId = (int)Session["userId"];
            
            alertGroupService = new AlertGroupService();
            alertGroupModel._FormMode = FormModeEnum.Edit;

           
          
            if (ModelState.IsValid)
            { 
                alertGroupService.Update(alertGroupModel);
                alertGroupModel = alertGroupService.GetById(alertGroupModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            alertGroupService = new AlertGroupService();

            if (Id != 0)
            {
                alertGroupService = new AlertGroupService();
                alertGroupService.DeleteById(Id);

            }

            AlertGroupModel alertGroupModel = new AlertGroupModel();
            return PartialView(VIEW_FORM_PARTIAL, alertGroupModel);
        }


       

    }

 

}