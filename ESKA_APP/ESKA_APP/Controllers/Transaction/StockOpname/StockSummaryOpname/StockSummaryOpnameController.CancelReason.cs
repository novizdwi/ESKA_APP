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
    public partial class StockSummaryOpnameController : BaseController
    {

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/CancelReason/CancelReason_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/CancelReason/CancelReason_Form_Partial";

        public ActionResult CancelReason_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            stockSummaryOpnameService = new StockSummaryOpnameService();

            StockSummaryOpnameModel model;

            if (Id != 0)
            {
                model = stockSummaryOpnameService.GetById(userId, Id);
            }
            else
            {
                model = stockSummaryOpnameService.GetNewModel(userId);
            }

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }


        public ActionResult PopupCancelReasonLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new StockSummaryOpnameModel();

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, model);
        }
    }
}