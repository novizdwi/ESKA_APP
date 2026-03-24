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
using Models.Setting.Query;

namespace Controllers.Setting
{
    public partial class QueryController : BaseController
    {

        string VIEW_DETAIL = "Query";
        string VIEW_FORM_PARTIAL = "Partial/Query_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Query_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Query_Panel_List_Partial";

        QueryService queryService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int Id =0)
        {

            queryService = new QueryService();
            QueryModel queryModel;
            if (Id == 0)
            {

                queryModel = queryService.GetNewModel();
                queryModel._FormMode = FormModeEnum.New;
            }
            else
            {
                queryService = new QueryService();
                queryModel = queryService.GetById(Id);
                if (queryModel != null)
                {
                    queryModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, queryModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            QueryModel queryModel;

            queryService = new QueryService();

            if (Id == 0)
            {
                queryModel = queryService.GetNewModel();
                queryModel._FormMode = FormModeEnum.New;
            }
            else
            {
                queryModel = queryService.GetById(Id);
                if (queryModel != null)
                {
                    queryModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }

 

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  QueryModel queryModel)
        {

            queryModel._UserId = (int)Session["userId"];

            queryService = new QueryService();
 

            if (ModelState.IsValid)
            {
                queryService.Add(queryModel);
                queryModel = queryService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            
            queryModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  QueryModel queryModel)
        {

            queryModel._UserId = (int)Session["userId"];

            queryService = new QueryService();
            queryModel._FormMode = FormModeEnum.Edit;

           
          
            if (ModelState.IsValid)
            {
                queryService.Update(queryModel);
                queryModel = queryService.GetById(queryModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            queryService = new QueryService();

            if (Id != 0)
            {
                queryService = new QueryService();
                queryService.DeleteById(Id);

            }

            QueryModel queryModel = new QueryModel();
            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        } 

    }
}