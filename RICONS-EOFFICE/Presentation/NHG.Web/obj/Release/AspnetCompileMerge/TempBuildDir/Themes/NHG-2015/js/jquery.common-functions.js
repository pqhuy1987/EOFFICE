/*Kiem tra ngay thang hien tai theo dinh dang dd/MM/yyyy*/
//$(this).prop("scrollHeight") - $(this).height() === get scroll height
var slTrangHienThi = 28;
function chkDt(obj) {
    dtFormat = 'dd/MM/yyyy';
    udt = $(obj).val();
    if (udt.indexOf("/") === -1) {
        alert('Not a valid date, format ' + dtFormat);
        $(obj).focus();
        return false;
    }
    dt1 = udt.split("/");
    dd1 = parseInt(dt1[0],0);
    mm1 = parseInt(dt1[1]);
    yy1 = parseInt(dt1[2]);
    if (isNaN(dd1) || isNaN(mm1) || isNaN(yy1)) {
        alert('Invalid Date !');
        return false;
    }
    dt2 = new Date(mm1 + '/' + dd1 + '/' + yy1);
    dd2 = dt2.getDate();
    mm2 = dt2.getMonth() + 1;
    yy2 = dt2.getFullYear();

    //alert(dd1+'/'+mm1+'/'+yy1);
    //alert(dd2+'/'+mm2+'/'+yy2);
    //check
    if (dd1 === dd2 && mm1 === mm2 && yy1 === yy2)
        alert('Valid Date !');
    else
        alert('Invalid Date !');
}
/*kiem tra ngay thang*/
/*starday endday
  starday > endday return true: 
  starday <= endday return false*/
