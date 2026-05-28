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

        string VIEW_TAB_CONTENT = "Partial/GoodsReceiptPo_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            goodsReceiptPoService = new GoodsReceiptPoService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<GoodsReceiptPoItem> modelList = goodsReceiptPoService.GoodsReceiptPo_Details(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }


    }
}