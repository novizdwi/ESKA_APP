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
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class ReProcessController : BaseController
    {

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/CancelReason/CancelReason_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/CancelReason/CancelReason_Form_Partial";

        public ActionResult CancelReason_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            reProcessService = new ReProcessService();

            ReProcessModel model;

            if (Id != 0)
            {
                model = reProcessService.GetById(userId, Id);
            }
            else
            {
                model = reProcessService.GetNewModel(userId);
            }

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }
    }
}
