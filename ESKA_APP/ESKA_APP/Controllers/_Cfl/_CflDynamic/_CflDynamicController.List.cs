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

using System.Data;

namespace Controllers._Cfl
{
    public partial class _CflDynamicController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflDynamic_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflDynamic_Panel_List_Partial";

        public CflDynamic_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflDynamic_ParamModel();
            cflParam.Code = Request["hidden_CflDynamicCode"];
            cflParam.Description = Request["hidden_CflDynamicDescription"];
            cflParam.Sql = Request["hidden_CflDynamicSql"];
            cflParam.IsMulti = Request["hidden_CflDynamicIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflDymanicParam = GetParam(Request);  

            DataTable dataTable = CflDynamic_Model.GetDataTable(userId, cflDymanicParam);
            ViewBag.dataTable = dataTable; 

            var viewModel = GetListModel(cflDymanicParam.Code);
            ProcessCustomBinding(userId, cflDymanicParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflDymanicParam = GetParam(Request);  

            DataTable dataTable = CflDynamic_Model.GetDataTable(userId, cflDymanicParam);
            ViewBag.dataTable = dataTable; 

            var viewModel = GetListModel(cflDymanicParam.Code);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflDymanicParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering

        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflDymanicParam = GetParam(Request);  

            DataTable dataTable = CflDynamic_Model.GetDataTable(userId, cflDymanicParam);
            ViewBag.dataTable = dataTable; 

            var viewModel = GetListModel(cflDymanicParam.Code);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflDymanicParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflDymanicParam = GetParam(Request);  

            DataTable dataTable = CflDynamic_Model.GetDataTable(userId, cflDymanicParam);
            ViewBag.dataTable = dataTable; 

            var viewModel = GetListModel(cflDymanicParam.Code);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflDymanicParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflDynamic" + name);

            if (viewModel == null)
            {
                viewModel = CflDynamic_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflDynamic_ParamModel cflDynamicParam, GridViewModel viewModel)
        {

            CflDynamic_Model.SetBindingData(viewModel, userId, cflDynamicParam); 
          

        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflDymanicParam = GetParam(Request);  

            DataTable dataTable = CflDynamic_Model.GetDataTable(userId, cflDymanicParam);
            ViewBag.dataTable = dataTable; 

            var viewModel = GetListModel(cflDymanicParam.Code);

            ProcessCustomBinding(userId, cflDymanicParam, viewModel);

            ViewBag.viewModel = viewModel; 

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflDymanicParam);
        }

    }
}