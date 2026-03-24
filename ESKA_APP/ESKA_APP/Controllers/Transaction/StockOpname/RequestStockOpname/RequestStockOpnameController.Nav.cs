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
using Models.Transaction.StockOpname;

namespace Controllers.Transaction.StockOpname
{
    public partial class RequestStockOpnameController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            RequestStockOpnameModel requestStockOpnameModel;
            requestStockOpnameService = new RequestStockOpnameService();

            requestStockOpnameModel = requestStockOpnameService.NavFirst(userId);
            if (requestStockOpnameModel != null)
            {
                requestStockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (requestStockOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            RequestStockOpnameModel requestStockOpnameModel;
            requestStockOpnameService = new RequestStockOpnameService();

            requestStockOpnameModel = requestStockOpnameService.NavPrevious(userId, Id);
            if (requestStockOpnameModel != null)
            {
                requestStockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (requestStockOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            RequestStockOpnameModel requestStockOpnameModel;
            requestStockOpnameService = new RequestStockOpnameService();

            requestStockOpnameModel = requestStockOpnameService.NavNext(userId, Id);
            if (requestStockOpnameModel != null)
            {

                requestStockOpnameModel._FormMode = FormModeEnum.Edit;

            }

            if (requestStockOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            RequestStockOpnameModel requestStockOpnameModel;
            requestStockOpnameService = new RequestStockOpnameService();

            requestStockOpnameModel = requestStockOpnameService.NavLast(userId);
            if (requestStockOpnameModel != null)
            {
                requestStockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (requestStockOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }



    }
}