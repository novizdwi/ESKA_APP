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

using Models._Sap;
using SAPbobsCOM;

namespace Models.Setting.ApprovalStage
{

    #region Models
    public class ApprovalStageModel
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
        public string StageName { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "required")]
        public int? MinApprove { get; set; }

        [Required(ErrorMessage = "required")]
        public int? MinReject { get; set; }

        public List<ApprovalStage_UserModel> ListUsers_ = new List<ApprovalStage_UserModel>();

        public List<ApprovalStage_RoleModel> ListRoles_ = new List<ApprovalStage_RoleModel>();

        public string[] UserTicks_ { get; set; }

        public string[] RoleTicks_ { get; set; }

    }

    public class ApprovalStage_UserModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? Id { get; set; }

        public long? DetId { get; set; }

        public string IsTick { get; set; }

        public int UserId { get; set; }

        public string UserName_ { get; set; }

        public string FirstName_ { get; set; }

        public string RoleName_ { get; set; }


    }

    public class ApprovalStage_RoleModel
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

    public class ApprovalStageService
    {

        public List<ApprovalStage_UserModel> GetApprovalStage_Users(long id = 0)
        {
            List<ApprovalStage_UserModel> models;
            string ssql = @"
                SELECT  T0.""Id"" AS ""Id"", 
                        COALESCE(T1.""DetId"",0) AS ""DetId"", 
                        T0.""Id"" AS ""UserId"", 
                        T0.""UserName"" AS ""UserName_"", 
                        T0.""FirstName"" AS ""FirstName_"",
                        COALESCE(T1.""IsTick"",'') AS ""IsTick"", 
                        T2.""RoleName"" as ""RoleName_"" 
                FROM ""Tm_User"" T0 
                LEFT JOIN ""Tm_ApprovalStage_User"" T1 ON T0.""Id"" = T1.""UserId""  AND T1.""Id"" = :p0 
                LEFT JOIN ""Tm_Role"" T2 ON T0.""RoleId"" = T2.""Id"" 
                ORDER BY T0.""UserName"" 
            ";

            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<ApprovalStage_UserModel>(ssql, id).ToList();
            }

            return models;

        }

        public List<ApprovalStage_RoleModel> GetApprovalStage_Roles(long id = 0)
        {
            List<ApprovalStage_RoleModel> models;
            string ssql = @"
                SELECT  T0.""Id"" AS ""Id"", 
                        COALESCE(T1.""DetId"", 0) AS ""DetId"", 
                        T0.""Id"" AS ""RoleId"", 
                        T0.""RoleName"" AS ""RoleName_"", 
                        COALESCE(T1.""IsTick"", '') AS ""IsTick"" 
                FROM ""Tm_Role"" T0 
                LEFT JOIN ""Tm_ApprovalStage_Role"" T1 ON T0.""Id"" = T1.""RoleId"" AND T1.""Id"" = :p0 
                ORDER BY T0.""RoleName""
            ";

            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<ApprovalStage_RoleModel>(ssql, id).ToList();
            }

            return models;
        }

        public int Add(ApprovalStageModel model)
        {
            int Id = 0;
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            Tm_ApprovalStage tm_ApprovalStage = new Tm_ApprovalStage();
                            CopyProperty.CopyProperties(model, tm_ApprovalStage, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tm_ApprovalStage.TransType = "ApprovalStage";

                            tm_ApprovalStage.CreatedDate = dtModified;
                            tm_ApprovalStage.CreatedUser = model._UserId;
                            tm_ApprovalStage.ModifiedDate = dtModified;
                            tm_ApprovalStage.ModifiedUser = model._UserId;

                            CONTEXT.Tm_ApprovalStage.Add(tm_ApprovalStage);
                            CONTEXT.SaveChanges();
                            Id = tm_ApprovalStage.Id;

                            String keyValue;
                            keyValue = tm_ApprovalStage.Id.ToString();

                            Detail_User(CONTEXT, model, Id);
                            //Detail_Role(CONTEXT, model, Id);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "ApprovalStage", CONTEXT, "after", "ApprovalStage", "add", "Id", keyValue);
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

        public void Update(ApprovalStageModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "ApprovalStage", CONTEXT, "before", "ApprovalStage", "update", "Id", keyValue);

                        Tm_ApprovalStage tm_ApprovalStage = CONTEXT.Tm_ApprovalStage.Find(model.Id);
                        if (tm_ApprovalStage != null)
                        {
                            var exceptColumns = new string[] { "Id", "TransType" };
                            CopyProperty.CopyProperties(model, tm_ApprovalStage, false, exceptColumns);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_ApprovalStage.ModifiedDate = dtModified;
                            tm_ApprovalStage.ModifiedUser = model._UserId;
                            CONTEXT.SaveChanges();

                            Detail_User(CONTEXT, model, model.Id);
                            //Detail_Role(CONTEXT, model, Id);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "ApprovalStage", CONTEXT, "after", "ApprovalStage", "update", "Id", keyValue);
                        }
                        CONTEXT_TRANS.Commit();

                    }

                }

            }
        }

        public void Delete(ApprovalStageModel model)
        {
            if (model != null)
            {
                if (model.Id != 0)
                {
                    DeleteById(model._UserId, model.Id);
                }
            }
        }

        public void DeleteById(int UserId, int id = 0)
        {
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {

                        Tm_ApprovalStage tm_ApprovalStage = CONTEXT.Tm_ApprovalStage.Find(id);
                        SpNotif.SpSysControllerTransNotif(UserId, "ApprovalStage", CONTEXT, "before", "ApprovalStage", "delete", "Id", id.ToString());
                        
                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_ApprovalStage_Role""  WHERE ""Id"" = :p0 ", id);

                        CONTEXT.Tm_ApprovalStage.Remove(tm_ApprovalStage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(UserId, "ApprovalStage", CONTEXT, "after", "ApprovalStage", "delete", "Id", id.ToString());

                        CONTEXT_TRANS.Commit();
                    }
                }

            }
        }


        public ApprovalStageModel GetNewModel()
        {
            ApprovalStageModel model = new ApprovalStageModel();
            model.MinApprove = 1;
            model.MinReject = 1;

            model.ListUsers_ = this.GetApprovalStage_Users(0);
            model.ListRoles_ = this.GetApprovalStage_Roles(0);

            return model;
        }

        public void Detail_User(HANA_APP CONTEXT, ApprovalStageModel pModel, int pId)
        {
            //------------------------------
            //Detail
            //------------------------------ 
            var ssql = @"SELECT * FROM  ""Tm_User"" T0 ";
            var Ticks_ = pModel.UserTicks_ ?? new string[] { };
            List<Tm_User> items = CONTEXT.Database.SqlQuery<Tm_User>(ssql).ToList();

            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = @"SELECT TOP 1 T0.""DetId"" FROM  ""Tm_ApprovalStage_User"" T0 WHERE T0.""Id"" = :p0 AND T0.""UserId"" = :p1 "; 
                long? DetId = CONTEXT.Database.SqlQuery<long?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_ApprovalStage_User();
                    item.Id = pId;
                    item.UserId = items[i].Id;
                    item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.Tm_ApprovalStage_User.Add(item);
                    CONTEXT.SaveChanges();

                    DetId = item.DetId;
                }
                else
                {
                    Tm_ApprovalStage_User item = CONTEXT.Tm_ApprovalStage_User.Find(DetId);
                    if (item != null)
                    {
                        item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                        item.ModifiedUser = pModel._UserId;
                        item.ModifiedDate = dtModified;

                        CONTEXT.SaveChanges();
                    }
                }
            }

        }

        public void Detail_Role(HANA_APP CONTEXT, ApprovalStageModel pModel, int pId)
        {
            //------------------------------
            //Detail
            //------------------------------ 
            var ssql = @"SELECT * FROM  ""Tm_User"" T0 ";
            var Ticks_ = pModel.UserTicks_?? new string[] { };
            List<Tm_User> items = CONTEXT.Database.SqlQuery<Tm_User>(ssql).ToList();

            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = @"SELECT TOP 1 T0.""DetId"" FROM  ""Tm_ApprovalStage_Role"" T0 WHERE T0.""Id"" = :p0  "; //AND T0.""UserId"" = :p1
                long? DetId = CONTEXT.Database.SqlQuery<long?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_ApprovalStage_Role();
                    item.Id = pId;
                    item.RoleId = items[i].Id;
                    item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;
                            
                    CONTEXT.Tm_ApprovalStage_Role.Add(item);
                    CONTEXT.SaveChanges();

                    DetId = item.DetId;
                }
                else
                {
                    Tm_ApprovalStage_Role item = CONTEXT.Tm_ApprovalStage_Role.Find(DetId);
                    if (item != null)
                    {
                        item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                        item.ModifiedUser = pModel._UserId;
                        item.ModifiedDate = dtModified;

                        CONTEXT.SaveChanges();
                    }
                }
            }

        }

        public ApprovalStageModel GetById(int userId, long id = 0)
        {
            ApprovalStageModel model = null;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Tm_ApprovalStage tm_ApprovalStage;
                    tm_ApprovalStage = CONTEXT.Tm_ApprovalStage.Find(id);

                    if (tm_ApprovalStage != null)
                    {
                        model = new ApprovalStageModel();
                        CopyProperty.CopyProperties(tm_ApprovalStage, model, false);

                        model.ListUsers_ = this.GetApprovalStage_Users(id);
                        model.ListRoles_ = this.GetApprovalStage_Roles(id);
                    }
                }

            }

            return model;
        }

        public ApprovalStageModel NavFirst(int userId)
        {
            ApprovalStageModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Tm_ApprovalStage");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_ApprovalStage\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(userId, Id.HasValue ? Id.Value : 0);
            }
            return model;

        }

        public ApprovalStageModel NavPrevious(int userId, long id = 0)
        {
            ApprovalStageModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalStage");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_ApprovalStage\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }

            return model;
        }

        public ApprovalStageModel NavNext(int userId, long id = 0)
        {
            ApprovalStageModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalStage");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_ApprovalStage\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }
            return model;
        }

        public ApprovalStageModel NavLast(int userId)
        {
            ApprovalStageModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalStage");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_ApprovalStage\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(userId, Id.HasValue ? Id.Value : 0);

            }
            return model;
        }

    }

    #endregion

}