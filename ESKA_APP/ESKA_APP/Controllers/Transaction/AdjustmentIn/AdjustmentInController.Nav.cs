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
    public partial class AdjustmentInController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel AdjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            AdjustmentInModel = adjustmentInService.NavFirst(userId);
            if (AdjustmentInModel != null)
            {
                AdjustmentInModel._FormMode = FormModeEnum.Edit;
            }

            if (AdjustmentInModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            AdjustmentInModel AdjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            AdjustmentInModel = adjustmentInService.NavPrevious(userId, Id);
            if (AdjustmentInModel != null)
            {
                AdjustmentInModel._FormMode = FormModeEnum.Edit;
            }

            if (AdjustmentInModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            AdjustmentInModel AdjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            AdjustmentInModel = adjustmentInService.NavNext(userId, Id);
            if (AdjustmentInModel != null)
            {

                AdjustmentInModel._FormMode = FormModeEnum.Edit;

            }

            if (AdjustmentInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel AdjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            AdjustmentInModel = adjustmentInService.NavLast(userId);
            if (AdjustmentInModel != null)
            {
                AdjustmentInModel._FormMode = FormModeEnum.Edit;
            }

            if (AdjustmentInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentInModel);
        }



    }
}