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

namespace Models.Setting.ApprovalTemplate
{

    #region Models
    public class ApprovalTemplateModel
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
        public string TemplateName { get; set; }

        //[Required(ErrorMessage = "required")]
        public string DocType { get; set; }

        [Required(ErrorMessage = "required")]
        public string ObjectCode { get; set; }

        public string IgnoreDept { get; set; }

        public string CanOverrideLastApproval { get; set; }

        public string IsActive { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Sql { get; set; }

        public List<ApprovalTemplate_UserModel> ListUsers_ = new List<ApprovalTemplate_UserModel>();

        public List<ApprovalTemplate_RoleModel> ListRoles_ = new List<ApprovalTemplate_RoleModel>();


        public List<ApprovalTemplate_DepartmentModel> ListDepts_ = new List<ApprovalTemplate_DepartmentModel>();

        public List<ApprovalTemplate_StageModel> ListStages_ = new List<ApprovalTemplate_StageModel>();

        public string[] UserTicks_ { get; set; }

        public string[] RoleTicks_ { get; set; }

        public ApprovalTemplate_Stages DetailStages_ { get; set; }

        public ApprovalTemplate_Departments DetailDepartments_ { get; set; }


    }

    public class ApprovalTemplate_UserModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int? _UserId { get; set; }

        public int? Id { get; set; }

        public long? DetId { get; set; }

        public string IsTick { get; set; }

        public int UserId { get; set; }

        public string UserName_ { get; set; }

