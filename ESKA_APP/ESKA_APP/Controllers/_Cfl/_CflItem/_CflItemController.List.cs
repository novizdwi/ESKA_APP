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
    public partial class _CflItemController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflItem_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflItem_Panel_List_Partial";

        public CflItem_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflItem_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "OriginItem" || cflParam.Type == "ToItem")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");
                cflParam.SqlWhere = string.Format(@" AND  COALESCE(T0.""U_IDU_IsItemFG"", 'N') = 'Y' ");
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];
            
            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflItemParam = GetParam(Request);

            var viewModel = GetListModel(cflItemParam.Name);
            ProcessCustomBinding(userId, cflItemParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflItemParam = GetParam(Request);

            var viewModel = GetListModel(cflItemParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflItemParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflItemParam = GetParam(Request);

            var viewModel = GetListModel(cflItemParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflItemParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflItemParam = GetParam(Request);

            var viewModel = GetListModel(cflItemParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflItemParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflItemList" + name);
            if (viewModel == null)
            {
                viewModel = CflItem_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflItem_ParamModel cflItemParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflItem_Model.GetDataRowCount(args, userId, cflItemParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflItem_Model.GetData(args, userId, cflItemParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflItemParam = GetParam(Request);

            var viewModel = GetListModel(cflItemParam.Name);
            ProcessCustomBinding(userId, cflItemParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflItemParam);
        }

    }
}