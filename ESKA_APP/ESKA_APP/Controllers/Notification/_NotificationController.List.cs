using DevExpress.Web.Mvc;
using Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESKA_APP.Controllers.Notification
{
    public class _NotificationController : Controller
    {
        string VIEW_LIST_PARTIAL = "~/Views/Notification/Partial/_Notification_List_Partial.cshtml";
        string VIEW_PANEL_LIST_PARTIAL = "~/Views/Notification/Partial/_Notification_Panel_List_Partial.cshtml";

       

        public Notification_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new Notification_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            //if (cflParam.Type == "TransferSummaryIn")
            //{
            //    cflParam.SqlWhere = string.Format(@"                 
            //    AND EXISTS(
            //        SELECT 1
            //        FROM ""Tx_TransferIn"" Ta
            //        WHERE Ta.""Status"" = 'Posted'
            //        AND T0.""Id"" = Ta.""BaseEntry""
            //        AND COALESCE(Ta.""BaseEntry"", 0) != 0
            //        AND NOT EXISTS(
            //            SELECT 1
            //            FROM ""Tx_TransferSummaryIn"" Tx
            //            INNER JOIN ""Tx_TransferSummaryIn_Ref"" Ty ON Tx.""Id"" = Ty.""Id""
            //            WHERE Ty.""BaseId"" = Ta.""Id""
            //            AND Tx.""Status"" != 'Cancel'
            //        )
            //   ) ");
            //}

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var notificationParam = GetParam(Request);

            var viewModel = GetListModel(notificationParam.Name);
            ProcessCustomBinding(userId, notificationParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var notificationParam = GetParam(Request);

            var viewModel = GetListModel(notificationParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, notificationParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var notificationParam = GetParam(Request);

            var viewModel = GetListModel(notificationParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, notificationParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var notificationParam = GetParam(Request);

            var viewModel = GetListModel(notificationParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, notificationParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvNotificationList" + name);
            if (viewModel == null)
            {
                viewModel = Notification_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, Notification_ParamModel notificationParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  Notification_Model.GetDataRowCount(args, userId, notificationParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  Notification_Model.GetData(args, userId, notificationParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var notificationParam = GetParam(Request);

            var viewModel = GetListModel(notificationParam.Name);
            ProcessCustomBinding(userId, notificationParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, notificationParam);
        }

        public ActionResult GetUnreadCount()
        {
            Notification_Model notification_Model =  new Notification_Model();
            if (Session["userId"] == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            int userId = Convert.ToInt32(Session["userId"]);

            int count = notification_Model.GetDataRowCount(userId);

            return Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}