        public string FirstName_ { get; set; }


    }

    public class ApprovalTemplate_RoleModel
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

    public class ApprovalTemplate_StageModel
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
        public int StageId { get; set; }
        public string StageName { get; set; }
        [Required(ErrorMessage = "required")]
        public int Step { get; set; }
        public string RuleType { get; set; }
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
        public int? TOPId { get; set; }
        public string IsApprSpCustomer { get; set; }
        public string SortCode { get; set; }
    }

    public class ApprovalTemplate_Stages
    {
        public List<int> deletedRowKeys { get; set; }
        public List<ApprovalTemplate_StageModel> insertedRowValues { get; set; }
        public List<ApprovalTemplate_StageModel> modifiedRowValues { get; set; }
    }

    public class ApprovalTemplate_DepartmentModel {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? Id { get; set; }

        public int? DetId { get; set; }
        public int? DepartmentId { get; set; }

        public string IsSelected { get; set; }

        public string DepartmentName_ { get; set; }

    }

    public class ApprovalTemplate_Departments
    {
        public List<int> deletedRowKeys { get; set; }
        public List<ApprovalTemplate_DepartmentModel> insertedRowValues { get; set; }
        public List<ApprovalTemplate_DepartmentModel> modifiedRowValues { get; set; }
    }

    #endregion

    #region Services

    public class ApprovalTemplateService
    {

        public List<ApprovalTemplate_UserModel> GetApprovalTemplate_Users(int id = 0)
        {
            List<ApprovalTemplate_UserModel> models;
            string ssql = @"
                SELECT  T1.""Id"" AS ""Id"", 
                        COALESCE(T1.""DetId"",0) AS ""DetId"", 
                        T0.""Id"" AS ""UserId"", 
                        T0.""UserName"" AS ""UserName_"", 
                        T0.""FirstName"" AS ""FirstName_"",
                        COALESCE(T1.""IsTick"",'') AS ""IsTick""   
                FROM ""Tm_User"" T0 
                LEFT JOIN ""Tm_ApprovalTemplate_User"" T1 ON T0.""Id"" = T1.""UserId""  AND T1.""Id"" = :p0 
                ORDER BY T0.""UserName""
            ";

            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<ApprovalTemplate_UserModel>(ssql, id).ToList();
            }
            return models;

        }

        public List<ApprovalTemplate_RoleModel> GetApprovalTemplate_Roles( int id = 0)
        {
            List<ApprovalTemplate_RoleModel> models;
            string ssql = @"
                SELECT T0.""Id"" AS ""Id"", 
                        COALESCE(T1.""DetId"", 0) AS ""DetId"", 
                        T0.""Id"" AS ""RoleId"", 
                        T0.""RoleName"" AS ""RoleName_"", 
                        COALESCE(T1.""IsTick"",'') AS ""IsTick""
                FROM ""Tm_Role"" T0 
                LEFT JOIN ""Tm_ApprovalTemplate_Role"" T1 ON T0.""Id"" = T1.""RoleId""  AND T1.""Id"" = :p0 
                ORDER BY T0.""RoleName""
            ";

            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<ApprovalTemplate_RoleModel>(ssql, id).ToList();
            }

            return models;

        }

        public List<ApprovalTemplate_StageModel> GetApprovalTemplate_Stages(int id = 0)
        {
            List<ApprovalTemplate_StageModel> models;
            string ssql = @"SELECT T0.* FROM ""Tm_ApprovalTemplate_Stage"" T0 WHERE T0.""Id"" = :p0 ORDER BY T0.""Step"", T0.""DetId"" ";

            ssql = string.Format(ssql, id);
            using (var CONTEXT = new HANA_APP())
            {
                models = CONTEXT.Database.SqlQuery<ApprovalTemplate_StageModel>(ssql, id).ToList();
            }

            return models;
        }

        public int Add(ApprovalTemplateModel model)
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
                            Tm_ApprovalTemplate tm_ApprovalTemplate = new Tm_ApprovalTemplate();
                            CopyProperty.CopyProperties(model, tm_ApprovalTemplate, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tm_ApprovalTemplate.TransType = "ApprovalTemplate";

                            tm_ApprovalTemplate.CreatedDate = dtModified;
                            tm_ApprovalTemplate.CreatedUser = model._UserId;
                            tm_ApprovalTemplate.ModifiedDate = dtModified;
                            tm_ApprovalTemplate.ModifiedUser = model._UserId;

                            CONTEXT.Tm_ApprovalTemplate.Add(tm_ApprovalTemplate);
                            CONTEXT.SaveChanges();
                            Id = tm_ApprovalTemplate.Id;

                            String keyValue;
                            keyValue = tm_ApprovalTemplate.Id.ToString();

                            string ssql = $@"UPDATE T0 SET 
                                ""Step"" = (
                                    SELECT ""Step""
                                    FROM(
                                        SELECT ROW_NUMBER() OVER(ORDER BY Ty.""SortCode"", Ty.""DetId"") AS ""Step"", Ty.""DetId""
                                        FROM ""Tm_ApprovalTemplate"" AS Tx
                                        INNER JOIN ""Tm_ApprovalTemplate_Stage"" AS Ty ON Tx.""Id"" = Tx.""Id""
                                        WHERE Tx.""Id"" = {Id}
                                    ) AS ""Sub""
                                    WHERE ""Sub"".""DetId"" = T0.""DetId""
                                )
                                FROM ""Tm_ApprovalTemplate_Stage"" T0
                                WHERE T0.""Id"" = {Id} ";
                            CONTEXT.Database.ExecuteSqlCommand(ssql);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "ApprovalTemplate", CONTEXT, "after", "Tm_ApprovalTemplate", "add", "Id", keyValue);
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

        public void Update(ApprovalTemplateModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();
                        
                        SpNotif.SpSysControllerTransNotif(model._UserId, "ApprovalTemplate", CONTEXT, "before", "Tm_ApprovalTemplate", "update", "Id", keyValue);

                        Tm_ApprovalTemplate tm_ApprovalTemplate = CONTEXT.Tm_ApprovalTemplate.Find(model.Id);
                        if (tm_ApprovalTemplate != null)
                        {

                            var exceptColumns = new string[] { "Id", "TransType" };
                            CopyProperty.CopyProperties(model, tm_ApprovalTemplate, false, exceptColumns);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_ApprovalTemplate.ModifiedDate = dtModified;
                            tm_ApprovalTemplate.ModifiedUser = model._UserId;
                            CONTEXT.SaveChanges();
                            
                            if (model.DetailStages_ != null)
                            {
                                if (model.DetailStages_.insertedRowValues != null)
                                {
                                    foreach (var insertedRow in model.DetailStages_.insertedRowValues)
                                    {
                                        Stage_Add(CONTEXT, insertedRow, model.Id, model._UserId);
                                    }
                                }
                                if (model.DetailStages_.modifiedRowValues != null)
                                {
                                    foreach (var modifiedRow in model.DetailStages_.modifiedRowValues)
                                    {
                                        Stage_Update(CONTEXT, modifiedRow, model._UserId);
                                    }
                                }

                                if(model.DetailStages_.deletedRowKeys != null)
                                {
                                    foreach (var deletedRowKeys in model.DetailStages_.deletedRowKeys)
                                    {
                                        Stage_Delete(CONTEXT, deletedRowKeys);
                                    }
                                }
                            }

                            Detail_User(CONTEXT, model, model.Id);

                            SpNotif.SpSysControllerTransNotif(model._UserId, "ApprovalTemplate", CONTEXT, "after", "Tm_ApprovalTemplate", "update", "Id", keyValue);
                        }
                        CONTEXT_TRANS.Commit();
                    }

                }
            }
        }

        public void Delete(ApprovalTemplateModel model)
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

                        string keyValue = id.ToString();
                        SpNotif.SpSysControllerTransNotif(UserId, "ApprovalTemplate", CONTEXT, "before", "Tm_ApprovalTemplate", "delete", "Id", keyValue);
                        Tm_ApprovalTemplate tm_ApprovalTemplate = CONTEXT.Tm_ApprovalTemplate.Find(id);

                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_ApprovalTemplate_Role""  WHERE ""Id"" = :p0 ", id);
                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_ApprovalTemplate_User""  WHERE ""Id"" = :p0 ", id);
                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_ApprovalTemplate_Role""  WHERE ""Id"" = :p0 ", id);
                        CONTEXT.Database.ExecuteSqlCommand(@"DELETE FROM ""Tm_ApprovalTemplate_Stage""  WHERE ""Id""= :p0 ", id);

                        CONTEXT.Tm_ApprovalTemplate.Remove(tm_ApprovalTemplate);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(UserId, "ApprovalTemplate", CONTEXT, "after", "Tm_ApprovalTemplate", "delete", "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                    }

                }

            }
        }


        public ApprovalTemplateModel GetNewModel()
        {
            ApprovalTemplateModel model = new ApprovalTemplateModel();
                model.IsActive = "Y";
                model.Sql = "/* \n"+
                            "{Id} \n" +
                            "{UserId} \n" +
                            "{BaseObject} \n" +
                            "{DbSap} \n" +
                            "*/ \n\n "+
                            "SELECT 'Y' FROM DUMMY ";
                model.ListUsers_ = this.GetApprovalTemplate_Users(0);
                model.ListRoles_ = this.GetApprovalTemplate_Roles(0);
                model.ListStages_ = this.GetApprovalTemplate_Stages(0);

            return model;
        }

        public void Detail_User(HANA_APP CONTEXT, ApprovalTemplateModel pModel, int pId)
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
                var ssql2 = @"SELECT TOP 1 T0.""DetId"" FROM  ""Tm_ApprovalTemplate_User"" T0 WHERE T0.""Id"" = :p0 AND T0.""UserId"" = :p1 ";
                long? DetId = CONTEXT.Database.SqlQuery<long?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    if (Ticks_.Contains(items[i].Id.ToString()))
                    {
                        var item = new Tm_ApprovalTemplate_User();
                        item.Id = pId;
                        item.UserId = items[i].Id;
                        item.IsTick = "Y";

                        item.CreatedUser = pModel._UserId;
                        item.CreatedDate = dtModified;
                        item.ModifiedUser = pModel._UserId;
                        item.ModifiedDate = dtModified;

                        CONTEXT.Tm_ApprovalTemplate_User.Add(item);
                        CONTEXT.SaveChanges();

                        DetId = item.DetId;
                    }
                }
                else
                {
                    Tm_ApprovalTemplate_User item = CONTEXT.Tm_ApprovalTemplate_User.Find(DetId);
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

        public void Detail_Role(HANA_APP CONTEXT, ApprovalTemplateModel pModel, int pId)
        {
            //------------------------------
            //Detail
            //------------------------------ 
            var ssql = @"SELECT * FROM  ""Tm_Role"" T0 ";
            var Ticks_ = pModel.UserTicks_ ?? new string[] { };
            List<Tm_Role> items = CONTEXT.Database.SqlQuery<Tm_Role>(ssql).ToList();

            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

            for (int i = 0; i < items.Count; i++)
            {
                var ssql2 = @"SELECT TOP 1 T0.""DetId"" FROM  ""Tm_ApprovalTemplate_Role"" T0 WHERE T0.""Id"" = :p0 AND T0.""RoleId"" = :p1 ";
                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_ApprovalTemplate_Role();
                    item.Id = pId;
                    item.RoleId = items[i].Id;
                    item.IsTick = Ticks_.Contains(items[i].Id.ToString()) ? "Y" : "N";

                    item.CreatedUser = pModel._UserId;
                    item.CreatedDate = dtModified;
                    item.ModifiedUser = pModel._UserId;
                    item.ModifiedDate = dtModified;

                    CONTEXT.Tm_ApprovalTemplate_Role.Add(item);
                    CONTEXT.SaveChanges();

                    DetId = item.DetId;

                }
                else
                {
                    Tm_ApprovalTemplate_Role item = CONTEXT.Tm_ApprovalTemplate_Role.Find(DetId);
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

        public int Detail_Add(ApprovalTemplate_StageModel model, int Id, int UserId)
        {
            int DetId = 0;
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        Tm_ApprovalTemplate_Stage tm_ApprovalTemplate_Stage = new Tm_ApprovalTemplate_Stage();

                        CopyProperty.CopyProperties(model, tm_ApprovalTemplate_Stage, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        tm_ApprovalTemplate_Stage.Id = Id;
                        tm_ApprovalTemplate_Stage.CreatedDate = dtModified;
                        tm_ApprovalTemplate_Stage.CreatedUser = UserId;
                        tm_ApprovalTemplate_Stage.ModifiedDate = dtModified;
                        tm_ApprovalTemplate_Stage.ModifiedUser = UserId;

                        CONTEXT.Tm_ApprovalTemplate_Stage.Add(tm_ApprovalTemplate_Stage);
                        CONTEXT.SaveChanges();
                        CONTEXT_TRANS.Commit();

                        DetId = tm_ApprovalTemplate_Stage.DetId;
                    }

                }

            }

            return DetId;

        }

        public void Detail_Update(ApprovalTemplate_StageModel model, int UserId)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        Tm_ApprovalTemplate_Stage item = CONTEXT.Tm_ApprovalTemplate_Stage.Find(model.DetId);
                        if (item != null)
                        {
                            var exceptColumns = new string[] { "DetId", "Id" };
                            CopyProperty.CopyProperties(model, item, false, exceptColumns);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            item.ModifiedDate = dtModified;
                            item.ModifiedUser = UserId;

                            CONTEXT.SaveChanges();
                            CONTEXT_TRANS.Commit();
                        }

                    }

                }

            }

        }

        public void Detail_Delete(int detId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    Tm_ApprovalTemplate_Stage tm_ApprovalTemplate_Stage = CONTEXT.Tm_ApprovalTemplate_Stage.Find(detId);
                    if(tm_ApprovalTemplate_Stage != null)
                    {
                        CONTEXT.Tm_ApprovalTemplate_Stage.Remove(tm_ApprovalTemplate_Stage);
                        CONTEXT_TRANS.Commit();

                    }

                }

            }

        }

        public ApprovalTemplateModel GetById(int id = 0)
        {
            ApprovalTemplateModel model = null;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Tm_ApprovalTemplate tm_ApprovalTemplate;
                    tm_ApprovalTemplate = CONTEXT.Tm_ApprovalTemplate.Find(id);

                    if (tm_ApprovalTemplate != null)
                    {
                        model = new ApprovalTemplateModel();
                        CopyProperty.CopyProperties(tm_ApprovalTemplate, model, false);

                    }

                    model.ListUsers_ = this.GetApprovalTemplate_Users(id);
                    model.ListStages_ = this.GetApprovalTemplate_Stages(id);
                }

            }

            return model;
        }

        public ApprovalTemplateModel NavFirst()
        {
            ApprovalTemplateModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_ApprovalTemplate"" T0   ORDER BY T0.""Id"" ASC ";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql).FirstOrDefault();
                model = this.GetById(Id.HasValue ? Id.Value : 0);

            }
            return model;

        }

        public ApprovalTemplateModel NavPrevious(int id = 0)
        {
            ApprovalTemplateModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_ApprovalTemplate"" T0   WHERE T0.""Id"" < :p0 ORDER BY T0.""Id"" DESC";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql, id).FirstOrDefault();

                if (Id != null)
                {
                    model = this.GetById(Id.HasValue ? Id.Value : 0);

                }
                else
                {
                    model = this.NavFirst();

                }

            }
            return model;

        }

        public ApprovalTemplateModel NavNext(int id = 0)
        {
            ApprovalTemplateModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_ApprovalTemplate"" T0   WHERE T0.""Id"" > :p0 ORDER BY T0.""Id"" ASC";
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

        public ApprovalTemplateModel NavLast()
        {
            ApprovalTemplateModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT TOP 1 T0.""Id"" FROM ""Tm_ApprovalTemplate"" T0   ORDER BY T0.""Id"" DESC";
                int? Id = CONTEXT.Database.SqlQuery<int?>(ssql).FirstOrDefault();
                model = this.GetById(Id.HasValue ? Id.Value : 0);

            }
                return model;

        }
        
        #endregion

        #region stage
        //-------------------------------------
        //Detail stages view model
        //-------------------------------------

        public int Stage_Add(HANA_APP CONTEXT, ApprovalTemplate_StageModel model, int Id, int UserId)
        {
            int DetId = 0;

            if (model != null)
            {
                {
                    {
                        Tm_ApprovalTemplate_Stage tm_ApprovalTemplate_Stage = new Tm_ApprovalTemplate_Stage();

                        CopyProperty.CopyProperties(model, tm_ApprovalTemplate_Stage, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        tm_ApprovalTemplate_Stage.Id = Id;
                        tm_ApprovalTemplate_Stage.CreatedDate = dtModified;
                        tm_ApprovalTemplate_Stage.CreatedUser = UserId;
                        tm_ApprovalTemplate_Stage.ModifiedDate = dtModified;
                        tm_ApprovalTemplate_Stage.ModifiedUser = UserId;

                        CONTEXT.Tm_ApprovalTemplate_Stage.Add(tm_ApprovalTemplate_Stage);
                        CONTEXT.SaveChanges();

                        DetId = tm_ApprovalTemplate_Stage.DetId;

                    }

                }

            }

            return DetId;

        }

        public void Stage_Update(HANA_APP CONTEXT, ApprovalTemplate_StageModel model, int UserId)
        {

            if (model != null)
            {
                {
                    {
                        Tm_ApprovalTemplate_Stage tm_ApprovalTemplate_Stage = CONTEXT.Tm_ApprovalTemplate_Stage.Find(model.DetId);

                        if (tm_ApprovalTemplate_Stage != null)
                        {

                            var exceptColumns = new string[] { "DetId", "Id" };
                            CopyProperty.CopyProperties(model, tm_ApprovalTemplate_Stage, false, exceptColumns);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_ApprovalTemplate_Stage.ModifiedDate = dtModified;
                            tm_ApprovalTemplate_Stage.ModifiedUser = UserId;

                            CONTEXT.SaveChanges();
                        }

                    }

                }
            }
        }

        public void Stage_Delete(HANA_APP CONTEXT, int detId = 0)
        {
            if (detId != 0)
            {
                Tm_ApprovalTemplate_Stage tm_ApprovalTemplate_Stage = CONTEXT.Tm_ApprovalTemplate_Stage.Find(detId);
                if (tm_ApprovalTemplate_Stage != null)
                {
                    CONTEXT.Tm_ApprovalTemplate_Stage.Remove(tm_ApprovalTemplate_Stage);
                    CONTEXT.SaveChanges();

                }

            }

        }

        #endregion

    }
    

}