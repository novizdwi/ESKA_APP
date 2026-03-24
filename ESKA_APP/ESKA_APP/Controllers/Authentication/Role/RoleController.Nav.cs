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
using Models.Authentication.Role;



namespace Controllers.Authentication
{
    public partial class RoleController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            RoleModel roleModel;
            roleService = new RoleService();

            roleModel = roleService.NavFirst();
            if (roleModel != null)
            {
                roleModel._FormMode = FormModeEnum.Edit;


            }

            if (roleModel == null)
            {
                //roleModel = roleService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            RoleModel roleModel;
            roleService = new RoleService();

            roleModel = roleService.NavPrevious(Id);
            if (roleModel != null)
            {

                roleModel._FormMode = FormModeEnum.Edit;

            }

            if (roleModel == null)
            {
                //roleModel = roleService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            RoleModel roleModel;
            roleService = new RoleService();

            roleModel = roleService.NavNext(Id);
            if (roleModel != null)
            {

                roleModel._FormMode = FormModeEnum.Edit;

            }

            if (roleModel == null)
            {
                //roleModel = roleService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            RoleModel roleModel;
            roleService = new RoleService();

            roleModel = roleService.NavLast();
            if (roleModel != null)
            {

                roleModel._FormMode = FormModeEnum.Edit;

            }

            if (roleModel == null)
            {
                //roleModel = roleService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }



    }
}