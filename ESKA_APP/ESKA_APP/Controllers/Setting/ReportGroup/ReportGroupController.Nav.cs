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
using Models.Setting.ReportGroup;

namespace Controllers.Setting
{
    public partial class ReportGroupController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            ReportGroupModel reportGroupModel;
            reportGroupService = new ReportGroupService();

            reportGroupModel = reportGroupService.NavFirst();
            if (reportGroupModel != null)
            {
                reportGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (reportGroupModel == null)
            {
                //reportGroupModel = reportGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            ReportGroupModel reportGroupModel;
            reportGroupService = new ReportGroupService();

            reportGroupModel = reportGroupService.NavPrevious(Id);
            if (reportGroupModel != null)
            {
                reportGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (reportGroupModel == null)
            {
                //reportGroupModel = reportGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            ReportGroupModel reportGroupModel;
            reportGroupService = new ReportGroupService();

            reportGroupModel = reportGroupService.NavNext(Id);
            if (reportGroupModel != null)
            {

                reportGroupModel._FormMode = FormModeEnum.Edit;

            }

            if (reportGroupModel == null)
            {
                //reportGroupModel = reportGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            ReportGroupModel reportGroupModel;
            reportGroupService = new ReportGroupService();

            reportGroupModel = reportGroupService.NavLast();
            if (reportGroupModel != null)
            {
                reportGroupModel._FormMode = FormModeEnum.Edit;
            }

            if (reportGroupModel == null)
            {
                //reportGroupModel = reportGroupService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }



    }
}