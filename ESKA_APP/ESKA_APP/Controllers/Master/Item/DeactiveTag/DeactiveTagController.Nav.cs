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
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class DeactiveTagController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel deactiveTagModel;
            deactiveTagService = new DeactiveTagService();

            deactiveTagModel = deactiveTagService.NavFirst(userId);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }

            if (deactiveTagModel == null)
            {
                //DeactiveTagModel = DeactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            DeactiveTagModel deactiveTagModel;
            deactiveTagService = new DeactiveTagService();

            deactiveTagModel = deactiveTagService.NavPrevious(userId,Id);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit;
            }

            if (deactiveTagModel == null)
            {
                //DeactiveTagModel = DeactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            DeactiveTagModel deactiveTagModel;
            deactiveTagService = new DeactiveTagService();

            deactiveTagModel = deactiveTagService.NavNext(userId,Id);
            if (deactiveTagModel != null)
            {

                deactiveTagModel._FormMode = FormModeEnum.Edit;

            }

            if (deactiveTagModel == null)
            {
                //DeactiveTagModel = DeactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            DeactiveTagModel deactiveTagModel;
            deactiveTagService = new DeactiveTagService();

            deactiveTagModel = deactiveTagService.NavLast(userId);
            if (deactiveTagModel != null)
            {
                deactiveTagModel._FormMode = FormModeEnum.Edit; 
            }

            if (deactiveTagModel == null)
            {
                //DeactiveTagModel = DeactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, deactiveTagModel);
        }



    }
}