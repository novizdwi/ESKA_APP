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
using Models.Setting.Report;


namespace Controllers.Setting
{
    public partial class ReportController : BaseController
    {

        public FileResult DownloadRpt(int Id = 0)
        {
            reportService = new ReportService();

            ReportModel model = reportService.GetById(Id);

            string fileName = model.ReportName +".rpt";

            return File(model.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
             
        }

        
 
    }

}