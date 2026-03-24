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

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            QueryModel queryModel;
            queryService = new QueryService();

            queryModel = queryService.NavFirst();
            if (queryModel != null)
            {
                queryModel._FormMode = FormModeEnum.Edit;
            }

            if (queryModel == null)
            {
                //queryModel = queryService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            QueryModel queryModel;
            queryService = new QueryService();

            queryModel = queryService.NavPrevious(Id);
            if (queryModel != null)
            {
                queryModel._FormMode = FormModeEnum.Edit;
            }

            if (queryModel == null)
            {
                //queryModel = queryService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            QueryModel queryModel;
            queryService = new QueryService();

            queryModel = queryService.NavNext(Id);
            if (queryModel != null)
            {

                queryModel._FormMode = FormModeEnum.Edit;

            }

            if (queryModel == null)
            {
                //queryModel = queryService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            QueryModel queryModel;
            queryService = new QueryService();

            queryModel = queryService.NavLast();
            if (queryModel != null)
            {
                queryModel._FormMode = FormModeEnum.Edit; 
            }

            if (queryModel == null)
            {
                //queryModel = queryService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryModel);
        }



    }
}