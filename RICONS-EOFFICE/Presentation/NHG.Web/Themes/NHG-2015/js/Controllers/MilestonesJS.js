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
        col_value: 'Nội dung mục tiêu'
    },
    {
        colspan: 1,
        col_class: 'ovh col4',
        col_id: '',
        col_value: 'TT(%)'
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
        col_value: 'T7'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T8'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T9'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T10'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T11'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T12'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T1'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T2'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T3'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T4'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T5'
    },
    {
        colspan: 1,
        col_class: 'ovh col6',
        col_id: '',
        col_value: 'T6'
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

    $("#btnHieuChinh").click(function () {       
        var listChecked = $('.chkCheck:checked');
        if (listChecked.length == 1)
        {
            var test = $(this).attr("href");
            $(this).attr("href",$(listChecked[0]).parent().next().next().children().attr("href"));
        }
        else
            return false;
    });

    $("#btnDeleted").click(function () {
        var listChecked = $('.chkCheck:checked');
        if (listChecked.length == 1) {
            var test = $(this).attr("href");
            $(this).attr("href", $(listChecked[0]).parent().next().next().children().attr("href"));
        }
        else
            return false;
    });

});

function GetData(isHeader, mkh, stt) {
    var makehoach = CheckNullOrEmpty(mkh) == true ? "0" : mkh;
    var STT = CheckNullOrEmpty(stt) == true ? "0" : stt;
    var DataJson = "{'madonvi':'" + $('#filter01').val() + "'," +
                    "'makehoach':'" + makehoach + "'," +
                    "'stt':'" + STT + "'," +
                    "'timkiem':'" + $("#txtTimKiem").val().trim() + "'" +
                   "}";
    fncGetData(linkContent + 'Milestones/SelectRows', DataJson, isHeader);
}

function ShowSubLine(e) {
    //tim nhung dong sub
    var maKeHoach = $(e).attr('mkh');
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