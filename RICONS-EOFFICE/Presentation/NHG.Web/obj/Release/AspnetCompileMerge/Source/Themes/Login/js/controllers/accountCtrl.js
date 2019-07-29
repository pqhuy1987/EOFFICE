define(['app'], function (app) {
    app.controller("aboutCtrl", function ($scope) {
        $scope.Message = "About Us";

        //get login
        $scope.getLogin = function () {
            $scope.user = this.user;
            $("#viewModal").modal('show');
        };
    });
});