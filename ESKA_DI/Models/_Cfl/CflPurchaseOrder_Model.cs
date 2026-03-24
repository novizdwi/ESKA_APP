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
    public class CflPurchaseOrder_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class CflPurchaseOrder_View__
    {
        public int? DocEntry { get; set; }
        public string DocNum { get; set; }

        public DateTime? TransDate { get; set; }

        public string VendorCode { get; set; }
        public string VendorName { get; set; }

        public string Address { get; set; }

        public string Comments { get; set; }

    }

    public class CflPurchaseOrder_Model
    {
        public static string ssql = @"
            SELECT DISTINCT T0.""DocEntry"", T0.""DocNum"", T0.""TransDate"", T0.""VendorCode"", T0.""VendorName"", T0.""Comments""
            FROM ""Tx_PurchaseOrder"" T0
            WHERE T0.""Status"" = 'Posted'
		    AND NOT EXISTS(
			    SELECT 1
			    FROM ""Tx_GoodsReceiptPO"" Tx
			    INNER JOIN ""Tx_GoodsReceiptPO_Ref"" Ty ON Tx.""Id"" = Ty.""Id""			
			    WHERE Ty.""BaseId"" = T0.""Id""
                AND Tx.""Status"" != 'Cancel'
		    )
        ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflPurchaseOrder_ParamModel cflPurchaseOrderParam)
        {
            var Cfl_Sql = CflPurchaseOrder_Model.ssql;
            
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflPurchaseOrderParam.SqlWhere != "")
            {
                sqlCriteria = cflPurchaseOrderParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflPurchaseOrder_ParamModel cflPurchaseOrderParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);
            e.Data = GetDataList(userId, cflPurchaseOrderParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflPurchaseOrder_View__> GetDataList(int userId, CflPurchaseOrder_ParamModel cflPurchaseOrderParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflPurchaseOrderParam.SqlWhere != "")
            {
                sqlCriteria = cflPurchaseOrderParam.SqlWhere + sqlCriteria;
            }

            var CflPurchaseOrders_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflPurchaseOrders_.Count == 0)
            {
                CflPurchaseOrder_View__ item = new CflPurchaseOrder_View__();
                CflPurchaseOrders_.Add(item);
            }

            return CflPurchaseOrders_;

        }

        public static List<CflPurchaseOrder_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflPurchaseOrder_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflPurchaseOrder_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflPurchaseOrder_ParamModel cflPurchaseOrderParam)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List PurchaseOrder";

            if (cflPurchaseOrderParam.Header != "")
            {
                settings.Name = "List PurchaseOrder " + cflPurchaseOrderParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("PurchaseOrderName");
            return settings;
        }


    }

}