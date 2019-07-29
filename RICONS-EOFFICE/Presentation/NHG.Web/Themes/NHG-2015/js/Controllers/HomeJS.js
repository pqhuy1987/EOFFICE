var configChosenMilestones = {
    '.chosen-select-plan': { width: "168px", disable_search_threshold: 10 }
}


$(document).ready(function (e) {
    $('#ScrollContent .boxContentFix').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

});