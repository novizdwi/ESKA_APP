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

namespace Models._Cfl
{
    public class CflTransferRequest_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class CflTransferRequest_View__
    {
        public string Id { get; set; }

        public string TransNo { get; set; }

        public string TransDate { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string TransitWhsCode { get; set; }

        public string TransitWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public string Comments { get; set; }


    }

    public class CflTransferRequest_Model
    {
        public static string ssql = @"
            SELECT DISTINCT T0.""Id"", 
                T0.""TransNo"", 
                T0.""TransDate"", 
                T0.""FromWhsCode"", 
                T0.""FromWhsName"", 
                IFNULL(T1.""U_IDU_WhsTransit"",'') AS ""TransitWhsCode"",
                IFNULL(T2.""WhsName"",'') AS ""TransitWhsName"",
                T0.""ToWhsCode"", T0.""ToWhsName"", 
                T0.""Comments""
            FROM ""Tx_TransferRequest"" T0
            LEFT JOIN ""{DbSap}"".""OWHS"" T1 ON T1.""WhsCode"" = T0.""FromWhsCode""
            LEFT JOIN ""{DbSap}"".""OWHS"" T2 ON T2.""WhsCode"" = IFNULL(T1.""U_IDU_WhsTransit"",'')
            WHERE T0.""Status"" = 'Posted' AND T1.""Inactive"" = 'N'
            ";


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflTransferRequest_ParamModel cflTransferRequestParam)
        {
            var Cfl_Sql = CflTransferRequest_Model.ssql;
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());
            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflTransferRequestParam.SqlWhere != "")
            {
                sqlCriteria = cflTransferRequestParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflTransferRequest_ParamModel cflTransferRequestParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);
            e.Data = GetDataList(userId, cflTransferRequestParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflTransferRequest_View__> GetDataList(int userId, CflTransferRequest_ParamModel cflTransferRequestParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflTransferRequestParam.SqlWhere != "")
            {
                sqlCriteria = cflTransferRequestParam.SqlWhere + sqlCriteria;
            }

            var CflTransferRequests_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflTransferRequests_.Count == 0)
            {
                CflTransferRequest_View__ item = new CflTransferRequest_View__();
                CflTransferRequests_.Add(item);
            }

            return CflTransferRequests_;

        }

        public static List<CflTransferRequest_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflTransferRequest_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());
            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflTransferRequest_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflTransferRequest_ParamModel cflTransferRequestParam)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List TransferRequest";

            if (cflTransferRequestParam.Header != "")
            {
                settings.Name = "List TransferRequest " + cflTransferRequestParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("TransferRequestName");
            return settings;
        }


    }

}