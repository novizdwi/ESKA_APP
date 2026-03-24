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
    public partial class TransferSummaryInController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/TransferSummaryIn_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            transferSummaryInService = new TransferSummaryInService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = transferSummaryInService.TransferSummaryIn_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}