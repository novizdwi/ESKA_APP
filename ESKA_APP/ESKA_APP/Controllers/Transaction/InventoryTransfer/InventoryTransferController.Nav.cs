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
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class InventoryTransferController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel InventoryTransferModel;
            inventoryTransferService = new InventoryTransferService();

            InventoryTransferModel = inventoryTransferService.NavFirst(userId);
            if (InventoryTransferModel != null)
            {
                InventoryTransferModel._FormMode = FormModeEnum.Edit;
            }

            if (InventoryTransferModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            InventoryTransferModel InventoryTransferModel;
            inventoryTransferService = new InventoryTransferService();

            InventoryTransferModel = inventoryTransferService.NavPrevious(userId, Id);
            if (InventoryTransferModel != null)
            {
                InventoryTransferModel._FormMode = FormModeEnum.Edit;
            }

            if (InventoryTransferModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            InventoryTransferModel InventoryTransferModel;
            inventoryTransferService = new InventoryTransferService();

            InventoryTransferModel = inventoryTransferService.NavNext(userId, Id);
            if (InventoryTransferModel != null)
            {

                InventoryTransferModel._FormMode = FormModeEnum.Edit;

            }

            if (InventoryTransferModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel InventoryTransferModel;
            inventoryTransferService = new InventoryTransferService();

            InventoryTransferModel = inventoryTransferService.NavLast(userId);
            if (InventoryTransferModel != null)
            {
                InventoryTransferModel._FormMode = FormModeEnum.Edit;
            }

            if (InventoryTransferModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }



    }
}