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

namespace Models.Master.Item
{
    #region Models

    public class ItemMasterModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public string Key { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; } 

        public List<ItemMaster_DetailModel> ListDetails_ = new List<ItemMaster_DetailModel>();
    }

    public class ItemMaster_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserItemCode { get; set; }

        public string ItemCode { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public Decimal? OnHand { get; set; }

        public Decimal? OnHandSAP_ { get; set; }
    }
    #endregion

    #region Services

    public class ItemMasterService
    {

        public ItemMasterModel GetNewModel(int userItemCode)
        {
            ItemMasterModel model = new ItemMasterModel();

            return model;
        }
        public ItemMasterModel GetByItemCode(int userItemCode, string itemCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetByItemCode(CONTEXT, userItemCode, itemCode);
            }
        }

        public ItemMasterModel GetByItemCode(HANA_APP CONTEXT, int userItemCode, string itemCode)
        {
            ItemMasterModel model = null;
            if (!string.IsNullOrWhiteSpace(itemCode))
            {
                string ssql = @"SELECT ""ItemCode"" AS ""Key"", *
                            FROM ""Tm_Item"" T0
                            WHERE T0.""ItemCode"" = :p0
                            ORDER BY T0.""ItemCode"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<ItemMasterModel>(ssql, itemCode).Single();

                model.ListDetails_ = this.ItemMaster_Details(CONTEXT, itemCode);
            }

            return model;
        }
        public List<ItemMaster_DetailModel> ItemMaster_Details(string itemCode = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ItemMaster_Details(CONTEXT, itemCode);
            }

        }

        public List<ItemMaster_DetailModel> ItemMaster_Details(HANA_APP CONTEXT, string itemCode = "")
        {
            string ssql = @"SELECT T0.*, T3.""OnHand"" AS ""OnHandSAP_""
                FROM ""Tm_Item_Warehouse"" T0
                LEFT JOIN """ + DbProvider.dbSap_Name + @""".""OITW"" T3 ON T0.""ItemCode"" = T3.""ItemCode"" AND T0.""WhsCode"" = T3.""WhsCode""
                WHERE T0.""ItemCode"" =:p0
                ORDER BY T0.""WhsCode"" ASC
            ";
            var items = CONTEXT.Database.SqlQuery<ItemMaster_DetailModel>(ssql, itemCode).ToList();
            return items;
        }

        public ItemMasterModel NavFirst(int userItemCode)
        {
            ItemMasterModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userItemCode, "ItemMaster");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                string itemCode = CONTEXT.Database.SqlQuery<string>("SELECT TOP 1 T0.\"ItemCode\" FROM \"Tm_Item\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"ItemCode\" ASC").FirstOrDefault();

                model = this.GetByItemCode(CONTEXT, userItemCode, itemCode);
            }

            return model;

        }
        public ItemMasterModel NavPrevious(int userItemCode, string itemCode = "")
        {
            ItemMasterModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userItemCode, "ItemMaster");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                string ItemCode = CONTEXT.Database.SqlQuery<string>("SELECT TOP 1 T0.\"ItemCode\" FROM \"Tm_Item\" T0 WHERE T0.\"ItemCode\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"ItemCode\" DESC", itemCode).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(itemCode))
                {
                    model = this.GetByItemCode(CONTEXT, userItemCode, itemCode);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userItemCode);
            }


            return model;
        }

        public ItemMasterModel NavNext(int userItemCode, string itemCode)
        {
            ItemMasterModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userItemCode, "ItemMaster");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                itemCode = CONTEXT.Database.SqlQuery<string>("SELECT TOP 1 T0.\"ItemCode\" FROM \"Tm_Item\" T0 WHERE T0.\"ItemCode\">:p0 " + sqlCriteria + "  ORDER BY T0.\"ItemCode\" ASC", itemCode).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(itemCode))
                {
                    model = this.GetByItemCode(CONTEXT, userItemCode, itemCode);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userItemCode);
            }

            return model;
        }

        public ItemMasterModel NavLast(int userItemCode)
        {
            ItemMasterModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userItemCode, "ItemMaster");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                string ItemCode = CONTEXT.Database.SqlQuery<string>("SELECT TOP 1 T0.\"ItemCode\" FROM \"Tm_Item\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"ItemCode\" DESC").FirstOrDefault();

                model = this.GetByItemCode(CONTEXT, userItemCode, ItemCode);
            }

            return model;
        }
    }


    #endregion

}