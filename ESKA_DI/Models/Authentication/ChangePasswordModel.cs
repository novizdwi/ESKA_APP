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

namespace Models.Authentication.ChangePassword
{

    #region Models

    public class ChangePasswordModel
    {

        public int UserId { get; set; }

        [Required(ErrorMessage = "required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "required")]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The password you entered do not match")]
        public string NewPassword_Confirm { get; set; }

    }
    #endregion

    #region Services

    public class ChangePasswordService
    {

        public void ChangePassword(ChangePasswordModel model)
        {
            if (model != null)
            {
                if (model.NewPassword != model.NewPassword_Confirm)
                {
                    throw new Exception("[VALIDATION]-The password you entered do not match");
                }

                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            string ssql = "SELECT COUNT(*) AS IDU "
                                        + " FROM \"Tm_User\" T0 "
                                        + " WHERE T0.\"Id\"=:p0 AND T0.\"Pwd\"=:p1 ";



                            if (CONTEXT.Database.SqlQuery<long>(ssql, model.UserId, Encryption.Encrypt(model.OldPassword)).FirstOrDefault() <= 0)
                            {
                                throw new Exception("[VALIDATION]-Old password not valid");
                            }

                            Tm_User tm_User = CONTEXT.Tm_User.Find(model.UserId);
                            if (tm_User != null)
                            {
                                tm_User.Pwd = Encryption.Encrypt(model.NewPassword);
                                CONTEXT.SaveChanges();
                            }

                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception)
                        {
                            CONTEXT_TRANS.Rollback();
                        }
                    }
                }

            }
        }

    }

    #endregion

}