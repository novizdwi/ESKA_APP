using Models;
using Models.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferOutController : BaseController
    {
        string VIEW_DETAIL = "TransferOut";
        string VIEW_FORM_PARTIAL = "Partial/TransferOut_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TransferOut_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TransferOut_Panel_List_Partial";


        TransferOutService transferOutService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            transferOutService = new TransferOutService();
            TransferOutModel transferOutModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                transferOutModel = transferOutService.GetNewModel(userId);
                transferOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferOutService = new TransferOutService();
                transferOutModel = transferOutService.GetById(userId, Id);
                transferOutModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, transferOutModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            TransferOutModel transferOutModel;

            transferOutService = new TransferOutService();
            if (Id == 0)
            {
                transferOutModel = transferOutService.GetNewModel(userId);
                transferOutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                transferOutModel = transferOutService.GetById(userId, Id);
                if (transferOutModel != null)
                {
                    transferOutModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    transferOutModel = transferOutService.GetNewModel(userId);
                    transferOutModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferOutModel transferOutModel)
        //{
        //    int userId = (int)Session["userId"];

        //    transferOutModel._UserId = (int)Session["userId"];
        //    transferOutService = new TransferOutService();

        //    if (ModelState.IsValid)
        //    {
        //        long Id = 0;

        //        Id = transferOutService.Add(transferOutModel);
        //        transferOutModel = transferOutService.GetById(userId, Id);
        //        transferOutModel._FormMode = Models.FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();
        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferOutModel transferOutModel)
        {
            int userId = (int)Session["userId"];

            transferOutModel._UserId = (int)Session["userId"];
            transferOutService = new TransferOutService();
            transferOutModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            transferOutService.Update(transferOutModel);
            transferOutModel = transferOutService.GetById(userId, transferOutModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  TransferOutModel transferOutModel)
        {
            int userId = (int)Session["userId"];

            transferOutModel._UserId = (int)Session["userId"];
            transferOutService = new TransferOutService();
            transferOutModel._FormMode = FormModeEnum.Edit;

            transferOutService.Update(transferOutModel);
            //transferOutService.PostAPI(userId, transferOutModel.Id);
            transferOutService.Post(userId, transferOutModel.Id);
            transferOutModel = transferOutService.GetById(userId, transferOutModel.Id);

            if (transferOutModel != null)
            {
                transferOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferOutModel = transferOutService.GetNewModel(userId);
                transferOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            TransferOutModel transferOutModel;

            transferOutService = new TransferOutService();
            transferOutService.Cancel(userId, Id, CancelReason);

            transferOutModel = transferOutService.GetById(userId, Id);
            if (transferOutModel != null)
            {
                transferOutModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                transferOutModel = transferOutService.GetNewModel(userId);
                transferOutModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }
        
    }
}