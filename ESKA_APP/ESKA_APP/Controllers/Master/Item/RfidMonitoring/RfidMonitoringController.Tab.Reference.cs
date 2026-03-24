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
using Models.Master.Item;

namespace Controllers.Master.Item
{
    public partial class RfidMonitoringController : BaseController
    {
        public ActionResult TabTransListPartial()
        {
            int userId = (int)Session["userId"];
            DateTime toDate = DateTime.Now;
            DateTime fromDate = toDate.AddMonths(-1);
            if (!string.IsNullOrEmpty(Request["cbFromDate"]))
            {
                fromDate = Convert.ToDateTime((Request["cbFromDate"]).ToString() ); 
            }
            if (!string.IsNullOrEmpty(Request["cbToDate"]))
            {
                toDate = Convert.ToDateTime((Request["cbToDate"]).ToString() ); 
            }
            rfidMonitoringService = new RfidMonitoringService();
            string itemCode = (Request["cbItemCode"]).ToString();
            string whsCode = (Request["cbWhsCode"]).ToString();
            string tagId = (Request["cbTagId"]).ToString();
            string status = (Request["cbStatus"]).ToString();

            var modelList = rfidMonitoringService.RfidMonitoring_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);

            return PartialView(VIEW_FORM_TABREFERENCE_PARTIAL, modelList);
        }




    }
}