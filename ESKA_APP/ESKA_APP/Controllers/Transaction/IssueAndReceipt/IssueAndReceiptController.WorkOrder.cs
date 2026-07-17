using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction;

namespace Controllers.Transaction
{
    public partial class IssueAndReceiptController : BaseController
    {
        string VIEW_WORKORDER_LIST_PARTIAL = "Partial/WorkOrder/WorkOrder_List_Partial";
        string VIEW_WORKORDER_PANEL_LIST_PARTIAL = "Partial/WorkOrder/WorkOrder_Panel_List_Partial";

        public IssueAndReceiptWorkOrder_ParamModel GetWorkOrderParam(HttpRequestBase Request)
        {
            var woParam = new IssueAndReceiptWorkOrder_ParamModel();
            woParam.Type = Request["hidden_WoType"];
            woParam.Name = Request["hidden_WoName"];
            woParam.Header = Request["hidden_WoHeader"];
            woParam.SqlWhere = Request["hidden_WoSqlWhere"];

            return woParam;
        }

        public ActionResult WorkOrderListPartial()
        {
            int userId = (int)Session["userId"];

            var woParam = GetWorkOrderParam(Request);

            var viewModel = GetWorkOrderListModel();
            ProcessWorkOrderCustomBinding(userId, woParam, viewModel);

            return PartialView(VIEW_WORKORDER_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult WorkOrderListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var woParam = GetWorkOrderParam(Request);

            var viewModel = GetWorkOrderListModel();
            viewModel.ApplyPagingState(pager);
            ProcessWorkOrderCustomBinding(userId, woParam, viewModel);

            return PartialView(VIEW_WORKORDER_LIST_PARTIAL, viewModel);
        }

        // Filtering
        public ActionResult WorkOrderListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var woParam = GetWorkOrderParam(Request);

            var viewModel = GetWorkOrderListModel();
            viewModel.ApplyFilteringState(filteringState);
            ProcessWorkOrderCustomBinding(userId, woParam, viewModel);

            return PartialView(VIEW_WORKORDER_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult WorkOrderListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var woParam = GetWorkOrderParam(Request);

            var viewModel = GetWorkOrderListModel();
            viewModel.ApplySortingState(column, reset);
            ProcessWorkOrderCustomBinding(userId, woParam, viewModel);

            return PartialView(VIEW_WORKORDER_LIST_PARTIAL, viewModel);
        }

        public ActionResult WorkOrder_PopupListOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var woParam = GetWorkOrderParam(Request);

            var viewModel = GetWorkOrderListModel();
            ProcessWorkOrderCustomBinding(userId, woParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_WORKORDER_PANEL_LIST_PARTIAL, woParam);
        }

        static GridViewModel GetWorkOrderListModel()
        {
            var viewModel = GridViewExtension.GetViewModel("gvWorkOrderList");
            if (viewModel == null)
            {
                viewModel = IssueAndReceiptWorkOrder_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessWorkOrderCustomBinding(int userId, IssueAndReceiptWorkOrder_ParamModel woParam, GridViewModel viewModel)
        {
            viewModel.ProcessCustomBinding(
                new GridViewCustomBindingGetDataRowCountHandler(args =>
                {
                    IssueAndReceiptWorkOrder_Model.GetDataRowCount(args, userId, woParam);
                }),
                new GridViewCustomBindingGetDataHandler(args =>
                {
                    IssueAndReceiptWorkOrder_Model.GetData(args, userId, woParam);
                })
            );
        }
    }
}