function check_over_date(startday, endday) {
    var dstart = new Date(startday.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"));
    var dend = new Date(endday.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"));
    if (dstart > dend)
        return true;
    else
        return false;
}
function CheckNullOrEmpty(strInput) {
    if (jQuery.type(strInput) === "undefined")
        return true;
    if (String(strInput).replace(/\s/g, '').length === 0)
        return true;
    if (strInput === null)
        return true;
    if (strInput.length === 0)
        return true;
    return false;
}
//ham loc dau tieng viec
function locdau(strInput, isKyTuDacBiet) {
    var str = strInput; // lấy chuỗi dữ liệu nhập vào
    str = str.toLowerCase(); // chuyển chuỗi sang chữ thường để xử lý
    /* tìm kiếm và thay thế tất cả các nguyên âm có dấu sang không dấu*/
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    if (isKyTuDacBiet) {
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
        /* tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự - */
        str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
        str = str.replace(/^\-+|\-+$/g, ""); //cắt bỏ ký tự - ở đầu và cuối chuỗi
    }
    return str; // xuất kết quả xữ lý ra
}
//ham kiem tra nhap so tu ban phim
function validateNumberKeyPress(event) {
    var key = window.event ? event.keyCode : event.which;
    if (key === 44 || key === 8 || key === 0 ||
        (event.ctrlKey && key === 99) ||
        (event.ctrlKey && key === 97) ||
        (event.ctrlKey && key === 118)) {
        return true;
    }
    else if (key < 48 || key > 57) {
        return event.preventDefault();
    }
    else return true;
}
function validateNumber(strInput) {
    if (!isNaN(parseInt(strInput)))
        return true;
    else
        return false;
}
//domain.com/index.aspx?option=abc
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function getQueryParameter(parameterName) {
    var queryString = window.top.location.search.substring(1);
    parameterName = parameterName + "=";
    if (queryString.length > 0) {
        begin = queryString.indexOf(parameterName);
        if (begin !== -1) {
            begin += parameterName.length;
            end = queryString.indexOf("&", begin);
            if (end === -1) {
                end = queryString.length;
            }
            return unescape(queryString.substring(begin, end));
        }
    }
    return "null";
}
//ham set style
function funcSetPseudoStyle(styleName, cssRule, style, newValue) {
    $.each(document.styleSheets, function (index, value) {
        var styleSheetName = value.href.substring(value.href.lastIndexOf('/') + 1);
        if (styleSheetName === styleName) {
            var iStyleSheet = index;
            $.each(document.styleSheets[iStyleSheet].cssRules, function (index, value) {
                if (value.selectorText.replace('::', ':') === cssRule)
                    document.styleSheets[iStyleSheet].cssRules[index].style[style] = newValue;
            });
            return false;
        }
    });
    //document.styleSheets[styleName].cssRules[cssRule].style[style] = value;
}
//xml to string
function xmlToString(xmlData) {
    var xmlString;
    //IE
    if (window.ActiveXObject) {
        xmlString = xmlData.xml;
    }
        // code for Mozilla, Firefox, Opera, etc.
    else {
        xmlString = (new XMLSerializer()).serializeToString(xmlData);
    }
    return xmlString;
}
//kiem tra email
function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) {
        return true;
    }
    else {
        return false;
    }
}
//ham lay chieu cao, chieu rong man hinh
function f_clientWidth() {
    return f_filterResults(
		window.innerWidth ? window.innerWidth : 0,
		document.documentElement ? document.documentElement.clientWidth : 0,
		document.body ? document.body.clientWidth : 0
	);
}
function f_clientHeight() {
    return f_filterResults(
		window.innerHeight ? window.innerHeight : 0,
		document.documentElement ? document.documentElement.clientHeight : 0,
		document.body ? document.body.clientHeight : 0
	);
}
function f_scrollLeft() {
    return f_filterResults(
		window.pageXOffset ? window.pageXOffset : 0,
		document.documentElement ? document.documentElement.scrollLeft : 0,
		document.body ? document.body.scrollLeft : 0
	);
}
function f_scrollTop() {
    return f_filterResults(
		window.pageYOffset ? window.pageYOffset : 0,
		document.documentElement ? document.documentElement.scrollTop : 0,
		document.body ? document.body.scrollTop : 0
	);
}
function f_filterResults(n_win, n_docel, n_body) {
    var n_result = n_win ? n_win : 0;
    if (n_docel && (!n_result || (n_result > n_docel)))
        n_result = n_docel;
    return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
}
function getWindowHeight() // viewport, not document
{
    var windowHeight = 0;
    if (typeof (window.innerHeight) === 'number') {
        // DOM compliant, IE9+
        windowHeight = window.innerHeight;
    }
    else {
        // IE6-8 workaround, Note: document can be smaller than window
        var ieStrict = document.documentElement.clientHeight; // w/out DTD gives 0
        var ieQuirks = document.body.clientHeight; // w/DTD gives document height
        windowHeight = (ieStrict > 0) ? ieStrict : ieQuirks;
    }
    return windowHeight;
}

function getWindowWidth() // viewport, not document
{
    var windowWidth = 0;
    if (typeof (window.innerWidth) === 'number') {
        // DOM compliant, IE9+
        windowWidth = window.innerWidth;
    }
    else {
        // IE6-8 workaround, Note: document can be smaller than window
        var ieStrict = document.documentElement.clientWidth; // w/out DTD gives 0
        var ieQuirks = document.body.clientWidth; // w/DTD gives document width
        windowWidth = (ieStrict > 0) ? ieStrict : ieQuirks;
    }
    return windowWidth;
}

function getScrollTop() {
    var scrollTop;
    if (typeof (window.pageYOffset) === 'number') {
        // DOM compliant, IE9+
        scrollTop = window.pageYOffset;
    }
    else {
        // IE6-8 workaround
        if (document.body && document.body.scrollTop) {
            // IE quirks mode
            scrollTop = document.body.scrollTop;
        }
        else if (document.documentElement && document.documentElement.scrollTop) {
            // IE6+ standards compliant mode
            scrollTop = document.documentElement.scrollTop;
        }
    }
    return scrollTop;
}

