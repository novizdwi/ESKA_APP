using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction.Adjustment;

namespace Controllers.Transaction.Adjustment
{
    public partial class AdjustmentInController : BaseController
    {

        string VIEW_TAB_CONTENT = "Partial/AdjustmentIn_Form_TabDetail_List_Partial";
        
        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            adjustmentInService = new AdjustmentInService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<AdjustmentIn_ItemModel> modelList = adjustmentInService.GetAdjustmentIn_Items(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

      



    }
}