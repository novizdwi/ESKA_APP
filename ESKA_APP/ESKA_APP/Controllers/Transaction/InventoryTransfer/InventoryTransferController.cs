using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction;
using Newtonsoft.Json;

namespace Controllers.Transaction
{
    public partial class InventoryTransferController : BaseController
    {
        string VIEW_DETAIL = "InventoryTransfer";
        string VIEW_FORM_PARTIAL = "Partial/InventoryTransfer_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/InventoryTransfer_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/InventoryTransfer_Panel_List_Partial";


        InventoryTransferService inventoryTransferService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            inventoryTransferService = new InventoryTransferService();
            InventoryTransferModel inventoryTransferModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                inventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                inventoryTransferModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryTransferService = new InventoryTransferService();
                inventoryTransferModel = inventoryTransferService.GetById(userId, Id);
                inventoryTransferModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, inventoryTransferModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            InventoryTransferModel InventoryTransferModel;

            inventoryTransferService = new InventoryTransferService();
            if (Id == 0)
            {
                InventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                InventoryTransferModel._FormMode = FormModeEnum.New;
            }
            else
            {
                InventoryTransferModel = inventoryTransferService.GetById(userId, Id);
                if (InventoryTransferModel != null)
                {
                    InventoryTransferModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    InventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                    InventoryTransferModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryTransferModel InventoryTransferModel)
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel._UserId = (int)Session["userId"];
            inventoryTransferService = new InventoryTransferService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventoryTransferService.Add(InventoryTransferModel);
                InventoryTransferModel = inventoryTransferService.GetById(userId, Id);
                InventoryTransferModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryTransferModel InventoryTransferModel)
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel._UserId = (int)Session["userId"];
            inventoryTransferService = new InventoryTransferService();
            InventoryTransferModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            inventoryTransferService.Update(InventoryTransferModel);
            InventoryTransferModel = inventoryTransferService.GetById(userId, InventoryTransferModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data, string Sorting)
        {
            int userId = (int)Session["userId"];

            inventoryTransferService = new InventoryTransferService();
            var result = inventoryTransferService.ChooseItem(userId, Id, Data, Sorting);

            return Content(result.ToString());
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryTransferModel InventoryTransferModel)
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel._UserId = (int)Session["userId"];
            inventoryTransferService = new InventoryTransferService();
            InventoryTransferModel._FormMode = FormModeEnum.Edit;
            
            inventoryTransferService.Post(userId, InventoryTransferModel);
            InventoryTransferModel = inventoryTransferService.GetById(userId, InventoryTransferModel.Id);

            if (InventoryTransferModel != null)
            {
                InventoryTransferModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                InventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                InventoryTransferModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel InventoryTransferModel;

            inventoryTransferService = new InventoryTransferService();
            inventoryTransferService.Cancel(userId, Id, CancelReason);

            InventoryTransferModel = inventoryTransferService.GetById(userId, Id);
            if (InventoryTransferModel != null)
            {
                InventoryTransferModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                InventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                InventoryTransferModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, InventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel inventoryTransferModel;

            inventoryTransferService = new InventoryTransferService();
            inventoryTransferService.RequestApproval(userId, id, templateId, approvalMessage);

            inventoryTransferModel = inventoryTransferService.GetById(userId, id);
            if (inventoryTransferModel != null)
            {
                inventoryTransferModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                inventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                inventoryTransferModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel inventoryTransferModel;

            inventoryTransferService = new InventoryTransferService();
            inventoryTransferService.Approve(userId, Id, ApprovalMessage);

            inventoryTransferModel = inventoryTransferService.GetById(userId, Id);
            if (inventoryTransferModel != null)
            {
                inventoryTransferModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                inventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                inventoryTransferModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryTransferModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            InventoryTransferModel inventoryTransferModel;

            inventoryTransferService = new InventoryTransferService();
            inventoryTransferService.Authorize(userId, Id, "Reject", ApprovalMessage);

            inventoryTransferModel = inventoryTransferService.GetById(userId, Id);
            if (inventoryTransferModel != null)
            {
                inventoryTransferModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                inventoryTransferModel = inventoryTransferService.GetNewModel(userId);
                inventoryTransferModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryTransferModel);
        }

        [HttpPost]
        public ActionResult BatchSaveAndAddRow(string rows, long Id, long detId)
        {
            List<InventoryTransferBatchModel> batchList = new List<InventoryTransferBatchModel>();
            if (!string.IsNullOrEmpty(rows))
            {
                batchList = JsonConvert.DeserializeObject<List<InventoryTransferBatchModel>>(rows);
            }
            
            batchList.Add(new InventoryTransferBatchModel()
            {
                Id = Id,
                DetId = detId, 
                Batch = "",
                Quantity = 0,
                AdmissionDate = null,
                Netto = 0,
            }); 

            Session["BatchList"] = batchList;
            return Json(new{Result = true, TotalRow = batchList.Count});
        }

    }
}