function getScrollLeft() {
    var scrollLeft;
    if (typeof (window.pageXOffset) === 'number') {
        // DOM compliant, IE9+
        scrollLeft = window.pageXOffset;
    }
    else {
        // IE6-8 workaround
        if (document.body && document.body.scrollLeft) {
            // IE quirks mode
            scrollLeft = document.body.scrollLeft;
        }
        else if (document.documentElement && document.documentElement.scrollLeft) {
            // IE6+ standards compliant mode
            scrollLeft = document.documentElement.scrollLeft;
        }
    }
    return scrollLeft;
}

function getScrollTopWidth(id) {
    var scrollbarWidth = 0;
    // Create the measurement node
    var scrollDiv = document.getElementById(id);
    // Get the scrollbar width
    scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth;
    return scrollbarWidth;
}

function getScrollLeftWidth(id) {
    var scrollbarWidth = 0;
    // Create the measurement node
    var scrollDiv = document.getElementById(id);
    // Get the scrollbar width
    scrollbarWidth = scrollDiv.offsetHeight - scrollDiv.clientHeight;
    return scrollbarWidth;
}

function funcGetCenTer(windowWidth, offsetLeft, imgWidth, popupWidth) {
    //lay vi tri giua hinh
    var iOffsetLeftCenter = offsetLeft + (imgWidth / 2);
    //
    return popupWidth - (getWindowWidth() - iOffsetLeftCenter) + (getWindowWidth() - windowWidth) - 10;
}

function AddNoticeTop(value) {
    $('#page-notices').html('<marquee width="100%" behavior="alternate" truespeed="500" >' + value + '</marquee>');
}

function fncDownload() {
    $('.fileDownloadForm').unbind('click');
    $('.fileDownloadForm').click(function () {
        $.fileDownload($(this).prop('action'), {
            httpMethod: "POST",
            data: $(this).serialize()
        });
    });
    
}

function funcMfUpload(ojb, strLink) {
    if (strLink === null)
        strLink = "ajax/upload/upload.ashx";
    ojb.mfupload({
        type: 'pdf,doc,xls,xlsx,docx,txt,rar,zip,7zip',
        maxsize: 4,
        post_upload: strLink,
        folder: "",
        ini_text: "",
        over_text: "",
        over_col: 'white',
        over_bkcol: '#ADDAED',
        init: function () { $("#uploaded").empty(); },
        start: function (result) {
            $("#uploaded").append('<div id="FILE' + result.fileno + '" class="files" ><div class="fname">' + result.filename + '</div><div class="prog"><div id="PRO' + result.fileno + '" class="prog_inn"></div></div></div>');
        },
        loaded: function (result) {
            if ($('.nAF').length > 0)
                $('#filesToUploadQueue').empty();
            $("#PRO" + result.fileno).parent().remove();
            $("#FILE" + result.fileno).html("Uploaded: " + result.filename + " (" + result.size + ")");
            $.each(result, function () {
                fncToHTMLFile(this.filename, this.length, this.icon);
            });
            fncDownload();
        },
        progress: function (result) {
            $("#PRO" + result.fileno).css("width", result.perc + "%");
        },
        error: function (error) {
            if (error.err_no === 1)
                ShowMessageBox('Tập tin không cho phép upload <br/>(pdf,doc,docx,xls,xlsx,txt,rar,zip,7zip)', "Thông báo", "okcancel", "Error");
        },
        completed: function (e, files, event) {
            $("#uploaded").html("");
            $("#uploaded").empty();
        }
    });
}

// Short-circuiting, and saving a parse operation
function isInt(value) {
    var x;
    if (isNaN(value)) {
        return false;
    }
    x = parseFloat(value);
    return (x | 0) === x;
}

