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

using Models.Transaction.StockOpname;

namespace Controllers.Transaction.StockOpname
{
    public partial class StockSummaryOpnameController : BaseController
    {

        string VIEW_TAB_Ref_COMPONENT = "Partial/StockSummaryOpname_Form_TabRef_Partial";

        public ActionResult TabRefListPartial()
        {
            int userId = (int)Session["userId"];

            stockSummaryOpnameService = new StockSummaryOpnameService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListRef = stockSummaryOpnameService.StockSummaryOpname_Refs(Id);

            return PartialView(VIEW_TAB_Ref_COMPONENT, modelListRef);
        }
        

    }
}