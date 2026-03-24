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
using System.IO;
using DevExpress.Web;

namespace Models.Transaction.StockOpname
{
    #region Models

    public class RequestStockOpnameModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }              

        public string TransType { get; set; }    

        public string TransNo { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string Description { get; set; }  

        public string Status { get; set; }   
        
        public string CancelReason { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }
    }


    #endregion

    #region Services

    public class RequestStockOpnameService
    {

        public RequestStockOpnameModel GetNewModel(int userId)
        {
            RequestStockOpnameModel model = new RequestStockOpnameModel();
            model.Status = "Draft";
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now.AddMonths(1);

            using (var CONTEXT = new HANA_APP())
            {
                string ssql = @"SELECT T0.""DefaultWhsCode""
                                FROM ""Tm_User"" T0
                                WHERE T0.""Id"" = :p0 
                ";
                model.WhsCode = CONTEXT.Database.SqlQuery<string>(ssql, userId).FirstOrDefault();

                ssql = @"SELECT T0.""WhsName""
                                FROM """+ DbProvider.dbSap_Name + @""".""OWHS"" T0
                                WHERE T0.""WhsCode"" = :p0 
                ";
                model.WhsName = CONTEXT.Database.SqlQuery<string>(ssql, model.WhsCode).FirstOrDefault();

            }

            return model;


        }
        public RequestStockOpnameModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public RequestStockOpnameModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            RequestStockOpnameModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"",
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_Request_StockOpname"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<RequestStockOpnameModel>(ssql, id).Single();

            }

            return model;
        }
        
        public RequestStockOpnameModel NavFirst(int userId)
        {
            RequestStockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "RequestStockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Request_StockOpname\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public RequestStockOpnameModel NavPrevious(int userId, long id = 0)
        {
            RequestStockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "RequestStockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Request_StockOpname\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public RequestStockOpnameModel NavNext(int userId, long id = 0)
        {
            RequestStockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "RequestStockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Request_StockOpname\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public RequestStockOpnameModel NavLast(int userId)
        {
            RequestStockOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "RequestStockOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Request_StockOpname\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(RequestStockOpnameModel model)
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

                            Tx_Request_StockOpname Tx_Request_StockOpname = new Tx_Request_StockOpname();
                            CopyProperty.CopyProperties(model, Tx_Request_StockOpname, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_Request_StockOpname.TransType = "RequestStockOpname";
                            Tx_Request_StockOpname.Status = "Posted";

                            Tx_Request_StockOpname.CreatedDate = dtModified;
                            Tx_Request_StockOpname.CreatedUser = model._UserId;
                            Tx_Request_StockOpname.ModifiedDate = dtModified;
                            Tx_Request_StockOpname.ModifiedUser = model._UserId;
                            
                            string dateX = model.StartDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'RequestStockOpname','" + dateX + "','') ").SingleOrDefault();
                            Tx_Request_StockOpname.TransNo = transNo;

                            CONTEXT.Tx_Request_StockOpname.Add(Tx_Request_StockOpname);
                            CONTEXT.SaveChanges();
                            Id = Tx_Request_StockOpname.Id;

                            String keyValue;
                            keyValue = Tx_Request_StockOpname.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "RequestStockOpname", CONTEXT, "after", "RequestStockOpname", "add", "Id", keyValue);


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

        public void Update(RequestStockOpnameModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "RequestStockOpname", CONTEXT, "before", "RequestStockOpname", "update", "Id", keyValue);

                                Tx_Request_StockOpname Tx_Request_StockOpname = CONTEXT.Tx_Request_StockOpname.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_Request_StockOpname.ModifiedDate = dtModified;
                                Tx_Request_StockOpname.ModifiedUser = model._UserId;

                                if (Tx_Request_StockOpname != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedDate","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_Request_StockOpname, false, exceptColumns);
                                    Tx_Request_StockOpname.ModifiedDate = dtModified;
                                    Tx_Request_StockOpname.ModifiedUser = model._UserId;
                                    CONTEXT.SaveChanges();
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "RequestStockOpname", CONTEXT, "after", "RequestStockOpname", "update", "Id", keyValue);
                                    
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

        public void Post(int userId, long id)
        {

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        Tx_Request_StockOpname Tx_Request_StockOpname = CONTEXT.Tx_Request_StockOpname.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "RequestStockOpname", CONTEXT, "before", "Tx_Request_StockOpname", "post", "Id", keyValue);

                        if (Tx_Request_StockOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_Request_StockOpname.Status = "Posted";

                            Tx_Request_StockOpname.ModifiedDate = dtModified;
                            Tx_Request_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "RequestStockOpname", CONTEXT, "after", "Tx_Request_StockOpname", "post", "Id", keyValue);


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

        public void Cancel(int userId, long Id, string cancelReason)
        {
            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = Id.ToString();

                        Tx_Request_StockOpname Tx_Request_StockOpname = CONTEXT.Tx_Request_StockOpname.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "RequestStockOpname", CONTEXT, "before", "Tx_Request_StockOpname", "cancel", "Id", keyValue);
                        if (Tx_Request_StockOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_Request_StockOpname.Status = "Cancel";
                            Tx_Request_StockOpname.CancelReason = cancelReason;
                            Tx_Request_StockOpname.ModifiedDate = dtModified;
                            Tx_Request_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "RequestStockOpname", CONTEXT, "after", "Tx_Request_StockOpname", "cancel", "Id", keyValue);


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


    #endregion

}