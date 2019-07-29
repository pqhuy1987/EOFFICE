define(['app'], function (app) {
    //defining service using factory method
    app.factory('userService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "UserService/";
        return {
            getUsersList: function () {
                var url = serviceurl + "GetUsersList";
                return $http.get(url);
            },
            getUser: function (user) {
                var url = serviceurl + "GetUser/" + user.UserId;
                return $http.get(url);
            },
            addUser: function (user) {
                var url = serviceurl + "AddUser";
                return $http.post(url, user);
            },
            deleteUser: function (user) {
                var url = serviceurl + "DeleteUser/" + user.UserId;
                return $http.delete(url);
            },
            updateUser: function (user) {
                var url = serviceurl + "ModifyUser/" + user.UserId;
                return $http.put(url, user);
            }
        };
    });
});