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
    public partial class AdjustmentInController : BaseController
    {
        string VIEW_DETAIL = "AdjustmentIn";
        string VIEW_FORM_PARTIAL = "Partial/AdjustmentIn_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/AdjustmentIn_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/AdjustmentIn_Panel_List_Partial";


        AdjustmentInService adjustmentInService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            adjustmentInService = new AdjustmentInService();
            AdjustmentInModel adjustmentInModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                adjustmentInModel = adjustmentInService.GetNewModel(userId);
                adjustmentInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                adjustmentInService = new AdjustmentInService();
                adjustmentInModel = adjustmentInService.GetById(userId, Id);
                adjustmentInModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, adjustmentInModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            AdjustmentInModel AdjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            if (Id == 0)
            {
                AdjustmentInModel = adjustmentInService.GetNewModel(userId);
                AdjustmentInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                AdjustmentInModel = adjustmentInService.GetById(userId, Id);
                if (AdjustmentInModel != null)
                {
                    AdjustmentInModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    AdjustmentInModel = adjustmentInService.GetNewModel(userId);
                    AdjustmentInModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentInModel AdjustmentInModel)
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel._UserId = (int)Session["userId"];
            adjustmentInService = new AdjustmentInService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = adjustmentInService.Add(AdjustmentInModel);
                AdjustmentInModel = adjustmentInService.GetById(userId, Id);
                AdjustmentInModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentInModel AdjustmentInModel)
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel._UserId = (int)Session["userId"];
            adjustmentInService = new AdjustmentInService();
            AdjustmentInModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            adjustmentInService.Update(AdjustmentInModel);
            AdjustmentInModel = adjustmentInService.GetById(userId, AdjustmentInModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data, string Sorting)
        {
            int userId = (int)Session["userId"];

            adjustmentInService = new AdjustmentInService();
            var result = adjustmentInService.ChooseItem(userId, Id, Data, Sorting);

            return Content(result.ToString());
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentInModel AdjustmentInModel)
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel._UserId = (int)Session["userId"];
            adjustmentInService = new AdjustmentInService();
            AdjustmentInModel._FormMode = FormModeEnum.Edit;
            
            adjustmentInService.Post(userId, AdjustmentInModel);
            AdjustmentInModel = adjustmentInService.GetById(userId, AdjustmentInModel.Id);

            if (AdjustmentInModel != null)
            {
                AdjustmentInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                AdjustmentInModel = adjustmentInService.GetNewModel(userId);
                AdjustmentInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel AdjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            adjustmentInService.Cancel(userId, Id, CancelReason);

            AdjustmentInModel = adjustmentInService.GetById(userId, Id);
            if (AdjustmentInModel != null)
            {
                AdjustmentInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                AdjustmentInModel = adjustmentInService.GetNewModel(userId);
                AdjustmentInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel adjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            adjustmentInService.RequestApproval(userId, id, templateId, approvalMessage);

            adjustmentInModel = adjustmentInService.GetById(userId, id);
            if (adjustmentInModel != null)
            {
                adjustmentInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                adjustmentInModel = adjustmentInService.GetNewModel(userId);
                adjustmentInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel adjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            adjustmentInService.Approve(userId, Id, ApprovalMessage);

            adjustmentInModel = adjustmentInService.GetById(userId, Id);
            if (adjustmentInModel != null)
            {
                adjustmentInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                adjustmentInModel = adjustmentInService.GetNewModel(userId);
                adjustmentInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel adjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            adjustmentInService.Authorize(userId, Id, "Reject", ApprovalMessage);

            adjustmentInModel = adjustmentInService.GetById(userId, Id);
            if (adjustmentInModel != null)
            {
                adjustmentInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                adjustmentInModel = adjustmentInService.GetNewModel(userId);
                adjustmentInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }

        [HttpPost]
        public ActionResult BatchSaveAndAddRow(string rows, long Id, long detId)
        {
            List<AdjustmentInBatchModel> batchList = new List<AdjustmentInBatchModel>();
            if (!string.IsNullOrEmpty(rows))
            {
                batchList = JsonConvert.DeserializeObject<List<AdjustmentInBatchModel>>(rows);
            }
            
            batchList.Add(new AdjustmentInBatchModel()
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