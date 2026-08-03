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
using Models.Production;

namespace Controllers.Production
{
    public partial class ProductionTaskController : BaseController
    {
        public ActionResult TabTransListPartial()
        {
            int userId = (int)Session["userId"]; 
            productionTaskService = new ProductionTaskService(); 

            var modelList = productionTaskService.ProductionTask_GetReferences(userId, "today");

            return PartialView(VIEW_FORM_TABREFERENCE_PARTIAL, modelList);
        }




    }
}