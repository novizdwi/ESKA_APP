using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Models;
using Models._Utils;
using System.Data;
using System.Dynamic;
using ESKA_DI.Models._EF;
using Models._Ef;

namespace Models.Notification
{
    public class Notification_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class Notification_View__
    {
        public int? Id { get; set; }
        public string TransType { get; set; }
        public string ObjectName { get; set; }
        public string TransNo { get; set; }
        public string Message { get; set; }
        public DateTime RequestDate { get; set; }
        public int CreatedUser { get; set; }
        public string FirstName { get; set; }

    }

    public class Notification_Model
    {
        public static string ssql = @"
            SELECT
                Tx.*,
                Ty.""FirstName"",
                Tz.""ObjectName""
            FROM(
                SELECT
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_AdjustmentIn"" T0
                INNER JOIN ""Tx_AdjustmentIn_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')
                UNION 

                SELECT 
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_AdjustmentOut"" T0
                INNER JOIN ""Tx_AdjustmentOut_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')            
                UNION

                SELECT 
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_GoodsReceiptPO"" T0
                INNER JOIN ""Tx_GoodsReceiptPO_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')            
                UNION

                 SELECT 
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_StockSummaryOpname"" T0
                INNER JOIN ""Tx_StockSummaryOpname_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')            
                UNION

                SELECT 
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_TransferRequest"" T0
                INNER JOIN ""Tx_TransferRequest_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')            
                UNION
            
                SELECT 
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_TransferSummaryOut"" T0
                INNER JOIN ""Tx_TransferSummaryOut_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')
            
                UNION
            
                SELECT 
                    T0.""Id"",
                    T0.""TransType"",
                    T0.""TransNo"" AS ""TransNo"" ,
                    T0.""ApprovalMessages"" AS ""Message"",
                    T0.""CreatedDate"" AS ""RequestDate"", 
                    T0.""CreatedUser"" AS ""CreatedUser""
                FROM ""Tx_TransferSummaryIn"" T0
                INNER JOIN ""Tx_TransferSummaryIn_Approval"" T1 ON T1.""Id"" = T0.""Id""
                WHERE T0.""ApprovalStatus"" = 'Waiting' 
                AND T1.""UserId"" = {UserId}
                AND T0.""Status"" NOT IN ('Cancel', 'Posted')
            ) Tx
            LEFT JOIN ""Tm_User"" Ty ON Tx.""CreatedUser"" = Ty.""Id""
            LEFT JOIN ""Ts_ObjectApproval"" Tz ON Tx.""TransType"" = Tz.""ObjectCode"" 
            
            ORDER BY Tx.""RequestDate"" DESC
        ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, Notification_ParamModel notificationParam)
        {
            var Cfl_Sql = Notification_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (notificationParam.SqlWhere != "")
            {
                sqlCriteria = notificationParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public int GetDataRowCount(int userId)
        {
            var Cfl_Sql = Notification_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());
                      

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " ;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            return dataRowCount;
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, Notification_ParamModel notificationParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);
            e.Data = GetDataList(userId, notificationParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<Notification_View__> GetDataList(int userId, Notification_ParamModel notificationParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (notificationParam.SqlWhere != "")
            {
                sqlCriteria = notificationParam.SqlWhere + sqlCriteria;
            }

            var Notifications_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (Notifications_.Count == 0)
            {
                Notification_View__ item = new Notification_View__();
                Notifications_.Add(item);
            }

            return Notifications_;

        }

        public static List<Notification_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = Notification_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<Notification_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(Notification_ParamModel notificationParam)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Notification";

            if (notificationParam.Header != "")
            {
                settings.Name = "List Notification " + notificationParam.Header;
            }

            settings.KeyFieldName = "TransNo";
            settings.Columns.Add("TransNo");
            return settings;
        }


    }

}