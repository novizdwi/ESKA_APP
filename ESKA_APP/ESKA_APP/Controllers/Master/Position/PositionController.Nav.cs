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
using Models.Master.Position;

namespace Controllers.Master
{
    public partial class PositionController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            PositionModel positionModel;
            positionService = new PositionService();

            positionModel = positionService.NavFirst(userId);
            if (positionModel != null)
            {
                positionModel._FormMode = FormModeEnum.Edit;
            }

            if (positionModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, positionModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            PositionModel positionModel;
            positionService = new PositionService();

            positionModel = positionService.NavPrevious(userId, Id);
            if (positionModel != null)
            {
                positionModel._FormMode = FormModeEnum.Edit;
            }

            if (positionModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, positionModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            PositionModel positionModel;
            positionService = new PositionService();

            positionModel = positionService.NavNext(userId, Id);
            if (positionModel != null)
            {

                positionModel._FormMode = FormModeEnum.Edit;

            }

            if (positionModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, positionModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            PositionModel positionModel;
            positionService = new PositionService();

            positionModel = positionService.NavLast(userId);
            if (positionModel != null)
            {
                positionModel._FormMode = FormModeEnum.Edit;
            }

            if (positionModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, positionModel);
        }



    }
}