<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rpt.aspx.cs" Inherits="ESKA_APP.Reports.Rpt" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>

</head>
<body>
<script src="../Scripts/jquery-2.2.0.min.js"></script>

<script type="text/javascript">

    //function RptPrint() {

    //    var dvReport = document.getElementById("dvReport");
    //    var frame1 = dvReport.getElementsByTagName("iframe")[0];
    //    if (navigator.appName.indexOf("Internet Explorer") != -1) {
    //        //alert('suhut');

    //        frame1.name = frame1.id;
    //        window.frames[frame1.id].focus();
    //        window.frames[frame1.id].print();


    //    }
    //    else {

    //        var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;

    //        frameDoc.print();


    //    }



    //}


</script>
<%--<input type="button" value="Print" onclick="RptPrint()" />--%>

<form id="form1" runat="server">
<div id="dvReport">

<CR:CrystalReportViewer ID="CrystalReport" runat="server" AutoDataBind="true" ToolPanelView="None" Height="50px" Width="350px" HasRefreshButton="True" ReuseParameterValuesOnRefresh="True" PrintMode="ActiveX" />

</div>
</form>
</body>
</html>

<script type="text/javascript">

    //jika bukan IE 
    //function GetIEVersion() {
    //    var sAgent = window.navigator.userAgent;
    //    var Idx = sAgent.indexOf("MSIE");

    //    // If IE, return version number.
    //    if (Idx > 0)
    //        return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

    //        // If IE 11 then look for Updated user agent string.
    //    else if (!!navigator.userAgent.match(/Trident\/7\./))
    //        return 11;

    //    else
    //        return 0; //It is not IE
    //}


    //if (GetIEVersion()<1) {
    //    $('#CrystalReport_toptoolbar_print').prop("onclick", null).attr("onclick", null);
    //    $('#CrystalReport_toptoolbar_print').click(function () {
    //        RptPrint();
    //    });
    //}

</script>
