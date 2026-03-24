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
    public partial class _CflToWarehouseController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflToWarehouse_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflToWarehouse_Panel_List_Partial";

        public CflToWarehouse_ParamModel GetParam(HttpRequestBase Request)
        {
            int userId = (int)Session["userId"];
            var cflParam = new CflToWarehouse_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "ChangeItem")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");
                if(userId != 1)
                {
                    cflParam.SqlWhere = string.Format(" AND " +
                        " T0.\"WhsCode\" IN (SELECT T0_.\"WhsCode\" FROM \"Tm_User_Warehouse\" T0_ WHERE COALESCE(T0_.\"IsTick\",'N') = 'Y' AND T0_.\"Id\"={0} ) " +
                        " ", userId);
                }
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflToWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflToWarehouseParam.Name);
            ProcessCustomToWarehouseding(userId, cflToWarehouseParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflToWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflToWarehouseParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomToWarehouseding(userId, cflToWarehouseParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflToWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflToWarehouseParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomToWarehouseding(userId, cflToWarehouseParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflToWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflToWarehouseParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomToWarehouseding(userId, cflToWarehouseParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflToWarehouseList" + name);
            if (viewModel == null)
            {
                viewModel = CflToWarehouse_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomToWarehouseding(int userId, CflToWarehouse_ParamModel cflParam, GridViewModel viewModel)
        {
            CflToWarehouse_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflToWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflToWarehouseParam.Name);
            ProcessCustomToWarehouseding(userId, cflToWarehouseParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflToWarehouseParam);
        }

    }
}