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
    public partial class ChangeItemController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/ChangeItem_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            changeItemService = new ChangeItemService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = changeItemService.ChangeItem_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}