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

        string VIEW_TAB_Ref_COMPONENT = "Partial/TransferSummaryIn_Form_TabRef_Partial";

        public ActionResult TabRefListPartial()
        {
            int userId = (int)Session["userId"];

            transferSummaryInService = new TransferSummaryInService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListRef = transferSummaryInService.TransferSummaryIn_Refs(Id);

            return PartialView(VIEW_TAB_Ref_COMPONENT, modelListRef);
        }
        

    }
}