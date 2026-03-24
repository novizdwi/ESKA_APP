using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using ESKA_DI.Models._EF;

using SAPbobsCOM;
using Models._Sap;

namespace Models.Api
{

    #region Models

    public class Delivery_Mutex
    {
        private static System.Threading.Mutex Delivery_TransactionLock = new System.Threading.Mutex();

        public static void wait()
        {
            Delivery_TransactionLock.WaitOne();
        }

        public static void release()
        {
            Delivery_TransactionLock.ReleaseMutex();
        }

    }

    public class DeliveryApiModel
    {

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string TransType { get; set; }

        public string TransNo { get; set; }

        public DateTime DocumentDate { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string Remarks { get; set; }

        public string SppOwner { get; set; }

        public long? SppId { get; set; }

        public string SyncDesc { get; set; }

        public List<DeliveryApi_ItemModel> Lines { get; set; } //= new List<Delivery_ItemModel>();

    }

    public class DeliveryApi_ItemModel
    {
        public string ItemCode { get; set; }

        public double Quantity { get; set; }

        public double Capacity { get; set; }

        public double Price { get; set; }

        public Int32 BinAbsEntry { get; set; }

        public string WarehouseCode { get; set; }

        public Int32? BaseType { get; set; }

        public Int32? BaseEntry { get; set; }

        public Int32? BaseLine { get; set; }
    }

    public class DeliveryResultModel
    {
        public string NewDocEntry { get; set; }
        public string NewDocNum { get; set; }
    }

    #endregion

    #region Services

    public class DeliveryApiServices
    {
        //APP
        public List<DeliveryApiModel> CheckLOSyncDeliveryBiayaKirim()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CheckLOSyncDeliveryBiayaKirim(CONTEXT);
            }
        }

        //APP
        public List<DeliveryApiModel> CheckLOSyncDeliveryBiayaKirim(HANA_APP CONTEXT)
        {
            List<DeliveryApiModel> result = null;
            string ssql = @"SELECT T0.""Id"", T0.""SppOwner"", T0.""SppId"", IFNULL(T1.""SyncDlvryBiayaAngkutError"",'') AS ""SyncDesc""
                        FROM ""Tx_LoadingOrder"" T0
                        WHERE  IFNULL(T0.""IsSyncDlvryBiayaAngkut"", 'N') = 'N' AND T0.""Status"" IN ('Delivered') 
                        AND IFNULL(T0.""SppId"", 0) <> 0
                        ";
            //ssql = string.Format(ssql, DbProvider.dbSap_Name);
            result = CONTEXT.Database.SqlQuery<DeliveryApiModel>(ssql).ToList();

            return result;
        }
         

    }

    #endregion

}