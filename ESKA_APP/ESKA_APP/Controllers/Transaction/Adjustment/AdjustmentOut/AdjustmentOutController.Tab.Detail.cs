using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction.Adjustment;

namespace Controllers.Transaction.Adjustment
{
    public partial class AdjustmentOutController : BaseController
    {

        string VIEW_TAB_CONTENT = "Partial/AdjustmentOut_Form_TabDetail_List_Partial";
        
        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            adjustmentOutService = new AdjustmentOutService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<AdjustmentOut_ItemModel> modelList = adjustmentOutService.GetAdjustmentOut_Items(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

      



    }
}