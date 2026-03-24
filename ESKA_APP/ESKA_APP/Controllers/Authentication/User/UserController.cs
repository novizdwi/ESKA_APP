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

        string VIEW_DETAIL = "User";
        string VIEW_FORM_PARTIAL = "Partial/User_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/User_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/User_Panel_List_Partial";

        IMasterService<UserModel, int> userService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(int Id = 0)
        {

            userService = new UserService();
            UserModel userModel;
            if (Id == 0)
            {

                userModel = userService.GetNewModel();
                userModel._FormMode = FormModeEnum.New;
            }
            else
            {
                userService = new UserService();
                userModel = userService.GetById(Id);
                if (userModel != null)
                {
                    userModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, userModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            UserModel userModel;

            userService = new UserService();

            if (Id == 0)
            {
                userModel = userService.GetNewModel();
                userModel._FormMode = FormModeEnum.New;
            }
            else
            {
                userModel = userService.GetById(Id);
                if (userModel != null)
                {
                    userModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  UserModel userModel)
        {

            userModel._UserId = (int)Session["userId"];

            userService = new UserService();


            if (ModelState.IsValid)
            {
                userService.Add(userModel);
                userModel = userService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }


            userModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  UserModel userModel)
        {

            userModel._UserId = (int)Session["userId"];

            userService = new UserService();
            userModel._FormMode = FormModeEnum.Edit;



            if (ModelState.IsValid)
            {
                userService.Update(userModel);
                userModel = userService.GetById(userModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            userService = new UserService();

            if (Id != 0)
            {
                userService = new UserService();
                userService.DeleteById(Id);

            }

            UserModel userModel = new UserModel();
            return PartialView(VIEW_FORM_PARTIAL, userModel);
        }

    }
}