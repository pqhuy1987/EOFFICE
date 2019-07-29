
var configChosenDanhmucvanphongpham = {
    '.chosen-select-plan': { width: "290px", disable_search_threshold: 10 },
    '.chosen-select-loaikehoach': { width: "250px", disable_search_threshold: 10 }
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
        col_class: 'ovh col2 stt',
        col_id: '',
        col_value: 'STT'
    },
    {
        colspan: 1,
        col_class: 'ovh col3',
        col_id: '',
        col_value: 'Mã VPP'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Tên văn phòng phẩm'
    },

    {
        colspan: 1,
        col_class: 'ovh col5',
        col_id: '',
        col_value: 'Đơn vị tính'
    },

     {
         colspan: 1,
         col_class: 'ovh col6',
         col_id: '',
         col_value: 'Số lượng'
     },

    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: 'Số tiền'
    },

    {
        colspan: 1,
        col_class: 'ovh col8',
        col_id: '',
        col_value: 'Thành tiền'
    },
    {
            colspan: 1,
            col_class: 'ovh col9',
            col_id: '',
            col_value: ''
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    $('#ScrollContent .boxTableSelected').slimscroll({ height: 100 });

    //chosen combobox jquery
    for (var selector in configChosenDanhmucvanphongpham) {
        $(selector).chosen(configChosenDanhmucvanphongpham[selector]);
    }

    $("#filter01").chosen().change(function () {
        //GetData(false);
    });

    var ngayBD = $('#txtNgayDangKy');
    ngayBD.datepicker({
        dateFormat: "dd/mm/yy",
        defaultDate: new Date(),
        dayNamesShort: $.datepicker.regional.vi.dayNamesShort,
        dayNames: $.datepicker.regional.vi.dayNames,
        monthNamesShort: $.datepicker.regional.vi.monthNamesShort,
        monthNames: $.datepicker.regional.vi.monthNames,
        showAnim: "slideDown",
        firstDay: 1 // Start with Monday
    });
    ngayBD.datepicker("setDate", new Date());
    ngayBD.change(function () {
        if (!check_date(this))
            ngayBD.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngayKT.val())) {
            ngayKT.datepicker("setDate", $(this).val());
        }
    });

    GetData(true);

});

function GetData(isHeader, mpb, stt) {
    //var makehoach = CheckNullOrEmpty(mkh) == true ? "0" : maphongban;
    //var STT = CheckNullOrEmpty(stt) == true ? "0" : stt;
    var DataJson = "";
    fncGetData_chitiet(linkContent + 'Home/SelectRows_Dangkyvanphongpham_chitiet', DataJson, isHeader);
    fncGetData(linkContent + 'Home/SelectRows_DM_vanphongpham', DataJson, isHeader);
    
}


function ShowSubLine(e) {
    //tim nhung dong sub
    var maKeHoach = "";//$(e).attr('mpb');
    var tableContent = $("#tableContent");
    var rows = tableContent.find('tr[subparent="' + maKeHoach + '"]');
    if(rows.length > 0)
    {
        rows.slideToggle();
    }
    else
    {
        GetData(false, maKeHoach, $(e).parent().parent().find(".stt").html().trim());
    }
}

//ham set vi tri combobox chon gio
//function fncPostionDropDown(chosenDrop, objUpdateValue) {
//    chosenDrop = $(".chosen-drop-grid").css({
//        "top": objUpdateValue.position().top + objUpdateValue.outerHeight(true),
//        "left": objUpdateValue.position().left,
//    });
//}