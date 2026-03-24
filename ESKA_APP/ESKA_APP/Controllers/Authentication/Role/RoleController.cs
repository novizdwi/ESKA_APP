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
        string VIEW_DETAIL = "Role";
        string VIEW_FORM_PARTIAL = "Partial/Role_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Role_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Role_Panel_List_Partial";

        RoleService roleService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public static void CreateTreeViewNodesRecursive(List<Role_AuthModel> items, MVCxTreeViewNodeCollection nodesCollection, string parentCode = "")
        {
            if (items != null)
            {
                List<Role_AuthModel> items_ = items.Where(x => x.ParentCode == parentCode).ToList();

                foreach (Role_AuthModel item in items_)
                {
                    MVCxTreeViewNode node = nodesCollection.Add(item.MenuName, item.MenuCode);

                    if (item.IsAccess == "Y")
                    {
                        node.Checked = true;
                    }
                    else
                    {
                        node.Checked = false;
                    }


                    CreateTreeViewNodesRecursive(items, node.Nodes, item.MenuCode);

                    if (!string.IsNullOrEmpty(item.Url))
                    {
                        node.NodeStyle.Paddings.PaddingLeft = System.Web.UI.WebControls.Unit.Pixel(40);
                    }
                }
            }

        }

        public ActionResult Detail(int Id = 0)
        {

            roleService = new RoleService();
            RoleModel roleModel;
            if (Id == 0)
            {

                roleModel = roleService.GetNewModel();
                roleModel._FormMode = FormModeEnum.New;
            }
            else
            {
                roleService = new RoleService();
                roleModel = roleService.GetById(Id);
                if (roleModel != null)
                {
                    roleModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, roleModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            RoleModel roleModel;

            roleService = new RoleService();

            if (Id == 0)
            {
                roleModel = roleService.GetNewModel();
                roleModel._FormMode = FormModeEnum.New;
            }
            else
            {
                roleModel = roleService.GetById(Id);
                if (roleModel != null)
                {
                    roleModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView("Partial/Role_Form_Partial", roleModel);
        }


        private void fillCheckValue(RoleModel roleModel)
        {
            //key nya bukan id menu , akan tetapi key nya adalah dari generator devexpress

            //name/id
            //----------------
            var chkNames = roleModel.ChkNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> chkNamesD = new Dictionary<string, string>();
            foreach (var chkName in chkNames)
            {
                var temp = chkName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                chkNamesD.Add(temp[0], temp[1]);
            }


            //value/check or uncheck
            //-----------------
            var chkValues = roleModel.ChkValues.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> chkValuesD = new Dictionary<string, string>();

            foreach (var chkValue in chkValues)
            {
                var temp = chkValue.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                chkValuesD.Add(temp[0], temp[1]);
            }

            //mapping
            //---------------
            List<Role_AuthModel> role_Auths = new List<Role_AuthModel>();
            foreach (KeyValuePair<string, string> entry in chkNamesD)
            {
                if (chkValuesD.ContainsKey(entry.Key))
                {
                    Role_AuthModel item = new Role_AuthModel();
                    item.MenuCode = entry.Value;
                    if (chkValuesD[entry.Key] == "C")
                    {
                        item.IsAccess = "Y";
                    }
                    else
                    {
                        item.IsAccess = "N";
                    }

                    role_Auths.Add(item);
                }
            }

            roleModel.role_Auths = role_Auths;
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  RoleModel roleModel)
        {

            roleModel._UserId = (int)Session["userId"];

            roleService = new RoleService();

            fillCheckValue(roleModel);

            if (ModelState.IsValid)
            {
                roleService.Add(roleModel);
                roleModel = roleService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }


            roleModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  RoleModel roleModel)
        {

            roleModel._UserId = (int)Session["userId"];

            roleService = new RoleService();
            roleModel._FormMode = FormModeEnum.Edit;

            fillCheckValue(roleModel);

            if (ModelState.IsValid)
            {
                roleService.Update(roleModel);
                roleModel = roleService.GetById(roleModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            roleService = new RoleService();

            if (Id != 0)
            {
                roleService = new RoleService();
                roleService.DeleteById(Id);

            }

            RoleModel roleModel = new RoleModel();
            return PartialView(VIEW_FORM_PARTIAL, roleModel);
        }

    }
}