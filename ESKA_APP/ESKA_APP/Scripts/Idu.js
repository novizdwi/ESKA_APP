
function GridviewInPopupAdjustSize(popup, gv, pheight) {
    if (pheight == null) {
        pheight = 0;
    }
    var height = popup.height - 90 - pheight;
    var width = popup.width - 30;
    gv.SetHeight(height);
    gv.SetWidth(width);
}

//function GridviewInPopupAdjustSize(popup, gv, pheight=0) {

//    var height = popup.height - 90 - pheight;
//    var width = popup.width - 30;
//    gv.SetHeight(height);
//    gv.SetWidth(width);
//}

function OnClickColumnLink(s, e, url, Id) {

    var guid = CreateGuid();
    window.open('', guid);
    SubmitForm(guid, url, 'post', {
        Id: Id
    });
}


//Generate form to Object
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

//Generate form to Object
function ToIduDateString(dt) {
    var dd = dt.getDate().toString();
    var mm = (dt.getMonth() + 1).toString(); //January is 0!

    var yyyy = dt.getFullYear().toString();
    var strDate = yyyy + '-' + mm + '-' + dd;
    return strDate;
}

Date.prototype.ToIduyyyymmdd = function () {
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();

    return [this.getFullYear(),
            (mm > 9 ? '' : '0') + mm,
            (dd > 9 ? '' : '0') + dd
    ].join('-');
};

//return an array of objects according to key, value, or key and value matching
function IsRowSelected(obj, value) {
    for (var i = 0; i < obj.length; i++) {
        if (obj[i] == value) {
            return true;
        }
    }
    return false;
}



//Get Gridview to json
function GetGvDetails(gvName) {

    var gv = ASPxClientGridView.Cast(gvName);

    var batchEditHelper = gv.GetBatchEditHelper();


    batchEditHelper.CanUpdate();


    var editState = batchEditHelper.GetEditState();

    var insertedRowValues = editState.insertedRowValues;

    var Inserts = new Array();
    $.each(insertedRowValues, function (name, value) {
        Inserts.push(value);
    });

    var modifiedRowValues = editState.modifiedRowValues;
    var modifieds = new Array();


    //tidak jadi di pakai
    //if (gvName == 'gvPaymentCustomerInvoiceDetail') {

    //    //untuk jika untuk input yg pilih invoice yg seperti di pilih invoice di payment sap
    //    //if ((gvName == 'gvPaymentCustomerInvoiceDetail') ||
    //    //(gvName == 'gvPpuInvoiceDetail') ||
    //    //(gvName == 'gvPpuKbDetail') || 
    //    //(gvName == 'gvPpuLgDetail')) {

    //    var keys = gv.GetSelectedKeysOnPage();

    //    $.each(modifiedRowValues, function (name, value) {
    //        var IsCheck_ = IsRowSelected(keys, name);

    //        $.extend(value, { IsCheck_: IsCheck_, Id: Id.GetValue() });
    //        modifieds.push(value);
    //    });
    //} 

    if (gvName == 'gvCreditCardMdrBranchPercentDetail') {
        $.each(modifiedRowValues, function (name, value) {
            $.extend(value, { BranchCode: name, Id: Id.GetValue() });
            modifieds.push(value);
        });
    } else if (gvName == 'gvUserAuthBranchDetail') {
        $.each(modifiedRowValues, function (name, value) {
            $.extend(value, { BranchCode: name, Id: Id.GetValue() });
            modifieds.push(value);
        });
    }

    else if (gvName == 'gvFakturPajakEditorListFakturPajakList') {
        $.each(modifiedRowValues, function (name, value) {
            $.extend(value, {
                Id: name,
                //FakturPajakReference: Object.values(value)[0],
                //FakturPajakType: Object.values(value)[0],
                //FakturPajakReplacement: Object.values(value)[1],
                //FakturPajakNo: Object.values(value)[2],
                //ProductName: Object.values(value)[3],
                //FakturDate: Object.values(value)[4],
                //BpCode: Object.values(value)[5],
                //BpName: Object.values(value)[6],
                //Npwp: Object.values(value)[7],
                //DppTc: Object.values(value)[8],
                //PpnTc: Object.values(value)[9],

                FakturPajakReference: value['Tp_FakturPajak___.FakturPajakReference'],
                FakturPajakType: value['Tp_FakturPajak___.FakturPajakType'],
                FakturPajakReplacement: value['Tp_FakturPajak___.FakturPajakReplacement'],
                FakturPajakNo: value['Tp_FakturPajak___.FakturPajakNo'],
                ProductName: value['Tp_FakturPajak___.ProductName'],
                FakturDate: value['Tp_FakturPajak___.FakturDate'],
                BpCode: value['Tp_FakturPajak___.BpCode'],
                BpName: value['Tp_FakturPajak___.BpName'],
                Npwp: value['Tp_FakturPajak___.Npwp'],
                NpwpAddress: value['Tp_FakturPajak___.NpwpAddress'],
                DppTc: value['Tp_FakturPajak___.DppTc'],
                PpnTc: value['Tp_FakturPajak___.PpnTc'],

            });
            modifieds.push(value);
        });
    }
    else {
        if (gv.keyName == 'DetId') {
            $.each(modifiedRowValues, function (name, value) {
                $.extend(value, { DetId: name, Id: Id.GetValue() });
                modifieds.push(value);
            });
        } else if (gv.keyName == 'DetDetId') {
            $.each(modifiedRowValues, function (name, value) {
                $.extend(value, { DetDetId: name, DetId: DetId.GetValue(), Id: Id.GetValue() });
                modifieds.push(value);
            });
        } else if (gv.keyName == 'Id') {
            $.each(modifiedRowValues, function (name, value) {
                $.extend(value, { Id: name });
                modifieds.push(value);
            });
        } else {
            $.each(modifiedRowValues, function (name, value) {
                $.extend(value, { DetId: name, Id: Id.GetValue() });
                modifieds.push(value);
            });
        }
    }

    var deletedRowKeys = editState.deletedRowKeys;
    var deletes = new Array();
    $.each(deletedRowKeys, function (name, value) {
        deletes.push(value);
    });

    var details = {};

    $.extend(details, { insertedRowValues: Inserts });
    $.extend(details, { modifiedRowValues: modifieds });
    $.extend(details, { deletedRowKeys: deletes });

    return details;
}


