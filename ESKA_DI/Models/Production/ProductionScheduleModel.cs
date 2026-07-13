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

    public class ProductionScheduleModel
    {
        public int UserId { get; set; }

        public int? Id { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<ProductionSchedule_ReferenceModel> ListReferences_ = new List<ProductionSchedule_ReferenceModel>();

        public ProductionSchedule_Detail Details_ = new ProductionSchedule_Detail();
    }


    public class ProductionSchedule_ReferenceModel
    {

        public int Id { get; set; }

        public int? VisOrder { get; set; }

        public string TransNo { get; set; }

        public string SerialNumber { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }
        
        public DateTime? StartDate { get; set; }

        public decimal? Quantity { get; set; }

        public string Status { get; set; }

        public string ProductionStatus { get; set; }

        List<ProductionScheduleDetailModel> ListDetails_ = new List<ProductionScheduleDetailModel>();
    }
     
    public class ProductionSchedule_Detail
    {
        public int? UserId { get; set; }
        public List<long> deletedRowKeys { get; set; }
        public List<ProductionSchedule_ReferenceModel> insertedRowValues { get; set; }
        public List<ProductionSchedule_ReferenceModel> modifiedRowValues { get; set; }
    }

    public class ProductionScheduleDetailModel
    {

        public int? Id {get; set;}
        
        public int? DetId { get; set; }
        
        public int? DocEntry { get; set; }
        
        public string DocNum { get; set; }
        
        public string ItemCode { get; set; }
        
        public string ItemName { get; set; }

        public string Uom { get; set; }

        public int? ProductionTaskId { get; set; }

        public string ProductionTaskTransNo { get; set; }

        public int? Sort { get; set; }
        
        public string RoutingName { get; set; }
        
        public string OperatorName { get; set; }
        
        public DateTime? ProcessingDate { get; set; }
        
        public TimeSpan? Clock { get; set; }
        
        public TimeSpan? PracticeHours { get; set; }
        
        public int MachineNo { get; set; }

        public string ProductionStatus { get; set; }

        public decimal? PlannedQty { get; set; }

        public decimal? Quantity { get; set; }

    }
    public class ProductionTaskGenerateModel
    {
        public int? Sort { get; set; }
		public long? BaseId { get; set; }
        public long BaseDetId { get; set; }
        public int? DocEntry { get; set; }
        public string DocNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime? PlannedDate { get; set; }
        public decimal? QuantityPlanned { get; set; } 
        public string Uom { get; set; } 
    }

    #endregion

    #region Services

    public class ProductionScheduleService
    {

        public ProductionScheduleModel GetNewModel(int userId)
        {
            ProductionScheduleModel model = new ProductionScheduleModel();
            model.UserId = userId; 
            model.ListReferences_ = ProductionSchedule_GetReferences(userId);

            return model;
        }

        public ProductionScheduleModel Find(int userId)
        {
            ProductionScheduleModel model = new ProductionScheduleModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionSchedule_GetReferences(userId);
            return model;
        }


        //-------------------------------------
        //Detail  ProductionSchedule_Reference
        //-------------------------------------
        public ProductionScheduleModel GetListByParam(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            ProductionScheduleModel model = new ProductionScheduleModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionSchedule_GetReferences(userId);

            return model;
        }

        //-------------------------------------
        //Detail  ProductionSchedule_Reference
        //-------------------------------------
        public List<ProductionSchedule_ReferenceModel> ProductionSchedule_GetReferences(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProductionSchedule_GetReferences(CONTEXT, userId);
            }
        }

        public List<ProductionSchedule_ReferenceModel> ProductionSchedule_GetReferences(HANA_APP CONTEXT, int userId)
        {
            string sql = @"
            CALL ""SpProductionSchedule_GetReferences"" (
                :p0 --userId
            )";
            return CONTEXT.Database.SqlQuery<ProductionSchedule_ReferenceModel>(sql, userId).ToList();
        }

        public List<ProductionScheduleDetailModel> ProductionSchedule_TabReferenceDetails(long id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProductionSchedule_TabReferenceDetails(CONTEXT, id);
            }
        }

        public List<ProductionScheduleDetailModel> ProductionSchedule_TabReferenceDetails(HANA_APP CONTEXT, long id)
        {
            string ssql = @"
                SELECT T0.*,
                    T1.""ItemCode"",
                    T1.""ProdName"" AS ""ItemName"",
                    T1.""PlannedQty"" AS ""PlannedQty"",
                    T1.""Uom"" AS ""Uom"",
                    T2.""Id"" AS ""ProductionTaskId"",
                    T2.""TransNo"" AS ""ProductionTaskTransNo"",
                    T2.""Status"" AS ""ProductionStatus""
                    FROM ""Tx_ProcessCard_Detail"" T0
                INNER JOIN """ + DbProvider.dbSap_Name + @""".""OWOR"" T1 ON T0.""Id"" = T1.""U_IDU_WebId"" AND T0.""Sort"" = T1.""U_IDU_RoutingLevel""
                LEFT JOIN ""Tx_ProductionTask"" T2 ON T0.""Id"" = T2.""BaseId"" AND T0.""DetId"" = T2.""BaseDetId"" 
                WHERE T0.""Id"" = :p0
                ORDER BY T0.""Sort"" ASC
            ";
            var detailModel = CONTEXT.Database.SqlQuery<ProductionScheduleDetailModel>(ssql, id).ToList();
            return detailModel;
        }


        public void Update(ProductionSchedule_Detail model)
        {            
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {  
                            if(model.modifiedRowValues != null )
                            {
                                SpNotif.SpSysControllerTransNotif((int)model.UserId, "ProductionSchedule", CONTEXT, "before", "ProductionSchedule", "update", "Id", "0");
                                if (model.modifiedRowValues.Count > 0)
                                {
                                    foreach (var detail in model.modifiedRowValues)
                                    {
                                        UpdateDetail(CONTEXT, (int)model.UserId, detail);

                                    }
                                    CONTEXT.SaveChanges();
                                }
                                SpNotif.SpSysControllerTransNotif((int)model.UserId, "ProductionSchedule", CONTEXT, "after", "ProductionSchedule", "update", "Id", "0");
                                CONTEXT_TRANS.Commit();
                            } 
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

        private void UpdateDetail(HANA_APP CONTEXT, int userId, ProductionSchedule_ReferenceModel model)
        {
            if (model != null)
            {
                Tx_ProcessCard tx_ProcessCard = CONTEXT.Tx_ProcessCard.Find(model.Id);
                if (tx_ProcessCard != null)
                {
                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_ProcessCard.ProductionStatus = model.ProductionStatus;
                    tx_ProcessCard.VisOrder = model.VisOrder;

                    tx_ProcessCard.ModifiedDate = dtModified;
                    tx_ProcessCard.ModifiedUser = userId;
                    if(model.ProductionStatus == "Released" && tx_ProcessCard.IsCreatedActivity != "Y" )
                    {
                        tx_ProcessCard.IsCreatedActivity = "Y";
                        string sql = @" CALL ""SpProductionSchedule_GenerateProductionTask"" (:p0, :p1)";
                        List<ProductionTaskGenerateModel> productionActivities = CONTEXT.Database.SqlQuery<ProductionTaskGenerateModel>(sql, userId, model.Id).ToList();
                        if(productionActivities != null)
                        {
                            if(productionActivities.Count != 0)
                            {
                                foreach(var activites in productionActivities)
                                {
                                    Tx_ProductionTask tx_ProductionTask = new Tx_ProductionTask();
                                    CopyProperty.CopyProperties(activites, tx_ProductionTask, false);

                                    tx_ProductionTask.TransType = "ProductionTask";

                                    string dateX = DateTime.Now.ToString("yyyy-MM-dd");
                                    string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + userId + ",'ProductionTask','" + dateX + "','') ").SingleOrDefault();
                                    tx_ProductionTask.TransNo = transNo;
                                    tx_ProductionTask.Status = "Open";
                                    tx_ProductionTask.IsRunningTask = "N";
                                    tx_ProductionTask.Uom = activites.Uom;

                                    tx_ProductionTask.CreatedDate = dtModified;
                                    tx_ProductionTask.CreatedUser = userId;
                                    tx_ProductionTask.ModifiedDate = dtModified;
                                    tx_ProductionTask.ModifiedUser = userId;

                                    CONTEXT.Tx_ProductionTask.Add(tx_ProductionTask);
                                    CONTEXT.SaveChanges();

                                }
                            }
                        }

                    }

                }
            }
        }
    }

    #endregion

}