using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models; 

using System.Net; 
using Models.Transaction.Adjustment;
using Models._Utils;
using System.IO;
using ESKA_DI.Models._EF;
using ESKA_APP;

namespace Controllers.Transaction.Adjustment
{
    public partial class AdjustmentInController : BaseController
    {
        string VIEW_ATTACHMENT_PANEL_PARTIAL = "Partial/Attachment/Attachment_Panel_Partial";
        string VIEW_ATTACHMENT_FORM_PARTIAL = "Partial/Attachment/Attachment_Form_Partial";

        public ActionResult Attachment_PopupListLoadOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ViewBag.Id = Id;
            return PartialView(VIEW_ATTACHMENT_PANEL_PARTIAL);
        }

        public ActionResult Attachment_DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            ViewBag.Id = Id;
            return PartialView(VIEW_ATTACHMENT_FORM_PARTIAL);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Attachment_Upload()
        {
            int userId = (int)Session["userId"];

            var Id = Request["Id"];

            //var UploadMultiFile = UploadControlExtension.GetUploadedFiles("UploadMultiFile", AdjustmentInUploadControlHelper.ValidationSettings, AdjustmentInUploadControlHelper.FileUploadComplete);
            var UploadMultiFile = UploadControlExtension.GetUploadedFiles("UploadMultiFile", CommonFileHelper.ValidationSettings, CommonFileHelper.FileUploadComplete);


            if (UploadMultiFile != null)
            {
                for (int i = 0; i < UploadMultiFile.Length; i++)
                {
                    if (!UploadMultiFile[i].IsValid)
                    {
                        return null;
                    }
                }

                adjustmentInService = new AdjustmentInService();

                List<AdjustmentIn_AttachmentModel> listModel = new List<AdjustmentIn_AttachmentModel>();

                for (int i = 0; i < UploadMultiFile.Length; i++)
                {
                    if (UploadMultiFile[i].FileBytes.Length > 0 && UploadMultiFile[i].IsValid)
                    {

                        var guid = Guid.NewGuid().ToString();

                        AdjustmentIn_AttachmentModel model = new AdjustmentIn_AttachmentModel();
                        model.Id = long.Parse(Id);
                        model.FileName = UploadMultiFile[i].FileName;
                        model.Guid = guid;
                        model._UserId = (int)Session["userId"];
                        model.FileIndex_ = i;
                        listModel.Add(model);
                    }
                }

                adjustmentInService.Detail_Add(listModel);

                for (int i = 0; i < listModel.Count; i++)
                {
                    var fileNameSplit = listModel[i].FileName.Split('.');
                    string fileExt = fileNameSplit.Count() > 1 ? "." + fileNameSplit[1] : "";
                    string strFilename = CommonFileHelper.GetFilePath(listModel[i].Guid + "_" + listModel[i].FileName, moduleName, fileExt);

                    //string strFilename = AdjustmentInUploadControlHelper.GetFilePath(listModel[i].Guid + "_" + listModel[i].FileName);

                    UploadMultiFile[listModel[i].FileIndex_].SaveAs(strFilename);

                }

            }



            return null;
        }


        public FileResult Attachment_Download(int DetId = 0)
        {
            int userId = (int)Session["userId"];

            using (var CONTEXT = new HANA_APP()) {
                Tx_AdjustmentIn_Attachment model = CONTEXT.Tx_AdjustmentIn_Attachment.Find(DetId);
                if (model != null)
                {
                    string fileName = model.FileName;
                    var fileNameSplit = fileName.Split('.');
                    string fileExt = fileNameSplit.Count() > 1 ? "." + fileNameSplit[1] : "";

                    //return File(AdjustmentInUploadControlHelper.GetFilePath(model.Guid + "_" + model.FileName), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    return File(CommonFileHelper.GetFilePath(model.Guid + "_" + model.FileName, moduleName, fileExt), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
            return null;

        }



    }



    //public class AdjustmentInUploadControlHelper
    //{
    //    public const string UploadDirectory = "~/Content/Attachment/AdjustmentIn/";

    //    public static DevExpress.Web.ASPxUploadControl.ValidationSettings ValidationSettings = new DevExpress.Web.ASPxUploadControl.ValidationSettings()
    //    {
    //        AllowedFileExtensions = new string[] { ".xls", ".xlsx", ".doc", ".docx", ".pdf", ".txt", ".csv", ".jpg", ".jpeg", ".jpe", ".gif", ".png", ".msg" },
    //        MaxFileSize = 2097152
    //    };

    //    public static string GetFilePath(string FileName)
    //    {
    //        var resultFilePath = UploadDirectory + FileName;
    //        return HttpContext.Current.Request.MapPath(resultFilePath);
    //    }

    //    public static void FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    //    {
    //        if (e.UploadedFile.IsValid)
    //        {
    //            e.CallbackData = "mantap";
    //        }
    //        else
    //        {
    //            e.CallbackData = "";
    //        }

    //    }
    //}


}