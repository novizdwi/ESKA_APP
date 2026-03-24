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

using Models.Transaction.Purchasing;

namespace Controllers.Transaction.Purchasing
{
    public partial class PurchaseOrderScanController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/PurchaseOrderScan_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            purchaseOrderScanService = new PurchaseOrderScanService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = purchaseOrderScanService.PurchaseOrder_Items(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}