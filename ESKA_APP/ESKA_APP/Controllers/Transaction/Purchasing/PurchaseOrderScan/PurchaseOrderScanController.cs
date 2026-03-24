using Models;
using Models.Transaction.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Purchasing
{
    public partial class PurchaseOrderScanController : BaseController
    {
        string VIEW_DETAIL = "PurchaseOrderScan";
        string VIEW_FORM_PARTIAL = "Partial/PurchaseOrderScan_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/PurchaseOrderScan_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/PurchaseOrderScan_Panel_List_Partial";


        PurchaseOrderScanService purchaseOrderScanService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            purchaseOrderScanService = new PurchaseOrderScanService();
            PurchaseOrderScanModel purchaseOrderModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                purchaseOrderModel = purchaseOrderScanService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }
            else
            {
                purchaseOrderScanService = new PurchaseOrderScanService();
                purchaseOrderModel = purchaseOrderScanService.GetById(userId, Id);
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, purchaseOrderModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            PurchaseOrderScanModel purchaseOrderModel;

            purchaseOrderScanService = new PurchaseOrderScanService();
            if (Id == 0)
            {
                purchaseOrderModel = purchaseOrderScanService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }
            else
            {
                purchaseOrderModel = purchaseOrderScanService.GetById(userId, Id);
                if (purchaseOrderModel != null)
                {
                    purchaseOrderModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    purchaseOrderModel = purchaseOrderScanService.GetNewModel(userId);
                    purchaseOrderModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  PurchaseOrderModel purchaseOrderModel)
        //{
        //    int userId = (int)Session["userId"];

        //    purchaseOrderModel._UserId = (int)Session["userId"];
        //    purchaseOrderService = new PurchaseOrderService();

        //    if (ModelState.IsValid)
        //    {
        //        long Id = 0;

        //        Id = purchaseOrderService.Add(purchaseOrderModel);
        //        purchaseOrderModel = purchaseOrderService.GetById(userId, Id);
        //        purchaseOrderModel._FormMode = Models.FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();
        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  PurchaseOrderScanModel purchaseOrderModel)
        {
            int userId = (int)Session["userId"];

            purchaseOrderModel._UserId = (int)Session["userId"];
            purchaseOrderScanService = new PurchaseOrderScanService();
            purchaseOrderModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            purchaseOrderScanService.Update(purchaseOrderModel);
            purchaseOrderModel = purchaseOrderScanService.GetById(userId, purchaseOrderModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  PurchaseOrderScanModel purchaseOrderModel)
        {
            int userId = (int)Session["userId"];

            purchaseOrderModel._UserId = (int)Session["userId"];
            purchaseOrderScanService = new PurchaseOrderScanService();
            purchaseOrderModel._FormMode = FormModeEnum.Edit;

            purchaseOrderScanService.Update(purchaseOrderModel);
            //purchaseOrderService.PostAPI(userId, purchaseOrderModel.Id);
            purchaseOrderScanService.Post(userId, purchaseOrderModel.Id);
            purchaseOrderModel = purchaseOrderScanService.GetById(userId, purchaseOrderModel.Id);

            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                purchaseOrderModel = purchaseOrderScanService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            PurchaseOrderScanModel purchaseOrderModel;

            purchaseOrderScanService = new PurchaseOrderScanService();
            purchaseOrderScanService.Cancel(userId, Id, CancelReason);

            purchaseOrderModel = purchaseOrderScanService.GetById(userId, Id);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                purchaseOrderModel = purchaseOrderScanService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }
        
    }
}