function GetGvRowCount(gv) {
    var jml = 0;

    var batchEditHelper = gv.GetBatchEditHelper();

    var rowsInserted = gv.batchEditHelper.GetEditState().insertedRowValues;
    var deletedRowKeys = gv.batchEditHelper.GetEditState().deletedRowKeys;

    var countInserted = Object.keys(rowsInserted).length;

    for (var i = 0; i < gv.keys.length; i++) {
        //setelah row di delete ternyata data yg lama masih tersimpan sehingga perlu cek.
        //sehingga apabila sudah pernah row di delete tidak ikut di perhitungan
        var b = isRowDelete(deletedRowKeys, gv.keys[i]);
        if (!b) {
            jml = jml + 1;

        }
    }

    //var rows = gv.batchEditHelper.GetEditState().insertedRowValues;
    for (var key in rowsInserted) {
        jml = jml + 1;
    }

    return jml;

}

function isRowDelete(deletedRowKeys, keyValue) {
    var result = false;
    $.each(deletedRowKeys, function (name, value) {
        if (value == keyValue) {
            result = true;
            return false;
        }
    });

    return result;
}

//Get Gridview to json
function GetGvCflTicketReservationDetails(gvName) {

    var gv = ASPxClientGridView.Cast(gvName);

    var batchEditHelper = gv.GetBatchEditHelper();

    batchEditHelper.CanUpdate();

    var editState = batchEditHelper.GetEditState();

    var modifiedRowValues = editState.modifiedRowValues;
    var modifieds = new Array();

    var keys = gv.GetSelectedKeysOnPage();

    $.each(modifiedRowValues, function (name, value) {
        if (IsRowSelected(keys, name)) {
            modifieds.push({ ResvDetId: name, QtyInvoice: value['InputQtyInvoice___.QtyInvoice_'] });
        }
    });

    var details = {};

    $.extend(details, { modifiedRowValues: modifieds });

    return modifieds;
}

/*
untuk dynamic submit form
cara pemakaian :
1. muncul windows baru :
            window.open('', 'test01');  
            myFunction('test01', '@Url.Action("Detail", "SeriesTourCode")', 'post', {
                param: 1 
            });
2. tidak muncul windows baru:  
            myFunction('', '@Url.Action("Detail", "SeriesTourCode")', 'post', {
                param: 1 
            });

*/

