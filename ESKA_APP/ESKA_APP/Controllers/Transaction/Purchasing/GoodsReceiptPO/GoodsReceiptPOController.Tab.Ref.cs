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

using Models.Transaction.Purchasing;

namespace Controllers.Transaction.Purchasing
{
    public partial class GoodsReceiptPOController : BaseController
    {

        string VIEW_TAB_Ref_COMPONENT = "Partial/GoodsReceiptPO_Form_TabRef_Partial";

        public ActionResult TabRefListPartial()
        {
            int userId = (int)Session["userId"];

            goodsReceiptPOService = new GoodsReceiptPOService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListRef = goodsReceiptPOService.GoodsReceiptPO_Refs(Id);

            return PartialView(VIEW_TAB_Ref_COMPONENT, modelListRef);
        }
        

    }
}