var configChosenMilestones = {
    '.chosen-select-nguoithuchien': { width: "568px", disable_search_threshold: 10 },
    '.chosen-select-loaikehoach': { width: "568px", disable_search_threshold: 10 }
}

$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    var ngayBD = $('#txtNgayBatDau');
    var ngayKT = $('#txtNgayKetThuc');

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
        if (check_over_date(ngayBD.val(), $(this).val())) {
            ngayBD.datepicker("setDate", $(this).val());
        }
    });
});

//ham set vi tri combobox chon gio
function fncPostionDropDown(chosenDrop, objUpdateValue) {
    chosenDrop = $(".chosen-drop-grid").css({
        "top": objUpdateValue.position().top + objUpdateValue.outerHeight(true),
        "left": objUpdateValue.position().left,
    });
}