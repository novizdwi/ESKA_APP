using Models;
using Models.Transaction.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class DeactiveTagController : BaseController
    {
        string VIEW_DETAIL = "DeactiveTag";
        string VIEW_FORM_PARTIAL = "Partial/DeactiveTag_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/DeactiveTag_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/DeactiveTag_Panel_List_Partial";


        DeactiveTagService deactiveTagService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            deactiveTagService = new DeactiveTagService();
            DeactiveTagModel deactiveTagModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }
            else
            {
                deactiveTagService = new DeactiveTagService();
                deactiveTagModel = deactiveTagService.GetById(userId, Id);
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, deactiveTagModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            DeactiveTagModel deactiveTagModel;

            deactiveTagService = new DeactiveTagService();
            if (Id == 0)
            {
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }
            else
            {
                deactiveTagModel = deactiveTagService.GetById(userId, Id);
                if (deactiveTagModel != null)
                {
                    deactiveTagModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    deactiveTagModel = deactiveTagService.GetNewModel(userId);
                    deactiveTagModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  DeactiveTagModel DeactiveTagModel)
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel._UserId = (int)Session["userId"];
            deactiveTagService = new DeactiveTagService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = deactiveTagService.Add(DeactiveTagModel);
                DeactiveTagModel = deactiveTagService.GetById(userId, Id);
                DeactiveTagModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, DeactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  DeactiveTagModel deactiveTagModel)
        {
            int userId = (int)Session["userId"];

            deactiveTagModel._UserId = (int)Session["userId"];
            deactiveTagService = new DeactiveTagService();
            deactiveTagModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            deactiveTagService.Update(deactiveTagModel);
            deactiveTagModel = deactiveTagService.GetById(userId, deactiveTagModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  DeactiveTagModel deactiveTagModel)
        {
            int userId = (int)Session["userId"];

            deactiveTagModel._UserId = (int)Session["userId"];
            deactiveTagService = new DeactiveTagService();
            deactiveTagModel._FormMode = FormModeEnum.Edit;

            //deactiveTagService.Update(deactiveTagModel);
            //deactiveTagService.PostAPI(userId, deactiveTagModel.Id);
            deactiveTagService.Post(userId, deactiveTagModel.Id);
            deactiveTagModel = deactiveTagService.GetById(userId, deactiveTagModel.Id);

            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel deactiveTagModel;

            deactiveTagService = new DeactiveTagService();
            deactiveTagService.Cancel(userId, Id, CancelReason);

            deactiveTagModel = deactiveTagService.GetById(userId, Id);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data)
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();
            var result = deactiveTagService.ChooseItem(userId, Id, Data);


            return Content(result.ToString());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel deactiveTagModel;

            deactiveTagService = new DeactiveTagService();
            deactiveTagService.RequestApproval(userId, id, templateId, approvalMessage);

            deactiveTagModel = deactiveTagService.GetById(userId, id);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel deactiveTagModel;

            deactiveTagService = new DeactiveTagService();
            deactiveTagService.Approve(userId, Id, ApprovalMessage);

            deactiveTagModel = deactiveTagService.GetById(userId, Id);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel deactiveTagModel;

            deactiveTagService = new DeactiveTagService();
            deactiveTagService.Authorize(userId, Id, "Reject", ApprovalMessage);

            deactiveTagModel = deactiveTagService.GetById(userId, Id);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                deactiveTagModel = deactiveTagService.GetNewModel(userId);
                deactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }


    }
}