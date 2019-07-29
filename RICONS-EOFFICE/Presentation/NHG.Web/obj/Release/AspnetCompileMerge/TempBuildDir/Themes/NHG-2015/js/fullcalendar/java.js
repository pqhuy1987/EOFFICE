// JavaScript Document
$(document).ready(function() {
    //Load menu
	$('#main-menu > .menu-item > .menu-item-text').hover(function(){
	    $(this).parent().children('.menu-item-hover').css("background-color", "Red");
	    },
	    function(){
	        $(this).parent().children('.menu-item-hover').css("background-color", "#420100");
	    }
    );
    
    $('#checknhacnho').click(function(e){
        if($(this).is(':checked'))
            $('#nhacnho').css('display', 'inline-block');
        else
            $('#nhacnho').css('display', 'none');
    });
    
    $('#checknhacnhoUp').click(function(e){
        if($(this).is(':checked'))
            $('#nhacnhoUp').css('display', 'inline-block');
        else
            $('#nhacnhoUp').css('display', 'none');
    });
    
    $('#checkCaNgay').click(function(e){
        if($(this).is(':checked'))
        {
            $('#startHour').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#startMin').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#startMeridiem').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#endHour').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#endMin').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#endMeridiem').css('opacity', '0.5').attr('disabled', 'disabled');
        }
        else
        {
            $('#startHour').css('opacity', '1').removeAttr('disabled');
            $('#startMin').css('opacity', '1').removeAttr('disabled');
            $('#startMeridiem').css('opacity', '1').removeAttr('disabled');
            $('#endHour').css('opacity', '1').removeAttr('disabled');
            $('#endMin').css('opacity', '1').removeAttr('disabled');
            $('#endMeridiem').css('opacity', '1').removeAttr('disabled');
        }
    });

    $('#checkCaNgayUp').click(function (e) {
        if ($(this).is(':checked')) {
            $('#startHourUp').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#startMinUp').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#startMeridiemUp').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#endHourUp').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#endMinUp').css('opacity', '0.5').attr('disabled', 'disabled');
            $('#endMeridiemUp').css('opacity', '0.5').attr('disabled', 'disabled');
        }
        else {
            $('#startHourUp').css('opacity', '1').removeAttr('disabled');
            $('#startMinUp').css('opacity', '1').removeAttr('disabled');
            $('#startMeridiemUp').css('opacity', '1').removeAttr('disabled');
            $('#endHourUp').css('opacity', '1').removeAttr('disabled');
            $('#endMinUp').css('opacity', '1').removeAttr('disabled');
            $('#endMeridiemUp').css('opacity', '1').removeAttr('disabled');
        }
    });
    
    $(document).delegate("input.nhacnho", "keydown", function (event) {
        if (!((event.which >= 96 && event.which <= 105) || (event.which == 9) || (event.which >= 48 && event.which <= 57)
                || (event.which === 8) || (event.which == 46) || (event.which >= 35 && event.which <= 40))) {
            event.preventDefault();
        }
    });
    
    // hide #back-top first
    $("#back-top").hide();
    
    // fade in #back-top
    $(function () {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 100) {
                $('#back-top').fadeIn();
            } else {
                $('#back-top').fadeOut();
            }
        });
        // scroll body to 0px on click
        $('#back-top a').click(function () {
            $('body,html').animate({
            scrollTop: 0
            }, 800);
            return false;
            });
            // scroll body to 0px on click
            $('.footer_infor_img').click(function () {
            $('body,html').animate({
            scrollTop: 0
            }, 800);
            return false;
        });
    }); 
});

function Huy() {
    var tag = document.getElementById("Insert");
    tag.style.display = "none";
    $('#selectedItems').text('');
    initCheckTime();
};

function Thoat() {
    var tag = document.getElementById("Update");
    tag.style.display = "none";
    $('#selectedItems').text('');
    initCheckTime();
};

function initCheckTime(){
    $('#checkCaNgayUp').prop('checked', false);
    $('#startHourUp').css('opacity', '1').removeAttr('disabled');
    $('#startMinUp').css('opacity', '1').removeAttr('disabled');
    $('#startMeridiemUp').css('opacity', '1').removeAttr('disabled');
    $('#endHourUp').css('opacity', '1').removeAttr('disabled');
    $('#endMinUp').css('opacity', '1').removeAttr('disabled');
    $('#endMeridiemUp').css('opacity', '1').removeAttr('disabled');
    
    $('#checkCaNgay').prop('checked', false);
    $('#startHour').css('opacity', '1').removeAttr('disabled');
    $('#startMin').css('opacity', '1').removeAttr('disabled');
    $('#startMeridiem').css('opacity', '1').removeAttr('disabled');
    $('#endHour').css('opacity', '1').removeAttr('disabled');
    $('#endMin').css('opacity', '1').removeAttr('disabled');
    $('#endMeridiem').css('opacity', '1').removeAttr('disabled');
}

