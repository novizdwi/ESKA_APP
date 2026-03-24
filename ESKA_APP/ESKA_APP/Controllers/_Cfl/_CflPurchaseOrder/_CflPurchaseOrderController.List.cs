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
    public partial class _CflPurchaseOrderController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflPurchaseOrder_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflPurchaseOrder_Panel_List_Partial";

        public CflPurchaseOrder_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflPurchaseOrder_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "InventoryReceipt")
            {
                cflParam.SqlWhere = string.Format(@"                 
                    AND NOT EXISTS(
                        SELECT T1.""Id""
                        FROM ""Tx_GoodsReceiptPO"" T1
                        WHERE T0.""DocEntry"" = T1.""BaseEntry""
                        AND T1.""Status"" NOT IN('Cancel')
                    )  
                ");
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderParam.Name);
            ProcessCustomBinding(userId, cflPurchaseOrderParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflPurchaseOrderParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflPurchaseOrderParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflPurchaseOrderParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflPurchaseOrderList" + name);
            if (viewModel == null)
            {
                viewModel = CflPurchaseOrder_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflPurchaseOrder_ParamModel cflPurchaseOrderParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflPurchaseOrder_Model.GetDataRowCount(args, userId, cflPurchaseOrderParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflPurchaseOrder_Model.GetData(args, userId, cflPurchaseOrderParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderParam.Name);
            ProcessCustomBinding(userId, cflPurchaseOrderParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflPurchaseOrderParam);
        }

    }
}