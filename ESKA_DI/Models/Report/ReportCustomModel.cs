using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using Models._Utils;
using Models._CrystalReport;
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models.Report.ReportCustom
{

    #region Models


    public static class ReportCustomGetList
    {

        public static DataTable GetDataTable(string Sql, int userId)
        {

            var Query_Sql = Sql;

            Query_Sql = Query_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Query_Sql = Query_Sql.Replace("{UserId}", userId.ToString());

            return GeneralGetList.GetDataTable(Query_Sql);
        }


    }


    public class ReportCustomModel_View
    {
        public int _UserId { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SortCode { get; set; }

        public int Id { get; set; }
        public string ReportName { get; set; }

    }

    public class ReportCustomModel
    {
        public int _UserId { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SortCode { get; set; }

        public List<ReportCustom_ReportNameModel> ListReportNames_ { get; set; }

    }

    public class ReportCustom_ReportNameModel
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
    }

    public class ReportCustom_ReportModel
    {

        public int Id { get; set; }

        public string ReportName { get; set; }

        public string Description { get; set; }

        public Guid? Uid { get; set; }

        public string OutputType { get; set; }

        public string IsActive { get; set; }

        public List<ReportCustom_Report_ParamModel> ListParams_ = new List<ReportCustom_Report_ParamModel>();



    }


    public class ReportCustom_Report_ParamModel
    {

        public int? Id { get; set; }

        public int? DetId { get; set; }

        public string SortCode { get; set; }

        public string ParamName { get; set; }

        public string IsMandatory { get; set; }

        public string IsHide { get; set; }

        public string TypeData { get; set; }

        public string Caption { get; set; }

        public string DefaultValue { get; set; }

        public string Sql { get; set; }

        public string TypeControl { get; set; }

        public string TypeChoose { get; set; }
    }

    #endregion

    #region Services

    public class ReportCustomService
    {


        public List<ReportCustomModel> GetModels(int UserId)
        {
            string where1 = "";
            string where2 = "";
            string ssql = "";

            List<ReportCustomModel> models;

            using (var CONTEXT = new HANA_APP())
            {
                if (GeneralGetList.GetIsAdmin(CONTEXT, UserId) != "Y")
                {
                    where1 = string.Format(" WHERE T1.\"UserId\"={0} ", UserId);
                    where2 = string.Format(" WHERE T2.\"Id\"={0} ", UserId);
                    ssql = @"
                            SELECT T0.""Id"" AS ""GroupId"", T0.""GroupName"" AS ""GroupName"", T0.""SortCode"",
                                    T1.""Id"", T1.""ReportName"" 
                            FROM ""Tm_ReportGroup"" T0  
                            INNER JOIN 
                                (
                                    /* Report Group */
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""ReportName"" 
                                    FROM ""Tm_Report"" T0   
                                    INNER JOIN 
	                                    (
	                                    SELECT DISTINCT   T0.""Id""
	                                    FROM (
		                                        SELECT T2.""Id""
		                                        FROM ""Tm_ReportGroup"" T0   
		                                        INNER JOIN ""Tm_ReportGroup_User"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y' 
                                                INNER JOIN ""Tm_Report"" T2 ON T0.""Id""=T2.""GroupId"" 
		                                        {0} 
		                                        UNION   
		                                        SELECT T3.""Id""
		                                        FROM ""Tm_ReportGroup"" T0   
		                                        INNER JOIN ""Tm_ReportGroup_Role"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                        INNER JOIN""Tm_User"" T2   ON T1.""RoleId""=T2.""RoleId"" 
                                                INNER JOIN ""Tm_Report"" T3 ON T0.""Id""=T3.""GroupId"" 
		                                        {1}   
	                                        ) T0 
	                                    ) T1 ON T0.""Id""=T1.""Id""
                                    WHERE T0.""IsActive""='Y'  
                                    UNION 
                                    /* Report */
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""ReportName"" 
                                    FROM ""Tm_Report"" T0   
                                    INNER JOIN 
	                                    (
	                                    SELECT DISTINCT   T0.""Id""
	                                    FROM (
		                                    SELECT T0.""Id""
		                                    FROM ""Tm_Report"" T0   
		                                    INNER JOIN ""Tm_Report_User"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                    {0} 
		                                    UNION   
		                                    SELECT T0.""Id""
		                                    FROM ""Tm_Report"" T0   
		                                    INNER JOIN ""Tm_Report_Role"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                    INNER JOIN ""Tm_User"" T2   ON T1.""RoleId""=T2.""RoleId""
		                                    {1}   
	                                    ) T0 
	                                    ) T1 ON T0.""Id""=T1.""Id""
                                    WHERE T0.""IsActive""='Y' 
                                ) T1 ON T0.""Id""=T1.""GroupId"" 
                                ORDER BY T0.""SortCode"", T0.""Id"" ";
                    ssql = string.Format(ssql, where1, where2);

                    models = CONTEXT.Database.SqlQuery<ReportCustomModel_View>(ssql)
                                  .ToList() // Execute the query first
                                  .GroupBy(q => q.GroupId)
                                  .Select(g => new ReportCustomModel
                                  {
                                      GroupId = g.Key,
                                      GroupName = g.Select(b => b.GroupName).FirstOrDefault(),
                                      ListReportNames_ = g.Select(b => new ReportCustom_ReportNameModel { Id = b.Id, ReportName = b.ReportName }).ToList()
                                  })
                                  .ToList();


                }
                else
                {
                    ssql = @"
                            SELECT T0.""Id"" AS ""GroupId"", T0.""GroupName"" AS ""GroupName"", T0.""SortCode"",
                                    T1.""Id"", T1.""ReportName"" 
                            FROM ""Tm_ReportGroup"" T0  
                            INNER JOIN 
                                (
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""ReportName"" 
                                    FROM ""Tm_Report"" T0   
                                    WHERE T0.""IsActive""='Y' 
                                ) T1 ON T0.""Id""=T1.""GroupId"" 
                                ORDER BY T0.""SortCode"", T0.""Id"" ";

                    models = CONTEXT.Database.SqlQuery<ReportCustomModel_View>(ssql)
                              .ToList() // Execute the query first
                              .GroupBy(q => q.GroupId)
                              .Select(g => new ReportCustomModel
                              {
                                  GroupId = g.Key,
                                  GroupName = g.Select(b => b.GroupName).FirstOrDefault(),
                                  ListReportNames_ = g.Select(b => new ReportCustom_ReportNameModel { Id = b.Id, ReportName = b.ReportName }).ToList()
                              })
                              .ToList();



                }

            }
            return models;
        }


        public ReportCustom_ReportModel GetReportCustom_Report(int id = 0)
        {

            string ssql1 = @"SELECT T0.""Id"", T0.""ReportName"", T0.""Description"", T0.""Uid"", T0.""OutputType"", T0.""IsActive""  
                            FROM ""Tm_Report"" T0     
                            WHERE T0.""Id""=:p0";

            ReportCustom_ReportModel reportModel;
            using (var CONTEXT = new HANA_APP())
            {
                reportModel = CONTEXT.Database.SqlQuery<ReportCustom_ReportModel>(ssql1, id).SingleOrDefault();

                if (reportModel != null)
                {
                    string ssql2 = @"SELECT T0.*  
                                FROM ""Tm_Report_Param"" T0    
                                WHERE T0.""Id""=:p0
                                ORDER BY T0.""SortCode"",T0.""DetId""
                                ";



                    var report_ParamModels = CONTEXT.Database.SqlQuery<ReportCustom_Report_ParamModel>(ssql2, id).ToList();

                    if (report_ParamModels == null)
                    {
                        report_ParamModels = new List<ReportCustom_Report_ParamModel>();
                    }

                    reportModel.ListParams_ = report_ParamModels;

                }
            }

            return reportModel;
        }


    }

    #endregion

}