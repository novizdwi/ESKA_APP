using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading; 

using System.Net;

using System.Web.Mvc.Razor;


using Models.Authentication.ChangePassword;

namespace Controllers.Authentication
{
   
    public partial class ChangePasswordController : BaseController
    {

        string VIEW_DETAIL = "ChangePassword";
        string VIEW_FORM_PARTIAL = "Partial/ChangePassword_Form_Partial"; 

        ChangePasswordService changePasswordService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int UserId =0)
        {
          

            ChangePasswordModel changePasswordModel=new ChangePasswordModel(); 

            return View(VIEW_DETAIL, changePasswordModel);
        }

 


        [HttpPost, ValidateInput(false)]
        public ActionResult ChangePassword([ModelBinder(typeof(DevExpressEditorsBinder))]  ChangePasswordModel changePasswordModel)
        {

            if (ModelState.IsValid)
            {
                changePasswordService = new ChangePasswordService();
                changePasswordModel.UserId = (int)Session["userId"];
                changePasswordService.ChangePassword(changePasswordModel);
                changePasswordModel = new ChangePasswordModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 
            

            return PartialView(VIEW_FORM_PARTIAL, changePasswordModel);
        }

        

    }
}