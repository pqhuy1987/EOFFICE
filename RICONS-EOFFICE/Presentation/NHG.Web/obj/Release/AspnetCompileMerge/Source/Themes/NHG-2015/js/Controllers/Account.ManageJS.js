var idActive = 28;
var configChosenMilestones = {
    '.chosen-select-donvi': { width: "180px", disable_search_threshold: 10 },
    '.chosen-select-phongban': { width: "180px", disable_search_threshold: 10 },
    '.chosen-select-donvinew': { width: "100%", disable_search_threshold: 10 },
    '.chosen-select-phongbannew': { width: "100%", disable_search_threshold: 10 },
    '.chosen-select-chucdanhnew': { width: "100%", disable_search_threshold: 10 }
}
var dataHeader_Milestones =
[{
    col_class: 'header-box',
    col_id: '',
    col_value: [{
        colspan: 1,
        col_class: 'ovh col1',
        col_id: '',
        col_value: '<input type="checkbox" onclick="SelectAll(this);" class="chkCheck" />'
    },
    {
        colspan: 1,
        col_class: 'ovh col2',
        col_id: '',
        col_value: 'STT'
    },
    {
        colspan: 1,
        col_class: 'ovh col3',
        col_id: '',
        col_value: 'Tình trạng'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Tên đăng nhập'
    },
    {
        colspan: 1,
        col_class: 'ovh col5',
        col_id: '',
        col_value: 'Họ và tên'
    },
     {
         colspan: 1,
         col_class: 'ovh col6',
         col_id: '',
         col_value: 'Email'
     },
    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: 'Phòng ban'
    },
    {
        colspan: 1,
        col_class: 'ovh col8',
        col_id: '',
        col_value: 'Chức danh'
    },
    {
        colspan: 1,
        col_class: 'ovh col9',
        col_id: '',
        col_value: 'Đơn vị'
    },
    {
        colspan: 1,
        col_class: 'ovh col10',
        col_id: '',
        col_value: ''
    }]
}];


$(document).ready(function (e) {
    //set menu active
    fncSetMenuActive(idActive);
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    $("#filter01").chosen().change(function () {
        GetData(false);
    });
    $("#filter02").chosen().change(function () {
        GetData(false);
    });
    $("#chkSelectKichhoat1").change(function () {
        if( $('#chkSelectKichhoat1').prop("checked"))
        {
            $('#spKichhoat1').html("Kích hoạt");
        }
        else
        {
            $('#spKichhoat1').html("Không kích hoạt");
        }
        GetData(false);
    });

    $("#chkSelectKichhoat").change(function () {
        if ($('#chkSelectKichhoat').prop("checked")) {
            $('#spKichhoat').html("Kích hoạt");
        }
        else {
            $('#spKichhoat').html("Không kích hoạt");
        }
    });

    $('#txtdiachimail').on('keypress', function (event) {
        //debugger;
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            GetData(false);
        }
    });
    GetData(true);
    //ShowMessageBox('Bạn có muốn xóa văn bản này?', "Xóa Văn Bản", "okcancel", "question");
});

function GetData(isHeader) {
    var strKichHoat = $('#chkSelectKichhoat').prop("checked") == true ? "1" : "0";



    var DataJson = "{'madonvi':" + $('#filter01').val() + "," +
                        "'maphongban':'" + $('#filter02').val() + "'," +
                        "'kichhoat':'" + strKichHoat + "'," +
                        "'tendangnhap':'" + $('#txtdiachimail').val() + "'" +
                        "}";
    fncGetData(linkContent + 'Account/SelectRows', DataJson, isHeader);
}

///ham ho tro chuc nang xoa van ban
function fncDeletes() {
    var strTextMessage = '';
    if ($('.rows-box .chkCheck:checked').length < 0)
        strTextMessage = '(' + $('.rows-box .chkCheck:checked').length + ')';
    ShowMessageBox('Bạn có muốn xóa ' + strTextMessage + ' văn bản này?', "Xóa Văn Bản", "okcancel", "question");
    //set su kien cho nut button Submit
    $('#btnSubmitMessage').click(function (e) {
        HideMessageBox();
        ShortAlertWithAutoHide('Đang xử lý...');
        var vInputChecked = $('.rows-box .chkCheck:checked').map(function () { return $(this).attr('codeid'); }).get();
        //tao data json de postback len server
        var DataJson = "{";
        DataJson += "'type':'del',";
        DataJson += "'mavanban':'" + vInputChecked.join(',') + "'";
        DataJson += "}";
        $('.rows-box .chkCheck:checked').parent().parent().remove(); //xoa dong duoc chon tren gridview
        $('.chkCheck').prop("checked", false); //uncheck nhung checkbox khong duoc xoa
        CheckDeleteAll();
        fncRequestToServer(DataJson);
    });
    $("#btnCloseDialog").click(function (e) {
        $("#overlayAlert").hide();
        $("#MessageBox").fadeOut(400).remove();
        DeSelectedAll();
    });
}