function SubmitForm(target, action, method, input) {
    'use strict';
    var form;
    form = $('<form />', {
        target: target,
        action: action,
        method: method,
        style: 'display: none;'
    });
    if (typeof input !== 'undefined' && input !== null) {
        $.each(input, function (name, value) {
            $('<input />', {
                type: 'hidden',
                name: name,
                value: value
            }).appendTo(form);
        });
    }
    form.appendTo('body').submit();
    form.remove();
}





function CreateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}


function GvExportTo(gvName, urlAction) {

    SubmitForm('', urlAction, 'post', {
        hidden_CpGvFind_FilterExpression: gvName.cpGvFind_FilterExpression,
        hidden_CpGvFind_SortExpression: gvName.cpGvFind_SortExpression,
        hidden_CpGvFind_PageIndex: gvName.cpGvFind_PageIndex,
        hidden_CpGvFind_PageSize: gvName.cpGvFind_PageSize
    });

}

function GvCflExportTo(gvName, urlAction) {

    SubmitForm('', urlAction, 'post', {
        hidden_CpGvFind_FilterExpression: gvName.cpGvFind_FilterExpression,
        hidden_CpGvFind_SortExpression: gvName.cpGvFind_SortExpression,
        hidden_CpGvFind_PageIndex: gvName.cpGvFind_PageIndex,
        hidden_CpGvFind_PageSize: gvName.cpGvFind_PageSize,
        hidden_CflType: gvName.cpGvFind_CflType,
        hidden_CflName: gvName.cpGvFind_CflName,
        hidden_CflHeader: gvName.cpGvFind_CflHeader,
        hidden_CflSqlWhere: gvName.cpGvFind_CflSqlWhere,
        hidden_CflIsMulti: gvName.cpGvFind_CflIsMulti

    });

}


function GvCflDynamicExportTo(gvName, urlAction) {

    SubmitForm('', urlAction, 'post', {
        hidden_CpGvFind_FilterExpression: gvName.cpGvFind_FilterExpression,
        hidden_CpGvFind_SortExpression: gvName.cpGvFind_SortExpression,
        hidden_CpGvFind_PageIndex: gvName.cpGvFind_PageIndex,
        hidden_CpGvFind_PageSize: gvName.cpGvFind_PageSize,
        hidden_CpGvFind_CflDynamicCode: gvName.cpGvFind_CflDynamicCode,
        hidden_CpGvFind_CflDynamicDescription: gvName.cpGvFind_CflDynamicDescription,
        hidden_CpGvFind_CflDynamicSql: gvName.cpGvFind_CflDynamicSql
    });

}

function PrintLayout(s, e, Layout_Id, Layout_OutputType, urlAction) {
    if (Layout_OutputType == 'Excel') {
        SubmitForm('', urlAction, 'post', {
            Id: Id.GetValue(),
            Layout_Id: Layout_Id
        });
    } else if (Layout_OutputType == 'ExcelDataOnly') {
        SubmitForm('', urlAction, 'post', {
            Id: Id.GetValue(),
            Layout_Id: Layout_Id
        });
    } else if (Layout_OutputType == 'Csv') {
        SubmitForm('', urlAction, 'post', {
            Id: Id.GetValue(),
            Layout_Id: Layout_Id
        });
    } else if (Layout_OutputType == 'Text') {
        SubmitForm('', urlAction, 'post', {
            Id: Id.GetValue(),
            Layout_Id: Layout_Id
        });
    } else {
        var guid = CreateGuid();
        window.open('', guid);
        SubmitForm(guid, urlAction, 'post', {
            Id: Id.GetValue(),
            Layout_Id: Layout_Id
        });
    }
}

function PrintLayoutBpkCheck(s, e, Layout_Id, Layout_OutputType, urlAction, CheckDetId) {
    if (Layout_OutputType == 'Excel') {
        SubmitForm('', urlAction, 'post', {
            Id: CheckDetId,
            Layout_Id: Layout_Id
        });
    } else if (Layout_OutputType == 'ExcelDataOnly') {
        SubmitForm('', urlAction, 'post', {
            Id: CheckDetId,
            Layout_Id: Layout_Id
        });
    } else if (Layout_OutputType == 'Csv') {
        SubmitForm('', urlAction, 'post', {
            Id: CheckDetId,
            Layout_Id: Layout_Id
        });
    } else if (Layout_OutputType == 'Text') {
        SubmitForm('', urlAction, 'post', {
            Id: CheckDetId,
            Layout_Id: Layout_Id
        });
    } else {
        var guid = CreateGuid();
        window.open('', guid);
        SubmitForm(guid, urlAction, 'post', {
            Id: CheckDetId,
            Layout_Id: Layout_Id
        });
    }
}

