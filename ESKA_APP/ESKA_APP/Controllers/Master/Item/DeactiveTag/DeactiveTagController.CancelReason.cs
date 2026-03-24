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
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class DeactiveTagController : BaseController
    {

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/CancelReason/CancelReason_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/CancelReason/CancelReason_Form_Partial";

        public ActionResult CancelReason_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();

            DeactiveTagModel model;

            if (Id != 0)
            {
                model = deactiveTagService.GetById(userId, Id);
            }
            else
            {
                model = deactiveTagService.GetNewModel(userId);
            }

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }


        public ActionResult PopupCancelReasonLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new DeactiveTagModel();

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, model);
        }
    }
}