function check_date(field) {
    var checkstr = "0123456789";
    var DateField = field;
    var Datevalue = "";
    var DateTemp = "";
    var seperator = "/";
    var day;
    var month;
    var year;
    var leap = 0;
    var err = 0;
    var i;
    err = 0;
    try {
        DateValue = $(DateField).val();
        /* Delete all chars except 0..9 */
        for (i = 0; i < DateValue.length; i++) {
            if (checkstr.indexOf(DateValue.substr(i, 1)) >= 0) {
                DateTemp = DateTemp + DateValue.substr(i, 1);
            }
        }
        DateValue = DateTemp;
        /* Always change date to 8 digits - string*/
        /* if year is entered as 2-digit / always assume 20xx */
        if (DateValue.length === 6) {
            DateValue = DateValue.substr(0, 4) + '20' + DateValue.substr(4, 2);
        }
        if (DateValue.length !== 8) {
            err = 19;
        }
        /* year is wrong if year = 0000 */
        year = DateValue.substr(4, 4);
        if (year === 0) {
            err = 20;
        }
        /* Validation of month*/
        month = DateValue.substr(2, 2);
        if ((month < 1) || (month > 12)) {
            err = 21;
        }
        /* Validation of day*/
        day = DateValue.substr(0, 2);
        if (day < 1) {
            err = 22;
        }
        /* Validation leap-year / february / day */
        if ((year % 4 === 0) || (year % 100 === 0) || (year % 400 === 0)) {
            leap = 1;
        }
        if ((month === 2) && (leap === 1) && (day > 29)) {
            err = 23;
        }
        if ((month === 2) && (leap !== 1) && (day > 28)) {
            err = 24;
        }
        /* Validation of other months */
        if ((day > 31) && ((month === "01") || (month === "03") || (month === "05") || (month === "07") || (month === "08") || (month === "10") || (month === "12"))) {
            err = 25;
        }
        if ((day > 30) && ((month === "04") || (month === "06") || (month === "09") || (month === "11"))) {
            err = 26;
        }
        /* if 00 ist entered, no error, deleting the entry */
        if ((day === 0) && (month === 0) && (year === 0)) {
            err = 0; day = ""; month = ""; year = ""; seperator = "";
        }
        /* if no error, write the completed date to Input-Field (e.g. 13.12.2001) */
        if (err === 0) {
            DateField.value = day + seperator + month + seperator + year;
            return true;
        }
            /* Error-message if err != 0 */
        else {
            $(field).focus();
            if ($(field).parent().children(".div_error").length === 0) {
                $(field).parent().append("<span class='div_error'>*</span>");
                $('.div_error').fadeOut(5000, function (e) {
                    $(this).remove();
                });
            }
            return false;
        }
    }
    catch (Error) {
        if ($(field).parent().children(".div_error").length === 0) {
            $(field).parent().append("<span class='div_error'>*</span>");
            $('.div_error').fadeOut(5000, function (e) {
                $(this).remove();
            });
        }
        return false;
    }
}

