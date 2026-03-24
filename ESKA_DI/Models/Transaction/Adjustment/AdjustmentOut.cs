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
using Models;

namespace Models.Transaction.Adjustment
{
    #region Models
    public class AdjustmentOutModel
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

        public DateTime? TransDate { get; set; }

        public DateTime? PostingDate { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public int? DocEntry { get; set; }

        public string DocNum_ { get; set; }

        public string ScanDeviceId { get; set; }

        public string AdjustmentTypeCode { get; set; }

        public string AdjustmentTypeName { get; set; }

        public string WhsCode { get; set; }
        
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

        public string CancelReason { get; set; }

        public string CheckNeedApproval_ { get; set; }

        public string IsOpeningBalance { get; set; }

        [Required(ErrorMessage = "required")]
        public string Comments { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }


        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public List<AdjustmentOut_ItemModel> ListDetails_ = new List<AdjustmentOut_ItemModel>();

        public AdjustmentOut_Detail Details_ { get; set; }

        public List<AdjustmentOut_AttachmentModel> ListAttachments_ = new List<AdjustmentOut_AttachmentModel>();

        public List<GetCodeNameModel> PillarsList { get; set; }

        public List<GetCodeNameModel> ClassList { get; set; }

        public List<GetCodeNameModel> SubClass1List { get; set; }

        public List<GetCodeNameModel> SubClass2List { get; set; }

        public List<GetCodeNameModel> ProjectList { get; set; }
    }

