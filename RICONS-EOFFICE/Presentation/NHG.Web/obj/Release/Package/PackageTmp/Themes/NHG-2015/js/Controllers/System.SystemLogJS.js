var configChosenMilestones = {
    '.chosen-select-plan': { width: "290px", disable_search_threshold: 10 }
}
var dataHeader_Milestones =
[{
    col_class: 'header-box',
    col_id: '',
    col_value: [{
        colspan: 1,
        col_class: 'ovh col1',
        col_id: '',
        col_value: 'STT'
    },
    {
        colspan: 1,
        col_class: 'ovh col2',
        col_id: '',
        col_value: 'Nội dung'
    },
    {
        colspan: 1,
        col_class: 'ovh col3',
        col_id: '',
        col_value: 'Cấp độ'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Ngày Tạo'
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    $("#filter01").chosen().change(function (event) {
        GetData(false);
    });
    GetData(true);
});

function GetData(isHeader) {
    var strKichHoat = $('#chkSelectKichhoat').prop("checked") == true ? "1" : "0";
    var DataJson = "{'madonvi':" + $('#filter01').val() + "," +
                        "'timkiem':'" + $("#txtTimKiem").val().trim() + "'" +
                        "}";
    fncGetData(linkContent + 'System/ListLog', DataJson, isHeader);
}