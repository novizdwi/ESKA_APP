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
using Models;
using Models.Authentication.User;

namespace Controllers.Authentication
{
    public partial class UserController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/User_TabRouting_List_Partial";

        public ActionResult TabRoutingListPartial()
        {
            int userId = (int)Session["userId"];

            userService = new UserService();

            var Id = Convert.ToInt32(Request["cbId"]);


            List<User_RoutingModel> modelListDetail = userService.GetRoutingById(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}