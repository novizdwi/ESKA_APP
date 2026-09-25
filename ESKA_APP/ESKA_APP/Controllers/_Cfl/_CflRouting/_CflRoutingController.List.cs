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
using Models._Cfl;


namespace Controllers._Cfl
{
    public partial class _CflRoutingController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflRouting_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflRouting_Panel_List_Partial";

        public CflRouting_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflRouting_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];


            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflRoutingParam = GetParam(Request);

            var viewModel = GetListModel(cflRoutingParam.Name);
            ProcessCustomRoutingding(userId, cflRoutingParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflRoutingParam = GetParam(Request);

            var viewModel = GetListModel(cflRoutingParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomRoutingding(userId, cflRoutingParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflRoutingParam = GetParam(Request);

            var viewModel = GetListModel(cflRoutingParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomRoutingding(userId, cflRoutingParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflRoutingParam = GetParam(Request);

            var viewModel = GetListModel(cflRoutingParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomRoutingding(userId, cflRoutingParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflRoutingList" + name);
            if (viewModel == null)
            {
                viewModel = CflRouting_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomRoutingding(int userId, CflRouting_ParamModel cflParam, GridViewModel viewModel)
        {
            CflRouting_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflRoutingParam = GetParam(Request);

            var viewModel = GetListModel(cflRoutingParam.Name);
            ProcessCustomRoutingding(userId, cflRoutingParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflRoutingParam);
        }

    }
}