//return an array of objects according to key, value, or key and value matching
function GetDataFromJson(obj, key, val) {
    for (var i = 0; i < obj.length; i++) {
        if (obj[i][key] == val) {
            return obj[i];
        }
    }
    return null;
}



function GetDataJson(action, method, functionName, input) {

    $.ajax({
        type: method,
        url: action,
        data: input,
        beforeSend: function () {
            OnBegin();
        },
        success: function (response) {
            var strFun = functionName;
            var strParam = response;
            //Create the function
            var fn = window[strFun];

            //Call the function
            fn(strParam);

        },
        error: function (jqXhr, textStatus, errorThrown) {
            OnFailure(jqXhr, textStatus, errorThrown)
        },
        complete: function () {
            OnComplete()
        }

    });;
}

/*
    Obj : yg akan di set rate nya
    FunctionName : function yg akan di panggil ketika sukses
*/
function GetRateCurr(url, curCode, rateDate, FunctionName) {

    if (curCode != '') {
        if (curCode == 'IDR') {

            var strFun = FunctionName;
            var strParam = 1;
            //Create the function
            var fn = window[strFun];

            //Call the function
            fn(strParam);

        }
        else {
            var data = { CurCode: curCode, RateDate: rateDate };
            GetDataJson(url, "POST", FunctionName, data);

        }
    }

    return;
}


function RefreshValidationNotice(formName) { //"#formDetail"
    //kalau sebelumnya sudah tampil validation notice maka akan menampikkan notif
    var validator = $(formName).validate();
    var errors = validator.numberOfInvalids();
    if (errors) {
        $(formName).valid();
    }
}



function ComboboxHideColumn(comboxName) {

    var isChrome = !!window.chrome && !!window.chrome.webstore;
    if (!isChrome) {
        var test = ASPxClientComboBox.Cast(comboxName);
        test.HideDropDown();
        //alert('test');

        $("#" + comboxName + "_DDD_L_H tbody tr td").each(function () {
            if (this.attributes.style.value == 'width:0px;') {
                $(this).css('display', 'none');
                //this.attributes.style.value = "width:0px; display:none;";
            }
        });


        $("#" + comboxName + "_DDD_L_LBT tbody tr td").each(function () {
            if (this.attributes.style.value == 'width:0px;') {
                $(this).css('display', 'none');
                //this.attributes.style.value = "width:0px; display:none;";
            }
        });

        test.ShowDropDown();
    }



}


/**
 * Number.prototype.format(n, x, s, c)
 * 
 * @param integer n: length of decimal
 * @param integer x: length of whole part
 * @param mixed   s: sections delimiter
 * @param mixed   c: decimal delimiter
 */
Number.prototype.format = function (n, x, s, c) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
};

function strStartsWith(str, prefix) {
    return str.indexOf(prefix) === 0;
}

