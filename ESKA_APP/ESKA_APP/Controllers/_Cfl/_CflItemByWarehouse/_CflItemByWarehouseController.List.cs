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
    public partial class _CflItemByWarehouseController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflItemByWarehouse_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflItemByWarehouse_Panel_List_Partial";

        public CflItemByWarehouse_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflItemByWarehouse_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "TransferRequest")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND " +
                                                " T0.\"WhsCode\"= (SELECT T0_.\"FromWhsCode\" FROM \"Tx_TransferRequest\" T0_ WHERE T0_.\"Id\"={0} ) " +
                                                " ", hidden_CflDocId);


            }


            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflItemByWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflItemByWarehouseParam.Name);
            ProcessCustomBinding(userId, cflItemByWarehouseParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflItemByWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflItemByWarehouseParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflItemByWarehouseParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflItemByWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflItemByWarehouseParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflItemByWarehouseParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflItemByWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflItemByWarehouseParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflItemByWarehouseParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflItemByWarehouseList" + name);
            if (viewModel == null)
            {
                viewModel = CflItemByWarehouse_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflItemByWarehouse_ParamModel cflItemByWarehouseParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflItemByWarehouse_Model.GetDataRowCount(args, userId, cflItemByWarehouseParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflItemByWarehouse_Model.GetData(args, userId, cflItemByWarehouseParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflItemByWarehouseParam = GetParam(Request);

            var viewModel = GetListModel(cflItemByWarehouseParam.Name);
            ProcessCustomBinding(userId, cflItemByWarehouseParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflItemByWarehouseParam);
        }

    }
}