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
        public int Id { get; set; }

        public List<ProductionSchedule_ReferenceModel> ListReferences_ = new List<ProductionSchedule_ReferenceModel>();
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

    public class ProductionScheduleDetailModel
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
                    T1.""PlannedQty"" AS ""PlannedQty""
                    FROM ""Tx_ProcessCard_Detail"" T0
                INNER JOIN """ + DbProvider.dbSap_Name + @""".""OWOR"" T1 ON T0.""Id"" = T1.""U_IDU_WebId"" AND T0.""Sort"" = T1.""U_IDU_RoutingLevel""
                WHERE T0.""Id"" = :p0
                ORDER BY T0.""Sort"" ASC
            ";
            var detailModel = CONTEXT.Database.SqlQuery<ProductionScheduleDetailModel>(ssql, id).ToList();
            return detailModel;
        }

    }

    #endregion

}