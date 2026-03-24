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
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models.Report.ReportAlert
{

    public class ReportAlert_ParamModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Sql { get; set; }
    }

    public class ReportAlertDetail__List_Model
    {

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, ReportAlert_ParamModel param)
        {

            var Alert_Sql = param.Sql;

            Alert_Sql = Alert_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Alert_Sql = Alert_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Alert_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            using (var CONTEXT = new HANA_APP())
            {
                dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
            }
            e.DataRowCount = dataRowCount;
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, ReportAlert_ParamModel param)
        {


            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, param, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);
        }



        public static IEnumerable GetDataList(int userId, ReportAlert_ParamModel param, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            var Alert_Sql = param.Sql;

            Alert_Sql = Alert_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Alert_Sql = Alert_Sql.Replace("{UserId}", userId.ToString());


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

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Alert_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            //var items = CONTEXT.Database.SqlQuery<ExpandoObject>(ssql + sqlSort + ssqlLimit).ToList();
            IEnumerable<dynamic> items;
            using (var CONTEXT = new HANA_APP())
            {
                items = EfIduHanaRsExtensionsApp.GetData(CONTEXT, ssql + sqlSort + ssqlLimit);
            }
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {


            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings( int userId, ReportAlert_ParamModel param)
        {
            DataTable datatable = GetDataTable( userId, param);

            GridViewSettings settings = new GridViewSettings();
            settings.Name = param.Name;
            settings.KeyFieldName = "Key";


            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                switch (datatable.Columns[i].DataType.ToString())
                {
                    case "System.DateTime":

                        var column = settings.Columns.Add(datatable.Columns[i].ColumnName);//.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
                        column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";

                        var arr = datatable.Columns[i].ColumnName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length > 1)
                        {
                            column.Caption = arr[0];
                            if (arr[1].ToUpper() == "DATETIME")
                            {
                                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy HH:mm:ss";
                            }

                        }
                        break;

                    case "System.Decimal":
                    case "System.Double":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    default:

                        settings.Columns.Add(datatable.Columns[i].ColumnName);

                        break;
                }


            }

            settings.Columns["Key"].Visible = false;


            return settings;
        }


        //Hanya untuk mengambil column apa saya yg ada di dalam query 
        //makanya hasilnya selalu RecordCount=0

        public static DataTable GetDataTable(int userId, ReportAlert_ParamModel param)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataTable(CONTEXT,userId, param);
            }

        }


        public static DataTable GetDataTable(HANA_APP CONTEXT, int userId, ReportAlert_ParamModel param)
        {
            var Alert_Sql = param.Sql;

            Alert_Sql = Alert_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Alert_Sql = Alert_Sql.Replace("{UserId}", userId.ToString());


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Alert_Sql + ") T0  WHERE 1=0 ";

            var dataTable = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, ssql);
            //---------------------------- 

            return dataTable;

        }


    }


}