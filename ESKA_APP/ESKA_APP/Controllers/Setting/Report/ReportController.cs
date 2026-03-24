using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;

using DevExpress.Web;

using System.Net;

using Models;
using Models.Setting.Report;

namespace Controllers.Setting
{
    public partial class ReportController : BaseController
    {

        string VIEW_DETAIL = "Report";
        string VIEW_FORM_PARTIAL = "Partial/Report_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Report_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Report_Panel_List_Partial";

        ReportService reportService; 

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
 

        public ActionResult Detail(int Id =0)
        {

            reportService = new ReportService();
            ReportModel reportModel;
            if (Id == 0)
            {

                reportModel = reportService.GetNewModel();
                reportModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reportService = new ReportService();
                reportModel = reportService.GetById(Id);
                if (reportModel != null)
                {
                    reportModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, reportModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            ReportModel reportModel;

            reportService = new ReportService();

            if (Id == 0)
            {
                reportModel = reportService.GetNewModel();
                reportModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reportModel = reportService.GetById(Id);
                if (reportModel != null)
                {
                    reportModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }

 

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ReportModel reportModel)
        {

            reportModel._UserId = (int)Session["userId"];

            //value di set sesaat sebelum di post ke server : tidak tertampung di DXMVCEditorsValues sengginga tidak terbaca di model
            reportModel._HiddenUid = Request["_HiddenUid"];

            reportService = new ReportService();
 

            if (ModelState.IsValid)
            {
                if (reportModel._HiddenUid != "")
                {
                    reportModel.Data = System.IO.File.ReadAllBytes(ReportUploadControlHelper.GetFilePath(reportModel._HiddenUid));
                }

                reportService.Add(reportModel);
                reportModel = reportService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            
            reportModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ReportModel reportModel)
        {

            reportModel._UserId = (int)Session["userId"];
            //value di set sesaat sebelum di post ke server : tidak tertampung di DXMVCEditorsValues sengginga tidak terbaca di model
            reportModel._HiddenUid = Request["_HiddenUid"];

            reportService = new ReportService();
            reportModel._FormMode = FormModeEnum.Edit;

           
          
            if (ModelState.IsValid)
            {
                if (reportModel._HiddenUid != "")
                {
                    reportModel.Data = System.IO.File.ReadAllBytes(ReportUploadControlHelper.GetFilePath(reportModel._HiddenUid));
                }

                reportService.Update(reportModel);
                reportModel = reportService.GetById(reportModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            } 

            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            reportService = new ReportService();

            if (Id != 0)
            {
                reportService = new ReportService();
                reportService.DeleteById(Id);

            }

            ReportModel reportModel = new ReportModel();
            return PartialView(VIEW_FORM_PARTIAL, reportModel);
        }


        public ActionResult CallbacksUpload(IEnumerable<UploadedFile> Files)
        {

            return null;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            {
                var actionName = (string)requestContext.RouteData.Values["Action"];
                switch (actionName)
                {
                    case "CallbacksUpload":
                        var binder = (DevExpressEditorsBinder)ModelBinders.Binders.DefaultBinder;
                        //binder.UploadControlBinderSettings.ValidationSettings = ReportUploadControlHelper.ValidationSettings;
                        binder.UploadControlBinderSettings.FileUploadCompleteHandler = ReportUploadControlHelper.FileUploadComplete;
                        break;
                }

                base.Initialize(requestContext);
            }

        }


    }




    public class ReportUploadControlHelper
    {
        public const string UploadDirectory = "~/Content/Temp/";

        public static UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".rpt" },
            MaxFileSize = 20971520
        };

        public static string GetFilePath(string FileName)
        {
            var resultFilePath = UploadDirectory + FileName + ".rpt";
            return HttpContext.Current.Request.MapPath(resultFilePath);
        }

        public static void FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                // Save uploaded file to some location
                StreamReader reader = new StreamReader(e.UploadedFile.FileContent);
                var uid = Guid.NewGuid();

                var resultFilePath = UploadDirectory + uid + ".rpt";

                if (!System.IO.File.Exists(resultFilePath))
                {
                    e.UploadedFile.SaveAs(HttpContext.Current.Request.MapPath(resultFilePath));
                }


                e.CallbackData = uid.ToString();
            }
            else
            {
                e.CallbackData = "";
            }

        }
    }


}