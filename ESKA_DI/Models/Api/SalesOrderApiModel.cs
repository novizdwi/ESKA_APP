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

    //public class SalesOrder_Mutex
    //{
    //    private static System.Threading.Mutex SalesOrder_TransactionLock = new System.Threading.Mutex();

    //    public static void wait()
    //    {
    //        SalesOrder_TransactionLock.WaitOne();
    //    }

    //    public static void release()
    //    {
    //        SalesOrder_TransactionLock.ReleaseMutex();
    //    }

    //}

    //public class SalesOrderII_Mutex
    //{
    //    private static System.Threading.Mutex SalesOrderII_TransactionLock = new System.Threading.Mutex();

    //    public static void wait()
    //    {
    //        SalesOrderII_TransactionLock.WaitOne();
    //    }

    //    public static void release()
    //    {
    //        SalesOrderII_TransactionLock.ReleaseMutex();
    //    }

    //}

    //public class SalesOrderApiModel
    //{

    //    public Int32? DocEntry { get; set; }

    //    public Int32? DocNum { get; set; }

    //    public DateTime DocumentDate { get; set; }

    //    public string SadpIICode { get; set; }

    //    public string Destination { get; set; }

    //    public string ShippingType { get; set; }

    //    public string CustomerCode { get; set; }

    //    public string CustomerName { get; set; }

    //    public string Remarks { get; set; }

    //    public string SyncDesc { get; set; }

    //    public List<SalesOrderApi_ItemModel> Lines { get; set; } //= new List<SalesOrder_ItemModel>();

    //}

    //public class SalesOrderApi_ItemModel
    //{
    //    public string ItemCode { get; set; }

    //    public double Quantity { get; set; }

    //    public double Capacity { get; set; }

    //    public double Price { get; set; }

    //    public string WarehouseCode { get; set; }
    //}

    //public class SalesOrderResultModel
    //{
    //    public string NewDocEntry { get; set; }
    //}

    #endregion

    #region Services

//    public class SalesOrderApiServices
//    {
//        //SADP_I
//        public SalesOrderResultModel Add(SalesOrderApiModel model)
//        {
//            SalesOrderResultModel result = new SalesOrderResultModel();
//            if (model != null)
//            {
//                SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();
//                try
//                {
//                    oCompany.StartTransaction();

//                    SAPbobsCOM.Documents oSalesOrder = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
//                    oSalesOrder.DocType = BoDocumentTypes.dDocument_Items;
//                    oSalesOrder.CardCode = model.CustomerCode; //model.SadpIICode; //
//                    //if (model.BplId.HasValue)
//                    //{
//                    //    oSalesOrder.BPL_IDAssignedToInvoice = model.BplId.Value;
//                    //}
//                    //oSalesOrder.SalesPersonCode = model.SalesPersonCode;
//                    if (model.DocumentDate != null)
//                        oSalesOrder.DocDate = model.DocumentDate;
//                    if (model.DocumentDate != null)
//                        oSalesOrder.TaxDate = model.DocumentDate;
//                    if (model.DocumentDate != null)
//                        oSalesOrder.DocDueDate = model.DocumentDate;
//                    oSalesOrder.Comments = model.Remarks;
//                    oSalesOrder.DocObjectCode = BoObjectTypes.oOrders;

//                    oSalesOrder.UserFields.Fields.Item("U_Area").Value = model.Destination;
//                    oSalesOrder.UserFields.Fields.Item("U_ShippingType").Value = model.ShippingType;
//                    oSalesOrder.UserFields.Fields.Item("U_SODocEntry").Value = model.DocEntry;                   
//                    oSalesOrder.UserFields.Fields.Item("U_SODocNum").Value = model.DocNum;
//                    oSalesOrder.UserFields.Fields.Item("U_SOCardCode").Value = model.SadpIICode;  //model.CustomerCode;
//                    oSalesOrder.UserFields.Fields.Item("U_SOCardName").Value = model.CustomerName;

//                    foreach (var it in model.Lines)
//                    {
//                        oSalesOrder.Lines.ItemCode = it.ItemCode;
//                        //dikasih default warehousenya aja
//                        string sQuery = @"SELECT IFNULL(T0.""DfltWH"",'') FROM ""OITM"" T0 WHERE T0.""ItemCode"" = '{0}'  ";
//                        sQuery = string.Format(sQuery, it.ItemCode);
//                        string whsDefault = SapCompany.RetRstField(oCompany, sQuery);
//                        if (whsDefault == "")
//                        {
//                            throw new Exception("Setting Default Warehouse untuk item code (SADP I):" + it.ItemCode);
//                        }
//                        oSalesOrder.Lines.WarehouseCode = whsDefault ;//it.WarehouseCode;

//                        oSalesOrder.Lines.Quantity = it.Quantity;
//                        oSalesOrder.Lines.UserFields.Fields.Item("U_IDU_Capacity").Value = it.Capacity;

