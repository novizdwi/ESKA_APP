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
    public partial class AdjustmentOutController : BaseController
    {
        string VIEW_DETAIL = "AdjustmentOut";
        string VIEW_FORM_PARTIAL = "Partial/AdjustmentOut_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/AdjustmentOut_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/AdjustmentOut_Panel_List_Partial";


        AdjustmentOutService adjustmentOutService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            adjustmentOutService = new AdjustmentOutService();
            AdjustmentOutModel adjustmentOutModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                adjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                adjustmentOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                adjustmentOutService = new AdjustmentOutService();
                adjustmentOutModel = adjustmentOutService.GetById(userId, Id);
                adjustmentOutModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, adjustmentOutModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            AdjustmentOutModel AdjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            if (Id == 0)
            {
                AdjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                AdjustmentOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                AdjustmentOutModel = adjustmentOutService.GetById(userId, Id);
                if (AdjustmentOutModel != null)
                {
                    AdjustmentOutModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    AdjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                    AdjustmentOutModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentOutModel AdjustmentOutModel)
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel._UserId = (int)Session["userId"];
            adjustmentOutService = new AdjustmentOutService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = adjustmentOutService.Add(AdjustmentOutModel);
                AdjustmentOutModel = adjustmentOutService.GetById(userId, Id);
                AdjustmentOutModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentOutModel AdjustmentOutModel)
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel._UserId = (int)Session["userId"];
            adjustmentOutService = new AdjustmentOutService();
            AdjustmentOutModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            adjustmentOutService.Update(AdjustmentOutModel);
            AdjustmentOutModel = adjustmentOutService.GetById(userId, AdjustmentOutModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data, string Sorting)
        {
            int userId = (int)Session["userId"];

            adjustmentOutService = new AdjustmentOutService();
            var result = adjustmentOutService.ChooseItem(userId, Id, Data, Sorting);

            return Content(result.ToString());
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentOutModel AdjustmentOutModel)
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel._UserId = (int)Session["userId"];
            adjustmentOutService = new AdjustmentOutService();
            AdjustmentOutModel._FormMode = FormModeEnum.Edit;
            
            adjustmentOutService.Post(userId, AdjustmentOutModel);
            AdjustmentOutModel = adjustmentOutService.GetById(userId, AdjustmentOutModel.Id);

            if (AdjustmentOutModel != null)
            {
                AdjustmentOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                AdjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                AdjustmentOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel AdjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutService.Cancel(userId, Id, CancelReason);

            AdjustmentOutModel = adjustmentOutService.GetById(userId, Id);
            if (AdjustmentOutModel != null)
            {
                AdjustmentOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                AdjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                AdjustmentOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel adjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutService.RequestApproval(userId, id, templateId, approvalMessage);

            adjustmentOutModel = adjustmentOutService.GetById(userId, id);
            if (adjustmentOutModel != null)
            {
                adjustmentOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                adjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                adjustmentOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel adjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutService.Approve(userId, Id, ApprovalMessage);

            adjustmentOutModel = adjustmentOutService.GetById(userId, Id);
            if (adjustmentOutModel != null)
            {
                adjustmentOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                adjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                adjustmentOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel adjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutService.Authorize(userId, Id, "Reject", ApprovalMessage);

            adjustmentOutModel = adjustmentOutService.GetById(userId, Id);
            if (adjustmentOutModel != null)
            {
                adjustmentOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                adjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                adjustmentOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }

        [HttpPost]
        public ActionResult BatchSaveAndAddRow(string rows, long Id, long detId)
        {
            List<AdjustmentOutBatchModel> batchList = new List<AdjustmentOutBatchModel>();
            if (!string.IsNullOrEmpty(rows))
            {
                batchList = JsonConvert.DeserializeObject<List<AdjustmentOutBatchModel>>(rows);
            }
            
            batchList.Add(new AdjustmentOutBatchModel()
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