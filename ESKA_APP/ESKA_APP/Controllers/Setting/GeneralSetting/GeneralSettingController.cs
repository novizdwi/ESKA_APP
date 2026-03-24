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

using Models.Setting.GeneralSetting;
using Models._Utils;

namespace Controllers.Setting
{

    public partial class GeneralSettingController : BaseController
    {

        string VIEW_DETAIL = "GeneralSetting";
        string VIEW_FORM_PARTIAL = "Partial/GeneralSetting_Form_Partial";

        GeneralSettingService generalSettingService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(int Id = 0)
        {


            GeneralSettingModel generalSettingModel;

            generalSettingService = new GeneralSettingService();
            generalSettingModel = generalSettingService.GetById(Id);


            return View(VIEW_DETAIL, generalSettingModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            GeneralSettingModel generalSettingModel;

            generalSettingService = new GeneralSettingService();


            generalSettingModel = generalSettingService.GetById(Id);


            return PartialView(VIEW_FORM_PARTIAL, generalSettingModel);
        }
         

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  GeneralSettingModel generalSettingModel)
        {
            generalSettingModel._UserId = (int)Session["userId"];
            generalSettingService = new GeneralSettingService();


            if (ModelState.IsValid)
            {
                generalSettingService.Update(generalSettingModel);
                generalSettingModel = generalSettingService.GetById(generalSettingModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, generalSettingModel);
        }


    }
}

//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DevExpress.Web.Mvc;
//using System.IO;
//using System.Threading;

//using DevExpress.Web.ASPxUploadControl;

//using System.Net;

//using Models;
//using Models.Setting.GeneralSetting;

//namespace Controllers.Setting
//{

//    public partial class GeneralSettingController : BaseController
//    {

//        string VIEW_DETAIL = "GeneralSetting";
//        string VIEW_FORM_PARTIAL = "Partial/GeneralSetting_Form_Partial";

//        GeneralSettingService generalSettingService;

//        public ActionResult Index()
//        {
//            return RedirectToAction("Detail");
//        }

//        public ActionResult Detail(int Id = 0)
//        {


//            GeneralSettingModel generalSettingModel;

//            generalSettingService = new GeneralSettingService();
//            generalSettingModel = generalSettingService.GetById(Id);

//            return View(VIEW_DETAIL, generalSettingModel);
//        }


//        public ActionResult DetailPartial(int Id = 0)
//        {

//            GeneralSettingModel generalSettingModel;

//            generalSettingService = new GeneralSettingService();


//            generalSettingModel = generalSettingService.GetById(Id);


//            return PartialView(VIEW_FORM_PARTIAL, generalSettingModel);
//        }



//        [HttpPost, ValidateInput(false)]
//        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  GeneralSettingModel generalSettingModel)
//        {
//            generalSettingModel._UserId = (int)Session["userId"];
//            generalSettingService = new GeneralSettingService();


//            if (ModelState.IsValid)
//            {
//                generalSettingService.Update(generalSettingModel);
//                generalSettingModel = generalSettingService.GetById(generalSettingModel.Id);
//            }
//            else
//            {
//                string message = GetErrorModel();

//                throw new Exception(string.Format("[VALIDATION] {0}", message));
//            } 

//            return PartialView(VIEW_FORM_PARTIAL, generalSettingModel);
//        }


//    }
//}