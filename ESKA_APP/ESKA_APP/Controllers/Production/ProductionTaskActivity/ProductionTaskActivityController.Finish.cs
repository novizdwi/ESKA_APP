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

        public ActionResult TabItemListPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            productionActivityService = new ProductionActivityService();

            var modelList = productionActivityService.GetProductionTaskDetailItems(DetId);

            return PartialView(VIEW_TAB_ITEM, modelList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabItemListAddNewRow([Bind] ProductionTaskDetailItemModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model.Id = Id;
                model.DetDetId = DetId;
                model._UserId = (int)Session["userId"];
                productionActivityService.ProductionTaskActivity_AddNewItem(model);
            }

            return TabItemListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabItemListUpdateRow([Bind] ProductionTaskDetailItemModel model, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                model._UserId = (int)Session["userId"];
                productionActivityService.ProductionTaskActivity_UpdateItem(model);
            }

            return TabItemListPartial(id, detId);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabItemListDeleteRow(long DetDetId, long Id = 0, long DetId = 0)
        {
            long id = Id;
            long detId = DetId;
            int userId = (int)Session["userId"];
            productionActivityService = new ProductionActivityService();

            if (ModelState.IsValid)
            {
                productionActivityService.ProductionTaskActivity_DeleteItemBatch(userId, Id, DetId, DetDetId);
            }

            return TabItemListPartial(id, detId);
        }


    }
}
