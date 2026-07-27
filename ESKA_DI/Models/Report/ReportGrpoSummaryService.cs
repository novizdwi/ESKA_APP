using System;
using System.Collections.Generic;
using System.Linq;
using Models._Ef;

namespace Models.Report
{
    public class GrpoStatusSummaryModel
    {
        public string Status { get; set; }
        public int Jumlah { get; set; }
    }

    public class ReportGrpoSummaryService
    {
        // Komposisi jumlah dokumen GRPO per Status, difilter rentang TransDate (inklusif).
        public List<GrpoStatusSummaryModel> GetStatusSummary(DateTime? dateFrom, DateTime? dateTo)
        {
            string sql = @"SELECT ""Status"", COUNT(*) AS ""Jumlah""
                            FROM ""Tx_GoodsReceiptPO""
                            WHERE 1=1 ";

            var paramList = new List<object>();
            int p = 0;

            if (dateFrom.HasValue)
            {
                sql += " AND \"TransDate\" >= :p" + p + " ";
                paramList.Add(dateFrom.Value.Date);
                p++;
            }
            if (dateTo.HasValue)
            {
                // dateTo inklusif: bandingkan < dateTo + 1 hari
                sql += " AND \"TransDate\" < :p" + p + " ";
                paramList.Add(dateTo.Value.Date.AddDays(1));
                p++;
            }

            sql += " GROUP BY \"Status\" ORDER BY \"Status\" ";

            return DbProvider.dbApp.Database.SqlQuery<GrpoStatusSummaryModel>(sql, paramList.ToArray()).ToList();
        }
    }
}
