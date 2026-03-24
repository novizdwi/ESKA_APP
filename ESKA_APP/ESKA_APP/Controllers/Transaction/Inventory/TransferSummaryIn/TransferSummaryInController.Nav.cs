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
    public partial class TransferSummaryInController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TransferSummaryInModel transferSummaryInModel;
            transferSummaryInService = new TransferSummaryInService();

            transferSummaryInModel = transferSummaryInService.NavFirst(userId);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }

            if (transferSummaryInModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TransferSummaryInModel transferSummaryInModel;
            transferSummaryInService = new TransferSummaryInService();

            transferSummaryInModel = transferSummaryInService.NavPrevious(userId, Id);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }

            if (transferSummaryInModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TransferSummaryInModel transferSummaryInModel;
            transferSummaryInService = new TransferSummaryInService();

            transferSummaryInModel = transferSummaryInService.NavNext(userId, Id);
            if (transferSummaryInModel != null)
            {

                transferSummaryInModel._FormMode = FormModeEnum.Edit;

            }

            if (transferSummaryInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TransferSummaryInModel transferSummaryInModel;
            transferSummaryInService = new TransferSummaryInService();

            transferSummaryInModel = transferSummaryInService.NavLast(userId);
            if (transferSummaryInModel != null)
            {
                transferSummaryInModel._FormMode = FormModeEnum.Edit;
            }

            if (transferSummaryInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryInModel);
        }



    }
}