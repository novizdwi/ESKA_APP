using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;

namespace Models.Setting.AlertGroup
{

    #region Models


    public static class AlertGroupGetList
    {

    }

    public class AlertGroupModel
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
        public string GroupName { get; set; }

        public string Description { get; set; }

        public string SortCode { get; set; }

    }



    #endregion

    #region Services

    public class AlertGroupService
    {


        public void Add(AlertGroupModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_AlertGroup tm_AlertGroup = new Tm_AlertGroup();
                            CopyProperty.CopyProperties(model, tm_AlertGroup, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tm_AlertGroup.TransType = "AlertGroup";
                            tm_AlertGroup.CreatedDate = dtModified;
                            tm_AlertGroup.CreatedUser = model._UserId;
                            tm_AlertGroup.ModifiedDate = dtModified;
                            tm_AlertGroup.ModifiedUser = model._UserId;

                            CONTEXT.Tm_AlertGroup.Add(tm_AlertGroup);
                            CONTEXT.SaveChanges();

                            String keyValue;
                            keyValue = tm_AlertGroup.Id.ToString();
                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_AlertGroup", "add", "Id", keyValue);

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

        public void Update(AlertGroupModel model)
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

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_AlertGroup", "update", "Id", keyValue);


                            Tm_AlertGroup tm_AlertGroup = CONTEXT.Tm_AlertGroup.Find(model.Id);
                            if (tm_AlertGroup != null)
                            {
                                var exceptColumns = new string[] { "Id" };
                                CopyProperty.CopyProperties(model, tm_AlertGroup, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tm_AlertGroup.ModifiedDate = dtModified;
                                tm_AlertGroup.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_AlertGroup", "update", "Id", keyValue);

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

        public void Delete(AlertGroupModel model)
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
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tm_AlertGroup", "delete", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_AlertGroup\" WHERE \"Id\"=:p0 ", id);

                            SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tm_AlertGroup", "delete", "Id", keyValue);
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


        public AlertGroupModel GetNewModel()
        {
            AlertGroupModel model = new AlertGroupModel();


            return model;
        }

        public AlertGroupModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public AlertGroupModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            AlertGroupModel model = null;
            if (id != 0)
            {
                model = CONTEXT.Database.SqlQuery<AlertGroupModel>("SELECT T0.* FROM \"Tm_AlertGroup\" T0 WHERE T0.\"Id\"=:p0 ", id).Single();
            }

            return model;
        }

        public AlertGroupModel NavFirst()
        {
            AlertGroupModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_AlertGroup\" T0 ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public AlertGroupModel NavPrevious(int id = 0)
        {
            AlertGroupModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_AlertGroup\" T0 WHERE T0.\"Id\"<:p0 ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public AlertGroupModel NavNext(int id = 0)
        {
            AlertGroupModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_AlertGroup\" T0 WHERE T0.\"Id\">:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public AlertGroupModel NavLast()
        {
            AlertGroupModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_AlertGroup\" T0 ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }



    }

    #endregion

}