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

namespace Models.Authentication.User
{

    public class View___
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }

    }

    public class User__List_Model
    {

        static string ViewSql = @"SELECT T0.""Id"",T0.""UserName"",T0.""FirstName"",T0.""MidleName"",T0.""LastName"",T0.""Email"",T1.""RoleName"" 
                                FROM ""Tm_User"" T0   
                                LEFT JOIN ""Tm_Role"" T1  ON T0.""RoleId""=T1.""Id""
                                "; 

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



            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }


            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);
        }

        public static List<View___> GetDataList(string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            var views = new List<View___>();

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
            settings.Name = "List User";
            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id");
            settings.Columns.Add("FirstName");
            settings.Columns.Add("MidleName");
            settings.Columns.Add("LastName");
            settings.Columns.Add("Email");
            settings.Columns.Add("RoleName");

            return settings;
        }

    }


}