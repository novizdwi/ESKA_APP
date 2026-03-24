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
using Models._CrystalReport;
using System.Web.Script.Serialization;

namespace Models.Report.ReportQuery
{

    public class ReportQuery_ParamModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Sql { get; set; }
        public List<CrystalReportParam> crtParams = new List<CrystalReportParam>();

        public int RowCount { get; set; }
    }

    public class ReportQueryDetail__List_Model
    {

        public static void SetBindingData(GridViewModel state, int userId, ReportQuery_ParamModel param)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var dataRowCount = GetRowCount(CONTEXT, userId, param, sqlCriteria);
                var dataList = GetDataList(CONTEXT, userId, param, sqlCriteria, sqlSort, state.Pager.PageIndex, state.Pager.PageSize);

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

        public static int GetRowCount(HANA_APP CONTEXT, int userId, ReportQuery_ParamModel param, string sqlCriteria)
        {

            var Query_Sql = param.Sql;

            Query_Sql = Query_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Query_Sql = Query_Sql.Replace("{UserId}", userId.ToString());



            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }


            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (param.crtParams != null)
            {
                for (int i = 0; i < param.crtParams.Count; i++)
                {
                    if (param.crtParams[i].ParamTypeChoose == "Multi")
                    {
                        string strMulti = "";

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string[] values;
                        if (param.crtParams[i].ParamValues != null)
                        {
                            values = serializer.Deserialize<string[]>(param.crtParams[i].ParamValues);
                            foreach (var m in values)
                            {
                                string temp01 = "";

                                if ((param.crtParams[i].ParamTypeData == "Integer") || (param.crtParams[i].ParamTypeData == "Amount"))
                                {
                                    temp01 = m;
                                }
                                else
                                {
                                    temp01 = "'" + m.Replace("'", "''") + "'";
                                }

                                if (string.IsNullOrEmpty(strMulti))
                                {
                                    strMulti = temp01;
                                }
                                else
                                {
                                    strMulti = strMulti + "," + temp01;
                                }

                            }
                        }
                        else
                        {
                            strMulti = "NULL";
                        }
                        Query_Sql = Query_Sql.Replace("{" + param.crtParams[i].ParamName + "}", strMulti);
                    }
                    else
                    {
                        string temp01 = "";

                        if ((param.crtParams[i].ParamTypeData == "Integer") || (param.crtParams[i].ParamTypeData == "Amount"))
                        {
                            temp01 = param.crtParams[i].ParamValue;
                        }
                        else
                        {
                            temp01 = "'" + param.crtParams[i].ParamValue.Replace("'", "''") + "'";
                        }

                        Query_Sql = Query_Sql.Replace("{" + param.crtParams[i].ParamName + "}", temp01);
                    }
                }
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Query_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            param.RowCount = dataRowCount;

            return dataRowCount;
        }


        public static IEnumerable GetDataList(int userId, ReportQuery_ParamModel param, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        { 
            return GetDataList(DbProvider.dbApp, userId, param, sqlCriteria, sqlSort, PageIndex, PageSize);

            
        }


        public static IEnumerable GetDataList(HANA_APP CONTEXT, int userId, ReportQuery_ParamModel param, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            var Query_Sql = param.Sql;

            Query_Sql = Query_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Query_Sql = Query_Sql.Replace("{UserId}", userId.ToString());


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

            if (param.crtParams != null)
            {
                for (int i = 0; i < param.crtParams.Count; i++)
                {
                    if (param.crtParams[i].ParamTypeChoose == "Multi")
                    {
                        string strMulti = "";

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string[] values;
                        if (param.crtParams[i].ParamValues != null)
                        {
                            values = serializer.Deserialize<string[]>(param.crtParams[i].ParamValues);
                            foreach (var m in values)
                            {
                                string temp01 = "";

                                if ((param.crtParams[i].ParamTypeData == "Integer") || (param.crtParams[i].ParamTypeData == "Amount"))
                                {
                                    temp01 = m;
                                }
                                else
                                {
                                    temp01 = "'" + m.Replace("'", "''") + "'";
                                }

                                if (string.IsNullOrEmpty(strMulti))
                                {
                                    strMulti = temp01;
                                }
                                else
                                {
                                    strMulti = strMulti + "," + temp01;
                                }

                            }
                        }
                        else
                        {
                            strMulti = "NULL";
                        }
                        Query_Sql = Query_Sql.Replace("{" + param.crtParams[i].ParamName + "}", strMulti);
                    }
                    else
                    {
                        string temp01 = "";

                        if ((param.crtParams[i].ParamTypeData == "Integer") || (param.crtParams[i].ParamTypeData == "Amount"))
                        {
                            temp01 = param.crtParams[i].ParamValue;
                        }
                        else
                        {
                            temp01 = "'" + param.crtParams[i].ParamValue.Replace("'", "''") + "'";
                        }

                        Query_Sql = Query_Sql.Replace("{" + param.crtParams[i].ParamName + "}", temp01);
                    }
                }
            }

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Query_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
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


        public static GridViewSettings CreateExportGridViewSettings(int userId, ReportQuery_ParamModel param)
        {
            DataTable datatable;
            using (var CONTEXT = new HANA_APP())
            {
                datatable = GetDataTable(CONTEXT, userId, param);
            }

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

        public static DataTable GetDataTable(int userId, ReportQuery_ParamModel param)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataTable(CONTEXT, userId, param);
            }

        }

        public static DataTable GetDataTable(HANA_APP CONTEXT, int userId, ReportQuery_ParamModel param)
        {
            var Query_Sql = param.Sql;

            Query_Sql = Query_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Query_Sql = Query_Sql.Replace("{UserId}", userId.ToString());

            if (param.crtParams != null)
            {
                for (int i = 0; i < param.crtParams.Count; i++)
                {
                    if (param.crtParams[i].ParamTypeChoose == "Multi")
                    {
                        string strMulti = "";

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string[] values;
                        if (param.crtParams[i].ParamValues != null)
                        {
                            values = serializer.Deserialize<string[]>(param.crtParams[i].ParamValues);
                            foreach (var m in values)
                            {
                                string temp01 = "";

                                if ((param.crtParams[i].ParamTypeData == "Integer") || (param.crtParams[i].ParamTypeData == "Amount"))
                                {
                                    temp01 = m;
                                }
                                else
                                {
                                    temp01 = "'" + m.Replace("'", "''") + "'";
                                }

                                if (string.IsNullOrEmpty(strMulti))
                                {
                                    strMulti = temp01;
                                }
                                else
                                {
                                    strMulti = strMulti + "," + temp01;
                                }

                            }
                        }
                        else
                        {
                            strMulti = "NULL";
                        }
                        Query_Sql = Query_Sql.Replace("{" + param.crtParams[i].ParamName + "}", strMulti);
                    }
                    else
                    {
                        string temp01 = "";

                        if ((param.crtParams[i].ParamTypeData == "Integer") || (param.crtParams[i].ParamTypeData == "Amount"))
                        {
                            temp01 = param.crtParams[i].ParamValue;
                        }
                        else
                        {
                            temp01 = "'" + param.crtParams[i].ParamValue.Replace("'", "''") + "'";
                        }

                        Query_Sql = Query_Sql.Replace("{" + param.crtParams[i].ParamName + "}", temp01);
                    }
                }
            }


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Query_Sql + ") T0  WHERE 1=0 ";

            var dataTable = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, ssql);
            //---------------------------- 

            return dataTable;

        }


    }


}