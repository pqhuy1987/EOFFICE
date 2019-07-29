function getHM(date) {
	var hour = date.getHours();
	var minute = date.getMinutes();
	var ret = (hour > 9 ? hour : "0" + hour) + ":" + (minute > 9 ? minute : "0" + minute);
	return ret;
}
$(document).ready(function () {
	//debugger;
	var DATA_FEED_URL = "";
	var arrT = [];
	var tt = "{0}:{1}";
	for (var i = 0; i < 24; i++) {
		arrT.push({ text: StrFormat(tt, [i >= 10 ? i : "0" + i, "00"]) }, { text: StrFormat(tt, [i >= 10 ? i : "0" + i, "30"]) });
	}
	$("#timezone").val(new Date().getTimezoneOffset() / 60 * -1);
	$("#stparttime").dropdown({
		dropheight: 200,
		dropwidth: 60,
		selectedchange: function () { },
		items: arrT
	});
	$("#etparttime").dropdown({
		dropheight: 200,
		dropwidth: 60,
		selectedchange: function () { },
		items: arrT
	});
	var check = $("#IsAllDayEvent").click(function (e) {
		if (this.checked) {
			$("#stparttime").val("00:00").hide();
			$("#etparttime").val("00:00").hide();
		}
		else {
			var d = new Date();
			var p = 60 - d.getMinutes();
			if (p > 30) p = p - 30;
			d = DateAdd("n", p, d);
			$("#stparttime").val(getHM(d)).show();
			$("#etparttime").val(getHM(DateAdd("h", 1, d))).show();
		}
	});
	if (check[0].checked) {
		$("#stparttime").val("00:00").hide();
		$("#etparttime").val("00:00").hide();
	}
	$("#Savebtn").click(function () {
	    var param = [
            { "name": "Subject", value: Encoder.htmlEncode($('#Subject').val()) },
            { "name": "Location", value: Encoder.htmlEncode($('#Location').val()) },
            { "name": "Description", value: Encoder.htmlEncode($('#Description').val()) },
            { "name": "colorvalue", value: $('#colorvalue').val() },
            { "name": "stpartdate", value: $('#stpartdate').val() },
            { "name": "stparttime", value: $('#stparttime').val() },
            { "name": "etpartdate", value: $('#etpartdate').val() },
            { "name": "etparttime", value: $('#etparttime').val() },
            { "name": "IsAllDayEvent", value: $('#IsAllDayEvent').is(":checked") == true ? "1" : "0" }
	    ];
	    $.post(DATA_FEED_URL + "?method=adddetails",
            param,
            function (data) {
                if (data.IsSuccess) {
                    CloseModelWindow(null, true);
                    $("#gridcontainer").reload();                    
                }
                else {
                    alert("Error occurs.\r\n" + data.Msg);
                }
            }
        , "json");
	    //$("#fmEdit").submit(); CloseModelWindow();
	});
	$("#Closebtn").click(function () { CloseModelWindow(); });
	$("#Deletebtn").click(function () {
	    if (confirm("Bạn có muốn xóa sự kiện này")) {	        
	        var param = [{ "name": "calendarId", value: $(this).attr("calendarId") }];
			$.post(DATA_FEED_URL + "?method=remove",
				param,
				function (data) {
					if (data.IsSuccess) {
					    //ShortAlertWithAutoHide('Đã xóa sự kiện.', 2100);
						CloseModelWindow(null, true);
					}
					else {
						alert("Error occurs.\r\n" + data.Msg);
					}
				}
			, "json");
		}
	});

	$("#stpartdate,#etpartdate").datepicker({
	    dateFormat: "dd/mm/yy",
	    defaultDate: new Date(),
	    dayNamesShort: $.datepicker.regional.vi.dayNamesShort,
	    dayNames: $.datepicker.regional.vi.dayNames,
	    monthNamesShort: $.datepicker.regional.vi.monthNamesShort,
	    monthNames: $.datepicker.regional.vi.monthNames,
	    showAnim: "slideDown",
	    firstDay: 1 // Start with Monday
	});
	var cv = $("#colorvalue").val();
	if (cv == "") {
		cv = "-1";
	}
	$("#calendarcolor").colorselect({ title: "Color", index: cv, hiddenid: "colorvalue" });
	//to define parameters of ajaxform
	var options = {
		beforeSubmit: function () {
			return true;
		},
		dataType: "json",
		success: function (data) {
			alert(data.Msg);
			if (data.IsSuccess) {
				CloseModelWindow(null, true);
			}
		}
	};
	$.validator.addMethod("date", function (value, element) {
	    return check_date(element);
	}, "Ngày không đúng định dạng dd/MM/yyyy");
	$.validator.addMethod("time", function (value, element) {
		return this.optional(element) || /^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$/.test(value);
	}, "Thời gian không đúng định dạng");
	$.validator.addMethod("safe", function (value, element) {
		return this.optional(element) || /^[^$\<\>]+$/.test(value);
	}, "không có ký tự đặc biệt");
	$("#fmEdit").validate({
		submitHandler: function (form) { $("#fmEdit").ajaxSubmit(options); },
		errorElement: "div",
		errorClass: "cusErrorPanel",
		errorPlacement: function (error, element) {
			showerror(error, element);
		}
	});
	function showerror(error, target) {
		var pos = target.position();
		var height = target.height();
		var newpos = { left: pos.left, top: pos.top + height + 2 }
		var form = $("#fmEdit");
		error.appendTo(form).css(newpos);
	}
});