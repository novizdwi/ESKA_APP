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


namespace Models.Master.Item
{
    public class ListFindParamItemMaster
    {
        public bool IsFindTransDate { get; set; }
        public DateTime? TransDate_From { get; set; }
        public DateTime? TransDate_To { get; set; }
    }

    public class ItemMasterView___
    {
        public long Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

    }

    public class ItemMaster__List_Model
    {
        static string ViewSql = "SELECT *" +
                                "FROM \"Tm_Item\" T0 " +
                                "ORDER BY T0.\"CreatedDate\" DESC";

        public static void SetBindingData(GridViewModel state, int userId, ListFindParamItemMaster cflParam)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ItemMaster");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    if (string.IsNullOrEmpty(sqlCriteria))
                    {
                        sqlCriteria = formAuthorizeSqlWhere;
                    }
                    else
                    {
                        sqlCriteria = sqlCriteria + " AND " + formAuthorizeSqlWhere;
                    }

                }

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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<ItemMasterView___> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, ListFindParamItemMaster param, string sqlCriteria)
        {

            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlCriteria != "")
            {
                //sqlCriteria = " AND ( " + sqlCriteria + " )";
                sqlCriteria = " AND  " + sqlCriteria;
            }



            int dataRowCount = 0;
            string ssql = " ";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            //if (param != null)
            //{
            //    if (param.IsFindTransDate)
            //    {
            //        if ((param.TransDate_From != null) && (param.TransDate_To != null))
            //        {
            //            //ssql = ssql + " AND \"TransDate\">=:p0 AND \"TransDate\"<=:p1 ";
            //            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql, param.TransDate_From.Value.Date, param.TransDate_To.Value.Date).FirstOrDefault<int>();
            //        }
            //        else if (param.TransDate_From != null)
            //        {
            //            //ssql = ssql + " AND \"TransDate\">=:p0 ";
            //            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql, param.TransDate_From.Value.Date).FirstOrDefault<int>();
            //        }
            //        else if (param.TransDate_To != null)
            //        {
            //            //ssql = ssql + " AND \"TransDate\"<=:p0 ";
            //            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql, param.TransDate_To.Value.Date).FirstOrDefault<int>();
            //        }
            //    }
            //    else
            //    {
            //        dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
            //    }
            //}
            //else
            //{
            //    dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
            //}

            return dataRowCount;
        }

        public static List<ItemMasterView___> GetDataList(HANA_APP CONTEXT, int userId, ListFindParamItemMaster param, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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


            //if (sqlSort == "")
            //{
            //    sqlSort = " ORDER BY \"TransDate\" DESC ";
            //}

            var views = new List<ItemMasterView___>();

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit).ToList();

            //if (param != null)
            //{
            //    views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit).ToList();

            //    if (param.IsFindTransDate)
            //    {
            //        if ((param.TransDate_From != null) && (param.TransDate_To != null))
            //        {
            //            //ssql = ssql + " AND \"TransDate\">=:p0 AND \"TransDate\"<=:p1 ";
            //            views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit, param.TransDate_From.Value.Date, param.TransDate_To.Value.Date).ToList();
            //        }
            //        else if (param.TransDate_From != null)
            //        {
            //            //ssql = ssql + " AND \"TransDate\">=:p0 ";
            //            views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit, param.TransDate_From.Value.Date).ToList();
            //        }
            //        else if (param.TransDate_To != null)
            //        {
            //            //ssql = ssql + " AND \"TransDate\"<=:p0 ";
            //            views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit, param.TransDate_To.Value.Date).ToList();
            //        }
            //    }
            //    else
            //    {
            //        views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit).ToList();
            //    }
            //}
            //else
            //{
            //    views = CONTEXT.Database.SqlQuery<ItemMasterView___>(ssql + sqlSort + ssqlLimit).ToList();
            //}

            if (views.Count == 0)
            {
                ItemMasterView___ view = new ItemMasterView___();
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
            settings.Name = "List Dokumen";

            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id").Visible = false;
            settings.Columns.Add("TransNo", "No. Dokumen");
            settings.Columns.Add("TransDate", "Trans Date");
            settings.Columns.Add("WhsCode", "Whs Code");
            settings.Columns.Add("WhsName", "Whs Name");
            settings.Columns.Add("OriginItemCode", "Origin Item Code");
            settings.Columns.Add("OriginItemName", "Origin Item Name"); 
            settings.Columns.Add("TargetItemCode", "To Item Code");
            settings.Columns.Add("TargetItemName", "To Item Name"); 
            settings.Columns.Add("Status", "Status");
            return settings;
        }

    }


}