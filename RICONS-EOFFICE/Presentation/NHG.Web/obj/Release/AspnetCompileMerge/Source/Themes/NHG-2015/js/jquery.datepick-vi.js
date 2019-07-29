(function($) {
	$.datepicker.regional['vi'] = {
		monthNames: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu',
		'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
		monthNamesShort: ['Thg 1', 'Thg 2', 'Thg 3', 'Thg 4', 'Thg 5', 'Thg 6',
		'Thg 7', 'Thg 8', 'Thg 9', 'Thg 10', 'Thg 11', 'Thg 12'],
		dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
		dayNamesShort: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
		dayNamesMin: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
	    weekHeader: 'Tuần',
		dateFormat: 'dd/mm/yyyy', firstDay: 0,
		renderer: $.datepicker.defaultRenderer,
		prevText: '&#x3c;Trước', prevStatus: 'Tháng trước',
		prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: 'Năm trước',
		nextText: 'Tiếp&#x3e;', nextStatus: 'Tháng sau',
		nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: 'Năm sau',
		currentText: 'Hôm nay', currentStatus: 'Tháng hiện tại',
		todayText: 'Hôm nay', todayStatus: 'Tháng hiện tại',
		clearText: 'Xóa', clearStatus: 'Xóa ngày hiện tại',
		closeText: 'Đóng', closeStatus: 'Đóng và không lưu lại thay đổi',
		yearStatus: 'Năm khác', monthStatus: 'Tháng khác',
		weekText: 'Tu', weekStatus: 'Tuần trong năm',
		dayStatus: 'Đang chọn DD, \'ngày\' d M', defaultStatus: 'Chọn ngày',
		isRTL: false
	};
	$.datepicker.setDefaults($.datepicker.regional['vi']);
})(jQuery);