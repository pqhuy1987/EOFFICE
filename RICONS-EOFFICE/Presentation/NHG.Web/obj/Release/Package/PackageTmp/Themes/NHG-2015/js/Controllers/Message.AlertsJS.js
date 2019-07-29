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
        col_value: 'Ngày thông báo'
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    $("#filter01").chosen().change(function () {
        GetData(false, 0);
    });
    GetData(true, 0);

    $('#girdInfoLoad .divExentd').click(function () {
        GetData(false, GetCurrentPage());
    });
});

function GetData(isHeader, page) {
    var DataJson = "{'noidung':'" + $("#txtTimKiem").val().trim() + "'," +
                   "'curentPage':" + page + "" +
                   "}";
    fncGetData(linkContent + 'Message/SelectAlerts', DataJson, isHeader);
}