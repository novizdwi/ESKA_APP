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

using Models.Report.ReportCustom;
using Models._Utils;

namespace Controllers.Report
{
    public partial class ReportCustomController : BaseController
    {

        string VIEW_DETAIL = "ReportCustom";
        string VIEW_FORM_PARTIAL = "Partial/ReportCustom_Form_Partial";
        string VIEW_DETAIL_PARAM_PARTIAL = "Partial/ReportCustom_ParamDetail_Partial";

        ReportCustomService reportCustomService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail()
        {
            int userId = (int)Session["userId"];

            reportCustomService = new ReportCustomService();
            List<ReportCustomModel> reportCustomModels;

            reportCustomModels = reportCustomService.GetModels(userId);

            return View(VIEW_DETAIL, reportCustomModels);
        }

        public ActionResult DetailPartial()
        {
            int userId = (int)Session["userId"];

            reportCustomService = new ReportCustomService();
            List<ReportCustomModel> reportCustomModels;

            reportCustomModels = reportCustomService.GetModels(userId);

            return PartialView(VIEW_FORM_PARTIAL, reportCustomModels);
        }


        //-------------------------------------
        public ActionResult DetailParamPartial(int Report_Id = 0)
        {

            ViewBag.Report_Id = Report_Id;


            return PartialView(VIEW_DETAIL_PARAM_PARTIAL);
        }

        public static ReportCustom_ReportModel GetReportModel(int Report_Id = 0)
        {
            var service = new ReportCustomService();
            ReportCustom_ReportModel reportModels;
            reportModels = service.GetReportCustom_Report(Report_Id);

            return reportModels;
        }


        public static void CreateTreeViewNodesRecursive(List<ReportCustomModel> reportCustomModels, MVCxTreeViewNodeCollection nodesCollection, string parentCode = "")
        {

            if (reportCustomModels != null)
            {
                foreach (var item in reportCustomModels)
                {
                    MVCxTreeViewNode node1 = nodesCollection.Add(item.GroupName, "TvGroupReport_" + item.GroupId.ToString());

                    var reports = item.ListReportNames_;
                    if (reports != null)
                    {
                        foreach (var report in reports)
                        {
                            MVCxTreeViewNode node = node1.Nodes.Add(report.ReportName, "TvReport_" + report.Id.ToString());
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