using Models;
using Models.Transaction.Adjustment;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Adjustment
{
    public partial class AdjustmentInController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/ItemTag/ItemTag_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/ItemTag/ItemTag_Form_Partial";
        string VIEW_ITEMTAG_CONTENT = "Partial/ItemTag/ViewApproval_TabContent_List_Partial";

        public ActionResult ViewItemTag_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];

            adjustmentInService = new AdjustmentInService();
            var model = new AdjustmentInItemTagView___();
            if(id != 0 && detId != 0)
            {
                model = adjustmentInService.GetItemTags(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }

        public ActionResult ViewItemTagListPartial()
        {
            int userId = (int)Session["userId"];

            adjustmentInService = new AdjustmentInService();

            var Id = Convert.ToInt64(Request["cbId"]);
            var DetId = Convert.ToInt64(Request["cbDetId"]);

            List<AdjustmentIn_Item_TagModel> modelList = adjustmentInService.GetItemTagDetail(Id, DetId);

            return PartialView(VIEW_APPROVAL_CONTENT, modelList);
        }

        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new AdjustmentInModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }
    }

}