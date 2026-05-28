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
    public partial class _CflGoodsReceiptPoController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflGoodsReceiptPo_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflGoodsReceiptPo_Panel_List_Partial";

        public CflGoodsReceiptPo_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflGoodsReceiptPo_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            //if (cflParam.Type == "InventoryReceipt")
            //{
            //    cflParam.SqlWhere = string.Format(@"                 
            //        AND NOT EXISTS(
            //            SELECT T1.""Id""
            //            FROM ""Tx_GoodsReceiptPO"" T1
            //            WHERE T0.""DocEntry"" = T1.""BaseEntry""
            //            AND T1.""Status"" NOT IN('Cancel')
            //        )  
            //    ");
            //}

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflGoodsReceiptPoParam = GetParam(Request);

            var viewModel = GetListModel(cflGoodsReceiptPoParam.Name);
            ProcessCustomBinding(userId, cflGoodsReceiptPoParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflGoodsReceiptPoParam = GetParam(Request);

            var viewModel = GetListModel(cflGoodsReceiptPoParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflGoodsReceiptPoParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflGoodsReceiptPoParam = GetParam(Request);

            var viewModel = GetListModel(cflGoodsReceiptPoParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflGoodsReceiptPoParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflGoodsReceiptPoParam = GetParam(Request);

            var viewModel = GetListModel(cflGoodsReceiptPoParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflGoodsReceiptPoParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflGoodsReceiptPoList" + name);
            if (viewModel == null)
            {
                viewModel = CflGoodsReceiptPo_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflGoodsReceiptPo_ParamModel cflGoodsReceiptPoParam, GridViewModel viewModel)
        {
            CflGoodsReceiptPo_Model.SetBindingData(viewModel, userId, cflGoodsReceiptPoParam);

            //viewModel.ProcessCustomBinding(
            //  new GridViewCustomBindingGetDataRowCountHandler(args =>
            //  {
            //      CflGoodsReceiptPo_Model.GetDataRowCount(args, userId, cflGoodsReceiptPoParam);
            //  }),
            //  new GridViewCustomBindingGetDataHandler(args =>
            //  {
            //      CflGoodsReceiptPo_Model.GetData(args, userId, cflGoodsReceiptPoParam);
            //  })
          //);


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflGoodsReceiptPoParam = GetParam(Request);

            var viewModel = GetListModel(cflGoodsReceiptPoParam.Name);
            ProcessCustomBinding(userId, cflGoodsReceiptPoParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflGoodsReceiptPoParam);
        }

    }
}