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

    public class StockSummaryOpnameModel
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

        public string TransType { get; set; }

        public long Id { get; set; }

        public long? RequestId { get; set; }

        public string RequestNo { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? PostingDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public string DocNum_ { get; set; }

        public string ScanDeviceId { get; set; }

        [Required(ErrorMessage = "required")]
        public string PillarsCode { get; set; }

        public string PillarsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        [Required(ErrorMessage = "required")]
        public string SubClass1Code { get; set; }

        public string SubClass1Name { get; set; }

        [Required(ErrorMessage = "required")]
        public string SubClass2Code { get; set; }

        public string SubClass2Name { get; set; }

        [Required(ErrorMessage = "required")]
        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string Status { get; set; }

        public string CheckNeedApproval_ { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public List<StockSummaryOpname_DetailModel> ListDetail_ = new List<StockSummaryOpname_DetailModel>();

        public List<StockSummaryOpname_RefModel> ListRef_ = new List<StockSummaryOpname_RefModel>();

        public StockSummaryOpname_Ref Refs_ { get; set; }

        public StockSummaryOpname_Detail Details_ { get; set; }

        public List<GetCodeNameModel> PillarsList { get; set; }

        public List<GetCodeNameModel> ClassList { get; set; }

        public List<GetCodeNameModel> SubClass1List { get; set; }

        public List<GetCodeNameModel> SubClass2List { get; set; }

        public List<GetCodeNameModel> ProjectList { get; set; }
    }

    public class StockSummaryOpname_ApprovalModel
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

        public int? StageId { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; }

        public int? Step { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public DateTime? ActionDate { get; set; }
    }


    public class StockSummaryOpname_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockSummaryOpname_ApprovalModel> insertedRowValues { get; set; }
        public List<StockSummaryOpname_ApprovalModel> modifiedRowValues { get; set; }
    }


    public class StockSummaryOpname_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockSummaryOpname_DetailModel> insertedRowValues { get; set; }
        public List<StockSummaryOpname_DetailModel> modifiedRowValues { get; set; }
    }

    public class StockSummaryOpname_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int? RowNo { get; set; }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public int? IdPDO { get; set; }

        public string PDODocNum_ { get; set; }

        public int? GoodsReceiptDocEntry { get; set; }

        public string GRDocNum_ { get; set; }

        public string Bagian { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string AcctCode { get; set; }

        public string AcctName { get; set; }

        public decimal? UnitPriceTc { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public decimal? QuantityOnHandSAP { get; set; }

        public decimal? QuantityOnHandSAP_ { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityValid  { get; set; }

        public decimal? QuantityScan { get; set; }

        public decimal? OnHand { get; set; }

        public decimal? QtyVariance { get; set; }

        public decimal? QtyVariance_ { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public string PillarsCode { get; set; }

        public string PillarsName { get; set; }

        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        public string SubClass1Code { get; set; }

        public string SubClass1Name { get; set; }

        public string SubClass2Code { get; set; }

        public string SubClass2Name { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public long? DocEntry { get; set; }

        public long? LineNum { get; set; }

        public long? BaseEntry { get; set; }

        public int? BaseLine { get; set; }

        public string LineStatus { get; set; }

        public string IsBypassSAP { get; set; }
    }

    public class StockSummaryOpname_Ref
    {
        public List<long> deletedRowKeys { get; set; }
        public List<StockSummaryOpname_RefModel> insertedRowValues { get; set; }
        public List<StockSummaryOpname_RefModel> modifiedRowValues { get; set; }
    }

    public class StockSummaryOpname_RefModel
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

        public long? BaseId { get; set; }

        public string BaseNo { get; set; }

        public DateTime? BaseCreatedDate { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public string BaseCreatedUser_ { get; set; }
    }


    public class StockSummaryOpnameItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<StockSummaryOpnameItemTagModel> StockSummaryOpnameItemTagModel___ { get; set; }
    }

    public class StockSummaryOpnameItemTagModel
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

    public class GRPOAddResultModel
    {
        public string DocEntry { get; set; }

        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    public class GoodsReceiptResultModel
    {
        public long DetId { get; set; }

        public long? GoodsReceiptId { get; set; }
    }

    public class StockSummaryOpnameApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<StockSummaryOpname_ApprovalModel> ApprovalStepList__ = new List<StockSummaryOpname_ApprovalModel>();

        public StockSummaryOpname_Approval ApprovalStep__ { get; set; }
    }

    #endregion

    #region Services

    public class StockSummaryOpnameService
    {

        public StockSummaryOpnameModel GetNewModel(int userId)
        {
            StockSummaryOpnameModel model = new StockSummaryOpnameModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;

            model.PillarsList = GeneralGetList.GetCostCenterList("1");
            model.ClassList = GeneralGetList.GetCostCenterList("2");
            model.SubClass1List = GeneralGetList.GetCostCenterList("3");
            model.SubClass2List = GeneralGetList.GetCostCenterList("4");
            model.ProjectList = GeneralGetList.GetProjectList();
            return model;
        }

        public StockSummaryOpnameModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public StockSummaryOpnameModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method ="")
        {
            StockSummaryOpnameModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *,
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_StockSummaryOpname"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";
                model = CONTEXT.Database.SqlQuery<StockSummaryOpnameModel>(ssql, id).Single();

                if (model.DocEntry != null)
                {
                    string getDocNum = @"SELECT T1.""DocNum""
                    FROM ""Tx_StockSummaryOpname"" T0
                    INNER JOIN """ + DbProvider.dbSap_Name + @""".""OIQR"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                    WHERE T0.""Id"" = :p0 
                    ORDER BY T0.""Id"" ASC
                ";
                    model.DocNum_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id).FirstOrDefault();
                }

                //if (model.Id > 0)
                //{
                //    ssql = "CALL \"SpSysCheckNeedApproval\" (" + userId.ToString() + "," + model.Id.ToString() + ",'StockSummaryOpname') ";
                //    model.CheckNeedApproval_ = CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
                //}

                model.ListDetail_ = this.StockSummaryOpname_Details(CONTEXT, id);
                model.ListRef_ = this.StockSummaryOpname_Refs(CONTEXT, id);

                if (method != "post")
                {
                    if (model.Status == "Draft")
                    {
                        int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'StockSummaryOpname', :p1) ", userId, model.Id).FirstOrDefault();
                        model.ApprovalTemplateId_ = approvalId;
                    }

                    if (model.ApprovalStatus == "Waiting")
                    {
                        string getDocNum = @"SELECT 'Y'
			                FROM ""Tx_StockSummaryOpname"" T0
			                INNER JOIN  ""Tx_StockSummaryOpname_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
			                WHERE T0.""Id"" = :p0 
			                AND T1.""UserId"" = :p1
		                ";

                        model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                    }

                    model.PillarsList = GeneralGetList.GetCostCenterList("1");
                    model.ClassList = GeneralGetList.GetCostCenterList("2");
                    model.SubClass1List = GeneralGetList.GetCostCenterList("3");
                    model.SubClass2List = GeneralGetList.GetCostCenterList("4");
                    model.ProjectList = GeneralGetList.GetProjectList();
                }
            }

            return model;
        }
        public List<StockSummaryOpname_DetailModel> StockSummaryOpname_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return StockSummaryOpname_Details(CONTEXT, id);
            }

        }

        public List<StockSummaryOpname_DetailModel> StockSummaryOpname_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT ROW_NUMBER() OVER (PARTITION BY ""Id""  ORDER BY ""DetId"") AS ""RowNo"",
                T0.*, COALESCE(T1.""OnHand"",0) AS ""OnHand"", (COALESCE(T0.""QuantityValid"",0) - COALESCE(T1.""OnHand"",0)) AS ""QtyVariance_"",
                T3.""OnHand"" AS ""QuantityOnHandSAP_""
                FROM ""Tx_StockSummaryOpname_Item"" T0
                LEFT JOIN ""Tm_Item_Warehouse"" T1 ON T0.""ItemCode"" = T1.""ItemCode"" AND T0.""WhsCode"" = T1.""WhsCode""
                LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITW"" T3 ON T0.""ItemCode"" = T3.""ItemCode"" AND T0.""WhsCode"" = T3.""WhsCode""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var StockSummaryOpname = CONTEXT.Database.SqlQuery<StockSummaryOpname_DetailModel>(ssql, id).ToList();
            return StockSummaryOpname;
        }

        public List<StockSummaryOpname_RefModel> StockSummaryOpname_Refs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return StockSummaryOpname_Refs(CONTEXT, id);
            }

        }

        public List<StockSummaryOpname_RefModel> StockSummaryOpname_Refs(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""TransDate"", T2.""FirstName"" AS ""BaseCreatedUser_""
                FROM ""Tx_StockSummaryOpname_Ref"" T0
                INNER JOIN ""Tx_StockOpname"" T1 ON T0.""BaseId"" = T1.""Id""
                LEFT JOIN ""Tm_User"" T2 ON T0.""BaseCreatedUser"" = T2.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var StockSummaryOpname = CONTEXT.Database.SqlQuery<StockSummaryOpname_RefModel>(ssql, id).ToList();
            return StockSummaryOpname;
        }

        public List<StockSummaryOpname_ApprovalModel> GetStockSummaryOpname_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetStockSummaryOpname_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<StockSummaryOpname_ApprovalModel> GetStockSummaryOpname_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_StockSummaryOpname_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<StockSummaryOpname_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public StockSummaryOpnameModel NavFirst(int userId)
        {
            StockSummaryOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockSummaryOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockSummaryOpname\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public StockSummaryOpnameModel NavPrevious(int userId, long id = 0)
        {
            StockSummaryOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockSummaryOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockSummaryOpname\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public StockSummaryOpnameModel NavNext(int userId, long id = 0)
        {
            StockSummaryOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockSummaryOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockSummaryOpname\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public StockSummaryOpnameModel NavLast(int userId)
        {
            StockSummaryOpnameModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "StockSummaryOpname");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_StockSummaryOpname\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public bool RefreshItem(int userId, long id)
        {
            bool ret = true;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpStockSummaryOpname_AddItemDetail\"(:p0,:p1,'Refresh')", userId, id);
                        CONTEXT_TRANS.Commit();
                    }
                }
            }
            return ret;
        }

        public long Add(StockSummaryOpnameModel model)
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
                            Tx_StockSummaryOpname tx_StockSummaryOpname = new Tx_StockSummaryOpname();
                            CopyProperty.CopyProperties(model, tx_StockSummaryOpname, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            
                            tx_StockSummaryOpname.TransType = "StockSummaryOpname";
                            tx_StockSummaryOpname.CreatedDate = dtModified;
                            tx_StockSummaryOpname.CreatedUser = model._UserId;
                            tx_StockSummaryOpname.ModifiedDate = dtModified;
                            tx_StockSummaryOpname.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'StockSummaryOpname','" + dateX + "','') ").SingleOrDefault();
                            tx_StockSummaryOpname.TransNo = transNo;

                            CONTEXT.Tx_StockSummaryOpname.Add(tx_StockSummaryOpname);
                            CONTEXT.SaveChanges();
                            Id = tx_StockSummaryOpname.Id;

                            String keyValue;
                            keyValue = tx_StockSummaryOpname.Id.ToString();
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "StockSummaryOpname", CONTEXT, "after", "StockSummaryOpname", "add", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpStockSummaryOpname_AddItemDetail\"(:p0,:p1,'Add')", model._UserId, Id);

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

        public void Update(StockSummaryOpnameModel model, string method = "")
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "StockSummaryOpname", CONTEXT, "before", "StockSummaryOpname", "update", "Id", keyValue);


                                Tx_StockSummaryOpname tx_StockSummaryOpname = CONTEXT.Tx_StockSummaryOpname.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                               
                                if (tx_StockSummaryOpname != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_StockSummaryOpname, false, exceptColumns);

                                    tx_StockSummaryOpname.ModifiedDate = dtModified;
                                    tx_StockSummaryOpname.ModifiedUser = model._UserId;

                                    if (method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"StockSummaryOpname_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
                                    }

                                    CONTEXT.SaveChanges();

                                    if (model.Details_ != null)
                                    {
                                    //    if (model.Details_.insertedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.insertedRowValues)
                                    //        {
                                    //            Detail_Add(CONTEXT, detail, model.Id, model._UserId);
                                    //        }
                                    //    }

                                    if (model.Details_.modifiedRowValues != null)
                                    {
                                        foreach (var detail in model.Details_.modifiedRowValues)
                                        {
                                            Detail_Update(CONTEXT, detail, model._UserId);
                                        }
                                    }

                                    //    if (model.Details_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.Details_.deletedRowKeys)
                                    //        {
                                    //            StockSummaryOpname_DetailModel detailModel = new StockSummaryOpname_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    }
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "StockSummaryOpname", CONTEXT, "after", "StockSummaryOpname", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, StockSummaryOpname_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_StockSummaryOpname_Item Tx_StockSummaryOpname_Item = new Tx_StockSummaryOpname_Item();

        //        CopyProperty.CopyProperties(model, Tx_StockSummaryOpname_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_StockSummaryOpname_Item.Id = Id;
        //        Tx_StockSummaryOpname_Item.CreatedDate = dtModified;
        //        Tx_StockSummaryOpname_Item.CreatedUser = UserId;
        //        Tx_StockSummaryOpname_Item.ModifiedDate = dtModified;
        //        Tx_StockSummaryOpname_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_StockSummaryOpname_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_StockSummaryOpname_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_StockSummaryOpname_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_StockSummaryOpname_Item.Add(Tx_StockSummaryOpname_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_StockSummaryOpname_Item.DetId;

        //    }

        //    return DetId;

        //}

        public void Detail_Update(HANA_APP CONTEXT, StockSummaryOpname_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_StockSummaryOpname_Item Tx_StockSummaryOpname_Item = CONTEXT.Tx_StockSummaryOpname_Item.Find(model.DetId);

                if (Tx_StockSummaryOpname_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QtyVariance", "QuantityOnHandSAP" };
                    CopyProperty.CopyProperties(model, Tx_StockSummaryOpname_Item, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_StockSummaryOpname_Item.ModifiedDate = dtModified;
                    Tx_StockSummaryOpname_Item.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }

        //public void Detail_Delete(HANA_APP CONTEXT, StockSummaryOpname_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_StockSummaryOpname_Item\"  WHERE \"DetId\"=:p0", model.DetId);

        //            CONTEXT.SaveChanges();


        //        }
        //    }

        //}

        public void Post(int userId, StockSummaryOpnameModel stockSummaryOpnameModel)
        {
            try
            {
                Update(stockSummaryOpnameModel, "Post");
                PostSAP(userId, stockSummaryOpnameModel.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;
            StockSummaryOpnameModel StockSummaryOpname = GetById(userId, id, "post");
            int docEntry = 0;
            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        oCompany = SAPCachedCompany.GetCompany();
                        oCompany.StartTransaction();

                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "before", "Tx_StockSummaryOpname", "post", "Id", keyValue);

                        Tx_StockSummaryOpname tx_StockSummaryOpname = CONTEXT.Tx_StockSummaryOpname.Find(id);
                        if (tx_StockSummaryOpname == null)
                        {
                            throw new Exception($"[VALIDATION] - Data not found");
                        }

                        if(StockSummaryOpname.ListDetail_.All(q => q.QuantityValid == 0))
                        {
                            throw new Exception($"[VALIDATION] - No record created");
                        }

                        if(StockSummaryOpname.ListDetail_.Any(x => x.IsBypassSAP != "Y"))
                        {
                            docEntry = AddInventoryPosting(oCompany, userId, id, StockSummaryOpname);
                            if(docEntry <= 0)
                            {
                                throw new Exception($"[VALIDATION] - No inventory posting created");
                            }
                        }

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        
                        tx_StockSummaryOpname.DocEntry = docEntry;
                        tx_StockSummaryOpname.PostingDate = dtModified;
                        tx_StockSummaryOpname.Status = "Posted";
                        tx_StockSummaryOpname.IsAfterPosted = "Y";
                        tx_StockSummaryOpname.ModifiedDate = dtModified;
                        tx_StockSummaryOpname.ModifiedUser = userId;

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_InsertItemTag\"(:p0,:p1, 'StockSummaryOpname','A')", userId, keyValue);

                        CONTEXT.SaveChanges();

                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "after", "Tx_StockSummaryOpname", "post", "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpStockSummaryOpname_UpdateStockOpnameStatus\"(:p0,:p1,'post')", userId, id);

                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        }

                        CONTEXT_TRANS.Commit();
                    }

                    catch (Exception ex)
                    {
                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }

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
                    finally
                    {
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }

        private int AddInventoryPosting(Company oCompany, int userId, long id, StockSummaryOpnameModel model)
        {
            int newDocEntry = -1;
            int nErr;
            string errMsg;

            SAPbobsCOM.CompanyService oCS = (SAPbobsCOM.CompanyService)oCompany.GetCompanyService();
            SAPbobsCOM.InventoryPostingsService oInventoryPostingsService = oCS.GetBusinessService(SAPbobsCOM.ServiceTypes.InventoryPostingsService);
            SAPbobsCOM.InventoryPosting oDocument = oInventoryPostingsService.GetDataInterface(SAPbobsCOM.InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

            oDocument.PostingDate = DateTime.Now;

            if(!string.IsNullOrWhiteSpace(model.Comments))
            {
                oDocument.Remarks = model.Comments;
                oDocument.JournalRemark = model.Comments;
            }

            oDocument.UserFields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            oDocument.UserFields.Item("U_IDU_WebTransNo").Value = model.TransNo;
            if(model.ListDetail_.Count > 0)
            {
                foreach(var item in model.ListDetail_.Where(x=>x.IsBypassSAP != "Y"))
                {
                    if(item.QuantityValid > 0)
                    {
                        InventoryPostingLine line = oDocument.InventoryPostingLines.Add();
                        line.ItemCode = item.ItemCode;
                        line.WarehouseCode = item.WhsCode;
                        line.CountedQuantity = Convert.ToDouble(item.QuantityValid);
                        line.UoMCode = item.Uom?? "" ;

                        line.Price = (double)item.UnitPriceTc;
                        
                        line.InventoryOffsetIncreaseAccount = item.AcctCode;
                        line.InventoryOffsetDecreaseAccount = item.AcctCode;

                        line.CostingCode = item.PillarsCode;
                        line.CostingCode2 = item.ClassCode;
                        line.CostingCode3 = item.SubClass1Code;
                        line.CostingCode4 = item.SubClass2Code;
                        line.ProjectCode = item.ProjectCode;

                        line.UserFields.Item("U_IDU_WebId").Value = Convert.ToInt32(item.Id);
                        line.UserFields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);
                    }
                }
            }

            InventoryPostingParams oParams = oInventoryPostingsService.Add(oDocument);
            newDocEntry = oParams.DocumentEntry;

            if (newDocEntry <= 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Inventory Posting : " + nErr.ToString() + "|" + errMsg);
            }

            return newDocEntry;
        }


        //private GRPOAddResultModel AddStockSummaryOpname(Company oCompany, int userId, long id, StockSummaryOpnameModel model)
        //{
        //    GRPOAddResultModel result = new GRPOAddResultModel();

        //    int nErr;
        //    string errMsg;
        //    //SAPbobsCOM.Recordset rsDetailSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

        //    SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

        //    oDocument.DocDate = (DateTime)model.TransDate;
        //    oDocument.DocDueDate = (DateTime)model.TransDate;
        //    oDocument.TaxDate = (DateTime)model.TransDate;

        //    oDocument.CardCode = model.VendorCode;
        //    oDocument.CardName = model.VendorCode;

        //    if (model.RefNo != null)
        //    {
        //        oDocument.NumAtCard = model.RefNo;
        //    }

        //    if (model.Comments != null)
        //    {
        //        oDocument.Comments = model.Comments;
        //    }

        //    if (model.Address != null)
        //    {
        //        oDocument.Address = model.Address;
        //    }

        //    oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
        //    oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;

        //    int i = 0;
        //    Dictionary<long, int> InsertedLine = new Dictionary<long, int>();
        //    if (model.ListDetails_.Count > 0)
        //    {
        //        foreach (var item in model.ListDetails_)
        //        {
        //            oDocument.Lines.BaseType = 22;
        //            oDocument.Lines.BaseEntry = Convert.ToInt32(item.BaseEntry);
        //            oDocument.Lines.BaseLine = Convert.ToInt32(item.BaseLine);

        //            //oDocument.Lines.ItemCode = item.ItemCode;
        //            oDocument.Lines.WarehouseCode = item.WhsCode;
        //            oDocument.Lines.Quantity = (double)item.QuantityScan;

        //            if (item.UomEntry != null)
        //            {
        //                oDocument.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
        //            }

        //            //if (item.FreeText != null)
        //            //{
        //            //    oDocument.Lines.FreeText = item.FreeText;
        //            //}

        //            oDocument.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
        //            oDocument.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);

        //            oDocument.Lines.Add();
        //            InsertedLine.Add(Convert.ToInt64(item.DetId) , i);
        //            i ++;
        //        }
        //    }

        //    int docAdd = oDocument.Add();
        //    if (docAdd != 0)
        //    {
        //        nErr = oCompany.GetLastErrorCode();
        //        errMsg = oCompany.GetLastErrorDescription();

        //        SapCompany.CleanUp(oDocument);

        //        throw new Exception("[VALIDATION] - Add Goods Receipt PO : " + nErr.ToString() + "|" + errMsg);
        //    }

        //    result.DocEntry = oCompany.GetNewObjectKey();
        //    result.LineMapping = InsertedLine;

        //    SapCompany.CleanUp(oDocument);
        //    return result;
        //}



        private List<GoodsReceiptResultModel> AddReceiveFromProduction(Company oCompany, int userId, long id, StockSummaryOpnameModel model)
        {
            List<GoodsReceiptResultModel> result = new List<GoodsReceiptResultModel>();
            int nErr;
            string errMsg;
            
            if (model.ListDetail_.Count > 0)
            {
                foreach (var item in model.ListDetail_)
                {
                    SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
                    oDocument.DocDate = model.TransDate?? DateTime.Now;

                    oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                    oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;

                    oDocument.Lines.BaseType = 202;
                    oDocument.Lines.BaseEntry = item.IdPDO ?? 0;
                    oDocument.Lines.Quantity = double.Parse(item.QuantityScan.ToString());

                    oDocument.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                    oDocument.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);

                    oDocument.Lines.Add();

                    if (oDocument.Add() != 0)
                    {

                        nErr = oCompany.GetLastErrorCode();
                        errMsg = oCompany.GetLastErrorDescription();

                        SapCompany.CleanUp(oDocument);

                        throw new Exception("[VALIDATION] - Add ReceiptProduction | " + nErr.ToString() + "|" + errMsg);

                    }
                    string docEntry =  oCompany.GetNewObjectKey();
                    result.Add( 
                        new GoodsReceiptResultModel
                        {
                            DetId = Convert.ToInt64(item.DetId),
                            GoodsReceiptId = Convert.ToInt32(docEntry)
                        }
                    );
                }
            }

            return result;
        }

        public void Cancel(int userId, long Id, string cancelReason)
        {
            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        oCompany.StartTransaction();

                        String keyValue;
                        keyValue = Id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "before", "Tx_StockSummaryOpname", "cancel", "Id", keyValue);

                        Tx_StockSummaryOpname tx_StockSummaryOpname = CONTEXT.Tx_StockSummaryOpname.Find(Id);
                        if (tx_StockSummaryOpname != null)
                        {
                            //if (tx_StockSummaryOpname.IsAfterPosted == "Y")
                            //{
                            //    Cancel_InventoryPosting(oCompany, Convert.ToInt32(tx_StockSummaryOpname.DocEntry));
                            //    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_InsertItemTag\"(:p0,:p1, 'StockSummaryOpname','I')", userId, Id);
                            //    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpStockSummaryOpname_UpdateStockOpnameStatus\"(:p0,:p1,'cancel')", userId, Id);
                            //}

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_StockSummaryOpname.Status = "Cancel";
                            tx_StockSummaryOpname.CancelReason = cancelReason;
                            tx_StockSummaryOpname.ModifiedDate = dtModified;
                            tx_StockSummaryOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "after", "Tx_StockSummaryOpname", "cancel", "Id", keyValue);

                        oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

                        CONTEXT_TRANS.Commit();
                    }

                    catch (Exception ex)
                    {
                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }

                        CONTEXT_TRANS.Rollback();

                        string errorMessage;
                        if (ex.Message.StartsWith("[VALIDATION]"))
                        {
                            errorMessage = ex.Message;
                        }
                        else
                        {
                            errorMessage = $"[VALIDATION] {ex.Message}";
                        }

                        throw new Exception(errorMessage);
                    }
                    finally
                    {
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }

        public void RequestApproval(int userId, long id, int templateId, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {
                StockSummaryOpnameModel StockSummaryOpnameModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "before", "Tx_StockSummaryOpname", "requestApproval", "Id", keyValue);

                        Tx_StockSummaryOpname tx_StockSummaryOpname = CONTEXT.Tx_StockSummaryOpname.Find(id);
                        if (tx_StockSummaryOpname != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_StockSummaryOpname.IsApproval = "Y";
                            tx_StockSummaryOpname.ApprovalMessages = approvalMessages;
                            tx_StockSummaryOpname.ApprovalStatus = "Waiting";
                            tx_StockSummaryOpname.ModifiedDate = dtModified;
                            tx_StockSummaryOpname.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'StockSummaryOpname',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "after", "Tx_StockSummaryOpname", "requestApproval", "Id", keyValue);
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

        public void Approve(int userId, long id, string approvalMessage)
        {
            string approvalStatus = string.Empty;
            try
            {
                approvalStatus = Authorize(userId, id, "Approve", approvalMessage);

                if (approvalStatus == "Approved")
                {
                    //StockSummaryOpnameModel StockSummaryOpnameModel = GetById(userId, id);
                    //this.Update(StockSummaryOpnameModel, "Post");
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"StockSummaryOpname_UpdateItem\"(:p0,:p1)", userId, id);
                            CONTEXT.SaveChanges();
                        }
                    }

                    this.PostSAP(userId, id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Authorize(int userId, long id, string action, string approvalMessage)
        {
            string approvalStatus = string.Empty;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "before", "Tx_StockSummaryOpname", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'StockSummaryOpname', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "StockSummaryOpname", CONTEXT, "after", "Tx_StockSummaryOpname", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();

                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_StockSummaryOpname"" T0
                            WHERE T0.""Id"" = :p0 
                        ";

                        approvalStatus = CONTEXT.Database.SqlQuery<string>(strApprovalStatus, id).FirstOrDefault();
                        return approvalStatus;
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

        public StockSummaryOpnameItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;
            StockSummaryOpnameItemTagView___ model = new StockSummaryOpnameItemTagView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_StockSummaryOpname_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<StockSummaryOpnameItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_StockSummaryOpname_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.StockSummaryOpnameItemTagModel___ = CONTEXT.Database.SqlQuery<StockSummaryOpnameItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public StockSummaryOpnameApprovalView___ GetViewApproval(long id)
        {
            StockSummaryOpnameApprovalView___ model = new StockSummaryOpnameApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_StockSummaryOpname"" T0 
                    LEFT JOIN ""Tx_StockSummaryOpname_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<StockSummaryOpnameApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetStockSummaryOpname_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

    }


    #endregion

}