//dynatree
function DeselectAll(object) {
    $(object).dynatree("getRoot").visit(function (node) {
        node.select(false);
    });
    return false;
}
function setNodeTree(id, idNode) {
    var idSelect = idNode.split(',');
    for (var i = 0; i < idSelect.length; i++) {
        $(id).dynatree("getTree").selectKey(idSelect[i]);
    }
}
/*checkbox*/
function SelectAll(Chk) {
    var xState = Chk.checked;
    var elm = $('.rows-box .chkCheck');
    $(elm).prop("checked", xState); CheckDeleteAll();
}
function DeSelectedAll() {
    var elm = $('.rows-box .chkCheck');
    $(elm).prop("checked", false); CheckDeleteAll();
    $('.header-box .chkCheck').prop("checked", false);
}
function Select(Chk) {
    var xState = Chk.checked;
    $(Chk).prop("checked", xState);
    try {
        var checkedLengt = $('.rows-box .chkCheck:checked').length;
        var elm = $('.rows-box .chkCheck').length;
        $('.header-box .chkCheck').prop("checked", false);
        if (checkedLengt === elm)
            $('.header-box .chkCheck').prop("checked", true);
        CheckDeleteAll();
        CheckRestoreAll();

    }
    catch (Error) { }
}
function CheckDeleteAll() {
    $('#del-all').removeClass('delete_all').addClass('undelete_all');
    HighlightRows();
    var checkedLengt = $('.rows-box .chkCheck:checked').length;
    if (checkedLengt > 0) {
        $('#del-all').addClass('delete_all').removeClass('undelete_all');
        if (!CheckNullOrEmpty($('#btnDelAll'))) {
            fncShowButton($('#btnDelAll'));
        }
        if (!CheckNullOrEmpty($('#btnNhanDoi'))) {
            fncShowButton($('#btnNhanDoi'));
        }
    }
    else {
        if (checkedLengt === 0) {
            if (!CheckNullOrEmpty($('#btnDelAll'))) {
                fncHideButton($('#btnDelAll'));
            }
            if (!CheckNullOrEmpty($('#btnNhanDoi'))) {
                fncHideButton($('#btnNhanDoi'));
            }
        }
    }
}
function HighlightRows() {
    $('.rows-box .chkCheck').parent().parent().removeClass('HighLightRow');
    $('.rows-box .chkCheck:checked').parent().parent().addClass('HighLightRow');
}
function CheckRestoreAll() {
    try {
        $('#restore-all').removeClass('delete_all').addClass('undelete_all');
        var checkedLengt = $('.rows-box .chkCheck:checked').length;
        if (checkedLengt > 0)
            $('#del-all').addClass('delete_all').removeClass('undelete_all');
    }
    catch (Error) {
    }
}

//Capitalize the first letter of string in JavaScript
function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function GetCurrentPage() {
    var iTrangDau = parseInt($('#girdInfo .currentRow').html().trim());
    return iTrangDau;
}

function fncGetData_Danhsachnghiphep(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === true) {
                fncBuilCalendar($('#tableContent'), dataHeader_Milestones, obJson.data);
                $('#boxContent .CRC').remove();
                //$("#boxContent #tableContent").colResizable({
                //    disable: true
                //});
                //$("#boxContent #tableContent").colResizable({
                //    liveDrag: true,
                //    gripInnerHtml: "<div class='grip'></div>",
                //    draggingClass: "dragging"//,
                //});

                var tableClone = $('#tableContent').find('thead tr.header-box').clone();
                if ($('#tableFixedHeader').find('thead tr.header-box').length == 0)
                    $('#tableFixedHeader').append(tableClone);
            }
            else {
                debugger;
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent'), obJson.data, obJson.RowID)
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                    else {
                        $("#tableContent tbody").html("");
                        fncBuildBody($('#tableContent'), obJson.data);
                    }

                    if ($('.divExentd').length == 0) {
                        $('#girdInfoLoad').append('<div class="divExentd"><i class="retweet_32"></i>Xem thêm</div>');
                        $('.divExentd').click(function () {
                            GetData(false);
                        });
                    }
                }
                else {
                    $('#txttrangthu').val(0);
                    $("#tableContent tbody").html("");
                    $('#divLoadExtend .divExentd').remove();
                }
            }
            $('#girdInfo .Pages').html(obJson.Pages);
            $('#girdInfo .tongdong').html(obJson.tongdong);
        },
        error: function () {
            RemoveLoader();
        }
    });
}

