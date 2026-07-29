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
        public string Batch { get; set; }

        [Required(ErrorMessage = "required")]
        public Decimal? Quantity { get; set; }
        public Decimal? QuantityPlanned { get; set; }
        public Decimal? QuantityActual { get; set; }
        public Decimal? QuantityRemain { get; set; }
        public string Comments { get; set; }

        public List<ProductionTask_Detail_Item> ListItem_ = new List<ProductionTask_Detail_Item>();
        public ProductionTask_Detail_Item Items_ { get; set; }
    }

    public class ProductionTask_Detail_Item
    { 
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
                    WHERE T1.""DetId"" = :p0
                ";
                ProductionActivityFinishModel model = CONTEXT.Database.SqlQuery<ProductionActivityFinishModel>(SqlSelect, detId).SingleOrDefault();
                model.ListItem_ = this.GetProductionTaskDetailItems(detId);

                return model;
            }
        }

        public List<ProductionTask_Detail_Item> GetProductionTaskDetailItems(long? detId)
        {
            List<ProductionTask_Detail_Item> ret = new List<ProductionTask_Detail_Item>();
            using (var CONTEXT = new HANA_APP())
            {
                string SqlSelect = @"
                    SELECT *
                    FROM ""Tx_ProductionTask_Activity_Item"" T0
                    WHERE T0.""DetId"" = :p0
                ";

                ret = CONTEXT.Database.SqlQuery<ProductionTask_Detail_Item>(SqlSelect, detId).ToList();
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

    }

    #endregion

}