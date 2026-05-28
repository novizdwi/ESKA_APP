using Models;
using Models.Transaction;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction
{
    public partial class StockOpnameController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/Batch/Batch_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/Batch/Batch_Form_Partial";

        public ActionResult ViewBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];

            stockOpnameService = new StockOpnameService();
            var model = new StockOpnameBatchView___();
            if(id != 0 && detId != 0)
            {
                model = stockOpnameService.GetBatch(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }


        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new StockOpnameModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }
    }

}