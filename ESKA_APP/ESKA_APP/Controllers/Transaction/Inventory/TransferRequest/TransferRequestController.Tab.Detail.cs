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

using Models.Transaction.Inventory;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferRequestController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/TransferRequest_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            transferRequestService = new TransferRequestService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = transferRequestService.TransferRequest_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}