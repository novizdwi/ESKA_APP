using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Production;
using Newtonsoft.Json;

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

        [HttpPost]
        public ActionResult PauseActivity(ProductionActivityPauseModel pauseModel)
        {
            int userId = (int)Session["userId"];

            productionActivityService = new ProductionActivityService();
            productionActivityService.PauseActivity(userId, pauseModel);
            
            var model = productionActivityService.GetById(userId, pauseModel.Id??0);
            return PartialView(VIEW_FORM_PARTIAL, model);
        }

        [HttpPost]
        public ActionResult StartActivity(ProductionActivityPauseModel pauseModel)
        {
            int userId = (int)Session["userId"];

            productionActivityService = new ProductionActivityService();
            productionActivityService.StartActivity(userId, pauseModel);

            var model = productionActivityService.GetById(userId, pauseModel.Id ?? 0);
            return PartialView(VIEW_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FinishActivity(ProductionActivityFinishModel model, string itemQuantitiesJson)
        {
            int userId = (int)Session["userId"];

            // Kontribusi QuantitySession item Direction=In diketik manual di client (tidak
            // persisted sampai Finish disubmit) -> dikirim terpisah sebagai JSON, bukan lewat
            // model binding grid biasa.
            if (!string.IsNullOrEmpty(itemQuantitiesJson))
            {
                model.ListItem_ = JsonConvert.DeserializeObject<List<ProductionTaskDetailItemModel>>(itemQuantitiesJson);
            }

            productionActivityService = new ProductionActivityService();
            productionActivityService.FinishActivity(userId, model);

            // kembalikan form utama yang sudah ter-refresh (dipakai RefreshAfterSuccess di JS)
            var refreshed = productionActivityService.GetById(userId, model.Id??0 );
            return PartialView(VIEW_FORM_PARTIAL, refreshed);
        }

    }
}