function initUpdate(msg) {
    initCheckTime();
    $("td.highlight").removeClass("highlight");
    $("input.chk").prop('checked', false);
    
    $('#tieudeUp').val(msg[0].strTieuDe);
    $('#noidungUp').val(msg[0].strNoiDung);
    $('#phonghopUp').val(msg[0].strPhongHop);
    var startday = new Date(msg[0].strNgayBatDau);
    $('#ngaybatdauUp').val(startday.format('dd/mm/yyyy'));
    var starttime = startday.format("shortTime", false).split(' ')[0].split(':');
    $('#startHourUp').val(starttime[0]).attr('selected', true);

    if (starttime[0].toString().length === 1)
        $('#startHourUp').val("0" + starttime[0].toString()).attr('selected', true);
    else
        $('#startHourUp').val(starttime[0]).attr('selected', true);
    
    $('#startMinUp').val(starttime[1]).attr('selected', true);
    $('#startMeridiemUp').val(startday.format("shortTime", false).split(' ')[1]).attr('selected', true);

    var endday = new Date(msg[0].strNgayKetThuc);
    $('#ngayketthucUp').val(endday.format('dd/mm/yyyy'));
    var endtime = endday.format("shortTime", false).split(' ')[0].split(':');

    if (endtime[0].toString().length === 1)
        $('#endHourUp').val("0" + endtime[0].toString()).attr('selected', true);
    else
        $('#endHourUp').val(endtime[0]).attr('selected', true);
        
    $('#endMinUp').val(endtime[1]).attr('selected', true);
    $('#endMeridiemUp').val(endday.format("shortTime", false).split(' ')[1]).attr('selected', true);

    $('#checkCaNgayUp').prop('checked', false);
    if (startday.format("shortTime", false) === "12:00 AM" && endday.format("shortTime", false) === "11:59 PM") {
        $('#checkCaNgayUp').prop('checked', true);
        $('#startHourUp').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#startMinUp').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#startMeridiemUp').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#endHourUp').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#endMinUp').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#endMeridiemUp').css('opacity', '0.5').attr('disabled', 'disabled');
    }
    $('#txtdexuatUp').val(msg[0].strDeXuat);
    $('#nhacnhoUp').val(msg[0].strSoNgayNhacNho);
    if (msg[0].strLoaiNhacNho === "0") {
        $('#checknhacnhoUp').prop('checked', false);
        $('#nhacnhoUp').css('display', 'none');
    }
    else {
        $('#checknhacnhoUp').prop('checked', true);
    }

    $('div#Update fieldset div:first').animate({scrollTop: 0}, 0);
    $('#selectUuTienUp').val(msg[0].strDoUuTien).attr('selected', true);
};

function initInsert(startDay, endDay, allDay)
{
    initCheckTime();
    $("td.highlight").removeClass("highlight");
    $("input.chk").prop('checked', false);
    $('#tieude').val('');
    $('#noidung').val('');
    $('#phonghop').val('');
    $('#txtdexuat').val('');
    $('#nhacnho').val('0');
    $('#checknhacnho').prop('checked', true);
    $('#nhacnho').css('display', 'block');
    var tempStart = new Date(startDay);
    $('#ngaybatdau').val(tempStart.format('dd/mm/yyyy'));
    var hourStart = tempStart.format("shortTime", false).split(' ')[0].split(':');
    if (hourStart[0].toString().length === 1)
        $('#startHour').val("0" + hourStart[0].toString()).attr('selected', true);
    else
        $('#startHour').val(hourStart[0].toString()).attr('selected', true);
    $('#startMin').val(hourStart[1]).attr('selected',true);
    $('#startMeridiem').val(tempStart.format("shortTime", false).split(' ')[1]).attr('selected',true);

    var tempEnd = new Date(endDay);
    $('#ngayketthuc').val(tempEnd.format('dd/mm/yyyy'));
    var hourEnd = tempEnd.format("shortTime", false).split(' ')[0].split(':');
    $('#endHour').val(hourEnd[0]).attr('selected', true);
    if (hourEnd[0].toString().length === 1)
        $('#endHour').val("0" + hourEnd[0].toString()).attr('selected', true);
    else
        $('#endHour').val(hourEnd[0]).attr('selected', true);
    $('#endMin').val(hourEnd[1]).attr('selected',true);
    $('#endMeridiem').val(tempEnd.format("shortTime", false).split(' ')[1]).attr('selected',true);
    if(allDay === true){
        $('#startHour').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#startMin').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#startMeridiem').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#endHour').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#endMin').css('opacity', '0.5').attr('disabled', 'disabled');
        $('#endMeridiem').css('opacity', '0.5').attr('disabled', 'disabled');

        $('#checkCaNgay').prop('checked', true);
    }
    else{
        $('#startHour').css('opacity', '1').removeAttr('disabled');
        $('#startMin').css('opacity', '1').removeAttr('disabled');
        $('#startMeridiem').css('opacity', '1').removeAttr('disabled');
        $('#endHour').css('opacity', '1').removeAttr('disabled');
        $('#endMin').css('opacity', '1').removeAttr('disabled');
        $('#endMeridiem').css('opacity', '1').removeAttr('disabled');
        $("#checkCaNgay").prop('checked', false);
    }
    $('div#Insert fieldset div:first').animate({scrollTop: 0}, 0);
};