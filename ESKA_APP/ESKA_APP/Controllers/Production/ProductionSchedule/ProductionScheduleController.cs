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

        string VIEW_DETAIL = "ProductionSchedule";
        string VIEW_FORM_PARTIAL = "Partial/ProductionSchedule_Form_Partial";
        string VIEW_FORM_TABREFERENCE_PARTIAL = "Partial/ProductionSchedule_Form_TabReference_List_Partial";

        ProductionScheduleService productionScheduleService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            productionScheduleService = new ProductionScheduleService();
            ProductionScheduleModel productionScheduleModel;
            ViewBag.initNew = true;

            productionScheduleModel = productionScheduleService.GetNewModel(userId);
            productionScheduleModel.UserId = userId;

            return View(VIEW_DETAIL, productionScheduleModel);
        }

        public ActionResult DetailPartial(DateTime? fromDate = null, DateTime? toDate = null, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            int userId = (int)Session["userId"];
            fromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            toDate = toDate ?? DateTime.Now;
            
            ProductionScheduleModel productionScheduleModel;

            productionScheduleService = new ProductionScheduleService();
            productionScheduleModel = productionScheduleService.GetNewModel(userId);

            return PartialView(VIEW_FORM_PARTIAL, productionScheduleModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))] ProductionSchedule_Detail productionScheduleModel)
        {
            int userId = (int)Session["userId"];

            productionScheduleModel.UserId = (int)Session["userId"];
            productionScheduleService = new ProductionScheduleService();

            productionScheduleService.Update(productionScheduleModel);
            ProductionScheduleModel model = productionScheduleService.GetNewModel(userId);

            return PartialView(VIEW_FORM_PARTIAL, model);
        }
    }
}