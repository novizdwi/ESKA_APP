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
    public partial class RfidMonitoringController : BaseController
    {

        string VIEW_DETAIL = "RfidMonitoring";
        string VIEW_FORM_PARTIAL = "Partial/RfidMonitoring_Form_Partial";
        string VIEW_FORM_TABREFERENCE_PARTIAL = "Partial/RfidMonitoring_Form_TabReference_List_Partial";

        RfidMonitoringService rfidMonitoringService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            rfidMonitoringService = new RfidMonitoringService();
            RfidMonitoringModel rfidMonitoringModel;
            ViewBag.initNew = true;

            rfidMonitoringModel = rfidMonitoringService.GetNewModel(userId);
            rfidMonitoringModel.UserId = userId;

            return View(VIEW_DETAIL, rfidMonitoringModel);
        }

        public ActionResult DetailPartial(DateTime? fromDate = null, DateTime? toDate = null, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            int userId = (int)Session["userId"];
            fromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            toDate = toDate ?? DateTime.Now;
            
            RfidMonitoringModel rfidMonitoringModel;

            rfidMonitoringService = new RfidMonitoringService();
            rfidMonitoringModel = rfidMonitoringService.GetNewModel(userId);

            return PartialView(VIEW_FORM_PARTIAL, rfidMonitoringModel);
        }

        public ActionResult Find(DateTime? fromDate = null, DateTime? toDate = null, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            int userId = (int)Session["userId"];
            DateTime fromDate2 = fromDate ?? DateTime.Now.AddMonths(-1);
            DateTime toDate2 = toDate ?? DateTime.Now;

            rfidMonitoringService = new RfidMonitoringService();
            RfidMonitoringModel models = rfidMonitoringService.Find(userId, fromDate2, toDate2, itemCode, whsCode, tagId, status);

            return PartialView(VIEW_FORM_PARTIAL, models);
        }
    }
}