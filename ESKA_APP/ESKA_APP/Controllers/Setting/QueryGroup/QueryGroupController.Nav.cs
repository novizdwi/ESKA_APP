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
using Models.Setting.QueryGroup;

namespace Controllers.Setting
{
    public partial class QueryGroupController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            QueryGroupModel queryGroupModel;
            queryGroupService = new QueryGroupService();

            queryGroupModel = queryGroupService.NavFirst();
            if (queryGroupModel != null)
            {
                queryGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (queryGroupModel == null)
            {
                //queryGroupModel = queryGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            QueryGroupModel queryGroupModel;
            queryGroupService = new QueryGroupService();

            queryGroupModel = queryGroupService.NavPrevious(Id);
            if (queryGroupModel != null)
            {
                queryGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (queryGroupModel == null)
            {
                //queryGroupModel = queryGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            QueryGroupModel queryGroupModel;
            queryGroupService = new QueryGroupService();

            queryGroupModel = queryGroupService.NavNext(Id);
            if (queryGroupModel != null)
            {

                queryGroupModel._FormMode = FormModeEnum.Edit;

            }

            if (queryGroupModel == null)
            {
                //queryGroupModel = queryGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            QueryGroupModel queryGroupModel;
            queryGroupService = new QueryGroupService();

            queryGroupModel = queryGroupService.NavLast();
            if (queryGroupModel != null)
            {
                queryGroupModel._FormMode = FormModeEnum.Edit; 
            }

            if (queryGroupModel == null)
            {
                //queryGroupModel = queryGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, queryGroupModel);
        }



    }
}