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

        string VIEW_SCALE_PANEL_PARTIAL = "Partial/Scale/Scale_Panel_Partial";
        string VIEW_SCALE_FORM_PARTIAL = "Partial/Scale/Scale_Form_Partial";
        string VIEW_TAB_SCALE = "Partial/Scale/Scale_TabScaleList_List_Partial";

        public ActionResult ViewScale_PopupListOnDemandPartial(long id = 0, long detId = 0, long detDetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;
            ViewBag.DetDetId = detDetId;

            goodsReceiptPoService = new GoodsReceiptPoService();
            var model = new GoodsReceiptPoScaleView___();
            if (id != 0 && detId != 0 && detDetId != 0)
            {
                model = goodsReceiptPoService.GetScale(id, detId, detDetId);
            }
            return PartialView(VIEW_SCALE_PANEL_PARTIAL, model);
        }

        public ActionResult TabScaleListPartial(long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            ViewBag.DetDetId = DetDetId;
            goodsReceiptPoService = new GoodsReceiptPoService();

            var modelList = goodsReceiptPoService.GoodsReceiptPo__ItemBatchScaleList(DetDetId);

            return PartialView(VIEW_TAB_SCALE, modelList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabScaleListAddNewRow([Bind] GoodsReceiptPoScaleModel model, long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            long id = Id;
            long detId = DetId;
            long detDetId = DetDetId;
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                model.DetDetId = DetDetId;
                model._UserId = (int)Session["userId"];
                goodsReceiptPoService.GoodsReceiptPo_AddNewItemBatchScale(model);
            }

            return TabScaleListPartial(id, detId, detDetId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabScaleListUpdateRow([Bind] GoodsReceiptPoScaleModel model, long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            long id = Id;
            long detId = DetId;
            long detDetId = DetDetId;
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                model._UserId = (int)Session["userId"];
                goodsReceiptPoService.GoodsReceiptPo_UpdateItemBatchScale(model);
            }

            return TabScaleListPartial(id, detId, detDetId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabScaleListDeleteRow(long DetDetDetId, long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            long id = Id;
            long detId = DetId;
            long detDetId = DetDetId;
            int userId = (int)Session["userId"];
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                goodsReceiptPoService.GoodsReceiptPo_DeleteItemBatchScale(userId, DetDetId, DetDetDetId);
            }

            return TabScaleListPartial(id, detId, detDetId);
        }

        // Tombol "Scale" (Timbang) -> simpan permintaan timbang ke Tp_ScaleStaging.
        [HttpPost, ValidateInput(false)]
        public ContentResult SaveScaleStaging(long Id, long DetId, long DetDetId, long DetDetDetId)
        {
            int userId = (int)Session["userId"];
            new Models._Utils.ScaleStagingService().SaveFromScale(userId, "GoodsReceiptPo", Id, DetId, DetDetId, DetDetDetId);
            return Content("OK");
        }


    }

}
