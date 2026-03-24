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

namespace Models.Master.Item
{
    #region Models

    public class ChangeItemModel
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

        [Required(ErrorMessage = "required")]
        public string WhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string OriginItemCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string OriginItemName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToItemCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToItemName { get; set; } 

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; } 

        public List<ChangeItem_DetailModel> ListDetails_ = new List<ChangeItem_DetailModel>();

        public ChangeItem_Detail Details_ { get; set; }
    }
    public class ChangeItem_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<ChangeItem_DetailModel> insertedRowValues { get; set; }
        public List<ChangeItem_DetailModel> modifiedRowValues { get; set; }
    }
    public class ChangeItem_DetailModel
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

        public string TagId { get; set; }
               
        public string Comments { get; set; }
    }
    #endregion

    #region Services

    public class ChangeItemService
    {

        public ChangeItemModel GetNewModel(int userId)
        {
            ChangeItemModel model = new ChangeItemModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;

            return model;
        }
        public ChangeItemModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public ChangeItemModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ChangeItemModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_ChangeItem"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<ChangeItemModel>(ssql, id).Single();

                model.ListDetails_ = this.ChangeItem_Details(CONTEXT, id);
            }

            return model;
        }
        public List<ChangeItem_DetailModel> ChangeItem_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ChangeItem_Details(CONTEXT, id);
            }

        }

        public List<ChangeItem_DetailModel> ChangeItem_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.* 
                FROM ""Tx_ChangeItem_Tag"" T0
                WHERE ""Id"" =:p0
                ORDER BY ""DetId"" ASC
            ";
            var items = CONTEXT.Database.SqlQuery<ChangeItem_DetailModel>(ssql, id).ToList();
            return items;
        }

        public ChangeItemModel NavFirst(int userId)
        {
            ChangeItemModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ChangeItem");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ChangeItem\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public ChangeItemModel NavPrevious(int userId, long id = 0)
        {
            ChangeItemModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ChangeItem");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ChangeItem\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public ChangeItemModel NavNext(int userId, long id = 0)
        {
            ChangeItemModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ChangeItem");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ChangeItem\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public ChangeItemModel NavLast(int userId)
        {
            ChangeItemModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ChangeItem");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ChangeItem\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public long Add(ChangeItemModel model)
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

                            Tx_ChangeItem Tx_ChangeItem = new Tx_ChangeItem();
                            CopyProperty.CopyProperties(model, Tx_ChangeItem, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_ChangeItem.TransType = "ChangeItem";
                            Tx_ChangeItem.CreatedDate = dtModified;
                            Tx_ChangeItem.CreatedUser = model._UserId;
                            Tx_ChangeItem.ModifiedDate = dtModified;
                            Tx_ChangeItem.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'ChangeItem','" + dateX + "','') ").SingleOrDefault();
                            Tx_ChangeItem.TransNo = transNo;

                            CONTEXT.Tx_ChangeItem.Add(Tx_ChangeItem);
                            CONTEXT.SaveChanges();
                            Id = Tx_ChangeItem.Id;

                            String keyValue;
                            keyValue = Tx_ChangeItem.Id.ToString();

                            if (model.Details_ != null)
                            {
                                if (model.Details_.insertedRowValues != null)
                                {
                                    foreach (var detail in model.Details_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, detail, Convert.ToInt64(keyValue), model._UserId);
                                    }
                                }
                            }

                            //    CONTEXT.Database.ExecuteSqlCommand(
                            //    "CALL \"SpGoodsReceiptPO_AddItemDetail\" ({0}, {1}, {2})",
                            //    model._UserId,
                            //    keyValue
                            //);
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "ChangeItem", CONTEXT, "after", "ChangeItem", "add", "Id", keyValue);

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

        public void Update(ChangeItemModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "ChangeItem", CONTEXT, "before", "ChangeItem", "update", "Id", keyValue);

                                Tx_ChangeItem Tx_ChangeItem = CONTEXT.Tx_ChangeItem.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_ChangeItem.ModifiedDate = dtModified;
                                Tx_ChangeItem.ModifiedUser = model._UserId;

                                if (Tx_ChangeItem != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_ChangeItem, false, exceptColumns);
                                    Tx_ChangeItem.ModifiedDate = dtModified;
                                    Tx_ChangeItem.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                    if (model.Details_ != null)
                                    {
                                        if (model.Details_.insertedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.insertedRowValues)
                                            {
                                                Detail_Add(CONTEXT, detail, model.Id, model._UserId);
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
                                                ChangeItem_DetailModel detailModel = new ChangeItem_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "ChangeItem", CONTEXT, "after", "ChangeItem", "update", "Id", keyValue);
                                    
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

        public long Detail_Add(HANA_APP CONTEXT, ChangeItem_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_ChangeItem_Tag Tx_ChangeItem_Tag = new Tx_ChangeItem_Tag();

                CopyProperty.CopyProperties(model, Tx_ChangeItem_Tag, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_ChangeItem_Tag.Id = Id;
                Tx_ChangeItem_Tag.CreatedDate = dtModified;
                Tx_ChangeItem_Tag.CreatedUser = UserId;
                Tx_ChangeItem_Tag.ModifiedDate = dtModified;
                Tx_ChangeItem_Tag.ModifiedUser = UserId;               


                CONTEXT.Tx_ChangeItem_Tag.Add(Tx_ChangeItem_Tag);
                CONTEXT.SaveChanges();
                DetId = Tx_ChangeItem_Tag.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, ChangeItem_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_ChangeItem_Tag Tx_ChangeItem_Tag = CONTEXT.Tx_ChangeItem_Tag.Find(model.DetId);

                if (Tx_ChangeItem_Tag != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_ChangeItem_Tag, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_ChangeItem_Tag.ModifiedDate = dtModified;
                    Tx_ChangeItem_Tag.ModifiedUser = UserId;
                   
                    CONTEXT.SaveChanges();

                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, ChangeItem_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ChangeItem_Tag\"  WHERE \"DetId\"=:p0", model.DetId);

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
                            SpNotif.SpSysControllerTransNotif(UserId, "ChangeItem", CONTEXT, "before", "Tx_ChangeItem", "chooseItem", "Id", keyValue);

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

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpChangeItem_ChooseItem\"(:p0,:p1,:p2)", UserId, Id, sqlWhere);

                            keyValue = Id.ToString();
                            SpNotif.SpSysControllerTransNotif(UserId, "ChangeItem", CONTEXT, "after", "Tx_ChangeItem", "chooseItem", "Id", keyValue);

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

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = id.ToString();

                        Tx_ChangeItem tx_ChangeItem = CONTEXT.Tx_ChangeItem.Find(id);
                        SpNotif.SpSysControllerTransNotif(userId, "ChangeItem", CONTEXT, "before", "ChangeItem", "post", "Id", keyValue); 

                        if (tx_ChangeItem != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_ChangeItem.Status = "Posted";
                            tx_ChangeItem.IsAfterPosted = "Y";
                            tx_ChangeItem.ModifiedDate = dtModified;
                            tx_ChangeItem.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "ChangeItem", CONTEXT, "after", "Tx_ChangeItem", "post", "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpChangeItem_UpdateItem\"(:p0,:p1)", userId, id);

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

                        Tx_ChangeItem tx_ChangeItem = CONTEXT.Tx_ChangeItem.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "ChangeItem", CONTEXT, "before", "Tx_ChangeItem", "cancel", "Id", keyValue);
                        if (tx_ChangeItem != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_ChangeItem.Status = "Cancel";
                            tx_ChangeItem.CancelReason = cancelReason;
                            tx_ChangeItem.ModifiedDate = dtModified;
                            tx_ChangeItem.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "ChangeItem", CONTEXT, "after", "Tx_ChangeItem", "cancel", "Id", keyValue);


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