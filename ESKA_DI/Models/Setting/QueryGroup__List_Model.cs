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
using ESKA_DI.Models._EF;
using Models._Ef;

using System.Data.Entity;
using System.Threading.Tasks;

namespace Models.Setting.QueryGroup
{

    public class View___
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public string SortCode { get; set; }
    }

    public class QueryGroup__List_Model
    {
        static string ViewSql = "SELECT * FROM \"Tm_QueryGroup\" ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);


            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            using (var CONTEXT = new HANA_APP())
            {
                dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
            }

            e.DataRowCount = dataRowCount;
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);


            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);
        }



        public static List<View___> GetDataList(string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            var views = new List<View___>();


            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);
            using (var CONTEXT = new HANA_APP())
            {
                views = CONTEXT.Database.SqlQuery<View___>(ssql + sqlSort + ssqlLimit).ToList();
            }

            if (views.Count == 0)
            {
                View___ view = new View___();
                views.Add(view);
            }

            return views;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings()
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List QueryGroup";
            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id", "Id");
            settings.Columns.Add("GroupName", "Group Name");

            return settings;
        }

    }


}