var configChosenMilestones = {
    '.chosen-select-plan': { width: "290px", disable_search_threshold: 10 }
}
var dataHeader_Report2 =
    [
        {
            colspan: 1,
            col_class: 'ovh col1',
            col_id: '',
            col_value: '<input type="checkbox" onclick="SelectAll(this);" class="chkCheck"/>'
        },
        {
            colspan: 1,
            col_class: 'ovh col1',
            col_id: '',
            col_value: 'STT'
        },
        {
            colspan: 1,
            col_class: 'ovh col2',
            col_id: '',
            col_value:'Nội dung báo cáo'
        },
        {
            colspan: 1,
            col_class: 'ovh col3',
            col_id: '',
            col_value: 'Người thực hiện'
        },
        {
            colspan: 1,
            col_class: 'ovh col4',
            col_id: '',
            col_value: 'Ngày báo cáo'
        }
    ];

$(document).ready(function (e) {
    GetData(true);
});
function GetData(isHeader) {
    var DataJson = "{'madonvi':" + $('#filter01').val() + "," +
                     "'timkiem':'" + $("#txtTimKiem").val().trim() + "'" +
                   "}";
    fncGetData(linkContent + 'Milestones/SelectRows_report2', DataJson, isHeader);
}