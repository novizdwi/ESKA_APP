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
    public partial class _CflTagController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflTag_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflTag_Panel_List_Partial";

        public CflTag_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflTag_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "ChangeItem")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                var hidden_CflOriginItemCode = (string)Request["hidden_CflOriginItemCode"];
                var hidden_CflWhsCode = (string)Request["hidden_CflWhsCode"];

                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");
                cflParam.SqlWhere = string.Format(" " +
                    "AND \"Status\" = 'A' " +
                    "AND \"ItemCode\" = '"+ hidden_CflOriginItemCode + "' " +
                    "AND \"WhsCode\" = '"+ hidden_CflWhsCode + "' ");
            }
            else if(cflParam.Type == "DeactiveTag")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");
                cflParam.SqlWhere = string.Format(" " +
                    "AND \"Status\" = 'A'  ");
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];
            
            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflTagParam = GetParam(Request);

            var viewModel = GetListModel(cflTagParam.Name);
            ProcessCustomBinding(userId, cflTagParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflTagParam = GetParam(Request);

            var viewModel = GetListModel(cflTagParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflTagParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflTagParam = GetParam(Request);

            var viewModel = GetListModel(cflTagParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflTagParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflTagParam = GetParam(Request);

            var viewModel = GetListModel(cflTagParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflTagParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflTagList" + name);
            if (viewModel == null)
            {
                viewModel = CflTag_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflTag_ParamModel cflTagParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflTag_Model.GetDataRowCount(args, userId, cflTagParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflTag_Model.GetData(args, userId, cflTagParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflTagParam = GetParam(Request);

            var viewModel = GetListModel(cflTagParam.Name);
            ProcessCustomBinding(userId, cflTagParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflTagParam);
        }

    }
}