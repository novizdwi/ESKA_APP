using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models; 

using System.Net;

using Models.Transaction;
using Models._Utils;
using Models.Transaction.Adjustment;

namespace Controllers.Transaction.Adjustment
{
    public partial class AdjustmentOutController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel adjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            adjustmentOutModel = adjustmentOutService.NavFirst(userId);
            if (adjustmentOutModel != null)
            {
                adjustmentOutModel._FormMode = FormModeEnum.Edit;
            }

            if (adjustmentOutModel == null)
            {
                //AdjustmentOutModel = AdjustmentOutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            AdjustmentOutModel adjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            adjustmentOutModel = adjustmentOutService.NavPrevious(userId,Id);
            if (adjustmentOutModel != null)
            {
                adjustmentOutModel._FormMode = FormModeEnum.Edit;
            }

            if (adjustmentOutModel == null)
            {
                //AdjustmentOutModel = AdjustmentOutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            AdjustmentOutModel adjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            adjustmentOutModel = adjustmentOutService.NavNext(userId,Id);
            if (adjustmentOutModel != null)
            {

                adjustmentOutModel._FormMode = FormModeEnum.Edit;

            }

            if (adjustmentOutModel == null)
            {
                //AdjustmentOutModel = AdjustmentOutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            AdjustmentOutModel adjustmentOutModel;
            adjustmentOutService = new AdjustmentOutService();

            adjustmentOutModel = adjustmentOutService.NavLast(userId);
            if (adjustmentOutModel != null)
            {
                adjustmentOutModel._FormMode = FormModeEnum.Edit; 
            }

            if (adjustmentOutModel == null)
            {
                //AdjustmentOutModel = AdjustmentOutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentOutModel);
        }



    }
}