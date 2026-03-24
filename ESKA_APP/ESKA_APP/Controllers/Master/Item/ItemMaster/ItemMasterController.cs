using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Item;

namespace Controllers.Master.Item
{
    public partial class ItemMasterController : BaseController
    {
        string VIEW_DETAIL = "ItemMaster";
        string VIEW_FORM_PARTIAL = "Partial/ItemMaster_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ItemMaster_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ItemMaster_Panel_List_Partial";


        ItemMasterService ItemMasterService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(string itemCode)
        {
            int userId = (int)Session["userId"];


            ItemMasterService = new ItemMasterService();
            ItemMasterModel ItemMasterModel;
            if (string.IsNullOrWhiteSpace(itemCode) )
            {
                ViewBag.initNew = true;
                ItemMasterModel = ItemMasterService.GetNewModel(userId);
                ItemMasterModel._FormMode = FormModeEnum.New;
            }
            else
            {
                ItemMasterService = new ItemMasterService();
                ItemMasterModel = ItemMasterService.GetByItemCode(userId, itemCode);
                ItemMasterModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, ItemMasterModel);
        }

        public ActionResult DetailPartial(string itemCode)
        {
            int userId = (int)Session["userId"];


            ItemMasterModel ItemMasterModel;

            ItemMasterService = new ItemMasterService();
            if (string.IsNullOrWhiteSpace(itemCode))
            {
                ItemMasterModel = ItemMasterService.GetNewModel(userId);
                ItemMasterModel._FormMode = FormModeEnum.New;
            }
            else
            {
                ItemMasterModel = ItemMasterService.GetByItemCode(userId, itemCode);
                if (ItemMasterModel != null)
                {
                    ItemMasterModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    ItemMasterModel = ItemMasterService.GetNewModel(userId);
                    ItemMasterModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, ItemMasterModel);
        }
    }
}