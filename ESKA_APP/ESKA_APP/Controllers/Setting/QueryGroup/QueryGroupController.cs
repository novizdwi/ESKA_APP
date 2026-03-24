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
using Models.Setting.QueryGroup;

namespace Controllers.Setting
{
    public partial class QueryGroupController : BaseController
    {

        string VIEW_DETAIL = "QueryGroup";
        string VIEW_FORM_PARTIAL = "Partial/QueryGroup_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/QueryGroup_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/QueryGroup_Panel_List_Partial";

        QueryGroupService queryGroupService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int Id =0)
        {

            queryGroupService = new QueryGroupService();
            QueryGroupModel queryGroupModel;
            if (Id == 0)
            {

                queryGroupModel = queryGroupService.GetNewModel();
                queryGroupModel._FormMode = FormModeEnum.New;
            }
            else
            {
                queryGroupService = new QueryGroupService();
                queryGroupModel = queryGroupService.GetById(Id);
                if (queryGroupModel != null)
                {
                    queryGroupModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, queryGroupModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            QueryGroupModel queryGroupModel;

            queryGroupService = new QueryGroupService();

            if (Id == 0)
            {
                queryGroupModel = queryGroupService.GetNewModel();
                queryGroupModel._FormMode = FormModeEnum.New;
            }
            else
            {
                queryGroupModel = queryGroupService.GetById(Id);
                if (queryGroupModel != null)
                {
                    queryGroupModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }

 

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  QueryGroupModel queryGroupModel)
        {

            queryGroupModel._UserId = (int)Session["userId"];

            
            queryGroupService = new QueryGroupService();
 

            if (ModelState.IsValid)
            { 
                queryGroupService.Add(queryGroupModel);
                queryGroupModel = queryGroupService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            
            queryGroupModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  QueryGroupModel queryGroupModel)
        {

            queryGroupModel._UserId = (int)Session["userId"];
            
            queryGroupService = new QueryGroupService();
            queryGroupModel._FormMode = FormModeEnum.Edit;

           
          
            if (ModelState.IsValid)
            { 
                queryGroupService.Update(queryGroupModel);
                queryGroupModel = queryGroupService.GetById(queryGroupModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            queryGroupService = new QueryGroupService();

            if (Id != 0)
            {
                queryGroupService = new QueryGroupService();
                queryGroupService.DeleteById(Id);

            }

            QueryGroupModel queryGroupModel = new QueryGroupModel();
            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }


       

    }

 

}