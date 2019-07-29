var configChosenMilestones = {
    '.chosen-select-donvi': { width: "290px", disable_search_threshold: 10 },
    '.chosen-select-phongban': { width: "290px", disable_search_threshold: 10 }
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
        col_class: 'ovh col2',
        col_id: '',
        col_value: 'STT'
    },
    {
        colspan: 1,
        col_class: 'ovh col3',
        col_id: '',
        col_value: 'Họ & tên'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'Tên đăng nhập'
    },
    {
        colspan: 1,
        col_class: 'ovh col5',
        col_id: '',
        col_value: 'Phòng ban'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'Tình trạng'
    },
    {
        colspan: 1,
        col_class: 'ovh col7',
        col_id: '',
        col_value: ''
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

    fncGetData(linkContent + 'Account/SelectRows');
});
