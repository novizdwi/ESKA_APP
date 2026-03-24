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
    public partial class TransferInController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TransferInModel transferInModel;
            transferInService = new TransferInService();

            transferInModel = transferInService.NavFirst(userId);
            if (transferInModel != null)
            {
                transferInModel._FormMode = FormModeEnum.Edit;
            }

            if (transferInModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TransferInModel transferInModel;
            transferInService = new TransferInService();

            transferInModel = transferInService.NavPrevious(userId, Id);
            if (transferInModel != null)
            {
                transferInModel._FormMode = FormModeEnum.Edit;
            }

            if (transferInModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TransferInModel transferInModel;
            transferInService = new TransferInService();

            transferInModel = transferInService.NavNext(userId, Id);
            if (transferInModel != null)
            {

                transferInModel._FormMode = FormModeEnum.Edit;

            }

            if (transferInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TransferInModel transferInModel;
            transferInService = new TransferInService();

            transferInModel = transferInService.NavLast(userId);
            if (transferInModel != null)
            {
                transferInModel._FormMode = FormModeEnum.Edit;
            }

            if (transferInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferInModel);
        }



    }
}