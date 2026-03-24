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
using Models.Setting.Report;

namespace Controllers.Setting
{
    public partial class ReportController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            ReportModel reportModel;
            reportService = new ReportService();

            reportModel = reportService.NavFirst();
            if (reportModel != null)
            {
                reportModel._FormMode = FormModeEnum.Edit;
            }

            if (reportModel == null)
            {
                //reportModel = reportService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(int Id = 0)
        {

            ReportModel reportModel;
            reportService = new ReportService();

            reportModel = reportService.NavPrevious(Id);
            if (reportModel != null)
            {
                reportModel._FormMode = FormModeEnum.Edit;
            }

            if (reportModel == null)
            {
                //reportModel = reportService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(int Id = 0)
        {


            ReportModel reportModel;
            reportService = new ReportService();

            reportModel = reportService.NavNext(Id);
            if (reportModel != null)
            {

                reportModel._FormMode = FormModeEnum.Edit;

            }

            if (reportModel == null)
            {
                //reportModel = reportService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            ReportModel reportModel;
            reportService = new ReportService();

            reportModel = reportService.NavLast();
            if (reportModel != null)
            {
                reportModel._FormMode = FormModeEnum.Edit; 
            }

            if (reportModel == null)
            {
                //reportModel = reportService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }



    }
}