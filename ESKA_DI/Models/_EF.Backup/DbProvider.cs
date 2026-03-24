using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MJL_DI.Models._EF;

using System.Configuration;

namespace Models._Ef
{

    public class DbProvider
    {
        const string EfContextKeyDbApp = "EfContextKeyDbApp_";

        const string EfContextKeyDbApp_Name = "EfContextKeyDbApp_Name_";

        const string EfContextKeyDbSegel_Name = "EfContextKeyDbSegel_Name_";

        const string EfContextKeyDbSadpIApp_Name = "EfContextKeyDbSadpIApp_Name_";

        const string EfContextKeyDbDmlApp_Name = "EfContextKeyDbDmlApp_Name_";

        //const string EfContextKeyDbSap = "EfContextKeyDbSap_";

        const string EfContextKeyDbSap_Name = "EfContextKeyDbSap_Name_";

        const string EfContextKeyDbSap_Name2 = "EfContextKeyDbSap_Name2_"; //sadp2
         
        public static HANA_APP dbApp
        {
            //get
            //{
            //    if (HttpContext.Current.Items[EfContextKeyDbApp] == null)
            //    {
            //        HANA_APP db_ = new HANA_APP();
            //        HttpContext.Current.Items[EfContextKeyDbApp] = db_;
            //    }

            //    return (HANA_APP)HttpContext.Current.Items[EfContextKeyDbApp];
            //}

            get
            {
                HANA_APP db_ = new HANA_APP();
                return db_;
            }

        }

        public static string dbApp_Name
        {
            get
            {
                string connStr  = dbApp.Database.Connection.ConnectionString;// ConfigurationManager.ConnectionStrings["HANA_APP"].ToString();
                //string connStr = ConfigurationManager.ConnectionStrings["HANA_APP"].ToString();
                if (HttpContext.Current.Items[EfContextKeyDbApp_Name] == null)
                {
                    var connectionString = new Sap.Data.Hana.HanaConnectionStringBuilder(connStr);

                    HttpContext.Current.Items[EfContextKeyDbApp_Name] = connectionString.CurrentSchema;
                }
                return (string)HttpContext.Current.Items[EfContextKeyDbApp_Name];
            }
        }

        public static string dbSegel_Name
        {
            get
            {
                //string connStr  = dbApp.Database.Connection.ConnectionString;// ConfigurationManager.ConnectionStrings["HANA_APP"].ToString();
                string connStr = ConfigurationManager.ConnectionStrings["SEGEL_APP"].ToString();
                if (HttpContext.Current.Items[EfContextKeyDbSegel_Name] == null)
                {
                    var connectionString = new Sap.Data.Hana.HanaConnectionStringBuilder(connStr);

                    HttpContext.Current.Items[EfContextKeyDbSegel_Name] = connectionString.CurrentSchema;
                }
                return (string)HttpContext.Current.Items[EfContextKeyDbSegel_Name];
            }
        }


        public static string dbSadpIApp_Name
        {
            get
            {
                //string connStr  = dbApp.Database.Connection.ConnectionString;// ConfigurationManager.ConnectionStrings["HANA_APP"].ToString();
                string connStr = ConfigurationManager.ConnectionStrings["SADPI_APP"].ToString();
                if (HttpContext.Current.Items[EfContextKeyDbSadpIApp_Name] == null)
                {
                    var connectionString = new Sap.Data.Hana.HanaConnectionStringBuilder(connStr);

                    HttpContext.Current.Items[EfContextKeyDbSadpIApp_Name] = connectionString.CurrentSchema;
                }
                return (string)HttpContext.Current.Items[EfContextKeyDbSadpIApp_Name];
            }
        }

        public static string dbDmlApp_Name
        {
            get
            {
                //string connStr  = dbApp.Database.Connection.ConnectionString;// ConfigurationManager.ConnectionStrings["HANA_APP"].ToString();
                string connStr = ConfigurationManager.ConnectionStrings["DML_APP"].ToString();
                if (HttpContext.Current.Items[EfContextKeyDbDmlApp_Name] == null)
                {
                    var connectionString = new Sap.Data.Hana.HanaConnectionStringBuilder(connStr);

                    HttpContext.Current.Items[EfContextKeyDbDmlApp_Name] = connectionString.CurrentSchema;
                }
                return (string)HttpContext.Current.Items[EfContextKeyDbDmlApp_Name];
            }
        }

        //public static DbSap dbSap
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Items[EfContextKeyDbSap_] == null)
        //        {
        //            DbSap db_ = new DbSap();
        //            db_.CommandTimeout = 60 * 5;
        //            HttpContext.Current.Items[EfContextKeyDbSap_] = db_;


        //        }
        //        return (DbSap)HttpContext.Current.Items[EfContextKeyDbSap_];
        //    } 

        //}

        public static string dbSap_Name
        {
            get
            {
                string connStr = ConfigurationManager.ConnectionStrings["HANA_SAP"].ToString();
                if (HttpContext.Current.Items[EfContextKeyDbSap_Name] == null)
                {
                    var connectionString = new Sap.Data.Hana.HanaConnectionStringBuilder(connStr);

                    HttpContext.Current.Items[EfContextKeyDbSap_Name] = connectionString.CurrentSchema;
                }
                return (string)HttpContext.Current.Items[EfContextKeyDbSap_Name];
            }
        }

        public static string dbSap_Name2
        {

            get
            {
                string connStr2 = ConfigurationManager.ConnectionStrings["HANA_SAP2"].ToString();
                if (HttpContext.Current.Items[EfContextKeyDbSap_Name2] == null)
                {
                    var connectionString = new Sap.Data.Hana.HanaConnectionStringBuilder(connStr2);

                    HttpContext.Current.Items[EfContextKeyDbSap_Name2] = connectionString.CurrentSchema;
                }
                return (string)HttpContext.Current.Items[EfContextKeyDbSap_Name2];
            }
        }

    }

}