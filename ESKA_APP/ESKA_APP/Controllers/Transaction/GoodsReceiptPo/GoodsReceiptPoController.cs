using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class GoodsReceiptPoController : BaseController
    {
        string VIEW_DETAIL = "GoodsReceiptPo";
        string VIEW_FORM_PARTIAL = "Partial/GoodsReceiptPo_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/GoodsReceiptPo_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/GoodsReceiptPo_Panel_List_Partial";


        GoodsReceiptPoService goodsReceiptPoService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            goodsReceiptPoService = new GoodsReceiptPoService();
            GoodsReceiptPoModel goodsReceiptPoModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                goodsReceiptPoModel = goodsReceiptPoService.GetNewModel(userId);
                goodsReceiptPoModel._FormMode = FormModeEnum.New;
            }
            else
            {
                goodsReceiptPoService = new GoodsReceiptPoService();
                goodsReceiptPoModel = goodsReceiptPoService.GetById(userId, Id);
                goodsReceiptPoModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, goodsReceiptPoModel);
        }

        //public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        //{
        //    int userId = (int)Session["userId"];


        //    GoodsReceiptPoModel GoodsReceiptPoModel;

        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    if (Id == 0)
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //    }
        //    else
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, Id);
        //        if (GoodsReceiptPoModel != null)
        //        {
        //            GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
        //        }
        //        else
        //        {
        //            GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //            GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //        }
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodsReceiptPoModel GoodsReceiptPoModel)
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPoModel._UserId = (int)Session["userId"];
            goodsReceiptPoService = new GoodsReceiptPoService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = goodsReceiptPoService.Add(GoodsReceiptPoModel);
                GoodsReceiptPoModel = goodsReceiptPoService.GetById(userId, Id);
                GoodsReceiptPoModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }




            return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodsReceiptPoModel GoodsReceiptPoModel)
        //{
        //    int userId = (int)Session["userId"];

        //    GoodsReceiptPoModel._UserId = (int)Session["userId"];
        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;



        //    //if (ModelState.IsValid)
        //    //{
        //    GoodsReceiptPoService.Update(GoodsReceiptPoModel);
        //    GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, GoodsReceiptPoModel.Id);
        //    //}
        //    //else
        //    //{
        //    //    string message = GetErrorModel();

        //    //    throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    //}

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodsReceiptPoModel GoodsReceiptPoModel)
        //{
        //    int userId = (int)Session["userId"];

        //    GoodsReceiptPoModel._UserId = (int)Session["userId"];
        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;

        //    GoodsReceiptPoService.Post(userId, GoodsReceiptPoModel);
        //    GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, GoodsReceiptPoModel.Id);

        //    if (GoodsReceiptPoModel != null)
        //    {
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Cancel(long Id, string CancelReason = "")
        //{
        //    int userId = (int)Session["userId"];

        //    GoodsReceiptPoModel GoodsReceiptPoModel;

        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    GoodsReceiptPoService.Cancel(userId, Id, CancelReason);

        //    GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, Id);
        //    if (GoodsReceiptPoModel != null)
        //    {
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        //{
        //    int userId = (int)Session["userId"];

        //    GoodsReceiptPoModel GoodsReceiptPoModel;

        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    GoodsReceiptPoService.RequestApproval(userId, id, templateId, approvalMessage);

        //    GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, id);
        //    if (GoodsReceiptPoModel != null)
        //    {
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Approve(long Id, string ApprovalMessage = "")
        //{
        //    int userId = (int)Session["userId"];

        //    GoodsReceiptPoModel GoodsReceiptPoModel;

        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    GoodsReceiptPoService.Approve(userId, Id, ApprovalMessage);

        //    GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, Id);
        //    if (GoodsReceiptPoModel != null)
        //    {
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Reject(long Id, string ApprovalMessage = "")
        //{
        //    int userId = (int)Session["userId"];

        //    GoodsReceiptPoModel GoodsReceiptPoModel;

        //    GoodsReceiptPoService = new GoodsReceiptPoService();
        //    GoodsReceiptPoService.Authorize(userId, Id, "Reject", ApprovalMessage);

        //    GoodsReceiptPoModel = GoodsReceiptPoService.GetById(userId, Id);
        //    if (GoodsReceiptPoModel != null)
        //    {
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        GoodsReceiptPoModel = GoodsReceiptPoService.GetNewModel(userId);
        //        GoodsReceiptPoModel._FormMode = FormModeEnum.New;
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        //}

    }
}