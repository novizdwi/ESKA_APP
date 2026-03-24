using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;

using Models._Sap;
using SAPbobsCOM;


namespace Models.Master.Item
{

    #region Models

    public class RfidMonitoringModel
    {
        public int UserId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string ItemCode { get; set; }

        public string WhsCode { get; set; }

        public string TagId { get; set; }

        public string Status { get; set; }

        public List<RfidMonitoring_ReferenceModel> ListReferences_ = new List<RfidMonitoring_ReferenceModel>();
    }


    public class RfidMonitoring_ReferenceModel
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

    public class RfidMonitoringItemTagView___
    {
        public string TagId { get; set; }

        public List<RfidMonitoringItemTagModel> RfidMonitoringItemTagModel___ { get; set; }
    }

    public class RfidMonitoringItemTagModel
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

        public DateTime? CreatedDate { get; set; }

        public string CreatedUser { get; set; }
    }

    #endregion

    #region Services

    public class RfidMonitoringService
    {

        public RfidMonitoringModel GetNewModel(int userId)
        {
            DateTime toDate = DateTime.Now;
            DateTime fromDate = toDate.AddMonths(-1);

            RfidMonitoringModel model = new RfidMonitoringModel();
            model.UserId = userId;
            model.FromDate = fromDate;
            model.ToDate = toDate;
            model.ItemCode = null;
            model.WhsCode = null;
            model.TagId = null;
            model.Status = null;

            model.ListReferences_ = RfidMonitoring_GetReferences(userId, fromDate, toDate, null, null, null, null);

            return model;
        }

        public RfidMonitoringModel Find(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            RfidMonitoringModel model = new RfidMonitoringModel();
            model.UserId = userId;
            model.FromDate = fromDate;
            model.ToDate = toDate;
            model.ItemCode = itemCode;
            model.WhsCode = whsCode;
            model.TagId = tagId;
            model.Status = status;

            model.ListReferences_ = this.RfidMonitoring_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);
            return model;
        }


        //-------------------------------------
        //Detail  RfidMonitoring_Reference
        //-------------------------------------
        public RfidMonitoringModel GetListByParam(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            RfidMonitoringModel model = new RfidMonitoringModel();
            model.UserId = userId;
            model.FromDate = fromDate;
            model.ToDate = toDate;
            model.ItemCode = itemCode;
            model.WhsCode = whsCode;
            model.TagId = tagId;
            model.Status = status;

            model.ListReferences_ = this.RfidMonitoring_GetReferences(userId, fromDate, toDate, itemCode, whsCode, tagId, status);

            return model;
        }

        //-------------------------------------
        //Detail  RfidMonitoring_Reference
        //-------------------------------------
        public List<RfidMonitoring_ReferenceModel> RfidMonitoring_GetReferences(int userId, DateTime fromDate, DateTime toDate, string itemCode, string whsCode, string tagId, string status)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return RfidMonitoring_GetReferences(CONTEXT, userId, fromDate, toDate, itemCode, whsCode, tagId, status);
            }
        }

        public List<RfidMonitoring_ReferenceModel> RfidMonitoring_GetReferences(HANA_APP CONTEXT, int userId, DateTime fromDate, DateTime toDate, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            string sql = @"
            CALL ""SpRfidMonitoring_GetReferences"" (
                :p0, --userId
                :p1, --fromDate
                :p2, --toDate
                :p3, --itemCode
                :p4, --whsCode
                :p5, --tagId
                :p6 --Status
            )";

            return CONTEXT.Database.SqlQuery<RfidMonitoring_ReferenceModel>(sql, 
                userId,
                fromDate,
                toDate,
                itemCode,
                whsCode,
                tagId,
                status
                ).ToList();
        }

        public RfidMonitoringItemTagView___ GetItemTags(string tagId)
        {
            string sql = null;

            RfidMonitoringItemTagView___ model = new RfidMonitoringItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                model.TagId = tagId;

                sql = @"SELECT TOP 20 
                        T0.""LogId"",
                        LTRIM(
                            REPLACE_REGEXPR('([A-Z])' IN T0.""BaseType"" WITH ' \1' OCCURRENCE ALL)
                        ) AS ""TransType"",
                        T0.""BaseId"",
                        T0.""BaseTransNo"",
                        T0.""ItemCode"",
                        T0.""ItemName"",
                        T0.""NewItemCode"",
                        T0.""NewItemName"",
                        T0.""WhsCode"",
                        T0.""WhsName"",
                        CASE WHEN T0.""OldStatus"" = 'I' THEN 'Inactive'
                             WHEN T0.""OldStatus"" = 'A' THEN 'Active'
                             WHEN T0.""OldStatus"" = 'P' THEN 'Pending'
                        ELSE T0.""OldStatus""
                        END AS ""Status"",
                        T0.""OldTagId"",
                        T0.""NewTagId"",
                        T0.""CreatedDate"",
                        T1.""FirstName"" AS ""CreatedUser"",
                        T2.""Name"" AS ""TransactionName_""
                    FROM ""Tm_Item_Warehouse_Tag_Log"" T0 
                    INNER JOIN ""Tm_User"" T1 ON T0.""CreatedUser"" = T1.""Id""
                    LEFT JOIN ""Ts_List"" T2 ON T0.""BaseType"" = T2.""Code"" AND T2.""Type"" = 'RFIDTransType' 
                    WHERE T0.""OldTagId"" = '" + tagId + @"'
                        OR(T0.""NewTagId"" = '" + tagId + @"' AND T0.""BaseType"" = 'ReplaceTags')
                    ORDER BY T0.""CreatedDate"" DESC 
                    ";

                model.RfidMonitoringItemTagModel___ = CONTEXT.Database.SqlQuery<RfidMonitoringItemTagModel>(sql).ToList();
            }

            return model;
        }

    }

    #endregion

}