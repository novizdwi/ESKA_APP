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

namespace Controllers.Production
{
    public partial class ProductionScheduleController : BaseController
    {
        public ActionResult TabTransListPartial()
        {
            int userId = (int)Session["userId"];
            productionScheduleService = new ProductionScheduleService();

            var modelList = productionScheduleService.ProductionSchedule_GetReferences(userId);

            return PartialView(VIEW_FORM_TABREFERENCE_PARTIAL, modelList);
        }

        // Dipanggil sesudah user memindahkan baris (drag & drop).
        // ids = Id Tx_ProcessCard sesuai urutan tampil terbaru -> VisOrder diisi ulang 1..N.
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateVisOrder(List<long> ids)
        {
            int userId = (int)Session["userId"];
            productionScheduleService = new ProductionScheduleService();

            productionScheduleService.UpdateVisOrder(userId, ids);

            return Json(new { success = true });
        }




    }
}