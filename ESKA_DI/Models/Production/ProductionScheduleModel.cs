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

    public class ProductionSchedulerModel
    {
        public int UserId { get; set; } 

        public List<ProductionScheduler_ReferenceModel> ListReferences_ = new List<ProductionScheduler_ReferenceModel>();
    }


    public class ProductionScheduler_ReferenceModel
    {

        public int Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string TagId { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string TransactionCode { get; set; }

        public string TransactionName { get; set; }

        public DateTime? LastModifiedDate { get; set; }

    }

    public class ProductionSchedulerItemTagView___
    {
        public string TagId { get; set; }

        public List<ProductionSchedulerItemTagModel> ProductionSchedulerItemTagModel___ { get; set; }
    }

    public class ProductionSchedulerItemTagModel
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

    public class ProductionSchedulerService
    {

        public ProductionSchedulerModel GetNewModel(int userId)
        {
            DateTime toDate = DateTime.Now;
            DateTime fromDate = toDate.AddMonths(-1);

            ProductionSchedulerModel model = new ProductionSchedulerModel();
            model.UserId = userId; 

            model.ListReferences_ = ProductionScheduler_GetReferences(userId, fromDate, toDate, null, null, null, null);

            return model;
        }

        public ProductionSchedulerModel Find(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            ProductionSchedulerModel model = new ProductionSchedulerModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionScheduler_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);
            return model;
        }


        //-------------------------------------
        //Detail  ProductionScheduler_Reference
        //-------------------------------------
        public ProductionSchedulerModel GetListByParam(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            ProductionSchedulerModel model = new ProductionSchedulerModel();
            model.UserId = userId; 

            model.ListReferences_ = this.ProductionScheduler_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);

            return model;
        }

        //-------------------------------------
        //Detail  ProductionScheduler_Reference
        //-------------------------------------
        public List<ProductionScheduler_ReferenceModel> ProductionScheduler_GetReferences(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ProductionScheduler_GetReferences(CONTEXT, userId, fromDate, toDate, itemCode, whsCode, tagId, status);
            }
        }

        public List<ProductionScheduler_ReferenceModel> ProductionScheduler_GetReferences(HANA_APP CONTEXT, int userId, DateTime fromDate, DateTime toDate, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            string sql = @"
            CALL ""SpProductionScheduler_GetReferences"" (
                :p0 --userId
            )";

            return CONTEXT.Database.SqlQuery<ProductionScheduler_ReferenceModel>(sql,  
                userId 
                ).ToList();
        }

    }

    #endregion

}