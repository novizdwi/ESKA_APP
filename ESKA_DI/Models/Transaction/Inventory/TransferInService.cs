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

namespace Models.Transaction.Inventory
{
    #region Models

    public class TransferInModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string UserName { get; set; }

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public long? BaseEntry { get; set; }

        public string BaseDocNum { get; set; }

        public string DocNum { get; set; }

        public string RefNo { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public long? SummaryId_ { get; set; }

        public string SummaryTransNo_ { get; set; }

        public List<TransferIn_DetailModel> ListDetails_ = new List<TransferIn_DetailModel>();

        public TransferIn_Detail Details_ { get; set; }
    }

    public class TransferIn_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferIn_DetailModel> insertedRowValues { get; set; }
        public List<TransferIn_DetailModel> modifiedRowValues { get; set; }
    }
    public class TransferIn_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOpen { get; set; }

        public decimal? QuantityScan { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public long? BaseEntry { get; set; }

        public long? BaseLine { get; set; }

        public string BaseType { get; set; }

        public string Comments { get; set; }
    }

    public class TransferInItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<TransferInItemTagModel> TransferInItemTagModel___ { get; set; }
    }

    public class TransferInItemTagModel
    {
        public int RowNo { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public long DetDetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string TagId { get; set; }

        public decimal? Quantity { get; set; }

        public string EventType { get; set; }

        public string Status { get; set; }
    }

    #endregion

    #region Services

    public class TransferInService
    {

        public TransferInModel GetNewModel(int userId)
        {
            TransferInModel model = new TransferInModel();
            model.Status = "Draft";
            return model;
        }
        public TransferInModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public TransferInModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            TransferInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_TransferIn"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<TransferInModel>(ssql, id).Single();

                model.ListDetails_ = this.TransferIn_Details(CONTEXT, id);
                if(model.Status != "Draft")
                {
                    string sSummaryId = @"
                        SELECT TOP 1 T1.""Id""
                        FROM ""Tx_TransferSummaryIn_Ref"" T0
                        INNER JOIN ""Tx_TransferSummaryIn"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                       ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryId_ = CONTEXT.Database.SqlQuery<long?>(sSummaryId, id).SingleOrDefault();

                    string sSummaryNo = @"
                        SELECT TOP 1 T1.""TransNo""
                        FROM ""Tx_TransferSummaryIn_Ref"" T0
                        INNER JOIN ""Tx_TransferSummaryIn"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                       ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryTransNo_ = CONTEXT.Database.SqlQuery<string>(sSummaryNo, id).SingleOrDefault();

                }
            }

            return model;
        }
        public List<TransferIn_DetailModel> TransferIn_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferIn_Details(CONTEXT, id);
            }

        }

        public List<TransferIn_DetailModel> TransferIn_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT * 
                FROM ""Tx_TransferIn_Item"" 
                WHERE ""Id"" =:p0
                ORDER BY ""DetId"" ASC
            ";
            var purchaseOrder = CONTEXT.Database.SqlQuery<TransferIn_DetailModel>(ssql, id).ToList();
            return purchaseOrder;
        }

        public TransferInModel NavFirst(int userId)
        {
            TransferInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public TransferInModel NavPrevious(int userId, long id = 0)
        {
            TransferInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferIn\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TransferInModel NavNext(int userId, long id = 0)
        {
            TransferInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferIn\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TransferInModel NavLast(int userId)
        {
            TransferInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferIn\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(TransferInModel model)
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

                            Tx_TransferIn Tx_TransferIn = new Tx_TransferIn();
                            CopyProperty.CopyProperties(model, Tx_TransferIn, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_TransferIn.TransType = "TransferIn";
                            Tx_TransferIn.CreatedDate = dtModified;
                            Tx_TransferIn.CreatedUser = model._UserId;
                            Tx_TransferIn.ModifiedDate = dtModified;
                            Tx_TransferIn.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TransferIn','" + dateX + "','') ").SingleOrDefault();
                            Tx_TransferIn.TransNo = transNo;

                            CONTEXT.Tx_TransferIn.Add(Tx_TransferIn);
                            CONTEXT.SaveChanges();
                            Id = Tx_TransferIn.Id;

                            String keyValue;
                            keyValue = Tx_TransferIn.Id.ToString();

                        //    CONTEXT.Database.ExecuteSqlCommand(
                        //    "CALL \"SpTransferIn_AddItemDetail\" ({0}, {1}, {2})",
                        //    model._UserId,
                        //    keyValue
                        //);

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "TransferIn", "add", "Id", keyValue);


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

        public void Update(TransferInModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "TransferIn", CONTEXT, "before", "TransferIn", "update", "Id", keyValue);


                                Tx_TransferIn Tx_TransferIn = CONTEXT.Tx_TransferIn.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_TransferIn.ModifiedDate = dtModified;
                                Tx_TransferIn.ModifiedUser = model._UserId;

                                if (Tx_TransferIn != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser"};
                                    CopyProperty.CopyProperties(model, Tx_TransferIn, false, exceptColumns);
                                    
                                    Tx_TransferIn.ModifiedDate = dtModified;
                                    Tx_TransferIn.ModifiedUser = model._UserId;
                                    
                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TransferIn", CONTEXT, "after", "TransferIn", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, TransferIn_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_TransferIn_Item Tx_TransferIn_Item = new Tx_TransferIn_Item();

        //        CopyProperty.CopyProperties(model, Tx_TransferIn_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_TransferIn_Item.Id = Id;
        //        Tx_TransferIn_Item.CreatedDate = dtModified;
        //        Tx_TransferIn_Item.CreatedUser = UserId;
        //        Tx_TransferIn_Item.ModifiedDate = dtModified;
        //        Tx_TransferIn_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_TransferIn_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_TransferIn_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_TransferIn_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_TransferIn_Item.Add(Tx_TransferIn_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_TransferIn_Item.DetId;

        //    }

        //    return DetId;

        //}

        //public void Detail_Update(HANA_APP CONTEXT, TransferIn_DetailModel model, int UserId)
        //{
        //    if (model != null)
        //    {

        //        Tx_TransferIn_Item Tx_TransferIn_Item = CONTEXT.Tx_TransferIn_Item.Find(model.DetId);

        //        if (Tx_TransferIn_Item != null)
        //        {
        //            var exceptColumns = new string[] { "DetId", "Id" };
        //            CopyProperty.CopyProperties(model, Tx_TransferIn_Item, false, exceptColumns);


        //            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //            Tx_TransferIn_Item.ModifiedDate = dtModified;
        //            Tx_TransferIn_Item.ModifiedUser = UserId;
        //            if (model.StartDate != null && model.EndDate == null)
        //            {
        //                Tx_TransferIn_Item.Status = "On Progress";
        //            }
        //            else if (model.StartDate != null && model.EndDate != null)
        //            {
        //                Tx_TransferIn_Item.Status = "Close";
        //            }
        //            else
        //            {
        //                Tx_TransferIn_Item.Status = "Open";
        //            }
        //            CONTEXT.SaveChanges();

        //        }


        //    }

        //}

        //public void Detail_Delete(HANA_APP CONTEXT, TransferIn_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_TransferIn_Item\"  WHERE \"DetId\"=:p0", model.DetId);

        //            CONTEXT.SaveChanges();


        //        }
        //    }

        //}

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

                        Tx_TransferIn tx_TransferIn = CONTEXT.Tx_TransferIn.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "TransferIn", CONTEXT, "before", "Tx_TransferIn", "post", "Id", keyValue);

                        if (tx_TransferIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferIn.Status = "Posted";
                            tx_TransferIn.IsAfterPosted = "Y";
                            tx_TransferIn.ModifiedDate = dtModified;
                            tx_TransferIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferIn", CONTEXT, "after", "Tx_TransferIn", "post", "Id", keyValue);


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

                        Tx_TransferIn tx_TransferIn = CONTEXT.Tx_TransferIn.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "TransferIn", CONTEXT, "before", "Tx_TransferIn", "cancel", "Id", keyValue);
                        if (tx_TransferIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferIn.Status = "Cancel";
                            tx_TransferIn.CancelReason = cancelReason;
                            tx_TransferIn.ModifiedDate = dtModified;
                            tx_TransferIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferIn", CONTEXT, "after", "Tx_TransferIn", "cancel", "Id", keyValue);


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


        public TransferInItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            TransferInItemTagView___ model = new TransferInItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_TransferIn_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<TransferInItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_TransferIn_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.TransferInItemTagModel___ = CONTEXT.Database.SqlQuery<TransferInItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

    }


    #endregion

}