function fncGetData_Kiemkekho(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            debugger;
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === true) {
                $("#tableContent tbody").html("");
                fncBuilCalendar_header($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();                                           // <table class="table_danhSach table_fixed" id="tableFixedHeader"></table>
                //$("#boxContent #tableContent").colResizable({                             // <table class="table_danhSach table_fixed" id="tableContent">
                //    disable: true
                //});
                //$("#boxContent #tableContent").colResizable({
                //    liveDrag: true,
                //    gripInnerHtml: "<div class='grip'></div>",
                //    draggingClass: "dragging"//,
                //});

                //$('#txtNgayDangKy').val(obJson.songaytrongthang);

                fncBuilCalendar_body($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();

                //var tableClone = $('#tableContent').find('thead tr.header-box').clone();
                //if ($('#tableFixedHeader').find('thead tr.header-box').length == 0)
                //    $('#tableFixedHeader').append(tableClone);
            }
            else {
                //debugger;
                $("#tableContent tbody").html("");
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent'), obJson.data, obJson.RowID)
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                    else {
                        $("#tableContent tbody").html("");
                        fncBuildBody($('#tableContent'), obJson.data);
                    }

                    if ($('.divExentd').length == 0) {
                        $('#girdInfoLoad').append('<div class="divExentd"><i class="retweet_32"></i>Xem thêm</div>');
                        $('.divExentd').click(function () {
                            GetData(false);
                        });
                    }
                }
                else if (obJson.data.length == 0 && obJson.xemthem == "true") {
                    fncBuildBody($('#tableContent'), obJson.data);
                    $('#divLoadExtend .divExentd').remove();
                }
                else {
                    $("#tableContent tbody").html("");
                    fncBuildBody($('#tableContent'), obJson.data);
                }


            }
            $('#girdInfo .total').html(obJson.total);
            $('#girdInfo .currentRow').html(obJson.curentPage); 
            $('#girdInfo .sumRow').html(obJson.Pages);

            $('#girdInfo .songaytrongthang').html(obJson.songaytrongthang);

        },
        error: function () {
            RemoveLoader();
        }
    });
}

function fncGetData_Timekeeping(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            //debugger;
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === true) {
                //fncBuilCalendar_header($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();                                           // <table class="table_danhSach table_fixed" id="tableFixedHeader"></table>
                //$("#boxContent #tableContent").colResizable({                             // <table class="table_danhSach table_fixed" id="tableContent">
                //    disable: true
                //});
                //$("#boxContent #tableContent").colResizable({
                //    liveDrag: true,
                //    gripInnerHtml: "<div class='grip'></div>",
                //    draggingClass: "dragging"//,
                //});

                $('#txtsongaytrongthang').val(obJson.songaytrongthang);

                //var phongban = obJson.maphongban;
                //$('#filter01').val(phongban);
                //for (var selector in configChosenDanhmucphongban) {
                //    $(selector).change();
                //    $(selector).trigger('chosen:updated');
                //}





                //$('#txtthang').val(obJson.thangnam);
                $("#tableContent tbody").html("");
                fncBuilCalendar_body($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();

                var tableClone = $('#tableContent').find('thead tr.header-box').clone();
                if ($('#tableFixedHeader').find('thead tr.header-box').length == 0)
                    $('#tableFixedHeader').append(tableClone);
            }
            else {
                //debugger;
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent'), obJson.data, obJson.RowID)
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                    else {
                        $("#tableContent tbody").html("");
                        fncBuildBody($('#tableContent'), obJson.data);
                    }

                    if ($('.divExentd').length == 0) {
                        $('#girdInfoLoad').append('<div class="divExentd"><i class="retweet_32"></i>Xem thêm</div>');
                        $('.divExentd').click(function () {
                            GetData(false);
                        });
                    }
                }
                else if (obJson.data.length == 0 && obJson.xemthem == "true") {
                    fncBuildBody($('#tableContent'), obJson.data);
                    $('#divLoadExtend .divExentd').remove();
                }
                else {
                    $('#txttrangthu').val(0);
                    $("#tableContent tbody").html("");
                    fncBuildBody($('#tableContent'), obJson.data);
                }
            }

            $('#girdInfo .Pages').html(obJson.Pages);
            $('#girdInfo .songaytrongthang').html(obJson.songaytrongthang);
            $('#girdInfo .tongdong').html(obJson.tongdong);

        },
        error: function () {
            RemoveLoader();
        }
    });
}

