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

namespace Models.Setting.Alert
{

    #region Models



    public class AlertModel
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
        public int? GroupId { get; set; }

        [Required(ErrorMessage = "required")]
        public string AlertName { get; set; }

        [Required(ErrorMessage = "required")]
        public string IsActive { get; set; }

        [Required(ErrorMessage = "required")]
        public int? Frequency { get; set; }

        [Required(ErrorMessage = "required")]
        public string Sql { get; set; }

        public List<Alert_UserModel> ListUsers_ = new List<Alert_UserModel>();

        public List<Alert_RoleModel> ListRoles_ = new List<Alert_RoleModel>();

        public string[] UserTicks_ { get; set; }

        public string[] RoleTicks_ { get; set; }

    }

    public class Alert_UserModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? Id { get; set; }

        public int? DetId { get; set; }

        public string IsTick { get; set; }

        public int UserId { get; set; }

        public string UserName_ { get; set; }
    }

    public class Alert_RoleModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? Id { get; set; }

        public int? DetId { get; set; }

        [Required(ErrorMessage = "required")]
        public string IsTick { get; set; }

        public int? RoleId { get; set; }

        public string RoleName_ { get; set; }
    }


    #endregion

    #region Services

    public class AlertService
    {
        public List<Alert_UserModel> GetAlert_Users(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", IFNULL(T1.\"DetId\",0) AS \"DetId\", T0.\"Id\" AS \"UserId\", T0.\"UserName\" AS \"UserName_\", IFNULL(T1.\"IsTick\",'') AS \"IsTick\"   "
                            + " FROM \"Tm_User\" T0 "
                            + " LEFT JOIN \"Tm_Alert_User\" T1 ON T0.\"Id\"=T1.\"UserId\"  AND T1.\"Id\"=:p1 "
                            + " ORDER BY T0.\"UserName\" ";

            return CONTEXT.Database.SqlQuery<Alert_UserModel>(ssql, id, id).ToList();
        }

        public List<Alert_RoleModel> GetAlert_Roles(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", IFNULL(T1.\"DetId\",0) AS \"DetId\", T0.\"Id\" AS \"RoleId\",T0.\"RoleName\" AS \"RoleName_\", IFNULL(T1.\"IsTick\",'') AS \"IsTick\"   "
                            + " FROM \"Tm_Role\" T0 "
                            + " LEFT JOIN \"Tm_Alert_Role\" T1 ON T0.\"Id\"=T1.\"RoleId\"  AND T1.\"Id\"=:p1 "
                            + " ORDER BY T0.\"RoleName\"";

            return CONTEXT.Database.SqlQuery<Alert_RoleModel>(ssql, id, id).ToList();
        }


        public void Add(AlertModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            int Id = 0;

                            Tm_Alert tm_Alert = new Tm_Alert();
                            CopyProperty.CopyProperties(model, tm_Alert, false);

                            tm_Alert.TransType = "Alert";

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_Alert.CreatedDate = dtModified;
                            tm_Alert.CreatedUser = model._UserId;
                            tm_Alert.ModifiedDate = dtModified;
                            tm_Alert.ModifiedUser = model._UserId;

                            CONTEXT.Tm_Alert.Add(tm_Alert);
                            CONTEXT.SaveChanges();


                            Id = tm_Alert.Id;

                            Detail_User(CONTEXT, model, Id);
                            Detail_Role(CONTEXT, model, Id);

                            String keyValue;
                            keyValue = tm_Alert.Id.ToString();
                            SpNotif.SpSysTransNotif(model._UserId,CONTEXT, "after", "Tm_Alert", "add", "Id", keyValue);

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

        public void Update(AlertModel model)
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

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_Alert", "update", "Id", keyValue);


                            Tm_Alert tm_Alert = CONTEXT.Tm_Alert.Find(model.Id);
                            if (tm_Alert != null)
                            {
                                var exceptColumns = new string[] { "Id", "TransType" };
                                CopyProperty.CopyProperties(model, tm_Alert, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tm_Alert.ModifiedDate = dtModified;
                                tm_Alert.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();


                                Detail_User(CONTEXT, model, model.Id);
                                Detail_Role(CONTEXT, model, model.Id);

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Alert", "update", "Id", keyValue);


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

        public void Delete(AlertModel model)
        {
            if (model != null)
            {
                if (model.Id != 0)
                {
                    DeleteById(model.Id);
                }
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
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tm_Alert", "delete", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Alert\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Alert_Role\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Alert_User\"  WHERE \"Id\"=:p0", id);

                            SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tm_Alert", "delete", "Id", keyValue);
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

        public void Detail_User(HANA_APP CONTEXT, AlertModel pModel, int pId)
        {

            //------------------------------
            //Detail
            //------------------------------  
            var Ticks_ = pModel.UserTicks_;
            if (Ticks_ == null)
            {
                Ticks_ = new string[] { };

            }

            List<Tm_User> items = CONTEXT.Tm_User.ToList();

            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = "SELECT TOP 1 T0.\"DetId\" FROM  \"Tm_Alert_User\" T0 WHERE T0.\"Id\" = :p0 AND T0.\"UserId\"=:p1 ";

                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Alert_User();
                    item.Id = pId;
                    item.UserId = items[i].Id;
                    if (Ticks_.Contains(items[i].Id.ToString()))
                    {
                        item.IsTick = "Y";
                    }
                    else
                    {
                        item.IsTick = "N";
                    }

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.Tm_Alert_User.Add(item);
                    CONTEXT.SaveChanges();

                }
                else
                {
                    var item = CONTEXT.Tm_Alert_User.Find(DetId);
                    if (Ticks_.Contains(items[i].Id.ToString()))
                    {
                        item.IsTick = "Y";
                    }
                    else
                    {
                        item.IsTick = "N";
                    }

                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.SaveChanges();


                }
            }


        }

        public void Detail_Role(HANA_APP CONTEXT, AlertModel pModel, int pId)
        {

            //------------------------------
            //Detail
            //------------------------------   
            var Ticks_ = pModel.RoleTicks_;
            if (Ticks_ == null)
            {
                Ticks_ = new string[] { };

            }

            List<Tm_Role> items = CONTEXT.Tm_Role.ToList();

            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = "SELECT TOP 1 T0.\"DetId\" FROM  \"Tm_Alert_Role\" T0 WHERE T0.\"Id\" =:p0 AND T0.\"RoleId\"=:p1 ";

                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Alert_Role();
                    item.Id = pId;
                    item.RoleId = items[i].Id;
                    if (Ticks_.Contains(items[i].Id.ToString()))
                    {
                        item.IsTick = "Y";
                    }
                    else
                    {
                        item.IsTick = "N";
                    }

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.Tm_Alert_Role.Add(item);
                    CONTEXT.SaveChanges();


                }
                else
                {
                    var item = CONTEXT.Tm_Alert_Role.Find(DetId);
                    if (Ticks_.Contains(items[i].Id.ToString()))
                    {
                        item.IsTick = "Y";
                    }
                    else
                    {
                        item.IsTick = "N";
                    }

                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.SaveChanges();
                }
            } 
        } 

        public AlertModel GetNewModel()
        {
            AlertModel model = new AlertModel();
            using (var CONTEXT = new HANA_APP())
            {
                model.ListUsers_ = this.GetAlert_Users(CONTEXT, 0);
                model.ListRoles_ = this.GetAlert_Roles(CONTEXT, 0);
            }
            return model;
        }

        public AlertModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }


        public AlertModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            AlertModel model = null;
            if (id != 0)
            {
                model = CONTEXT.Database.SqlQuery<AlertModel>("SELECT T0.* FROM \"Tm_Alert\" T0 WHERE T0.\"Id\"=:p0 ", id).Single();

                if (model != null)
                { 
                    model.ListUsers_ = this.GetAlert_Users(CONTEXT,id);
                    model.ListRoles_ = this.GetAlert_Roles(CONTEXT,id);
                }
            }

            return model;
        }

        public AlertModel NavFirst()
        {
            AlertModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Alert\" T0 ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT,Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public AlertModel NavPrevious(int id = 0)
        {
            AlertModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Alert\" T0 WHERE T0.\"Id\"<:p0 ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public AlertModel NavNext(int id = 0)
        {
            AlertModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Alert\" T0 WHERE T0.\"Id\">:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, Id.Value);
                }
                //else
                //{
                //    model = model = this.NavLast();
                //}
            }

            if (model == null)
            {
                model = model = this.NavLast();
            }


            return model;

        }

        public AlertModel NavLast()
        {
            AlertModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Alert\" T0 ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }


    }

    #endregion

}