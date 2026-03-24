using Models;
using Models.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferSummaryInController : BaseController
    {
        string VIEW_DETAIL = "TransferSummaryIn";
        string VIEW_FORM_PARTIAL = "Partial/TransferSummaryIn_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TransferSummaryIn_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TransferSummaryIn_Panel_List_Partial";


        TransferSummaryInService transferSummaryInService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            transferSummaryInService = new TransferSummaryInService();
            TransferSummaryInModel transferSummaryInModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferSummaryInService = new TransferSummaryInService();
                transferSummaryInModel = transferSummaryInService.GetById(userId, Id);
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, transferSummaryInModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            TransferSummaryInModel transferSummaryInModel;

            transferSummaryInService = new TransferSummaryInService();
            if (Id == 0)
            {
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferSummaryInModel = transferSummaryInService.GetById(userId, Id);
                if (transferSummaryInModel != null)
                {
                    transferSummaryInModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                    transferSummaryInModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferSummaryInModel transferSummaryInModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryInModel._UserId = (int)Session["userId"];
            transferSummaryInService = new TransferSummaryInService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = transferSummaryInService.Add(transferSummaryInModel);
                transferSummaryInModel = transferSummaryInService.GetById(userId, Id);
                transferSummaryInModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferSummaryInModel transferSummaryInModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryInModel._UserId = (int)Session["userId"];
            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            transferSummaryInService.Update(transferSummaryInModel);
            transferSummaryInModel = transferSummaryInService.GetById(userId, transferSummaryInModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferSummaryInModel transferSummaryInModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryInModel._UserId = (int)Session["userId"];
            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInModel._FormMode = FormModeEnum.Edit;
            
            transferSummaryInService.Post(userId, transferSummaryInModel);
            transferSummaryInModel = transferSummaryInService.GetById(userId, transferSummaryInModel.Id);

            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryInModel transferSummaryInModel;

            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInService.Cancel(userId, Id, CancelReason);

            transferSummaryInModel = transferSummaryInService.GetById(userId, Id);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryInModel transferSummaryInModel;

            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInService.RequestApproval(userId, id, templateId, approvalMessage);

            transferSummaryInModel = transferSummaryInService.GetById(userId, id);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryInModel transferSummaryInModel;

            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInService.Approve(userId, Id, ApprovalMessage);

            transferSummaryInModel = transferSummaryInService.GetById(userId, Id);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryInModel transferSummaryInModel;

            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInService.Authorize(userId, Id, "Reject", ApprovalMessage);

            transferSummaryInModel = transferSummaryInService.GetById(userId, Id);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryInModel = transferSummaryInService.GetNewModel(userId);
                transferSummaryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        public ActionResult RefreshItem(TransferSummaryInModel transferSummaryInModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryInService = new TransferSummaryInService();
            transferSummaryInModel._FormMode = FormModeEnum.Edit;

            transferSummaryInService.RefreshItem(userId, transferSummaryInModel.Id);
            transferSummaryInModel = transferSummaryInService.GetById(userId, transferSummaryInModel.Id);

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);

        }
    }
}