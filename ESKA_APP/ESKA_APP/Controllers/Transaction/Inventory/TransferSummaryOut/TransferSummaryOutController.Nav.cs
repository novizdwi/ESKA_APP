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
    public partial class TransferSummaryOutController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TransferSummaryOutModel transferSummaryOutModel;
            transferSummaryOutService = new TransferSummaryOutService();

            transferSummaryOutModel = transferSummaryOutService.NavFirst(userId);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }

            if (transferSummaryOutModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TransferSummaryOutModel transferSummaryOutModel;
            transferSummaryOutService = new TransferSummaryOutService();

            transferSummaryOutModel = transferSummaryOutService.NavPrevious(userId, Id);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }

            if (transferSummaryOutModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TransferSummaryOutModel transferSummaryOutModel;
            transferSummaryOutService = new TransferSummaryOutService();

            transferSummaryOutModel = transferSummaryOutService.NavNext(userId, Id);
            if (transferSummaryOutModel != null)
            {

                transferSummaryOutModel._FormMode = FormModeEnum.Edit;

            }

            if (transferSummaryOutModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TransferSummaryOutModel transferSummaryOutModel;
            transferSummaryOutService = new TransferSummaryOutService();

            transferSummaryOutModel = transferSummaryOutService.NavLast(userId);
            if (transferSummaryOutModel != null)
            {
                transferSummaryOutModel._FormMode = FormModeEnum.Edit;
            }

            if (transferSummaryOutModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, transferSummaryOutModel);
        }



    }
}