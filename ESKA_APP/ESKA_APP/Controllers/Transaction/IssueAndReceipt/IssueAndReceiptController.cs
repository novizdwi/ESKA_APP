using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction;
using System.Linq;
using Newtonsoft.Json;

namespace Controllers.Transaction
{
    public partial class IssueAndReceiptController : BaseController
    {
        string VIEW_DETAIL = "IssueAndReceipt";
        string VIEW_FORM_PARTIAL = "Partial/IssueAndReceipt_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/IssueAndReceipt_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/IssueAndReceipt_Panel_List_Partial";


        IssueAndReceiptService issueAndReceiptService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            issueAndReceiptService = new IssueAndReceiptService();
            IssueAndReceiptModel IssueAndReceiptModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                IssueAndReceiptModel = issueAndReceiptService.GetNewModel(userId);
                IssueAndReceiptModel._FormMode = FormModeEnum.New;
            }
            else
            {
                issueAndReceiptService = new IssueAndReceiptService();
                IssueAndReceiptModel = issueAndReceiptService.GetById(userId, Id);
                IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, IssueAndReceiptModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            IssueAndReceiptModel IssueAndReceiptModel;

            issueAndReceiptService = new IssueAndReceiptService();
            if (Id == 0)
            {
                IssueAndReceiptModel = issueAndReceiptService.GetNewModel(userId);
                IssueAndReceiptModel._FormMode = FormModeEnum.New;
            }
            else
            {
                IssueAndReceiptModel = issueAndReceiptService.GetById(userId, Id);
                if (IssueAndReceiptModel != null)
                {
                    IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    IssueAndReceiptModel = issueAndReceiptService.GetNewModel(userId);
                    IssueAndReceiptModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))] IssueAndReceiptModel IssueAndReceiptModel)
        {
            int userId = (int)Session["userId"];

            IssueAndReceiptModel._UserId = userId;
            issueAndReceiptService = new IssueAndReceiptService();

            if (ModelState.IsValid)
            {
                long Id = issueAndReceiptService.Add(IssueAndReceiptModel);
                IssueAndReceiptModel = issueAndReceiptService.GetById(userId, Id);
                IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))] IssueAndReceiptModel IssueAndReceiptModel)
        {
            int userId = (int)Session["userId"];

            IssueAndReceiptModel._UserId = userId;
            issueAndReceiptService = new IssueAndReceiptService();
            IssueAndReceiptModel._FormMode = FormModeEnum.Edit;

            issueAndReceiptService.Update(IssueAndReceiptModel);
            IssueAndReceiptModel = issueAndReceiptService.GetById(userId, IssueAndReceiptModel.Id);

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))] IssueAndReceiptModel IssueAndReceiptModel)
        //{
        //    int userId = (int)Session["userId"];

        //    IssueAndReceiptModel._UserId = (int)Session["userId"];
        //    IssueAndReceiptService = new IssueAndReceiptService();
        //    IssueAndReceiptModel._FormMode = FormModeEnum.Edit;

        //    IssueAndReceiptService.Post(userId, IssueAndReceiptModel);
        //    IssueAndReceiptModel = IssueAndReceiptService.GetById(userId, IssueAndReceiptModel.Id);

        //    if (IssueAndReceiptModel != null)
        //    {
        //        IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        IssueAndReceiptModel = IssueAndReceiptService.GetNewModel(userId);
        //        IssueAndReceiptModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        //}


        //[HttpPost, ValidateInput(false)]
        //public ActionResult Cancel(long Id, string CancelReason = "")
        //{
        //    int userId = (int)Session["userId"];

        //    IssueAndReceiptModel IssueAndReceiptModel;

        //    IssueAndReceiptService = new IssueAndReceiptService();
        //    IssueAndReceiptService.Cancel(userId, Id, CancelReason);

        //    IssueAndReceiptModel = IssueAndReceiptService.GetById(userId, Id);
        //    if (IssueAndReceiptModel != null)
        //    {
        //        IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        IssueAndReceiptModel = IssueAndReceiptService.GetNewModel(userId);
        //        IssueAndReceiptModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        //}

    }
}