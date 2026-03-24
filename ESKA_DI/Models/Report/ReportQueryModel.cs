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
using Models._Ef;
using ESKA_DI.Models._EF;



namespace Models.Report.ReportQuery
{

    #region Models

    public class ReportQueryModel_View
    {
        public int _UserId { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SortCode { get; set; }

        public int Id { get; set; }
        public string QueryName { get; set; }

    }

    public static class ReportQueryGetList
    {

        public static DataTable GetDataTable(string Sql, int userId)
        {

            var Query_Sql = Sql;

            Query_Sql = Query_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Query_Sql = Query_Sql.Replace("{UserId}", userId.ToString()); 

            var ssql = Query_Sql;

            return EfIduHanaRsExtensionsApp.IduGetDataTable(ssql);
        }


    }

    public class ReportQueryModel
    {
        public int _UserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SortCode { get; set; }

        public List<ReportQuery_QueryNameModel> ListQueryNames_ { get; set; }


    }

    public class ReportQuery_QueryNameModel
    {
        public int Id { get; set; }
        public string QueryName { get; set; }
    }


    public class ReportQuery_QueryModel
    {

        public int Id { get; set; }

        public string QueryName { get; set; }

        public string IsActive { get; set; }

        public List<ReportQuery_Query_ParamModel> ListParams_ = new List<ReportQuery_Query_ParamModel>();



    }

    public class ReportQuery_Query_ParamModel
    {

        public int? Id { get; set; }

        public int? DetId { get; set; }

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

    public class ReportQueryService
    {

