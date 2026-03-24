using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Collections;

using Models;
using Sap.Data.Hana;
using System.Data;

using Models._CrystalReport;

using System.Web.Script.Serialization;

using Models._Ef;
using ESKA_DI.Models._EF;
using System.Globalization;

namespace Models._Utils
{



    public class Rpt
    {
        public static ReportDocument GetRptDoc(HttpRequest Request)
        {

            String ReportFile = HttpUtility.UrlDecode(Request["ReportFile"]);

            String ReportParam = HttpUtility.UrlDecode(Request["ReportParam"]);

            String DatabaseType = HttpUtility.UrlDecode(Request["DatabaseType"]);

            return GetRptDoc(ReportFile, ReportParam, DatabaseType);

        }

        public static ReportDocument GetRptDoc(HttpRequestBase Request)
        {

            String ReportFile = HttpUtility.UrlDecode(Request["ReportFile"]);

            String ReportParam = HttpUtility.UrlDecode(Request["ReportParam"]);

            String DatabaseType = HttpUtility.UrlDecode(Request["DatabaseType"]);

            return GetRptDoc(ReportFile, ReportParam, DatabaseType);

        }


        public static ReportDocument GetRptDoc(string ReportFile, string ReportParam, string DatabaseType)
        {
            ReportDocument rptDoc; 

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            List<CrystalReportParam> reportParams;
            reportParams = serializer.Deserialize<List<CrystalReportParam>>(ReportParam);

            String FileName = HttpContext.Current.Server.MapPath(ReportFile);

            if (System.IO.File.Exists(FileName))
            {
                rptDoc = Models._Utils.Rpt.Report(FileName, reportParams, DatabaseType);
                return rptDoc;
            }
            else
            {
                return null;
            }

        }

        public static DataTable GetPrintLayouts(string controllerName, int userId)
        {
            DataTable dt;
            using (var CONTEXT = new HANA_APP())
            {
                if (GeneralGetList.GetIsAdmin(CONTEXT, userId) != "Y")
                {
                    string ssql = @"SELECT T0.""Id"", T0.""LayoutName"" , T0.""OutputType""
                            FROM ""Tm_Layout"" T0  
                            INNER JOIN 
	                            (
	                            SELECT DISTINCT   T0.""Id""
	                            FROM (
		                            SELECT T0.""Id""
		                            FROM ""Tm_Layout"" T0 
		                            INNER JOIN ""Tm_Layout_User"" T1 ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                            WHERE T1.""UserId""=:p0
		                            UNION   
		                            SELECT T0.""Id""
		                            FROM ""Tm_Layout"" T0 
		                            INNER JOIN ""Tm_Layout_Role"" T1 ON T0.""Id""=T1.""Id"" 
		                            INNER JOIN ""Tm_User"" T2 ON T1.""RoleId""=T2.""RoleId""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                            WHERE T2.""Id""=:p1
	                            ) T0 
	                            ) T1 ON T0.""Id""=T1.""Id""
                            WHERE T0.""LayoutFormCode""=:p2 AND T0.""IsActive""='Y' ";

                    dt = GeneralGetList.GetDataTable(CONTEXT, ssql, userId, userId, controllerName);
                }
                else
                {
                    string ssql = @"SELECT T0.""Id"", T0.""LayoutName"" , T0.""OutputType""
                                FROM ""Tm_Layout"" T0 
                                WHERE T0.""LayoutFormCode""=:p0 AND T0.""IsActive""='Y' ";


                    dt = GeneralGetList.GetDataTable(CONTEXT, ssql, controllerName);
                }
            }
            return dt;

        }

