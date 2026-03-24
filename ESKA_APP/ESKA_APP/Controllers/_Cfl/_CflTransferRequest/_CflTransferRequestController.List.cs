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
    public partial class _CflTransferRequestController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflTransferRequest_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflTransferRequest_Panel_List_Partial";

        public CflTransferRequest_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflTransferRequest_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            int userId = (int)Session["userId"];

            string ssql;
            if (cflParam.Type == "TransferSummaryOut")
            {

                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");
                ssql = @"AND EXISTS(
                    SELECT 1
                    FROM ""Tx_TransferOut"" Ta
                    WHERE Ta.""Status"" = 'Posted'
                    AND T0.""Id"" = Ta.""BaseEntry""
                    AND NOT EXISTS(
                        SELECT 1
                        FROM ""Tx_TransferSummaryOut"" Tx
                        INNER JOIN ""Tx_TransferSummaryOut_Ref"" Ty ON Tx.""Id"" = Ty.""Id""
                        WHERE Ty.""BaseId"" = Ta.""Id""
                        AND Tx.""Status"" != 'Cancel'
                    )
                )
                ";

                if(userId != 1)
                {
                    ssql += @"AND EXISTS(
                        SELECT 1
                        FROM ""Tm_User_Warehouse"" Tx
                        WHERE Tx.""WhsCode"" = T0.""FromWhsCode"" 
                    )";
                } 
                cflParam.SqlWhere = string.Format(ssql, hidden_CflDocId);
            }
            if (cflParam.Type == "TransferSummaryIn")
            {
                cflParam.IsMulti = Request["hidden_CflIsMulti"];
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");
                ssql = @"AND
                    T0.""Id"" NOT IN (SELECT T0_.""BaseEntry"" FROM ""Tx_TransferSummaryIn"" T0_ WHERE T0_.""Status""='Cancel' ) AND
                    T0.""Id"" IN (SELECT T0_.""BaseEntry"" FROM ""Tx_TransferSummaryOut"" T0_ WHERE T0_.""Status""='Posted' AND IFNULL(T0_.""DocEntry"",0) <> 0 )
                ";
                cflParam.SqlWhere = string.Format(ssql, hidden_CflDocId);
            }
            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflTransferRequestList" + name);
            if (viewModel == null)
            {
                viewModel = CflTransferRequest_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflTransferRequest_ParamModel cflTransferRequestParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflTransferRequest_Model.GetDataRowCount(args, userId, cflTransferRequestParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflTransferRequest_Model.GetData(args, userId, cflTransferRequestParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflTransferRequestParam);
        }

    }
}