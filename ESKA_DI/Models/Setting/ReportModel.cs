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

namespace Models.Setting.Report
{

    #region Models


    public static class ReportGetList
    {



    }



    public class ReportModel
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
        public int? GroupId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ReportName { get; set; }

        public string Description { get; set; }

        public Guid? Uid { get; set; }

        public byte[] Data { get; set; }

        [Required(ErrorMessage = "required")]
        public string IsActive { get; set; }

        [Required(ErrorMessage = "required")]
        public string DatabaseType { get; set; }

        [Required(ErrorMessage = "required")]
        public string OutputType { get; set; }

        public List<Report_ParamModel> ListParams_ = new List<Report_ParamModel>();

        public Report_DetailParamsModel DetailParams_ { get; set; }

        public List<Report_UserModel> ListUsers_ = new List<Report_UserModel>();

        public List<Report_RoleModel> ListRoles_ = new List<Report_RoleModel>();

        public string[] UserTicks_ { get; set; }

        public string[] RoleTicks_ { get; set; }





    }

    public class Report_ParamModel
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

        public string SortCode { get; set; }

        public string ParamName { get; set; }

        public string IsMandatory { get; set; }

        public string IsHide { get; set; }

        public string TypeData { get; set; }

        public string Caption { get; set; }

        public string DefaultValue { get; set; }

        public string Sql { get; set; }

        public string TypeControl { get; set; }

        public string TypeChoose { get; set; }
    }



    public class Report_DetailParamsModel
    {
        public List<int> deletedRowKeys { get; set; }
        public List<Report_ParamModel> insertedRowValues { get; set; }
        public List<Report_ParamModel> modifiedRowValues { get; set; }
    }

    public class Report_UserModel
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

    public class Report_RoleModel
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

    public class ReportService
    {
        public List<Report_ParamModel> GetReport_Params(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT * FROM \"Tm_Report_Param\" T0 WHERE T0.\"Id\"=:p0  ORDER BY T0.\"SortCode\", T0.\"DetId\" ";

            return CONTEXT.Database.SqlQuery<Report_ParamModel>(ssql, id).ToList();
        }

        public List<Report_UserModel> GetReport_Users(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", IFNULL(T1.\"DetId\",0) AS \"DetId\", T0.\"Id\" AS \"UserId\",T0.\"UserName\" AS \"UserName_\", IFNULL(T1.\"IsTick\",'') AS \"IsTick\"   "
                            + " FROM \"Tm_User\" T0 "
                            + " LEFT JOIN \"Tm_Report_User\" T1 ON T0.\"Id\"=T1.\"UserId\"  AND T1.\"Id\"=:p1 "
                            + " ORDER BY T0.\"UserName\"";

            return CONTEXT.Database.SqlQuery<Report_UserModel>(ssql, id, id).ToList();
        }

        public List<Report_RoleModel> GetReport_Roles(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT :p0 AS \"Id\", IFNULL(T1.\"DetId\",0) AS \"DetId\", T0.\"Id\" AS \"RoleId\",T0.\"RoleName\" AS \"RoleName_\", IFNULL(T1.\"IsTick\",'') AS \"IsTick\"   "
                            + " FROM \"Tm_Role\" T0 "
                            + " LEFT JOIN \"Tm_Report_Role\" T1 ON T0.\"Id\"=T1.\"RoleId\"  AND T1.\"Id\"=:p1 "
                            + " ORDER BY T0.\"RoleName\"";

            return CONTEXT.Database.SqlQuery<Report_RoleModel>(ssql, id, id).ToList();
        }


        public void Add(ReportModel model)
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

                            Tm_Report tm_Report = new Tm_Report();
                            CopyProperty.CopyProperties(model, tm_Report, false);

                            tm_Report.TransType = "Report";

                            if (model._HiddenUid != "")
                            {
                                tm_Report.Uid = model._HiddenUid;
                            }

                            if ((model._HiddenUid != "") && (model.Data != null))
                            {
                                tm_Report.Data = model.Data;
                            }

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tm_Report.CreatedDate = dtModified;
                            tm_Report.CreatedUser = model._UserId;
                            tm_Report.ModifiedDate = dtModified;
                            tm_Report.ModifiedUser = model._UserId;

                            CONTEXT.Tm_Report.Add(tm_Report);
                            CONTEXT.SaveChanges();


                            Id = tm_Report.Id;

                            if (model.DetailParams_ != null)
                            {
                                foreach (var pax in model.DetailParams_.insertedRowValues)
                                {
                                    Detail_Add(CONTEXT, pax, Id, model._UserId);
                                }
                            }
                            Detail_User(CONTEXT, model, Id);
                            Detail_Role(CONTEXT, model, Id);


                            String keyValue;
                            keyValue = tm_Report.Id.ToString();
                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Report", "add", "Id", keyValue);


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

        public void Update(ReportModel model)
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

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_Report", "update", "Id", keyValue);


                            Tm_Report tm_Report = CONTEXT.Tm_Report.Find(model.Id);
                            if (tm_Report != null)
                            {
                                var exceptColumns = new string[] { "Id", "Uid", "Data", "TransType" };
                                CopyProperty.CopyProperties(model, tm_Report, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                if (model._HiddenUid != "")
                                {
                                    tm_Report.Uid = model._HiddenUid;
                                }

                                if ((model._HiddenUid != "") && (model.Data != null))
                                {
                                    tm_Report.Data = model.Data;
                                }

                                tm_Report.ModifiedDate = dtModified;
                                tm_Report.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();


                                if (model.DetailParams_ != null)
                                {
                                    if (model.DetailParams_.insertedRowValues != null)
                                    {
                                        foreach (var param in model.DetailParams_.insertedRowValues)
                                        {
                                            Detail_Add(CONTEXT, param, model.Id, model._UserId);
                                        }
                                    }

                                    if (model.DetailParams_.modifiedRowValues != null)
                                    {
                                        foreach (var param in model.DetailParams_.modifiedRowValues)
                                        {
                                            Detail_Update(CONTEXT, param, model._UserId);
                                        }
                                    }

                                    if (model.DetailParams_.deletedRowKeys != null)
                                    {
                                        foreach (var param in model.DetailParams_.deletedRowKeys)
                                        {
                                            Report_ParamModel model2 = new Report_ParamModel();
                                            model2.DetId = param;
                                            Detail_Delete(CONTEXT, model2);
                                        }
                                    }
                                }

                                Detail_User(CONTEXT, model, model.Id);
                                Detail_Role(CONTEXT, model, model.Id);


                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Report", "update", "Id", keyValue);

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

        public void Delete(ReportModel model)
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
                            SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tm_Report", "delete", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Report\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Report_Role\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Report_User\"  WHERE \"Id\"=:p0", id);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Report_Param\"  WHERE \"Id\"=:p0", id);

                            SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tm_Report", "delete", "Id", keyValue);
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

        public void Detail_User(HANA_APP CONTEXT, ReportModel pModel, int pId)
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
                var ssql2 = "SELECT TOP 1 T0.\"DetId\" FROM  \"Tm_Report_User\" T0 WHERE T0.\"Id\" = :p0 AND T0.\"UserId\"=:p1 ";

                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Report_User();
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

                    CONTEXT.Tm_Report_User.Add(item);
                    CONTEXT.SaveChanges();

                }
                else
                {
                    var item = CONTEXT.Tm_Report_User.Find(DetId);
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

        public void Detail_Role(HANA_APP CONTEXT, ReportModel pModel, int pId)
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
                var ssql2 = "SELECT TOP 1 T0.\"DetId\" FROM  \"Tm_Report_Role\" T0 WHERE T0.\"Id\" =:p0 AND T0.\"RoleId\"=:p1 ";

                int? DetId = CONTEXT.Database.SqlQuery<int?>(ssql2, pId, items[i].Id).FirstOrDefault();

                if (DetId == null)
                {
                    var item = new Tm_Report_Role();
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

                    CONTEXT.Tm_Report_Role.Add(item);
                    CONTEXT.SaveChanges();


                }
                else
                {
                    var item = CONTEXT.Tm_Report_Role.Find(DetId);
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


        public ReportModel GetNewModel()
        {
            ReportModel model = new ReportModel();
            //model.ListParams_ = this.GetReport_Params(0);
            using (var CONTEXT = new HANA_APP())
            {
                model.ListUsers_ = this.GetReport_Users(CONTEXT, 0);
                model.ListRoles_ = this.GetReport_Roles(CONTEXT, 0);
            }

            return model;
        }

        public ReportModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public ReportModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            ReportModel model = null;
            if (id != 0)
            {

                model = CONTEXT.Database.SqlQuery<ReportModel>("SELECT T0.* FROM \"Tm_Report\" T0 WHERE T0.\"Id\"=:p0 ", id).Single();

                if (model != null)
                {
                    model.ListParams_ = this.GetReport_Params(CONTEXT, id);
                    model.ListUsers_ = this.GetReport_Users(CONTEXT, id);
                    model.ListRoles_ = this.GetReport_Roles(CONTEXT, id);
                }
            }

            return model;
        }

        public ReportModel NavFirst()
        {
            ReportModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Report\" T0 ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public ReportModel NavPrevious(int id = 0)
        {
            ReportModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Report\" T0 WHERE T0.\"Id\"<:p0 ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public ReportModel NavNext(int id = 0)
        {
            ReportModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Report\" T0 WHERE T0.\"Id\">:p0 ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public ReportModel NavLast()
        {
            ReportModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                int? Id = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Report\" T0 ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }



        //---------------------------------------
        //Report_ParamModel
        //--------------------------------------- 
        public int Detail_Add(HANA_APP CONTEXT, Report_ParamModel model, int Id, int UserId)
        {
            int DetId = 0;

            if (model != null)
            {

                Tm_Report_Param tm_Report_Param = new Tm_Report_Param();

                CopyProperty.CopyProperties(model, tm_Report_Param, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_Report_Param.Id = Id;
                tm_Report_Param.CreatedDate = dtModified;
                tm_Report_Param.CreatedUser = UserId;
                tm_Report_Param.ModifiedDate = dtModified;
                tm_Report_Param.ModifiedUser = UserId;

                CONTEXT.Tm_Report_Param.Add(tm_Report_Param);
                CONTEXT.SaveChanges();

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, Report_ParamModel model, int UserId)
        {

            if (model != null)
            {

                Tm_Report_Param tm_Report_Param = CONTEXT.Tm_Report_Param.Find(model.DetId);

                if (tm_Report_Param != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, tm_Report_Param, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_Report_Param.ModifiedDate = dtModified;
                    tm_Report_Param.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }
        }

        public void Detail_Delete(HANA_APP CONTEXT, Report_ParamModel model)
        {

            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Report_Param\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }
        }


    }

    #endregion

}