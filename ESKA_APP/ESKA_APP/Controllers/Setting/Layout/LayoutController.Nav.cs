using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;  

using System.Net;

using Models;
using Models.Setting.Layout;

namespace Controllers.Setting
{
    public partial class LayoutController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            LayoutModel layoutModel;
            layoutService = new LayoutService();

            layoutModel = layoutService.NavFirst();
            if (layoutModel != null)
            {
                layoutModel._FormMode = FormModeEnum.Edit;
            }

            if (layoutModel == null)
            {
                //layoutModel = layoutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            LayoutModel layoutModel;
            layoutService = new LayoutService();

            layoutModel = layoutService.NavPrevious(Id);
            if (layoutModel != null)
            {
                layoutModel._FormMode = FormModeEnum.Edit;
            }

            if (layoutModel == null)
            {
                //layoutModel = layoutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            LayoutModel layoutModel;
            layoutService = new LayoutService();

            layoutModel = layoutService.NavNext(Id);
            if (layoutModel != null)
            {

                layoutModel._FormMode = FormModeEnum.Edit;

            }

            if (layoutModel == null)
            {
                //layoutModel = layoutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            LayoutModel layoutModel;
            layoutService = new LayoutService();

            layoutModel = layoutService.NavLast();
            if (layoutModel != null)
            {
                layoutModel._FormMode = FormModeEnum.Edit; 
            }

            if (layoutModel == null)
            {
                //layoutModel = layoutService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }



    }
}