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
    public partial class StockOpnameScanController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            StockOpnameScanModel stockOpnameModel;
            stockOpnameScanService = new StockOpnameScanService();

            stockOpnameModel = stockOpnameScanService.NavFirst(userId);
            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (stockOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            StockOpnameScanModel stockOpnameModel;
            stockOpnameScanService = new StockOpnameScanService();

            stockOpnameModel = stockOpnameScanService.NavPrevious(userId, Id);
            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            if (stockOpnameModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            StockOpnameScanModel stockOpnameModel;
            stockOpnameScanService = new StockOpnameScanService();

            stockOpnameModel = stockOpnameScanService.NavNext(userId, Id);
            if (stockOpnameModel != null)
            {

                stockOpnameModel._FormMode = FormModeEnum.Edit;

            }

            if (stockOpnameModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            StockOpnameScanModel stockOpnameScanModel;
            stockOpnameScanService = new StockOpnameScanService();

            stockOpnameScanModel = stockOpnameScanService.NavLast(userId);
            if (stockOpnameScanModel != null)
            {
                stockOpnameScanModel._FormMode = FormModeEnum.Edit;
            }

            if (stockOpnameScanModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameScanModel);
        }



    }
}