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

    public class TransferOutModel
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

        public long? BaseEntry { get; set; }

        public string BaseDocNum { get; set; }

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

        public List<TransferOut_DetailModel> ListDetails_ = new List<TransferOut_DetailModel>();

        public TransferOut_Detail Details_ { get; set; }
    }
    public class TransferOut_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferOut_DetailModel> insertedRowValues { get; set; }
        public List<TransferOut_DetailModel> modifiedRowValues { get; set; }
    }
    public class TransferOut_DetailModel
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

    public class TransferOutItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<TransferOutItemTagModel> TransferOutItemTagModel___ { get; set; }
    }

    public class TransferOutItemTagModel
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

    public class TransferOutService
    {

        public TransferOutModel GetNewModel(int userId)
        {
            TransferOutModel model = new TransferOutModel();
            model.Status = "Draft";
            return model;
        }
        public TransferOutModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public TransferOutModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            TransferOutModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_TransferOut"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<TransferOutModel>(ssql, id).Single();

                model.ListDetails_ = this.TransferOut_Details(CONTEXT, id);

                if (model.Status != "Draft")
                {
                    string sSummaryId = @"
                        SELECT TOP 1 T1.""Id""
                        FROM ""Tx_TransferSummaryOut_Ref"" T0
                        INNER JOIN ""Tx_TransferSummaryOut"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                        ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryId_ = CONTEXT.Database.SqlQuery<long?>(sSummaryId, id).SingleOrDefault();

                    string sSummaryNo = @"
                        SELECT TOP 1 T1.""TransNo""
                        FROM ""Tx_TransferSummaryOut_Ref"" T0
                        INNER JOIN ""Tx_TransferSummaryOut"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                        ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryTransNo_ = CONTEXT.Database.SqlQuery<string>(sSummaryNo, id).SingleOrDefault();

                }

            }

            return model;
        }
        public List<TransferOut_DetailModel> TransferOut_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferOut_Details(CONTEXT, id);
            }

        }

        public List<TransferOut_DetailModel> TransferOut_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT * 
                FROM ""Tx_TransferOut_Item"" 
                WHERE ""Id"" =:p0
                ORDER BY ""DetId"" ASC
            ";
            var purchaseOrder = CONTEXT.Database.SqlQuery<TransferOut_DetailModel>(ssql, id).ToList();
            return purchaseOrder;
        }

        public TransferOutModel NavFirst(int userId)
        {
            TransferOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferOut\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public TransferOutModel NavPrevious(int userId, long id = 0)
        {
            TransferOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferOut\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TransferOutModel NavNext(int userId, long id = 0)
        {
            TransferOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferOut\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TransferOutModel NavLast(int userId)
        {
            TransferOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferOut\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public bool AddItem(int userId, long id)
        {
            bool ret = true;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferOut_AddItemDetail\"(:p0,:p1,'Refresh')", userId, id);
                }
            }
            return ret;
        }

        public bool AddItemTag(int userId, long id)
        {
            bool ret = true;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferOut_AddTagDetail\"(:p0,:p1,'Refresh')", userId, id);
                }
            }
            return ret;
        }

        public long Add(TransferOutModel model)
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

                            Tx_TransferOut Tx_TransferOut = new Tx_TransferOut();
                            CopyProperty.CopyProperties(model, Tx_TransferOut, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_TransferOut.TransType = "TransferOut";
                            Tx_TransferOut.CreatedDate = dtModified;
                            Tx_TransferOut.CreatedUser = model._UserId;
                            Tx_TransferOut.ModifiedDate = dtModified;
                            Tx_TransferOut.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TransferOut','" + dateX + "','') ").SingleOrDefault();
                            Tx_TransferOut.TransNo = transNo;

                            CONTEXT.Tx_TransferOut.Add(Tx_TransferOut);
                            CONTEXT.SaveChanges();
                            Id = Tx_TransferOut.Id;

                            String keyValue;
                            keyValue = Tx_TransferOut.Id.ToString();

                        //    CONTEXT.Database.ExecuteSqlCommand(
                        //    "CALL \"SpTransferOut_AddItemDetail\" ({0}, {1}, {2})",
                        //    model._UserId,
                        //    keyValue
                        //);

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "TransferOut", "add", "Id", keyValue);


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

        public void Update(TransferOutModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "TransferOut", CONTEXT, "before", "TransferOut", "update", "Id", keyValue);


                                Tx_TransferOut Tx_TransferOut = CONTEXT.Tx_TransferOut.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_TransferOut.ModifiedDate = dtModified;
                                Tx_TransferOut.ModifiedUser = model._UserId;

                                if (Tx_TransferOut != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_TransferOut, false, exceptColumns);
                                    Tx_TransferOut.ModifiedDate = dtModified;
                                    Tx_TransferOut.ModifiedUser = model._UserId;

                                    //if (model.StartDate != null)
                                    //{
                                    //    Tx_TransferOut.Status2 = "On Progress";
                                    //}
                                    //else
                                    //{
                                    //    Tx_TransferOut.Status2 = "Open";
                                    //}

                                    //if (model.EndDate != null)
                                    //{
                                    //    Tx_TransferOut.Status2 = "Close";
                                    //}
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
                                    //            TransferOut_DetailModel detailModel = new TransferOut_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TransferOut", CONTEXT, "after", "TransferOut", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, TransferOut_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_TransferOut_Item Tx_TransferOut_Item = new Tx_TransferOut_Item();

        //        CopyProperty.CopyProperties(model, Tx_TransferOut_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_TransferOut_Item.Id = Id;
        //        Tx_TransferOut_Item.CreatedDate = dtModified;
        //        Tx_TransferOut_Item.CreatedUser = UserId;
        //        Tx_TransferOut_Item.ModifiedDate = dtModified;
        //        Tx_TransferOut_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_TransferOut_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_TransferOut_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_TransferOut_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_TransferOut_Item.Add(Tx_TransferOut_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_TransferOut_Item.DetId;

        //    }

        //    return DetId;

        //}

        //public void Detail_Update(HANA_APP CONTEXT, TransferOut_DetailModel model, int UserId)
        //{
        //    if (model != null)
        //    {

        //        Tx_TransferOut_Item Tx_TransferOut_Item = CONTEXT.Tx_TransferOut_Item.Find(model.DetId);

        //        if (Tx_TransferOut_Item != null)
        //        {
        //            var exceptColumns = new string[] { "DetId", "Id" };
        //            CopyProperty.CopyProperties(model, Tx_TransferOut_Item, false, exceptColumns);


        //            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //            Tx_TransferOut_Item.ModifiedDate = dtModified;
        //            Tx_TransferOut_Item.ModifiedUser = UserId;
        //            if (model.StartDate != null && model.EndDate == null)
        //            {
        //                Tx_TransferOut_Item.Status = "On Progress";
        //            }
        //            else if (model.StartDate != null && model.EndDate != null)
        //            {
        //                Tx_TransferOut_Item.Status = "Close";
        //            }
        //            else
        //            {
        //                Tx_TransferOut_Item.Status = "Open";
        //            }
        //            CONTEXT.SaveChanges();

        //        }


        //    }

        //}

        //public void Detail_Delete(HANA_APP CONTEXT, TransferOut_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_TransferOut_Item\"  WHERE \"DetId\"=:p0", model.DetId);

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

                        Tx_TransferOut tx_TransferOut = CONTEXT.Tx_TransferOut.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "TransferOut", CONTEXT, "before", "Tx_TransferOut", "post", "Id", keyValue);

                        if (tx_TransferOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferOut.Status = "Posted";
                            tx_TransferOut.IsAfterPosted = "Y";
                            tx_TransferOut.ModifiedDate = dtModified;
                            tx_TransferOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferOut", CONTEXT, "after", "Tx_TransferOut", "post", "Id", keyValue);


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

                        Tx_TransferOut tx_TransferOut = CONTEXT.Tx_TransferOut.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "TransferOut", CONTEXT, "before", "Tx_TransferOut", "cancel", "Id", keyValue);
                        if (tx_TransferOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferOut.Status = "Cancel";
                            tx_TransferOut.CancelReason = cancelReason;
                            tx_TransferOut.ModifiedDate = dtModified;
                            tx_TransferOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferOut", CONTEXT, "after", "Tx_TransferOut", "cancel", "Id", keyValue);


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


        public TransferOutItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            TransferOutItemTagView___ model = new TransferOutItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_TransferOut_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<TransferOutItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_TransferOut_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.TransferOutItemTagModel___ = CONTEXT.Database.SqlQuery<TransferOutItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

    }


    #endregion

}