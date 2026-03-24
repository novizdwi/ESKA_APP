using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Controllers;
using System.Web.Script.Serialization;
using Models;
using DevExpress.Web.Mvc;

using System.Net;

using Models._CrystalReport;


using System.IO;

using CrystalDecisions.CrystalReports.Engine;

namespace Controllers
{

    //tidak jadi di pakai di pindahkan ke base controler saja
    public class _CrystalReportController : BaseController
    {
        //public ActionResult Layout()
        //{
        //    long Id = long.Parse(Request["Id"]);
        //    int Layout_Id = int.Parse(Request["Layout_Id"]);

        //    List<CrystalReportParam> crtParams = new List<CrystalReportParam>();

        //    CrystalReportParam param = new CrystalReportParam();
        //    param.ParamName = "@Id";
        //    param.ParamValue = Id.ToString();
        //    crtParams.Add(param);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    String param1 = MvcHtmlString.Create(serializer.Serialize(crtParams)).ToString();

        //    ViewBag.Title = "Preview - Layout";

        //    Tm_Layout tm_Layout;
        //    tm_Layout = DbProvider.dbApp.SingleOrDefault<Tm_Layout>((object)Layout_Id);

        //    string fileName = Request.MapPath("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt");
        //    if (!System.IO.File.Exists(fileName))
        //    {
        //        System.IO.File.WriteAllBytes(fileName, tm_Layout.Data);
        //    }

        //    ViewBag.CrtFile = HttpUtility.UrlEncode("~/Content/Temp/" + tm_Layout.Uid.ToString() + ".rpt");

        //    ViewBag.CrtParam = HttpUtility.UrlEncode(param1);

        //    return View("Rpt");
        //}

        ////PDF
        //[HttpGet]
        //public ActionResult DisplayPdf()
        //{ 
        //    ReportDocument rptDoc;
        //    rptDoc = Models._Utils.Rpt.GetRptDoc(Request);  
        //    if (rptDoc!=null)
        //    { 
        //        Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        return new FileStreamResult(stream, "application/pdf");

        //        //Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
        //        //return new FileStreamResult(stream, "application/vnd.ms-excel");

        //    }
        //    else
        //    {
        //        return null;
        //    } 
        //}

        ////EXCEL
        //[HttpGet]
        //public ActionResult DisplayExcel()
        //{
        //    ReportDocument rptDoc;
        //    rptDoc = Models._Utils.Rpt.GetRptDoc(Request);
        //    if (rptDoc != null)
        //    { 
        //        Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
        //        return new FileStreamResult(stream, "application/vnd.ms-excel"); 
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

    }
}