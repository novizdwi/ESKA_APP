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
using Models.Transaction.Purchasing;

namespace Controllers.Transaction.Purchasing
{
    public partial class PurchaseOrderScanController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            PurchaseOrderScanModel purchaseOrderModel;
            purchaseOrderScanService = new PurchaseOrderScanService();

            purchaseOrderModel = purchaseOrderScanService.NavFirst(userId);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            if (purchaseOrderModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            PurchaseOrderScanModel purchaseOrderModel;
            purchaseOrderScanService = new PurchaseOrderScanService();

            purchaseOrderModel = purchaseOrderScanService.NavPrevious(userId, Id);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            if (purchaseOrderModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            PurchaseOrderScanModel purchaseOrderModel;
            purchaseOrderScanService = new PurchaseOrderScanService();

            purchaseOrderModel = purchaseOrderScanService.NavNext(userId, Id);
            if (purchaseOrderModel != null)
            {

                purchaseOrderModel._FormMode = FormModeEnum.Edit;

            }

            if (purchaseOrderModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            PurchaseOrderScanModel purchaseOrderScanModel;
            purchaseOrderScanService = new PurchaseOrderScanService();

            purchaseOrderScanModel = purchaseOrderScanService.NavLast(userId);
            if (purchaseOrderScanModel != null)
            {
                purchaseOrderScanModel._FormMode = FormModeEnum.Edit;
            }

            if (purchaseOrderScanModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderScanModel);
        }



    }
}