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

using Models.Master.Item;

namespace Controllers.Master.Item
{
    public partial class ItemMasterController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/ItemMaster_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            ItemMasterService = new ItemMasterService();

            var ItemCode = Convert.ToString(Request["cbId"]);


            var modelListDetail = ItemMasterService.ItemMaster_Details(ItemCode);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}