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
using Models.Master.Item;

namespace Controllers.Master.Item
{
    public partial class ItemMasterController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ItemMasterModel ItemMasterModel;
            ItemMasterService = new ItemMasterService();

            ItemMasterModel = ItemMasterService.NavFirst(userId);
            if (ItemMasterModel != null)
            {
                ItemMasterModel._FormMode = FormModeEnum.Edit;
            }

            if (ItemMasterModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ItemMasterModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(string itemCode)
        {
            int userId = (int)Session["userId"];


            ItemMasterModel ItemMasterModel;
            ItemMasterService = new ItemMasterService();

            ItemMasterModel = ItemMasterService.NavPrevious(userId, itemCode);
            if (ItemMasterModel != null)
            {
                ItemMasterModel._FormMode = FormModeEnum.Edit;
            }

            if (ItemMasterModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ItemMasterModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(string itemCode)
        {
            int userId = (int)Session["userId"];

            ItemMasterModel ItemMasterModel;
            ItemMasterService = new ItemMasterService();

            ItemMasterModel = ItemMasterService.NavNext(userId, itemCode);
            if (ItemMasterModel != null)
            {

                ItemMasterModel._FormMode = FormModeEnum.Edit;

            }

            if (ItemMasterModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ItemMasterModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ItemMasterModel ItemMasterModel;
            ItemMasterService = new ItemMasterService();

            ItemMasterModel = ItemMasterService.NavLast(userId);
            if (ItemMasterModel != null)
            {
                ItemMasterModel._FormMode = FormModeEnum.Edit;
            }

            if (ItemMasterModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ItemMasterModel);
        }



    }
}