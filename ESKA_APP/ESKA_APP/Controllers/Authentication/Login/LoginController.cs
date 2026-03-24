using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

using System.Net;

using System.Web.Mvc.Razor;
using Models.Authentication.Login;
using Models._Utils;

namespace Controllers.Authentication
{
    public partial class LoginController : Controller
    {
        string VIEW_DETAIL = "Login";
        string VIEW_FORM_PARTIAL = "Partial/Login_Form_Partial";
        string VIEW_PANEL_PARTIAL = "Partial/Login_Panel_Partial";

        LoginService loginService;

        public ActionResult Index()
        {
            if (Session["userId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Detail");
        }
        public ActionResult Detail(int UserId = 0)
        {
            if (Session["userId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            LoginModel loginModel = new LoginModel();
            return View(VIEW_DETAIL, loginModel);
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //System.Diagnostics.Debug.Print("test OnActionExecuted");
            if (filterContext.Exception != null)
            {

                if (filterContext.Exception.Message.Substring(0, 12) == "[VALIDATION]")
                {
                    var content = filterContext.Exception.Message;

                    filterContext.Result = new ContentResult

                    {
                        ContentType = "text/plain",//Thanks Colin  
                        Content = content
                    };

                    filterContext.HttpContext.Response.Status =

                       "500 " + filterContext.Exception.Message

                       .Replace("\r", " ")

                       .Replace("\n", " ");

                    filterContext.ExceptionHandled = true;

                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }

            }

        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Login(string UserName, string Pwd)
        {
            var loginModel = new LoginModel();
            loginModel.UserName = UserName;
            loginModel.Pwd = Pwd;

            loginService = new LoginService();

            string lastUrl = "";

            if (!loginService.Login(loginModel))
            {
                throw new Exception(string.Format("[VALIDATION] {0}", "Login fail"));
            }
            else
            {

                var model = loginService.GetLoginInfo(loginModel.UserName);


                Session["userId"] = model.UserId;

                Session["userName"] = model.UserName;

                Session["roleName"] = model.RoleName;

                Session["isAdmin"] = GeneralGetList.GetIsAdmin(model.RoleName);

                Session["branchCode"] = model.WhsCode;

                Session["branchName"] = model.WhsName;




                if (!string.IsNullOrEmpty(model.LastController))
                {
                    if (GeneralGetList.GetAuthAction((int)Session["userId"], model.LastController + "/" + "Detail"))
                    {
                        lastUrl = Url.Action("Detail", model.LastController);
                    }
                }

                if (string.IsNullOrEmpty(lastUrl))
                {
                    lastUrl = Url.Action("Index", "Home");
                }
            }

            return Content(lastUrl);
        }

        public ActionResult PopupLoadOnDemandPartial()
        {
            LoginModel loginModel = new LoginModel();
            return PartialView(VIEW_PANEL_PARTIAL, loginModel);
        }
    }
}