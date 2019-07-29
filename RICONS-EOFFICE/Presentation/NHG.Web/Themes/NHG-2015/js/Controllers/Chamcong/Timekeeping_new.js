
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
        col_value: 'Mã phòng ban'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Tên phòng ban'
    },
    {
        colspan: 1,
        col_class: 'ovh col5',
        col_id: '',
        col_value: 'Trực thuộc đơn vị'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'Số điện thoại'
    },
    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: 'Ghi chú'
    },
    {
    colspan: 1,
    col_class: 'ovh col8',
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
        //debugger;
        var DataJson = $('#filter01').val();
        $.ajax({
            type: 'POST',
            url: '/Timekeeping/SelectRows_themmoi_laymaphongban',
            datatype: "JSON",
            data: "DataJson=" + DataJson,
            cache: false,
        })

        if ($("#txtngaylamviecden").val().trim().length != 10 || $("#txtngaylamviectu").val().trim().length != 10) {
            //alert('Nhập ngày làm việc đến không hợp lệ');
            return false;
        }
        else {
            renderDate($("#txtthang").val().trim());
            GetData(false);
        }
    });

    var ngaysinh = $('#txtngaysinh');
    //debugger;
    ngaysinh.datepicker({
        dateFormat: "dd/mm/yy",
        defaultDate: new Date(),
        dayNamesShort: $.datepicker.regional.vi.dayNamesShort,
        dayNames: $.datepicker.regional.vi.dayNames,
        monthNamesShort: $.datepicker.regional.vi.monthNamesShort,
        monthNames: $.datepicker.regional.vi.monthNames,
        showAnim: "slideDown",
        firstDay: 1 // Start with Monday
    });
    //ngayBD.datepicker("setDate", new Date());

    ngaysinh.change(function () {
        //debugger;
        if (!check_date(this))
            ngaysinh.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngaysinh.val())) {
            ngaysinh.datepicker("setDate", $(this).val());
        }
    });

    var ngayBD = $('#txtngaylamviectu');
    debugger;
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
    var ngayhien = new Date();
    var dd = ngayhien.getDate();
    var mm = ngayhien.getMonth() + 1;
    var yy = ngayhien.getFullYear();
    if (mm == '1' || mm == '01') {
        mm = 12;
        yy = yy - 1;
    }
    else mm = mm - 1;

    ngayBD.datepicker("setDate", "21/" + mm + "/" + yy);

    //ngayBD.change(function () {
    //    //debugger;
    //    if (!check_date(this))
    //        ngayBD.datepicker("setDate", new Date());
    //    if (check_over_date($(this).val(), ngayBD.val())) {
    //        ngayBD.datepicker("setDate", $(this).val());
    //    }
    //});

    var ngayKT = $('#txtngaylamviecden');
    ngayKT.datepicker({
        dateFormat: "dd/mm/yy",
        defaultDate: new Date(),
        dayNamesShort: $.datepicker.regional.vi.dayNamesShort,
        dayNames: $.datepicker.regional.vi.dayNames,
        monthNamesShort: $.datepicker.regional.vi.monthNamesShort,
        monthNames: $.datepicker.regional.vi.monthNames,
        showAnim: "slideDown",
        firstDay: 1 // Start with Monday
    });

    mm = ngayhien.getMonth() + 1;
    yy = ngayhien.getFullYear();
    ngayKT.datepicker("setDate", "20/"+mm+"/"+yy);
    ngayKT.change(function () {
        if (!check_date(this))
            ngayKT.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngayKT.val())) {
            ngayKT.datepicker("setDate", $(this).val());
        }
    });
   

    GetData(false);
});

function GetData(isHeader) {
    debugger;

    var tungay = $('#txtngaylamviectu').val();
    var denngay = $('#txtngaylamviecden').val();
    var thang = $('#txtthang').val();
    var maphongban = $('#filter01').val();
    var thang = $('#txtthang').val();

    var DataJson = "{'ngaylamviectu':'" + tungay + "'," +
                     "'ngaylamviecden':'" + denngay + "'," +
                      "'maphongban':'" + maphongban + "'," +
                     "'thang':'" + thang + "'" +
                   "}";
    fncGetData_Timekeeping_new(linkContent + 'Timekeeping/SelectRows_themmoi', DataJson, isHeader);
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