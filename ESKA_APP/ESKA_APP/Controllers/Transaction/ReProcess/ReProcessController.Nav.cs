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
    public partial class ReProcessController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ReProcessModel ReProcessModel;
            reProcessService = new ReProcessService();

            ReProcessModel = reProcessService.NavFirst(userId);
            if (ReProcessModel != null)
            {
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }

            if (ReProcessModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ReProcessModel ReProcessModel;
            reProcessService = new ReProcessService();

            ReProcessModel = reProcessService.NavPrevious(userId, Id);
            if (ReProcessModel != null)
            {
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }

            if (ReProcessModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ReProcessModel ReProcessModel;
            reProcessService = new ReProcessService();

            ReProcessModel = reProcessService.NavNext(userId, Id);
            if (ReProcessModel != null)
            {

                ReProcessModel._FormMode = FormModeEnum.Edit;

            }

            if (ReProcessModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ReProcessModel ReProcessModel;
            reProcessService = new ReProcessService();

            ReProcessModel = reProcessService.NavLast(userId);
            if (ReProcessModel != null)
            {
                ReProcessModel._FormMode = FormModeEnum.Edit;
            }

            if (ReProcessModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ReProcessModel);
        }

    }
}