using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SAPbobsCOM;

namespace Models._Utils
{
    public class SapCompany
    {
        public static string RetCoaCode(SAPbobsCOM.Company company, string coaCode)
        {
            return SapCompany.RetRstField(company, "SELECT T0.\"AcctCode\" FROM \"OACT\" T0 WHERE T0.\"ActId\"='" + coaCode + "'"); 
        }

        public static string RetRstField(SAPbobsCOM.Company company, string ssql)
        {
            SAPbobsCOM.Recordset rs;
            string temp;
            rs = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(ssql);
            if (rs.EoF)
            {
                temp = "";
            }
            else
            {
                temp = rs.Fields.Item(0).Value.ToString();
            }

            return temp;

        }

        public static dynamic RetRstFieldObjDynamic(SAPbobsCOM.Company company, string ssql)
        {
            SAPbobsCOM.Recordset rs;
            dynamic temp;
            rs = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(ssql);
            if (rs.EoF)
            {
                temp = null;
            }
            else
            {
                temp = rs.Fields.Item(0).Value;
            }

            return temp;

        }

        public static SAPbobsCOM.Recordset GetRs(SAPbobsCOM.Company company, string ssql)
        {
            SAPbobsCOM.Recordset rs;
            rs = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(ssql);

            return rs;

        }

        public static void ExecuteQuery(SAPbobsCOM.Company company, string ssql)
        {
            SAPbobsCOM.Recordset rs;
            rs = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(ssql);

        }

        public static void CleanUp(Object obj)
        {
            if (obj != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
        }

        public static void CleanUpGCCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}
