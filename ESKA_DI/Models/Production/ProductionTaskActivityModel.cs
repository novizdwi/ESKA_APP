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

namespace Models.Production
{

    #region Models

    public class ProductionTaskActivityModel
    {
        public int UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string TransNo { get; set; }
        
        public string ItemCode { get; set; }
        
        public string ItemName { get; set; }
        
        public string DocNum { get; set; }
        
        public string RoutingName { get; set; }
        
        public decimal? QuantityPlanned { get; set; }
        
        public decimal? QuantityActual { get; set; }
        
        public decimal? QuantityRemain { get; set; }

        public long? EstimatedHours { get; set; }

        public long? ActualHours { get; set; }

        public DateTime? ActivityDate { get; set; }

        public string ActivityStatus { get; set; }

    }

    public class ProductionActivityPauseModel
    {
        public long? Id { get; set; }

        public long? DetId { get; set; }
        
        [Required(ErrorMessage = "required")]
        public string PauseReason { get; set; }
        
        public string PauseComments { get; set; }
    }

    public class ProductionActivityFinishModel
    {
        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string FinishTransNo { get; set; }
    
        public string FinishDocNum { get; set; }

        public string FinsihItemCode { get; set; }

        public string FinishItemName { get; set; }

        [Required(ErrorMessage = "required")]
        public string FinishBatch { get; set; }

        [Required(ErrorMessage = "required")]
        public Decimal? Quantity { get; set; }
        public Decimal? QuantityPlanned { get; set; }
        public Decimal? QuantityActual { get; set; }
        public Decimal? QuantityRemain { get; set; }
        public string Comments { get; set; }

        public List<ProductionTaskDetailItemModel> ListItem_ = new List<ProductionTaskDetailItemModel>();
        public ProductionTaskDetailItemModel Items_ { get; set; }
    }

    public class ProductionTaskDetailItemModel
    {
        public int _UserId { get; set; }
        public long? Id { get; set; }
        public long? DetId { get; set; }
        public long? DetDetId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string Direction { get; set; }
        public string Uom { get; set; }
        public string Batch { get; set; }
        public Decimal? QuantityPlanned { get; set; }
        public string QuantityActual { get; set; }
        public string Comments { get; set; }
    
    }

    #endregion

    #region Services

    public class ProductionActivityService
    {
        private static string SqlSelect = @"
        SELECT
	        T0.""Id"",
	        T1.""DetId"",
	        T0.""TransNo"",
	        T0.""ItemCode"",
	        T0.""ItemName"",
	        T0.""DocNum"",
	        T2.""RoutingName"",
	        T0.""QuantityPlanned"",
	        T0.""QuantityActual"",
	        COALESCE(T0.""QuantityPlanned"", 0 ) - COALESCE(T0.""QuantityActual"", 0) AS ""QuantityRemain"",
	        T0.""EstimatedHours"",
	        T0.""ActualHours"",
	        T0.""CreatedDate"" AS ""ActivityDate"",
            T1.""Status"" AS ""ActivityStatus""
        FROM ""Tx_ProductionTask"" T0
        INNER JOIN ""Tx_ProductionTask_Activity"" T1 ON T0.""Id"" = T1.""Id""
        INNER JOIN ""Tx_ProcessCard_Detail"" T2 ON T0.""BaseId"" = T2.""Id"" AND T0.""BaseDetId"" = T2.""DetId""
        WHERE T1.""Status"" NOT IN ('Finished')
        AND T0.""Id"" = :p0
        ";

        public void ProductionTaskActivity_DeleteItemBatch(int userId, long id, long detId, long detDetId)
        {
            throw new NotImplementedException();
        }

        public ProductionTaskActivityModel GetNewModel(int userId, long id)
        {
            ProductionTaskActivityModel model = this.GetById(userId, id);

            model.UserId = userId;
            return model;
        }

        public void PauseActivity(int userId, ProductionActivityPauseModel model)
        {
            SetStatus(userId, model, "pause");
        }

        public void StartActivity(int userId, ProductionActivityPauseModel model)
        {
            SetStatus(userId, model, "start");
        }

