using Models;
using Models.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferSummaryOutController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/ItemTag/ItemTag_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/ItemTag/ItemTag_Form_Partial";
        string VIEW_ITEMTAG_CONTENT = "Partial/ItemTag/ViewApproval_TabContent_List_Partial";

        public ActionResult ViewItemTag_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutService = new TransferSummaryOutService();
            var model = new TransferSummaryOutItemTagView___();
            if(id != 0 && detId != 0)
            {
                model = transferSummaryOutService.GetItemTags(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }


        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new TransferSummaryOutModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }
    }

}