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
using System.Data;
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Controllers.Report
{
    public partial class ReportAlertController : BaseController
    {

        public ActionResult ListDetailPartial()
        {
            int userId = (int)Session["userId"];

            int Alert_Id = 0;
            if (Request["Alert_Id"] == null)
            {
                Alert_Id = 0;
            }
            else
            {
                Alert_Id = int.Parse(Request["Alert_Id"]);
            }


            //--------------------------------
            if (Alert_Id == 0)
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
                            Alert_Id = int.Parse(arr[1]);
                        }
                    }

                }
            }

            //  Alert_Id = 2;
            ViewBag.Alert_Id = Alert_Id;
            var param = GetParam(Alert_Id);
            DataTable dataTable = ReportAlertDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;


            //---------------------------------
            var viewModel = GetListDetailModel(Alert_Id, userId);
            DetailProcessCustomBinding(userId, param, viewModel);
            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListDetailPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];
            var Alert_Id = int.Parse(Request["Alert_Id"]);

            var param = GetParam(Alert_Id);
            DataTable dataTable = ReportAlertDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;

            var viewModel = GetListDetailModel(Alert_Id, userId);
            viewModel.ApplyPagingState(pager);
            DetailProcessCustomBinding(userId, param, viewModel);

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }

        // Filtering

        public ActionResult ListDetailFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            int Alert_Id = 0;
            if (Request["Alert_Id"] == null)
            {
                Alert_Id = 0;
            }
            else
            {
                Alert_Id = int.Parse(Request["Alert_Id"]);
            }


            //--------------------------------
            if (Alert_Id == 0)
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
                            Alert_Id = int.Parse(arr[1]);
                        }
                    }

                }
            }

            //  Alert_Id = 2;
            ViewBag.Alert_Id = Alert_Id;
            var param = GetParam(Alert_Id);

            DataTable dataTable = ReportAlertDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;

            //---------------------------------
            var viewModel = GetListDetailModel(Alert_Id, userId);
            viewModel.ApplyFilteringState(filteringState);
            DetailProcessCustomBinding(userId, param, viewModel);
            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListDetailSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];
            var Alert_Id = int.Parse(Request["Alert_Id"]);
            var param = GetParam(Alert_Id);
            DataTable dataTable = ReportAlertDetail__List_Model.GetDataTable(userId, param);
            ViewBag.dataTable = dataTable;


            var viewModel = GetListDetailModel(Alert_Id, userId);
            viewModel.ApplySortingState(column, reset);
            DetailProcessCustomBinding(userId, param, viewModel);

            return PartialView(VIEW_LIST_DETAIL_PARTIAL, viewModel);
        }



        static GridViewModel GetListDetailModel(int Alert_Id, int UserId)
        {

            var viewModel = GridViewExtension.GetViewModel("gvDetail|" + Alert_Id.ToString() + "|");
            if (viewModel == null)
            {
                viewModel = ReportAlertDetail__List_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void DetailProcessCustomBinding(int userId, ReportAlert_ParamModel param, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
             new GridViewCustomBindingGetDataRowCountHandler(args =>
             {
                 ReportAlertDetail__List_Model.GetDataRowCount(args, userId, param);
             }),
             new GridViewCustomBindingGetDataHandler(args =>
             {
                 ReportAlertDetail__List_Model.GetData(args, userId, param);
             })
         );


        }


        public static GridViewModel GetListDetailModel2(int Alert_Id, int UserId)
        {

            GridViewModel viewModel = GetListDetailModel(Alert_Id, UserId);

            var param = GetParam(Alert_Id);

            DetailProcessCustomBinding(UserId, param, viewModel);


            return viewModel;
        }

        public static DataTable GetDataTable2(int Alert_Id, int userId)
        {

            var param = GetParam(Alert_Id);

            DataTable dataTable = ReportAlertDetail__List_Model.GetDataTable(userId, param);

            GridViewModel viewModel = GetListDetailModel(Alert_Id, userId);

            return dataTable;
        }





        public static ReportAlert_ParamModel GetParam(int Alert_Id)
        {
            var param = new ReportAlert_ParamModel();

            var tm_Alert = DbProvider.dbApp.Tm_Alert.Find(Alert_Id);
            if (tm_Alert != null)
            {
                param.Code = Alert_Id.ToString();
                param.Name = tm_Alert.AlertName;
                param.Sql = tm_Alert.Sql;

            }
            else
            {
                param.Code = Alert_Id.ToString();
                param.Name = "";
                param.Sql = "";
            }

            return param;
        }


    }
}