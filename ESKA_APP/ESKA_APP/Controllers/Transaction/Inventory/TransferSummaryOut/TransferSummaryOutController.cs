using Models;
using Models.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferSummaryOutController : BaseController
    {
        string VIEW_DETAIL = "TransferSummaryOut";
        string VIEW_FORM_PARTIAL = "Partial/TransferSummaryOut_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TransferSummaryOut_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TransferSummaryOut_Panel_List_Partial";


        TransferSummaryOutService transferSummaryOutService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            transferSummaryOutService = new TransferSummaryOutService();
            TransferSummaryOutModel transferSummaryOutModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferSummaryOutService = new TransferSummaryOutService();
                transferSummaryOutModel = transferSummaryOutService.GetById(userId, Id);
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, transferSummaryOutModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            TransferSummaryOutModel transferSummaryOutModel;

            transferSummaryOutService = new TransferSummaryOutService();
            if (Id == 0)
            {
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferSummaryOutModel = transferSummaryOutService.GetById(userId, Id);
                if (transferSummaryOutModel != null)
                {
                    transferSummaryOutModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                    transferSummaryOutModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferSummaryOutModel transferSummaryOutModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutModel._UserId = (int)Session["userId"];
            transferSummaryOutService = new TransferSummaryOutService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = transferSummaryOutService.Add(transferSummaryOutModel);
                transferSummaryOutModel = transferSummaryOutService.GetById(userId, Id);
                transferSummaryOutModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferSummaryOutModel transferSummaryOutModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutModel._UserId = (int)Session["userId"];
            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            transferSummaryOutService.Update(transferSummaryOutModel);
            transferSummaryOutModel = transferSummaryOutService.GetById(userId, transferSummaryOutModel.Id);
            transferSummaryOutModel._FormMode = Models.FormModeEnum.Edit;
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferSummaryOutModel transferSummaryOutModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutModel._UserId = (int)Session["userId"];
            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            
            transferSummaryOutService.Post(userId, transferSummaryOutModel);
            transferSummaryOutModel = transferSummaryOutService.GetById(userId, transferSummaryOutModel.Id);

            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryOutModel transferSummaryOutModel;

            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutService.Cancel(userId, Id, CancelReason);

            transferSummaryOutModel = transferSummaryOutService.GetById(userId, Id);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryOutModel transferSummaryOutModel;

            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutService.RequestApproval(userId, id, templateId, approvalMessage);

            transferSummaryOutModel = transferSummaryOutService.GetById(userId, id);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryOutModel transferSummaryOutModel;

            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutService.Approve(userId, Id, ApprovalMessage);

            transferSummaryOutModel = transferSummaryOutService.GetById(userId, Id);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferSummaryOutModel transferSummaryOutModel;

            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutService.Authorize(userId, Id, "Reject", ApprovalMessage);

            transferSummaryOutModel = transferSummaryOutService.GetById(userId, Id);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferSummaryOutModel = transferSummaryOutService.GetNewModel(userId);
                transferSummaryOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        public ActionResult RefreshItem(TransferSummaryOutModel transferSummaryOutModel)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutService = new TransferSummaryOutService();
            transferSummaryOutModel._FormMode = FormModeEnum.Edit;

            transferSummaryOutService.RefreshItem(userId, transferSummaryOutModel.Id);
            transferSummaryOutModel = transferSummaryOutService.GetById(userId, transferSummaryOutModel.Id);

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);

        }
    }
}