function fncGetData_Timekeeping_new(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            debugger;
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === true) {
                //fncBuilCalendar_header($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();                                           // <table class="table_danhSach table_fixed" id="tableFixedHeader"></table>

                $("#tableContent tbody").html("");
                fncBuilCalendar_body($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();

                var tableClone = $('#tableContent').find('thead tr.header-box').clone();
                if ($('#tableFixedHeader').find('thead tr.header-box').length == 0)
                    $('#tableFixedHeader').append(tableClone);
            }
            else {
                //debugger;
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent'), obJson.data, obJson.RowID)
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                    else {
                        $("#tableContent tbody").html("");
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                }
                else {
                   
                    $("#tableContent tbody").html("");
                    fncBuildBody($('#tableContent'), obJson.data);
                }
            }
            $('#txtphongban_congtruong').val(obJson.phongban_congtruong);
            $('#txtngaycong_le').val(obJson.ngaycong_le1);

            $('#txtnghiphep').val(obJson.nghiphep1);
            $('#txtnghihieu_hi').val(obJson.nghihieuhi1);
            $('#txtnghiom_thaisan').val(obJson.nghiom_ts1);
            $('#txtnghikhongluong').val(obJson.nghikhongluong1);
            $('#txtsongayduoc_traluong').val(obJson.songayduoctraluong1);
            $('#txtsongaypc_tiencom').val(obJson.snphucapcom1);
            $('#txtsongay_congchuan').val(obJson.songaycongchuan1);
            $('#txtchenhlech_ngaycong').val(obJson.chenhlechngaycong1);

        },
        error: function () {
            RemoveLoader();
        }
        
    });
}


function fncGetData_Timekeeping_Edit(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            debugger;
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === true) {
                //fncBuilCalendar_header($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();                                           // <table class="table_danhSach table_fixed" id="tableFixedHeader"></table>

                $("#tableContent tbody").html("");
                fncBuilCalendar_body($('#tableContent'), dataHeader_Milestones, obJson.data);  // <div id="boxContent"> <div class="boxTable boxTable1">  
                $('#tableContent .CRC').remove();

                var tableClone = $('#tableContent').find('thead tr.header-box').clone();
                if ($('#tableFixedHeader').find('thead tr.header-box').length == 0)
                    $('#tableFixedHeader').append(tableClone);
            }
            else {
                //debugger;
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent'), obJson.data, obJson.RowID)
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                    else {
                        $("#tableContent tbody").html("");
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                }
                else {

                    $("#tableContent tbody").html("");
                    fncBuildBody($('#tableContent'), obJson.data);
                }
            }
            //$('#txtphongban_congtruong').val(obJson.phongban_congtruong);
            //$('#txtngaycong_le').val(obJson.ngaycong_le1);

            //$('#txtnghiphep').val(obJson.nghiphep1);
            //$('#txtnghihieu_hi').val(obJson.nghihieuhi1);
            //$('#txtnghiom_thaisan').val(obJson.nghiom_ts1);
            //$('#txtnghikhongluong').val(obJson.nghikhongluong1);
            //$('#txtsongayduoc_traluong').val(obJson.songayduoctraluong1);
            //$('#txtsongaypc_tiencom').val(obJson.snphucapcom1);
            //$('#txtsongay_congchuan').val(obJson.songaycongchuan1);
            //$('#txtchenhlech_ngaycong').val(obJson.chenhlechngaycong1);

        },
        error: function () {
            RemoveLoader();
        }

    });
}


function fncGetData_Timekeeping_new_searchmnv(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            debugger;
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === false) {

                $('#txthovaten').val(obJson.hovaten);
                $('#txtemail').val(obJson.email);
                $('#txtngaysinh').val(obJson.ngaysinh);
                $('#txtmanhanvien').val(obJson.manhanvien);

                var phongban = obJson.maphongban;
                $('#filter01').val(phongban);
                for (var selector in configChosenDanhmucphongban) {
                    $(selector).change();
                    $(selector).trigger('chosen:updated');
                }

            }
        },
        error: function () {
            RemoveLoader();
        }
    });
}



