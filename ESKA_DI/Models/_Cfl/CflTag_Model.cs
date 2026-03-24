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
    public class CflTag_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflTag_View__
    {
        public string TagId { get; set; } 

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string Status { get; set; }

        public string Status_ { get; set; }

    }

    public class CflTag_Model
    {


        public static string ssql = @" SELECT T0.*,
            T1.""Name"" AS ""Status_""
            FROM  ""Tm_Item_Warehouse_Tag"" T0
            LEFT JOIN ""Ts_List"" T1 ON T0.""Status"" = T1.""Code"" AND T1.""Type"" = 'RFIDStatus'
        ";


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflTag_ParamModel cflTagParam)
        {

            var Cfl_Sql = CflTag_Model.ssql;

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

            if (cflTagParam.SqlWhere != "")
            {
                sqlCriteria = cflTagParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflTag_ParamModel cflTagParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflTagParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflTag_View__> GetDataList(int userId, CflTag_ParamModel cflTagParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflTagParam.SqlWhere != "")
            {
                sqlCriteria = cflTagParam.SqlWhere + sqlCriteria;
            }

              

            var CflTags_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflTags_.Count == 0)
            {
                CflTag_View__ tag = new CflTag_View__();
                CflTags_.Add(tag);
            }


            return CflTags_;

        }

        public static List<CflTag_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflTag_Model.ssql;

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

            var tags = DbProvider.dbApp.Database.SqlQuery<CflTag_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return tags;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflTag_ParamModel cflTagParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Tag";

            if (cflTagParam.Header != "")
            {
                settings.Name = "List Tag " + cflTagParam.Header;
            }

            settings.KeyFieldName = "TagId";
            settings.Columns.Add("TagId");
            return settings;
        }


    }

}