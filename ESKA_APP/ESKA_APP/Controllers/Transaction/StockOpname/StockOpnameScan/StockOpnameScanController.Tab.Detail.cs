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
    public partial class StockOpnameScanController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/StockOpnameScan_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            stockOpnameScanService = new StockOpnameScanService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = stockOpnameScanService.StockOpname_Items(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}