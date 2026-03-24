using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models; 

using System.Net;

using Models.Report.ReportAlert;
using Models._Utils;

namespace Controllers.Report
{
    public partial class ReportAlertController : BaseController
    {

        string VIEW_DETAIL = "ReportAlert";
        string VIEW_FORM_PARTIAL = "Partial/ReportAlert_Form_Partial";
        string VIEW_LIST_DETAIL_PARTIAL = "Partial/ReportAlert_ListDetail_Partial";

        ReportAlertService reportAlertService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail()
        {
            int userId = (int)Session["userId"];

            reportAlertService = new ReportAlertService();
            List<ReportAlertModel> reportAlertModels;

            reportAlertModels = reportAlertService.GetModels(userId); 

            return View(VIEW_DETAIL, reportAlertModels);
        }

        public ActionResult DetailPartial()
        {
            int userId = (int)Session["userId"];

            reportAlertService = new ReportAlertService();
            List<ReportAlertModel> reportAlertModels;

            reportAlertModels = reportAlertService.GetModels(userId);



            return PartialView(VIEW_FORM_PARTIAL, reportAlertModels);
        }

        public static void CreateTreeViewNodesRecursive(List<ReportAlertModel> reportAlertModels, MVCxTreeViewNodeCollection nodesCollection, string parentCode = "")
        {

            if (reportAlertModels != null)
            {
                foreach (var item in reportAlertModels)
                {
                    MVCxTreeViewNode node1 = nodesCollection.Add(item.GroupName, "TvGroupAlert_" + item.GroupId.ToString());

                    var alerts = item.ListAlertNames_;
                    if (alerts != null)
                    {
                        foreach (var alert in alerts)
                        {
                            MVCxTreeViewNode node = node1.Nodes.Add(alert.AlertName, "TvAlert_" + alert.Id.ToString());
                            node.Image.Url = "~/Content/Images/button/menu_default.png";
                            node.Image.Height = 15;
                            node.Image.UrlSelected = "~/Content/Images/button/menu_selected.png";
                            //node.NodeStyle.Font.Underline = true;
                            node.NavigateUrl = "javascript:void(0)";
                        }
                    }
                }
            }

        }


    }
}