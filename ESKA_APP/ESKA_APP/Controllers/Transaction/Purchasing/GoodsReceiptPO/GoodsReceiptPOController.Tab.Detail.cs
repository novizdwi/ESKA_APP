using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction.Purchasing;

namespace Controllers.Transaction.Purchasing
{
    public partial class GoodsReceiptPOController : BaseController
    {

        string VIEW_TAB_CONTENT = "Partial/GoodsReceiptPO_Form_TabDetail_List_Partial";
        
        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            goodsReceiptPOService = new GoodsReceiptPOService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<GoodsReceiptPO_DetailModel> modelList = goodsReceiptPOService.GoodsReceiptPO_Details(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

      



    }
}