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


using Models._Sap;
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models._Utils
{
    public class GetCodeNameModel
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public static class GeneralGetList
    {
        //mendapatkan semua url : punya authorize ke form tertentu atau tidak
        public static DataTable GetMenuUrls(int userId)
        {
            string ssql = @"SELECT DISTINCT T3.""Url""
                            FROM ""Tm_User"" T0   
                            INNER JOIN ""Tm_Role"" T1   ON T0.""RoleId""=T1.""Id"" 
                            INNER JOIN ""Tm_Role_Auth"" T2   ON T1.""Id""=T2.""Id"" AND T2.""IsAccess""='Y' 
                            INNER JOIN ""Ts_Menu"" T3   ON T2.""MenuCode""=T3.""MenuCode"" AND T3.""Url"" LIKE '%/Detail' 
                            WHERE T0.""Id""=:p0";

            return EfIduHanaRsExtensionsApp.IduGetDataTable(ssql, userId);

        }

        public static string GetFormTransAuthorize(int userId, string formCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetFormTransAuthorize(CONTEXT, userId, formCode);
            }
        }

        public static string GetIsCanAccessOpeningBalance(int UserId, string MenuCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT TOP 1 COALESCE(T1.""IsAccess"",'N') 
                    FROM ""Tm_User"" T0X 
                    INNER JOIN  ""Tm_Role"" T0 ON T0.""Id"" = T0X.""RoleId""
                    INNER JOIN ""Tm_Role_Auth"" T1 ON T1.""Id"" = T0.""Id"" 
                    WHERE T0X.""Id"" = {0} AND T1.""MenuCode"" = '{1}'
                ";

                ssql = string.Format(ssql, UserId, MenuCode);
                return CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
            }
        }

        public static string GetCheckIsApproval(long Id, string ObjectCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT COALESCE(T0.""IsApproval"",'N') 
                    FROM ""Tx_"+ ObjectCode + @""" T0
                    WHERE T0.""Id"" = {0}                ";

                ssql = string.Format(ssql, Id);
                return CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
            }
        }

        public static string GetFormTransAuthorize(HANA_APP CONTEXT, int userId, string formCode)
        {

            int? roleId = GetValue<int?>(CONTEXT, "SELECT TOP 1 IFNULL(T0.\"RoleId\",0) AS \"RoleId\" FROM \"Tm_User\" T0   WHERE T0.\"Id\"=:p0", userId);
            string roleName = GetValue<string>(CONTEXT, "SELECT TOP 1 IFNULL(T0.\"RoleName\",'') AS \"RoleName\" FROM \"Tm_Role\" T0   WHERE T0.\"Id\"=:p0", roleId);

            if (GetIsAdmin(roleName) == "Y")
            {
                return "All";
            }

            //All Form
            if (GetValue<string>(CONTEXT, "SELECT T0.\"IsAccess\" FROM \"Tm_Role_Auth\" T0   WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1 ", roleId, formCode + "/Detail#All") == "Y")
            {
                return "All";
            }
            //Branch
            else if (GetValue<string>(CONTEXT, "SELECT T0.\"IsAccess\" FROM \"Tm_Role_Auth\" T0   WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1 ", roleId, formCode + "/Detail#Branch") == "Y")
            {
                return "Branch";
            }
            //User
            else if (GetValue<string>(CONTEXT, "SELECT T0.\"IsAccess\" FROM \"Tm_Role_Auth\" T0   WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1 ", roleId, formCode + "/Detail#User") == "Y")
            {
                return "User";
            }
            else
            {
                return "";
            }
        }

        public static string GetFormTransAuthorizeSqlWhere(int userId, string formCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetFormTransAuthorizeSqlWhere(CONTEXT, userId, formCode);
            }
        }

        public static string GetFormTransAuthorizeSqlWhere(HANA_APP CONTEXT, int userId, string formCode)
        {
            string formAuthorize = GetFormTransAuthorize(CONTEXT, userId, formCode); 

            string ssql = "";

            if (formAuthorize == "All")
            {
                ssql = "";
            }
            //else if (formAuthorize == "Branch")
            //{
            //    string branch = GeneralGetList.GetAuthBranchCodeByUserId(CONTEXT, userId);

            //    if (!string.IsNullOrEmpty(branch))
            //    {
            //        ssql = ssql + "  \"CreatedBranch\" IN (" + branch + ") ";
            //    }
            //    else
            //    {
            //        ssql = ssql + "  1=0 ";
            //    }
            //}
            else if (formAuthorize == "User")
            {
                ssql = ssql + "  \"CreatedUser\"=" + userId.ToString();
            }
            else
            {
                ssql = ssql + "  1=0 ";
            }

            return ssql;
        }

        public static string GetAuthBranchCodeByUserId(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAuthBranchCodeByUserId(CONTEXT, userId);
            }
        }

        public static string GetAuthBranchCodeByUserId(HANA_APP CONTEXT, int userId)
        {
//            string ssql = @" 
//	                        SELECT TOP 1 ''''||IFNULL(T0_.""WhsCode"",'')||'''' AS ""WhsCode"" 
//	                        FROM ""Tm_User"" T0_ 
//                            WHERE T0_.""Id""=:p0 ";

            string ssql = @"SELECT STRING_AGG(''''||T0.""BranchCode""||'''', ', ') AS IDU
                            FROM  ( 
	                            SELECT TOP 1 IFNULL(T0_.""WhsCode"",'') AS ""BranchCode"" 
	                            FROM ""Tm_User"" T0_ 
	                            WHERE T0_.""Id""=:p0
	                            UNION  
	                            SELECT IFNULL(T0_.""BranchCode"",'') AS ""BranchCode"" 
	                            FROM ""Tm_User_AuthBranch"" T0_ 
	                            WHERE T0_.""Id""=:p1 AND T0_.""IsTick""='Y'
                            ) T0";


            return GetValue<string>(CONTEXT, ssql, userId, userId);
        }

        public static string GetRegionalByUserId(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetRegionalByUserId(CONTEXT, userId);
            }
        }

        public static string GetRegionalByUserId(HANA_APP CONTEXT, int userId)
        {
            string ssql = @" 
	                        SELECT TOP 1 T0_.""Regional"" AS ""Regional"" 
	                        FROM ""Tm_User"" T0_ 
                            WHERE T0_.""Id""=:p0 ";

            return GetValue<string>(CONTEXT, ssql, userId);
        }



        public static DataTable GetDocumentType(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""Id"", T0.""DocumentName""  FROM ""Tm_DocContent"" T0   WHERE T0.""Type""=:p0  ";
                return GetDataTable(CONTEXT, ssql, strType);
            }
        }

        public static DataTable GetObjectApprovals()
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""ObjectCode"", T0.""ObjectName""  FROM ""Ts_ObjectApproval"" T0 ORDER BY T0.""Sort"" ASC ";
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static string GetApprovalActive(string objectCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetApprovalActive(CONTEXT, objectCode);
            }
        }

        public static string GetApprovalActive(HANA_APP CONTEXT, string objectCode)
        {
            var ssql = @"SELECT TOP 1 IFNULL(T0.""IsActive"",'N') AS ""IsActive"" 
                         FROM ""Tm_ApprovalTemplate"" T0 
                         INNER JOIN ""Tm_ApprovalTemplate_User"" T1 ON T1.""Id"" = T0.""Id""
                         WHERE T0.""ObjectCode""=:p0 AND IFNULL(T1.""IsTick"",'') = 'Y' ORDER BY T0.""CreatedDate"" ASC "; //AND T1.""UserId"" =:p1

            return GetValue<string>(CONTEXT, ssql, objectCode);

        }

        public static DataTable GetApprovalStages()
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""Id"", T0.""StageName""  FROM ""Tm_ApprovalStage"" T0 ORDER BY T0.""StageName"" ASC ";
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static DataTable GetDocumentTypePlusAll(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT -1 AS ""Id"", 'ALL' AS ""DocumentName"" FROM DUMMY UNION ALL SELECT T0.""Id"", T0.""DocumentName""  FROM ""Tm_DocContent"" T0   WHERE T0.""Type""=:p0  ";
                return GetDataTable(CONTEXT, ssql, strType);
            }
        }

        public static DataTable GetDocumentBaseOnCategoryPlusAll(string strType, string strCategory)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT -1 AS ""Id"", 'ALL' AS ""DocumentName"" FROM DUMMY UNION ALL SELECT DISTINCT T0.""Id"", T0.""DocumentName""  FROM ""Tm_DocContent"" T0 INNER JOIN ""Tm_DocNonShipping"" T1 ON T1.""Type"" = T0.""Id""   WHERE T0.""Type""=:p0 AND T1.""Category"" =:p1  ";
                return GetDataTable(CONTEXT, ssql, strType, strCategory);
            }
        }

        public static DataTable GetItemList()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetItemList(CONTEXT);
            }
        }

        public static DataTable GetItemList(HANA_APP CONTEXT)
        { 
            var ssql = @"SELECT DISTINCT ""ItemCode"" FROM ""Tm_Item"" ";
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetList(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetList(CONTEXT, strType);
            }
        }

        public static DataTable GetListPlusAll(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT 'ALL' AS ""Code"", 'ALL' AS ""Name"" FROM DUMMY UNION ALL SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0 ";

                return GetDataTable(CONTEXT, ssql, strType);
            }
        }

        public static DataTable GetPositions()
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"
                SELECT NULL AS ""Code"", NULL AS ""Name"" FROM DUMMY

                UNION ALL
                
                SELECT T0.""Code"", T0.""Name"" FROM ""{0}"".""@POSITION"" T0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static DataTable GetDefaultWhsCodes(int id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"
                    SELECT * FROM (
                        SELECT NULL AS ""Code"", NULL AS ""Name"" FROM DUMMY

                        UNION ALL

                        SELECT T0.""WhsCode"" AS ""Code"", 
                        T0.""WhsName""  AS ""Name""
                        FROM ""{0}"".""OWHS"" T0  
                        LEFT JOIN ""Tm_User_Warehouse"" T1 ON T0.""WhsCode"" = T1.""WhsCode"" AND  T1.""Id"" = {1} 
                    ) TX ORDER BY TX.""Code"" ASC
                ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name, id);
                return GetDataTable(CONTEXT, ssql);
            }
        }


        public static DataTable GetListPointEx(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetListPointEx(CONTEXT, strType);
            }
        }

        public static DataTable GetList(HANA_APP CONTEXT, string strType)
        {
            var ssql = @"SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0 ORDER BY T0.""Sort"" ASC ";

            return GetDataTable(CONTEXT, ssql, strType);
        }

        public static DataTable GetListPointEx(HANA_APP CONTEXT, string strType)
        {
            var ssql = @"SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0  ";

            return GetDataTable(CONTEXT, ssql, strType);
        }

        public static DataTable GetList(string strType, string strCategory)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetList(CONTEXT, strType, strCategory);
            }
        }

        public static DataTable GetList(HANA_APP CONTEXT, string strType, string strCategory)
        {
            var ssql = @"SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0  AND T0.""Category""=:p1 ORDER BY T0.""Sort"" ";

            return GetDataTable(CONTEXT, ssql, strType, strCategory);

        }

        public static DataTable GetUserWarehouse(int userId = -1)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetUserWarehouse(CONTEXT, userId);
            }
        }

        public static DataTable GetUserWarehouse(HANA_APP CONTEXT, int userId = -1)
        {
            var ssql = "";
            if (userId == 1)
            {
                ssql = @"SELECT DISTINCT T1.""WhsCode"" AS ""Code"", T1.""WhsName"" AS ""Name"" 
                    FROM ""{0}"".""OWHS"" T1
                    ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
            }
            else
            {
                ssql = @"SELECT T0.""WhsCode"" AS ""Code"", T1.""WhsName"" AS ""Name"" 
                FROM ""Tm_User_Warehouse"" T0   
                INNER JOIN ""{0}"".""OWHS"" T1 ON T0.""WhsCode"" = T1.""WhsCode""
                WHERE T0.""Id""= '{1}'  AND T0.""IsTick"" = 'Y'  ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name, userId);
            }

            var ret = GetDataTable(CONTEXT, ssql);
            return ret;

        }

        public static string GetWarehouseCodeByUserId(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetWarehouseCodeByUserId(CONTEXT, userId);
            }
        }

        public static string GetWarehouseCodeByUserId(HANA_APP CONTEXT, int userId)
        {
            var ssql = @"SELECT T0.""WhsCode""  FROM ""Tm_User"" T0   WHERE T0.""Id""=:p0    ";

            return GetValue<string>(CONTEXT, ssql, userId);

        }

        public static string GetWarehouseName(string whsCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""WhsName""  FROM  ""{0}"".""OWHS"" T0   WHERE T0.""WhsCode""= '{1}'   ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name, whsCode);
                return GetValue<string>(CONTEXT, ssql);
            }
        }

        public static string GetItemName(string itemCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""ItemName""  FROM  ""{0}"".""OITM"" T0   WHERE T0.""ItemCode""= '{1}'   ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name, itemCode);
                return GetValue<string>(CONTEXT, ssql);
            }
        }

        public static decimal? GetItemWarehouseStock(string itemCode, string whsCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""OnHand""  FROM  ""{0}"".""OITW"" T0   WHERE T0.""ItemCode"" = '{1}' AND T0.""WhsCode""= '{2}'   ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name, itemCode, whsCode);
                return GetValue<decimal?>(CONTEXT, ssql);
            }
        }

        public static string GetUserNameByUserId(int? userId)
        {
            if (userId.HasValue)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    return GetUserNameByUserId(CONTEXT, userId);
                }
            }
            else 
            {
                return "";
            }
        }

        public static string GetUserNameByUserId(HANA_APP CONTEXT, int? userId)
        {
            var ssql = @"SELECT TOP 1 IFNULL(T0.""UserName"",'') AS ""UserName"" FROM ""Tm_User"" T0   WHERE T0.""Id""=:p0  ";

            return GetValue<string>(CONTEXT, ssql, userId);
        }

        public static string GeFirstNameByUserId(int? userId)
        {
            if (userId.HasValue)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    return GeFirstNameByUserId(CONTEXT, userId);
                }
            }
            else
            {
                return "";
            }
        }

        public static string GeFirstNameByUserId(HANA_APP CONTEXT, int? userId)
        {
            var ssql = @"SELECT TOP 1 IFNULL(T0.""FirstName"",'') AS ""FirstName"" FROM ""Tm_User"" T0   WHERE T0.""Id""=:p0  ";

            return GetValue<string>(CONTEXT, ssql, userId);
        }

        public static DataTable GetPaymentTypes()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetPaymentTypes(CONTEXT);
            }
        }

        public static DataTable GetPaymentTypes(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""Code"", T0.""Name""
                            FROM ""{0}"".""@IDU_PAYMENT_TYPE"" T0 ORDER BY T0.""Name"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }


        public static DataTable GetAreas()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAreas(CONTEXT);
            }
        }

        public static DataTable GetAreas(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""Code"", T0.""Name""
                            FROM ""{0}"".""@IDU_AREA"" T0 ORDER BY T0.""Name"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetBPGroup(string GroupType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetBPGroup(CONTEXT, GroupType);
            }
        }

        public static DataTable GetBPGroup(HANA_APP CONTEXT, string GroupType)
        {
            string ssql = @"SELECT T0.""GroupCode"", T0.""GroupName""
                            FROM ""{0}"".""OCRG"" T0 WHERE T0.""GroupType"" = '{1}' ORDER BY T0.""GroupCode"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name, GroupType);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetCapacity(string strJenis)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetCapacity(CONTEXT, strJenis);
            }
        }

        public static DataTable GetCapacity(HANA_APP CONTEXT, string strJenis)
        {
            string ssql = @"SELECT TO_DECIMAL(T0.""U_Capacity"",10, 2) AS ""Capacity""
                            FROM ""{0}"".""@IDU_CAPACITY"" T0 WHERE T0.""U_Jenis"" = '{1}' ORDER BY T0.""U_Capacity"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name, strJenis);
            return GetDataTable(CONTEXT, ssql);
        }

        public static T GetValue<T>(string ssql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetValue<T>(CONTEXT, ssql, parameters);
            }
        }

        public static T GetValue<T>(HANA_APP CONTEXT, string ssql, params object[] parameters)
        {

            return CONTEXT.Database.SqlQuery<T>(ssql, parameters).FirstOrDefault();
        }


        //public static DataTable GetCurrencies()
        //{
        //    var ssql = PetaPoco.Sql.Builder
        //             .Append("SELECT CurrCode AS CurCode, CurrName AS CurName FROM [" + DbProvider.dbSap_Name + "].DBO.OCRN (NOLOCK)  "
        //         );
        //    return PetaPocoIduSqlRsExtensionsApp.IduGetDataTable(ssql);
        //}

        public static DataTable GetLayoutForms()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetLayoutForms(CONTEXT);
            }
        }

        public static DataTable GetLayoutForms(HANA_APP CONTEXT)
        {
            var ssql = @"SELECT T0.""LayoutFormCode"", T0.""LayoutFormName"" FROM ""Ts_LayoutForm"" T0   ORDER BY T0.""Sort"" ASC";

            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetRoles()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetRoles(CONTEXT);
            }
        }

        public static DataTable GetRoles(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"RoleName\" FROM \"Tm_Role\" T0   ORDER BY T0.\"RoleName\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetEmployees()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetEmployees(CONTEXT);
            }
        }

        public static DataTable GetEmployees(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""empID"" AS ""empID"",T0.""ExtEmpNo"", T0.""firstName"" AS ""firstName"", T0.""lastName"" AS ""lastName"", T0.""middleName"" AS ""middleName"" 
                            FROM  ""{0}"".""OHEM"" T0       
                            ORDER BY T0.""firstName"", T0.""lastName"", T0.""middleName"" ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        
        public static DataTable GetWarehouses()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetWarehouses(CONTEXT);
            }
        }

        public static DataTable GetWarehouses(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""WhsCode"", T0.""WhsName""  
                            FROM  ""{0}"".""OWHS"" T0       
                            ORDER BY T0.""WhsCode"" ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        //
        public static DataTable GetSeries(string ObjType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetSeries(CONTEXT, ObjType);
            }
        }

        public static DataTable GetSeries(HANA_APP CONTEXT, string ObjType)
        {
            string ssql = @"SELECT T0.""Series"", T0.""SeriesName""  
                            FROM  ""{0}"".""NNM1"" T0  
                            WHERE T0.""ObjectCode"" = '" + ObjType + "' ORDER BY T0.\"ObjectCode\" DESC ";



            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        public static string GetCustomerCode(string docEntry)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetCustomerCode(CONTEXT, docEntry);
            }
        }

        public static string GetCustomerCode(HANA_APP CONTEXT, string docEntry)
        {
            string ssql = @"SELECT T0.""CardCode""  
                            FROM  ""{0}"".""OINV"" T0   
                            WHERE T0.""DocEntry"" = " + docEntry + " ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetValue<string>(CONTEXT, ssql, docEntry);
        }

        public static DataTable GetSalesmans()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetSalesmans(CONTEXT);
            }
        }

        public static DataTable GetSalesmans(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""SlpCode"", T0.""SlpName"",T0.""U_SalesPersonCode""   
                            FROM  ""{0}"".""OSLP"" T0       
                            ORDER BY T0.""U_SalesPersonCode"" ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        
        public static DataTable GetReportGroups()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetReportGroups(CONTEXT);
            }
        }

        public static DataTable GetReportGroups(HANA_APP CONTEXT)
        {

            var ssql = @"SELECT T0.""Id"", T0.""GroupName"" FROM ""Tm_ReportGroup"" T0   ORDER BY T0.""SortCode"", T0.""Id"" ";

            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetQueryGroups()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetQueryGroups(CONTEXT);
            }
        }

        public static DataTable GetQueryGroups(HANA_APP CONTEXT)
        {
            var ssql = @"SELECT T0.""Id"", T0.""GroupName"" FROM ""Tm_QueryGroup"" T0   ORDER BY T0.""SortCode"", T0.""Id"" ";

            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetAlertGroups()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAlertGroups(CONTEXT);
            }
        }

        public static DataTable GetAlertGroups(HANA_APP CONTEXT)
        {

            var ssql = @"SELECT T0.""Id"", T0.""GroupName"" FROM ""Tm_AlertGroup"" T0   ORDER BY T0.""SortCode"", T0.""Id"" ";

            return GetDataTable(CONTEXT, ssql);

        }

        public static string GetIsAdmin(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetIsAdmin(CONTEXT, userId);
            }
        }


        public static string GetIsAdmin(HANA_APP CONTEXT, int userId)
        {
            int? roleId = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 IFNULL(T0.\"RoleId\",0) AS \"RoleId\" FROM \"Tm_User\" T0   WHERE T0.\"Id\"=:p0", userId).FirstOrDefault();
            string roleName = CONTEXT.Database.SqlQuery<string>("SELECT TOP 1 IFNULL(T0.\"RoleName\",'') AS \"RoleName\" FROM \"Tm_Role\" T0   WHERE T0.\"Id\"=:p0", roleId).FirstOrDefault();
            return GetIsAdmin(roleName);
        }


        public static string GetIsAdmin(string roleName)
        {
            if ((roleName.ToLower() == "admin") || (roleName.ToLower() == "administrator"))
            {
                return "Y";
            }
            else
            {
                return "N";
            }

        }

        public static bool GetAuthAction(int userId, string url)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAuthAction(CONTEXT, userId, url);
            }
        }

        public static bool GetAuthAction(HANA_APP CONTEXT, int userId, string url)
        {
            string ssql = @"SELECT DISTINCT T0.""Id""
                            FROM ""Tm_User"" T0   
                            INNER JOIN ""Tm_Role"" T1   ON T0.""RoleId""=T1.""Id"" 
                            INNER JOIN ""Tm_Role_Auth"" T2   ON T1.""Id""=T2.""Id"" AND T2.""IsAccess""='Y' 
                            INNER JOIN ""Ts_Menu"" T3   ON T2.""MenuCode""=T3.""MenuCode"" AND T3.""Url"" =:p1 
                            WHERE T0.""Id""=:p0";


            int? id = CONTEXT.Database.SqlQuery<int?>(ssql, userId, url).FirstOrDefault();
            if (id.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static DataTable GetDataTable(string sql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataTable(CONTEXT, sql, parameters);
            }
        }

        public static DataTable GetDataTable(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            var s = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, sql, parameters);

            return s;
        }

        public static DataSet GetDataSet(string sql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataSet(CONTEXT, sql, parameters);
            }
        }

        public static DataSet GetDataSet(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            var s = EfIduHanaRsExtensionsApp.IduGetDataSet(CONTEXT, sql, parameters);

            return s;
        }

        public static DataTable ExpandoToDataTable(this IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;

            var dt = new DataTable();
            foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }
        
        public static DataTable GetCostCenters(string dimCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""OcrCode"" AS ""Code"", ""OcrName"" AS ""Name""
                    FROM ""{0}"".""OOCR""
                    WHERE ""DimCode"" = '{1}'
                    AND ""Active"" = 'Y'
                    ORDER BY ""OcrCode"" ASC
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, dimCode);
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static List<GetCodeNameModel> GetCostCenterList(string dimCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""OcrCode"" AS ""Code"", ""OcrName"" AS ""Name""
                    FROM ""{0}"".""OOCR""
                    WHERE ""DimCode"" = '{1}'
                    AND ""Active"" = 'Y'
                    ORDER BY ""OcrCode"" ASC
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, dimCode);
                List<GetCodeNameModel> list = CONTEXT.Database.SqlQuery<GetCodeNameModel>(ssql).ToList();
                return list;
            }
        }

        public static List<GetCodeNameModel> GetProjectList()
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""PrjCode"" AS ""Code"", ""PrjName"" AS ""Name""
                    FROM ""{0}"".""OPRJ""
                    WHERE ""Locked"" = 'N'
                    ORDER BY ""PrjCode"" ASC
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                List<GetCodeNameModel> list = CONTEXT.Database.SqlQuery<GetCodeNameModel>(ssql).ToList();
                return list;
            }
        }

        public static string GetCostCenterName(string OcrCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""OcrName"" AS ""Name""
                    FROM ""{0}"".""OOCR""
                    WHERE ""OcrCode"" = '{1}'
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, OcrCode);
                return CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
            }
        }

        public static DataTable GetProjects()
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""PrjCode"" AS ""Code"", ""PrjName"" AS ""Name""
                    FROM ""{0}"".""OPRJ""
                    WHERE ""Locked"" = 'N'
                    ORDER BY ""PrjCode"" ASC
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static string GetProjectName(string projectCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""PrjName"" AS ""Name""
                    FROM ""{0}"".""OPRJ""
                    WHERE ""PrjCode"" = '{1}'
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, projectCode);
                return CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
            }
        }

        public static DataTable GetSAPCodeOfAccount()
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT ""AcctCode"" AS ""Code"", ""AcctName"" AS ""Name""
                    FROM ""{0}"".""OACT""
                    WHERE ""Levels"" = 6
                    ORDER BY ""AcctCode"" ASC
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static string GetSAPCoaAdjustment(string code)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT T0.""U_IDU_Account"" AS ""Code""
                    FROM ""{0}"".""@ADJUSTMENTTYPE"" AS T0
                    WHERE T0.""Code"" = '{1}'
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, code);
                return CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
            }
        }

        public static DataTable GetSAPCodeOfAccountWithNull()
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT NULL AS ""Code"", NULL AS ""Name"" FROM DUMMY
                        UNION ALL
                    SELECT ""AcctCode"" AS ""Code"", ""AcctName"" AS ""Name""
                    FROM ""{0}"".""OACT""
                    WHERE ""Levels"" = 6
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static DataTable GetSAPAdjustmentTypeWithNull(string adjustmentType = "IN" )
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT NULL AS ""Code"", NULL AS ""Name"" FROM DUMMY
                        UNION ALL
                    SELECT T0.""Code"" AS ""Code"", T0.""Name"" AS ""Name""
                    FROM ""{0}"".""@ADJUSTMENTTYPE"" AS T0
                    WHERE ""U_IDU_Type"" = {1}
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, adjustmentType);
                return GetDataTable(CONTEXT, ssql);
            }
        }

        public static DataTable GetSAPAdjustmentType(string adjustmentType = "IN" )
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"
                    SELECT T0.""Code"" AS ""Code"", T0.""Name"" AS ""Name""
                    FROM ""{0}"".""@ADJUSTMENTTYPE"" AS T0
                    WHERE ""U_IDU_Type"" = {1}
                ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, adjustmentType);
                return GetDataTable(CONTEXT, ssql);
            }
        }


        public static DataTable GetSAPUserFields(string TableId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT T0.""FldValue"" AS ""Code"", T0.""Descr"" AS ""Name""
                            FROM ""{0}"".""UFD1"" T0 WHERE T0.""TableID"" = '{1}' ORDER BY T0.""FldValue"" ASC ";

                ssql = string.Format(ssql, DbProvider.dbSap_Name, TableId);
                return GetDataTable(CONTEXT, ssql);
            }
        }

    }


}
