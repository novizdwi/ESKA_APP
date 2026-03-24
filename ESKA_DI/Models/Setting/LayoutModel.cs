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

namespace Models.Setting.Layout
{

    #region Models

    public static class LayoutGetList
    {

    }

    public class LayoutModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }


        public int _UserId { get; set; }

        public string _HiddenUid { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "required")]
        public string LayoutFormCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string LayoutName { get; set; }

        public string Description { get; set; }

        public Guid? Uid { get; set; }

        public byte[] Data { get; set; }

        [Required(ErrorMessage = "required")]
        public string IsActive { get; set; }

        [Required(ErrorMessage = "required")]
        public string OutputType { get; set; }

        public List<Layout_UserModel> ListUsers_ = new List<Layout_UserModel>();

        public List<Layout_RoleModel> ListRoles_ = new List<Layout_RoleModel>();

        public string[] UserTicks_ { get; set; }

        public string[] RoleTicks_ { get; set; }

    }

    public class Layout_UserModel
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

    public class Layout_RoleModel
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

        public int? RoleId { get; set; }

        public string RoleName_ { get; set; }
    }


    #endregion

    #region Services

    public class LayoutService
    {

        public List<Layout_UserModel> GetLayout_Users(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", IFNULL(T1.\"DetId\",0) AS \"DetId\", T0.\"Id\" AS \"UserId\",T0.\"UserName\" AS \"UserName_\", IFNULL(T1.\"IsTick\",'') AS \"IsTick\"   "
                            + " FROM \"Tm_User\" T0 "
                            + " LEFT JOIN \"Tm_Layout_User\" T1 ON T0.\"Id\"=T1.\"UserId\"  AND T1.\"Id\"=:p1 "
                            + " ORDER BY T0.\"UserName\"";

            return CONTEXT.Database.SqlQuery<Layout_UserModel>(ssql, id, id).ToList();
        }

        public List<Layout_RoleModel> GetLayout_Roles(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", IFNULL(T1.\"DetId\",0) AS \"DetId\", T0.\"Id\" AS \"RoleId\",T0.\"RoleName\" AS \"RoleName_\", IFNULL(T1.\"IsTick\",'') AS \"IsTick\"   "
                           + " FROM \"Tm_Role\" T0 "
                           + " LEFT JOIN \"Tm_Layout_Role\" T1 ON T0.\"Id\"=T1.\"RoleId\"  AND T1.\"Id\"=:p1 "
                           + " ORDER BY T0.\"RoleName\"";

            return CONTEXT.Database.SqlQuery<Layout_RoleModel>(ssql, id, id).ToList();
        }


        public void Add(LayoutModel model)
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

                            Tm_Layout tm_Layout = new Tm_Layout();
                            CopyProperty.CopyProperties(model, tm_Layout, false);

                            tm_Layout.TransType = "Layout";

                            if (model._HiddenUid != "")
                            {
                                tm_Layout.Uid = model._HiddenUid;
                            }

                            if ((model._HiddenUid != "") && (model.Data != null))
                            {
                                tm_Layout.Data = model.Data;
                            }

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_Layout.CreatedDate = dtModified;
                            tm_Layout.CreatedUser = model._UserId;
                            tm_Layout.ModifiedDate = dtModified;
                            tm_Layout.ModifiedUser = model._UserId;

                            CONTEXT.Tm_Layout.Add(tm_Layout);
                            CONTEXT.SaveChanges();


                            Id = tm_Layout.Id;

                            Detail_User(CONTEXT, model, Id);
                            Detail_Role(CONTEXT, model, Id);

                            String keyValue;
                            keyValue = tm_Layout.Id.ToString();
                            SpNotif.SpSysTransNotif(model._UserId,CONTEXT, "after", "Tm_Layout", "add", "Id", keyValue);


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

        public void Update(LayoutModel model)
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

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_Layout", "update", "Id", keyValue);


                            Tm_Layout tm_Layout = CONTEXT.Tm_Layout.Find(model.Id);
                            if (tm_Layout != null)
                            {
                                var exceptColumns = new string[] { "Id", "Uid", "Data", "TransType" };
                                CopyProperty.CopyProperties(model, tm_Layout, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                if (model._HiddenUid != "")
                                {
                                    tm_Layout.Uid = model._HiddenUid;
                                }

                                if ((model._HiddenUid != "") && (model.Data != null))
                                {
                                    tm_Layout.Data = model.Data;
                                }

                                tm_Layout.ModifiedDate = dtModified;
                                tm_Layout.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();


                                Detail_User(CONTEXT, model, model.Id);
                                Detail_Role(CONTEXT, model, model.Id);

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Layout", "update", "Id", keyValue);

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

        public void Delete(LayoutModel model)
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
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tm_Layout", "delete", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Layout\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Layout_Role\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Layout_User\"  WHERE \"Id\"=:p0", id);

                            SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tm_Layout", "delete", "Id", keyValue);
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


        public void Detail_User(HANA_APP CONTEXT, LayoutModel pModel, int pId)
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
                var ssql2 = "SELECT TOP 1 T0.\"DetId\" FROM  \"Tm_Layout_User\" T0 WHERE T0.\"Id\" = :p0 AND T0.\"UserId\"=:p1 ";

                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Layout_User();
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

                    CONTEXT.Tm_Layout_User.Add(item);
                    CONTEXT.SaveChanges();

                }
                else
                {
                    var item = CONTEXT.Tm_Layout_User.Find(DetId);
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

        public void Detail_Role(HANA_APP CONTEXT, LayoutModel pModel, int pId)
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
                var ssql2 = "SELECT TOP 1 T0.\"DetId\" FROM  \"Tm_Layout_Role\" T0 WHERE T0.\"Id\" =:p0 AND T0.\"RoleId\"=:p1 ";

                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Layout_Role();
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

                    CONTEXT.Tm_Layout_Role.Add(item);
                    CONTEXT.SaveChanges();


                }
                else
                {
                    var item = CONTEXT.Tm_Layout_Role.Find(DetId);
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


        public LayoutModel GetNewModel()
        {
            LayoutModel model = new LayoutModel();
            using (var CONTEXT = new HANA_APP())
            {
                model.ListUsers_ = this.GetLayout_Users(CONTEXT, 0);
                model.ListRoles_ = this.GetLayout_Roles(CONTEXT, 0);
            }
            return model;
        }

        public LayoutModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public LayoutModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            LayoutModel model = null;
            if (id != 0)
            {

                model = CONTEXT.Database.SqlQuery<LayoutModel>("SELECT T0.* FROM \"Tm_Layout\" T0 WHERE T0.\"Id\"=:p0 ", id).Single();

                if (model != null)
                {
                    model.ListUsers_ = this.GetLayout_Users(CONTEXT, id);
                    model.ListRoles_ = this.GetLayout_Roles(CONTEXT, id);
                }
            }

            return model;
        }

        public LayoutModel NavFirst()
        {
            LayoutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Layout\" T0 ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public LayoutModel NavPrevious(int id = 0)
        {
            LayoutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Layout\" T0 WHERE T0.\"Id\"<:p0 ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public LayoutModel NavNext(int id = 0)
        {
            LayoutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Layout\" T0 WHERE T0.\"Id\">:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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
                model = this.NavLast();
            }

            return model;

        }

        public LayoutModel NavLast()
        {
            LayoutModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Layout\" T0 ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

    }

    #endregion

}