        public static bool GetAuthLayout(int userId, int Layout_Id)
        {
            string ssql = @"
	                            SELECT DISTINCT   T0.""Id""
	                            FROM (
		                            SELECT T0.""Id""
		                            FROM ""Tm_Layout"" T0 
		                            INNER JOIN ""Tm_Layout_User"" T1 ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                            WHERE T0.""Id""=:p0 AND T1.""UserId""=:p2 AND  T0.""IsActive""='Y'
		                            UNION   
		                            SELECT T0.""Id""
		                            FROM ""Tm_Layout"" T0 
		                            INNER JOIN ""Tm_Layout_Role"" T1 ON T0.""Id""=T1.""Id"" 
		                            INNER JOIN ""Tm_User"" T2 ON T1.""RoleId""=T2.""RoleId""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                            WHERE T0.""Id""=:p1 AND T2.""Id""=:p3  AND  T0.""IsActive""='Y'
	                            ) T0   ";
            int? id;
            using (var CONTEXT = new HANA_APP())
            {
                id = CONTEXT.Database.SqlQuery<int?>(ssql, Layout_Id, Layout_Id, userId, userId).FirstOrDefault();

            }

            if (id.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool GetAuthreport(int userId, int Report_Id)
        {
            string ssql = @" 
	                            SELECT DISTINCT   T0.""Id""
	                            FROM (
		                            SELECT T0.""Id""
		                            FROM ""Tm_Report"" T0   
		                            INNER JOIN ""Tm_Report_User"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                            WHERE T0.""Id""=:p0 AND T1.""UserId""=:p2 AND T0.""IsActive""='Y'
		                            UNION   
		                            SELECT T0.""Id""
		                            FROM ""Tm_Report"" T0   
		                            INNER JOIN ""Tm_Report_Role"" T1   ON T0.""Id""=T1.""Id""  AND IFNULL(T1.""IsTick"",'N')='Y'
		                            INNER JOIN ""Tm_User"" T2 ON T1.""RoleId""=T2.""RoleId"" 
		                            WHERE T0.""Id""=:p1 AND T2.""Id""=:p3 AND T0.""IsActive""='Y' 
	                            ) T0  
                                 ";
            int? id;
            using (var CONTEXT = new HANA_APP())
            {
                id = CONTEXT.Database.SqlQuery<int?>(ssql, Report_Id, Report_Id, userId, userId).FirstOrDefault();
            }

            if (id.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        //- ini yg di pakai: tiap menampilkan laporan/layout tidak bikin koneksi baru
        //- logic-nya dengan melempar datatable ke datasource rpt (setDataSource)
        //- cukup melempar parameter saja, sp nya akan di cari otomatis di properti dalam rpt
        //- dengan melempar datasource maka parameter sp (@) akan diabaikan otomatis: sehingga perlu membuat parameter baru lagi yg sama dengan parameter sp.
        //- parameter baru : nama sama(tanpa @), type obtional parameter true

        public static ReportDocument Report(string pstrPath, List<CrystalReportParam> crtParams, string DatabaseType)
        {
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(pstrPath);

            //return ReportByNewConn(cryRpt, crtParams);

            //exit

            if (cryRpt.Database.Tables.Count <= 0)
            {
                return cryRpt;
            }

            if (cryRpt.Database.Tables.Count > 1)
            {
                return ReportByNewConn(cryRpt, crtParams);
            }

            if (cryRpt.Subreports.Count > 1)
            {
                return ReportByNewConn(cryRpt, crtParams);
            }

            if (cryRpt.Database.Tables.Count == 1)
            {
                string className = "";
                string commandSql = "";
                foreach (dynamic table in cryRpt.ReportClientDocument.DatabaseController.Database.Tables)
                {
                    className = table.ClassName;
                    if (className == "CrystalReports.CommandTable")
                    {
                        commandSql = table.CommandText;
                    }

                }


                if (!((className == "CrystalReports.CommandTable") || (className == "CrystalReports.Procedure")))
                {
                    return ReportByNewConn(cryRpt, crtParams);
                }


                if (className == "CrystalReports.Procedure")
                {
                    string dbName = "";
                    if (DatabaseType == "Sap")
                    {
                        dbName = "\"" + DbProvider.dbSap_Name.ToString() + "\".";
                    } 

                    commandSql = dbName + "\"" + cryRpt.Database.Tables[0].LogOnInfo.TableName.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[0] + "\"";
                }


                using (var CONTEXT = new HANA_APP())
                {
                    HanaConnection HanaConn = (HanaConnection)CONTEXT.Database.Connection;
                    if (HanaConn == null)
                    {
                        CONTEXT.Database.Connection.Open();
                        HanaConn = (HanaConnection)CONTEXT.Database.Connection;
                    }


                    using (HanaCommand cmd = new HanaCommand(commandSql, HanaConn))
                    {
                        if (className == "CrystalReports.Procedure")
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                        }

                        if (crtParams != null)
                        {
                            for (int i = 0; i <= crtParams.Count - 1; i++)
                            {

                                //if (crtParams[i].ParamName.Substring(0, 1) == ":")
                                //{
                                if (className == "CrystalReports.CommandTable")
                                {
                                    commandSql = commandSql.Replace("'{?" + crtParams[i].ParamName + "}'", crtParams[i].ParamName);
                                }

                                if ((crtParams[i].ParamTypeChoose == "Multi"))
                                {

                                    //DataTable dt = new DataTable("temp");
                                    //dt.Columns.Add("ParameterValue", typeof(string));

                                    string ssql = "";
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    string[] values;
                                    if (crtParams[i].ParamValues != null)
                                    {
                                        values = serializer.Deserialize<string[]>(crtParams[i].ParamValues);
                                        foreach (var m in values)
                                        {
                                            if (string.IsNullOrEmpty(ssql))
                                            {
                                                ssql = "SELECT '" + m.Replace("'", "''") + "' AS \"ParameterValue\" FROM DUMMY ";
                                            }
                                            else
                                            {
                                                ssql = ssql + " UNION ALL SELECT '" + m.Replace("'", "''") + "' AS \"ParameterValue\" FROM DUMMY ";
                                            }
                                            //dt.Rows.Add(m);
                                        }
                                    }


                                    cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.NVarChar).Value = ssql;//dt;

                                    //var param = new SqlParameter(crtParams[i].ParamName, dt);
                                    //param.DbType = DbType.Object;
                                    //param.Direction = ParameterDirection.Input;
                                    //cmd.Parameters.Add(param);

                                }
                                else
                                {
                                    if (crtParams[i].ParamValue == null)
                                    {
                                        cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.NVarChar).Value = DBNull.Value;//crtParams[i].ParamValue;
                                    }
                                    else
                                    {
                                        if (crtParams[i].ParamTypeData == "Date")
                                        {
                                            string inputFormat = "MM/dd/yyyy hh:mm:ss";
                                            var dateTime = DateTime.ParseExact(crtParams[i].ParamValue, inputFormat, CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.TimeStamp).Value = dateTime;
                                        }
                                        else if (crtParams[i].ParamTypeData == "DateTime")
                                        {
                                            string inputFormat = "MM/dd/yyyy hh:mm:ss";
                                            var dateTime = DateTime.ParseExact(crtParams[i].ParamValue, inputFormat, CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.TimeStamp).Value = dateTime;
                                        }
                                        else if (crtParams[i].ParamTypeData == "Integer")
                                        {
                                            cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.Decimal).Value = decimal.Parse(crtParams[i].ParamValue);
                                        }
                                        else if (crtParams[i].ParamTypeData == "Amount")
                                        {
                                            cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.Decimal).Value = decimal.Parse(crtParams[i].ParamValue);
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add(crtParams[i].ParamName, HanaDbType.NVarChar).Value = crtParams[i].ParamValue;
                                        }

                                    }
                                    //var param = new SqlParameter(crtParams[i].ParamName, crtParams[i].ParamValue);
                                    //param.DbType = DbType.String;
                                    //param.Direction = ParameterDirection.Input;
                                    //cmd.Parameters.Add(param);
                                }

                                //}
                            }

                        }

                        cmd.CommandText = commandSql;
                        var adapter = new HanaDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        cryRpt.SetDataSource(ds.Tables[0]);

                    }
                }
            }