function fncGetData(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            var obJson = jQuery.parseJSON(rValue);
            RemoveLoader();
            if (isHeader === true) {
                fncBuilCalendar($('#tableContent'), dataHeader_Milestones, obJson.data);
                $('#boxContent .CRC').remove();
                $("#boxContent #tableContent").colResizable({
                    disable: true
                });
                $("#boxContent #tableContent").colResizable({
                    liveDrag: true,
                    gripInnerHtml: "<div class='grip'></div>",
                    draggingClass: "dragging"//,
                });

                var tableClone = $('#tableContent').find('thead tr.header-box').clone();
                if ($('#tableFixedHeader').find('thead tr.header-box').length == 0)
                    $('#tableFixedHeader').append(tableClone);
            }
            else {
                debugger;
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent'), obJson.data, obJson.RowID)
                        fncBuildBody($('#tableContent'), obJson.data);
                    }
                    else {
                        $("#tableContent tbody").html("");
                        fncBuildBody($('#tableContent'), obJson.data);
                    }

                    if ($('.divExentd').length == 0) {
                        $('#girdInfoLoad').append('<div class="divExentd"><i class="retweet_32"></i>Xem thêm</div>');
                        $('.divExentd').click(function () {
                            GetData(false);
                        });
                    }
                }
                else {
                    $("#tableContent tbody").html("");
                    $('#divLoadExtend .divExentd').remove();
                }
            }
            $('#girdInfo .currentRow').html(obJson.curentPage);
            $('#girdInfo .sumRow').html(obJson.Pages);
                        
        },
        error: function () {
            RemoveLoader();
        }
    });
}

function fncGetData_chitiet(url, data, isHeader) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            //debugger;
            var obJson = jQuery.parseJSON(rValue);
            //alert(rValue);
            RemoveLoader();
            if (isHeader === true) {
                fncBuilCalendar($('#tableContent_chitiet'), dataHeader_Milestones, obJson.data);
                $('#boxTableDataSelected .CRC').remove();
                $("#boxTableDataSelected #tableContent_chitiet").colResizable({
                    disable: true
                });
                $("#boxTableDataSelected #tableContent_chitiet").colResizable({
                    liveDrag: true,
                    gripInnerHtml: "<div class='grip'></div>",
                    draggingClass: "dragging"//,
                });

                var tableClone = $('#tableContent_chitiet').find('thead tr.header-box').clone();
                if ($('#tableFixedHeaderselected').find('thead tr.header-box').length == 0)
                    $('#tableFixedHeaderselected').append(tableClone);
            }
            else {
                if (obJson.data.length > 0) {
                    if (obJson.SubRow == "true") {
                        fncAddRowAfterRow($('#tableContent_chitiet'), obJson.data, obJson.RowID)
                    }
                    else {
                        fncBuildBody($('#tableContent_chitiet'), obJson.data);
                    }

                    if ($('.divExentd').length == 0) {
                        $('#girdInfoLoad').append('<div class="divExentd"><i class="retweet_32"></i>Xem thêm</div>');
                        $('.divExentd').click(function () {
                            GetData(false);
                        });
                    }
                }
                else {
                    $('#divLoadExtend .divExentd').remove();
                }
            }
            $('#girdInfo .currentRow').html(obJson.curentPage);
            $('#girdInfo .sumRow').html(obJson.Pages);

        },
        error: function () {
            RemoveLoader();
        }
    });
}

function fncPostData(url, data) {
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json",
        datatype: "html",
        data: data,
        cache: false,
        beforeSend: function () {
            AddLoader();
        },
        success: function (rValue) {
            if (rValue == "1")
                alert("Hoan thanh");
        },
        error: function () {
            RemoveLoader();
        }
    });
}

function fncSetMenuActive(id) {
    //reset active all item
    $('.nav-primary').find('li').removeClass('active');
    $('.nav-primary').find('li[madanhmuc="27"]').addClass('active');
    $('.nav-primary').find('li[madanhmuc="'+ id +'"]').addClass('active');
}