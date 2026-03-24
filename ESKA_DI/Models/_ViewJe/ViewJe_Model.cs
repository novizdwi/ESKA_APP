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

namespace Models._ViewJe
{

    #region Models

    public class ViewJeMasterModel_View
    {
        //[Required(ErrorMessage = "required")]
        public int? TransId { get; set; }

        //[Required(ErrorMessage = "required")]
        public string DocType { get; set; }

        //[Required(ErrorMessage = "required")]
        public string DocNo { get; set; }

        //[Required(ErrorMessage = "required")]
        public string JeNo { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "required")]
        public string IsMultiCur { get; set; }



        //detail


        //[Required(ErrorMessage = "required")]
        public int? LineId { get; set; }

        //[Required(ErrorMessage = "required")]
        public string AccountCode { get; set; }

        //[Required(ErrorMessage = "required")]
        public string AccountName { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CurCode { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? DebitTc { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? CreditTc { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? DebitLc { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? CreditLc { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Project { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter1 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter2 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter3 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter4 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter5 { get; set; }

    }

    public class ViewJeMasterModel
    {
        //[Required(ErrorMessage = "required")]
        public int? TransId { get; set; }

        //[Required(ErrorMessage = "required")]
        public string DocType { get; set; }

        //[Required(ErrorMessage = "required")]
        public string DocNo { get; set; }

        //[Required(ErrorMessage = "required")]
        public string JeNo { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "required")]
        public string IsMultiCur { get; set; }

        public List<ViewJeDetailModel> Details_ { get; set; }
    }

    public class ViewJeDetailModel
    {

        //[Required(ErrorMessage = "required")]
        public int? LineId { get; set; }

        //[Required(ErrorMessage = "required")]
        public string AccountCode { get; set; }

        //[Required(ErrorMessage = "required")]
        public string AccountName { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CurCode { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? DebitTc { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? CreditTc { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? DebitLc { get; set; }

        //[Required(ErrorMessage = "required")]
        public decimal? CreditLc { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Project { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter1 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter2 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter3 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter4 { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CostCenter5 { get; set; }

    }

    #endregion

    #region Services

    public static class ViewJeModel
    {
        public static List<ViewJeMasterModel> GetMasterList(string code, long id)
        {
            using (var CONTEXT = new HANA_APP())
            {

                var models = CONTEXT.Database.SqlQuery<ViewJeMasterModel_View>("CALL \"Sp" + code + "_SapViewJe\"(" + id.ToString() + ") ")
                              .ToList() // Execute the query first
                              .GroupBy(q => q.TransId)
                              .Select(g => new ViewJeMasterModel
                              {
                                  TransId = g.Key,
                                  DocType = g.Select(b => b.DocType).FirstOrDefault(),
                                  DocNo = g.Select(b => b.DocNo).FirstOrDefault(),
                                  JeNo = g.Select(b => b.JeNo).FirstOrDefault(),
                                  Description = g.Select(b => b.Description).FirstOrDefault(),
                                  IsMultiCur = g.Select(b => b.IsMultiCur).FirstOrDefault(),
                                  Details_ = g.Select(b => new ViewJeDetailModel
                                  {
                                      LineId = b.LineId,
                                      AccountCode = b.AccountCode,
                                      AccountName = b.AccountName,
                                      CurCode = b.CurCode,
                                      DebitTc = b.DebitTc,
                                      CreditTc = b.CreditTc,
                                      DebitLc = b.DebitLc,
                                      CreditLc = b.CreditLc,
                                      Project = b.Project,
                                      CostCenter1 = b.CostCenter1,
                                      CostCenter2 = b.CostCenter2,
                                      CostCenter3 = b.CostCenter3,
                                      CostCenter4 = b.CostCenter4,
                                      CostCenter5 = b.CostCenter5
                                  }).ToList()
                              })
                              .ToList();

                return models;

            }
        }
    }
    #endregion

}