            if (crtParams != null)
            {
                for (int i = 0; i <= crtParams.Count - 1; i++)
                {
                    //if (cryRpt.ParameterFields.Contains(crtParams[i].ParamName.Replace("@", "")))
                    //{
                    //    cryRpt.SetParameterValue(crtParams[i].ParamName.Replace("@", ""), crtParams[i].ParamValue);
                    //}
                    //if (cryRpt.ParameterFields.Contains(crtParams[i].ParamName))
                    //{
                    //    cryRpt.SetParameterValue(crtParams[i].ParamName, crtParams[i].ParamValue);
                    //}


                    for (int j = 0; j <= cryRpt.ParameterFields.Count - 1; j++)
                    {

                        if (cryRpt.ParameterFields[j].Name.ToString() == "#" + crtParams[i].ParamName)
                        {
                            if (crtParams[i].ParamValue != null)
                            {
                                cryRpt.ParameterFields[j].CurrentValues.AddValue(crtParams[i].ParamValue);
                            }
                            else
                            {
                                cryRpt.ParameterFields[j].CurrentValues.IsNoValue = true;
                            }
                        }

                        if (cryRpt.ParameterFields[j].Name.ToString() == crtParams[i].ParamName)
                        {
                            cryRpt.ParameterFields[j].CurrentValues.AddValue(crtParams[i].ParamValue);

                        }
                    }


                }
            }


            return cryRpt;

        }



