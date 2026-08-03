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

        string VIEW_SCALE_PANEL_PARTIAL = "Partial/Scale/Scale_Panel_Partial";
        string VIEW_SCALE_FORM_PARTIAL = "Partial/Scale/Scale_Form_Partial";
        string VIEW_TAB_SCALE = "Partial/Scale/Scale_TabScaleList_List_Partial";

        // Satu popup Timbangan dipakai bersama Issue & Receipt; dibedakan parameter `side`.
        public ActionResult ViewScale_PopupListOnDemandPartial(long id = 0, long detId = 0, long detDetId = 0, string side = "Issue")
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = id;
            ViewBag.DetId = detId;
            ViewBag.DetDetId = detDetId;
            ViewBag.Side = side;

            reProcessService = new ReProcessService();
            var model = new ReProcessScaleView___();
            if (id != 0 && detId != 0 && detDetId != 0)
            {
                model = reProcessService.GetScale(id, detId, detDetId, side);
            }
            return PartialView(VIEW_SCALE_PANEL_PARTIAL, model);
        }

        public ActionResult TabScaleListPartial(long Id = 0, long DetId = 0, long DetDetId = 0, string side = "Issue")
        {
            int userId = (int)Session["userId"];
            ViewBag.Id = Id;
            ViewBag.DetId = DetId;
            ViewBag.DetDetId = DetDetId;
            ViewBag.Side = side;
            reProcessService = new ReProcessService();

            var modelList = reProcessService.ReProcess__ItemBatchScaleList(DetDetId, side);

            return PartialView(VIEW_TAB_SCALE, modelList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabScaleListAddNewRow([Bind] ReProcessScaleModel model, long Id = 0, long DetId = 0, long DetDetId = 0, string side = "Issue")
        {
            long id = Id;
            long detId = DetId;
            long detDetId = DetDetId;
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                model.DetDetId = DetDetId;
                model._UserId = (int)Session["userId"];
                reProcessService.ReProcess_AddNewItemBatchScale(model, side);
            }

            return TabScaleListPartial(id, detId, detDetId, side);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabScaleListUpdateRow([Bind] ReProcessScaleModel model, long Id = 0, long DetId = 0, long DetDetId = 0, string side = "Issue")
        {
            long id = Id;
            long detId = DetId;
            long detDetId = DetDetId;
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                model._UserId = (int)Session["userId"];
                reProcessService.ReProcess_UpdateItemBatchScale(model, side);
            }

            return TabScaleListPartial(id, detId, detDetId, side);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TabScaleListDeleteRow(long DetDetDetId, long Id = 0, long DetId = 0, long DetDetId = 0, string side = "Issue")
        {
            long id = Id;
            long detId = DetId;
            long detDetId = DetDetId;
            int userId = (int)Session["userId"];
            reProcessService = new ReProcessService();

            if (ModelState.IsValid)
            {
                reProcessService.ReProcess_DeleteItemBatchScale(userId, DetDetId, DetDetDetId, side);
            }

            return TabScaleListPartial(id, detId, detDetId, side);
        }


    }

}
