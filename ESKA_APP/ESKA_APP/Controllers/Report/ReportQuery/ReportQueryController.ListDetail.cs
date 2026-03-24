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
using System.Data;
using Models._Ef;
using ESKA_DI.Models._EF;

using Models._CrystalReport;
using System.Web.Script.Serialization;

namespace Controllers.Report
{
    public partial class ReportQueryController : BaseController
    {

        public ActionResult ShowResultQuery([ModelBinder(typeof(DevExpressEditorsBinder))] List<CrystalReportParam> Params)
        {


            List<CrystalReportParam> crtParams = new List<CrystalReportParam>();


            int userId = (int)Session["userId"];

            int Query_Id = 0;
            if (Request["Query_Id"] == null)
            {
                Query_Id = 0;
            }
            else
            {
                Query_Id = int.Parse(Request["Query_Id"]);
            }


            //  Query_Id = 2;
            ViewBag.Query_Id = Query_Id;
            var param = GetParam(Query_Id);
            param.crtParams = Params;

            DataTable dataTable = ReportQueryDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;






            //---------------------------------
            var viewModel = GetListDetailModel(Query_Id, userId);
            DetailProcessCustomBinding(userId, param, viewModel);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            String param1 = MvcHtmlString.Create(serializer.Serialize(Params)).ToString();

            ViewBag.Params = param1;


            ViewBag.RowCount = param.RowCount;

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);

        }

        public ActionResult ListDetailPartial()
        {
            int userId = (int)Session["userId"];

            int Query_Id = 0;
            if (Request["Query_Id"] == null)
            {
                Query_Id = 0;
            }
            else
            {
                Query_Id = int.Parse(Request["Query_Id"]);
            }


            //--------------------------------
            if (Query_Id == 0)
            {
                string DXCallbackName = "";

                if (Request["DXCallbackName"] != null)
                {
                    DXCallbackName = Request["DXCallbackName"];

                    if (DXCallbackName.StartsWith("gvDetail|"))
                    {

                        var arr = DXCallbackName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length > 1)
                        {
                            Query_Id = int.Parse(arr[1]);
                        }
                    }

                }
            }

            //  Query_Id = 2;
            ViewBag.Query_Id = Query_Id;
            var param = GetParam(Query_Id);
            DataTable dataTable = ReportQueryDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;


            //---------------------------------
            var viewModel = GetListDetailModel(Query_Id, userId);
            DetailProcessCustomBinding(userId, param, viewModel);

            ViewBag.RowCount = param.RowCount;

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListDetailPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];
            var Query_Id = int.Parse(Request["Query_Id"]);

            var param = GetParam(Query_Id);
            DataTable dataTable = ReportQueryDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;

            var viewModel = GetListDetailModel(Query_Id, userId);
            viewModel.ApplyPagingState(pager);
            DetailProcessCustomBinding(userId, param, viewModel);



            ViewBag.RowCount = param.RowCount;

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }

        // Filtering

        public ActionResult ListDetailFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            int Query_Id = 0;
            if (Request["Query_Id"] == null)
            {
                Query_Id = 0;
            }
            else
            {
                Query_Id = int.Parse(Request["Query_Id"]);
            }


            //--------------------------------
            if (Query_Id == 0)
            {
                string DXCallbackName = "";

                if (Request["DXCallbackName"] != null)
                {
                    DXCallbackName = Request["DXCallbackName"];

                    if (DXCallbackName.StartsWith("gvDetail|"))
                    {

                        var arr = DXCallbackName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length > 1)
                        {
                            Query_Id = int.Parse(arr[1]);
                        }
                    }

                }
            }

            //  Query_Id = 2;
            ViewBag.Query_Id = Query_Id;
            var param = GetParam(Query_Id);

            DataTable dataTable = ReportQueryDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;

            //---------------------------------
            var viewModel = GetListDetailModel(Query_Id, userId);
            viewModel.ApplyFilteringState(filteringState);
            DetailProcessCustomBinding(userId, param, viewModel);



            ViewBag.RowCount = param.RowCount;

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListDetailSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];
            var Query_Id = int.Parse(Request["Query_Id"]);
            var param = GetParam(Query_Id);
            DataTable dataTable = ReportQueryDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;


            var viewModel = GetListDetailModel(Query_Id, userId);
            viewModel.ApplySortingState(column, reset);
            DetailProcessCustomBinding(userId, param, viewModel); 

            ViewBag.RowCount = param.RowCount;

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }



        static GridViewModel GetListDetailModel(int Query_Id, int UserId)
        {

            var viewModel = GridViewExtension.GetViewModel("gvDetail|" + Query_Id.ToString() + "|");
            if (viewModel == null)
            {
                viewModel = ReportQueryDetail__List_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void DetailProcessCustomBinding(int userId, ReportQuery_ParamModel param, GridViewModel viewModel)
        {
            ReportQueryDetail__List_Model.SetBindingData(viewModel, userId, param);  

        }


        public static GridViewModel GetListDetailModel2(int Query_Id, int UserId)
        {

            GridViewModel viewModel = GetListDetailModel(Query_Id, UserId);

            var param = GetParam(Query_Id);

            DetailProcessCustomBinding(UserId, param, viewModel);


            return viewModel;
        }

        public static DataTable GetDataTable2(int Query_Id, int userId)
        {

            var param = GetParam(Query_Id);

            DataTable dataTable = ReportQueryDetail__List_Model.GetDataTable(userId, param);

            GridViewModel viewModel = GetListDetailModel(Query_Id, userId);

            return dataTable;
        }





        public static ReportQuery_ParamModel GetParam(int Query_Id)
        {
            var param = new ReportQuery_ParamModel();

            var tm_Query = DbProvider.dbApp.Tm_Query.Find(Query_Id);
            if (tm_Query != null)
            {
                param.Code = Query_Id.ToString();
                param.Name = tm_Query.QueryName;
                param.Sql = tm_Query.Sql;

            }
            else
            {
                param.Code = Query_Id.ToString();
                param.Name = "";
                param.Sql = "";
            }

            return param;
        }


    }
}