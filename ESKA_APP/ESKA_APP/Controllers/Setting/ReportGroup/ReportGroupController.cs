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
using Models.Setting.ReportGroup;

namespace Controllers.Setting
{
    public partial class ReportGroupController : BaseController
    {

        string VIEW_DETAIL = "ReportGroup";
        string VIEW_FORM_PARTIAL = "Partial/ReportGroup_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ReportGroup_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ReportGroup_Panel_List_Partial";

        ReportGroupService reportGroupService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(int Id = 0)
        {

            reportGroupService = new ReportGroupService();
            ReportGroupModel reportGroupModel;
            if (Id == 0)
            {

                reportGroupModel = reportGroupService.GetNewModel();
                reportGroupModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reportGroupService = new ReportGroupService();
                reportGroupModel = reportGroupService.GetById(Id);
                if (reportGroupModel != null)
                {
                    reportGroupModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, reportGroupModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            ReportGroupModel reportGroupModel;

            reportGroupService = new ReportGroupService();

            if (Id == 0)
            {
                reportGroupModel = reportGroupService.GetNewModel();
                reportGroupModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reportGroupModel = reportGroupService.GetById(Id);
                if (reportGroupModel != null)
                {
                    reportGroupModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ReportGroupModel reportGroupModel)
        {

            reportGroupModel._UserId = (int)Session["userId"];


            reportGroupService = new ReportGroupService();


            if (ModelState.IsValid)
            {
                reportGroupService.Add(reportGroupModel);
                reportGroupModel = reportGroupService.GetNewModel();
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }


            reportGroupModel._FormMode = Models.FormModeEnum.New;

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ReportGroupModel reportGroupModel)
        {

            reportGroupModel._UserId = (int)Session["userId"];

            reportGroupService = new ReportGroupService();
            reportGroupModel._FormMode = FormModeEnum.Edit;



            if (ModelState.IsValid)
            {
                reportGroupService.Update(reportGroupModel);
                reportGroupModel = reportGroupService.GetById(reportGroupModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id = 0)
        {

            reportGroupService = new ReportGroupService();

            if (Id != 0)
            {
                reportGroupService = new ReportGroupService();
                reportGroupService.DeleteById(Id);

            }

            ReportGroupModel reportGroupModel = new ReportGroupModel();
            return PartialView(VIEW_FORM_PARTIAL, reportGroupModel);
        }




    }



}