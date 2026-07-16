using DevExpress.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Models._Ef;
using Models._Utils;

namespace Models.Transaction
{
    public class IssueAndReceiptWorkOrder_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
    }

    public class IssueAndReceiptWorkOrder_View__
    {
        public int? BaseEntry { get; set; }
        public int? BaseDocNum { get; set; }
        public string BaseProcessCardTransNo { get; set; }
        public long? BaseProcessCardId { get; set; }
        public int? BaseProcessCardSort { get; set; }
        public string BaseProcessCardRoutingCode { get; set; }
        public string BaseProcessCardRoutingName { get; set; }
        public int? BaseProcessCardOperatorId { get; set; }
        public string BaseProcessCardOperatorName { get; set; }
    }

    public class IssueAndReceiptWorkOrder_Model
    {
        // Work Order (OWOR) yang masih Open, satu baris per Route Stage (Tx_ProcessCard_Detail."Sort").
        // Status OWOR: P=Planned, R=Released (Open); L=Closed, C=Cancelled.
        public static string ssql = @"
                                    SELECT
                                        T2.""DocEntry"" AS ""BaseEntry"",
                                        T2.""DocNum"" AS ""BaseDocNum"",
                                        T0.""TransNo"" AS ""BaseProcessCardTransNo"",
                                        T0.""Id"" AS ""BaseProcessCardId"",
                                        T1.""Sort"" AS ""BaseProcessCardSort"",
                                        T1.""RoutingCode"" AS ""BaseProcessCardRoutingCode"",
                                        T1.""RoutingName"" AS ""BaseProcessCardRoutingName"",
                                        T1.""OperatorId"" AS ""BaseProcessCardOperatorId"",
                                        T1.""OperatorName"" AS ""BaseProcessCardOperatorName""
                                    FROM ""Tx_ProcessCard"" T0
                                    INNER JOIN ""Tx_ProcessCard_Detail"" T1
                                        ON T1.""Id"" = T0.""Id""
                                    INNER JOIN ""{DbSap}"".""OWOR"" T2
                                        ON T2.""U_IDU_WebId"" = T0.""Id""
                                        AND T2.""U_IDU_WebTransNo"" = T0.""TransNo""
                                        AND T2.""U_IDU_RoutingLevel"" = T1.""Sort""
                                    WHERE T2.""Status"" IN ('P','R')
                                    ORDER BY T0.""TransNo"" ASC, T1.""Sort"" ASC
                                ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, IssueAndReceiptWorkOrder_ParamModel workOrderParam)
        {
            var Wo_Sql = IssueAndReceiptWorkOrder_Model.ssql;

            Wo_Sql = Wo_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Wo_Sql = Wo_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (workOrderParam != null && !string.IsNullOrEmpty(workOrderParam.SqlWhere))
            {
                sqlCriteria = workOrderParam.SqlWhere + sqlCriteria;
            }

            string ssql = "SELECT COUNT(*) AS IDU FROM (" + Wo_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            e.DataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, IssueAndReceiptWorkOrder_ParamModel workOrderParam)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);
            e.Data = GetDataList(userId, workOrderParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);
        }

        public static List<IssueAndReceiptWorkOrder_View__> GetDataList(int userId, IssueAndReceiptWorkOrder_ParamModel workOrderParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (workOrderParam != null && !string.IsNullOrEmpty(workOrderParam.SqlWhere))
            {
                sqlCriteria = workOrderParam.SqlWhere + sqlCriteria;
            }

            var workOrders_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (workOrders_.Count == 0)
            {
                IssueAndReceiptWorkOrder_View__ item = new IssueAndReceiptWorkOrder_View__();
                workOrders_.Add(item);
            }

            return workOrders_;
        }

        public static List<IssueAndReceiptWorkOrder_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            var Wo_Sql = IssueAndReceiptWorkOrder_Model.ssql;

            Wo_Sql = Wo_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Wo_Sql = Wo_Sql.Replace("{UserId}", userId.ToString());

            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }

            string ssql = "SELECT T0.* FROM (" + Wo_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<IssueAndReceiptWorkOrder_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;
        }

        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;
        }
    }
}
