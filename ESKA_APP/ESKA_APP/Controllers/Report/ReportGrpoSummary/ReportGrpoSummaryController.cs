using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Models.Report;

namespace Controllers.Report
{
    public partial class ReportGrpoSummaryController : BaseController
    {
        string VIEW_DETAIL = "ReportGrpoSummary";
        string VIEW_CHART_PARTIAL = "Partial/ReportGrpoSummary_Chart_Partial";
        string VIEW_EMBED_PARTIAL = "~/Views/Report/ReportGrpoSummary/Partial/ReportGrpoSummary_Embed_Partial.cshtml";

        ReportGrpoSummaryService reportGrpoSummaryService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail()
        {
            int userId = (int)Session["userId"];

            // Default rentang: awal bulan berjalan s/d hari ini.
            DateTime dateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime dateTo = DateTime.Today;

            ViewBag.TransDate_From = dateFrom;
            ViewBag.TransDate_To = dateTo;

            // Chart tidak dirender saat awal — muncul setelah user klik Tampilkan (AJAX ChartPartial).
            return View(VIEW_DETAIL);
        }

        // Dipanggil dari halaman Custom Report (node GRPO Summary di grup Purchasing)
        [HttpPost, ValidateInput(false)]
        public ActionResult EmbedPartial()
        {
            // Default rentang: awal bulan berjalan s/d hari ini.
            ViewBag.TransDate_From = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.TransDate_To = DateTime.Today;

            return PartialView(VIEW_EMBED_PARTIAL);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ChartPartial(DateTime? TransDate_From, DateTime? TransDate_To)
        {
            reportGrpoSummaryService = new ReportGrpoSummaryService();
            List<GrpoStatusSummaryModel> model = reportGrpoSummaryService.GetStatusSummary(TransDate_From, TransDate_To);

            return PartialView(VIEW_CHART_PARTIAL, model);
        }
    }
}
