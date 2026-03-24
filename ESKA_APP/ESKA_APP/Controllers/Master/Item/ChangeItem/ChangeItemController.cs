using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Item;

namespace Controllers.Master.Item
{
    public partial class ChangeItemController : BaseController
    {
        string VIEW_DETAIL = "ChangeItem";
        string VIEW_FORM_PARTIAL = "Partial/ChangeItem_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ChangeItem_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ChangeItem_Panel_List_Partial";


        ChangeItemService changeItemService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            changeItemService = new ChangeItemService();
            ChangeItemModel changeItemModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                changeItemModel = changeItemService.GetNewModel(userId);
                changeItemModel._FormMode = FormModeEnum.New;
            }
            else
            {
                changeItemService = new ChangeItemService();
                changeItemModel = changeItemService.GetById(userId, Id);
                changeItemModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, changeItemModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            ChangeItemModel changeItemModel;

            changeItemService = new ChangeItemService();
            if (Id == 0)
            {
                changeItemModel = changeItemService.GetNewModel(userId);
                changeItemModel._FormMode = FormModeEnum.New;
            }
            else
            {
                changeItemModel = changeItemService.GetById(userId, Id);
                if (changeItemModel != null)
                {
                    changeItemModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    changeItemModel = changeItemService.GetNewModel(userId);
                    changeItemModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ChangeItemModel changeItemModel)
        {
            int userId = (int)Session["userId"];

            changeItemModel._UserId = (int)Session["userId"];
            changeItemService = new ChangeItemService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = changeItemService.Add(changeItemModel);
                changeItemModel = changeItemService.GetById(userId, Id);
                changeItemModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ChangeItemModel changeItemModel)
        {
            int userId = (int)Session["userId"];

            changeItemModel._UserId = (int)Session["userId"];
            changeItemService = new ChangeItemService();
            changeItemModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            changeItemService.Update(changeItemModel);
            changeItemModel = changeItemService.GetById(userId, changeItemModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  ChangeItemModel changeItemModel)
        {
            int userId = (int)Session["userId"];

            changeItemModel._UserId = (int)Session["userId"];
            changeItemService = new ChangeItemService();
            changeItemModel._FormMode = FormModeEnum.Edit;

            changeItemService.Update(changeItemModel);
            //changeItemService.PostAPI(userId, changeItemModel.Id);
            changeItemService.Post(userId, changeItemModel.Id);
            changeItemModel = changeItemService.GetById(userId, changeItemModel.Id);

            if (changeItemModel != null)
            {
                changeItemModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                changeItemModel = changeItemService.GetNewModel(userId);
                changeItemModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            ChangeItemModel changeItemModel;

            changeItemService = new ChangeItemService();
            changeItemService.Cancel(userId, Id, CancelReason);

            changeItemModel = changeItemService.GetById(userId, Id);
            if (changeItemModel != null)
            {
                changeItemModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                changeItemModel = changeItemService.GetNewModel(userId);
                changeItemModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data)
        {
            int userId = (int)Session["userId"];

            changeItemService = new ChangeItemService();
            var result = changeItemService.ChooseItem(userId, Id, Data);


            return Content(result.ToString());
        }

    }
}