//                        //if (it.Price != null && it.Price != 0)
//                        //{
//                        //    oSalesOrder.Lines.Price = it.Price;
//                        //}
//                        //else 
//                        //{
//                            string ssql = @"SELECT IFNULL(T0.""U_PricePerLiter"",0) FROM ""@IDU_DELIVERY_PRICE1"" T0 WHERE T0.""Code"" = '{0}' AND T0.""U_Area"" = '{1}' AND T0.""U_Jenis"" = '{2}' AND IFNULL(T0.""U_Capacity"",0) = CASE WHEN T0.""U_Jenis"" = 'VESSEL' THEN 0 ELSE {3} END  ";
//                            ssql = string.Format(ssql, model.CustomerCode, model.Destination, model.ShippingType, it.Capacity);
//                            string harga = SapCompany.RetRstField(oCompany, ssql);
//                            if (harga == "")
//                            {
//                                throw new Exception("Delivery price belum di setting :" + model.CustomerCode + "|" + model.ShippingType + "|" + model.Destination + "|" + it.Capacity );
//                            }
//                            else
//                            {
//                                oSalesOrder.Lines.Price = double.Parse(harga);
//                            }
//                        //}

//                        oSalesOrder.Lines.Add();
//                    }


//                    if (oSalesOrder.Add() != 0)
//                    {
//                        throw new Exception(oCompany.GetLastErrorCode() + "|" + oCompany.GetLastErrorDescription());
//                    }

//                    string docEntry;
//                    docEntry = oCompany.GetNewObjectKey();

//                    result.NewDocEntry = docEntry;

//                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
//                }
//                catch (Exception ex)
//                {

//                    if (oCompany.InTransaction)
//                    {
//                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
//                    }
//                    throw new Exception(ex.Message);
//                    //throw ex;
//                }
//                finally
//                {
//                    SAPCachedCompany.Release(oCompany);
//                }

//            }
//            return result;

//        }

//        //SADP_I
//        public SalesOrderResultModel CheckByDocEntry(SalesOrderApiModel model)
//        {
//            using (var CONTEXT = new HANA_APP())
//            {
//                return CheckByDocEntry(CONTEXT, model);
//            }
//        }

//        //SADP_I
//        public SalesOrderResultModel CheckByDocEntry(HANA_APP CONTEXT, SalesOrderApiModel model)
//        {
//            SalesOrderResultModel result = null;
//            if (model.DocEntry != 0)
//            {
//                string ssql = @"SELECT T0.""DocEntry"" AS ""NewDocEntry""
//                            FROM ""{0}"".""ORDR"" T0
//                            WHERE T0.""U_SODocEntry""=:p0 AND T0.""U_SODocNum""= :p1 AND T0.""U_SOCardCode""= ':p2'  ";
//                ssql = string.Format(ssql, DbProvider.dbSap_Name);
//                result = CONTEXT.Database.SqlQuery<SalesOrderResultModel>(ssql, model.DocEntry, model.DocNum, model.CustomerCode).FirstOrDefault();
//            }

//            return result;
//        }

//        //SADP_II
//        public SalesOrderResultModel UpdateSO(Int32? DocEntry, string ResultType, string ResultDesc)
//        {
//            SalesOrderResultModel result1 = new SalesOrderResultModel();
//            result1.NewDocEntry = null;
//            if (!DocEntry.HasValue)
//            {
//                return result1;
//            }

//            SAPbobsCOM.Company oCompany = SAPCachedCompanyII.GetCompanyII();
//            try
//            {
//                oCompany.StartTransaction();

//                SAPbobsCOM.Documents oSalesOrder = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
//                oSalesOrder.GetByKey(DocEntry.Value);

//                if (ResultType == "fail")
//                {
//                    oSalesOrder.UserFields.Fields.Item("U_IDU_IsSync").Value = "N";
//                    oSalesOrder.UserFields.Fields.Item("U_IDU_SyncError").Value = ResultDesc;
//                }
//                else
//                {
//                    oSalesOrder.UserFields.Fields.Item("U_IDU_IsSync").Value = "Y";
//                    oSalesOrder.UserFields.Fields.Item("U_IDU_SyncError").Value = ResultDesc;
//                    oSalesOrder.UserFields.Fields.Item("U_IDU_SyncDocEntry").Value = DocEntry.Value;
//                }

//                if (oSalesOrder.Update() != 0)
//                {
//                    throw new Exception(oCompany.GetLastErrorCode() + "|" + oCompany.GetLastErrorDescription());
//                }

//                string DocE = DocEntry.Value.ToString();
//                result1.NewDocEntry = DocE;

//                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
//            }
//            catch (Exception ex)
//            {

//                if (oCompany.InTransaction)
//                {
//                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
//                }
//                throw new Exception(ex.Message);
//                //throw ex;
//            }
//            finally
//            {
//                SAPCachedCompanyII.ReleaseII(oCompany);
//            }
//            return result1;
//        }



//        //SADP_II
//        public List<SalesOrderApiModel> CheckSoDeliveredBySADPI()
//        {
//            using (var CONTEXT = new HANA_APP())
//            {
//                return CheckSODeliveredBySADPI(CONTEXT);
//            }
//        }

