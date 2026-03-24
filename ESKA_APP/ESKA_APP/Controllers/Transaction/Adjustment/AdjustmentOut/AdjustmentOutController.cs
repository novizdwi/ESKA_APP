using Models;
using Models.Transaction.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction.Adjustment;

namespace Controllers.Transaction.Adjustment
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


            AdjustmentOutModel adjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            if (Id == 0)
            {
                adjustmentOutModel = adjustmentOutService.GetNewModel(userId);
                adjustmentOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
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
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentOutModel adjustmentOutModel)
        {
            int userId = (int)Session["userId"];

            adjustmentOutModel._UserId = (int)Session["userId"];
            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            adjustmentOutService.Update(adjustmentOutModel);
            adjustmentOutModel = adjustmentOutService.GetById(userId, adjustmentOutModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentOutModel adjustmentOutModel)
        {
            int userId = (int)Session["userId"];

            adjustmentOutModel._UserId = (int)Session["userId"];
            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutModel._FormMode = FormModeEnum.Edit;
            
            adjustmentOutService.Post(userId, adjustmentOutModel);
            adjustmentOutModel = adjustmentOutService.GetById(userId, adjustmentOutModel.Id);

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
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel adjustmentOutModel;

            adjustmentOutService = new AdjustmentOutService();
            adjustmentOutService.Cancel(userId, Id, CancelReason);

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
    }
}