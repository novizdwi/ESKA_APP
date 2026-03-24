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
using BBS_DI.Models._EF;
using Models._Sap;
using SAPbobsCOM;



namespace Models._Alert
{

    #region Models

    public static class AlertGetList
    {


    }
    public class AlertModel {
        public long Id { get;set; }
        public int UserId { get; set; }
        public string BaseObject { get; set; }
        public long BaseId { get; set; }
        public string NotificationMessage { get; set; }
        public int SenderId { get; set; }
        public string IsRead { get; set; }
        public DateTime? LastShowTime { get; set; }
        public string RelativeDate { get; set; }
        public string TransNo { get; set; }
        public int? DayDifference { get; set; }

    }

    public class Alert_ViewModel
    {

        public List<AlertModel> Tp_UserAlertUnread_ = new List<AlertModel>();

        public List<AlertModel> Tp_UserAlertAll_ = new List<AlertModel>();

    }
    #endregion

    #region Services

    public class AlertNotificationService
    {

        public Alert_ViewModel GetAlertNotification(int userId =0)
        {
            Alert_ViewModel model = new Alert_ViewModel();
            if (userId != 0)
            {
                model.Tp_UserAlertAll_ = GetAlertNotificationModel(userId, "All");
                model.Tp_UserAlertUnread_ = GetAlertNotificationModel(userId, "Unread");
            }
            return model;
        }

        public List<AlertModel> GetAlertNotificationModel(int userId,string tab)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAlertNotificationModel(CONTEXT, id);
            }
        }

        public List<AlertModel> GetAlertNotificationModel(HANA_APP CONTEXT, int userId, string tab)
        {
            string ssql = @"
                SELECT TOP 24 T0.*,                    
                    ABS(DATEDIFF(DAY, GETDATE() , T0.""LastShowTime"")) ""DayDifference""
                FROM ""Tp_UserAlert"" T0
                LEFT JOIN ""Ts_List"" T2 ON T0.""BaseType"" = T2.""Code"" AND T2.""Type"" = 'RFIDTransType'
                WHERE T0.""UserId"" = :p0
            ";

            string tabRead = tab == "Unread" ? " AND ISNULL(T0.IsRead,'N') = 'N' " : "";
            ssql += tabRead;
            ssql += " ORDER BY T0.Id ASC ";
            return CONTEXT.Database.SqlQuery<AlertModel>(ssql, userId).ToList();
        }

        public void MarkAllAsRead(int userId)
        {
            var sql1 = "UPDATE T0 SET   "
                    + " IsRead = 'Y' "
                    + " FROM [" + DbProvider.dbApp_Name + "].dbo.Tp_UserAlert T0 "
                    + " WHERE T0.UserId = @0";
            DbProvider.dbApp.Execute(sql1, userId);
        }
    }

    #endregion

}