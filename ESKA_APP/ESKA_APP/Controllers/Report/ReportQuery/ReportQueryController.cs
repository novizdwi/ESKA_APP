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

using Models.Report.ReportQuery;
using Models._Utils;

namespace Controllers.Report
{
    public partial class ReportQueryController : BaseController
    {

        string VIEW_DETAIL = "ReportQuery";
        string VIEW_FORM_PARTIAL = "Partial/ReportQuery_Form_Partial";
        string VIEW_LIST_DETAIL_PARTIAL = "Partial/ReportQuery_ListDetail_Partial";
        string VIEW_DETAIL_PARAM_PARTIAL = "Partial/ReportQuery_ParamDetail_Partial";

        ReportQueryService reportQueryService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail()
        {
            int userId = (int)Session["userId"];

            reportQueryService = new ReportQueryService();
            List<ReportQueryModel> reportQueryModels;

            reportQueryModels = reportQueryService.GetModels(userId);

            return View(VIEW_DETAIL, reportQueryModels);
        }

        public ActionResult DetailPartial()
        {
            int userId = (int)Session["userId"];

            reportQueryService = new ReportQueryService();
            List<ReportQueryModel> reportQueryModels;

            reportQueryModels = reportQueryService.GetModels(userId);

            return PartialView(VIEW_FORM_PARTIAL, reportQueryModels);
        }

        public ActionResult DetailParamPartial(int Query_Id = 0)
        {

            ViewBag.Query_Id = Query_Id;


            return PartialView(VIEW_DETAIL_PARAM_PARTIAL);
        }

        public static ReportQuery_QueryModel GetQueryModel(int Report_Id = 0)
        {
            var service = new ReportQueryService();
            ReportQuery_QueryModel queryModels;
            queryModels = service.GetReportQuery_Query(Report_Id);

            return queryModels;
        }

        public static void CreateTreeViewNodesRecursive(List<ReportQueryModel> reportQueryModels, MVCxTreeViewNodeCollection nodesCollection, string parentCode = "")
        {

            if (reportQueryModels != null)
            {
                foreach (var item in reportQueryModels)
                {
                    MVCxTreeViewNode node1 = nodesCollection.Add(item.GroupName, "TvGroupQuery_" + item.GroupId.ToString());

                    var reports = item.ListQueryNames_;
                    if (reports != null)
                    {
                        foreach (var query in reports)
                        {
                            MVCxTreeViewNode node = node1.Nodes.Add(query.QueryName, "TvQuery_" + query.Id.ToString());
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