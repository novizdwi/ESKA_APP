using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Production;

namespace Controllers.Production
{
    public partial class ProcessCardController : BaseController
    {
        string VIEW_DETAIL = "ProcessCard";
        string VIEW_FORM_PARTIAL = "Partial/ProcessCard_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ProcessCard_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ProcessCard_Panel_List_Partial";


        ProcessCardService processCardService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            processCardService = new ProcessCardService();
            ProcessCardModel processCardModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                processCardModel = processCardService.GetNewModel(userId);
                processCardModel._FormMode = FormModeEnum.New;
            }
            else
            {
                processCardService = new ProcessCardService();
                processCardModel = processCardService.GetById(userId, Id);
                processCardModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, processCardModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            ProcessCardModel ProcessCardModel;

            processCardService = new ProcessCardService();
            if (Id == 0)
            {
                ProcessCardModel = processCardService.GetNewModel(userId);
                ProcessCardModel._FormMode = FormModeEnum.New;
            }
            else
            {
                ProcessCardModel = processCardService.GetById(userId, Id);
                if (ProcessCardModel != null)
                {
                    ProcessCardModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    ProcessCardModel = processCardService.GetNewModel(userId);
                    ProcessCardModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ProcessCardModel ProcessCardModel)
        {
            int userId = (int)Session["userId"];

            ProcessCardModel._UserId = (int)Session["userId"];
            processCardService = new ProcessCardService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = processCardService.Add(ProcessCardModel);
                ProcessCardModel = processCardService.GetById(userId, Id);
                ProcessCardModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ProcessCardModel ProcessCardModel)
        {
            int userId = (int)Session["userId"];

            ProcessCardModel._UserId = (int)Session["userId"];
            processCardService = new ProcessCardService();
            ProcessCardModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            processCardService.Update(ProcessCardModel);
            ProcessCardModel = processCardService.GetById(userId, ProcessCardModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data, string Sorting)
        {
            int userId = (int)Session["userId"];

            processCardService = new ProcessCardService();
            var result = processCardService.ChooseItem(userId, Id, Data, Sorting);

            return Content(result.ToString());
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  ProcessCardModel ProcessCardModel)
        {
            int userId = (int)Session["userId"];

            ProcessCardModel._UserId = (int)Session["userId"];
            processCardService = new ProcessCardService();
            ProcessCardModel._FormMode = FormModeEnum.Edit;
            
            processCardService.Post(userId, ProcessCardModel);
            ProcessCardModel = processCardService.GetById(userId, ProcessCardModel.Id);

            if (ProcessCardModel != null)
            {
                ProcessCardModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                ProcessCardModel = processCardService.GetNewModel(userId);
                ProcessCardModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            ProcessCardModel ProcessCardModel;

            processCardService = new ProcessCardService();
            processCardService.Cancel(userId, Id, CancelReason);

            ProcessCardModel = processCardService.GetById(userId, Id);
            if (ProcessCardModel != null)
            {
                ProcessCardModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                ProcessCardModel = processCardService.GetNewModel(userId);
                ProcessCardModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            ProcessCardModel processCardModel;

            processCardService = new ProcessCardService();
            processCardService.RequestApproval(userId, id, templateId, approvalMessage);

            processCardModel = processCardService.GetById(userId, id);
            if (processCardModel != null)
            {
                processCardModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                processCardModel = processCardService.GetNewModel(userId);
                processCardModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, processCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            ProcessCardModel processCardModel;

            processCardService = new ProcessCardService();
            processCardService.Approve(userId, Id, ApprovalMessage);

            processCardModel = processCardService.GetById(userId, Id);
            if (processCardModel != null)
            {
                processCardModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                processCardModel = processCardService.GetNewModel(userId);
                processCardModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, processCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            ProcessCardModel processCardModel;

            processCardService = new ProcessCardService();
            processCardService.Authorize(userId, Id, "Reject", ApprovalMessage);

            processCardModel = processCardService.GetById(userId, Id);
            if (processCardModel != null)
            {
                processCardModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                processCardModel = processCardService.GetNewModel(userId);
                processCardModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, processCardModel);
        }

        public ActionResult GetOperators(string RoutingCode)
        {
            var data = Models._Utils.GeneralGetList.GetUserRouting(RoutingCode);

            return PartialView("_OperatorList", data);
        }

        [HttpPost]
        public JsonResult GetOperatorByRouting( string routingCode )
        {
            var list = Models._Utils.GeneralGetList .GetUserRoutingList(routingCode);
            return Json(list);
        }
    }
}