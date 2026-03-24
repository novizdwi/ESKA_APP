using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

using Models._Utils;
using Models;
using Models.Setting.AlertGroup;

namespace Controllers.Setting
{
    public partial class AlertGroupController : BaseController
    {
        public ActionResult ExportTo()
        {
            var hidden_CpGvFind_FilterExpression = Request["hidden_CpGvFind_FilterExpression"];
            var hidden_CpGvFind_SortExpression = Request["hidden_CpGvFind_SortExpression"];
            var hidden_CpGvFind_PageIndex =  Request["hidden_CpGvFind_PageIndex"];
            var hidden_CpGvFind_PageSize =  Request["hidden_CpGvFind_PageSize"];


            List<View___> views = AlertGroup__List_Model.GetDataList(hidden_CpGvFind_FilterExpression, hidden_CpGvFind_SortExpression, Convert.ToInt32(hidden_CpGvFind_PageIndex), Convert.ToInt32(hidden_CpGvFind_PageSize));
            return GridViewExportHelper.ExportTypes["XLS"].Method(GridViewExportHelper.ExportGridViewSettings, views);
             
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
            public static GridViewSettings ExportGridViewSettings
            {
                get
                {
                    if (exportGridViewSettings == null)
                        exportGridViewSettings = CreateExportGridViewSettings();
                    return exportGridViewSettings;
                }
            }


            static GridViewSettings CreateExportGridViewSettings()
            {


                GridViewSettings settings = AlertGroup__List_Model.CreateExportGridViewSettings();
                
             

                return settings;
            }
        }

	}
}