using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;
using System.Configuration;

namespace Models._Utils
{
    public class SpNotif
    {

        public static Boolean SpSysTransNotif(int userId,HANA_APP CONTEXT, string category, string objCode, string transType, string fieldKeys, string fieldValues)
        {
            return SpNotif.SpSysTransNotif(userId,CONTEXT, category, objCode, transType, fieldKeys, fieldValues, "");
        }

        public static Boolean SpSysTransNotif(int userId,HANA_APP CONTEXT, string category, string objCode, string transType, string fieldKeys, string fieldValues, string fieldParentValues)
        {
            string errorMassage;

            //SpNotifMessage massage = CONTEXT.Database.SqlQuery<SpNotifMessage>("CALL \"SpSysTransNotif\" (:p0,:p1,:p2,:p3,:p4,:p5) '", category, objCode, transType, fieldKeys, fieldValues, fieldParentValues).Single();
            SpNotifMessage massage = CONTEXT.Database.SqlQuery<SpNotifMessage>("CALL \"SpSysTransNotif\" (" + userId.ToString() + ",'" + category.Replace("'", "''") + "','" + objCode.Replace("'", "''") + "','" + transType.Replace("'", "''") + "','" + fieldKeys.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "','" + fieldParentValues.Replace("'", "''") + "') ").Single();


            if (massage.error == 0)
            {
                return true;
            }
            else
            {
                errorMassage = string.Format("[VALIDATION] {0} - {1}", massage.error, massage.error_message);
                throw new Exception(errorMassage);
            }

        }

        public static Boolean SpSysTransNotif(int userId,SAPbobsCOM.Company oCompany, string category, string objCode, string transType, string fieldKeys, string fieldValues)
        {
            return SpNotif.SpSysTransNotif(userId,oCompany, category, objCode, transType, fieldKeys, fieldValues, "");
        }

        public static Boolean SpSysTransNotif( int userId,SAPbobsCOM.Company oCompany, string category, string objCode, string transType, string fieldKeys, string fieldValues, string fieldParentValues)
        {
            string errorMassage;

            SAPbobsCOM.Recordset rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string ssql = "CALL \"" + DbProvider.dbApp_Name + "\".\"SpSysTransNotif\" (" + userId.ToString() + ",'" + category.Replace("'", "''") + "','" + objCode.Replace("'", "''") + "','" + transType.Replace("'", "''") + "','" + fieldKeys.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "','" + fieldParentValues.Replace("'", "''") + "') ";
            rs = _Utils.SapCompany.GetRs(oCompany, ssql);

            SpNotifMessage massage = new SpNotifMessage();
            massage.error = rs.Fields.Item("error").Value;
            massage.error_message = rs.Fields.Item("error_message").Value;


            if (massage.error == 0)
            {
                return true;
            }
            else
            {
                errorMassage = string.Format("[VALIDATION] {0} - {1}", massage.error, massage.error_message);
                throw new Exception(errorMassage);
            }

        }


        public static Boolean SpSysControllerTransNotif(int userId, string ControllerName, HANA_APP CONTEXT, string category, string objCode, string transType, string fieldKeys, string fieldValues)
        {
            return SpNotif.SpSysControllerTransNotif(userId, ControllerName, CONTEXT, category, objCode, transType, fieldKeys, fieldValues, "");
        }

