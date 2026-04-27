using System;
using System.Linq;
using Models._Utils;
using ESKA_DI.Models._EF;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Master.Position
{
    #region Models

    public class PositionModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int Id { get; set; }

        public string Key { get; set; }

        public string PositionName { get; set; }

        public string IsActive { get; set; }

        public string CreatedDate_ { get; set; }

        public int CreatedUser_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public int ModifiedUser_ { get; set; }

    }
    #endregion

    #region Services

    public class PositionService
    {

        public PositionModel GetNewModel(int userItemCode)
        {
            PositionModel model = new PositionModel();
            model.IsActive = "Y";
            return model;
        }
        public PositionModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public PositionModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            PositionModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tm_Position"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<PositionModel>(ssql, id).Single();

               // model.ListDetails_ = this.Position_Details(CONTEXT, id);
            }

            return model;
        }

        public long Add(PositionModel model)
        {
            long Id = 0;

            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_Position Tm_Position = new Tm_Position();
                            CopyProperty.CopyProperties(model, Tm_Position, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tm_Position.PositionName = model.PositionName;
                            Tm_Position.IsActive = model.IsActive;
                            Tm_Position.CreatedDate = dtModified;
                            Tm_Position.CreatedUser = model._UserId;
                            Tm_Position.ModifiedDate = dtModified;
                            Tm_Position.ModifiedUser = model._UserId;


                            CONTEXT.Tm_Position.Add(Tm_Position);
                            CONTEXT.SaveChanges();
                            Id = Tm_Position.Id;

                            String keyValue;
                            keyValue = Tm_Position.Id.ToString();

                           // SpNotif.SpSysControllerTransNotif(model._UserId, "Position", CONTEXT, "after", "Position", "add", "Id", keyValue);

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

            return Id;

        }

        public void Update(PositionModel model)
        {
            if (model != null)
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

                                //SpNotif.SpSysControllerTransNotif(model._UserId, "Position", CONTEXT, "before", "Position", "update", "Id", keyValue);

                                Tm_Position Tm_Position = CONTEXT.Tm_Position.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tm_Position.ModifiedDate = dtModified;
                                Tm_Position.ModifiedUser = model._UserId;

                                if (Tm_Position != null)
                                {
                                    var exceptColumns = new string[] { "Id", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tm_Position, false, exceptColumns);
                                    Tm_Position.ModifiedDate = dtModified;
                                    Tm_Position.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                    //if (model.Details_ != null)
                                    //{
                                    //    if (model.Details_.insertedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.insertedRowValues)
                                    //        {
                                    //            Detail_Add(CONTEXT, detail, model.Id, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.Details_.modifiedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.modifiedRowValues)
                                    //        {
                                    //            Detail_Update(CONTEXT, detail, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.Details_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.Details_.deletedRowKeys)
                                    //        {
                                    //            Position_DetailModel detailModel = new Position_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}

                                   // SpNotif.SpSysControllerTransNotif(model._UserId, "Position", CONTEXT, "after", "Position", "update", "Id", keyValue);

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


        }

        public PositionModel NavFirst(int userId)
        {
            PositionModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Position");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Position\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public PositionModel NavPrevious(int userId, long id = 0)
        {
            PositionModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Position");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Position\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }


            return model;
        }

        public PositionModel NavNext(int userId, long id = 0)
        {
            PositionModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Position");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Position\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }

            return model;
        }

        public PositionModel NavLast(int userId)
        {
            PositionModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Position");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Position\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }


    }


    #endregion

}