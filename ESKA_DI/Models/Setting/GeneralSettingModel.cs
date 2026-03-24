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

namespace Models.Setting.GeneralSetting
{

    #region Models

    public class GeneralSettingModel
    {

        public int _UserId { get; set; }

        public int Id { get; set; }

        public string StockOpnameCoaCode {get;set;}

        public string StockOpnameCoaName  {get;set;}

        public List<GeneralSetting_AdjustmentCoaModel> ListCoas_ = new List<GeneralSetting_AdjustmentCoaModel>();

        public GeneralSetting_AdjustmentCoas DetailCoas_ { get; set; }

    }

    public class GeneralSetting_AdjustmentCoaModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string U_IDU_Account { get; set; }

        public string U_IDU_Type { get; set; }

        public string AcctName { get; set; }

    }

    public class GeneralSetting_AdjustmentCoas
    {
        public List<string> deletedRowKeys { get; set; }
        public List<GeneralSetting_AdjustmentCoaModel> insertedRowValues { get; set; }
        public List<GeneralSetting_AdjustmentCoaModel> modifiedRowValues { get; set; }
    }
    

    #endregion

    #region Services

    public class GeneralSettingService
    {

        public void Update(GeneralSettingModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_GeneralSetting tm_GeneralSetting = CONTEXT.Database.SqlQuery<Tm_GeneralSetting>("SELECT TOP 1 * FROM \"Tm_GeneralSetting\" ").SingleOrDefault();
                            if (tm_GeneralSetting != null)
                            {
                                tm_GeneralSetting = CONTEXT.Tm_GeneralSetting.Find(tm_GeneralSetting.Id);

                                String keyValue;
                                keyValue = model.Id.ToString();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_GeneralSetting", "update", "Id", keyValue);

                                var exceptColumns = new string[] { "Id" };
                                CopyProperty.CopyProperties(model, tm_GeneralSetting, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tm_GeneralSetting.ModifiedDate = dtModified;
                                tm_GeneralSetting.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_GeneralSetting", "update", "Id", keyValue);

                            }
                            else
                            {
                                tm_GeneralSetting = new Tm_GeneralSetting();
                                CopyProperty.CopyProperties(model, tm_GeneralSetting, false);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                tm_GeneralSetting.TransType = "GeneralSetting";
                                tm_GeneralSetting.CreatedDate = dtModified;
                                tm_GeneralSetting.CreatedUser = model._UserId;
                                tm_GeneralSetting.ModifiedDate = dtModified;
                                tm_GeneralSetting.ModifiedUser = model._UserId;

                                CONTEXT.Tm_GeneralSetting.Add(tm_GeneralSetting);
                                CONTEXT.SaveChanges();

                                String keyValue;
                                keyValue = tm_GeneralSetting.Id.ToString();
                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_GeneralSetting", "add", "Id", keyValue);
                            }
                            
                            
                            CONTEXT_TRANS.Commit();
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


        public GeneralSettingModel GetNewModel()
        {
            GeneralSettingModel model = new GeneralSettingModel();

            return model;
        }

        public GeneralSettingModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public GeneralSettingModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            GeneralSettingModel model = null;

            Tm_GeneralSetting tm_GeneralSetting = CONTEXT.Database.SqlQuery<Tm_GeneralSetting>("SELECT TOP 1 * FROM \"Tm_GeneralSetting\" ").SingleOrDefault();

            if (tm_GeneralSetting != null)
            {
                model = new GeneralSettingModel();
                CopyProperty.CopyProperties(tm_GeneralSetting, model, false);

                model.ListCoas_ = this.GeneralSetting_Coas(CONTEXT);
            }
            else
            {
                model = GetNewModel();
            }

            return model;
        }

        //-------------------------------------
        //Detail  GeneralSetting_CoaModel
        //-------------------------------------
        public List<GeneralSetting_AdjustmentCoaModel> GeneralSetting_Coas(HANA_APP CONTEXT)
        {
            string ssql = @"
                SELECT T0.*, T1.""AcctName"" AS ""AcctName""
                FROM ""{0}"".""@ADJUSTMENTTYPE"" AS T0
                INNER JOIN ""{0}"".""OACT"" AS T1 ON T0.""U_IDU_Account"" = T1.""AcctCode""
            ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<GeneralSetting_AdjustmentCoaModel>(ssql).ToList();
        }

        //---------------------------------------
        //GeneralSetting_CoaModel
        //--------------------------------------- 
        public long Detail_Add(HANA_APP CONTEXT, GeneralSetting_AdjustmentCoaModel model, int UserId)
        {
            long Id = 0;

            if (model != null)
            {

                Tm_GeneralSetting_Coa tm_GeneralSetting_Coa = new Tm_GeneralSetting_Coa();

                CopyProperty.CopyProperties(model, tm_GeneralSetting_Coa, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_GeneralSetting_Coa.CreatedDate = dtModified;
                tm_GeneralSetting_Coa.CreatedUser = UserId;
                tm_GeneralSetting_Coa.ModifiedDate = dtModified;
                tm_GeneralSetting_Coa.ModifiedUser = UserId;

                CONTEXT.Tm_GeneralSetting_Coa.Add(tm_GeneralSetting_Coa);
                CONTEXT.SaveChanges();
                Id = tm_GeneralSetting_Coa.Id;

            }

            return Id;

        }

        public void Detail_Update(HANA_APP CONTEXT, GeneralSetting_AdjustmentCoaModel model, int UserId)
        {
            if (model != null)
            {

                Tm_GeneralSetting_Coa tm_GeneralSetting_Coa = CONTEXT.Tm_GeneralSetting_Coa.Find(model.Code);

                if (tm_GeneralSetting_Coa != null)
                {
                    var exceptColumns = new string[] { "Code"};
                    CopyProperty.CopyProperties(model, tm_GeneralSetting_Coa, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_GeneralSetting_Coa.ModifiedDate = dtModified;
                    tm_GeneralSetting_Coa.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, GeneralSetting_AdjustmentCoaModel model)
        {
            //if (model.Code != null)
            //{
            //    if (model.Code != 0)
            //    {
            //        CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_GeneralSetting_Coa\"  WHERE \"Id\"=:p0", model.Code);

            //        CONTEXT.SaveChanges();
            //    }
            //}

        }
        
    }


    #endregion

}