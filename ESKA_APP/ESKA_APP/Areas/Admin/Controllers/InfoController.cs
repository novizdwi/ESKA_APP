using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Areas.Admin.Controllers
{
    public partial class InfoController : Controller
    {
        public ActionResult Index()
        {

            var ConnectionPoolCount = Models._Sap.SAPCachedCompany.ConnectionPoolCount().ToString();

            var str = "ConnectionPoolCount:" + ConnectionPoolCount;

            return Content(str);
        }
    }

}
