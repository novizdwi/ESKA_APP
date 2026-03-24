using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;


using System.IO;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models.Authentication.Role
{

    #region Models



    public class RoleModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "required")]
        public string RoleName { get; set; }

        public string ChkValues { get; set; }

        public string ChkNames { get; set; }

        public List<Role_AuthModel> role_Auths { get; set; }

    }

    public class Role_AuthModel
    {
        public int Id { get; set; }
        public string MenuCode { get; set; }
        public string IsAccess { get; set; }
        public string MenuName { get; set; }
        public string ParentCode { get; set; }
        public string Url { get; set; }
    }

    #endregion

    #region Services

    public class RoleService
    {
        public List<Role_AuthModel> GetRole_Auths(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", T0.\"MenuCode\" AS \"MenuCode\", IFNULL(T1.\"IsAccess\",'') AS \"IsAccess\", T0.\"MenuName\",T0.\"ParentCode\",T0.\"Url\" "
                            + " FROM \"Ts_Menu\" T0 "
                            + " LEFT JOIN \"Tm_Role_Auth\" T1 ON T0.\"MenuCode\"=T1.\"MenuCode\"  AND T1.\"Id\"=:p1 "
                            + " ORDER BY T0.\"Sort\"";

            var items = new List<Role_AuthModel>();
            items = CONTEXT.Database.SqlQuery<Role_AuthModel>(ssql, id, id).ToList();
            return items;
        }

        public void Add(RoleModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_Role tm_Role = new Tm_Role();
                            CopyProperty.CopyProperties(model, tm_Role, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tm_Role.TransType = "Role";
                            tm_Role.CreatedDate = dtModified;
                            tm_Role.CreatedUser = model._UserId;
                            tm_Role.ModifiedDate = dtModified;
                            tm_Role.ModifiedUser = model._UserId;

                            CONTEXT.Tm_Role.Add(tm_Role);
                            CONTEXT.SaveChanges();

                            var Id = tm_Role.Id;

                            string keyValue;
                            keyValue = Id.ToString();

                            if (model.role_Auths != null)
                            {
                                if (model.role_Auths.Count > 0)
                                {
                                    for (int i = 0; i < model.role_Auths.Count; i++)
                                    {
                                        Tm_Role_Auth tm_Role_Auth = new Tm_Role_Auth();
                                        tm_Role_Auth.Id = Id;
                                        tm_Role_Auth.MenuCode = model.role_Auths[i].MenuCode;
                                        tm_Role_Auth.IsAccess = model.role_Auths[i].IsAccess;
                                        CONTEXT.Tm_Role_Auth.Add(tm_Role_Auth);
                                        CONTEXT.SaveChanges();
                                    }
                                }
                            }

                            //String keyValue;
                            //keyValue = tm_Role.Id.ToString();
                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Role", "add", "Id", keyValue);

                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMassage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMassage = ex.Message;
                            }
                            else
                            {
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }


            }
        }

        public void Update(RoleModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            String keyValue;
                            keyValue = model.Id.ToString();

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_Role", "update", "Id", keyValue);

                            Tm_Role tm_Role = CONTEXT.Tm_Role.Find(model.Id);
                            if (tm_Role != null)
                            {
                                var exceptColumns = new string[] { "Id", "TransType" };
                                CopyProperty.CopyProperties(model, tm_Role, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tm_Role.ModifiedDate = dtModified;
                                tm_Role.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();

                                if (model.role_Auths != null)
                                {
                                    if (model.role_Auths.Count > 0)
                                    {
                                        for (int i = 0; i < model.role_Auths.Count; i++)
                                        {

                                            Tm_Role_Auth tm_Role_Auth = CONTEXT.Tm_Role_Auth.SqlQuery("SELECT * FROM \"Tm_Role_Auth\" T0 WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1", model.Id, model.role_Auths[i].MenuCode).SingleOrDefault();

                                            if (tm_Role_Auth == null)
                                            {
                                                tm_Role_Auth = new Tm_Role_Auth();
                                                tm_Role_Auth.Id = model.Id;
                                                tm_Role_Auth.MenuCode = model.role_Auths[i].MenuCode;
                                                tm_Role_Auth.IsAccess = model.role_Auths[i].IsAccess;
                                                CONTEXT.Tm_Role_Auth.Add(tm_Role_Auth);
                                            }
                                            else
                                            {
                                                tm_Role_Auth.Id = model.Id;
                                                tm_Role_Auth.MenuCode = model.role_Auths[i].MenuCode;
                                                tm_Role_Auth.IsAccess = model.role_Auths[i].IsAccess;
                                            }
                                            CONTEXT.SaveChanges();
                                        }

                                    }
                                }

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Role", "update", "Id", keyValue);

                            }

                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMassage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMassage = ex.Message;
                            }
                            else
                            {
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }


            }
        }

        public void Delete(RoleModel model)
        {
            if (model != null)
            {
                DeleteById(model.Id);
            }
        }

        public void DeleteById(int id = 0)
        {
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            string keyValue = id.ToString();
                            int userId = 1;
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tm_Role", "delete", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Role\" WHERE \"Id\"=:p0 ", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Role_Auth\" WHERE \"Id\"=:p0 ", id);

                            SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tm_Role", "delete", "Id", keyValue);
                            CONTEXT_TRANS.Commit();
                        }
                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMassage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMassage = ex.Message;
                            }
                            else
                            {
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }

            }
        }


        public RoleModel GetNewModel()
        {
            RoleModel model = new RoleModel();
            using (var CONTEXT = new HANA_APP())
            {
                model.role_Auths = this.GetRole_Auths(CONTEXT, 0);
            }
            return model;
        }

        public RoleModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public RoleModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            RoleModel model = null;
            if (id != 0)
            {
                model = CONTEXT.Database.SqlQuery<RoleModel>("SELECT T0.* FROM \"Tm_Role\" T0 WHERE T0.\"Id\"=:p0 ", id).Single();

                if (model != null)
                {
                    model.role_Auths = this.GetRole_Auths(CONTEXT, id);
                }
            }

            return model;
        }

        public RoleModel NavFirst()
        {
            RoleModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Role\" T0 ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public RoleModel NavPrevious(int id = 0)
        {
            RoleModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Role\" T0 WHERE T0.\"Id\"<:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, Id.Value);
                }
                //else
                //{
                //    model = this.NavFirst();
                //}
            }

            if (model == null)
            {
                model = this.NavFirst();
            }

            return model;

        }

        public RoleModel NavNext(int id = 0)
        {
            RoleModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Role\" T0 WHERE T0.\"Id\">:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, Id.Value);
                }
                //else
                //{
                //      model = this.NavLast();
                //}
            }

            if (model == null)
            {
                model = this.NavLast();
            }

            return model;

        }

        public RoleModel NavLast()
        {
            RoleModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Role\" T0 ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }


    }

    #endregion

}