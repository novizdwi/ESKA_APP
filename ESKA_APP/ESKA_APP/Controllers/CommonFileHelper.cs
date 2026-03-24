using DevExpress.Web;
using Models._Utils;
using Models.Transaction.Inventory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;


namespace ESKA_APP
{
    public class CommonFileHelper
    {
        protected static string[] docExtensions = new string[] { ".xls", ".xlsx", ".doc", ".docx", ".pdf", ".txt", ".csv" };
        protected static string[] imgExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
        protected static string[] mailExtensions = new string[] { ".msg" };
        protected static string[] rptExtensions = new string[] {".rpt"};

        protected static string GetUploadDirectory()
        {
            string _BaseDirectory = Directory.GetDirectoryRoot(HostingEnvironment.MapPath("~/"));
            //string _BaseDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/")).Parent.Parent.FullName;
            var path = Path.Combine(_BaseDirectory, ConfigurationManager.AppSettings["UploadDirectory"], ConfigurationManager.AppSettings["WebName"], "Attachment");            
            return path;
        }
        protected static string GetRptDirectory()
        {
            string _BaseDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.Parent.Parent.FullName;
            return Path.Combine(_BaseDirectory, ConfigurationManager.AppSettings["UploadDirectory"], ConfigurationManager.AppSettings["WebName"], "Temp");
        }

        public static UploadControlValidationSettings RptValidationSettings = new UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".rpt" },
            MaxFileSize = 2097152
        };

        public static DevExpress.Web.UploadControlValidationSettings ValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xls", ".xlsx", ".doc", ".docx", ".pdf", ".txt", ".csv", ".jpg", ".jpeg", ".jpe", ".gif", ".png", ".msg" },
            //MaxFileSize = 6291456
            MaxFileSize = 2097152
        };

        public static DevExpress.Web.UploadControlValidationSettings CsvValiadationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".csv" },
            //MaxFileSize = 6291456
            MaxFileSize = 2097152

        };
        public static string GetFilePath(string FileName, string ModuleName, string extension)
        {
            string uploadDirectory = GetUploadDirectory();
            string fileTypePath = "";
            //if (Array.IndexOf(docExtensions, extension.ToLower()) > -1)
            //{
            //    fileTypePath = "docs";
            //}
            //}
            //else if (Array.IndexOf(imgExtensions, extension.ToLower()) > -1)
            //{
            //    fileTypePath = "img";
            //}

            Directory.CreateDirectory(Path.Combine(uploadDirectory, ModuleName, fileTypePath));
            var resultFilePath = Path.Combine(uploadDirectory, ModuleName, fileTypePath, FileName);
            return resultFilePath;
        }

        public static void FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.UploadedFile.IsValid)
            {
                e.CallbackData = "";
                return;
            }

            string fileName = Path.GetFileName(e.UploadedFile.FileName);

            // Kirim nama file ke client
            e.CallbackData = fileName;
        }

        public static void TransferRequestFileComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.UploadedFile.IsValid)
            {
                e.CallbackData = JsonConvert.SerializeObject(new { success = false, message = "Invalid file" });
                return;
            }

            try
            {
                using (var reader = new StreamReader(e.UploadedFile.FileContent))
                {
                    int lineNumber = 0;
                    TransferRequestTemplateHeader model = new TransferRequestTemplateHeader();
                    List<TransferRequestTemplateDetail> details = new List<TransferRequestTemplateDetail>();

                    var timeout = TimeSpan.FromSeconds(300);
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    while (!reader.EndOfStream)
                    {
                        if (sw.Elapsed > timeout)
                            throw new TimeoutException("Process reading file timeout.");

                        string line = reader.ReadLine();
                        lineNumber++;

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var columns = line.Split(';');

                        if (lineNumber == 2)
                        {
                            if (columns.Take(3).Any(c => string.IsNullOrWhiteSpace(c)))
                                throw new Exception("Invalid header empty value of: TransDate, FromWarehouse, or ToWarehouse");
                            if (!DateTime.TryParseExact(columns[0], "dd-MM-yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime transDate))
                                throw new Exception($"invalid date format: {columns[0]}");

                            model = new TransferRequestTemplateHeader
                            {
                                TransDate = transDate,
                                FromWhsCode = columns[1],
                                ToWhsCode = columns[2],
                                Comments = columns[3]
                            };

                            if (model.FromWhsCode == model.ToWhsCode)
                                throw new Exception("From warehouse must be different than To warehouse");

                            model.FromWhsName = GeneralGetList.GetWarehouseName(model.FromWhsCode);
                            model.ToWhsName = GeneralGetList.GetWarehouseName(model.ToWhsCode);

                        }
                        else if (lineNumber >= 4)
                        {
                            if (columns.Take(2).Any(c => string.IsNullOrWhiteSpace(c)))
                                throw new Exception("Invalid header empty value of ItemCode, or Quantity");
                            if (!decimal.TryParse(columns[1], out decimal qty))
                                throw new Exception($"Invalid quantity: {columns[1]} at line {lineNumber}");

                            var detail = new TransferRequestTemplateDetail
                            {
                                ItemCode = columns[0],
                                ItemName = GeneralGetList.GetItemName(columns[0]),
                                Quantity = qty,
                                QuantityOpen = GeneralGetList.GetItemWarehouseStock(columns[0], model.FromWhsCode),
                                Comments = columns.Length > 2 ? columns[2] : null
                            };
                            details.Add(detail);
                        }

                        model.Detail_ = details;
                    }

                    // sukses
                    e.CallbackData = JsonConvert.SerializeObject(new { success = true, message = "Success" , FileName = e.UploadedFile.FileName, Model = model});
                }
            }
            catch (Exception ex)
            {
                e.CallbackData = JsonConvert.SerializeObject(new { success = false, message = ex.Message });
            }
        }

        public static void DeleteFile(string ModuleName, string fileName)
        {
            var fileNameSplit = fileName.Split('.');
            string extension = fileNameSplit.Count() > 1 ? "." + fileNameSplit[1] : "";
            string fileTypePath = "";

            //if (Array.IndexOf(docExtensions, extension) > -1)
            //{
            //    fileTypePath = "docs";
            //}
            //else if (Array.IndexOf(imgExtensions, extension) > -1)
            //{
            //    fileTypePath = "img";
            //}

            string uploadDirectory = GetUploadDirectory();
            var resultFilePath = Path.Combine(uploadDirectory, ModuleName, fileTypePath, fileName);
            if (System.IO.File.Exists(resultFilePath))
                System.IO.File.Delete(resultFilePath);
        }
    }
}