function OnClickBtnPrintNotif(s, e, url) {

    $.ajax({
        type: "POST",
        url: url,
        data: { Id: Id.GetValue() },
        beforeSend: function () {
            OnBegin();
        },
        success: function (response) {
            if (!response) {
                PostTrans('Y');
            } else {
                popupPrint.ShowAtPos(250, 100);
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {

            OnFailure(jqXhr, textStatus, errorThrown)


        },
        complete: function () {
            OnComplete()
        }
    });

}


function ParseDateEdit(s, e) {
    var strDate = e.value.toString().trim();

    if (strDate != "") {
        if (!(/[^0-9]/.test(strDate))) {
            var today = new Date();
            var dd = today.getDate().toString();
            var mm = (today.getMonth() + 1).toString(); //January is 0!

            var yyyy = today.getFullYear().toString();


            var dt = new Date(yyyy + '-' + mm + '-' + dd);

            if (strDate.length == 1) {
                dd = strDate;
            }
            else if (strDate.length == 2) {
                if (!isNaN(Date.parse(yyyy + '-' + mm + '-' + strDate))) {
                    dd = strDate;

                } else {
                    dd = strDate[0];
                    mm = strDate[1];
                }
            }

            else if (strDate.length == 3) {
                if (!isNaN(Date.parse(yyyy + '-' + strDate[2] + '-' + strDate[0] + strDate[1]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2];

                }
                else if (!isNaN(Date.parse(yyyy + '-' + strDate[1] + strDate[2] + '-' + strDate[0]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2];
                }
                else if (!isNaN(Date.parse(yyyy + '-' + strDate[1] + '-' + strDate[0]))) {
                    dd = strDate[0];
                    mm = strDate[1];

                }
            }

            else if (strDate.length == 4) {
                if (!isNaN(Date.parse(yyyy + '-' + strDate[2] + strDate[3] + '-' + strDate[0] + strDate[1]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2] + strDate[3];

                }
                else if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + strDate[2] + strDate[3] + '-' + strDate[1] + '-' + strDate[0]))) {
                    dd = strDate[0];
                    mm = strDate[1];
                    yyyy = yyyy[0] + yyyy[1] + strDate[2] + strDate[3];
                }
            }

            else if (strDate.length == 5) {
                if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + yyyy[2] + strDate[4] + '-' + strDate[2] + strDate[3] + '-' + strDate[0] + strDate[1]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2] + strDate[3];
                    yyyy = yyyy[0] + yyyy[1] + yyyy[2] + strDate[4];
                }
            }
            else if (strDate.length == 6) {
                if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + strDate[4] + strDate[5] + '-' + strDate[2] + strDate[3] + '-' + strDate[0] + strDate[1]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2] + strDate[3];
                    yyyy = yyyy[0] + yyyy[1] + strDate[4] + strDate[5];
                }
            }

            else if (strDate.length == 7) {
                if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + strDate[4] + strDate[5] + '-' + strDate[2] + strDate[3] + '-' + strDate[0] + strDate[1]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2] + strDate[3];
                    yyyy = yyyy[0] + yyyy[1] + strDate[4] + strDate[5];
                }
            }

            else if (strDate.length == 8) {
                if (!isNaN(Date.parse(strDate[4] + strDate[5] + strDate[6] + strDate[7] + '-' + strDate[2] + strDate[3] + '-' + strDate[0] + strDate[1]))) {
                    dd = strDate[0] + strDate[1];
                    mm = strDate[2] + strDate[3];
                    yyyy = strDate[4] + strDate[5] + strDate[6] + strDate[7];
                }
            }

            dt = new Date(yyyy + '-' + mm + '-' + dd);
            s.SetValue(dt);
        }
    }
}

function ParseMonthEdit(s, e) {
    var strDate = e.value.toString().trim();

    if (strDate != "") {
        if (!(/[^0-9]/.test(strDate))) {
            var today = new Date();
            var dd = today.getDate().toString();
            var mm = (today.getMonth() + 1).toString(); //January is 0!

            var yyyy = today.getFullYear().toString();


            var dt = new Date(yyyy + '-' + mm + '-' + dd);

            if (strDate.length == 1) {
                mm = strDate;
            }
            else if (strDate.length == 2) {
                if (!isNaN(Date.parse(yyyy + '-' + strDate + '-' + dd))) {
                    mm = strDate;

                } else {
                    mm = strDate[0];
                }
            }

            else if (strDate.length == 3) {
                if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + strDate[1] + strDate[2] + '-' + strDate[0] + '-' + dd))) {
                    mm = strDate[0];
                    yyyy = yyyy[0] + yyyy[1] + strDate[1] + strDate[2];
                }
                else if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + yyyy[2] + strDate[2] + '-' + strDate[0] + strDate[1] + '-' + dd))) {
                    mm = strDate[0] + strDate[1];
                    yyyy = yyyy[0] + yyyy[1] + yyyy[2] + strDate[2];
                }
                else if (!isNaN(Date.parse(yyyy[0] + strDate[0] + strDate[1] + strDate[2] + '-' + mm + '-' + dd))) {
                    yyyy = yyyy[0] + strDate[0] + strDate[1] + strDate[2];
                }
            }

            else if (strDate.length == 4) {
                if (!isNaN(Date.parse(yyyy[0] + yyyy[1] + strDate[2] + strDate[3] + '-' + strDate[0] + strDate[1] + '-' + dd))) {
                    mm = strDate[0] + strDate[1];
                    yyyy = yyyy[0] + yyyy[1] + strDate[2] + strDate[3];
                }

            }



            dt = new Date(yyyy + '-' + mm + '-' + dd);
            s.SetValue(dt);
        }
    }
}