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

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            GoodsReceiptPoModel GoodsReceiptPoModel;

            goodsReceiptPoService = new GoodsReceiptPoService();
            if (Id == 0)
            {
                GoodsReceiptPoModel = goodsReceiptPoService.GetNewModel(userId);
                GoodsReceiptPoModel._FormMode = FormModeEnum.New;
            }
            else
            {
                GoodsReceiptPoModel = goodsReceiptPoService.GetById(userId, Id);
                if (GoodsReceiptPoModel != null)
                {
                    GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    GoodsReceiptPoModel = goodsReceiptPoService.GetNewModel(userId);
                    GoodsReceiptPoModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        }

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


        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))] GoodsReceiptPoModel GoodsReceiptPoModel)
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPoModel._UserId = (int)Session["userId"];
            goodsReceiptPoService = new GoodsReceiptPoService();
            GoodsReceiptPoModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            goodsReceiptPoService.Update(GoodsReceiptPoModel);
            GoodsReceiptPoModel = goodsReceiptPoService.GetById(userId, GoodsReceiptPoModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, GoodsReceiptPoModel);
        }

    }
}