        public static Boolean SpSysControllerTransNotif(int userId, string ControllerName, HANA_APP CONTEXT, string category, string objCode, string transType, string fieldKeys, string fieldValues, string fieldParentValues)
        {
            string errorMassage;
            string x = "CALL \"Sp" + ControllerName + "__TransNotif\" (" + userId.ToString() + ",'" + category.Replace("'", "''") + "','" + objCode.Replace("'", "''") + "','" + transType.Replace("'", "''") + "','" + fieldKeys.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "','" + fieldParentValues.Replace("'", "''") + "') ";

            //SpNotifMessage massage = CONTEXT.Database.SqlQuery<SpNotifMessage>("CALL \"SpSysTransNotif\" (:p0,:p1,:p2,:p3,:p4,:p5) '", category, objCode, transType, fieldKeys, fieldValues, fieldParentValues).Single();
            SpNotifMessage massage = CONTEXT.Database.SqlQuery<SpNotifMessage>("CALL \"Sp" + ControllerName + "__TransNotif\" (" + userId.ToString() + ",'" + category.Replace("'", "''") + "','" + objCode.Replace("'", "''") + "','" + transType.Replace("'", "''") + "','" + fieldKeys.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "','" + fieldParentValues.Replace("'", "''") + "') ").Single();


            if (massage.error == 0)
            {
                return true;
            }
            else
            {
                errorMassage = string.Format("[VALIDATION] {0} - {1}", massage.error, massage.error_message);
                throw new Exception(errorMassage);
            }

        }

        public static Boolean SpSysControllerTransNotif(int userId, string ControllerName, SAPbobsCOM.Company oCompany, string category, string objCode, string transType, string fieldKeys, string fieldValues)
        {
            return SpNotif.SpSysControllerTransNotif(userId, ControllerName, oCompany, category, objCode, transType, fieldKeys, fieldValues, "");
        }

        public static Boolean SpSysControllerTransNotif(int userId, string ControllerName, SAPbobsCOM.Company oCompany, string category, string objCode, string transType, string fieldKeys, string fieldValues, string fieldParentValues)
        {
            string errorMassage;

            SAPbobsCOM.Recordset rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string ssql = "CALL \"" + DbProvider.dbApp_Name + "\".\"Sp" + ControllerName + "__TransNotif\" (" + userId.ToString() + ",'" + category.Replace("'", "''") + "','" + objCode.Replace("'", "''") + "','" + transType.Replace("'", "''") + "','" + fieldKeys.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "','" + fieldParentValues.Replace("'", "''") + "') ";
            //string ssql = "CALL \"" + ConfigurationManager.ConnectionStrings["HANA_APP2"].ToString() + "\".\"Sp" + ControllerName + "__TransNotif\" (" + userId.ToString() + ",'" + category.Replace("'", "''") + "','" + objCode.Replace("'", "''") + "','" + transType.Replace("'", "''") + "','" + fieldKeys.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "','" + fieldParentValues.Replace("'", "''") + "') ";
            
            rs = _Utils.SapCompany.GetRs(oCompany, ssql);

            SpNotifMessage massage = new SpNotifMessage();
            massage.error = rs.Fields.Item("error").Value;
            massage.error_message = rs.Fields.Item("error_message").Value;


            if (massage.error == 0)
            {
                return true;
            }
            else
            {
                errorMassage = string.Format("[VALIDATION] {0} - {1}", massage.error, massage.error_message);
                throw new Exception(errorMassage);
            }

        }



        public static Boolean SpSysPrintLayoutNotif(  string controler, string fieldValues, int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return SpSysPrintLayoutNotif(CONTEXT, controler, fieldValues, userId);
            }

        }

        public static Boolean SpSysPrintLayoutNotif(HANA_APP CONTEXT, string controler, string fieldValues, int userId)
        {
            string errorMassage;

            SpNotifMessage massage = CONTEXT.Database.SqlQuery<SpNotifMessage>("CALL \"SpSysPrintLayoutNotif\" ('" + controler.Replace("'", "''") + "','" + fieldValues.Replace("'", "''") + "'," + userId.ToString() + ") ").Single();


            if (massage.error == 0)
            {
                return true;
            }
            else
            {
                errorMassage = string.Format("[VALIDATION] {0} - {1}", massage.error, massage.error_message);
                throw new Exception(errorMassage);
            }

        }



    }

    public class SpNotifMessage
    {
        public int error { set; get; }
        public String error_message { set; get; }
    }
}