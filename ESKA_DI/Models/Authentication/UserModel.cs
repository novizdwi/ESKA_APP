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

namespace Models.Authentication.User
{

    #region Models

    public static class UserGetList
    {
        public static void UpdateLastController(int UserId, string ControllerName)
        {
            using (var CONTEXT = new HANA_APP())
            {
                CONTEXT.Database.ExecuteSqlCommand("UPDATE T0 SET T0.\"LastControlle\"=:p1 FROM \"Tm_User\" T0 WHERE T0.\"Id\"=:p0", UserId, ControllerName);
            }
        }
    }

    public class UserModel
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
        public string UserName { get; set; }

        [Required(ErrorMessage = "required")]
        public string FirstName { get; set; }

        public string MidleName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [EmailAddress]
        public string Email { get; set; }

        public bool? isSetPassword { get; set; }

        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password you entered do not match")]
        public string Password_Confirm { get; set; }

        [Required(ErrorMessage = "required")]
        public int? RoleId { get; set; }

        //[Required(ErrorMessage = "required")]
        public int? EmpId { get; set; }

        public int? PositionId { get; set; } 

        public string IsActive { get; set; }

        public List<User_RoutingModel> ListRouting_ = new List<User_RoutingModel>();

        public User_Routing_Contents DetailContents_ { get; set; }
    }

    public class User_RoutingModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;
        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int? _UserId { get; set; }

        public int RowNo { get; set; }

        public int? Id { get; set; }

        public string RoutingCode { get; set; }

        public string RoutingName { get; set; }

        public string IsTick { get; set; }

        public string RoutingCode_ { get; set; }

        public string RoutingName_ { get; set; }

        public int Level_ { get; set; }

    }

    public class User_Routing_Contents
    {
        public List<int> deletedRowKeys { get; set; }

        public List<User_RoutingModel> insertedRowValues { get; set; }

        public List<User_RoutingModel> modifiedRowValues { get; set; }
    }
    #endregion

    #region Services

    public class UserService : IMasterService<UserModel, int>
    {

        public void Add(UserModel model)
        {

            if (model != null)
            {
                if (model.Password != model.Password_Confirm)
                {
                    throw new Exception("[VALIDATION]-The password you entered do not match");
                }

                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_User tm_User = new Tm_User();
                            CopyProperty.CopyProperties(model, tm_User, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tm_User.TransType = "User";
                            if (model.isSetPassword == true)
                            {
                                tm_User.Pwd = Encryption.Encrypt(model.Password == null ? "" : model.Password);
                            }
                            else
                            {
                                tm_User.Pwd = Encryption.Encrypt("password");
                            }
                            tm_User.CreatedDate = dtModified;
                            tm_User.CreatedUser = model._UserId;
                            tm_User.ModifiedDate = dtModified;
                            tm_User.ModifiedUser = model._UserId;

                            CONTEXT.Tm_User.Add(tm_User);
                            CONTEXT.SaveChanges();

                            String keyValue;
                            keyValue = tm_User.Id.ToString();

                            if (model.DetailContents_ != null)
                            {
                                if (model.DetailContents_.insertedRowValues != null)
                                {
                                    foreach (var modifiedRow in model.DetailContents_.modifiedRowValues)
                                    {
                                        Content_Update(CONTEXT, modifiedRow, model.Id, model._UserId, dtModified);
                                    }
                                }
                            }

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_User", "add", "Id", keyValue);

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

        public void Update(UserModel model)
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

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_User", "update", "Id", keyValue);


                            Tm_User tm_User = CONTEXT.Tm_User.Find(model.Id);
                            if (tm_User != null)
                            {
                                var exceptColumns = new string[] { "Id", "Pwd" };
                                CopyProperty.CopyProperties(model, tm_User, false, exceptColumns);
                                if (model.isSetPassword == true)
                                {
                                    tm_User.Pwd = Encryption.Encrypt(model.Password == null ? "" : model.Password);
                                }

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tm_User.ModifiedDate = dtModified;
                                tm_User.ModifiedUser = model._UserId;

                                var changes = CONTEXT.SaveChanges();

                                if (model.DetailContents_ != null)
                                {
                                    if (model.DetailContents_.modifiedRowValues != null)
                                    {
                                        foreach (var modifiedRow in model.DetailContents_.modifiedRowValues)
                                        {
                                            Content_Update(CONTEXT, modifiedRow, model.Id, model._UserId, dtModified);
                                        }
                                    }
                                }

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_User", "update", "Id", keyValue);
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

        public void Content_Update(HANA_APP CONTEXT, User_RoutingModel model, int id, int userId, DateTime dtModified)
        {
            int? detId = 0;

            if (model != null)
            {
                detId = CONTEXT.Database.SqlQuery<int?>(@"SELECT TOP 1 T0.""DetId"" FROM ""Tm_User_Routing"" T0 WHERE T0.""Id"" = :p0 AND T0.""RoutingCode"" = :p1 ", id, model.RoutingCode_).FirstOrDefault() ?? 0;
                if (detId == 0)
                {
                    Tm_User_Routing Tm_User_Routing = new Tm_User_Routing();
                    CopyProperty.CopyProperties(model, Tm_User_Routing, false);

                    Tm_User_Routing.RoutingCode = model.RoutingCode_;
                    Tm_User_Routing.RoutingName = model.RoutingName_;
                    Tm_User_Routing.CreatedDate = dtModified;
                    Tm_User_Routing.CreatedUser = userId;
                    Tm_User_Routing.ModifiedDate = dtModified;
                    Tm_User_Routing.ModifiedUser = userId;

                    CONTEXT.Tm_User_Routing.Add(Tm_User_Routing);
                    var u = CONTEXT.SaveChanges();

                }
                else
                {
                    var Tm_User_Routing = CONTEXT.Tm_User_Routing.Find(detId);
                    if (Tm_User_Routing != null)
                    {
                        var exceptColumns = new string[] { "DetId", "Id" };
                        CopyProperty.CopyProperties(model, Tm_User_Routing, false, exceptColumns);

                        Tm_User_Routing.RoutingName = model.RoutingName_;
                        Tm_User_Routing.ModifiedDate = dtModified;
                        Tm_User_Routing.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                    }
                }
            }

        }

        public void Delete(UserModel model)
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
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tm_User", "delete", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_User\" WHERE \"Id\"=:p0 ", id);
                           
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tm_User", "delete", "Id", keyValue);

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

        public UserModel GetNewModel()
        {
            UserModel model = new UserModel();
            model.isSetPassword = true;
            model.IsActive = "Y";
            model.ListRouting_ = GetRoutingById(0);
            return model;
        }

        public UserModel GetById(int id = 0)
        {
            UserModel model = null;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    model = CONTEXT.Database.SqlQuery<UserModel>("SELECT T0.* FROM \"Tm_User\" T0 WHERE T0.\"Id\"=:p0 ", id).Single();
                    model.isSetPassword = false;

                    model.ListRouting_ = this.GetRoutingById(CONTEXT, model.Id);
                }
            }


            return model;
        }

        public UserModel NavFirst()
        {
            UserModel model = null;
            int? Id;
            using (var CONTEXT = new HANA_APP())
            {
                Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_User\" T0 ORDER BY T0.\"Id\" ASC").FirstOrDefault();


            }
            model = this.GetById(Id.HasValue ? Id.Value : 0);
            return model;

        }

        public UserModel NavPrevious(int id = 0)
        {
            UserModel model = null;
            int? Id;
            using (var CONTEXT = new HANA_APP())
            {
                Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_User\" T0 WHERE T0.\"Id\"<:p0 ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();

                //else
                //{
                //    model = this.NavFirst();
                //}
            }
            if (Id.HasValue)
            {
                model = this.GetById(Id.Value);
            }

            if (model == null)
            {
                model = this.NavFirst();
            }

            return model;

        }

        public UserModel NavNext(int id = 0)
        {
            UserModel model = null;
            int? Id;
            using (var CONTEXT = new HANA_APP())
            {
                Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_User\" T0 WHERE T0.\"Id\">:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                //if (Id.HasValue)
                //{
                //    model = this.GetById(CONTEXT, Id.Value);
                //}
                //else
                //{
                //    model = model = this.NavLast();
                //}
            }

            if (Id.HasValue)
            {
                model = this.GetById(Id.Value);
            }

            if (model == null)
            {
                model = this.NavLast();
            }


            return model;

        }

        public UserModel NavLast()
        {
            UserModel model = null;
            int? Id;
            using (var CONTEXT = new HANA_APP())
            {
                Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_User\" T0 ORDER BY T0.\"Id\" DESC").FirstOrDefault();


            }
            model = this.GetById(Id.HasValue ? Id.Value : 0);
            return model;

        }

        public List<User_RoutingModel> GetRoutingById(int id) {
            using (var CONTEXT = new HANA_APP()) {
                return GetRoutingById(CONTEXT, id);
            }
        }

        private List<User_RoutingModel> GetRoutingById(HANA_APP CONTEXT, int id)
        {
            string ssql = "";
            ssql = @"SELECT
                        ROW_NUMBER() OVER (ORDER BY T0.""Code"") AS ""RowNo"",
                        T1.""Id"" AS ""Id"",
	                    T1.""RoutingCode"" AS ""RoutingCode"",
	                    T1.""IsTick"" AS ""IsTick"",
	                    T0.""Code"" AS ""RoutingCode_"",
	                    T0.""U_RoutingName"" AS ""RoutingName_"",
                        T0.""U_Level"" AS ""Level_""
                    FROM ""{0}"".""@IDU_MASTER_ROUTING"" T0
                    LEFT JOIN  ""Tm_User_Routing"" T1 ON T0.""Code"" = T1.""RoutingCode"" AND T1.""Id"" = :p0 
                    ORDER BY T0.""Code"", T0.""U_Level"" ASC
                    ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<User_RoutingModel>(ssql, id).ToList();


        }

    }

    #endregion

}