var titlepage = " - Văn Phòng Điện Tử";
$(document).ready(function (e) {
    
});

function fncGetDataJsonForSearch(stringJsonName) {
    var dataSource;
    switch (stringJsonName) {
        case "jsPhongBan":
            dataSource = jsPhongBan;
            break;
        case "jsDiaDiem":
            dataSource = jsDiaDiem;
            break;
        case "jsLanhDao":
            dataSource = jsLanhDao;
            break;
        case "jsThanhPhan":
            dataSource = jsThanhPhan;
            break;
        default:
            dataSource = null;
            break;
    }
    return dataSource;
}

function fncFixHeightSize() {
    var iHeightOver = getWindowHeight() - $('#top').outerHeight(true) - $('#funcButtons').outerHeight(true) - $('#page-notices').outerHeight(true) - $('#bottom').outerHeight(true) - 1;
    $('#main-menu').css('height', iHeightOver);
    $('#pnlContent').css('height', iHeightOver);
    $("body").css('overflow', 'hidden');
    $('#center').css('height', iHeightOver);
    $('#main-menu').slimscroll({ height: '"' + iHeightOver.toString() + '"' });
    $('#Page #center #center-right').css("width", $("body").outerWidth(true) - $('#Page #center #center-left').outerWidth(true));

}

function funcGetCenTer(windowWidth, offsetLeft, imgWidth, popupWidth) {
    //lay vi tri giua man hinh
    var iOffsetLeftCenter = offsetLeft + (imgWidth / 2);
    return popupWidth - (getWindowWidth() - iOffsetLeftCenter) - 10;
}

//scroll #pnlContent to .table_danhSach (danh sach lich lam viec da dang ky)
function fncScrollToContent() {
    $('#pnlContent').animate({
        scrollTop: $('.divFormEdit').outerHeight(true)
    }, 800);
}

//scroll #pnlContent to top
function fncScrollToTop() {
    $('#pnlContent').animate({
        scrollTop: 0
    }, 800);
}
