using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;  

using System.Net;

using Models;
using Models.Authentication.User;

namespace Controllers.Authentication
{
    public partial class UserController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            UserModel userModel;
            userService = new UserService();

            userModel = userService.NavFirst();
            if (userModel != null)
            {
                userModel._FormMode = FormModeEnum.Edit;
            }

            if (userModel == null)
            {
                //userModel = userService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            UserModel userModel;
            userService = new UserService();

            userModel = userService.NavPrevious(Id);
            if (userModel != null)
            {
                userModel._FormMode = FormModeEnum.Edit;
            }

            if (userModel == null)
            {
                //userModel = userService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            UserModel userModel;
            userService = new UserService();

            userModel = userService.NavNext(Id);
            if (userModel != null)
            {

                userModel._FormMode = FormModeEnum.Edit;

            }

            if (userModel == null)
            {
                //userModel = userService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            UserModel userModel;
            userService = new UserService();

            userModel = userService.NavLast();
            if (userModel != null)
            {
                userModel._FormMode = FormModeEnum.Edit; 
            }

            if (userModel == null)
            {
                //userModel = userService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }



    }
}