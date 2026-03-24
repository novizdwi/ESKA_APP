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



namespace Models.Report.ReportAlert
{

    #region Models

    public class ReportAlertModel_View
    {
        public int _UserId { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SortCode { get; set; }

        public int Id { get; set; }
        public string AlertName { get; set; }

    }

    public class ReportAlertModel
    {
        public int _UserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SortCode { get; set; }

        public List<ReportAlert_AlertNameModel> ListAlertNames_ { get; set; }


    }

    public class ReportAlert_AlertNameModel
    {
        public int Id { get; set; }
        public string AlertName { get; set; }
    }

    #endregion

    #region Services

    public class ReportAlertService
    {

        public List<ReportAlertModel> GetModels(int UserId)
        {
            string where1 = "";
            string where2 = "";
            string ssql = "";

            List<ReportAlertModel> models;

            using (var CONTEXT = new HANA_APP())
            {
                if (GeneralGetList.GetIsAdmin(CONTEXT,UserId) != "Y")
                {
                    where1 = " WHERE T1.\"UserId\"=:p0 ";
                    where2 = " WHERE T2.\"Id\"=:p1 ";
                    ssql = @"
                            SELECT T0.""Id"" AS ""GroupId"", T0.""GroupName"" AS ""GroupName"", T0.""SortCode"",
                                    T1.""Id"", T1.""AlertName"" 
                            FROM ""Tm_AlertGroup"" T0  
                            INNER JOIN 
                                (
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""AlertName"" 
                                    FROM ""Tm_Alert"" T0   
                                    INNER JOIN 
	                                    (
	                                    SELECT DISTINCT   T0.""Id""
	                                    FROM (
		                                    SELECT T0.""Id""
		                                    FROM ""Tm_Alert"" T0   
		                                    INNER JOIN ""Tm_Alert_User"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                    {0} 
		                                    UNION   
		                                    SELECT T0.""Id""
		                                    FROM ""Tm_Alert"" T0   
		                                    INNER JOIN ""Tm_Alert_Role"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                                    INNER JOIN ""Tm_User"" T2   ON T1.""RoleId""=T2.""RoleId ""
		                                    {1}   
	                                    ) T0 
	                                    ) T1 ON T0.""Id""=T1.""Id""
                                    WHERE T0.""IsActive""='Y' 
                                ) T1 ON T0.""Id""=T1.""GroupId"" 
                                ORDER BY T0.""SortCode"", T0.""Id"" ";
                    ssql = string.Format(ssql, where1, where2);

                    models = CONTEXT.Database.SqlQuery<ReportAlertModel_View>(ssql, UserId, UserId)
                                  .ToList() // Execute the query first
                                  .GroupBy(q => q.GroupId)
                                  .Select(g => new ReportAlertModel
                                  {
                                      GroupId = g.Key,
                                      GroupName = g.Select(b => b.GroupName).FirstOrDefault(),
                                      ListAlertNames_ = g.Select(b => new ReportAlert_AlertNameModel { Id = b.Id, AlertName = b.AlertName }).ToList()
                                  })
                                  .ToList();


                }
                else
                {
                    ssql = @"
                            SELECT T0.""Id"" AS ""GroupId"", T0.""GroupName"" AS ""GroupName"", T0.""SortCode"",
                                    T1.""Id"", T1.""AlertName"" 
                            FROM ""Tm_AlertGroup"" T0  
                            INNER JOIN 
                                (
                                    SELECT T0.""Id"", T0.""GroupId"", T0.""AlertName"" 
                                    FROM ""Tm_Alert"" T0   
                                    WHERE T0.""IsActive""='Y' 
                                ) T1 ON T0.""Id""=T1.""GroupId"" 
                                ORDER BY T0.""SortCode"", T0.""Id"" ";

                    models = CONTEXT.Database.SqlQuery<ReportAlertModel_View>(ssql)
                              .ToList() // Execute the query first
                              .GroupBy(q => q.GroupId)
                              .Select(g => new ReportAlertModel
                              {
                                  GroupId = g.Key,
                                  GroupName = g.Select(b => b.GroupName).FirstOrDefault(),
                                  ListAlertNames_ = g.Select(b => new ReportAlert_AlertNameModel { Id = b.Id, AlertName = b.AlertName }).ToList()
                              })
                              .ToList();



                }
            }
            return models;
        }


    }

    #endregion

}