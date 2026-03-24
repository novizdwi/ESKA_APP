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
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models.Setting.Approval
{

    public class ApprovalView__
    {
        public int Id { get; set; }

        public string TransType { get; set; }

        public string FormCode { get; set; }

        public string ApprovalName { get; set; }

        public string IsActive { get; set; }
    }

    public class Approval__List_Model
    {
        private static string ViewSql = "SELECT * FROM \"Tm_Approval\" ";

        public static void SetBindingData(GridViewModel state)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var dataRowCount = GetRowCount(sqlCriteria);
                var dataList = GetDataList(sqlCriteria, sqlSort, state.Pager.PageIndex, state.Pager.PageSize);

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


        public static void GetData(GridViewCustomBindingGetDataArgs e, List<ApprovalView__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(string sqlCriteria)
        {
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND  " + sqlCriteria;
            }

            int dataRowCount = 0;
            string ssql = " ";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            using (var CONTEXT = new HANA_APP())
            {
                dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
            }

            return dataRowCount;
        }


        public static List<ApprovalView__> GetDataList(string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (sqlSort == "")
            {
                sqlSort = " ORDER BY (SELECT NULL) ";
            }

            List<ApprovalView__> views;
            string ssql = "";
            ssql = "SELECT T0.* FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            using (var CONTEXT = new HANA_APP())
            {
                views = CONTEXT.Database.SqlQuery<ApprovalView__>(ssql + sqlSort + ssqlLimit).ToList();
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
            settings.Name = "List Approval";
            settings.KeyFieldName = "Tm_Approval___.Id";
            settings.Columns.Add("Tm_Approval___.Id", "Id");
            settings.Columns.Add("Tm_Approval___.ApprovalName", "Approval Name"); 

            return settings;

        }

    }


}