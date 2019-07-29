
var configChosenDanhsachnghiphep = {
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
        col_value: 'Mã ĐK'
    },

    {
        colspan: 1,
        col_class: 'ovh col17',
        col_id: '',
        col_value: 'Mã NV'
    },

    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Họ và tên'
    },
    {
        colspan: 1,
        col_class: 'ovh col5',
        col_id: '',
        col_value: 'Ngày sinh'
    },
    //{
    //    colspan: 1,
    //    col_class: 'ovh col6',
    //    col_id: '',
    //    col_value: 'Tên chức danh'
    //},
    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: 'Tên phòng ban'
    },
    {
        colspan: 1,
        col_class: 'ovh col8',
        col_id: '',
        col_value: 'Nghỉ từ'
    },
    {
        colspan: 1,
        col_class: 'ovh col9',
        col_id: '',
        col_value: 'Số ngày'
    },
    {
        colspan: 1,
        col_class: 'ovh col10',
        col_id: '',
        col_value: 'Nghỉ đến'
    },
    //{
    //    colspan: 1,
    //    col_class: 'ovh col11',
    //    col_id: '',
    //    col_value: 'Phép CL'
    //},
    {
        colspan: 1,
        col_class: 'ovh col12',
        col_id: '',
        col_value: 'Lý do nghỉ'
    },
    {
        colspan: 1,
        col_class: 'ovh col13',
        col_id: '',
        col_value: 'Loại phép'
    },
    {
        colspan: 1,
        col_class: 'ovh col14',
        col_id: '',
        col_value: 'Trưởng BP'
    },
    {
        colspan: 1,
        col_class: 'ovh col15',
        col_id: '',
        col_value: 'Ban GĐ'
    },
    {
        colspan: 1,
        col_class: 'ovh col16',
        col_id: '',
        col_value: 'XN VLV'
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenDanhsachnghiphep) {
        $(selector).chosen(configChosenDanhsachnghiphep[selector]);
    }

    $("#filter01").chosen().change(function () {
        document.getElementById('txttrangthu').value = 1;
        GetData(false);
    });

    //$(".divExentd").click(function () {
    //    var aa= $('#girdInfo span').text();
    //    var currentRow = $('#girdInfo span.currentRow').text();
    //    GetData(false,"","",currentRow);
    //});

    var ngayBD = $('#txtnghitu');
    //debugger;
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
    var date = new Date();
    var yy = date.getFullYear();
    ngayBD.datepicker("setDate", "01/01/" +yy);

    ngayBD.change(function () {
        //debugger;
        //if (!check_date(this))
        //    ngayBD.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngayBD.val())) {
            ngayBD.datepicker("setDate", $(this).val());
        }
    });

    var ngayKT = $('#txtnghiden');
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
    var yy = date.getFullYear();
    ngayKT.datepicker("setDate", "31/12/" + yy);
    ngayKT.change(function () {
        //if (!check_date(this))
        //    ngayKT.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngayKT.val())) {
            ngayKT.datepicker("setDate", $(this).val());
        }
    });


    $("#btnHieuChinh").click(function () {
        var listChecked = $('.chkCheck:checked');
        var duyet = $(listChecked[0]).attr('duyet');
        var ID = $(listChecked[0]).attr('mdk');
        if (listChecked.length ==2)
        {
            duyet = $(listChecked[1]).attr('duyet');
            ID = $(listChecked[1]).attr('mdk');
            if (duyet != "1") {
                return $(this).attr("href", $(this).attr('href') + '/?madangky=' + encodeURIComponent(ID));
            }
            else {
                alert('Đơn xin nghỉ phép đã duyệt hoặc chọn không đúng 1 dòng');
                return false;
            }
        }
        else if (duyet != "1" && listChecked.length ==1) {
            return $(this).attr("href", $(this).attr('href') + '/?madangky=' + encodeURIComponent(ID));
        }
        else {
            alert('Đơn xin nghỉ phép đã duyệt hoặc chọn không đúng 1 dòng');
            return false;
        }
    });
    
    $("#btnDeleted").click(function () {
        var listChecked = $('.chkCheck:checked');
        var duyet = $(listChecked[0]).attr('duyet');
        var ID = $(listChecked[0]).attr('mdk');
        if (listChecked.length == 2) {
            duyet = $(listChecked[1]).attr('duyet');
            ID = $(listChecked[1]).attr('mdk');
            if (duyet != "1") {
                return $(this).attr("href", $(this).attr('href') + '/?madangky=' + encodeURIComponent(ID));
            }
            else {
                alert('Đơn xin nghỉ phép đã duyệt hoặc chọn không đúng 1 dòng.');
                return false;
            }
        }
        else if (duyet != "1" && listChecked.length == 1) {
            return $(this).attr("href", $(this).attr('href') + '/?madangky=' + encodeURIComponent(ID));
        }
        else {
            alert('Đơn xin nghỉ phép đã duyệt hoặc chọn không đúng 1 dòng');
            return false;
        }
    });

    $('#txtnghitu').on('keypress', function (event) {
        //debugger;
        document.getElementById('txttrangthu').value = 1;
        var keycode = (event.keyCode ? event.keyCode : event.which);
        var nghitu = $('#txtnghitu').val();
        var nghiden = $('#txtnghiden').val();
        if (keycode == '13') {
            GetData(false);
        }
    });
    $('#txtnghiden').on('keypress', function (event) {
        //debugger;
        document.getElementById('txttrangthu').value = 1;
        var keycode = (event.keyCode ? event.keyCode : event.which);
        var nghitu = $('#txtnghitu').val();
        var nghiden = $('#txtnghiden').val();
        if (keycode == '13') {
            GetData(false);
        }
    });

    $('#txttrangthu').on('keypress', function (event) {
        //debugger;
        var keycode = (event.keyCode ? event.keyCode : event.which);
        var trangthu = $('#txttrangthu').val();
        var currentRow = $('#girdInfo span.Pages').text();
        if (keycode == '13' && parseInt(trangthu) <= parseInt(currentRow)) {
            GetData(false);
        }
    });

    $("#btnNext").click(function () {
        debugger;
        var trangthu = $('#txttrangthu').val();
        var currentRow = $('#girdInfo span.Pages').text();
        if (parseInt(trangthu) < parseInt(currentRow)) {
            document.getElementById('txttrangthu').value = parseInt(trangthu) + 1;
            GetData(false);
        }
        else if (parseInt(trangthu) == parseInt(currentRow) && parseInt(trangthu) == 0 && currentRow>0) {
            document.getElementById('txttrangthu').value = parseInt(trangthu) + 1;
            GetData(false);
        }
    });

    $("#btnPre").click(function () {
        debugger;
        var trangthu = $('#txttrangthu').val();
        if (parseInt(trangthu) >1) {
            document.getElementById('txttrangthu').value = parseInt(trangthu) - 1;
            GetData(false);
        }
    });

    GetData(true);
});

function GetData(isHeader, mkh, stt, currentRow) {
    debugger;
    var DataJson = "{'maphongban':'" + $('#filter01').val() + "'," +
                     "'nghitu':'" + $('#txtnghitu').val() + "'," +
                     "'nghiden':'" + $('#txtnghiden').val() + "'," +
                      "'curentPage':'" + $("#txttrangthu").val() + "'" +
                    "}";
    fncGetData_Danhsachnghiphep(linkContent + 'Absent/SelectRows_Danhsachnghiphep', DataJson, isHeader);
}

function ShowSubLine(e) {
    //tim nhung dong sub
    var mdk = $(e).attr('mdk');
    var tableContent = $("#tableContent");
    var rows = tableContent.find('tr[subparent="' + mdk + '"]');
    if(rows.length > 0)
    {
        rows.slideToggle();
    }
    else
    {
        GetData(false, mdk, $(e).parent().parent().find(".stt").html().trim());
    }
}