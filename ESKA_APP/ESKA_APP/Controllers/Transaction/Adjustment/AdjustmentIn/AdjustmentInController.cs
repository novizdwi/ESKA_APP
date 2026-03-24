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


            AdjustmentInModel adjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            if (Id == 0)
            {
                adjustmentInModel = adjustmentInService.GetNewModel(userId);
                adjustmentInModel._FormMode = FormModeEnum.New;
            }
            else
            {
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
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentInModel adjustmentInModel)
        {
            int userId = (int)Session["userId"];

            adjustmentInModel._UserId = (int)Session["userId"];
            adjustmentInService = new AdjustmentInService();
            adjustmentInModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            adjustmentInService.Update(adjustmentInModel);
            adjustmentInModel = adjustmentInService.GetById(userId, adjustmentInModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  AdjustmentInModel adjustmentInModel)
        {
            int userId = (int)Session["userId"];

            adjustmentInModel._UserId = (int)Session["userId"];
            adjustmentInService = new AdjustmentInService();
            adjustmentInModel._FormMode = FormModeEnum.Edit;

            //adjustmentInService.Update(adjustmentInModel);
            //adjustmentInService.PostAPI(userId, adjustmentInModel.Id);
            adjustmentInService.Post(userId, adjustmentInModel);
            adjustmentInModel = adjustmentInService.GetById(userId, adjustmentInModel.Id);

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
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel adjustmentInModel;

            adjustmentInService = new AdjustmentInService();
            adjustmentInService.Cancel(userId, Id, CancelReason);

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

        public ActionResult GetPillars( string PillarsCode)
        {
            var list = Models._Utils.GeneralGetList.GetCostCenterList("1");
            return PartialView("_ComboBoxPillars", list);
        }

    }
}