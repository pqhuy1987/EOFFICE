
var configChosenMilestones = {
    '.chosen-select-plan': { width: "230px", disable_search_threshold: 10 }
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
    //{
    //    colspan: 1,
    //    col_class: 'ovh col2',
    //    col_id: '',
    //    col_value: 'STT'
    //},
    {
        colspan: 1,
        col_class: 'ovh col3',
        col_id: '',
        col_value: 'Nội dung mục tiêu'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Ts(%)'
    },
    {
        colspan: 1,
        col_class: 'ovh col5',
        col_id: '',
        col_value: 'Chỉ tiêu năm'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'Bắt đầu'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'Kết thúc'
    },
    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: 'Đánh giá'
    },
    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: 'Kết quả'
    },
    {
        colspan: 1,
        col_class: 'ovh col8',
        col_id: '',
        col_value: 'Tình trạng'
    }]
}];


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    $("#filter01").chosen().change(function () {
        444
    });
    GetData(true);
});

function GetData(isHeader) {
    var strKichHoat = $('#chkSelectKichhoat').prop("checked") == true ? "1" : "0";
    var DataJson = "{'madonvi':" + $('#filter01').val() + "," +
                        "'timkiem':'" + $("#txtTimKiem").val().trim() + "'" +
                        "}";
    fncGetData(linkContent + 'Task/SelectRows', DataJson, isHeader);
}