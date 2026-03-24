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


    public class CflDynamic_ParamModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Sql { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflDynamic_Model
    {

        public static void SetBindingData(GridViewModel state, int userId, CflDynamic_ParamModel cflParam)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var dataRowCount = GetRowCount(CONTEXT, userId, cflParam, sqlCriteria);
                var dataList = GetDataList(CONTEXT, userId, cflParam, sqlCriteria, sqlSort, state.Pager.PageIndex, state.Pager.PageSize);

                state.ProcessCustomBinding(
                  new GridViewCustomBindingGetDataRowCountHandler(args =>
                  {
                      GetDataRowCount(args, dataRowCount);
                  }),
                  new GridViewCustomBindingGetDataHandler(args =>
                  {
                      GetData(args, dataList);
                  })
              );
            }
        }

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int dataRowCount)
        {
            e.DataRowCount = dataRowCount;
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, IEnumerable dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflDynamic_ParamModel cflDynamicParam, string sqlCriteria)
        {

            var Cfl_Sql = cflDynamicParam.Sql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());



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
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            return dataRowCount;
        }

        public static IEnumerable GetDataList(int userId, CflDynamic_ParamModel cflDynamicParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            return GetDataList(userId, cflDynamicParam, sqlCriteria, sqlSort, PageIndex, PageSize);
        }


        public static IEnumerable GetDataList(HANA_APP CONTEXT, int userId, CflDynamic_ParamModel cflDynamicParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            var Cfl_Sql = cflDynamicParam.Sql;

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

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            //var items = CONTEXT.Database.SqlQuery<ExpandoObject>(ssql + sqlSort + ssqlLimit).ToList();
            var items = EfIduHanaRsExtensionsApp.GetData(CONTEXT, ssql + sqlSort + ssqlLimit);
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {


            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(int userId, CflDynamic_ParamModel cflDynamicParam)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CreateExportGridViewSettings(CONTEXT,userId, cflDynamicParam);
            }
        }

        public static GridViewSettings CreateExportGridViewSettings(HANA_APP CONTEXT, int userId, CflDynamic_ParamModel cflDynamicParam)
        {
            DataTable datatable = GetDataTable(CONTEXT, userId, cflDynamicParam);

            GridViewSettings settings = new GridViewSettings();
            settings.Name = cflDynamicParam.Code;
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
        public static DataTable GetDataTable(int userId, CflDynamic_ParamModel param)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataTable(CONTEXT, userId, param);
            }

        }

        public static DataTable GetDataTable(HANA_APP CONTEXT, int userId, CflDynamic_ParamModel cflDynamicParam)
        {
            var Cfl_Sql = cflDynamicParam.Sql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=0 ";

            var dataTable = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, ssql);
            //---------------------------- 

            return dataTable;

        }



    }


}