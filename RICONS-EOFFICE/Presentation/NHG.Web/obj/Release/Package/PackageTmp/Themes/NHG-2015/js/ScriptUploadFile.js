function fileSelected() {
    $('#filesToUploadQueue').html('');
    fileNames = "";
    var arrName = [];
    for (i = 0; i < document.getElementById('filesToUpload').files.length; i++) {
        var file = document.getElementById('filesToUpload').files[i];
       
        arrName.push(file.name);
        if (file) {
            var fileSize = 0;
            if (file.size > 1024 * 1024)
                fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
            else
                fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
        }
        $('#filesToUploadQueue').append('<div class="fileUploadQueueItem" id="filesToUploadEMDTEL">' +
                '<div class="fileIcon" ><img id="img-icon" src="templates/system/images/loading.gif" width="30" height="31"/></div>' +
                '<div class="fileName" >' + file.name + '</div>' +
                '<div class="fileSize"> ' + fileSize + '</div>' +
                '<div class="fileButton" >' +
                    '<button type="button" class="btn btn-trash start">' +
                    '<i class="icon-trash icon-white"></i>' +
                    '<span>Xóa</span>' +
                '</button>' +
                '</div></div>');

    }
    uploadFile('ajax/upload/upload.ashx', 'filesToUpload');
    return arrName.join(',');
}

function uploadFile(strLink) {
    if (document.getElementById('filesToUpload').files.length > 0) {
        var fd = new FormData();
        for (i = 0; i < document.getElementById('filesToUpload').files.length; i++) {
            fd.append("filesToUpload", document.getElementById('filesToUpload').files[i])
        }
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);
        xhr.open("POST", strLink); xhr.send(fd)
    }
};

function uploadFile(strLink, strobj) {
    if (document.getElementById(strobj).files.length > 0) {
        var fd = new FormData();
        for (i = 0; i < document.getElementById(strobj).files.length; i++) {
            fd.append("filesToUpload", document.getElementById(strobj).files[i])
        }
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);
        xhr.open("POST", strLink); xhr.send(fd)
    }
};

function uploadProgress(evt) {
    if (evt.lengthComputable) {
        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        //document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';
        //document.getElementById('prog').value = percentComplete;
    }
    else {
        //document.getElementById('progressNumber').innerHTML = 'unable to compute';
    }
}

function uploadFailed(evt) {
    alert('uploadfailed');
}

function uploadCanceled(evt) {
    alert('cancel');
}

function deleleFile() {
    document.getElementById('filesToUpload').files;
    var list = document.getElementById('fileList');
    while (list.hasChildNodes()) {
        list.removeChild(ul.firstChild)
    }
}

function HandleBrowseClick() {
    var fileinput = document.getElementById("filesToUpload"); fileinput.click();
}

function SetFileSize(fileSize) {
    if (parseInt(fileSize) > 1024 * 1024)
        return (Math.round(parseInt(fileSize) * 100 / (1024 * 1024)) / 100).toString() + 'MB';
    else
        return (Math.round(parseInt(fileSize) * 100 / 1024) / 100).toString() + 'KB';
}

function RemoveFile(obj) {
    $($(obj).parent().parent()).fadeOut("slow", function () {
        $($(obj).parent().parent()).remove();
    });
}