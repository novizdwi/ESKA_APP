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
using Models.Production;

namespace Controllers.Production   // <-- SESUAIKAN dengan namespace controller Anda
{
    // NB: class controller utama harus dideklarasikan "partial" juga.
    public partial class ProductionTaskActivityController : BaseController
    {

        string VIEW_FINISH_PANEL_PARTIAL = "Partial/Finish/Finish_Panel_Partial";
        string VIEW_FINISH_FORM_PARTIAL = "Partial/Finish/Finish_Form_Partial";
        string VIEW_TAB_ITEM = "Partial/Finish/Finish_TabItem_List_Partial";

        public ActionResult Finish_PopupListOnDemandPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;        
            ViewBag.DetId = DetId;  

            var productionActivityService = new ProductionActivityService();
            ProductionActivityFinishModel model = productionActivityService.GetFinishModel(Id, DetId);

            return PartialView(VIEW_FINISH_PANEL_PARTIAL, model);
        }

        // Dipakai PopupControl saat LoadContentViaCallback = OnFirstShow.
        public ActionResult PopupFinishLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new ProductionTaskActivityModel();

            return PartialView(VIEW_FINISH_FORM_PARTIAL, model);
        }

        // Id            = Tx_ProductionTask (pemilik item)
        // ActivityDetId = Tx_ProductionTask_Activity yang sedang dibuka (hanya untuk konteks popup)
        // DetId pada baris grid = kunci Tx_ProductionTask_Item
        public ActionResult TabItemListPartial(long Id = 0, long ActivityDetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.ActivityDetId = ActivityDetId;
            productionActivityService = new ProductionActivityService();

            // Item menempel pada task, jadi diambil pakai Id -- bukan kunci activity.
            // ActivityDetId dipakai hitung QuantitySession (kontribusi activity ini saja).
            var modelList = productionActivityService.GetProductionTaskDetailItems(Id, ActivityDetId);

            return PartialView(VIEW_TAB_ITEM, modelList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabItemListAddNewRow([Bind] ProductionTaskDetailItemModel model, long Id = 0, long ActivityDetId = 0)
        {
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model._UserId = (int)Session["userId"];
                productionActivityService.ProductionTaskActivity_AddNewItem(model);
            }

            return TabItemListPartial(Id, ActivityDetId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabItemListUpdateRow([Bind] ProductionTaskDetailItemModel model, long Id = 0, long ActivityDetId = 0)
        {
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model._UserId = (int)Session["userId"];
                productionActivityService.ProductionTaskActivity_UpdateItem(model);
            }

            return TabItemListPartial(Id, ActivityDetId);
        }

        // Nama parameter kunci harus sama dengan KeyFieldName grid ("DetId").
        [HttpPost, ValidateInput(false)]
        public ActionResult TabItemListDeleteRow(long DetId, long Id = 0, long ActivityDetId = 0)
        {
            int userId = (int)Session["userId"];
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                productionActivityService.ProductionTaskActivity_DeleteItem(userId, Id, DetId);
            }

            return TabItemListPartial(Id, ActivityDetId);
        }


    }
}
