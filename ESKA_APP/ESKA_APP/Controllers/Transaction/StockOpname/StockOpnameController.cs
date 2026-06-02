using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction;
using Newtonsoft.Json;

namespace Controllers.Transaction
{
    public partial class StockOpnameController : BaseController
    {
        string VIEW_DETAIL = "StockOpname";
        string VIEW_FORM_PARTIAL = "Partial/StockOpname_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/StockOpname_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/StockOpname_Panel_List_Partial";


        StockOpnameService stockOpnameService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            stockOpnameService = new StockOpnameService();
            StockOpnameModel stockOpnameModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                stockOpnameModel = stockOpnameService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                stockOpnameService = new StockOpnameService();
                stockOpnameModel = stockOpnameService.GetById(userId, Id);
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, stockOpnameModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            StockOpnameModel StockOpnameModel;

            stockOpnameService = new StockOpnameService();
            if (Id == 0)
            {
                StockOpnameModel = stockOpnameService.GetNewModel(userId);
                StockOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                StockOpnameModel = stockOpnameService.GetById(userId, Id);
                if (StockOpnameModel != null)
                {
                    StockOpnameModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    StockOpnameModel = stockOpnameService.GetNewModel(userId);
                    StockOpnameModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  StockOpnameModel StockOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockOpnameModel._UserId = (int)Session["userId"];
            stockOpnameService = new StockOpnameService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = stockOpnameService.Add(StockOpnameModel);
                StockOpnameModel = stockOpnameService.GetById(userId, Id);
                StockOpnameModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  StockOpnameModel StockOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockOpnameModel._UserId = (int)Session["userId"];
            stockOpnameService = new StockOpnameService();
            StockOpnameModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            stockOpnameService.Update(StockOpnameModel);
            StockOpnameModel = stockOpnameService.GetById(userId, StockOpnameModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data, string Sorting)
        {
            int userId = (int)Session["userId"];

            stockOpnameService = new StockOpnameService();
            var result = stockOpnameService.ChooseItem(userId, Id, Data, Sorting);

            return Content(result.ToString());
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  StockOpnameModel StockOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockOpnameModel._UserId = (int)Session["userId"];
            stockOpnameService = new StockOpnameService();
            StockOpnameModel._FormMode = FormModeEnum.Edit;
            
            stockOpnameService.Post(userId, StockOpnameModel);
            StockOpnameModel = stockOpnameService.GetById(userId, StockOpnameModel.Id);

            if (StockOpnameModel != null)
            {
                StockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                StockOpnameModel = stockOpnameService.GetNewModel(userId);
                StockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            StockOpnameModel StockOpnameModel;

            stockOpnameService = new StockOpnameService();
            stockOpnameService.Cancel(userId, Id, CancelReason);

            StockOpnameModel = stockOpnameService.GetById(userId, Id);
            if (StockOpnameModel != null)
            {
                StockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                StockOpnameModel = stockOpnameService.GetNewModel(userId);
                StockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, StockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            StockOpnameModel stockOpnameModel;

            stockOpnameService = new StockOpnameService();
            stockOpnameService.RequestApproval(userId, id, templateId, approvalMessage);

            stockOpnameModel = stockOpnameService.GetById(userId, id);
            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                stockOpnameModel = stockOpnameService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            StockOpnameModel stockOpnameModel;

            stockOpnameService = new StockOpnameService();
            stockOpnameService.Approve(userId, Id, ApprovalMessage);

            stockOpnameModel = stockOpnameService.GetById(userId, Id);
            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                stockOpnameModel = stockOpnameService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            StockOpnameModel stockOpnameModel;

            stockOpnameService = new StockOpnameService();
            stockOpnameService.Authorize(userId, Id, "Reject", ApprovalMessage);

            stockOpnameModel = stockOpnameService.GetById(userId, Id);
            if (stockOpnameModel != null)
            {
                stockOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                stockOpnameModel = stockOpnameService.GetNewModel(userId);
                stockOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, stockOpnameModel);
        }

        [HttpPost]
        public ActionResult BatchSaveAndAddRow(string rows, long Id, long detId)
        {
            List<StockOpnameBatchModel> batchList = new List<StockOpnameBatchModel>();
            if (!string.IsNullOrEmpty(rows))
            {
                batchList = JsonConvert.DeserializeObject<List<StockOpnameBatchModel>>(rows);
            }
            
            batchList.Add(new StockOpnameBatchModel()
            {
                Id = Id,
                DetId = detId, 
                Batch = "",
                Quantity = 0,
                AdmissionDate = null,
                Netto = 0,
            }); 

            Session["BatchList"] = batchList;
            return Json(new{Result = true, TotalRow = batchList.Count});
        }

    }
}