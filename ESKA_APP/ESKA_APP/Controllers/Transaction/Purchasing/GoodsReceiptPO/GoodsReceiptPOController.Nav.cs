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
using Models.Transaction.Purchasing;

namespace Controllers.Transaction.Purchasing
{
    public partial class GoodsReceiptPOController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPOModel goodsReceiptPOModel;
            goodsReceiptPOService = new GoodsReceiptPOService();

            goodsReceiptPOModel = goodsReceiptPOService.NavFirst(userId);
            if (goodsReceiptPOModel != null)
            {
                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
            }

            if (goodsReceiptPOModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            GoodsReceiptPOModel goodsReceiptPOModel;
            goodsReceiptPOService = new GoodsReceiptPOService();

            goodsReceiptPOModel = goodsReceiptPOService.NavPrevious(userId, Id);
            if (goodsReceiptPOModel != null)
            {
                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
            }

            if (goodsReceiptPOModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            GoodsReceiptPOModel goodsReceiptPOModel;
            goodsReceiptPOService = new GoodsReceiptPOService();

            goodsReceiptPOModel = goodsReceiptPOService.NavNext(userId, Id);
            if (goodsReceiptPOModel != null)
            {

                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;

            }

            if (goodsReceiptPOModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPOModel goodsReceiptPOModel;
            goodsReceiptPOService = new GoodsReceiptPOService();

            goodsReceiptPOModel = goodsReceiptPOService.NavLast(userId);
            if (goodsReceiptPOModel != null)
            {
                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
            }

            if (goodsReceiptPOModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }



    }
}