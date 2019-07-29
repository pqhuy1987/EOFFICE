
var configChosenDanhmucphongban = {
    '.chosen-select-plan': { width: "290px", disable_search_threshold: 10 },
    '.chosen-select-loaikehoach': { width: "220px", disable_search_threshold: 10 }
}

var dataHeader_Milestones =
[{
    col_class: 'header-box',
    col_id: '',
    col_value: [{
        colspan: 1,
        rowspan: 2,
        col_class: 'ovh col32 stt',
        col_id: '',
        col_value: 'STT'
    },
    {
        colspan: 1,
        rowspan:2,
        col_class: 'ovh col33',
        col_id: '',
        col_value: 'Họ và tên'
    },
    {
    colspan: 1,
    col_class: 'ovh col50',
    col_id: '',
    col_value: 'Ghi chú'
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenDanhmucphongban) {
        $(selector).chosen(configChosenDanhmucphongban[selector]);
    }
    $("#filter01").chosen().change(function () {
        //debugger;
        var maphongban = $("#filter01").val();
        var thang = $('#txtthang').val();
        document.getElementById('txttrangthu').value = 1;
        GetData(true, "", "", thang, maphongban);
    });

    $('#chkkiemtra').on('click', function (event) {
        //debugger;
        var maphongban = $("#filter01").val();
        var thang = $('#txtthang').val();
        document.getElementById('txttrangthu').value = 1;
        GetData(true, "", "", thang, maphongban);
    });


    $('#txttrangthu').on('keypress', function (event) {
        //debugger;
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            var thang = $('#txtthang').val();
            var maphongban = $("#filter01").val();
            GetData(true, "", "", thang, maphongban);
        }
    });


    $('input#txtthang').on('keypress', function (event) {
        //debugger;
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            document.getElementById('txttrangthu').value = 1;
            var thang = $('#txtthang').val();
            var maphongban = $("#filter01").val();
            GetData(true, "", "", thang, maphongban);
        }
    });

    $("#btnNext").click(function () {
       // debugger;
        var trangthu = $('#txttrangthu').val();
        var currentRow = $('#girdInfo span.Pages').text();
        if (parseInt(trangthu) < parseInt(currentRow)) {
            document.getElementById('txttrangthu').value = parseInt(trangthu) + 1;
            var thang = $('#txtthang').val();
            var maphongban = $("#filter01").val();
            GetData(true, "", "", thang, maphongban);
        }
        else if (parseInt(trangthu) == parseInt(currentRow) && parseInt(trangthu)==0) {
            document.getElementById('txttrangthu').value = parseInt(trangthu) + 1;
            var thang = $('#txtthang').val();
            var maphongban = $("#filter01").val();
            GetData(true, "", "", thang, maphongban);
        }
    });

    $("#btnPre").click(function () {
        //debugger;
        var trangthu = $('#txttrangthu').val();
        if (parseInt(trangthu) > 1) {
            document.getElementById('txttrangthu').value = parseInt(trangthu) - 1;
            var thang = $('#txtthang').val();
            var maphongban = $("#filter01").val();
            GetData(true, "", "", thang, maphongban);
        }
    });



    GetData(true);

});

function GetData(isHeader, mkh, stt, thang, maphongban) {
    //debugger;
    var currentRow = $("#txttrangthu").val();
    thang = $('#txtthang').val();
    maphongban = $("#filter01").val();
    var manv = $('#txtmanhanvien').val();
    var hovaten = $('#txthovatens').val(); 
    var chkktra = 0;
    if ($('#chkkiemtra:checked').length == 1)
        chkktra = 1;
    var trangthaiduyet = $('#txttrangthaiduyet').val();
    var STT = CheckNullOrEmpty(stt) == true ? "0" : stt;
    var DataJson = "{'songaycuathang':'" + 31 + "'," +
                     "'maphongban':'" + maphongban + "'," +
                     "'curentPage':'" + currentRow + "'," +
                     "'manhanvien':'" + manv + "'," +
                     "'hovaten':'" + hovaten + "'," +
                     "'chamcong':'" + chkktra + "'," +
                     "'trangthaiduyet':'" + trangthaiduyet + "'," +
                     "'thang':'" + thang + "'" +
                   "}";

    fncGetData_Timekeeping(linkContent + 'Timekeeping/SelectRows', DataJson, isHeader);
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