        public void SetStatus(int userId, ProductionActivityPauseModel model, string status)
        {
            string activityStatus = status == "pause" ? "Paused" : "OnProgress";
            string detailType = status == "pause" ? "Paused" : "Production";
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", status, "Id", model.DetId.ToString());

                            DateTime dtModified = DateTime.Now;
                            Tx_ProductionTask_Activity tx_ProductionTask_Activity = CONTEXT.Tx_ProductionTask_Activity.FirstOrDefault(x => x.DetId == model.DetId);
                            if (tx_ProductionTask_Activity != null)
                            {
                                tx_ProductionTask_Activity.Status = activityStatus;
                                tx_ProductionTask_Activity.ModifiedDate = dtModified;
                                tx_ProductionTask_Activity.ModifiedUser = userId;

                                var lastDetail = CONTEXT.Tx_ProductionTask_Activity_Log
                                    .Where(x => x.DetId == model.DetId)
                                    .OrderByDescending(x => x.DetDetId)
                                    .FirstOrDefault();
                                if (lastDetail != null)
                                {
                                    lastDetail.EndTime = dtModified;
                                    lastDetail.ModifiedDate = dtModified;
                                    lastDetail.ModifiedUser = userId;
                                }

                                Tx_ProductionTask_Activity_Log tx_ProductionTask_Activity_log = new Tx_ProductionTask_Activity_Log
                                {
                                    Id = model.Id,
                                    DetId = model.DetId,
                                    DetailType = detailType,
                                    PauseType = model.PauseReason,
                                    Comments = model.PauseComments,
                                    StartTime = dtModified,
                                    CreatedDate = dtModified,
                                    CreatedUser = userId,
                                    ModifiedDate = dtModified,
                                    ModifiedUser = userId
                                };
                                CONTEXT.Tx_ProductionTask_Activity_Log.Add(tx_ProductionTask_Activity_log);

                            }

                            CONTEXT.SaveChanges();
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", status, "Id", model.DetId.ToString());
                            CONTEXT_TRANS.Commit();
                        }
                        catch
                        {
                            CONTEXT_TRANS.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public ProductionTaskActivityModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }
        public ProductionActivityFinishModel GetFinishModel(long id = 0 , long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                string SqlSelect = @"
                    SELECT
	                    T0.""Id"",
                        T1.""DetId"",
                        T2.""SerialNumber"" AS ""FinishBatch"",
	                    T0.""TransNo"" AS ""FinishTransNo"",
	                    T0.""DocEntry"",
	                    T0.""DocNum"" AS ""FinishDocNum"",
	                    T0.""ItemCode"" AS ""FinsihItemCode"",
	                    T0.""ItemName"" AS ""FinishItemName"",
	                    T0.""QuantityPlanned"",
	                    T0.""QuantityActual"",
	                    COALESCE(T0.""QuantityPlanned"", 0) - COALESCE(T0.""QuantityActual"", 0) AS ""QuantityRemain""
                    FROM ""Tx_ProductionTask"" T0
                    INNER JOIN  ""Tx_ProductionTask_Activity"" T1 ON T0.""Id"" = T1.""Id""
                    INNER JOIN ""Tx_ProcessCard"" T2 ON  T0.""BaseId"" = T2.""Id""
                    WHERE T1.""DetId"" = :p0
                ";
                ProductionActivityFinishModel model = CONTEXT.Database.SqlQuery<ProductionActivityFinishModel>(SqlSelect, detId).SingleOrDefault();
                model.ListItem_ = this.GetProductionTaskDetailItems(detId);

                return model;
            }
        }

        public List<ProductionTaskDetailItemModel> GetProductionTaskDetailItems(long? detId)
        {
            List<ProductionTaskDetailItemModel> ret = new List<ProductionTaskDetailItemModel>();
            using (var CONTEXT = new HANA_APP())
            {
                string sqls = @"
                    SELECT *
                    FROM ""Tx_ProductionTask_Activity_Item"" T0
                    WHERE T0.""DetId"" = :p0
                ";

                ret = CONTEXT.Database.SqlQuery<ProductionTaskDetailItemModel>(sqls, detId).ToList();
            }

            return ret;
        }

        public ProductionActivityPauseModel GetPauseModel(long id, long detId)
        {
            ProductionActivityPauseModel model = new ProductionActivityPauseModel()
            {
                Id = id,
                DetId = detId
            };

            return model; 
        }
        public void FinishActivity(int userId, ProductionActivityFinishModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "finish", "Id", model.DetId.ToString() );

                            DateTime dtModified = DateTime.Now;
                            Tx_ProductionTask_Activity tx_ProductionTask_Activity = CONTEXT.Tx_ProductionTask_Activity.FirstOrDefault(x => x.DetId == model.DetId);
                            if (tx_ProductionTask_Activity != null)
                            {
                                tx_ProductionTask_Activity.Status = "Finished";
                                tx_ProductionTask_Activity.Quantity = model.Quantity;
                                tx_ProductionTask_Activity.ModifiedDate = dtModified;
                                tx_ProductionTask_Activity.ModifiedUser = userId;

                                var lastDetail = CONTEXT.Tx_ProductionTask_Activity_Log
                                    .Where(x => x.DetId == model.DetId)
                                    .OrderByDescending(x => x.DetDetId)
                                    .FirstOrDefault();
                                if (lastDetail != null)
                                {
                                    lastDetail.EndTime = dtModified;
                                    lastDetail.ModifiedDate = dtModified;
                                    lastDetail.ModifiedUser = userId;
                                }

                                Tx_ProductionTask_Activity_Log tx_ProductionTask_Activity_log = new Tx_ProductionTask_Activity_Log
                                {
                                    Id = model.Id,
                                    DetId = model.DetId,  
                                    DetailType = "Finish",
                                    Comments = model.Comments,
                                    StartTime = dtModified,
                                    CreatedDate = dtModified,
                                    CreatedUser = userId,
                                    ModifiedDate = dtModified,
                                    ModifiedUser = userId
                                };
                                CONTEXT.Tx_ProductionTask_Activity_Log.Add(tx_ProductionTask_Activity_log);
                            
                            }

