function ShowMessageBox(strText, strCaption, msButtons, msIcon) {
    try {
        var _strText = "Bạn có chắc muốn xóa không?";
        var _strCaption = "Xóa";
        var _strButtons = "<input class='buttoncustom blueButton windowConform' type='button' value='Đóng' id='btnCloseDialog'/>";
        var _strmsIcon = "";
        //kiem tra noi dung co bang null hoac "" khong, neu co thi nhap mac dinh
        if (strText != null)
            if (strText.replace(/\s/g, '').length > 0)
                _strText = strText;
        //kiem tra tieu de co bang null hoac "" khong, neu co thi nhap mac dinh
        if (strCaption != null)
            if (strCaption.replace(/\s/g, '').length > 0)
                _strCaption = strCaption;
        //kiem tra button co bang null hoac "" khong, neu co thi nhap mac dinh
        if (msButtons != null)
            if (msButtons.replace(/\s/g, '').length > 0)
                if (msButtons.replace(/\s/g, '').toLowerCase() == "okcancel")
                    _strButtons = "<input id='btnSubmitMessage' class='buttoncustom blueButton windowConform' type='button' value='Đồng ý'/>&nbsp;&nbsp;" + _strButtons;
        //kiem tra icon co bang null hoac "" khong, neu co thi nhap mac dinh
        if (msIcon != null)
            if (msIcon.replace(/\s/g, '').length > 0)
                if (msIcon.replace(/\s/g, '').toLowerCase() == "error" || msIcon.replace(/\s/g, '').toLowerCase() == "information" ||
                   msIcon.replace(/\s/g, '').toLowerCase() == "question" || msIcon.replace(/\s/g, '').toLowerCase() == "warning")
                    _strmsIcon = "<img src='templates/system/images/messagebox/icon48x48_" + msIcon + ".png'/>";
        var _strFormMessage = "<div id='MessageBox' class='web_dialog' style='height: auto;z-index:9120'>" +
            "<table style='width: 100%; border: 0px;' cellpadding='3' cellspacing='0'>" +
                "<tr>" +
                    "<td colspan='2' class='web_dialog_title' >" +
                        "<label >" + _strCaption + "</label>" +
                        "<a style='float:right' href='#' onclick='HideMessageBox();return false'><i class='btn_close_dialog'></i></a>" +
                    "</td>" +
                "</tr>" +
                "<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
                "<tr>" +
                    "<td id='tdMessageIcon'>" +
                        _strmsIcon +
                    "</td>" +
                    "<td >" +
                        "<b>" + _strText + "</b>" +
                    "</td>" +
                "</tr>" +
                "<tr><td>&nbsp;</td><td>&nbsp;</td></tr>" +
                "<tr>" +
                    "<td colspan='2' style='text-align: center;'>" +
                        _strButtons +
                    "</td>" +
                "</tr>" +
            "</table>" +
        "</div>";
        $("#overlayAlert").show();
        $('body form#form1').append(_strFormMessage);
        $("#MessageBox").show('4000');
        $("#btnCloseDialog").click(HideMessageBox);
    } catch (Error) {
        if (window.console) console.log(Error);
    }
}

function HideMessageBox() { $("#overlayAlert").hide(); $("#MessageBox").fadeOut(400).remove() };
function ShortAlertWithAutoHide(e, times) {
    if ($('.butterbar-container').length > 0)
        $('.butterbar-container').remove();
    $('body #form1').append('<div class="butterbar-container" style="display:none"><span id="butterbar">Lưu thành công.</span></div>');
    if (!CheckNullOrEmpty(e))
        $('#butterbar').html(e);
    if (!CheckNullOrEmpty(times))
        $('.butterbar-container').css('display', 'block').fadeOut(times, function (e) {
            $(this).remove();
        });
    else
        $('.butterbar-container').css('display', 'block');
}
function HideShortAlertWithAutoHide() {
    $('.butterbar-container').fadeOut("", function (e) {
        $(this).remove();
    });
}
/*end Checkbox*/
function ShowDialog(e, t, n, r) {
    $("#overlayAlert").show();
    $(t).fadeIn(0);
    try {
        $("#web_dialog_title_confirm").html(n);
        $("#web_dialog_content").html(r)
    } catch (i) { }

    if (e) {
        $("#overlayAlert").unbind("click");
    } else {
        $("#overlayAlert").unbind("click");
        $("#overlayAlert").click(function (e) {
            HideDialog(t);            
        })
    }
}
function HideDialog(e) {
    $("#overlayAlert").hide();
    $(e).fadeOut(0);    
}

function notifyOnBrowser(bodyContent) {
    if (CheckNullOrEmpty(bodyContent))
        bodyContent = "";
    // Let's check if the browser supports notifications
    if (!("Notification" in window)) {
        alert("This browser does not support desktop notification");
    }

        // Let's check whether notification permissions have already been granted
    else if (Notification.permission === "granted") {
        // If it's okay let's create a notification
        var notification = new Notification(bodyContent);
        setTimeout(notification.close.bind(notification), 6000);
    }

        // Otherwise, we need to ask the user for permission
    else if (Notification.permission !== 'denied') {
        Notification.requestPermission(function (permission) {
            // If the user accepts, let's create a notification
            if (permission === "granted") {
                var notification = new Notification(bodyContent);
                setTimeout(notification.close.bind(notification), 6000);
            }
        });
    }

    // At last, if the user has denied notifications, and you 
    // want to be respectful there is no need to bother them any more.
}