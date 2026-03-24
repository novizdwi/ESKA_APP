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

namespace Models.Transaction.StockOpname
{
    #region Models

    public class StockOpnameScanModel
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

        public string RequestNo { get; set; }

        public long? RequestId { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public long? SummaryId_ { get; set; }

        public string SummaryTransNo_ { get; set; }

        public List<StockOpnameScan_ItemModel> ListDetails_ = new List<StockOpnameScan_ItemModel>();

        public StockOpnameScan_Item Details_ { get; set; }
    }

    public class StockOpnameScan_Item
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockOpnameScan_ItemModel> insertedRowValues { get; set; }
        public List<StockOpnameScan_ItemModel> modifiedRowValues { get; set; }
    }

    public class StockOpnameScan_ItemModel
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

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityScan { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }
    }

    public class StockOpnameScanItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<StockOpnameScanItemTagModel> StockOpnameScan_Item_TagModel___ { get; set; }
    }

    public class StockOpnameScanItemTagModel
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

    public class StockOpnameScanService
    {

        public StockOpnameScanModel GetNewModel(int userId)
        {
            StockOpnameScanModel model = new StockOpnameScanModel();
            model.Status = "Draft";
            return model;
        }
        public StockOpnameScanModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public StockOpnameScanModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            StockOpnameScanModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T2.""WhsName"",
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_StockOpname"" T0
                            LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OWHS"" T2 ON T0.""WhsCode"" = T2.""WhsCode""
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<StockOpnameScanModel>(ssql, id).Single();

                model.ListDetails_ = this.StockOpname_Items(CONTEXT, id);

                if (model.Status != "Draft")
                {
                    string sSummaryId = @"
                        SELECT TOP 1 T1.""Id""
                        FROM ""Tx_StockSummaryOpname_Ref"" T0
                        INNER JOIN ""Tx_StockSummaryOpname"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                        ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryId_ = CONTEXT.Database.SqlQuery<long?>(sSummaryId, id).SingleOrDefault();

                    string sSummaryNo = @"
                        SELECT TOP 1 T1.""TransNo""
                        FROM ""Tx_StockSummaryOpname_Ref"" T0
                        INNER JOIN ""Tx_StockSummaryOpname"" T1 ON T0.""Id"" = T1.""Id""
                        WHERE T0.""Status"" NOT IN('Cancel')
                        AND T0.""BaseId"" = :p0
                        ORDER BY T1.""Id"" DESC
                    ";
                    model.SummaryTransNo_ = CONTEXT.Database.SqlQuery<string>(sSummaryNo, id).SingleOrDefault();

                }

            }

            return model;
        }

        public List<StockOpnameScan_ItemModel> StockOpname_Items(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return StockOpname_Items(CONTEXT, id);
            }

        }

        public List<StockOpnameScan_ItemModel> StockOpname_Items(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_StockOpname_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var stockOpname = CONTEXT.Database.SqlQuery<StockOpnameScan_ItemModel>(ssql, id).ToList();
            return stockOpname;
        }

        public StockOpnameScanModel NavFirst(int userId)
        {
            StockOpnameScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpnameScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public StockOpnameScanModel NavPrevious(int userId, long id = 0)
        {
            StockOpnameScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpnameScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public StockOpnameScanModel NavNext(int userId, long id = 0)
        {
            StockOpnameScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpnameScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public StockOpnameScanModel NavLast(int userId)
        {
            StockOpnameScanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockOpnameScan");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockOpname\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(StockOpnameScanModel model)
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

                            Tx_StockOpname Tx_StockOpname = new Tx_StockOpname();
                            CopyProperty.CopyProperties(model, Tx_StockOpname, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_StockOpname.TransType = "StockOpnameScan";
                            Tx_StockOpname.CreatedDate = dtModified;
                            Tx_StockOpname.CreatedUser = model._UserId;
                            Tx_StockOpname.ModifiedDate = dtModified;
                            Tx_StockOpname.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'StockOpnameScan','" + dateX + "','') ").SingleOrDefault();
                            Tx_StockOpname.TransNo = transNo;

                            CONTEXT.Tx_StockOpname.Add(Tx_StockOpname);
                            CONTEXT.SaveChanges();
                            Id = Tx_StockOpname.Id;

                            String keyValue;
                            keyValue = Tx_StockOpname.Id.ToString();

                            CONTEXT.Database.ExecuteSqlCommand(
                            "CALL \"SpGoodsReceiptPO_AddItemDetail\" ({0}, {1}, {2})",
                            model._UserId,
                            keyValue
                        );

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_StockOpname", "add", "Id", keyValue);


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

        public void Update(StockOpnameScanModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "StockOpnameScan", CONTEXT, "before", "StockOpnameScan", "update", "Id", keyValue);


                                Tx_StockOpname Tx_StockOpname = CONTEXT.Tx_StockOpname.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_StockOpname.ModifiedDate = dtModified;
                                Tx_StockOpname.ModifiedUser = model._UserId;

                                if (Tx_StockOpname != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "TransNo", "RequestId", "RequestNo", "WhsCode", "ScanDeviceId", "CreatedUser", "CreatedDate" };
                                    CopyProperty.CopyProperties(model, Tx_StockOpname, false, exceptColumns);
                                    Tx_StockOpname.ModifiedDate = dtModified;
                                    Tx_StockOpname.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();
                                    
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "StockOpnameScan", CONTEXT, "after", "StockOpnameScan", "update", "Id", keyValue);
                                    
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

                        Tx_StockOpname tx_StockOpname = CONTEXT.Tx_StockOpname.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpnameScan", CONTEXT, "before", "Tx_StockOpname", "post", "Id", keyValue);

                        if (tx_StockOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_StockOpname.Status = "Posted";
                            tx_StockOpname.ModifiedDate = dtModified;
                            tx_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpnameScan", CONTEXT, "after", "Tx_StockOpname", "post", "Id", keyValue);


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

                        Tx_StockOpname tx_StockOpname = CONTEXT.Tx_StockOpname.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpnameScan", CONTEXT, "before", "Tx_StockOpname", "cancel", "Id", keyValue);
                        if (tx_StockOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_StockOpname.Status = "Cancel";
                            tx_StockOpname.CancelReason = cancelReason;
                            tx_StockOpname.ModifiedDate = dtModified;
                            tx_StockOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "StockOpnameScan", CONTEXT, "after", "Tx_StockOpname", "cancel", "Id", keyValue);


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


        public StockOpnameScanItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            StockOpnameScanItemTagView___ model = new StockOpnameScanItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_StockOpname_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<StockOpnameScanItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_StockOpname_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.StockOpnameScan_Item_TagModel___ = CONTEXT.Database.SqlQuery<StockOpnameScanItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

    }


    #endregion

}