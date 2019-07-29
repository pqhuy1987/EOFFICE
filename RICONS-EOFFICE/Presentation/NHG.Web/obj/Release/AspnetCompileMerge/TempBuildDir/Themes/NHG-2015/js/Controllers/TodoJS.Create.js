
//$(document).ready(function (e) {
//    $('#btnSubmit').click(function (e) {
//        var postData = "{"+
//        "'data01':'" + "Newtonsoft.Json.Linq.JObject joParam" + "'," +
//        "'data02':'" + "23" + "'," +
//        "'data03':'" + "323" + "'," +
//        "'data04':'" + "222" + "'," +
//        "'data05':'" + "22222" + "'," +
//        "'data06':'" + "2222" + "'" +
//        "}";
//        fncPostData(linkContent + 'Milestones/Create', postData);
//    });
//});

var configChosenMilestones = {
    '.chosen-select-tinhtrang': { width: "147px", disable_search_threshold: 10 },
    '.chosen-select-douutien': { width: "147px", disable_search_threshold: 10 },
    '.chosen-select-nguoithuchien': { width: "147px", disable_search_threshold: 10 },
    '.chosen-select-kehoach': { width: "147px", disable_search_threshold: 10 },
    '.chosen-select-level': { width: "147px", disable_search_threshold: 10 }
}

$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    funcMfUpload($('#upload'), linkContent + "ajax/upload.ashx");
    $('#btnUpload').click(function () {
        $('#mf_file_upload').click();
    });
});

function fncToHTMLFile(fileName, fileSize, fileIcon) {
    var hiddenFileName = $('<input type="hidden" name="fileName" />').val(fileName);
    var hiddenTT = $('<input type="hidden" name="tt" value="vb1" />');
    var fileIcon = $('<div class="fileIcon" ></div>').append('<img id="img-icon" src="templates/system/images/filetype/' + fileIcon + '" width="30" height="30"/>');
    var fileName = $('<div class="fileName" title="' + fileName + '"></div>').append(fileName);
    var fileSize = $('<div class="fileSize"></div>').append(SetFileSize(fileSize));
    var btnRemove = $('<button type="button" class="btn-trash start" onclick="RemoveFile(this)"></button>').append('<i class="icon-trash icon-white"></i>').append('<span>Xóa</span>');
    var fileButton = $('<div class="fileButton" ></div>').append(btnRemove);
    var formDownload = $('<form action="' + linkContent + 'ajax/download/download.ashx" class="fileDownloadForm" method="post"></form>');
    formDownload.append(hiddenFileName);
    formDownload.append(hiddenTT);
    formDownload.append(fileIcon);
    formDownload.append(fileName);
    formDownload.append(fileSize);
    //formDownload.append(fileButton);
    var queueItem = $('<div class="fileUploadQueueItem" ></div>').append(formDownload).append(fileButton);
    $('#filesToUploadQueue').append(queueItem);
}

function fncToHTMLFile_Save(fileName, fileSize, fileIcon, maVanBan) {
    var hiddenFileName = $('<input type="hidden" name="fileName" />').val(fileName);
    var hiddenTT = $('<input type="hidden" name="tt" value="vb2"/>');
    var hiddenMVB = $('<input type="hidden" name="mvb" />').val(maVanBan);
    var fileIcon = $('<div class="fileIcon" ></div>').append('<img id="img-icon" src="templates/system/images/filetype/' + fileIcon + '" width="30" height="30"/>');
    var fileName = $('<div class="fileName" title="' + fileName + '"></div>').append(fileName);
    var fileSize = $('<div class="fileSize"></div>').append(SetFileSize(fileSize));
    var btnRemove = $('<button type="button" class="btn-trash start" onclick="RemoveFile(this)"></button>').append('<i class="icon-trash icon-white"></i>').append('<span>Xóa</span>');
    var fileButton = $('<div class="fileButton" ></div>').append(btnRemove);
    var formDownload = $('<form action="' + linkContent + 'ajax/download/download.ashx" class="fileDownloadForm" method="post"></form>');
    formDownload.append(hiddenFileName);
    formDownload.append(hiddenTT);
    formDownload.append(hiddenMVB);
    formDownload.append(fileIcon);
    formDownload.append(fileName);
    formDownload.append(fileSize);
    //formDownload.append(fileButton);
    var queueItem = $('<div class="fileUploadQueueItem" ></div>').append(formDownload).append(fileButton);
    $('#filesToUploadQueue').append(queueItem);
}