        //jadi di pakai meskipun dibawah :
        //ini tidak jadi dipakai karena boros koneksi, bayangkan tiap show 1 report/layout harus membuka 1 koneksi
        public static ReportDocument ReportByNewConn(ReportDocument cryRpt, List<CrystalReportParam> reportParams)
        {

            int i = 0;
            int j = 0;
            //ReportDocument cryRpt = new ReportDocument();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();


            //cryRpt.Load(pstrPath);



            if (reportParams != null)
            {
                for (i = 0; i <= reportParams.Count - 1; i++)
                {
                    for (j = 0; j <= cryRpt.ParameterFields.Count - 1; j++)
                    {

                        if (cryRpt.ParameterFields[j].Name.ToString() == reportParams[i].ParamName.ToString())
                        {
                            cryRpt.ParameterFields[j].CurrentValues.AddValue(reportParams[i].ParamValue);

                        }
                    }
                }
            }




            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder("");//CONTEXT.ConnectionString


            var _with1 = crConnectionInfo;
            _with1.ServerName = connectionString.DataSource;// "SUHUT\\SQL2014";
            _with1.DatabaseName = connectionString.InitialCatalog; // "TEST_PETAPOCO_MVC"; 


            //_with1.LogonProperties.Add("Provider", "SQLOLEDB");    

            //_with1.LogonProperties.
            if (connectionString.IntegratedSecurity == false)
            {
                _with1.IntegratedSecurity = false;
                _with1.UserID = connectionString.UserID;
                _with1.Password = connectionString.Password;
            }
            else
            {
                _with1.IntegratedSecurity = true;
            }



            crtableLogoninfo.ConnectionInfo = crConnectionInfo;


            SetConnectionInfo(cryRpt, crtableLogoninfos, crtableLogoninfo, crConnectionInfo);


            return cryRpt;

        }


        //jadi di pakai meskipun dibawah :
        //ini tidak jadi dipakai karena boros koneksi, bayangkan tiap show 1 report/layout harus membuka 1 koneksi
        public static ReportDocument ReportByNewConn(string pstrPath, List<CrystalReportParam> reportParams)
        {

            int i = 0;
            int j = 0;
            ReportDocument cryRpt = new ReportDocument();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();


            cryRpt.Load(pstrPath);



            if (reportParams != null)
            {
                for (i = 0; i <= reportParams.Count - 1; i++)
                {
                    for (j = 0; j <= cryRpt.ParameterFields.Count - 1; j++)
                    {

                        if (cryRpt.ParameterFields[j].Name.ToString() == reportParams[i].ParamName.ToString())
                        {
                            cryRpt.ParameterFields[j].CurrentValues.AddValue(reportParams[i].ParamValue);

                        }
                    }
                }
            }


            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder("");//CONTEXT.ConnectionString


            var _with1 = crConnectionInfo;
            _with1.ServerName = connectionString.DataSource;// "SUHUT\\SQL2014";
            _with1.DatabaseName = connectionString.InitialCatalog; // "TEST_PETAPOCO_MVC";



            if (connectionString.IntegratedSecurity == false)
            {
                _with1.IntegratedSecurity = false;
                _with1.UserID = connectionString.UserID;
                _with1.Password = connectionString.Password;
            }
            else
            {
                _with1.IntegratedSecurity = true;
            }


            crtableLogoninfo.ConnectionInfo = crConnectionInfo;


            SetConnectionInfo(cryRpt, crtableLogoninfos, crtableLogoninfo, crConnectionInfo);


            return cryRpt;

        }

        private static void SetConnectionInfo(ReportDocument cryRpt, TableLogOnInfos crtableLogoninfos, TableLogOnInfo crtableLogoninfo, ConnectionInfo crConnectionInfo)
        {

            int j;
            int i;
            string strTableLoc;


            for (i = 0; i <= cryRpt.Database.Tables.Count - 1; i++)
            {
                crtableLogoninfo = cryRpt.Database.Tables[i].LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                cryRpt.Database.Tables[i].ApplyLogOnInfo(crtableLogoninfo);
                strTableLoc = cryRpt.Database.Tables[i].Location.Substring(cryRpt.Database.Tables[i].Location.LastIndexOf(".") + 1);
                cryRpt.Database.Tables[i].Location = crConnectionInfo.DatabaseName + ".dbo." + strTableLoc;
            }

            for (j = 0; j <= cryRpt.Subreports.Count - 1; j++)
            {
                for (i = 0; i <= cryRpt.Subreports[j].Database.Tables.Count - 1; i++)
                {
                    crtableLogoninfo = cryRpt.Subreports[j].Database.Tables[i].LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    cryRpt.Subreports[j].Database.Tables[i].ApplyLogOnInfo(crtableLogoninfo);
                    strTableLoc = cryRpt.Database.Tables[i].Location.Substring(cryRpt.Database.Tables[i].Location.LastIndexOf(".") + 1);
                    cryRpt.Database.Tables[i].Location = crConnectionInfo.DatabaseName + ".dbo." + strTableLoc;
                }

                SetConnectionInfo(cryRpt, crtableLogoninfos, crtableLogoninfo, crConnectionInfo);

            }


        }



    }
}