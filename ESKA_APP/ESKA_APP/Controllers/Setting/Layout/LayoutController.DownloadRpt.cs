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
using Models.Setting.Layout;


namespace Controllers.Setting
{
    public partial class LayoutController : BaseController
    {

        public FileResult DownloadRpt(int Id = 0)
        {
            layoutService = new LayoutService();

            LayoutModel model = layoutService.GetById(Id);

            string fileName = model.LayoutName +".rpt";

            return File(model.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
             
        }

        
 
    }

}