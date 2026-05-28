using Models;
using Models.Transaction;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction
{
    public partial class GoodsReceiptPoController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/Batch/Batch_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/Batch/Batch_Form_Partial";

        public ActionResult ViewBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];

            goodsReceiptPoService = new GoodsReceiptPoService();
            var model = new GoodsReceiptPoBatchView___();
            if(id != 0 && detId != 0)
            {
                model = goodsReceiptPoService.GetBatch(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }


        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new GoodsReceiptPoModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }
    }

}