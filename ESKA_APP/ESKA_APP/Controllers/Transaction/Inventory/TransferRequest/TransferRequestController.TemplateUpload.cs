using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;

using System.Net;

using Models;
using System.Text;
using ESKA_APP;

using Newtonsoft.Json;
using Models.Transaction.Inventory;

namespace Controllers.Transaction.Inventory
{
    public partial class TransferRequestController : BaseController
    {
        string VIEW_TEMPLATEUPLOAD_PANEL_PARTIAL = "Partial/TemplateUpload/TemplateUpload_Panel_Partial";
        string VIEW_TEMPLATEUPLOAD_FORM_PARTIAL = "Partial/TemplateUpload/TemplateUpload_Form_Partial";

        public FileResult TemplateDownload(long Id = 0)
        {
            int userId = (int)Session["userId"];

            // Isi CSV (template/header)
            var csv = new StringBuilder();
            csv.AppendLine("TransDate (dd/mm/yyyy); FromWarehouse; ToWarehouse; Remarks");
            csv.AppendLine(";;;;");
            csv.AppendLine("ItemCode; Quantity; FreeText");
            csv.AppendLine(";;;");

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csv.ToString());

            return File(buffer, "text/csv", "Transfer Request Template.csv");
        }

        public ActionResult TemplateUpload_PopupListLoadOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ViewBag.Id = Id;
            return PartialView(VIEW_TEMPLATEUPLOAD_PANEL_PARTIAL);
        }

        public ActionResult Attachment_DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            ViewBag.Id = Id;
            return PartialView(VIEW_TEMPLATEUPLOAD_FORM_PARTIAL);
        }

        public ActionResult TemplateUpload()
        {
            int userId = (int)Session["userId"];
            var model = new TransferRequestModel();

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TemplateUploadFile()
        {
            int userId = (int)Session["userId"];
            var uploadedFiles = UploadControlExtension.GetUploadedFiles("UploadFile", CommonFileHelper.CsvValiadationSettings, CommonFileHelper.TransferRequestFileComplete);
            return new EmptyResult();
        }
    }
}