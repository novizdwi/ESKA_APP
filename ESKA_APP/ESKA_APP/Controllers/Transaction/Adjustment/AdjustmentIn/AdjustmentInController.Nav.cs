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
    public partial class AdjustmentInController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel adjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            adjustmentInModel = adjustmentInService.NavFirst(userId);
            if (adjustmentInModel != null)
            {
                adjustmentInModel._FormMode = FormModeEnum.Edit;
            }

            if (adjustmentInModel == null)
            {
                //AdjustmentInModel = AdjustmentInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            AdjustmentInModel adjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            adjustmentInModel = adjustmentInService.NavPrevious(userId,Id);
            if (adjustmentInModel != null)
            {
                adjustmentInModel._FormMode = FormModeEnum.Edit;
            }

            if (adjustmentInModel == null)
            {
                //AdjustmentInModel = AdjustmentInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            AdjustmentInModel adjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            adjustmentInModel = adjustmentInService.NavNext(userId,Id);
            if (adjustmentInModel != null)
            {

                adjustmentInModel._FormMode = FormModeEnum.Edit;

            }

            if (adjustmentInModel == null)
            {
                //AdjustmentInModel = AdjustmentInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            AdjustmentInModel adjustmentInModel;
            adjustmentInService = new AdjustmentInService();

            adjustmentInModel = adjustmentInService.NavLast(userId);
            if (adjustmentInModel != null)
            {
                adjustmentInModel._FormMode = FormModeEnum.Edit; 
            }

            if (adjustmentInModel == null)
            {
                //AdjustmentInModel = AdjustmentInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, adjustmentInModel);
        }



    }
}