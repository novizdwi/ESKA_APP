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

        // id    = Tx_ProductionTask
        // detId = kunci Tx_ProductionTask_Item (level 2) yang batch nya dibuka
        public ActionResult ViewBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;

            productionActivityService = new ProductionActivityService();
            var model = new ProductionTaskActivityBatchView___();
            if (detId != 0)
            {
                model = productionActivityService.GetProductionTaskActivity_Batch(id, detId);
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

        public ActionResult TabBatchListPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;

            productionActivityService = new ProductionActivityService();

            var modelList = productionActivityService.GetProductionTaskActivity_ItemBatchList(Id, DetId);

            return PartialView(VIEW_TAB_BATCH, modelList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListAddNewRow([Bind] ProductionTaskActivityBatchModel model, long Id = 0, long DetId = 0)
        {
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetId = DetId;
                model._UserId = (int)Session["userId"];

                productionActivityService.ProductionTaskActivity_AddNewItemBatch(model);
            }

            return TabBatchListPartial(Id, DetId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListUpdateRow([Bind] ProductionTaskActivityBatchModel model, long Id = 0, long DetId = 0)
        {
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetId = DetId;
                model._UserId = (int)Session["userId"];

                productionActivityService.ProductionTaskActivity_UpdateItemBatch(model);
            }

            return TabBatchListPartial(Id, DetId);
        }

        // Nama parameter kunci harus sama dengan KeyFieldName grid ("BatchId").
        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListDeleteRow(long BatchId, long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                productionActivityService.ProductionTaskActivity_DeleteItemBatch(userId, Id, DetId, BatchId);
            }

            return TabBatchListPartial(Id, DetId);
        }

    }

}
