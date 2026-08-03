using Models;
using Models.Production;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Production
{
    public partial class ProductionTaskActivityController : BaseController
    {

        string VIEW_ITEMBATCH_PANEL_PARTIAL = "Partial/Batch/Batch_Panel_Partial";
        string VIEW_ITEMBATCH_FORM_PARTIAL = "Partial/Batch/Batch_Form_Partial";
        string VIEW_TAB_BATCH = "Partial/Batch/Batch_TabBatchList_List_Partial";

        public ActionResult ViewBatch_PopupListOnDemandPartial(long id = 0, long detId = 0, long detDetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;
            ViewBag.DetDetId = detDetId;

            productionActivityService = new ProductionActivityService();
            var model = new ProductionTaskActivityBatchView___();
            if (detDetId != 0)
            {
                model = productionActivityService.GetProductionTaskActivity_Batch(id, detId, detDetId);
            }

            return PartialView(VIEW_ITEMBATCH_PANEL_PARTIAL, model);
        }

        // Dipakai PopupControl saat LoadContentViaCallback = OnFirstShow.
        public ActionResult PopupItemBatchLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new ProductionTaskActivityBatchView___();

            return PartialView(VIEW_ITEMBATCH_FORM_PARTIAL, model);
        }

        public ActionResult TabBatchListPartial(long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            ViewBag.DetDetId = DetDetId;

            productionActivityService = new ProductionActivityService();

            var modelList = productionActivityService.GetProductionTaskActivity_ItemBatchList(Id, DetId, DetDetId);

            return PartialView(VIEW_TAB_BATCH, modelList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListAddNewRow([Bind] ProductionTaskActivityBatchModel model, long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetId = DetId;
                model.DetDetId = DetDetId;
                model._UserId = (int)Session["userId"];

                productionActivityService.ProductionTaskActivity_AddNewItemBatch(model);
            }

            return TabBatchListPartial(Id, DetId, DetDetId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListUpdateRow([Bind] ProductionTaskActivityBatchModel model, long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetId = DetId;
                model.DetDetId = DetDetId;
                model._UserId = (int)Session["userId"];

                productionActivityService.ProductionTaskActivity_UpdateItemBatch(model);
            }

            return TabBatchListPartial(Id, DetId, DetDetId);
        }

        // Nama parameter kunci harus sama dengan KeyFieldName grid ("BatchId").
        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListDeleteRow(long BatchId, long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            int userId = (int)Session["userId"];
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                productionActivityService.ProductionTaskActivity_DeleteItemBatch(userId, Id, DetId, DetDetId, BatchId);
            }

            return TabBatchListPartial(Id, DetId, DetDetId);
        }

    }

}
