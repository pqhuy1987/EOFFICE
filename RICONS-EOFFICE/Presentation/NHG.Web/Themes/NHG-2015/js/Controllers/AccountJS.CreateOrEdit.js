
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
    '.chosen-select-kehoach': { width: "250px", disable_search_threshold: 10 },
    '.chosen-select-level': { width: "147px", disable_search_threshold: 10 }
}


$(document).ready(function (e) {
    $('#ScrollContent .boxTable2').slimscroll({ height: getWindowHeight() - $('.nhg-body header.header').height() - $(".HeaderFixed").height() });
    //chosen combobox jquery
    for (var selector in configChosenMilestones) {
        $(selector).chosen(configChosenMilestones[selector]);
    }

    //funcMfUpload($('#upload'), linkContent + "ajax/upload.ashx");
    //$('#btnUpload').click(function () {
    //    $('#mf_file_upload').click();
    //});

    $("#chkSelectKichhoat").change(function () {
        if ($('#chkSelectKichhoat').prop("checked")) {
            $('#spKichhoat').html("Kích hoạt");
        }
        else {
            $('#spKichhoat').html("Không kích hoạt");
        }
    });

    var ngayBD = $('#txtNgayDangKy');
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



    //fncTaoSuKienEdit();
    
    //notifyOnBrowser();
});

