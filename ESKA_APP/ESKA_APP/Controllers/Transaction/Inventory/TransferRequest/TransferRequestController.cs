using Models;
using Models.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferRequestController : BaseController
    {
        string VIEW_DETAIL = "TransferRequest";
        string VIEW_FORM_PARTIAL = "Partial/TransferRequest_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TransferRequest_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TransferRequest_Panel_List_Partial";


        TransferRequestService transferRequestService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            transferRequestService = new TransferRequestService();
            TransferRequestModel transferRequestModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferRequestService = new TransferRequestService();
                transferRequestModel = transferRequestService.GetById(userId, Id);
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, transferRequestModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            TransferRequestModel transferRequestModel;

            transferRequestService = new TransferRequestService();
            if (Id == 0)
            {
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferRequestModel = transferRequestService.GetById(userId, Id);
                if (transferRequestModel != null)
                {
                    transferRequestModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    transferRequestModel = transferRequestService.GetNewModel(userId);
                    transferRequestModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferRequestModel transferRequestModel)
        {
            int userId = (int)Session["userId"];

            transferRequestModel._UserId = (int)Session["userId"];
            transferRequestService = new TransferRequestService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = transferRequestService.Add(transferRequestModel);
                transferRequestModel = transferRequestService.GetById(userId, Id);
                transferRequestModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferRequestModel transferRequestModel)
        {
            int userId = (int)Session["userId"];

            transferRequestModel._UserId = (int)Session["userId"];
            transferRequestService = new TransferRequestService();
            transferRequestModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            transferRequestService.Update(transferRequestModel);
            transferRequestModel = transferRequestService.GetById(userId, transferRequestModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferRequestModel transferRequestModel)
        {
            int userId = (int)Session["userId"];

            transferRequestModel._UserId = (int)Session["userId"];
            transferRequestService = new TransferRequestService();
            transferRequestModel._FormMode = FormModeEnum.Edit;

            transferRequestService.Update(transferRequestModel);
            transferRequestService.Post(userId, transferRequestModel.Id);
            transferRequestModel = transferRequestService.GetById(userId, transferRequestModel.Id);

            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferRequestModel transferRequestModel;

            transferRequestService = new TransferRequestService();
            transferRequestService.RequestApproval(userId, id, templateId, approvalMessage);

            transferRequestModel = transferRequestService.GetById(userId, id);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferRequestModel transferRequestModel;

            transferRequestService = new TransferRequestService();
            transferRequestService.Approve(userId, Id, ApprovalMessage);

            transferRequestModel = transferRequestService.GetById(userId, Id);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            TransferRequestModel transferRequestModel;

            transferRequestService = new TransferRequestService();
            transferRequestService.Authorize(userId, Id, "Reject", ApprovalMessage);

            transferRequestModel = transferRequestService.GetById(userId, Id);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            TransferRequestModel transferRequestModel;

            transferRequestService = new TransferRequestService();
            transferRequestService.Cancel(userId, Id, CancelReason);

            transferRequestModel = transferRequestService.GetById(userId, Id);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferRequestModel = transferRequestService.GetNewModel(userId);
                transferRequestModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data)
        {
            int userId = (int)Session["userId"];

            transferRequestService = new TransferRequestService();
            var result = transferRequestService.ChooseItem(userId, Id, Data);


            return Content(result.ToString());
        }

    }
}