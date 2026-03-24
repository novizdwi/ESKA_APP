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
    public partial class _CflCostCenterController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflCostCenter_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflCostCenter_Panel_List_Partial";

        public CflCostCenter_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflCostCenter_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "Pillars")
            {
                cflParam.SqlWhere = string.Format(" AND T0.\"DimCode\" = '1' ");
            }


            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflCostCenterParam = GetParam(Request);

            var viewModel = GetListModel(cflCostCenterParam.Name);
            ProcessCustomBinding(userId, cflCostCenterParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflCostCenterParam = GetParam(Request);

            var viewModel = GetListModel(cflCostCenterParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflCostCenterParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflCostCenterParam = GetParam(Request);

            var viewModel = GetListModel(cflCostCenterParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflCostCenterParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflCostCenterParam = GetParam(Request);

            var viewModel = GetListModel(cflCostCenterParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflCostCenterParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflCostCenterList" + name);
            if (viewModel == null)
            {
                viewModel = CflCostCenter_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflCostCenter_ParamModel cflParam, GridViewModel viewModel)
        {
            CflCostCenter_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflCostCenterParam = GetParam(Request);

            var viewModel = GetListModel(cflCostCenterParam.Name);
            ProcessCustomBinding(userId, cflCostCenterParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflCostCenterParam);
        }

    }
}