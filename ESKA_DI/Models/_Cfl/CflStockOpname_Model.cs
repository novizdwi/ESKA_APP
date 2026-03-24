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
    public class CflStockOpname_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class CflStockOpname_View__
    {
        public string Id { get; set; }

        public int? RequestId { get; set; }
        public string RequestNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string WhsCode { get; set; }
        public string WhsName { get; set; }

        public string Comments { get; set; }
    }

    public class CflStockOpname_Model
    {
        public static string ssql = @"
            SELECT DISTINCT T0.""RequestId"" AS ""Id"", T0.""RequestId"", T0.""RequestNo"", T0.""TransDate"", T0.""Comments"", T0.""WhsCode"", T1.""WhsName""
            FROM ""Tx_StockOpname"" T0 " +
            @"LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OWHS"" T1 ON T0.""WhsCode"" = T1.""WhsCode""
            WHERE T0.""Status"" = 'Posted'
            AND NOT EXISTS(
			    SELECT 1
			    FROM ""Tx_StockSummaryOpname"" Tx
			    INNER JOIN ""Tx_StockSummaryOpname_Ref"" Ty ON Tx.""Id"" = Ty.""Id""			
			    WHERE Ty.""BaseId"" = T0.""Id""
                AND Tx.""Status"" != 'Cancel'
            )
        ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflStockOpname_ParamModel cflStockOpnameParam)
        {
            var Cfl_Sql = CflStockOpname_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflStockOpnameParam.SqlWhere != "")
            {
                sqlCriteria = cflStockOpnameParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflStockOpname_ParamModel cflStockOpnameParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);
            e.Data = GetDataList(userId, cflStockOpnameParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflStockOpname_View__> GetDataList(int userId, CflStockOpname_ParamModel cflStockOpnameParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflStockOpnameParam.SqlWhere != "")
            {
                sqlCriteria = cflStockOpnameParam.SqlWhere + sqlCriteria;
            }

            var CflStockOpnames_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflStockOpnames_.Count == 0)
            {
                CflStockOpname_View__ item = new CflStockOpname_View__();
                CflStockOpnames_.Add(item);
            }

            return CflStockOpnames_;

        }

        public static List<CflStockOpname_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflStockOpname_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflStockOpname_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflStockOpname_ParamModel cflStockOpnameParam)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List StockOpname";

            if (cflStockOpnameParam.Header != "")
            {
                settings.Name = "List StockOpname " + cflStockOpnameParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("StockOpnameName");
            return settings;
        }


    }

}