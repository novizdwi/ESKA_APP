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
    public partial class AdjustmentOutController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel AdjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            AdjustmentOutModel = adjustmentOutService.NavFirst(userId);
            if (AdjustmentOutModel != null)
            {
                AdjustmentOutModel._FormMode = FormModeEnum.Edit;
            }

            if (AdjustmentOutModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            AdjustmentOutModel AdjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            AdjustmentOutModel = adjustmentOutService.NavPrevious(userId, Id);
            if (AdjustmentOutModel != null)
            {
                AdjustmentOutModel._FormMode = FormModeEnum.Edit;
            }

            if (AdjustmentOutModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            AdjustmentOutModel AdjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            AdjustmentOutModel = adjustmentOutService.NavNext(userId, Id);
            if (AdjustmentOutModel != null)
            {

                AdjustmentOutModel._FormMode = FormModeEnum.Edit;

            }

            if (AdjustmentOutModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel AdjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            AdjustmentOutModel = adjustmentOutService.NavLast(userId);
            if (AdjustmentOutModel != null)
            {
                AdjustmentOutModel._FormMode = FormModeEnum.Edit;
            }

            if (AdjustmentOutModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, AdjustmentOutModel);
        }



    }
}