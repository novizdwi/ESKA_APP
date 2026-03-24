using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models._Cfl;

namespace Controllers._Cfl
{
    public partial class _CflPurchaseOrderController : BaseController
    {
        public ActionResult ExportTo()
        {
            int userId = (int)Session["userId"];

            var filterExpression = Request["hidden_CpGvFind_FilterExpression"];
            var sortExpression = Request["hidden_CpGvFind_SortExpression"];
            var pageIndex = Request["hidden_CpGvFind_PageIndex"];
            var pageSize = Request["hidden_CpGvFind_PageSize"];

            var cflPurchaseOrderParam = GetParam(Request);

            List<CflPurchaseOrder_View__> items = CflPurchaseOrder_Model.GetDataList(userId, cflPurchaseOrderParam, filterExpression, sortExpression, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
            return GridViewExportHelper.ExportTypes["XLS"].Method(GridViewExportHelper.ExportGridViewSettings(cflPurchaseOrderParam), items);

        }

        public enum GridViewExportType { None, Pdf, Xls, Xlsx, Rtf, Csv }

        public delegate ActionResult ExportMethod(GridViewSettings settings, object dataObject);

        public class ExportType
        {
            public string Title { get; set; }
            public ExportMethod Method { get; set; }
        }

        public partial class GridViewExportHelper
        {
            static Dictionary<string, ExportType> exportTypes;
            public static Dictionary<string, ExportType> ExportTypes
            {
                get
                {
                    if (exportTypes == null)
                        exportTypes = CreateExportTypes();
                    return exportTypes;
                }
            }
            static Dictionary<string, ExportType> CreateExportTypes()
            {
                Dictionary<string, ExportType> types = new Dictionary<string, ExportType>();
                types.Add("PDF", new ExportType { Title = "Export to PDF", Method = GridViewExtension.ExportToPdf });
                types.Add("XLS", new ExportType { Title = "Export to XLS", Method = GridViewExtension.ExportToXls });
                types.Add("XLSX", new ExportType { Title = "Export to XLSX", Method = GridViewExtension.ExportToXlsx });
                types.Add("RTF", new ExportType { Title = "Export to RTF", Method = GridViewExtension.ExportToRtf });
                types.Add("CSV", new ExportType { Title = "Export to CSV", Method = GridViewExtension.ExportToCsv });
                return types;
            }
        }


        //----------------------------

        public partial class GridViewExportHelper
        {
            static GridViewSettings exportGridViewSettings;
            public static GridViewSettings ExportGridViewSettings(CflPurchaseOrder_ParamModel cflPurchaseOrderParam)
            {
                //get
                //{
                if (exportGridViewSettings == null)
                    exportGridViewSettings = CreateExportGridViewSettings(cflPurchaseOrderParam);
                return exportGridViewSettings;
                //}
            }
            static GridViewSettings CreateExportGridViewSettings(CflPurchaseOrder_ParamModel cflPurchaseOrderParam)
            {

                GridViewSettings settings = CflPurchaseOrder_Model.CreateExportGridViewSettings(cflPurchaseOrderParam);


                return settings;
            }
        }

    }
}