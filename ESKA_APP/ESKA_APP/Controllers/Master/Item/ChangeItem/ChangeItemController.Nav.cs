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
    public partial class ChangeItemController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ChangeItemModel changeItemModel;
            changeItemService = new ChangeItemService();

            changeItemModel = changeItemService.NavFirst(userId);
            if (changeItemModel != null)
            {
                changeItemModel._FormMode = FormModeEnum.Edit;
            }

            if (changeItemModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ChangeItemModel changeItemModel;
            changeItemService = new ChangeItemService();

            changeItemModel = changeItemService.NavPrevious(userId, Id);
            if (changeItemModel != null)
            {
                changeItemModel._FormMode = FormModeEnum.Edit;
            }

            if (changeItemModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            ChangeItemModel changeItemModel;
            changeItemService = new ChangeItemService();

            changeItemModel = changeItemService.NavNext(userId, Id);
            if (changeItemModel != null)
            {

                changeItemModel._FormMode = FormModeEnum.Edit;

            }

            if (changeItemModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ChangeItemModel changeItemModel;
            changeItemService = new ChangeItemService();

            changeItemModel = changeItemService.NavLast(userId);
            if (changeItemModel != null)
            {
                changeItemModel._FormMode = FormModeEnum.Edit;
            }

            if (changeItemModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, changeItemModel);
        }



    }
}