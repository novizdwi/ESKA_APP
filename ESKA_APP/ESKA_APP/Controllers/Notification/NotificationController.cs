using Models._Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBS_APP.Controllers.Notification
{
    public class NotificationController : Controller
    {
        // dipanggil saat popup pertama kali muncul / refresh
        //public ActionResult Popup_OnDemand()
        //{
        //    return PartialView("~/Views/Notification/Notification.cshtml");
        //}

        //public ActionResult Popup_OnDemand()
        //{
        //    return PartialView("~/Views/Notification/Partial/Notification_PopupContent_Partial.cshtml");
        //}

        //// load grid pertama kali
        //public ActionResult Grid(string search = "")
        //{
        //    var data = GetDummyData(search);
        //    return PartialView("~/Views/Notification/Partial/Notification_Grid_Partial.cshtml", data);
        //}

        //// paging / refresh grid
        //public ActionResult GridCallback(string search = "")
        //{
        //    var data = GetDummyData(search);
        //    return PartialView("~/Views/Notification/Partial/Notification_Grid_Partial.cshtml", data);
        //}

        //// DATA SEMENTARA (biar popup muncul dulu)
        //private List<NotifApprovalDto> GetDummyData(string search)
        //{
        //    var list = new List<NotifApprovalDto>
        //{
        //    new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },new NotifApprovalDto{ TransNo="ITO-001", TransType = "Transfer Out" ,RequestDate=DateTime.Now, Message="Waiting Approval" },
        //    new NotifApprovalDto{ TransNo="ITI-002", TransType = "Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSO-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //    new NotifApprovalDto{ TransNo="ITSI-002", TransType = "Summarry Transfer IN" , RequestDate=DateTime.Now.AddDays(-1), Message="Approved" },
        //};

        //    if (!string.IsNullOrEmpty(search))
        //        list = list.FindAll(x => x.TransNo.Contains(search));

        //    return list;
        //}
    }
    
}