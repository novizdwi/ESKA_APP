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


namespace Models.Transaction.Inventory
{
    public class ListFindParamTransferSummaryIn
    {
        public bool IsFindTransDate { get; set; }
        public DateTime? TransDate_From { get; set; }
        public DateTime? TransDate_To { get; set; }
    }

    public class TransferSummaryInView___
    {
        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string BaseId { get; set; }

        public string BaseTransNo { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string TransitWhsCode { get; set; }

        public string TransitWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public DateTime? DocDate { get; set; }

        public string DocNum { get; set; }

        public string Status { get; set; }

        public string ApprovalStatus { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }

    public class TransferSummaryIn__List_Model
    {
        static string ViewSql = "SELECT *" +
                                "FROM \"Tx_TransferSummaryIn\" T0 " +
                                "ORDER BY T0.\"CreatedDate\" DESC";

        public static void SetBindingData(GridViewModel state, int userId, ListFindParamTransferSummaryIn cflParam)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryIn");
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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<TransferSummaryInView___> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, ListFindParamTransferSummaryIn param, string sqlCriteria)
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

            if (param != null)
            {
                if (param.IsFindTransDate)
                {
                    if ((param.TransDate_From != null) && (param.TransDate_To != null))
                    {
                        //ssql = ssql + " AND \"TransDate\">=:p0 AND \"TransDate\"<=:p1 ";
                        dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql, param.TransDate_From.Value.Date, param.TransDate_To.Value.Date).FirstOrDefault<int>();
                    }
                    else if (param.TransDate_From != null)
                    {
                        //ssql = ssql + " AND \"TransDate\">=:p0 ";
                        dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql, param.TransDate_From.Value.Date).FirstOrDefault<int>();
                    }
                    else if (param.TransDate_To != null)
                    {
                        //ssql = ssql + " AND \"TransDate\"<=:p0 ";
                        dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql, param.TransDate_To.Value.Date).FirstOrDefault<int>();
                    }
                }
                else
                {
                    dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
                }
            }
            else
            {
                dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();
            }

            return dataRowCount;
        }

        public static List<TransferSummaryInView___> GetDataList(HANA_APP CONTEXT, int userId, ListFindParamTransferSummaryIn param, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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
                sqlSort = " ORDER BY \"TransDate\" DESC ";
            }

            var views = new List<TransferSummaryInView___>();

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + ViewSql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);



            if (param != null)
            {
                if (param.IsFindTransDate)
                {
                    if ((param.TransDate_From != null) && (param.TransDate_To != null))
                    {
                        //ssql = ssql + " AND \"TransDate\">=:p0 AND \"TransDate\"<=:p1 ";
                        views = CONTEXT.Database.SqlQuery<TransferSummaryInView___>(ssql + sqlSort + ssqlLimit, param.TransDate_From.Value.Date, param.TransDate_To.Value.Date).ToList();
                    }
                    else if (param.TransDate_From != null)
                    {
                        //ssql = ssql + " AND \"TransDate\">=:p0 ";
                        views = CONTEXT.Database.SqlQuery<TransferSummaryInView___>(ssql + sqlSort + ssqlLimit, param.TransDate_From.Value.Date).ToList();
                    }
                    else if (param.TransDate_To != null)
                    {
                        //ssql = ssql + " AND \"TransDate\"<=:p0 ";
                        views = CONTEXT.Database.SqlQuery<TransferSummaryInView___>(ssql + sqlSort + ssqlLimit, param.TransDate_To.Value.Date).ToList();
                    }
                }
                else
                {
                    views = CONTEXT.Database.SqlQuery<TransferSummaryInView___>(ssql + sqlSort + ssqlLimit).ToList();
                }
            }
            else
            {
                views = CONTEXT.Database.SqlQuery<TransferSummaryInView___>(ssql + sqlSort + ssqlLimit).ToList();
            }

            if (views.Count == 0)
            {
                TransferSummaryInView___ view = new TransferSummaryInView___();
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
            settings.Columns.Add("VendorCode", "Vendor Code");
            settings.Columns.Add("VendorName", "Vendor Name");
            settings.Columns.Add("DocNum", "Doc Num");
            settings.Columns.Add("Status", "Status");
            return settings;
        }

    }


}