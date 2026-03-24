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
    public partial class TransferRequestController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TransferRequestModel transferRequestModel;
            transferRequestService = new TransferRequestService();

            transferRequestModel = transferRequestService.NavFirst(userId);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }

            if (transferRequestModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TransferRequestModel transferRequestModel;
            transferRequestService = new TransferRequestService();

            transferRequestModel = transferRequestService.NavPrevious(userId, Id);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }

            if (transferRequestModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TransferRequestModel transferRequestModel;
            transferRequestService = new TransferRequestService();

            transferRequestModel = transferRequestService.NavNext(userId, Id);
            if (transferRequestModel != null)
            {

                transferRequestModel._FormMode = FormModeEnum.Edit;

            }

            if (transferRequestModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TransferRequestModel transferRequestModel;
            transferRequestService = new TransferRequestService();

            transferRequestModel = transferRequestService.NavLast(userId);
            if (transferRequestModel != null)
            {
                transferRequestModel._FormMode = FormModeEnum.Edit;
            }

            if (transferRequestModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferRequestModel);
        }



    }
}