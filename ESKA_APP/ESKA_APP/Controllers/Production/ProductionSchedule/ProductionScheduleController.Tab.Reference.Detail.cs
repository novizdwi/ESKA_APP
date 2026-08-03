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
using Models.Production;

namespace Controllers.Production
{
    public partial class ProductionScheduleController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/ProductionSchedule_Form_TabReference_List_DetailPartial"; 
        
        public ActionResult TabReferenceDetailListPartial(long Id)
        {
            ViewData["Id"] = Id;
            int userId = (int)Session["userId"]; 
            productionScheduleService = new ProductionScheduleService();
            var modelListDetail = productionScheduleService.ProductionSchedule_TabReferenceDetails(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}