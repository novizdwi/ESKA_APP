using Models;
using Models.Transaction.StockOpname;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.StockOpname
{
    public partial class RequestStockOpnameController : BaseController
    {
        string VIEW_DETAIL = "RequestStockOpname";
        string VIEW_FORM_PARTIAL = "Partial/RequestStockOpname_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/RequestStockOpname_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/RequestStockOpname_Panel_List_Partial";


        RequestStockOpnameService requestStockOpnameService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            requestStockOpnameService = new RequestStockOpnameService();
            RequestStockOpnameModel requestStockOpnameModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                requestStockOpnameModel = requestStockOpnameService.GetNewModel(userId);
                requestStockOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                requestStockOpnameService = new RequestStockOpnameService();
                requestStockOpnameModel = requestStockOpnameService.GetById(userId, Id);
                requestStockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, requestStockOpnameModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            RequestStockOpnameModel requestStockOpnameModel;

            requestStockOpnameService = new RequestStockOpnameService();
            if (Id == 0)
            {
                requestStockOpnameModel = requestStockOpnameService.GetNewModel(userId);
                requestStockOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                requestStockOpnameModel = requestStockOpnameService.GetById(userId, Id);
                if (requestStockOpnameModel != null)
                {
                    requestStockOpnameModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    requestStockOpnameModel = requestStockOpnameService.GetNewModel(userId);
                    requestStockOpnameModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  RequestStockOpnameModel requestStockOpnameModel)
        {
            int userId = (int)Session["userId"];

            requestStockOpnameModel._UserId = (int)Session["userId"];
            requestStockOpnameService = new RequestStockOpnameService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = requestStockOpnameService.Add(requestStockOpnameModel);
                requestStockOpnameModel = requestStockOpnameService.GetById(userId, Id);
                requestStockOpnameModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  RequestStockOpnameModel requestStockOpnameModel)
        {
            int userId = (int)Session["userId"];

            requestStockOpnameModel._UserId = (int)Session["userId"];
            requestStockOpnameService = new RequestStockOpnameService();
            requestStockOpnameModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            requestStockOpnameService.Update(requestStockOpnameModel);
            requestStockOpnameModel = requestStockOpnameService.GetById(userId, requestStockOpnameModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  RequestStockOpnameModel requestStockOpnameModel)
        {
            int userId = (int)Session["userId"];

            requestStockOpnameModel._UserId = (int)Session["userId"];
            requestStockOpnameService = new RequestStockOpnameService();
            requestStockOpnameModel._FormMode = FormModeEnum.Edit;

            requestStockOpnameService.Update(requestStockOpnameModel);
            //requestStockOpnameService.PostAPI(userId, requestStockOpnameModel.Id);
            requestStockOpnameService.Post(userId, requestStockOpnameModel.Id);
            requestStockOpnameModel = requestStockOpnameService.GetById(userId, requestStockOpnameModel.Id);

            if (requestStockOpnameModel != null)
            {
                requestStockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                requestStockOpnameModel = requestStockOpnameService.GetNewModel(userId);
                requestStockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            RequestStockOpnameModel requestStockOpnameModel;

            requestStockOpnameService = new RequestStockOpnameService();
            requestStockOpnameService.Cancel(userId, Id, CancelReason);

            requestStockOpnameModel = requestStockOpnameService.GetById(userId, Id);
            if (requestStockOpnameModel != null)
            {
                requestStockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                requestStockOpnameModel = requestStockOpnameService.GetNewModel(userId);
                requestStockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, requestStockOpnameModel);
        }
        
    }
}