
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
        //GetData(false);
    });

    var ngayBD = $('#txtngayxinnghitu');
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
    ngayBD.datepicker("setDate", new Date());

    ngayBD.change(function () {
        //debugger;
        if (!check_date(this))
            ngayBD.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngayBD.val())) {
            ngayBD.datepicker("setDate", $(this).val());
        }
    });

    var ngayKT = $('#txtngayxinnghiden');
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
    ngayKT.datepicker("setDate", new Date());
    ngayKT.change(function () {
        if (!check_date(this))
            ngayKT.datepicker("setDate", new Date());
        if (check_over_date($(this).val(), ngayKT.val())) {
            ngayKT.datepicker("setDate", $(this).val());
        }
    });
   

    $("#txtsongayxinnghi").on('focusout', function () {
        debugger;
        if (isNaN($(this).val().replace(",", ".").replace(",", ".")) == false) {
            document.getElementById('txtsongayxinnghi').value = $(this).val().replace(",", ".").replace(",", ".");
            var ngayBD = $('#txtngayxinnghitu').val();
            var songay = parseFloat($(this).val());
            if ($(this).val() != "100" && $(this).val() != "0.50") {

                var parts = ngayBD.split("/");
                var tnn = parts[1] + "/" + parts[0] + "/" + parts[2];
                var date = new Date(tnn);
                var aaaa = new Date(tnn);
                var i = 0;
                var congngay = 0;
                for (i = 0; i < songay; i++) {
                    if (i != 0)
                        date.setDate(date.getDate() + 1);
                    switch (date.getDay()) {

                        case 0:
                            {
                                songay = songay + 1;
                                break;
                            }
                        case 6:
                            {
                                songay = songay + 0.5;
                                break;
                            }
                    }
                }

                if (songay > 1) songay = songay - 1;
                //date.setDate(date.getDate() + songay+congngay);
                var dd = date.getDate();
                var mm = date.getMonth() + 1;
                var yy = date.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                var someFormattedDate = dd + '/' + mm + '/' + yy;
                document.getElementById('txtngayxinnghiden').value = someFormattedDate;
            }
            else {
                document.getElementById('txtngayxinnghiden').value = $('#txtngayxinnghitu').val();
            }

            var parts = ngayBD.split("/");
            var tnn = parts[1] + "/" + parts[0] + "/" + parts[2];
            var date = new Date(tnn);

            songay = parseFloat($(this).val());

            var songayphepconlai = parseFloat($('#txtsongaynghi').val());

            if (songayphepconlai + songay == 0)
                $('#chknghiphep').prop('checked', false);

            var songayphephientai = songayphepconlai + 1 - songay;
            if (songayphephientai < 0)
                $("#chknghikhongluong").prop('checked', true);
            else $("#chknghikhongluong").prop('checked', false);
            document.getElementById('txtsongayphepconlai').value = songayphephientai;
        }
    });

    $("#btnHieuChinh").click(function () {

        var listChecked = $('.chkCheck:checked');
        console.log(listChecked);
        var ID = $(listChecked[0]).attr('mpb');
        console.log(ID);
        //return false;
        
        if (listChecked.length == 1) {
           

            return $(this).attr("href", $(this).attr('href') + '/?maphongban='+encodeURIComponent( ID ) );
        }
        else
            return false;
    });

    $("#btnDeleted").click(function () {
        var listChecked = $('.chkCheck:checked');
        console.log(listChecked);
        var ID = $(listChecked[0]).attr('mpb');
        console.log(ID);
        //return false;

        if (listChecked.length == 1) {


            return $(this).attr("href", $(this).attr('href') + '/?maphongban=' + encodeURIComponent(ID));
        }
        else
            return false;
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
    fncGetData(linkContent + 'Danhmuc/SelectRows', DataJson, isHeader);
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