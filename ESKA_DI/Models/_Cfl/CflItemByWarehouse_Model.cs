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
    public class CflItemByWarehouse_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflItemByWarehouse_View__
    {

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }
        
        public string UomEntry { get; set; }

        public string UomCode { get; set; }

        public decimal Quantity { get; set; }
    }

    public class CflItemByWarehouse_Model
    {


        public static string ssql = @"SELECT 
                                        T0.""ItemCode"", T0.""ItemName"", T1.""WhsCode"", T2.""WhsName"", 
                                        T1.""OnHand"" AS ""Quantity"", T0.""IUoMEntry"" AS ""UomEntry"",
                                        T3.""UomCode"", T3.""UomName"" 
                                        FROM ""{DbSap}"".""OITM"" T0 
                                        LEFT JOIN ""{DbSap}"".""OITW"" T1 ON T1.""ItemCode"" = T0.""ItemCode""
                                        LEFT JOIN ""{DbSap}"".""OWHS"" T2 ON T2.""WhsCode"" = T1.""WhsCode""
                                        LEFT JOIN ""{DbSap}"".""OUOM"" T3 ON T3.""UomEntry"" = T0.""IUoMEntry""
                                        WHERE T0.""frozenFor"" = 'N'
                                        ORDER BY T0.""ItemCode"", T1.""WhsCode""
                                    ";


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflItemByWarehouse_ParamModel cflItemParam)
        {

            var Cfl_Sql = CflItemByWarehouse_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflItemParam.SqlWhere != "")
            {
                sqlCriteria = cflItemParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflItemByWarehouse_ParamModel cflItemParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflItemParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflItemByWarehouse_View__> GetDataList(int userId, CflItemByWarehouse_ParamModel cflItemParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflItemParam.SqlWhere != "")
            {
                sqlCriteria = cflItemParam.SqlWhere + sqlCriteria;
            }

              

            var CflItemByWarehouses_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflItemByWarehouses_.Count == 0)
            {
                CflItemByWarehouse_View__ item = new CflItemByWarehouse_View__();
                CflItemByWarehouses_.Add(item);
            }


            return CflItemByWarehouses_;

        }

        public static List<CflItemByWarehouse_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflItemByWarehouse_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
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

            var items = DbProvider.dbApp.Database.SqlQuery<CflItemByWarehouse_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflItemByWarehouse_ParamModel cflItemParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Item";

            if (cflItemParam.Header != "")
            {
                settings.Name = "List Item " + cflItemParam.Header;
            }

            settings.KeyFieldName = "ItemCode";
            settings.Columns.Add("ItemName");
            return settings;
        }


    }

}