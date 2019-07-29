define(['app'], function (app) {
    app.filter("reverse", function () {
        return function (input) {
            var str = "";
            for (var i = 0; i < input.length; i++) {
                str = input.charAt(i) + str;
            }
            return str;
        }
    });
});