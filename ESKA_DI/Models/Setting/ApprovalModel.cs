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

namespace Models.Setting.Approval
{

    #region Models
    public class ApprovalModel
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
        public string FormCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ApprovalName { get; set; }

        [Required(ErrorMessage = "required")]
        public string IsActive { get; set; } 

        public List<Approval_UserModel> ListUsers_ = new List<Approval_UserModel>();

        public List<Approval_RoleModel> ListRoles_ = new List<Approval_RoleModel>();

        public string[] UserTicks_ { get; set; }

        public string[] RoleTicks_ { get; set; }

    }

    public class Approval_UserModel
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

    public class Approval_RoleModel
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

    public class ApprovalService
    {
        public List<Approval_UserModel> GetApproval_Users(int id = 0)
        {
            List<Approval_UserModel> models;
            string ssql = @"
                SELECT  T0.""Id"" AS ""Id"", 
                        COALESCE(T1.""DetId"", 0) AS ""DetId"", 
                        T0.""Id"" AS ""UserId"", 
                        T0.""UserName"" AS ""UserName_"", 
                        COALESCE(T1.""IsTick"",'') AS ""IsTick"" 
                FROM ""Tm_User"" T0 
                LEFT JOIN ""Tm_Approval_User"" T1 ON T0.""Id"" = T1.""UserId"" AND T1.""Id"" = :p0 
                ORDER BY T0.""UserName"" 
            ";

            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<Approval_UserModel>(ssql, id).ToList();
            }

            return models;
        }

        public List<Approval_RoleModel> GetApproval_Roles(int id = 0)
        {
            List<Approval_RoleModel> models;
            string ssql = @"
                SELECT  T0.""Id"" AS ""Id"", 
                        COALESCE(T1.""DetId"", 0) AS ""DetId"", 
                        T0.""Id"" AS ""RoleId"", 
                        T0.""RoleName"" AS ""RoleName_"", 
                        COALESCE(T1.""IsTick"",'') AS ""IsTick""
                FROM ""Tm_Role"" T0 
                LEFT JOIN ""Tm_Approval_Role"" T1 ON T0.""Id"" = T1.""RoleId""  AND T1.""Id"" = :p0 
                ORDER BY T0.""RoleName"" 
            ";

            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<Approval_RoleModel>(ssql, id).ToList();
            }

            return models;
        }


        public void Add(ApprovalModel model)
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
                            Tm_Approval tm_Approval = new Tm_Approval();
                            CopyProperty.CopyProperties(model, tm_Approval, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tm_Approval.TransType = "Approval";

                            tm_Approval.CreatedDate = dtModified;
                            tm_Approval.CreatedUser = model._UserId;
                            tm_Approval.ModifiedDate = dtModified;
                            tm_Approval.ModifiedUser = model._UserId;

                            CONTEXT.Tm_Approval.Add(tm_Approval);
                            CONTEXT.SaveChanges();
                            Id = tm_Approval.Id;

                            String keyValue;
                            keyValue = tm_Approval.Id.ToString();

                            Detail_User(CONTEXT, model, Id);
                            Detail_Role(CONTEXT, model, Id);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "Approval", CONTEXT, "after", "Approval", "add", "Id", keyValue);
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

        public void Update(ApprovalModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();
                        SpNotif.SpSysControllerTransNotif(model._UserId, "before", CONTEXT, "after", "Approval", "update", "Id", keyValue);

                        Tm_Approval tm_Approval = CONTEXT.Tm_Approval.Find(model.Id);
                        if (tm_Approval != null)
                        {

                            var exceptColumns = new string[] { "Id", "TransType" };
                            CopyProperty.CopyProperties(model, tm_Approval, false, exceptColumns);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_Approval.ModifiedDate = dtModified;
                            tm_Approval.ModifiedUser = model._UserId;
                            CONTEXT.SaveChanges();

                            Detail_User(CONTEXT, model, model.Id);
                            Detail_Role(CONTEXT, model, model.Id);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "after", CONTEXT, "after", "Approval", "update", "Id", keyValue);
                        }
                        CONTEXT_TRANS.Commit();

                    }

                }

            }
        }

        public void Delete(ApprovalModel model)
        {
            if (model != null)
            {
                if (model.Id != 0)
                {
                    DeleteById(model._UserId,model.Id);
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
                        Tm_Approval tm_Approval = CONTEXT.Tm_Approval.Find(id);

                        SpNotif.SpSysControllerTransNotif(UserId, "before", CONTEXT, "after", "Approval", "delete", "Id", id.ToString() );

                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_Approval_User""  WHERE ""Id"" = :p0 ", id);
                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_Approval_Role""  WHERE ""Id"" = :p0 ", id);

                        CONTEXT.Tm_Approval.Remove(tm_Approval);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(UserId, "after", CONTEXT, "after", "Approval", "delete", "Id", id.ToString() );

                        CONTEXT_TRANS.Commit();
                    }
                }

            }
        }

        public void Detail_User(HANA_APP CONTEXT, ApprovalModel pModel, int pId)
        {
            //------------------------------
            //Detail
            //------------------------------ 
            var Ticks_ = pModel.UserTicks_ ?? new string[] { };

            List<Tm_User> items = CONTEXT.Database.SqlQuery<Tm_User>(@"SELECT * FROM ""Tm_User"" T0 ").ToList();
            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>(@"SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = @"SELECT TOP 1 T0.""DetId"" FROM  ""Tm_Approval_User"" T0 WHERE T0.""Id"" = :p0 AND T0.""UserId"" = :p1 ";
                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Approval_User();
                    item.Id = pId;
                    item.UserId = items[i].Id ;
                    item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.Tm_Approval_User.Add(item);
                    CONTEXT.SaveChanges();

                    DetId = item.DetId;
                }
                else
                {
                    Tm_Approval_User item = CONTEXT.Tm_Approval_User.Find(DetId);
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

        public void Detail_Role(HANA_APP CONTEXT, ApprovalModel pModel, int pId)
        {
            //------------------------------
            //Detail
            //------------------------------ 
            var Ticks_ = pModel.UserTicks_ ?? new string[] { };
            List<Tm_Role> items = CONTEXT.Database.SqlQuery<Tm_Role>(@"SELECT * FROM  ""Tm_Role"" T0 ").ToList();
            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>(@"SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = @"SELECT TOP 1 T0.""DetId"" FROM  ""Tm_Approval_Role"" T0 WHERE T0.""Id"" = :p0 AND T0.""RoleId"" = :p1 ";
                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Approval_Role();
                    item.Id = pId;
                    item.RoleId = items[i].Id;
                    item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.Tm_Approval_Role.Add(item);
                    CONTEXT.SaveChanges();

                    DetId = item.DetId;

                }
                else
                {
                    Tm_Approval_Role item = CONTEXT.Tm_Approval_Role.Find(DetId);
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


        public ApprovalModel GetNewModel()
        {
            ApprovalModel model = new ApprovalModel();
            model.ListUsers_ = this.GetApproval_Users(0);
            model.ListRoles_ = this.GetApproval_Roles(0);

            return model;
        }

        public ApprovalModel GetById(int id = 0)
        {
            ApprovalModel model = null;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Tm_Approval tm_Approval;
                    tm_Approval = CONTEXT.Tm_Approval.Find(id);

                    if (tm_Approval != null)
                    {
                        model = new ApprovalModel();
                        CopyProperty.CopyProperties(tm_Approval, model, false);

                        model.ListUsers_ = this.GetApproval_Users(id);
                        model.ListRoles_ = this.GetApproval_Roles(id);
                    }
                }

            }

            return model;
        }

        public ApprovalModel NavFirst()
        {
            ApprovalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_Approval"" T0 ORDER BY T0.""Id"" ASC ";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql).FirstOrDefault();
                model = this.GetById(Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public ApprovalModel NavPrevious(int id = 0)
        {
            ApprovalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_Approval"" T0   WHERE T0.""Id""< :p0 ORDER BY T0.""Id"" DESC";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql, id).FirstOrDefault();
                if (Id != null)
                {
                    model = this.GetById(Id.HasValue ? Id.Value : 0);
                }

            }
            return model;
        }

        public ApprovalModel NavNext(int id = 0)
        {
            ApprovalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_Approval"" T0  WHERE T0.""Id"" > :p0 ORDER BY T0.""Id"" ASC";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql, id).FirstOrDefault();
                if (Id != null)
                {
                    model = this.GetById(Id.HasValue ? Id.Value : 0);
                }
                else
                {
                    model = model = this.NavLast();
                }

            }
            return model;

        }

        public ApprovalModel NavLast()
        {
            ApprovalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_Approval"" T0 ORDER BY T0.""Id"" DESC";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql).FirstOrDefault();
                if (Id != null)
                {
                    model = this.GetById(Id.HasValue ? Id.Value : 0);
                }
                else
                {
                    model = model = this.NavLast();
                }

            }
            return model;
        }

    }

    #endregion

}