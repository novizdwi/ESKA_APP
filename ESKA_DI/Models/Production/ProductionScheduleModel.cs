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

        public List<ProductionSchedule_ReferenceModel> ListReferences_ = new List<ProductionSchedule_ReferenceModel>();
    }


    public class ProductionSchedule_ReferenceModel
    {

        public int Id { get; set; }

        public string TransNo { get; set; }
        
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public DateTime? StartDate { get; set; }

        public decimal? Quantity { get; set; }

        public int? VisOrder { get; set; }

        List<ProductionScheduleDetailModel> ListDetails_ = new List<ProductionScheduleDetailModel>();
    } 

    public class ProductionScheduleDetailModel
    {
        public long LogId { get; set; }

        public string TransType { get; set; }

        public long? BaseId { get; set; }

        public string BaseTransNo { get; set; }

        public string TransactionName_ { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string NewItemCode { get; set; }

        public string NewItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string OldTagId { get; set; }

        public string NewTagId { get; set; }

        public string Status { get; set; }

        public string Event { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedUser { get; set; }
    }

    #endregion

    #region Services

    public class ProductionScheduleService
    {

        public ProductionScheduleModel GetNewModel(int userId)
        {
            DateTime toDate = DateTime.Now;
            DateTime fromDate = toDate.AddMonths(-1);

            ProductionScheduleModel model = new ProductionScheduleModel();
            model.UserId = userId; 

            model.ListReferences_ = ProductionSchedule_GetReferences(userId, fromDate, toDate, null, null, null, null);

            return model;
        }

        public ProductionScheduleModel Find(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            ProductionScheduleModel model = new ProductionScheduleModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionSchedule_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);
            return model;
        }


        //-------------------------------------
        //Detail  ProductionSchedule_Reference
        //-------------------------------------
        public ProductionScheduleModel GetListByParam(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            ProductionScheduleModel model = new ProductionScheduleModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionSchedule_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);

            return model;
        }

        //-------------------------------------
        //Detail  ProductionSchedule_Reference
        //-------------------------------------
        public List<ProductionSchedule_ReferenceModel> ProductionSchedule_GetReferences(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProductionSchedule_GetReferences(CONTEXT, userId, fromDate, toDate, itemCode, whsCode, tagId, status);
            }
        }

        public List<ProductionSchedule_ReferenceModel> ProductionSchedule_GetReferences(HANA_APP CONTEXT, int userId, DateTime fromDate, DateTime toDate, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            string sql = @"
            CALL ""SpProductionSchedule_GetReferences"" (
                :p0 --userId
            )";

            return CONTEXT.Database.SqlQuery<ProductionSchedule_ReferenceModel>(sql,  
                userId 
                ).ToList();
        }

    }

    #endregion

}