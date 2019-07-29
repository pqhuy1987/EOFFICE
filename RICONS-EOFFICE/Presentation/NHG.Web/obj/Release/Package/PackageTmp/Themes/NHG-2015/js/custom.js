/**
 *
 * @param typeAlert is success or fail
 * @param contain
 */
function alertForm(typeAlert, contain, timeDisplay, callbackFunction) {
    if(typeAlert == "success"){
        var dialogSuccess = bootbox.dialog({
            message: "<i class='fa fa-check text-success'></i><span class='text-success'>" + contain + "</span>",
            closeButton: false,
            onEscape: function () {
            }
        });
        setTimeout(function () {
            dialogSuccess.modal('hide');
            callbackFunction;
        }, timeDisplay);
    } else if (typeAlert == "fail"){
        var dialogSuccess = bootbox.dialog({
            message: "<i class='fa fa-check text-danger'></i><span class='text-danger'>" + contain + "</span>",
            closeButton: false
        });
        setTimeout(function () {
            dialogSuccess.modal('hide');
        }, timeDisplay);
    }
}