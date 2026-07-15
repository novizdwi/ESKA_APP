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

        public int Id { get; set; }

        public string TransNo { get; set; }
        
        public string ItemCode { get; set; }
        
        public string ItemName { get; set; }
        
        public string DocNum { get; set; }
        
        public string RoutingName { get; set; }
        
        public decimal? QuantityPlanned { get; set; }
        
        public decimal? QuantityActual { get; set; }
        
        public decimal? QuantityRemain { get; set; }
        public TimeSpan? EstimatedHours { get; set; }
        public TimeSpan? ActualHours { get; set; }

    }


    #endregion

    #region Services

    public class ProductionActivityService
    {
        private static string SqlSelect = @"
        SELECT
	        T0.""Id"",
	        T0.""TransNo"",
	        T0.""ItemCode"",
	        T0.""ItemName"",
	        T0.""DocNum"",
	        T1.""RoutingName"",
	        T0.""QuantityPlanned"",
	        T0.""QuantityActual"",
	        COALESCE(T0.""QuantityPlanned"", 0 ) - COALESCE(T0.""QuantityActual"", 0) AS ""QuantityRemain"",
	        T0.""EstimatedHours"",
	        T0.""ActualHours""
        FROM ""Tx_ProductionTask"" T0
        INNER JOIN ""Tx_ProcessCard_Detail"" T1 ON T0.""BaseId"" = T1.""Id"" AND T0.""BaseDetId"" = T1.""DetId""
        WHERE T0.""Id"" = :p0
        ";


        public ProductionTaskActivityModel GetNewModel(int userId, long id)
        {
            ProductionTaskActivityModel model = this.GetById(userId, id); 

            model.UserId = userId; 
            return model;
        }

        public ProductionTaskActivityModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
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

            return model;
        }

    }

    #endregion

}