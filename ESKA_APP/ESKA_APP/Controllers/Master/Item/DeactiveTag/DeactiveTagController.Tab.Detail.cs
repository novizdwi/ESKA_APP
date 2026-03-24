using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class DeactiveTagController : BaseController
    {

        string VIEW_TAB_CONTENT = "Partial/DeactiveTag_Form_TabDetail_List_Partial";
        
        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<DeactiveTag_ItemModel> modelList = deactiveTagService.GetDeactiveTag_Items(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

      



    }
}