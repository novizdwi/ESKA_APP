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

namespace Models.Master.Item
{
    #region Models
    public class DeactiveTagModel
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

        public string ScanDeviceId { get; set; }

        public string CancelReason { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }
        
        public string Comments { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }


        public int? ApprovalTemplateId_ { get; set; }

        public string IsEligibleApprove_ { get; set; }

        public List<DeactiveTag_ItemModel> ListDetails_ = new List<DeactiveTag_ItemModel>();

        public DeactiveTag_Detail Details_ { get; set; }
    }

    public class DeactiveTag_ApprovalModel
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


    public class DeactiveTag_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<DeactiveTag_ApprovalModel> insertedRowValues { get; set; }
        public List<DeactiveTag_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class DeactiveTag_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<DeactiveTag_ItemModel> insertedRowValues { get; set; }
        public List<DeactiveTag_ItemModel> modifiedRowValues { get; set; }
    }

    public class DeactiveTag_ItemModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string TagId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

    }
    
    public class DeactiveTagApprovalView___
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string RequestMassage { get; set; }

        public string ApprovalMessages { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<DeactiveTag_ApprovalModel> ApprovalStepList__ = new List<DeactiveTag_ApprovalModel>();

        public DeactiveTag_Approval ApprovalStep__ { get; set; }
    }


    #endregion Models

    #region Services
    public class DeactiveTagService
    {
        public DeactiveTagModel GetNewModel(int userId)
        {
            DeactiveTagModel model = new DeactiveTagModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;

            return model;
        }

        public DeactiveTagModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public DeactiveTagModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method ="")
        {
            DeactiveTagModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_DeactiveTag"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";


                model = CONTEXT.Database.SqlQuery<DeactiveTagModel>(ssql, id).Single();
                
                model.ListDetails_ = this.DeactiveTag_Details(CONTEXT, id);

                if (method != "post")
                {
                    if(model.Status == "Draft")
                    {                        
                        int? approvalId = CONTEXT.Database.SqlQuery<int?>(@"CALL ""SpApproval_CheckNeedApproval""(:p0, 'DeactiveTag', :p1) ", userId, model.Id).FirstOrDefault();
                        model.ApprovalTemplateId_ = approvalId;
                    }

                    if(model.ApprovalStatus == "Waiting")
                    {
                        string getDocNum = @"SELECT 'Y'
                            FROM ""Tx_DeactiveTag"" T0
                            INNER JOIN  ""Tx_DeactiveTag_Approval"" T1 ON T0.""Id"" = T1.""Id"" AND T1.""Status"" = 'Waiting'
                            WHERE T0.""Id"" = :p0 
                            AND T1.""UserId"" = :p1
                        ";
                        model.IsEligibleApprove_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id, userId).FirstOrDefault();
                    }
                }

            }

            return model;
        }

        public List<DeactiveTag_ItemModel> DeactiveTag_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DeactiveTag_Details(CONTEXT, id);
            }

        }

        public List<DeactiveTag_ItemModel> DeactiveTag_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_DeactiveTag_Item"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var DeactiveTag = CONTEXT.Database.SqlQuery<DeactiveTag_ItemModel>(ssql, id).ToList();
            return DeactiveTag;
        }

        public List<DeactiveTag_ApprovalModel> GetDeactiveTag_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDeactiveTag_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<DeactiveTag_ApprovalModel> GetDeactiveTag_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_DeactiveTag_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<DeactiveTag_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public DeactiveTagModel NavFirst(int userId)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public DeactiveTagModel NavPrevious(int userId, long id = 0)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public DeactiveTagModel NavNext(int userId, long id = 0)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public DeactiveTagModel NavLast(int userId)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(DeactiveTagModel model)
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

                            Tx_DeactiveTag tx_DeactiveTag = new Tx_DeactiveTag();
                            CopyProperty.CopyProperties(model, tx_DeactiveTag, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_DeactiveTag.TransType = "DeactiveTag";
                            tx_DeactiveTag.CreatedDate = dtModified;
                            tx_DeactiveTag.CreatedUser = model._UserId;
                            tx_DeactiveTag.ModifiedDate = dtModified;
                            tx_DeactiveTag.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'DeactiveTag','" + dateX + "','') ").SingleOrDefault();
                            tx_DeactiveTag.TransNo = transNo;

                            CONTEXT.Tx_DeactiveTag.Add(tx_DeactiveTag);
                            CONTEXT.SaveChanges();
                            Id = tx_DeactiveTag.Id;

                            String keyValue;
                            keyValue = tx_DeactiveTag.Id.ToString();

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_DeactiveTag", "add", "Id", keyValue);

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

        public void Update(DeactiveTagModel model, string method = "")
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "DeactiveTag", CONTEXT, "before", "DeactiveTag", "update", "Id", keyValue);


                                Tx_DeactiveTag tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                           
                                if (tx_DeactiveTag != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_DeactiveTag, false, exceptColumns);

                                    tx_DeactiveTag.ModifiedDate = dtModified;
                                    tx_DeactiveTag.ModifiedUser = model._UserId;
                                    
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
                                        if (model.Details_.modifiedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.modifiedRowValues)
                                            {
                                                Detail_Update(CONTEXT, detail, model._UserId);
                                            }
                                        }

                                        if (model.Details_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Details_.deletedRowKeys)
                                            {
                                                DeactiveTag_ItemModel detailModel = new DeactiveTag_ItemModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    CONTEXT.SaveChanges();
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "DeactiveTag", CONTEXT, "after", "DeactiveTag", "update", "Id", keyValue);

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

        public void Detail_Update(HANA_APP CONTEXT, DeactiveTag_ItemModel model, int UserId)
        {
            if (model != null)
            {

                Tx_DeactiveTag_Item tx_DeactiveTag_Item = CONTEXT.Tx_DeactiveTag_Item.Find(model.DetId);

                if (tx_DeactiveTag_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, tx_DeactiveTag_Item, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_DeactiveTag_Item.ModifiedDate = dtModified;
                    tx_DeactiveTag_Item.ModifiedUser = UserId;

                    //CONTEXT.SaveChanges();
                }

            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, DeactiveTag_ItemModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_DeactiveTag_Item\"  WHERE \"DetId\"=:p0", model.DetId);
                    CONTEXT.SaveChanges();

                }
            }

        }

        public bool ChooseItem(int UserId, long Id, string[] data)
        {
            if (data != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            String keyValue;
                            keyValue = Id.ToString();
                            SpNotif.SpSysControllerTransNotif(UserId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", "chooseItem", "Id", keyValue);

                            string sqlWhere;
                            if (data == null)
                            {
                                sqlWhere = "";
                            }
                            else if (data.Length == 0)
                            {
                                sqlWhere = "";
                            }
                            else
                            {
                                for (var i = 0; i < data.Length; i++)
                                {
                                    data[i] = "'" + data[i].Replace("'", "''") + "'";
                                }

                                sqlWhere = string.Join(",", data);
                            }

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpDeactiveTag_ChooseItem\"(:p0,:p1,:p2)", UserId, Id, sqlWhere);

                            keyValue = Id.ToString();
                            SpNotif.SpSysControllerTransNotif(UserId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", "chooseItem", "Id", keyValue);

                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMessage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMessage = ex.Message;
                            }
                            else
                            {
                                errorMessage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMessage);
                        }
                    }
                }


            }

            return true;

        }

        public void Post(int userId, long id)
        {
            try
            {

                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        Tx_DeactiveTag tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(id);
                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", "post", "Id", keyValue);
                        if (tx_DeactiveTag != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_DeactiveTag.PostingDate = dtModified;
                            tx_DeactiveTag.Status = "Posted";

                            tx_DeactiveTag.ModifiedDate = dtModified;
                            tx_DeactiveTag.ModifiedUser = userId;

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpDeactiveTag_DeactiveItemTag\"(:p0,:p1)", userId, id);
                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", "post", "Id", keyValue);                        
                        CONTEXT_TRANS.Commit();

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
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

                        Tx_DeactiveTag tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", "cancel", "Id", keyValue);
                        if (tx_DeactiveTag != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_DeactiveTag.Status = "Cancel";
                            tx_DeactiveTag.CancelReason = cancelReason;
                            tx_DeactiveTag.ModifiedDate = dtModified;
                            tx_DeactiveTag.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", "cancel", "Id", keyValue);


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
               DeactiveTagModel deactiveTagModel = GetById(userId, id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", "requestApproval", "Id", keyValue);

                        Tx_DeactiveTag tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(id);
                        if (tx_DeactiveTag != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_DeactiveTag.IsApproval = "Y";
                            tx_DeactiveTag.ApprovalMessages = approvalMessages;
                            tx_DeactiveTag.ApprovalStatus = "Waiting";
                            tx_DeactiveTag.ModifiedDate = dtModified;
                            tx_DeactiveTag.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Insert\"(:p0,'DeactiveTag',:p1, :p2)", userId, id, templateId);
                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", "requestApproval", "Id", keyValue);
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
                    //DeactiveTagModel deactiveTagModel = GetById(userId, id);
                    //this.Update(deactiveTagModel, "Post");
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpDeactiveTag_UpdateItem\"(:p0,:p1)", userId, id);
                            CONTEXT.SaveChanges();
                        }
                    }

                    //this.PostSAP(userId, id);
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

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", action.ToLower(), "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpApproval_Authorize\"(:p0, 'DeactiveTag', :p2, :p3, :p4)", userId, id, action, approvalMessage);
                        CONTEXT.SaveChanges();
                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", action.ToLower(), "Id", keyValue);

                        CONTEXT_TRANS.Commit();
                        string strApprovalStatus = @"
                            SELECT T0.""ApprovalStatus"" 
                            FROM ""Tx_DeactiveTag"" T0
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

        public DeactiveTagApprovalView___ GetViewApproval(long id)
        {
            DeactiveTagApprovalView___ model = new DeactiveTagApprovalView___();
            using (var CONTEXT = new HANA_APP())
            {
                string sql = @"
                    SELECT TOP 1 T0.""Id"", T0.""Status"", T0.""ApprovalMessages"", T1.""CreatedDate"", T2.""FirstName""
                    FROM ""Tx_DeactiveTag"" T0 
                    LEFT JOIN ""Tx_DeactiveTag_Approval"" T1 ON T0.""Id"" = T1.""Id"" 
                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                    WHERE T0.""Id""=:p0 
                ";

                model = CONTEXT.Database.SqlQuery<DeactiveTagApprovalView___>(sql, id).FirstOrDefault();

                model.ApprovalStepList__ = GetDeactiveTag_ApprovalSteps(CONTEXT, id);

            }

            return model;
        } 

        public List<DeactiveTag_ItemModel> GetDeactiveTag_Items(long id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DeactiveTag_Details(id);
            }
        }

    }

    #endregion Services
}
