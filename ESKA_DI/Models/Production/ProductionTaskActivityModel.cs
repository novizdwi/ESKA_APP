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
        public string PauseReason { get; set; }
        public string PauseComments { get; set; }
    }

    public class ProductionActivityFinishModel
    {
        public long? Id { get; set; }
        public long? DetId { get; set; }

        [Required(ErrorMessage = "required")]
        public Decimal? Quantity { get; set; }
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

        public void PauseActivity(int userId, int id)
        {
            SetStatus(userId, id, "Start");

        }

        public void StartActivity(int userId, int id)
        {
            SetStatus(userId, id, "Paused");
        }

        public ProductionTaskActivityModel GetNewModel(int userId, long id)
        {
            ProductionTaskActivityModel model = this.GetById(userId, id); 

            model.UserId = userId; 
            return model;
        }

        public void SetStatus(int userId, long id, string status)
        {
            throw new NotImplementedException();

            //using (var CONTEXT = new HANA_APP())
            //{
            //    string ssql = @"UPDATE ""Tx_ProductionTask_Activity"" SET ""Status"" = :p1 WHERE ""Id"" = :p0";
            //    CONTEXT.Database.ExecuteSqlCommand(ssql, id, status);
            //}
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
            ProductionActivityFinishModel model = new ProductionActivityFinishModel() { 
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

                                var lastDetail = CONTEXT.Tx_ProductionTask_Activity_Detail
                                    .Where(x => x.DetId == model.DetId)
                                    .OrderByDescending(x => x.DetDetId)
                                    .FirstOrDefault();
                                if (lastDetail != null)
                                {
                                    lastDetail.EndTime = dtModified;
                                    lastDetail.ModifiedDate = dtModified;
                                    lastDetail.ModifiedUser = userId;
                                }

                                Tx_ProductionTask_Activity_Detail tx_ProductionTask_Activity_Detail = new Tx_ProductionTask_Activity_Detail
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
                                CONTEXT.Tx_ProductionTask_Activity_Detail.Add(tx_ProductionTask_Activity_Detail);
                            
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