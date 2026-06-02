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
using Models.Production;

namespace Controllers.Production
{
    public partial class ProcessCardController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/ProcessCard_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            processCardService = new ProcessCardService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = processCardService.ProcessCard_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}