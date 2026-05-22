using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;


using System.Net;

using Models;
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class GoodsReceiptPoController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPoModel goodsReceiptPoModel;
            goodsReceiptPoService = new GoodsReceiptPoService();

            goodsReceiptPoModel = goodsReceiptPoService.NavFirst(userId);
            if (goodsReceiptPoModel != null)
            {
                goodsReceiptPoModel._FormMode = FormModeEnum.Edit;
            }

            if (goodsReceiptPoModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPoModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            GoodsReceiptPoModel goodsReceiptPoModel;
            goodsReceiptPoService = new GoodsReceiptPoService();

            goodsReceiptPoModel = goodsReceiptPoService.NavPrevious(userId, Id);
            if (goodsReceiptPoModel != null)
            {
                goodsReceiptPoModel._FormMode = FormModeEnum.Edit;
            }

            if (goodsReceiptPoModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPoModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];


            GoodsReceiptPoModel goodsReceiptPoModel;
            goodsReceiptPoService = new GoodsReceiptPoService();

            goodsReceiptPoModel = goodsReceiptPoService.NavNext(userId, Id);
            if (goodsReceiptPoModel != null)
            {

                goodsReceiptPoModel._FormMode = FormModeEnum.Edit;

            }

            if (goodsReceiptPoModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPoModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPoModel goodsReceiptPoModel;
            goodsReceiptPoService = new GoodsReceiptPoService();

            goodsReceiptPoModel = goodsReceiptPoService.NavLast(userId);
            if (goodsReceiptPoModel != null)
            {
                goodsReceiptPoModel._FormMode = FormModeEnum.Edit;
            }

            if (goodsReceiptPoModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPoModel);
        }

    }
}