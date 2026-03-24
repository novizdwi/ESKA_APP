using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;


using System.Web.Script.Serialization;



using Models._CrystalReport;

using System.IO;

using Models._Utils;

using Elmah;

using CrystalDecisions.CrystalReports.Engine;
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Controllers
{


    public class BaseController : Controller
    {

        //----------------
        // Error jika tembus di model : ini seharusnya tidak terjadi
        //
        //----------------

        public string GetErrorModel()
        {
            var errors = new Hashtable();
            foreach (var pair in ModelState)
            {
                if (pair.Value.Errors.Count > 0)
                {
                    errors[pair.Key] = pair.Value.Errors.Select(error => error.ErrorMessage).ToList();
                }
            }

            return new JavaScriptSerializer().Serialize(Json(new { success = false, errors }).Data);
        }

        //----------------
        //Layout
        //----------------
        public ActionResult Print_ListPartial()
        {


            return PartialView("~/Views/_CrystalReport/Layout/Print_List_Partial.cshtml");
        }


        public ActionResult Print_PopupListLoadOnDemandPartial()
        {



            return PartialView("~/Views/_CrystalReport/Layout/Print_Panel_Partial.cshtml");
        }


        public ActionResult CheckLayout(long Id)
        {
            var controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            var userId = (int)Session["userId"];


            var b = SpNotif.SpSysPrintLayoutNotif(controller, Id.ToString(), userId);

            return Json(b, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Layout()
        {
            long Id = long.Parse(Request["Id"]);
            int Layout_Id = int.Parse(Request["Layout_Id"]);

            List<CrystalReportParam> crtParams = new List<CrystalReportParam>();

            CrystalReportParam param = new CrystalReportParam();
            param.ParamName = ":Id";
            param.ParamValue = Id.ToString();
            param.ParamTypeData = "Integer";
            crtParams.Add(param);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            String param1 = MvcHtmlString.Create(serializer.Serialize(crtParams)).ToString();

            ViewBag.Title = "Preview - Layout";

            Tm_Layout tm_Layout;
            tm_Layout = DbProvider.dbApp.Tm_Layout.Find(Layout_Id);

            string fileName = Request.MapPath("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt");
            if (!System.IO.File.Exists(fileName))
            {
                System.IO.File.WriteAllBytes(fileName, tm_Layout.Data);
            }

            if (tm_Layout.OutputType == "Excel")
            {
                return DisplayExcelParam("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt", param1, "");
            }
            else if (tm_Layout.OutputType == "Csv")
            {
                return DisplayCsvParam("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt", param1, "");
            }
            else if (tm_Layout.OutputType == "Text")
            {
                return DisplayTextParam("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt", param1, "");
            }
            else
            {
                ViewBag.CrtFile = HttpUtility.UrlEncode("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt");

                ViewBag.CrtParam = HttpUtility.UrlEncode(param1);

                ViewBag.OutputType = tm_Layout.OutputType;

                ViewBag.DatabaseType = HttpUtility.UrlEncode("");

                return View("~/Views/_CrystalReport/Rpt.cshtml");
            }


        }

        //PDF
        [HttpGet]
        public ActionResult DisplayPdf()
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(Request);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "application/pdf");// { FileDownloadName = "Export.pdf" };
            }
            else
            {
                return null;
            }
        }

        //PDF Param
        [HttpGet]
        public ActionResult DisplayPdfParam(string ReportFile, string ReportParam, string DatabaseType)
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(ReportFile, ReportParam, DatabaseType);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "application/pdf");// { FileDownloadName = "Export.pdf" };
            }
            else
            {
                return null;
            }
        }

        //EXCEL
        [HttpGet]
        public ActionResult DisplayExcel()
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(Request);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = "Export.xls" };
            }
            else
            {
                return null;
            }
        }

        //EXCEL Param
        [HttpGet]
        public ActionResult DisplayExcelParam(string ReportFile, string ReportParam, string DatabaseType)
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(ReportFile, ReportParam, DatabaseType);
            if (rptDoc != null)
            {

                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = "Export.xls" };
            }
            else
            {
                return null;
            }
        }

        //Csv
        [HttpGet]
        public ActionResult DisplayCsv()
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(Request);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "text/csv") { FileDownloadName = "Export.csv" };
            }
            else
            {
                return null;
            }
        }

        //Csv Param
        [HttpGet]
        public ActionResult DisplayCsvParam(string ReportFile, string ReportParam, string DatabaseType)
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(ReportFile, ReportParam, DatabaseType);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "text/csv") { FileDownloadName = "Export.csv" };
            }
            else
            {
                return null;
            }
        }

        //Text
        [HttpGet]
        public ActionResult DisplayText()
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(Request);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Text);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "text/csv") { FileDownloadName = "Export.txt" };

            }
            else
            {
                return null;
            }
        }

        //Text Param
        [HttpGet]
        public ActionResult DisplayTextParam(string ReportFile, string ReportParam, string DatabaseType)
        {
            ReportDocument rptDoc;
            rptDoc = Models._Utils.Rpt.GetRptDoc(ReportFile, ReportParam, DatabaseType);
            if (rptDoc != null)
            {
                Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Text);
                rptDoc.Close();
                rptDoc.Dispose();
                return new FileStreamResult(stream, "text/csv") { FileDownloadName = "Export.txt" };
            }
            else
            {
                return null;
            }
        }

        //----------------
        //ViewJe
        //----------------
        public ActionResult ViewJe_GetResult(string viewName)
        {

            string code = Request["hidden_Code"];
            long id = long.Parse(Request["hidden_Id"]);

            var viewModel = Models._ViewJe.ViewJeModel.GetMasterList(code, id);


            //var ssqlDataTable = PetaPoco.Sql.Builder
            //       .Append("SELECT * FROM [" + DbProvider.dbSap_Name + "].DBO.ODIM T0 (NOLOCK) ORDER BY T0.DimCode"
            //   );
            var dtDim = GeneralGetList.GetDataTable("SELECT * FROM \"" + DbProvider.dbSap_Name + "\".\"ODIM\" T0 ORDER BY T0.\"DimCode\" ");
            ViewBag.DtDim = dtDim;

            return PartialView(viewName, viewModel);
        }

        public ActionResult ViewJe_PopupListLoadOnDemandPartial()
        {

            return ViewJe_GetResult("~/Views/_ViewJe/_ViewJe/Partial/_ViewJePanel.cshtml");
        }


        public ActionResult ViewJe_MasterDetailMasterPartial()
        {
            return ViewJe_GetResult("~/Views/_ViewJe/_ViewJe/Partial/_ViewJeGvMasterPartial.cshtml");
        }


        //
        // Summary:
        //     Called before the action method is invoked.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //untuk sementara datanya di tembak nanti kalo sudah jalan ini di nonactive kan
            ////------------------------------------------------------
            //Session["userId"] = 1;

            //Session["userName"] = "Admin";

            //Session["roleName"] = "Administrator";

            //Session["isAdmin"] = "Y";

            //Session["branchCode"] = "BSR00001";

            //Session["branchName"] = "SR PONDOK INDAH MAL";


            //------------------------------------------------------

            String controler = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            String action = filterContext.ActionDescriptor.ActionName;

            String errorMassage = "";

            if ((controler == "_Alert") && (action == "AjaxAlert"))
            {
                return;
            }

            if (Session["userName"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    errorMassage = "[VALIDATION] Session not exists (Ajax)";

                    if (controler != "_Alert")
                    {
                        throw new Exception(errorMassage);
                    }

                }
                else
                {
                    //Action("Detail", "Login");

                    var urlHelper = new UrlHelper(filterContext.RequestContext);

                    var redirectUrl = urlHelper.Action("Detail", "Login");

                    filterContext.Result = new RedirectResult(redirectUrl);
                    //base.OnActionExecuting(filterContext);
                    return;
                }

            }
            else
            {

                if (Session["isAdmin"] == "Y")
                {

                }
                else
                {

                    if (action.ToLower() == "checklayout")
                    {
                        if (!GeneralGetList.GetAuthAction((int)Session["userId"], controler + "/" + "Print"))
                        {
                            errorMassage = "[VALIDATION] tidak punya akses kesini";
                            throw new Exception(errorMassage);
                        }
                    }

                    else if (action.ToLower() == "layout")
                    {
                        int Layout_Id = int.Parse(Request["Layout_Id"]);
                        if (!Rpt.GetAuthLayout((int)Session["userId"], Layout_Id))
                        {
                            errorMassage = "[VALIDATION] tidak punya akses kesini : layout tidak di temukan";
                            throw new Exception(errorMassage);
                        }
                    }

                    else if ((action.ToLower() == "print") && (controler.ToLower() == "reportcustom"))
                    {
                        int Report_Id = int.Parse(Request["Report_Id"]);

                        if (!Rpt.GetAuthreport((int)Session["userId"], Report_Id))
                        {
                            errorMassage = "[VALIDATION] tidak punya akses kesini : report tidak di temukan";
                            throw new Exception(errorMassage);
                        }
                    }


                    string[] arrAction = { "detail", "add", "update", "post", "cancel", "approve", "reject", "waiting" };

                    string[] arrControlerAttachment = { "miceinquiry" };

                    string[] arrActionAttachment = { "attachment_upload", "attachment_download", "tabattachmenteditmodesdeletepartial" };


                    if (arrAction.Contains(action.ToLower()))
                    {
                        if (!GeneralGetList.GetAuthAction((int)Session["userId"], controler + "/" + action))
                        {
                            errorMassage = "[VALIDATION] tidak punya akses kesini";
                            throw new Exception(errorMassage);
                        }
                    }
                    else if ((arrActionAttachment.Contains(action.ToLower())) && (arrControlerAttachment.Contains(controler.ToLower())))
                    {
                        if (!GeneralGetList.GetAuthAction((int)Session["userId"], controler + "/" + action))
                        {
                            errorMassage = "[VALIDATION] tidak punya akses kesini";
                            throw new Exception(errorMassage);
                        }
                    }


                }


            }

            if (errorMassage != "")
            {
                if (errorMassage.Substring(0, 12) == "[VALIDATION]")
                {
                    var content = errorMassage;

                    filterContext.Result = new ContentResult

                    {
                        ContentType = "text/plain",//Thanks Colin  
                        Content = content
                    };

                    filterContext.HttpContext.Response.Status =

                       "500 " + errorMassage

                       .Replace("\r", " ")

                       .Replace("\n", " ");

                    //filterContext.HttpContext.AddError(new Exception(errorMassage));


                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }


            //System.Diagnostics.Debug.Print("test OnActionExecuting");
        }

        //
        // Summary:
        //     Called after the action method is invoked.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action. 
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //System.Diagnostics.Debug.Print("test OnActionExecuted");
            if (filterContext.Exception != null)
            {

                if (filterContext.Exception.Message.Substring(0, 12) == "[VALIDATION]")
                {
                    var content = filterContext.Exception.Message;

                    filterContext.Result = new ContentResult

                    {
                        ContentType = "text/plain",//Thanks Colin  
                        Content = content
                    };

                    filterContext.HttpContext.Response.Status =

                       "500 " + filterContext.Exception.Message

                       .Replace("\r", " ")

                       .Replace("\n", " ");

                    filterContext.ExceptionHandled = true;

                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }

            }

        }


        //
        // Summary:
        //     Called when authorization occurs.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action.
        //protected void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    System.Diagnostics.Debug.Print("test OnAuthentication");
        //}

        //
        // Summary:
        //     Called when authorization challenge occurs.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action.
        //protected virtual void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    System.Diagnostics.Debug.Print("test OnAuthenticationChallenge");
        //}

        //
        // Summary:
        //     Called when an unhandled exception occurs in the action.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action.
        protected override void OnException(ExceptionContext filterContext)
        {
            //ini supaya yg di kasih di error template jg di log di elmah

            //System.Diagnostics.Debug.Print("test OnException");
            base.OnException(filterContext);
            if (!filterContext.ExceptionHandled)
                return;
            var httpContext = filterContext.HttpContext.ApplicationInstance.Context;
            var signal = ErrorSignal.FromContext(httpContext);

            signal.Raise(filterContext.Exception, httpContext);

        }

        //
        // Summary:
        //     Called after the action result that is returned by an action method is executed.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action result.
        //protected virtual void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    System.Diagnostics.Debug.Print("test OnResultExecuted");
        //}

        //
        // Summary:
        //     Called before the action result that is returned by an action method is executed.
        //
        // Parameters:
        //   filterContext:
        //     Information about the current request and action result.
        //protected virtual void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    System.Diagnostics.Debug.Print("test OnResultExecuting");
        //}

        private bool IsAuthenticated(string controllerName, string actionName)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            //XDocument xDoc = null;

            //if (context.Cache["ControllerActionsSecurity"] == null)
            //{
            //    xDoc =  XDocument.Load(context.Server.MapPath("~/ControllerActionsSecurity.xml
            //    context.Cache.Insert("ControllerActionsSecurity",xDoc
            //}

            //xDoc = (XDocument) context.Cache["ControllerActionsSecurity"];
            //IEnumerable<XElement> elements = xDoc.Element("ControllerSecurity").Elements();

            //var role = (from e in elements
            //            where ((string)e.Attribute("controllerName")) == controllerName
            //            && ((string)e.Attribute("actionName")) == actionName
            //            select new { RoleName = e.Attribute("Roles").Value }).SingleOrDefault();

            //if (role == null) return true;

            //if (!User.IsInRole(role.RoleName))
            //        return false;


            return true;
        }

        //-----------------------------


    }
}