using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Collections;

using System.Web.Mvc;
using System.Web.Script.Serialization;

using DevExpress.Web.Mvc;
using Models._CrystalReport;
using Models._Utils;

namespace ESKA_APP.Reports
{
    public partial class Rpt : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ReportDocument rptDoc;
                rptDoc = Models._Utils.Rpt.GetRptDoc(Request);

                if (rptDoc != null)
                {

                    CrystalReport.ReportSource = rptDoc;
                    Session["ReportDocument"] = rptDoc;
                }
            }
            else
            {
                ReportDocument rptDoc = (ReportDocument)Session["ReportDocument"];
                CrystalReport.ReportSource = rptDoc;
            }

        }

    }
}