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
            if (id != 0 && detId != 0)
            {
                model = productionActivityService.GetProductionTaskActivity_Batch(id, detId, detDetId);
            }
            return PartialView(VIEW_ITEMBATCH_PANEL_PARTIAL, model);
        }

        public ActionResult TabBatchListPartial(long Id = 0, long DetId = 0, long DetDetId = 0)
        {
            int userId = (int)Session["userId"]; 
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            productionActivityService = new ProductionActivityService();
             
            var modelList = productionActivityService.GetProductionTaskActivity_ItemBatchList(Id, DetId, DetDetId); 

            return PartialView(VIEW_TAB_BATCH, modelList);
        }

        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new ProductionTaskActivityModel();

            return PartialView(VIEW_ITEMBATCH_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListAddNewRow([Bind] ProductionTaskActivityBatchModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetDetId = DetId;
                model._UserId = (int)Session["userId"];
                productionActivityService.ProductionTaskActivity_AddNewItemBatch(model);
            }

            return TabBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListUpdateRow([Bind] ProductionTaskActivityBatchModel model, long Id = 0, long DetId = 0, long DetDetId = 0) {
            long id = Id;
            long detId = DetId;
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            { 
                model._UserId = (int)Session["userId"];
                productionActivityService.ProductionTaskActivity_UpdateItemBatch(model);
            }

            return TabBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListDeleteRow(long DetDetId, long Id = 0, long DetId = 0)
        {
            long id = Id ;
            long detId = DetId ;
            int userId = (int)Session["userId"];
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                productionActivityService.ProductionTaskActivity_DeleteItemBatch(userId, Id, DetId, DetDetId);
            }

            return TabBatchListPartial(id, detId);
        }

    }

}