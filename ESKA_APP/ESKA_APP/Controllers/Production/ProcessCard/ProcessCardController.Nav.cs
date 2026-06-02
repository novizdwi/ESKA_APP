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
using Models.Production;

namespace Controllers.Production
{
    public partial class ProcessCardController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ProcessCardModel ProcessCardModel;
            processCardService = new ProcessCardService();

            ProcessCardModel = processCardService.NavFirst(userId);
            if (ProcessCardModel != null)
            {
                ProcessCardModel._FormMode = FormModeEnum.Edit;
            }

            if (ProcessCardModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ProcessCardModel ProcessCardModel;
            processCardService = new ProcessCardService();

            ProcessCardModel = processCardService.NavPrevious(userId, Id);
            if (ProcessCardModel != null)
            {
                ProcessCardModel._FormMode = FormModeEnum.Edit;
            }

            if (ProcessCardModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            ProcessCardModel ProcessCardModel;
            processCardService = new ProcessCardService();

            ProcessCardModel = processCardService.NavNext(userId, Id);
            if (ProcessCardModel != null)
            {

                ProcessCardModel._FormMode = FormModeEnum.Edit;

            }

            if (ProcessCardModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ProcessCardModel ProcessCardModel;
            processCardService = new ProcessCardService();

            ProcessCardModel = processCardService.NavLast(userId);
            if (ProcessCardModel != null)
            {
                ProcessCardModel._FormMode = FormModeEnum.Edit;
            }

            if (ProcessCardModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, ProcessCardModel);
        }



    }
}