//        //SADP_II
//        public List<SalesOrderApiModel> CheckSODeliveredBySADPI(HANA_APP CONTEXT)
//        {
//            List<SalesOrderApiModel> result = null;
//            string ssql = @"SELECT T0.""DocEntry"", T0.""DocNum"", T0.""CardCode"" AS ""CustomerCode"", IFNULL(T0.""U_IDU_SyncError"",'') AS ""SyncDesc""
//                        FROM ""{0}"".""ORDR"" T0
//                        WHERE IFNULL(T0.""U_IDU_IsSync"", 'N') = 'N' AND IFNULL(T0.""U_SADPI"", 'N') = 'Y' AND T0.""DocStatus"" = 'O' AND T0.""CANCELED"" <> 'Y' ";
//            ssql = string.Format(ssql, DbProvider.dbSap_Name2);
//            result = CONTEXT.Database.SqlQuery<SalesOrderApiModel>(ssql).ToList();

//            return result;
//        }

//        //SADP_II
//        public SalesOrderApiModel GetByDocEntry(Int32? docEntry = 0)
//        {
//            using (var CONTEXT = new HANA_APP())
//            {
//                return GetByDocEntry(CONTEXT, docEntry);
//            }
//        }

//        //SADP_II
//        public SalesOrderApiModel GetByDocEntry(HANA_APP CONTEXT, Int32? docEntry = 0)
//        {
//            SalesOrderApiModel model = null;
//            if (docEntry != 0)
//            {
////                string ssql = @"SELECT T0.""DocEntry"", T0.""DocNum"", T0.""DocDate"" AS ""DocumentDate"", T0.""CardCode"" AS ""CustomerCode"", T1.""CardName"" AS ""CustomerName"", T0.""Comments"" AS ""Remarks"",
////                        T2.""SadpIICardCode"" AS ""SadpIICode"", T0.""U_Area"" AS ""Destination"", T0.""U_ShippingType"" AS ""ShippingType""
////                        FROM ""{0}"".""ORDR"" T0
////                        LEFT JOIN ""Tm_GeneralSetting"" T2 ON 1=1
////                        LEFT JOIN ""{0}"".""OCRD"" T1 ON T0.""CardCode""= T2.""SadpIICardCode""
////                        WHERE T0.""DocEntry""=:p0 ";
//                string ssql = @"SELECT T0.""DocEntry"", T0.""DocNum"", T0.""DocDate"" AS ""DocumentDate"", T0.""CardCode"" AS ""CustomerCode"", T2.""SadpIICardCode"" AS ""CustomerName"", T0.""Comments"" AS ""Remarks"",
//                        T2.""SadpIICardCode"" AS ""SadpIICode"", T0.""U_Area"" AS ""Destination"", T0.""U_ShippingType"" AS ""ShippingType""
//                        FROM ""{0}"".""ORDR"" T0
//                        LEFT JOIN ""Tm_GeneralSetting"" T2 ON 1=1
//                        WHERE T0.""DocEntry""=:p0 ";
//                ssql = string.Format(ssql, DbProvider.dbSap_Name2);
//                model = CONTEXT.Database.SqlQuery<SalesOrderApiModel>(ssql, docEntry).Single();
//                model.Lines = this.SalesOrder_Lines(CONTEXT, docEntry);
//            }

//            return model;
//        }

//        //SADP_II
//        public List<SalesOrderApi_ItemModel> SalesOrder_Lines(HANA_APP CONTEXT, Int32? docEntry = 0)
//        {

////            string ssql = @"SELECT T0.""ItemCode"", T0.""Quantity"", T0.""Price"", T0.""WhsCode"" AS ""WarehouseCode"", T0.""U_IDU_Capacity"" AS ""Capacity""
////                        FROM ""{0}"".""RDR1"" T0
////                        INNER JOIN ""Tm_GeneralSetting"" T1 ON T0.""ItemCode"" = T1.""BiayaKirimItemCode""
////                        WHERE T0.""DocEntry""=:p0 ";
//            //persiapan nanti diubah -- sekarang SADPII cm ada solar
//            string ssql = @"SELECT T2.""BiayaKirimItemCode"" AS ""ItemCode"", T0.""Quantity"", T0.""Price"", T0.""WhsCode"" AS ""WarehouseCode"", T0.""U_IDU_Capacity"" AS ""Capacity""
//                        FROM ""{0}"".""RDR1"" T0
//                        INNER JOIN ""Tm_GeneralSetting"" T1 ON T0.""ItemCode"" = T1.""SolarItemCode""
//                        INNER JOIN ""Tm_GeneralSetting"" T2 ON 1 = 1
//                        WHERE T0.""DocEntry""=:p0 ";
//            ssql = string.Format(ssql, DbProvider.dbSap_Name2);
//            return CONTEXT.Database.SqlQuery<SalesOrderApi_ItemModel>(ssql, docEntry).ToList();
//        }
     
//    }

    #endregion

}