                            CONTEXT.SaveChanges();
                            SpNotif.SpSysControllerTransNotif(userId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "finish", "Id", model.DetId.ToString());
                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProductionTaskActivity_UpdateTask\"(:p0,:p1)", userId, model.DetId);
                            CONTEXT_TRANS.Commit();
                        }
                        catch
                        {
                            CONTEXT_TRANS.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public ProductionTaskActivityModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method = "")
        {
            long currentTaskId = id;
            if (currentTaskId == 0)
            {
                string ssql = @" SELECT ""Id"" AS ""CurrentTaskId""
                    FROM ""Tx_ProductionTask"" 
                    WHERE ""Status"" = 'Open'
                    AND ""IsRunningTask"" = 'Y' 
                    AND  ""OperatorId"" = :p0 
                ";
                currentTaskId = CONTEXT.Database.SqlQuery<long>(ssql, userId).FirstOrDefault();
                
            }

            ProductionTaskActivityModel model = new ProductionTaskActivityModel();

            if (currentTaskId != 0)
            {
                model = CONTEXT.Database.SqlQuery<ProductionTaskActivityModel>(SqlSelect, currentTaskId).SingleOrDefault();
            }
            if (model == null)
            {
                model = new ProductionTaskActivityModel { Id = 0 };
            }

            return model;
        }

        public long ProductionTaskActivity_AddNewItem(ProductionTaskDetailItemModel model)
        {
            long detDetId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        Tx_ProductionTask_Activity_Item tx_ProductionTask_Activity_Item = new Tx_ProductionTask_Activity_Item();
                        CopyProperty.CopyProperties(model, tx_ProductionTask_Activity_Item, false);

                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        tx_ProductionTask_Activity_Item.CreatedDate = dtModified;
                        tx_ProductionTask_Activity_Item.CreatedUser = model._UserId;
                        tx_ProductionTask_Activity_Item.ModifiedDate = dtModified;
                        tx_ProductionTask_Activity_Item.ModifiedUser = model._UserId;

                        CONTEXT.Tx_ProductionTask_Activity_Item.Add(tx_ProductionTask_Activity_Item);
                        CONTEXT.SaveChanges();
                        detDetId = tx_ProductionTask_Activity_Item.DetDetId;

                        String keyValue;
                        keyValue = tx_ProductionTask_Activity_Item.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "addItem", "Id", keyValue);

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

            return detDetId;
        }

        public void ProductionTaskActivity_UpdateItem(ProductionTaskDetailItemModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = model.Id.ToString();

                        SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "updateItem", "Id", keyValue);

                        Tx_ProductionTask_Activity_Item tx_ProductionTask_Activity_Item = CONTEXT.Tx_ProductionTask_Activity_Item.Find(model.DetDetId);
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                        if (tx_ProductionTask_Activity_Item != null)
                        {
                            var exceptColumns = new string[] { "Id", "DetId", "DetDetId", "CreatedUser", "CreatedDate" };
                            CopyProperty.CopyProperties(model, tx_ProductionTask_Activity_Item, false, exceptColumns);

                            tx_ProductionTask_Activity_Item.ModifiedDate = dtModified;
                            tx_ProductionTask_Activity_Item.ModifiedUser = model._UserId;

                            CONTEXT.SaveChanges();
                            //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProductionTaskActivity_UpdateItemQuantity\"(:p0, 'Tx_ProductionTaskActivity_Item_Batch',:p1, :p2)", model._UserId, model.DetId, 0);
                            SpNotif.SpSysControllerTransNotif(model._UserId, "ProductionTaskActivity", CONTEXT, "after", "ProductionTaskActivity", "updateItem", "Id", keyValue);

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

        public void ProductionTaskActivity_DeleteItem(int _userId, long Id, long DetId, long DetDetId) 
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    if (DetDetId != 0)
                    {
                        try
                        {
                            SpNotif.SpSysControllerTransNotif(_userId, "ProductionTaskActivity", CONTEXT, "before", "ProductionTaskActivity", "deleteItem", "Id", Id.ToString());

                            //CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ProductionTaskActivity_Item_Batch_Scale\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ProductionTask_Activity_Item\"  WHERE \"DetDetId\"=:p0", DetDetId);
                            CONTEXT.SaveChanges();

                            //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpProductionTaskActivity_UpdateItemQuantity\"(:p0, 'Tx_ProductionTaskActivity_Item_Batch',:p1, :p2)", _userId, DetId, 0);
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

    #endregion

}