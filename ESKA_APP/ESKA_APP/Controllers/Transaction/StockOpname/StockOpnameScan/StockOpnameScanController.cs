using Models;
using Models.Transaction.StockOpname;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.StockOpname
{
    public partial class StockOpnameScanController : BaseController
    {
        string VIEW_DETAIL = "StockOpnameScan";
        string VIEW_FORM_PARTIAL = "Partial/StockOpnameScan_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/StockOpnameScan_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/StockOpnameScan_Panel_List_Partial";


        StockOpnameScanService stockOpnameScanService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            stockOpnameScanService = new StockOpnameScanService();
            StockOpnameScanModel stockOpnameModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                stockOpnameModel = stockOpnameScanService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                stockOpnameScanService = new StockOpnameScanService();
                stockOpnameModel = stockOpnameScanService.GetById(userId, Id);
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, stockOpnameModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            StockOpnameScanModel stockOpnameModel;

            stockOpnameScanService = new StockOpnameScanService();
            if (Id == 0)
            {
                stockOpnameModel = stockOpnameScanService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                stockOpnameModel = stockOpnameScanService.GetById(userId, Id);
                if (stockOpnameModel != null)
                {
                    stockOpnameModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    stockOpnameModel = stockOpnameScanService.GetNewModel(userId);
                    stockOpnameModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  StockOpnameModel stockOpnameModel)
        //{
        //    int userId = (int)Session["userId"];

        //    stockOpnameModel._UserId = (int)Session["userId"];
        //    stockOpnameService = new StockOpnameService();

        //    if (ModelState.IsValid)
        //    {
        //        long Id = 0;

        //        Id = stockOpnameService.Add(stockOpnameModel);
        //        stockOpnameModel = stockOpnameService.GetById(userId, Id);
        //        stockOpnameModel._FormMode = Models.FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();
        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  StockOpnameScanModel stockOpnameModel)
        {
            int userId = (int)Session["userId"];

            stockOpnameModel._UserId = (int)Session["userId"];
            stockOpnameScanService = new StockOpnameScanService();
            stockOpnameModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            stockOpnameScanService.Update(stockOpnameModel);
            stockOpnameModel = stockOpnameScanService.GetById(userId, stockOpnameModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  StockOpnameScanModel stockOpnameModel)
        {
            int userId = (int)Session["userId"];

            stockOpnameModel._UserId = (int)Session["userId"];
            stockOpnameScanService = new StockOpnameScanService();
            stockOpnameModel._FormMode = FormModeEnum.Edit;

            stockOpnameScanService.Update(stockOpnameModel);
            //stockOpnameService.PostAPI(userId, stockOpnameModel.Id);
            stockOpnameScanService.Post(userId, stockOpnameModel.Id);
            stockOpnameModel = stockOpnameScanService.GetById(userId, stockOpnameModel.Id);

            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                stockOpnameModel = stockOpnameScanService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            StockOpnameScanModel stockOpnameModel;

            stockOpnameScanService = new StockOpnameScanService();
            stockOpnameScanService.Cancel(userId, Id, CancelReason);

            stockOpnameModel = stockOpnameScanService.GetById(userId, Id);
            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                stockOpnameModel = stockOpnameScanService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }
        
    }
}