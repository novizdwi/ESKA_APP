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
    public partial class StockSummaryOpnameController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel StockSummaryOpnameModel;
            stockSummaryOpnameService = new StockSummaryOpnameService();

            StockSummaryOpnameModel = stockSummaryOpnameService.NavFirst(userId);
            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (StockSummaryOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            StockSummaryOpnameModel StockSummaryOpnameModel;
            stockSummaryOpnameService = new StockSummaryOpnameService();

            StockSummaryOpnameModel = stockSummaryOpnameService.NavPrevious(userId, Id);
            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (StockSummaryOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            StockSummaryOpnameModel StockSummaryOpnameModel;
            stockSummaryOpnameService = new StockSummaryOpnameService();

            StockSummaryOpnameModel = stockSummaryOpnameService.NavNext(userId, Id);
            if (StockSummaryOpnameModel != null)
            {

                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;

            }

            if (StockSummaryOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel StockSummaryOpnameModel;
            stockSummaryOpnameService = new StockSummaryOpnameService();

            StockSummaryOpnameModel = stockSummaryOpnameService.NavLast(userId);
            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (StockSummaryOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }



    }
}