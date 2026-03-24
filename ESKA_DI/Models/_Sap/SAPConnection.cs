using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models._Sap
{
    public class SAPConnection
    {
        public SAPbobsCOM.Company CompanyInstance = null;
        public string Identity = "";
        public bool InUse = false;
        //
        //New function, create an object of SAPbobsCOM.Company and setup the connection
        //

        public SAPConnection(SAPbobsCOM.BoDataServerTypes Servertype, string Servername, string Dbuser, string Dbpassword, string SAPDBName, string SAPUser, string SAPUSerPassword, string SAPLicense)
        {
            int errCode = 0;
            string errMsg = "";

            dynamic oCompany = new SAPbobsCOM.Company();
            oCompany.DbServerType = Servertype;
            oCompany.Server = Servername;
            oCompany.DbUserName = Dbuser;
            oCompany.DbPassword = Dbpassword;
            oCompany.CompanyDB = SAPDBName;
            oCompany.UserName = SAPUser;
            oCompany.Password = SAPUSerPassword;
            oCompany.LicenseServer = SAPLicense;
            oCompany.UseTrusted = false;
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;

            // oCompany.ObserverDLL_DEBUG_Path = "D:/PROJECT/BAYU BUANA/APP/HANA/SAP_ObserverDLL_DEBUG_Path";

            try
            {
                if (oCompany.Connect() == 0)
                {
                    Identity = Servertype.ToString() + ";";
                    Identity = Identity + Servername + ";";
                    Identity = Identity + Dbuser + ";";
                    Identity = Identity + Dbpassword + ";";
                    Identity = Identity + SAPDBName + ";";
                    Identity = Identity + SAPUser + ";";
                    Identity = Identity + SAPUSerPassword + ";";
                    Identity = Identity + SAPLicense + ";";

                    CompanyInstance = oCompany;
                }
                else
                {
                    oCompany.GetLastError(out errCode, out errMsg);
                    throw new Exception("ErrorCode:" + errCode.ToString() + "|" + "ErrorDesc:" + errMsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}