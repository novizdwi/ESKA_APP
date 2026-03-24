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
    public partial class _CflWarehouseController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflWarehouse_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflWarehouse_Panel_List_Partial";

        public CflWarehouse_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflWarehouse_ParamModel();
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

            var cflWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflWarehouseParam.Name);
            ProcessCustomWarehouseding(userId, cflWarehouseParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflWarehouseParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomWarehouseding(userId, cflWarehouseParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflWarehouseParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomWarehouseding(userId, cflWarehouseParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflWarehouseParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomWarehouseding(userId, cflWarehouseParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflWarehouseList" + name);
            if (viewModel == null)
            {
                viewModel = CflWarehouse_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomWarehouseding(int userId, CflWarehouse_ParamModel cflParam, GridViewModel viewModel)
        {
            CflWarehouse_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflWarehouseParam.Name);
            ProcessCustomWarehouseding(userId, cflWarehouseParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflWarehouseParam);
        }

    }
}