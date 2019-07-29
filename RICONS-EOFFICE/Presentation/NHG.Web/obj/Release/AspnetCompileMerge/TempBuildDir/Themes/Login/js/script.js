var CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
var Keys = [
    "98tvajsdhg!8034y0h0~3uhf0cu3H40TUH30H2FLKJ+GEU502*9029+8H20495*",
    "0984tpeo^rhg09WHIN2043HNTX098~H98X07H98M942-9x8h9mg-8g598xm9521",
    "NVLIDSJnv0w45uv045vlskj~vo8uh0837013509*yrehfaiu!oiudfsjp098sdh",
    "*+pninutcpiu##piucpoieupcoun409*(13485c0439856c084n5609834750c!"]

$(document).ready(function (e) {
    $("#login-button").click(function () {
        CheckLogin();
    });
});

function CheckLogin() {
    $.ajax({
        type: 'POST',
        url: 'https://172.16.64.56/services/account/CheckLogin',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        data: '',
        beforeSend: function () {
            AddLoader();
        },
        complete: function () {
            RemoveLoader()
        },
        success: function (data) {
            
        },
        error: function (e) {
            RemoveLoader();
        }
    });
}

function EncodeCrypto(str) {
    var EncodedStr = "";
    var CharPos = 0;
    for (var i = 0; i < str.length; i++) {
        CharPos = CharSet.indexOf(str.substr(i, 1));
        for (var j = 0; j < Keys.length; j++) {
            EncodedStr += Keys[j].substr(CharPos, 1);
        }
    }
    return EncodedStr;
}