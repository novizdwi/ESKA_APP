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

namespace Models.Authentication.Login
{

    #region Models

    public class LoginModel
    {

        [Required(ErrorMessage = "required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "required")]
        public string Pwd { get; set; }

    }

    public class LoginInfoModel
    {

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string Departement { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string LastController { get; set; }

    }

    #endregion

    #region Services

    public class LoginService
    {

        public bool Login(LoginModel model)
        {
            string ssql = "SELECT COUNT(*) AS IDU "
                              + " FROM \"Tm_User\" T0 "
                              + " WHERE T0.\"UserName\"=:p0 AND T0.\"Pwd\"=:p1 ";
            long flag;

            using (var CONTEXT = new HANA_APP())
            {
                flag = CONTEXT.Database.SqlQuery<long>(ssql, model.UserName, Encryption.Encrypt(model.Pwd)).FirstOrDefault();
            }

            if (flag <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }


        public LoginInfoModel GetLoginInfo(string userName)
        {

            string ssql = "SELECT TOP 1 T0.\"Id\" AS \"UserId\", T0.\"UserName\" AS \"UserName\", IFNULL(T1.\"RoleName\",'') AS \"RoleName\", T4.\"Name\" AS \"Departement\", T0.\"LastController\", T5.\"WhsCode\", T5.\"WhsName\" " 
                            + "  FROM \"Tm_User\" T0 "
                            + "  LEFT JOIN \"Tm_Role\" T1 ON T0.\"RoleId\"=T1.\"Id\"  "
                            + "  LEFT JOIN \"" + DbProvider.dbSap_Name.ToString() + "\".\"OHEM\" T3 ON T0.\"EmpId\"=T3.\"empID\" "
                            + "  LEFT JOIN \"" + DbProvider.dbSap_Name.ToString() + "\".\"OUDP\" T4 ON T3.\"dept\"=T4.\"Code\" "
                            + "  LEFT JOIN \"" + DbProvider.dbSap_Name.ToString() + "\".\"OWHS\" T5 ON ''=T5.\"WhsCode\" "
                            + "  WHERE T0.\"UserName\"=:p0 ";

            var model = new LoginInfoModel();

            using (var CONTEXT = new HANA_APP())
            {
                model = CONTEXT.Database.SqlQuery<LoginInfoModel>(ssql, userName).SingleOrDefault();
            } 


            return model;


        }

        public LoginInfoModel GetLoginInfo(int userId)
        {

            string ssql = "SELECT TOP 1 T0.\"Id\" AS \"UserId\", T0.\"UserName\" AS \"UserName\", IFNULL(T1.\"RoleName\",'') AS \"RoleName\", T4.\"Name\" AS \"Departement\", T5.\"WhsCode\" ,T5.\"WhsName\"  "
                            + "  FROM \"Tm_User\" T0 "
                            + "  LEFT JOIN \"Tm_Role\" T1 ON T0.\"RoleId\"=T1.\"Id\"  "
                            + "  LEFT JOIN \"" + DbProvider.dbSap_Name.ToString() + "\".\"OHEM\" T3 ON T0.\"EmpId\"=T3.\"empID\" "
                            + "  LEFT JOIN \"" + DbProvider.dbSap_Name.ToString() + "\".\"OUDP\" T4 ON T3.\"dept\"=T4.\"Code\" "
                            + "  LEFT JOIN \"" + DbProvider.dbSap_Name.ToString() + "\".\"OWHS\" T5 ON ''=T5.\"WhsCode\" "
                            + "  WHERE T0.\"Id\"=@0 "; 
            

            var model = new LoginInfoModel();

            using (var CONTEXT = new HANA_APP())
            {
               model = CONTEXT.Database.SqlQuery<LoginInfoModel>(ssql, userId).SingleOrDefault();
            } 

            return model;


        }



    }

    #endregion

}