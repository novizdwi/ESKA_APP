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
using Models.Setting.Layout;


namespace Controllers.Setting
{
    public partial class LayoutController : BaseController
    {

        string VIEW_DETAIL = "Layout";
        string VIEW_FORM_PARTIAL = "Partial/Layout_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Layout_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Layout_Panel_List_Partial";

        LayoutService layoutService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(int Id = 0)
        {

            layoutService = new LayoutService();
            LayoutModel layoutModel;
            if (Id == 0)
            {

                layoutModel = layoutService.GetNewModel();
                layoutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                layoutService = new LayoutService();
                layoutModel = layoutService.GetById(Id);
                if (layoutModel != null)
                {
                    layoutModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, layoutModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            LayoutModel layoutModel;

            layoutService = new LayoutService();

            if (Id == 0)
            {
                layoutModel = layoutService.GetNewModel();
                layoutModel._FormMode = FormModeEnum.New;
            }
            else
            {
                layoutModel = layoutService.GetById(Id);
                if (layoutModel != null)
                {
                    layoutModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  LayoutModel layoutModel)
        {
            var _HiddenUid = Request["_HiddenUid"];
            layoutModel._HiddenUid = _HiddenUid;

            layoutModel._UserId = (int)Session["userId"];

            layoutService = new LayoutService();


            if (ModelState.IsValid)
            {
                if (layoutModel._HiddenUid != "")
                {
                    layoutModel.Data = System.IO.File.ReadAllBytes(LayoutUploadControlHelper.GetFilePath(layoutModel._HiddenUid));
                }

                layoutService.Add(layoutModel);
                layoutModel = layoutService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }


            layoutModel._FormMode = Models.FormModeEnum.New;


            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  LayoutModel layoutModel)
        {
            var _HiddenUid = Request["_HiddenUid"];
            layoutModel._HiddenUid = _HiddenUid;

            layoutModel._UserId = (int)Session["userId"];

            layoutService = new LayoutService();
            layoutModel._FormMode = FormModeEnum.Edit;


            if (ModelState.IsValid)
            {
                if (layoutModel._HiddenUid != "")
                {
                    layoutModel.Data = System.IO.File.ReadAllBytes(LayoutUploadControlHelper.GetFilePath(layoutModel._HiddenUid));
                }

                layoutService.Update(layoutModel);
                layoutModel = layoutService.GetById(layoutModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            layoutService = new LayoutService();

            if (Id != 0)
            {
                layoutService = new LayoutService();
                layoutService.DeleteById(Id);
            }

            LayoutModel layoutModel = new LayoutModel();
            return PartialView(VIEW_FORM_PARTIAL, layoutModel);
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
                        //binder.UploadControlBinderSettings.ValidationSettings = LayoutUploadControlHelper.ValidationSettings;
                        binder.UploadControlBinderSettings.FileUploadCompleteHandler = LayoutUploadControlHelper.FileUploadComplete;
                        break;
                }

                base.Initialize(requestContext);
            }

        }

    }



    public class LayoutUploadControlHelper
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