//ham tao su kien form edit
function fncTaoSuKienEdit() {
    var ngayBD = $('#txtNgayDangKy');
    var gio_start = $('#txtTuGio');
    var ngayKT = $('#txtDenNgay');
    var gio_end = $('#txtDenGio');
    var timeSelect = jQuery.parseJSON('['+ Encoder.htmlDecode(jsTime) + ']');
    //tuy chinh cho gio
    var chosenDrop = $(".chosen-drop-grid").css({
        "width": 70,
        'display': 'none'
    });
    var lstChoice = $("<ul class='chosen-results'></ul>");
    $.each(timeSelect, function () {
        lstChoice.append('<li class="active-result" data-option-array-index="' + this.id + '">' + this.value + '</li>');
    });
    chosenDrop.append(lstChoice);

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
    if (CheckNullOrEmpty(ngayBD.val())) {
        ngayBD.datepicker("setDate", new Date());
    }
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
    if (CheckNullOrEmpty(ngayKT.val())) {
        ngayKT.datepicker("setDate", new Date());
    }
    ngayKT.change(function () {
        if (!check_date(this))
            ngayKT.datepicker("setDate", new Date());
        if (check_over_date(ngayBD.val(), $(this).val())) {
            ngayBD.datepicker("setDate", $(this).val());
        }
    });

//    //thoi gian bat dau
    gio_start.click(function (e) {
        $(this).focus();
        fncPostionDropDown(chosenDrop, gio_start);
        fncShowDropDown();
        fncEventListChoice(lstChoice, chosenDrop, gio_start);

    });
    gio_start.change(function () {
        fncResetNgayGioKetThuc(false);
        if (CheckNullOrEmpty($("#txtNgay_start").val()))
            fncResetNgayGioKetThuc(true);
    });
    gio_start.keydown(function (evt) {
        fncPostionDropDown(chosenDrop, gio_start);
        fncShowDropDown();
        var stroke;
        stroke = evt.keyCode;
        switch (stroke) {
            case 8:
                //keyboard backspace                
                break;
            case 13:
                ////keyboard enter
                //$(e).attr('selectedid', lstChoice.find('.highlighted').attr('data-option-array-index'));
                //fncUpdateValueSelectedToGrid();
                break;
            case 38:
                //keyboard arrow up
                fncSetHighlightedSelection(lstChoice, null, "up");
                evt.preventDefault();
                break;
            case 40:
                //keyboard arrow down                        
                fncSetHighlightedSelection(lstChoice, null, "down");
                evt.preventDefault();
                break;
        }
    });
    gio_start.keyup(function (e) {
        //kiem tra phim muoi ten
        var stroke, checkKeyArrow;
        checkKeyArrow = false;
        stroke = e.keyCode;
        switch (stroke) {
            case 38://keyboard arrow up
            case 40://keyboard arrow down                        
                checkKeyArrow = true;
                break;
        }
        if (!checkKeyArrow) {
            if (!CheckNullOrEmpty($(this).val())) {
                var searchText = locdau($(this).val(), false).replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
                var escapedSearchText = locdau($(this).val(), false);
                var matcher = new RegExp(searchText, "i");
                //tim chuoi va tra ve danh sach
                var a = $.grep(timeSelect, function (item, index) {
                    return matcher.test(locdau(Encoder.htmlDecode(item.value)));
                });
                if (a.length > 0) {
                    lstChoice.html('');
                    var firstHighlighted = true;
                    $.each(a, function (stt) {
                        var valueItem = Encoder.htmlDecode(this.value);
                        var startpos = locdau(valueItem, false).search(matcher);
                        if (this.id !== "-1") {
                            var text = valueItem.substr(0, startpos + escapedSearchText.length) + '</em>' + valueItem.substr(startpos + escapedSearchText.length);
                            var search_text = text.substr(0, startpos) + '<em>' + text.substr(startpos);
                            if (firstHighlighted) {
                                firstHighlighted = false;
                                lstChoice.append('<li class="active-result highlighted" data-option-array-index="' + this.id + '">' + search_text + '</li>');
                            }
                            else
                                lstChoice.append('<li class="active-result" data-option-array-index="' + this.id + '">' + search_text + '</li>');
                        }
                    });
                }
                else {
                    lstChoice.html('');
                    lstChoice.append('<li class="active-result" data-option-array-index="">' + $(this).val() + '</li>');
                }
                fncEventListChoice(lstChoice, chosenDrop, gio_start);
            }
        }
    });
//    //thoi gian ket thuc
    gio_end.click(function () {
        $(this).focus();
        fncPostionDropDown(chosenDrop, gio_end);
        fncShowDropDown();
        fncEventListChoice(lstChoice, chosenDrop, gio_end);
    });
    gio_end.keydown(function (evt) {
        fncPostionDropDown(chosenDrop, gio_end);
        fncShowDropDown(evt);
        var stroke;
        stroke = evt.keyCode;
        switch (stroke) {
            case 8:
                //keyboard backspace                
                break;
            case 13:
                ////keyboard enter
                //$(e).attr('selectedid', lstChoice.find('.highlighted').attr('data-option-array-index'));
                //fncUpdateValueSelectedToGrid();
                break;
            case 38:
                //keyboard arrow up
                fncSetHighlightedSelection(lstChoice, null, "up");
                evt.preventDefault();
                break;
            case 40:
                //keyboard arrow down                        
                fncSetHighlightedSelection(lstChoice, null, "down");
                evt.preventDefault();
                break;
        }
    });
    gio_end.keyup(function (e) {
        if (!CheckNullOrEmpty($(this).val())) {
            var searchText = locdau($(this).val(), false).replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
            var escapedSearchText = locdau($(this).val(), false);
            var matcher = new RegExp(searchText, "i");
            //tim chuoi va tra ve danh sach
            var a = $.grep(timeSelect, function (item, index) {
                return matcher.test(locdau(Encoder.htmlDecode(item.value)));
            });
            if (a.length > 0) {
                lstChoice.html('');
                var firstHighlighted = true;
                $.each(a, function (stt) {
                    var valueItem = Encoder.htmlDecode(this.value);
                    var startpos = locdau(valueItem, false).search(matcher);
                    if (this.id !== "-1") {
                        var text = valueItem.substr(0, startpos + escapedSearchText.length) + '</em>' + valueItem.substr(startpos + escapedSearchText.length);
                        var search_text = text.substr(0, startpos) + '<em>' + text.substr(startpos);
                        if (firstHighlighted) {
                            firstHighlighted = false;
                            lstChoice.append('<li class="active-result highlighted" data-option-array-index="' + this.id + '">' + search_text + '</li>');
                        }
                        else
                            lstChoice.append('<li class="active-result" data-option-array-index="' + this.id + '">' + search_text + '</li>');
                    }
                });
            }
            else {
                lstChoice.html('');
                lstChoice.append('<li class="active-result" data-option-array-index="">' + $(this).val() + '</li>');
            }
            fncEventListChoice(lstChoice, chosenDrop, gio_end);
        }
    });

    funcMfUpload($('#upload'), "ajax/giaoviec/upload.ashx");

    $('#btnSave').click(function (e) {
        var DataJson = "";
        var fileNames = "";
        var isHoanThanh = $('#chkHoanThanh').is(':checked') == true ? "1" : "0";
        if ($('.fileUploadQueueItem').length > 0)
            fileNames = $('.fileUploadQueueItem .fileName').map(function () { return $(this).html().trim(); }).get().join(',');

        var strTypeInsert = "insert";
        if (!CheckNullOrEmpty($('#hdMa').val()))
            strTypeInsert = "update";

        DataJson += "{";
        DataJson += "'type':'" + strTypeInsert + "',";
        DataJson += "'macongviec':'" + Encoder.htmlEncode($('#hdMa').val()) + "',";
        DataJson += "'tieude':'" + Encoder.htmlEncode($('#tieude').val()) + "',";
        DataJson += "'ngaybatdau':'" + Encoder.htmlEncode($('#ngaybatdau').val()) + "',";
        DataJson += "'giobatdau':'" + Encoder.htmlEncode($('#giobatdau').val()) + "',";
        DataJson += "'ngayketthuc':'" + Encoder.htmlEncode($('#ngayketthuc').val()) + "',";
        DataJson += "'gioketthuc':'" + Encoder.htmlEncode($('#gioketthuc').val()) + "',";
        DataJson += "'xulychinh':'" + $('#xulychinh').val() + "',";
        DataJson += "'phoihopxuly':'" + $('#phoihopxuly').val() + "',";
        DataJson += "'theodoi':'" + $('#theodoi').val() + "',";
        DataJson += "'hoanthanh':'" + isHoanThanh + "',";
        DataJson += "'taptin':'" + fileNames + "',";
        DataJson += "'ghichu':'" + Encoder.htmlEncode($('#ghichu').val()) + "'";
        DataJson += "}";
        $('#hdMa').val('');
        fncRequestToServer(DataJson);
    });
    $('#btnCancel').click(function (e) {
        $('.divFormEdit').html('');
        fncLoadFormEdit();
    });

}

function fncToHTMLFile(fileName, fileSize, fileIcon) {
    var hiddenFileName = $('<input type="hidden" name="fileName" />').val(fileName);
    var hiddenTT = $('<input type="hidden" name="tt" value="vb1" />');
    var fileIcon = $('<div class="fileIcon" ></div>').append('<img id="img-icon" src="' + linkContent + 'themes/nhg-2015/images/filetype/' + fileIcon + '" width="30" height="30"/>');
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
    var fileIcon = $('<div class="fileIcon" ></div>').append('<img id="img-icon" src="' + linkContent + 'themes/nhg-2015/images/filetype/' + fileIcon + '" width="30" height="30"/>');
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

//ham set vi tri combobox chon gio
function fncPostionDropDown(chosenDrop, objUpdateValue) {
    chosenDrop = $(".chosen-drop-grid").css({
        "top": objUpdateValue.position().top + objUpdateValue.outerHeight(true),
        "left": objUpdateValue.position().left,
    });
}