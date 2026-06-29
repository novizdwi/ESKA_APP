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
    public partial class ProductionSchedulerController : BaseController
    {

        string VIEW_DETAIL = "ProductionScheduler";
        string VIEW_FORM_PARTIAL = "Partial/ProductionScheduler_Form_Partial";
        string VIEW_FORM_TABREFERENCE_PARTIAL = "Partial/ProductionScheduler_Form_TabReference_List_Partial";

        ProductionSchedulerService productionSchedulerService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            productionSchedulerService = new ProductionSchedulerService();
            ProductionSchedulerModel productionSchedulerModel;
            ViewBag.initNew = true;

            productionSchedulerModel = productionSchedulerService.GetNewModel(userId);
            productionSchedulerModel.UserId = userId;

            return View(VIEW_DETAIL, productionSchedulerModel);
        }

        public ActionResult DetailPartial(DateTime? fromDate = null, DateTime? toDate = null, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            int userId = (int)Session["userId"];
            fromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            toDate = toDate ?? DateTime.Now;
            
            ProductionSchedulerModel productionSchedulerModel;

            productionSchedulerService = new ProductionSchedulerService();
            productionSchedulerModel = productionSchedulerService.GetNewModel(userId);

            return PartialView(VIEW_FORM_PARTIAL, productionSchedulerModel);
        }

        public ActionResult Find(DateTime? fromDate = null, DateTime? toDate = null, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            int userId = (int)Session["userId"];
            DateTime fromDate2 = fromDate ?? DateTime.Now.AddMonths(-1);
            DateTime toDate2 = toDate ?? DateTime.Now;

            productionSchedulerService = new ProductionSchedulerService();
            ProductionSchedulerModel models = productionSchedulerService.Find(userId, fromDate2, toDate2, itemCode, whsCode, tagId, status);

            return PartialView(VIEW_FORM_PARTIAL, models);
        }
    }
}