    public class AdjustmentOut_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentOut_ItemModel> insertedRowValues { get; set; }
        public List<AdjustmentOut_ItemModel> modifiedRowValues { get; set; }
    }


    public class AdjustmentOut_ApprovalModel
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


    public class AdjustmentOut_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentOut_ApprovalModel> insertedRowValues { get; set; }
        public List<AdjustmentOut_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentOut_ItemModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

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

        public decimal? QuantityScan { get; set; }

        public decimal? QuantityPosted { get; set; }

        public decimal? EstQuantityPosted_ { get; set; }

        public decimal? OnHand_ { get; set; }

        public string LineStatus { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class AdjustmentOutItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public List<AdjustmentOut_Item_TagModel> AdjustmentOut_Item_TagModel___ { get; set; }

    }

    public class AdjustmentOut_Item_TagModel
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

        public string Information { get; set; }

        public string PostResultNote { get; set; }
    }


    public class AdjustmentOut_AttachmentModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int FileIndex_ { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string FileName { get; set; }

        public string Guid { get; set; }

    }

    public class AdjustmentOutApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<AdjustmentOut_ApprovalModel> ApprovalStepList__ = new List<AdjustmentOut_ApprovalModel>();

        public AdjustmentOut_Approval ApprovalStep__ { get; set; }
    }

    #endregion Models

    #region Services
    public class AdjustmentOutService
    {
        public AdjustmentOutModel GetNewModel(int userId)
        {
            AdjustmentOutModel model = new AdjustmentOutModel();
            model.Status = "Draft";
            return model;
        }

        public AdjustmentOutModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public AdjustmentOutModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            AdjustmentOutModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_AdjustmentOut"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";


                model = CONTEXT.Database.SqlQuery<AdjustmentOutModel>(ssql, id).Single();

                if(model.DocEntry != null)
                {
                    string getDocNum = @"SELECT T1.""DocNum""
                        FROM ""Tx_AdjustmentOut"" T0
                        INNER JOIN """ + DbProvider.dbSap_Name + @""".""OIGE"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                        WHERE T0.""Id"" = :p0 
                        ORDER BY T0.""Id"" ASC
                    ";
                    model.DocNum_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id).FirstOrDefault();
                }

                model.ListDetails_ = this.AdjustmentOut_Details(CONTEXT, id);
                model.ListAttachments_ = this.GetAdjustmentOut_Attachments(id);

                if (method != "post")
                {
                    if (model.Status == "Draft")
                    {
                        int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'AdjustmentOut', :p1) ", userId, model.Id).FirstOrDefault();
                        model.ApprovalTemplateId_ = approvalId;
                    }

                    if (model.ApprovalStatus == "Waiting")
                    {
                        string getDocNum = @"SELECT 'Y'
			                FROM ""Tx_AdjustmentOut"" T0
			                INNER JOIN  ""Tx_AdjustmentOut_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
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

        public List<AdjustmentOut_ItemModel> AdjustmentOut_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentOut_Details(CONTEXT, id);
            }

        }

        public List<AdjustmentOut_AttachmentModel> GetAdjustmentOut_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentOut_Attachments(CONTEXT, id);
            }

        }

        public List<AdjustmentOut_AttachmentModel> GetAdjustmentOut_Attachments(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_AdjustmentOut_Attachment"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var ret =  CONTEXT.Database.SqlQuery<AdjustmentOut_AttachmentModel>(ssql, id).ToList();
            return ret;
        }

        public List<AdjustmentOut_ItemModel> AdjustmentOut_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, 
                     COALESCE ( (SELECT COUNT(Tx.""TagId"")
                        FROM ""Tx_AdjustmentOut_Item_Tag"" Tx
                        INNER JOIN ""Tm_Item_Warehouse_Tag"" Ty ON Tx.""TagId"" = Ty.""TagId"" AND T2.""WhsCode"" = Ty.""WhsCode""  AND Ty.""Status"" = 'A'
                        WHERE Tx.""DetId"" = T0.""DetId""
                    ) , 0) AS ""EstQuantityPosted_"",
                    T3.""OnHand"" AS ""OnHand_""
                FROM ""Tx_AdjustmentOut_Item"" T0
                INNER JOIN ""Tx_AdjustmentOut"" T2 ON T0.""Id"" = T2.""Id""
                LEFT JOIN ""Tm_Item_Warehouse"" T3 ON T0.""ItemCode"" = T3.""ItemCode"" AND T0.""WhsCode"" = T3.""WhsCode""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var AdjustmentOut = CONTEXT.Database.SqlQuery<AdjustmentOut_ItemModel>(ssql, id).ToList();
            return AdjustmentOut;
        }

        public List<AdjustmentOut_ApprovalModel> GetAdjustmentOut_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentOut_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<AdjustmentOut_ApprovalModel> GetAdjustmentOut_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_AdjustmentOut_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<AdjustmentOut_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public AdjustmentOutModel NavFirst(int userId)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public AdjustmentOutModel NavPrevious(int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public AdjustmentOutModel NavNext(int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public AdjustmentOutModel NavLast(int userId)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public void Update(AdjustmentOutModel model, string method = "")
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "before", "AdjustmentOut", "update", "Id", keyValue);


                                Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                                           

                                if (tx_AdjustmentOut != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_AdjustmentOut, false, exceptColumns);
                                    tx_AdjustmentOut.ModifiedDate = dtModified;
                                    tx_AdjustmentOut.ModifiedUser = model._UserId;

                                    //CONTEXT.SaveChanges();

                                    if (model.Details_ != null)
                                    {
                                        if (model.Details_.modifiedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.modifiedRowValues)
                                            {
                                                Detail_Update(CONTEXT, detail, model._UserId);
                                            }
                                        }

                                    }

                                    if(method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
                                    }

                                    CONTEXT.SaveChanges();
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "AdjustmentOut", "update", "Id", keyValue);

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

        public void Detail_Update(HANA_APP CONTEXT, AdjustmentOut_ItemModel model, int UserId)
        {
            if (model != null)
            {

                Tx_AdjustmentOut_Item tx_AdjustmentOut_Item = CONTEXT.Tx_AdjustmentOut_Item.Find(model.DetId);

                if (tx_AdjustmentOut_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, tx_AdjustmentOut_Item, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_AdjustmentOut_Item.ModifiedDate = dtModified;
                    tx_AdjustmentOut_Item.ModifiedUser = UserId;

                    //CONTEXT.SaveChanges();

                }


            }

        }

        public void Post(int userId, AdjustmentOutModel adjustmentOutModel)
        {
            try
            {
                Update(adjustmentOutModel, "Post");
                PostSAP(userId, adjustmentOutModel.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;

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

                        AdjustmentOutModel syncAdjustmentOut = GetById(userId, id, "post");
                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "post", "Id", keyValue);

                        if (tx_AdjustmentOut != null)
                        {
                            string docEntry = string.Empty;
                            if (syncAdjustmentOut.IsOpeningBalance != "Y")
                            {
                                docEntry = AddGoodsIssue(oCompany, userId, id, syncAdjustmentOut);
                            }

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentOut.PostingDate = dtModified;
                            tx_AdjustmentOut.DocEntry = Convert.ToInt32(docEntry);
                            tx_AdjustmentOut.Status = "Posted";

                            tx_AdjustmentOut.IsAfterPosted = "Y";
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_UpdateItemTag\"(:p0,:p1)", userId, id);

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "post", "Id", keyValue);

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

        private string AddGoodsIssue(Company oCompany, int userId, long id, AdjustmentOutModel model)
        {
            string result = "";

            string CoaAdjustment = GeneralGetList.GetSAPCoaAdjustment(model.AdjustmentTypeCode);
            int nErr;
            string errMsg;
            SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            oDocument.DocDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(model.Comments))
            {
                oDocument.Comments = model.Comments;
            }

            if (!string.IsNullOrWhiteSpace(model.Comments))
            {
                oDocument.JournalMemo = model.Comments;
            }

            oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;
            oDocument.UserFields.Fields.Item("U_IDU_AdjustmentType").Value = model.AdjustmentTypeName;

            if(model.ListDetails_.Count > 0)
            {
                foreach(var item in model.ListDetails_)
                {
                    if(item.QuantityPosted > 0)
                    {
                        oDocument.Lines.ItemCode = item.ItemCode;
                        oDocument.Lines.ItemDescription = item.ItemName;
                        
                        oDocument.Lines.Quantity = double.Parse(item.QuantityPosted.ToString());
                        oDocument.Lines.AccountCode = CoaAdjustment;
                        oDocument.Lines.WarehouseCode = item.WhsCode;

                        oDocument.Lines.CostingCode = item.PillarsCode;
                        oDocument.Lines.CostingCode2 = item.ClassCode;
                        oDocument.Lines.CostingCode3 = item.SubClass1Code;
                        oDocument.Lines.CostingCode4 = item.SubClass2Code;
                        oDocument.Lines.ProjectCode = item.ProjectCode;

                        //if (item.UomEntry != null)
                        //{
                        //    oDocument.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                        //}

                        oDocument.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                        oDocument.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);

                        oDocument.Lines.Add();
                    }
                }
            }

            int docAdd = oDocument.Add();
            if (docAdd != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Add Goods Receipt : " + nErr.ToString() + "|" + errMsg);
            }
            result = oCompany.GetNewObjectKey();
            SapCompany.CleanUp(oDocument);

            return result;
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

                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "cancel", "Id", keyValue);
                        if (tx_AdjustmentOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentOut.Status = "Cancel";
                            tx_AdjustmentOut.CancelReason = cancelReason;
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "cancel", "Id", keyValue);


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

        public void RequestApproval(int userId, long id, int templateId, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {
                AdjustmentOutModel AdjustmentOutModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "requestApproval", "Id", keyValue);

                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(id);
                        if (tx_AdjustmentOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentOut.IsApproval = "Y";
                            tx_AdjustmentOut.ApprovalMessages = approvalMessages;
                            tx_AdjustmentOut.ApprovalStatus = "Waiting";
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'AdjustmentOut',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "requestApproval", "Id", keyValue);
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
                    //AdjustmentOutModel AdjustmentOutModel = GetById(userId, id);
                    //this.Update(AdjustmentOutModel, "Post");
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut_UpdateItem\"(:p0,:p1)", userId, id);
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

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'AdjustmentOut', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();

                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_AdjustmentOut"" T0
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


        public AdjustmentOutItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            AdjustmentOutItemTagView___ model = new AdjustmentOutItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName"", T1.""WhsCode""
                                FROM ""Tx_AdjustmentOut_Item"" T0   
                                INNER JOIN ""Tx_AdjustmentOut"" T1 ON T0.""Id"" = T1.""Id""
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetDetId"") AS ""RowNo"", T0.*,
                        CASE WHEN T1.""Status"" = 'Draft' THEN 
                             CASE WHEN COALESCE(T3.""TagId"", '') = '' THEN 'Tag Id not Exists in master item'
                                  WHEN T1.""WhsCode"" != T3.""WhsCode"" THEN 'Tag Id in different Warehouse'
                                  WHEN T3.""Status"" != 'A' THEN 'Tag Id is not acitve'
                             ELSE 'Valid' END 
                        ELSE T0.""Status"" END AS ""Information""
                        FROM ""Tx_AdjustmentOut_Item_Tag"" T0  
                        INNER JOIN ""Tx_AdjustmentOut"" T1 ON T0.""Id"" = T1.""Id""  
                        LEFT JOIN ""Tm_Item_Warehouse_Tag"" T3 ON T0.""TagId"" = T3.""TagId"" 
                        WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 
                ";

                model.AdjustmentOut_Item_TagModel___ = CONTEXT.Database.SqlQuery<AdjustmentOut_Item_TagModel>(sql, id, detId).ToList();
            }

            return model;
        }

        public AdjustmentOutApprovalView___ GetViewApproval(long id)
        {
            AdjustmentOutApprovalView___ model = new AdjustmentOutApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_AdjustmentOut"" T0 
                    LEFT JOIN ""Tx_AdjustmentOut_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetAdjustmentOut_ApprovalSteps(CONTEXT, id);

            }
            return model;
        }

        #region Attachment
        public AdjustmentOut_AttachmentModel GetAdjustmentOut_Attachments_GetById(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentOut_Attachments_GetById(CONTEXT, id);
            }
        }

        public AdjustmentOut_AttachmentModel GetAdjustmentOut_Attachments_GetById(HANA_APP CONTEXT, long id = 0)
        {
            AdjustmentOut_AttachmentModel model = null;
            if (id != 0)
            { 
                string ssql = "SELECT TOP 1 T0.*  "
                                + " FROM \"Tx_AdjustmentOut_Attachment\" T0 "
                                + " WHERE T0.\"DetId\" = :p0 ";

            model = CONTEXT.Database.SqlQuery<AdjustmentOut_AttachmentModel>(ssql, id).Single();
            }

            return model;
        }


        public long Detail_Add(List<AdjustmentOut_AttachmentModel> ListModel)
        {
            long Id = 0;

            if (ListModel != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        for (int i = 0; i < ListModel.Count; i++)
                        {
                            Tx_AdjustmentOut_Attachment tx_AdjustmentOut_Attachment = new Tx_AdjustmentOut_Attachment();
                            var model = ListModel[i];

                            CopyProperty.CopyProperties(model, tx_AdjustmentOut_Attachment, false);
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentOut_Attachment.CreatedDate = dtModified;
                            tx_AdjustmentOut_Attachment.CreatedUser = model._UserId;
                            tx_AdjustmentOut_Attachment.ModifiedDate = dtModified;
                            tx_AdjustmentOut_Attachment.ModifiedUser = model._UserId;

                            CONTEXT.Tx_AdjustmentOut_Attachment.Add(tx_AdjustmentOut_Attachment);
                            CONTEXT.SaveChanges();
                            string keyValue = tx_AdjustmentOut_Attachment.Id.ToString();
                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "add", "Id", keyValue);
                        }

                        CONTEXT_TRANS.Commit();
                    }
                }

            }

            return Id;

        }

        public void Attachment_Delete(AdjustmentOut_AttachmentModel model)
        {

            if (model != null)
            {
                if (model.DetId != 0)
                {
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            try
                            {
                                string keyValue = model.Id.ToString();
                                Tx_AdjustmentOut_Attachment tx_AdjustmentOutAttachment = CONTEXT.Tx_AdjustmentOut_Attachment.Find(model.DetId);

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "delete", "Id", keyValue);

                                if (tx_AdjustmentOutAttachment != null)
                                {
                                    CONTEXT.Tx_AdjustmentOut_Attachment.Remove(tx_AdjustmentOutAttachment);
                                    CONTEXT.SaveChanges();
                                }

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "delete", "Id", keyValue);
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

        public List<AdjustmentOut_ItemModel> GetAdjustmentOut_Items(long id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentOut_Details(id);
            }
        }


        #endregion
    }

    #endregion Services
}
