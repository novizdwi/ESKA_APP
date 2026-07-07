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
    public partial class IssueAndReceiptController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            IssueAndReceiptModel IssueAndReceiptModel;
            issueAndReceiptService = new IssueAndReceiptService();

            IssueAndReceiptModel = issueAndReceiptService.NavFirst(userId);
            if (IssueAndReceiptModel != null)
            {
                IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
            }

            if (IssueAndReceiptModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            IssueAndReceiptModel IssueAndReceiptModel;
            issueAndReceiptService = new IssueAndReceiptService();

            IssueAndReceiptModel = issueAndReceiptService.NavPrevious(userId, Id);
            if (IssueAndReceiptModel != null)
            {
                IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
            }

            if (IssueAndReceiptModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];


            IssueAndReceiptModel IssueAndReceiptModel;
            issueAndReceiptService = new IssueAndReceiptService();

            IssueAndReceiptModel = issueAndReceiptService.NavNext(userId, Id);
            if (IssueAndReceiptModel != null)
            {

                IssueAndReceiptModel._FormMode = FormModeEnum.Edit;

            }

            if (IssueAndReceiptModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            IssueAndReceiptModel IssueAndReceiptModel;
            issueAndReceiptService = new IssueAndReceiptService();

            IssueAndReceiptModel = issueAndReceiptService.NavLast(userId);
            if (IssueAndReceiptModel != null)
            {
                IssueAndReceiptModel._FormMode = FormModeEnum.Edit;
            }

            if (IssueAndReceiptModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, IssueAndReceiptModel);
        }

    }
}