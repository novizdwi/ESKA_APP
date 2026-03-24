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

namespace Models.Setting.ApprovalStage
{

    public class ApprovalStageView__
    {
        public int Id { get; set; }

        public string TransType { get; set; }

        public string StageName { get; set; }

        public string Description { get; set; }
    }

    public class ApprovalStage__List_Model
    {
        private static string ViewSql = "SELECT * FROM \"Tm_ApprovalStage\" ";

        public static void SetBindingData(GridViewModel state)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);
            
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

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int dataRowCount)
        {
            e.DataRowCount = dataRowCount;
        }


        public static void GetData(GridViewCustomBindingGetDataArgs e, List<ApprovalStageView__> dataList)
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

        public static List<ApprovalStageView__> GetDataList(string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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


            List<ApprovalStageView__> views;
            string ssql = "";
            ssql = "SELECT T0.* FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);
            using (var CONTEXT = new HANA_APP())
            {
                views = CONTEXT.Database.SqlQuery<ApprovalStageView__>(ssql + sqlSort + ssqlLimit).ToList();
            }

            return views;

        }


        public GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;

        }


        public GridViewSettings CreateExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List ApprovalStage";
            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id", "Id");
            settings.Columns.Add("StageName", "Stage Name");

            return settings;
        }

    }


}