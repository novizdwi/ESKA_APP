using Models;
using Models.Transaction;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction
{
    public partial class ReProcessController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/BatchReceipt/BatchReceipt_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/BatchReceipt/BatchReceipt_Form_Partial";
        string VIEW_TAB_BATCH = "Partial/BatchReceipt/BatchReceipt_TabBatchList_List_Partial";

        public ActionResult ViewReceiptBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;

            reProcessService = new ReProcessService();
            var model = new ReProcessBatchReceiptView___();
            if (id != 0 && detId != 0)
            {
                model = reProcessService.GetReceiptBatch(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }

        public ActionResult TabReceiptBatchListPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            reProcessService = new ReProcessService();

            var modelList = reProcessService.ReProcess__ReceiptItemBatchList(DetId);

            return PartialView(VIEW_TAB_BATCH, modelList);
        }

        public ActionResult PopupItemTagLoadOnDemandPartial()
        {

            int userId = (int)Session["userId"];
            var model = new ReProcessModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabReceiptBatchListAddNewRow([Bind] ReProcessBatchReceiptModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                //model.Id = Id;
                model.DetDetId = DetId;
                model._UserId = (int)Session["userId"];
                reProcessService.ReProcess__ReceiptAddNewItemBatch(model);
            }

            return TabReceiptBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabReceiptBatchListUpdateRow([Bind] ReProcessBatchReceiptModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                model._UserId = (int)Session["userId"];
                reProcessService.ReProcess_ReceiptUpdateItemBatch(model);
            }

            return TabReceiptBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabReceiptBatchListDeleteRow(long DetDetId, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            int userId = (int)Session["userId"];
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                reProcessService.ReProcess_ReceiptDeleteItemBatch(userId, Id, DetId, DetDetId);
            }

            return TabReceiptBatchListPartial(id, detId);
        }

    }

}