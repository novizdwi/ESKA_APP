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

    public class ProductionTaskModel
    {
        public int UserId { get; set; }

        public int? Id { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<ProductionTask_ReferenceModel> ListReferences_ = new List<ProductionTask_ReferenceModel>();

        public ProductionTask_Detail Details_ = new ProductionTask_Detail();
    }


    public class ProductionTask_ReferenceModel
    {

        public int Id { get; set; }   

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public DateTime? PlannedDate { get; set; }

        public decimal? QuantityPlanned { get; set; }

        public string Uom { get; set; }

        public string Status { get; set; }  
    }
     
    public class ProductionTask_Detail
    {
        public int? UserId { get; set; }
        public List<long> deletedRowKeys { get; set; }
        public List<ProductionTask_ReferenceModel> insertedRowValues { get; set; }
        public List<ProductionTask_ReferenceModel> modifiedRowValues { get; set; }
    }

    public class ProductionTaskDetailModel
    {

        public int? Id {get; set;}
        
        public int? DetId { get; set; }
        
        public int? DocEntry { get; set; }
        
        public string DocNum { get; set; }
        
        public string ItemCode { get; set; }
        
        public string ItemName { get; set; }
        
        public int? Sort { get; set; }
        
        public string RoutingName { get; set; }
        
        public string OperatorName { get; set; }
        
        public DateTime? ProcessingDate { get; set; }
        
        public TimeSpan? Clock { get; set; }
        
        public TimeSpan? PracticeHours { get; set; }
        
        public int MachineNo { get; set; }
        
        public string Status { get; set; }
        
        public decimal? PlannedQty { get; set; }

        public decimal? Quantity { get; set; }

    }
     
    #endregion

    #region Services

    public class ProductionTaskService
    {
        private static string SqlSelect = @"SELECT 
            T0.* 
            FROM ""Tx_ProductionTask"" T0 
            INNER JOIN ""Tx_ProcessCard"" T1 ON T0.""BaseId"" = T1.""Id"" 
            WHERE T0.""Status"" = 'Open' 
            AND T1.""ProductionStatus"" = 'Released' ";


        public ProductionTaskModel GetNewModel(int userId)
        {
            ProductionTaskModel model = new ProductionTaskModel();
            model.UserId = userId; 
            model.ListReferences_ = ProductionTask_GetReferences(userId);

            return model;
        }

        public ProductionTaskModel Find(int userId)
        {
            ProductionTaskModel model = new ProductionTaskModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionTask_GetReferences(userId);
            return model;
        }


        //-------------------------------------
        //Detail  ProductionTask_Reference
        //-------------------------------------
        public ProductionTaskModel GetListByParam(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            ProductionTaskModel model = new ProductionTaskModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionTask_GetReferences(userId);

            return model;
        }

        //-------------------------------------
        //Detail  ProductionTask_Reference
        //-------------------------------------
        public List<ProductionTask_ReferenceModel> ProductionTask_GetReferences(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProductionTask_GetReferences(CONTEXT, userId);
            }
        }

        public List<ProductionTask_ReferenceModel> ProductionTask_GetReferences(HANA_APP CONTEXT, int userId)
        {
            string sql = SqlSelect;
            if (userId != 1)
            {
                sql += @" AND ""OperatorId"" = "+userId+" ";
            }
            return CONTEXT.Database.SqlQuery<ProductionTask_ReferenceModel>(sql).ToList();
        }

        public List<ProductionTaskDetailModel> ProductionTask_TabReferenceDetails(long id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProductionTask_TabReferenceDetails(CONTEXT, id);
            }
        }

        public List<ProductionTaskDetailModel> ProductionTask_TabReferenceDetails(HANA_APP CONTEXT, long id)
        {
            string ssql = @"
                SELECT T0.*,
                    T1.""ItemCode"",
                    T1.""ProdName"" AS ""ItemName"",
                    T1.""PlannedQty"" AS ""PlannedQty""
                    FROM ""Tx_ProcessCard_Detail"" T0
                INNER JOIN """ + DbProvider.dbSap_Name + @""".""OWOR"" T1 ON T0.""Id"" = T1.""U_IDU_WebId"" AND T0.""Sort"" = T1.""U_IDU_RoutingLevel""
                WHERE T0.""Id"" = :p0
                ORDER BY T0.""Sort"" ASC
            ";
            var detailModel = CONTEXT.Database.SqlQuery<ProductionTaskDetailModel>(ssql, id).ToList();
            return detailModel;
        }


        public void Update(ProductionTask_Detail model)
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
                                SpNotif.SpSysControllerTransNotif((int)model.UserId, "ProductionTask", CONTEXT, "before", "ProductionTask", "update", "Id", "0");
                                if (model.modifiedRowValues.Count > 0)
                                {
                                    foreach (var detail in model.modifiedRowValues)
                                    { 

                                    }
                                    CONTEXT.SaveChanges();
                                }
                                SpNotif.SpSysControllerTransNotif((int)model.UserId, "ProductionTask", CONTEXT, "after", "ProductionTask", "update", "Id", "0");
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

        public void Close(long id, int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction()) 
                { 
                    Tx_ProductionTask tx_ProductionTask = CONTEXT.Tx_ProductionTask.Find(id);
                    {
                        SpNotif.SpSysControllerTransNotif((int)userId, "ProductionTask", CONTEXT, "before", "ProductionTask", "close", "Id", id.ToString() );
                        
                        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                        tx_ProductionTask.Status = "Closed";
                        tx_ProductionTask.ModifiedDate = dtModified;
                        tx_ProductionTask.ModifiedUser = userId;

                        CONTEXT.SaveChanges();

                        SpNotif.SpSysControllerTransNotif((int)userId, "ProductionTask", CONTEXT, "after", "ProductionTask", "close", "Id", id.ToString() );
                        CONTEXT_TRANS.Commit();
                    }
                }
            }
        }

    }

    #endregion

}