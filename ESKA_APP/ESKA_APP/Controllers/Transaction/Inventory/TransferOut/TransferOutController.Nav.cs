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
using Models.Transaction.Inventory;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferOutController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TransferOutModel transferOutModel;
            transferOutService = new TransferOutService();

            transferOutModel = transferOutService.NavFirst(userId);
            if (transferOutModel != null)
            {
                transferOutModel._FormMode = FormModeEnum.Edit;
            }

            if (transferOutModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TransferOutModel transferOutModel;
            transferOutService = new TransferOutService();

            transferOutModel = transferOutService.NavPrevious(userId, Id);
            if (transferOutModel != null)
            {
                transferOutModel._FormMode = FormModeEnum.Edit;
            }

            if (transferOutModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TransferOutModel transferOutModel;
            transferOutService = new TransferOutService();

            transferOutModel = transferOutService.NavNext(userId, Id);
            if (transferOutModel != null)
            {

                transferOutModel._FormMode = FormModeEnum.Edit;

            }

            if (transferOutModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TransferOutModel transferOutModel;
            transferOutService = new TransferOutService();

            transferOutModel = transferOutService.NavLast(userId);
            if (transferOutModel != null)
            {
                transferOutModel._FormMode = FormModeEnum.Edit;
            }

            if (transferOutModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferOutModel);
        }



    }
}