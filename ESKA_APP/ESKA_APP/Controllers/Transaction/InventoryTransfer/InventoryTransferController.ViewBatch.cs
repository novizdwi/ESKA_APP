using Models;
using Models.Transaction;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction
{
    public partial class InventoryTransferController : BaseController
    {

        string VIEW_ITEMBATCH_PANEL_PARTIAL = "Partial/Batch/Batch_Panel_Partial";
        string VIEW_ITEMBATCH_FORM_PARTIAL = "Partial/Batch/Batch_Form_Partial";
        string VIEW_TAB_BATCH = "Partial/Batch/Batch_TabBatchList_List_Partial";

        public ActionResult ViewBatch_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;

            inventoryTransferService = new InventoryTransferService();
            var model = new InventoryTransferBatchView___();
            if (id != 0 && detId != 0)
            {
                model = inventoryTransferService.GetInventoryTransfer_Batch(id, detId);
            }
            return PartialView(VIEW_ITEMBATCH_PANEL_PARTIAL, model);
        }

        public ActionResult TabBatchListPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"]; 
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            inventoryTransferService = new InventoryTransferService();
             
            var modelList = inventoryTransferService.GetInventoryTransfer_ItemBatchList(Id, DetId); 

            return PartialView(VIEW_TAB_BATCH, modelList);
        }

        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new InventoryTransferModel();

            return PartialView(VIEW_ITEMBATCH_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListAddNewRow([Bind] InventoryTransferBatchModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            inventoryTransferService = new InventoryTransferService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetId = DetId;
                model._UserId = (int)Session["userId"];
                inventoryTransferService.InventoryTransfer_AddNewItemBatch(model);
            }

            return TabBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListUpdateRow([Bind] InventoryTransferBatchModel model, long Id = 0, long DetId = 0) {
            long id = Id;
            long detId = DetId;
            inventoryTransferService = new InventoryTransferService();

            if (ModelState.IsValid)
            { 
                model._UserId = (int)Session["userId"];
                model.Id = Id;
                model.DetId = DetId;
                inventoryTransferService.InventoryTransfer_UpdateItemBatch(model);
            }

            return TabBatchListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabBatchListDeleteRow(long DetDetId, long Id = 0, long DetId = 0)
        {
            long id = Id ;
            long detId = DetId ;
            int userId = (int)Session["userId"];
            inventoryTransferService = new InventoryTransferService();

            if (ModelState.IsValid)
            {
                inventoryTransferService.InventoryTransfer_DeleteItemBatch(userId, Id, DetId, DetDetId);
            }

            return TabBatchListPartial(id, detId);
        }

    }

}