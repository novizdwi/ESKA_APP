using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using System.Configuration;


namespace Models._Sap
{
    public class SAPCachedCompanyII
    {
        private static Mutex PoolLockII = new Mutex();

        private static Mutex TransactionLockII = new Mutex();

        private static List<SAPConnection> ConnectionPoolII = new List<SAPConnection>();


        public static int ConnectionPoolCountII()
        {
            int result = 0;
            if (ConnectionPoolII != null)
            {
                result = ConnectionPoolII.Count();
            }
            return result;
        }
        //
        //Get a company instance from the pool, according it's connection parameters, if no one is available, create a new one.
        // 
        public static SAPbobsCOM.Company GetCompanyII()
        {
            TransactionLockII.WaitOne();

            SAPbobsCOM.BoDataServerTypes Servertype;
            switch (ConfigurationManager.AppSettings["Sap_DbTypeII"].ToString())
            {
                case "dst_HANADB":
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    break;
                //case "dst_MSSQL2016":
                //    Servertype = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                //    break;
                case "dst_MSSQL2014":
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    break;
                case "dst_MSSQL2012":
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    break;
                case "dst_MSSQL2008":
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                    break;
                case "dst_MSSQL2005":
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;
                    break;
                default:
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    break;
            }


            string Servername = ConfigurationManager.AppSettings["Sap_DbServerII"];
            string Dbuser = ConfigurationManager.AppSettings["Sap_DbUserNameII"];
            string Dbpassword = ConfigurationManager.AppSettings["Sap_DbPasswordII"];
            string SAPDBName = ConfigurationManager.AppSettings["Sap_DbNameII"];
            string SAPUser = ConfigurationManager.AppSettings["Sap_SapUserNameII"];
            string SAPUserPassword = ConfigurationManager.AppSettings["Sap_SapPasswordII"];
            string SAPLicense = ConfigurationManager.AppSettings["Sap_SapLicenseServerII"];



            SAPbobsCOM.Company ReturnCompany = null;

            string Id = null;
            Id = Servertype.ToString() + ";";
            Id = Id + Servername + ";";
            Id = Id + Dbuser + ";";
            Id = Id + Dbpassword + ";";
            Id = Id + SAPDBName + ";";
            Id = Id + SAPUser + ";";
            Id = Id + SAPUserPassword + ";";
            Id = Id + SAPLicense + ";";

            //Add synchronized
            PoolLockII.WaitOne();
            foreach (SAPConnection Con in ConnectionPoolII)
            {
                if (Id == Con.Identity & !Con.InUse)
                {
                    //If found, mark it in use, and return
                    ReturnCompany = Con.CompanyInstance;
                    Con.InUse = true;
                    PoolLockII.ReleaseMutex();
                    return ReturnCompany;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            //'If no connection found, quit the lock zone, and create a new connection and
            PoolLockII.ReleaseMutex();

            try
            {
                SAPConnection NewCon = new SAPConnection(Servertype, Servername, Dbuser, Dbpassword, SAPDBName, SAPUser, SAPUserPassword, SAPLicense);
                PoolLockII.WaitOne();
                ConnectionPoolII.Add(NewCon);
                NewCon.InUse = true;
                PoolLockII.ReleaseMutex();
                return NewCon.CompanyInstance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //
        //Release a company instance, back into the pool
        //
        public static void ReleaseII(SAPbobsCOM.Company oCompany)
        {

            //Add synchronized
            PoolLockII.WaitOne();
            foreach (SAPConnection Con in ConnectionPoolII)
            {
                if (object.ReferenceEquals(Con.CompanyInstance, oCompany))
                {
                    //If found, mark it not in use.
                    Con.InUse = false;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            PoolLockII.ReleaseMutex();
            TransactionLockII.ReleaseMutex();

        }
    }

}