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
        string VIEW_TAB_BATCH = "Partial/Batch/Batch_TabBatchList_List_Partial";

        public ActionResult ViewBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            //int userId = (int)Session["userId"];

            //goodsReceiptPoService = new GoodsReceiptPoService();
            //var model = new GoodsReceiptPoBatchView___();
            //if(id != 0 && detId != 0)
            //{
            //    model = goodsReceiptPoService.GetBatch(id, detId);
            //}
            //return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;

            goodsReceiptPoService = new GoodsReceiptPoService();
            var model = new GoodsReceiptPoBatchView___();
            if (id != 0 && detId != 0)
            {
                model = goodsReceiptPoService.GetBatch(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }

        public ActionResult TabBatchListPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            goodsReceiptPoService = new GoodsReceiptPoService();

            var modelList = goodsReceiptPoService.GetStockOpname_ItemBatchList(Id, DetId);

            return PartialView(VIEW_TAB_BATCH, modelList);
        }

        public ActionResult PopupItemTagLoadOnDemandPartial()
        {

            int userId = (int)Session["userId"];
            var model = new GoodsReceiptPoModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListAddNewRow([Bind] StockOpnameBatchModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetDetId = DetId;
                model._UserId = (int)Session["userId"];
                goodsReceiptPoService.StockOpname_AddNewItemBatch(model);
            }

            return TabBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListUpdateRow([Bind] StockOpnameBatchModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                model._UserId = (int)Session["userId"];
                goodsReceiptPoService.StockOpname_UpdateItemBatch(model);
            }

            return TabBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListDeleteRow(long DetDetId, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            int userId = (int)Session["userId"];
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                goodsReceiptPoService.StockOpname_DeleteItemBatch(userId, Id, DetId, DetDetId);
            }

            return TabBatchListPartial(id, detId);
        }

      
    }

}