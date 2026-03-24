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
using Models.Transaction;
using Models._Utils; 
using DevExpress.Web.ASPxHtmlEditor.Internal;
using ESKA_APP;

namespace Controllers.Transaction.Adjustment
{
    public partial class AdjustmentOutController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/AdjustmentOut_Form_TabAttachment_List_Partial";
        string moduleName = "AdjustmentOut";

        public ActionResult TabAttachmentListPartial()
        {
            int userId = (int)Session["userId"];

            adjustmentOutService = new AdjustmentOutService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelList = adjustmentOutService.GetAdjustmentOut_Attachments(Id);

            return PartialView(VIEW_TAB_ATTACHMENT, modelList);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        {
            int userId = (int)Session["userId"];

            adjustmentOutService = new AdjustmentOutService();

            var Id = Convert.ToInt64(Request["cbId"]);
            try
            {

                var model = adjustmentOutService.GetAdjustmentOut_Attachments_GetById(detId);
                if (model != null)
                {
                    adjustmentOutService.Attachment_Delete(model);
                    CommonFileHelper.DeleteFile(moduleName, model.Guid + "_" + model.FileName);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorDesc = ex.Message;
            }


            var modelList = adjustmentOutService.GetAdjustmentOut_Attachments(Id);
            return PartialView(VIEW_TAB_ATTACHMENT, modelList);
        }
    }


}