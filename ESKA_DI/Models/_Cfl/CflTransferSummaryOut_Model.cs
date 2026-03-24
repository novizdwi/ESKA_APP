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
    public class CflTransferSummaryOut_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class CflTransferSummaryOut_View__
    {
        public int? Id { get; set; }
        public string TransNo { get; set; }
        public string BaseType { get; set; }

        public DateTime? TransDate { get; set; }

        public string FromWhsCode { get; set; }
        public string FromWhsName { get; set; }
        public string TransitWhsCode { get; set; }
        public string TransitWhsName { get; set; }
        public string ToWhsCode { get; set; }
        public string ToWhsName { get; set; }


    }

    public class CflTransferSummaryOut_Model
    {
        public static string ssql = @"
            SELECT DISTINCT T0.""Id"", T0.""TransNo"", T0.""TransType"" AS ""BaseType"",
                T0.""TransDate"", 
                T0.""FromWhsCode"", 
                T0.""FromWhsName"", 
                T0.""TransitWhsCode"",
                T0.""TransitWhsName"",
                T0.""ToWhsCode"", 
                T0.""ToWhsName"", 
                T0.""Comments""
            FROM ""Tx_TransferSummaryOut"" T0
            WHERE T0.""Status"" = 'Posted' 

        ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflTransferSummaryOut_ParamModel cflTransferSummaryOutParam)
        {
            var Cfl_Sql = CflTransferSummaryOut_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflTransferSummaryOutParam.SqlWhere != "")
            {
                sqlCriteria = cflTransferSummaryOutParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflTransferSummaryOut_ParamModel cflTransferSummaryOutParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);
            e.Data = GetDataList(userId, cflTransferSummaryOutParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflTransferSummaryOut_View__> GetDataList(int userId, CflTransferSummaryOut_ParamModel cflTransferSummaryOutParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflTransferSummaryOutParam.SqlWhere != "")
            {
                sqlCriteria = cflTransferSummaryOutParam.SqlWhere + sqlCriteria;
            }

            var CflTransferSummaryOuts_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflTransferSummaryOuts_.Count == 0)
            {
                CflTransferSummaryOut_View__ item = new CflTransferSummaryOut_View__();
                CflTransferSummaryOuts_.Add(item);
            }

            return CflTransferSummaryOuts_;

        }

        public static List<CflTransferSummaryOut_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflTransferSummaryOut_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflTransferSummaryOut_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflTransferSummaryOut_ParamModel cflTransferSummaryOutParam)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List TransferSummaryOut";

            if (cflTransferSummaryOutParam.Header != "")
            {
                settings.Name = "List TransferSummaryOut " + cflTransferSummaryOutParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("TransferSummaryOutName");
            return settings;
        }


    }

}