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
    public partial class ProductionTaskController : BaseController
    {

        string VIEW_DETAIL = "ProductionTask";
        string VIEW_FORM_PARTIAL = "Partial/ProductionTask_Form_Partial";
        string VIEW_FORM_TABREFERENCE_PARTIAL = "Partial/ProductionTask_Form_TabReference_List_Partial";

        ProductionTaskService productionTaskService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            productionTaskService = new ProductionTaskService();
            ProductionTaskModel productionTaskModel;
            ViewBag.initNew = true;

            productionTaskModel = productionTaskService.GetNewModel(userId);
            productionTaskModel.UserId = userId;

            return View(VIEW_DETAIL, productionTaskModel);
        }

        public ActionResult DetailPartial(DateTime? fromDate = null, DateTime? toDate = null, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            int userId = (int)Session["userId"];
            fromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            toDate = toDate ?? DateTime.Now;
            
            ProductionTaskModel productionTaskModel;

            productionTaskService = new ProductionTaskService();
            productionTaskModel = productionTaskService.GetNewModel(userId);

            return PartialView(VIEW_FORM_PARTIAL, productionTaskModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))] ProductionTask_Detail productionTaskModel)
        {
            int userId = (int)Session["userId"];

            productionTaskModel.UserId = (int)Session["userId"];
            productionTaskService = new ProductionTaskService();

            productionTaskService.Update(productionTaskModel);
            ProductionTaskModel model = productionTaskService.GetNewModel(userId);

            return PartialView(VIEW_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Close(long Id)
        {

            int userId = (int)Session["userId"];
            
            productionTaskService = new ProductionTaskService();
            productionTaskService.Close(Id, userId);
            ProductionTaskModel productionTaskModel;
            ViewBag.initNew = true;

            productionTaskModel = productionTaskService.GetNewModel(userId);
            productionTaskModel.UserId = userId;

            return View(VIEW_DETAIL, productionTaskModel);
        }
    }
}