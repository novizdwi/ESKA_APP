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
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class StockOpnameController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            StockOpnameModel StockOpnameModel;
            stockOpnameService = new StockOpnameService();

            StockOpnameModel = stockOpnameService.NavFirst(userId);
            if (StockOpnameModel != null)
            {
                StockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (StockOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            StockOpnameModel StockOpnameModel;
            stockOpnameService = new StockOpnameService();

            StockOpnameModel = stockOpnameService.NavPrevious(userId, Id);
            if (StockOpnameModel != null)
            {
                StockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (StockOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            StockOpnameModel StockOpnameModel;
            stockOpnameService = new StockOpnameService();

            StockOpnameModel = stockOpnameService.NavNext(userId, Id);
            if (StockOpnameModel != null)
            {

                StockOpnameModel._FormMode = FormModeEnum.Edit;

            }

            if (StockOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            StockOpnameModel StockOpnameModel;
            stockOpnameService = new StockOpnameService();

            StockOpnameModel = stockOpnameService.NavLast(userId);
            if (StockOpnameModel != null)
            {
                StockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (StockOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }



    }
}