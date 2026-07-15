using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Production;

namespace Controllers.Production
{
    public partial class ProductionTaskActivityController : BaseController
    {
        string VIEW_DETAIL = "ProductionTaskActivity";
        string VIEW_FORM_PARTIAL = "Partial/ProductionTaskActivity_Form_Partial";

        ProductionActivityService productionActivityService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];
            productionActivityService = new ProductionActivityService();
            ProductionTaskActivityModel ProductionTaskActivityModel;
            ProductionTaskActivityModel = productionActivityService.GetNewModel(userId, Id);

            return View(VIEW_DETAIL, ProductionTaskActivityModel);
        }

        public ActionResult DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ProductionTaskActivityModel ProductionTaskActivityModel;

            productionActivityService = new ProductionActivityService();
            ProductionTaskActivityModel = productionActivityService.GetNewModel(userId, Id);

            return PartialView(VIEW_FORM_PARTIAL, ProductionTaskActivityModel);
        }
         
    }
}