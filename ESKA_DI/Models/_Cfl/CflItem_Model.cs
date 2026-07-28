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
    public class CflItem_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
        public string SourceType { get; set; } = "";//"" = master item; "PO" = ambil item dari PO Open (bisa dikembangkan: "ARInvoice","Delivery")
        public string SourceDocEntry { get; set; } = "";//DocEntry dokumen sumber (mis. PO) yang diambil otomatis dari form

    }


    public class CflItem_View__
    {

        public string ItemCode { get; set; }

        public string ItemName { get; set; }
        
        public string IUoMEntry { get; set; }

        public string UomCode { get; set; }

        public string U_IDU_RoutingGroup { get; set; }

        public string ManBtchNum { get; set; }

        // Diisi hanya saat sumber PO (ssqlPo); dipakai handler choose untuk baris detail GRPO
        public int? LineNum { get; set; }      // -> BaseLine

        public decimal? OpenQty { get; set; }  // -> Quantity (qty sisa PO)

        public string WhsCode { get; set; }    // -> Whse

        public decimal? UnitPrice { get; set; } // -> UnitPrice (harga baris PO)
    }

    public class CflItem_Model
    {


        public static string ssql = @"SELECT T0.""ItemCode"",  T0.""ItemName"" ,  T0.""ItemType"" , T0.""IUoMEntry"" , T2.""UomCode"", T1.""ItmsGrpNam"", T0.""ManBtchNum"",
                                    T0.""ItmsGrpCod"",  T0.""U_IDU_ProcessCard"", T0.""U_IDU_RoutingGroup"",
                                    CAST(NULL AS INTEGER) AS ""LineNum"", CAST(NULL AS DECIMAL) AS ""OpenQty"", CAST(NULL AS NVARCHAR(8)) AS ""WhsCode"", CAST(NULL AS DECIMAL) AS ""UnitPrice""
                                    FROM  ""{DbSap}"".""OITM"" T0
                                    LEFT OUTER JOIN ""{DbSap}"".""OITB"" T1 ON T0.""ItmsGrpCod"" = T1.""ItmsGrpCod""
                                    LEFT JOIN  ""{DbSap}"".""OUOM"" T2 ON T0.""IUoMEntry"" = T2.""UomEntry""
                                    WHERE LEFT(T0.""ItemCode"",1) !='J' AND T0.""frozenFor"" = 'N' ORDER BY T0.""ItemCode"" ASC
                                    ";

        // Dipakai saat SourceType berisi DocEntry PO: ambil item dari baris PO yang masih Open ({DocEntry}).
        // Kolom output SAMA PERSIS dengan ssql agar wrapper SELECT, kriteria filter grid, dan mapping view tetap jalan.
        public static string ssqlPo = @"
                                        SELECT
                                            T2.""ItemCode"",
                                            T2.""ItemName"",
                                            T2.""ItemType"",
                                            T2.""IUoMEntry"",
                                            T4.""UomCode"",
                                            T3.""ItmsGrpNam"",
                                            T2.""ManBtchNum"",
                                            T2.""ItmsGrpCod"",
                                            T2.""U_IDU_ProcessCard"",
                                            T2.""U_IDU_RoutingGroup"",
                                            T1.""LineNum"",
                                            T1.""Quantity"",
                                            T1.""OpenQty"",
                                            T1.""WhsCode"",
                                            T1.""Price"" AS ""UnitPrice""
                                        FROM ""{DbSap}"".""OPOR"" T0
                                        INNER JOIN ""{DbSap}"".""POR1"" T1
                                            ON T0.""DocEntry"" = T1.""DocEntry""
                                        INNER JOIN ""{DbSap}"".""OITM"" T2
                                            ON T1.""ItemCode"" = T2.""ItemCode""
                                        LEFT JOIN ""{DbSap}"".""OITB"" T3
                                            ON T2.""ItmsGrpCod"" = T3.""ItmsGrpCod""
                                        LEFT JOIN ""{DbSap}"".""OUOM"" T4
                                            ON T2.""IUoMEntry"" = T4.""UomEntry""
                                        WHERE
                                            T0.""DocStatus"" = 'O'
                                            AND T1.""LineStatus"" = 'O'
                                            AND T0.""DocEntry"" = {DocEntry}
                                            AND NOT EXISTS
                                            (
                                                SELECT 1
                                                FROM ""Tx_GoodsReceiptPO"" T5
                                                INNER JOIN ""Tx_GoodsReceiptPO_Item"" T6
                                                    ON T5.""Id"" = T6.""Id""
                                                WHERE
                                                    T5.""BaseEntry"" = T0.""DocEntry""
                                                    AND T6.""ItemCode"" = T2.""ItemCode""
                                                    AND T6.""BaseLine"" = T1.""LineNum""
                                            )
                                        ORDER BY
                                            T2.""ItemCode"" ASC
                                        ";

        // Master item + stok & gudang per baris (untuk Issue&Receipt standalone). Kolom output SAMA PERSIS
        // dengan ssql. OpenQty = OITW.OnHand (stok gudang), WhsCode = OITW.WhsCode. LEFT JOIN OITW: item
        // dengan >1 gudang muncul >1 baris; item tanpa baris OITW tetap muncul (WhsCode/OpenQty null).
        // Filter stok (mis. Issue = stok>0) dilakukan lewat SqlWhere caller: AND T0."OpenQty" > 0
        public static string ssqlStock = @"
                                        SELECT
                                            T0.""ItemCode"", T0.""ItemName"", T0.""ItemType"", T0.""IUoMEntry"", T3.""UomCode"",
                                            T2.""ItmsGrpNam"", T0.""ManBtchNum"", T0.""ItmsGrpCod"",
                                            T0.""U_IDU_ProcessCard"", T0.""U_IDU_RoutingGroup"",
                                            CAST(NULL AS INTEGER) AS ""LineNum"",
                                            T1.""OnHand"" AS ""OpenQty"",
                                            T1.""WhsCode"" AS ""WhsCode"",
                                            CAST(NULL AS DECIMAL) AS ""UnitPrice""
                                        FROM ""{DbSap}"".""OITM"" T0
                                        LEFT OUTER JOIN ""{DbSap}"".""OITW"" T1 ON T1.""ItemCode"" = T0.""ItemCode""
                                        LEFT OUTER JOIN ""{DbSap}"".""OITB"" T2 ON T0.""ItmsGrpCod"" = T2.""ItmsGrpCod""
                                        LEFT JOIN ""{DbSap}"".""OUOM"" T3 ON T0.""IUoMEntry"" = T3.""UomEntry""
                                        WHERE LEFT(T0.""ItemCode"",1) !='J' AND T0.""frozenFor"" = 'N'
                                        ORDER BY T0.""ItemCode"", T1.""WhsCode""
                                        ";

        // Pilih query dasar berdasarkan SourceType:
        //   "" (kosong)   -> master item (ssql)
        //   "PO"          -> item dari baris PO Open (ssqlPo), difilter SourceDocEntry (DocEntry PO dari form)
        //   "IssueStock"  -> master item + stok/gudang (ssqlStock); caller kirim SqlWhere stok>0 (tab Issue)
        //   "ReceiptItem" -> master item + stok/gudang (ssqlStock) tanpa filter stok (tab Receipt, pilih gudang tujuan)
        // Bisa dikembangkan ke sumber lain (ARInvoice, Delivery) dengan menambah cabang di sini.
        private static string GetBaseSql(CflItem_ParamModel cflItemParam)
        {
            string baseSql = CflItem_Model.ssql;

            if (cflItemParam != null && !string.IsNullOrEmpty(cflItemParam.SourceType))
            {
                int docEntry;
                bool hasDocEntry = int.TryParse(cflItemParam.SourceDocEntry, out docEntry);

                if (cflItemParam.SourceType == "PO" && hasDocEntry)
                {
                    baseSql = CflItem_Model.ssqlPo.Replace("{DocEntry}", docEntry.ToString());
                }
                else if (cflItemParam.SourceType == "IssueStock" || cflItemParam.SourceType == "ReceiptItem")
                {
                    baseSql = CflItem_Model.ssqlStock;
                }
            }

            baseSql = baseSql.Replace("{DbSap}", DbProvider.dbSap_Name);

            return baseSql;
        }


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflItem_ParamModel cflItemParam)
        {

            var Cfl_Sql = GetBaseSql(cflItemParam);

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflItemParam.SqlWhere != "")
            {
                sqlCriteria = cflItemParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflItem_ParamModel cflItemParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflItemParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflItem_View__> GetDataList(int userId, CflItem_ParamModel cflItemParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflItemParam.SqlWhere != "")
            {
                sqlCriteria = cflItemParam.SqlWhere + sqlCriteria;
            }

              

            var CflItems_ = GetDataList(userId, GetBaseSql(cflItemParam), sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflItems_.Count == 0)
            {
                CflItem_View__ item = new CflItem_View__();
                CflItems_.Add(item);
            }


            return CflItems_;

        }

        public static List<CflItem_View__> GetDataList(int userId, string Cfl_Sql, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflItem_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflItem_ParamModel cflItemParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Item";

            if (cflItemParam.Header != "")
            {
                settings.Name = "List Item " + cflItemParam.Header;
            }

            settings.KeyFieldName = "ItemCode";
            settings.Columns.Add("ItemName");
            return settings;
        }


    }

}