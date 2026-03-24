using Models;
using Models.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferInController : BaseController
    {
        string VIEW_DETAIL = "TransferIn";
        string VIEW_FORM_PARTIAL = "Partial/TransferIn_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TransferIn_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TransferIn_Panel_List_Partial";


        TransferInService transferInService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            transferInService = new TransferInService();
            TransferInModel transferInModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                transferInModel = transferInService.GetNewModel(userId);
                transferInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferInService = new TransferInService();
                transferInModel = transferInService.GetById(userId, Id);
                transferInModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, transferInModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            TransferInModel transferInModel;

            transferInService = new TransferInService();
            if (Id == 0)
            {
                transferInModel = transferInService.GetNewModel(userId);
                transferInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferInModel = transferInService.GetById(userId, Id);
                if (transferInModel != null)
                {
                    transferInModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    transferInModel = transferInService.GetNewModel(userId);
                    transferInModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferInModel transferInModel)
        //{
        //    int userId = (int)Session["userId"];

        //    transferInModel._UserId = (int)Session["userId"];
        //    transferInService = new TransferInService();

        //    if (ModelState.IsValid)
        //    {
        //        long Id = 0;

        //        Id = transferInService.Add(transferInModel);
        //        transferInModel = transferInService.GetById(userId, Id);
        //        transferInModel._FormMode = Models.FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();
        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferInModel transferInModel)
        {
            int userId = (int)Session["userId"];

            transferInModel._UserId = (int)Session["userId"];
            transferInService = new TransferInService();
            transferInModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            transferInService.Update(transferInModel);
            transferInModel = transferInService.GetById(userId, transferInModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferInModel transferInModel)
        {
            int userId = (int)Session["userId"];

            transferInModel._UserId = (int)Session["userId"];
            transferInService = new TransferInService();
            transferInModel._FormMode = FormModeEnum.Edit;

            transferInService.Update(transferInModel);
            //transferInService.PostAPI(userId, transferInModel.Id);
            transferInService.Post(userId, transferInModel.Id);
            transferInModel = transferInService.GetById(userId, transferInModel.Id);

            if (transferInModel != null)
            {
                transferInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferInModel = transferInService.GetNewModel(userId);
                transferInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            TransferInModel transferInModel;

            transferInService = new TransferInService();
            transferInService.Cancel(userId, Id, CancelReason);

            transferInModel = transferInService.GetById(userId, Id);
            if (transferInModel != null)
            {
                transferInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferInModel = transferInService.GetNewModel(userId);
                transferInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }
        
    }
}