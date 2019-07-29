
var configChosenDanhmucphongban = {
    '.chosen-select-plan': { width: "290px", disable_search_threshold: 10 },
    '.chosen-select-loaikehoach': { width: "100%", disable_search_threshold: 10 }
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
        col_value: 'Tên ngày nghỉ'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Ngày nghỉ'
    },
    {
    colspan: 1,
    col_class: 'ovh col5',
    col_id: '',
    col_value: ''
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenDanhmucphongban) {
        $(selector).chosen(configChosenDanhmucphongban[selector]);
    }

    $("#filter01").chosen().change(function () {
        //GetData(false);
    });

    $("#filter01").chosen().change(function () {
        444
    });
    GetData(true);

  

});

function GetData(isHeader, mkh, stt) {
    var makehoach = CheckNullOrEmpty(mkh) == true ? "0" : maphongban;
    var STT = CheckNullOrEmpty(stt) == true ? "0" : stt;
    var DataJson = "";
    debugger;
    fncGetData_Kiemkekho(linkContent + 'Danhmuc/SelectRows_holiday', DataJson, isHeader);
}

function ShowSubLine(e) {
    //tim nhung dong sub
    var maKeHoach = $(e).attr('mpb');
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