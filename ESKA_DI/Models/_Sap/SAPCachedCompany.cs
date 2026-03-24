using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using System.Configuration;


namespace Models._Sap
{
    public class SAPCachedCompany
    {
        private static Mutex PoolLock = new Mutex();

        private static Mutex TransactionLock = new Mutex();

        private static List<SAPConnection> ConnectionPool = new List<SAPConnection>();


        public static int ConnectionPoolCount()
        {
            int result = 0;
            if (ConnectionPool != null)
            {
                result = ConnectionPool.Count();
            }
            return result;
        }
        //
        //Get a company instance from the pool, according it's connection parameters, if no one is available, create a new one.
        // 
        public static SAPbobsCOM.Company GetCompany()
        {
            TransactionLock.WaitOne();

            SAPbobsCOM.BoDataServerTypes Servertype;
            switch (ConfigurationManager.AppSettings["Sap_DbType"].ToString())
            {
                case "dst_HANADB":
                    Servertype = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    break;
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


            string Servername = ConfigurationManager.AppSettings["Sap_DbServer"];
            string Dbuser = ConfigurationManager.AppSettings["Sap_DbUserName"];
            string Dbpassword = ConfigurationManager.AppSettings["Sap_DbPassword"];
            string SAPDBName = ConfigurationManager.AppSettings["Sap_DbName"];
            string SAPUser = ConfigurationManager.AppSettings["Sap_SapUserName"];
            string SAPUserPassword = ConfigurationManager.AppSettings["Sap_SapPassword"];
            string SAPLicense = ConfigurationManager.AppSettings["Sap_SapLicenseServer"];



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
            PoolLock.WaitOne();
            foreach (SAPConnection Con in ConnectionPool)
            {
                if (Id == Con.Identity & !Con.InUse)
                {
                    //If found, mark it in use, and return
                    ReturnCompany = Con.CompanyInstance;
                    Con.InUse = true;
                    PoolLock.ReleaseMutex();
                    return ReturnCompany;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            //'If no connection found, quit the lock zone, and create a new connection and
            PoolLock.ReleaseMutex();

            try
            {
                SAPConnection NewCon = new SAPConnection(Servertype, Servername, Dbuser, Dbpassword, SAPDBName, SAPUser, SAPUserPassword, SAPLicense);
                PoolLock.WaitOne();
                ConnectionPool.Add(NewCon);
                NewCon.InUse = true;
                PoolLock.ReleaseMutex();
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
        public static void Release(SAPbobsCOM.Company oCompany)
        {

            //Add synchronized
            PoolLock.WaitOne();
            foreach (SAPConnection Con in ConnectionPool)
            {
                if (object.ReferenceEquals(Con.CompanyInstance, oCompany))
                {
                    //If found, mark it not in use.
                    Con.InUse = false;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            PoolLock.ReleaseMutex();
            TransactionLock.ReleaseMutex();

        }
    }

}