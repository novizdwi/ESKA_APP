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

        public ActionResult Finish_PopupListOnDemandPartial(long Id = 0, long DetId = 0)
        {
            int userId = (int)Session["userId"];

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


    }
}