        public List<ReportQueryModel> GetModels(int UserId)
        {
            string where1 = "";
            string where2 = "";
            string ssql = "";
            List<ReportQueryModel> models;

            using (var CONTEXT = new HANA_APP())
            {
                if (GeneralGetList.GetIsAdmin(CONTEXT, UserId) != "Y")
                {
                    where1 = " WHERE T1.\"UserId\"=:p0 ";
                    where2 = " WHERE T2.\"Id\"=:p1 ";
                    ssql = @"
                            SELECT T0.""Id"" AS ""GroupId"", T0.""GroupName"" AS ""GroupName"", T0.""SortCode"",
                                    T1.""Id"", T1.""QueryName"" 
                            FROM ""Tm_QueryGroup"" T0  
                            INNER JOIN 
                                (
                                    /* Query Group */
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""QueryName"" 
                                    FROM ""Tm_Query"" T0   
                                    INNER JOIN 
	                                    (
	                                    SELECT DISTINCT   T0.""Id""
	                                    FROM (
		                                        SELECT T2.""Id""
		                                        FROM ""Tm_QueryGroup"" T0   
		                                        INNER JOIN ""Tm_QueryGroup_User"" T1   ON T0.""Id""=T1.""Id""  AND ISNULL(T1.""IsTick"",'N')='Y' 
                                                INNER JOIN ""Tm_Query"" T2 ON T0.""Id""=T2.""GroupId"" 
		                                        {0} 
		                                        UNION   
		                                        SELECT T3.""Id""
		                                        FROM ""Tm_QueryGroup"" T0   
		                                        INNER JOIN ""Tm_QueryGroup_Role"" T1   ON T0.""Id""=T1.""Id""  AND ISNULL(T1.""IsTick"",'N')='Y'
		                                        INNER JOIN""Tm_User"" T2   ON T1.""RoleId""=T2.""RoleId"" 
                                                INNER JOIN ""Tm_Query"" T3 ON T0.""Id""=T3.""GroupId"" 
		                                        {1}   
	                                        ) T0 
	                                    ) T1 ON T0.""Id""=T1.""Id""
                                    WHERE T0.""IsActive""='Y'  
                                    UNION 
                                    /* Query */
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""QueryName"" 
                                    FROM ""Tm_Query"" T0   
                                    INNER JOIN 
	                                    (
	                                    SELECT DISTINCT   T0.""Id""
	                                    FROM (
		                                    SELECT T0.""Id""
		                                    FROM ""Tm_Query"" T0   
		                                    INNER JOIN ""Tm_Query_User"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                    {0} 
		                                    UNION   
		                                    SELECT T0.""Id""
		                                    FROM ""Tm_Query"" T0   
		                                    INNER JOIN ""Tm_Query_Role"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                    INNER JOIN ""Tm_User"" T2   ON T1.""RoleId""=T2.""RoleId ""
		                                    {1}   
	                                    ) T0 
	                                    ) T1 ON T0.""Id""=T1.""Id""
                                    WHERE T0.""IsActive""='Y' 
                                ) T1 ON T0.""Id""=T1.""GroupId"" 
                                ORDER BY T0.""SortCode"", T0.""Id"" ";
                    ssql = string.Format(ssql, where1, where2);

                    models = CONTEXT.Database.SqlQuery<ReportQueryModel_View>(ssql, UserId, UserId)
                                  .ToList() // Execute the query first
                                  .GroupBy(q => q.GroupId)
                                  .Select(g => new ReportQueryModel
                                  {
                                      GroupId = g.Key,
                                      GroupName = g.Select(b => b.GroupName).FirstOrDefault(),
                                      ListQueryNames_ = g.Select(b => new ReportQuery_QueryNameModel { Id = b.Id, QueryName = b.QueryName }).ToList()
                                  })
                                  .ToList();


                }
                else
                {
                    ssql = @"
                            SELECT T0.""Id"" AS ""GroupId"", T0.""GroupName"" AS ""GroupName"", T0.""SortCode"",
                                    T1.""Id"", T1.""QueryName"" 
                            FROM ""Tm_QueryGroup"" T0  
                            INNER JOIN 
                                (
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""QueryName"" 
                                    FROM ""Tm_Query"" T0   
                                    WHERE T0.""IsActive""='Y' 
                                ) T1 ON T0.""Id""=T1.""GroupId"" 
                                ORDER BY T0.""SortCode"", T0.""Id"" ";

                    models = CONTEXT.Database.SqlQuery<ReportQueryModel_View>(ssql)
                              .ToList() // Execute the query first
                              .GroupBy(q => q.GroupId)
                              .Select(g => new ReportQueryModel
                              {
                                  GroupId = g.Key,
                                  GroupName = g.Select(b => b.GroupName).FirstOrDefault(),
                                  ListQueryNames_ = g.Select(b => new ReportQuery_QueryNameModel { Id = b.Id, QueryName = b.QueryName }).ToList()
                              })
                              .ToList();



                }
            }

            return models;
        }


        public ReportQuery_QueryModel GetReportQuery_Query(int id = 0)
        {

            string ssql1 = @"SELECT T0.""Id"", T0.""QueryName"", T0.""IsActive""  
                            FROM ""Tm_Query"" T0     
                            WHERE T0.""Id""=:p0";

            ReportQuery_QueryModel reportModel;
            using (var CONTEXT = new HANA_APP())
            {
                reportModel = CONTEXT.Database.SqlQuery<ReportQuery_QueryModel>(ssql1, id).SingleOrDefault();

                if (reportModel != null)
                {
                    string ssql2 = @"SELECT T0.*  
                                FROM ""Tm_Query_Param"" T0    
                                WHERE T0.""Id""=:p0
                                ORDER BY T0.""SortCode"",T0.""DetId""
                                ";



                    var report_ParamModels = CONTEXT.Database.SqlQuery<ReportQuery_Query_ParamModel>(ssql2, id).ToList();

                    if (report_ParamModels == null)
                    {
                        report_ParamModels = new List<ReportQuery_Query_ParamModel>();
                    }

                    reportModel.ListParams_ = report_ParamModels;

                }
            }

            return reportModel;
        }



    }

    #endregion

}