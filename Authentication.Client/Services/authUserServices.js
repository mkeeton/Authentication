(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("authUserServices",
              ["$resource",
               "appSettings",
               "currentUser",
                 authUserServices])


  function authUserServices($resource, appSettings, currentUser) {
    return {
      registration : $resource(appSettings.serverPath + "/api/Account/Register",null,
        {
          'register': { method: 'POST' }
        }
      ),

      login: $resource(appSettings.serverPath + "/Token", null,
                    {
                      'loginUser': {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        transformRequest: function (data, headersGetter) {
                          var str = [];
                          for (var d in data)
                            str.push(encodeURIComponent(d) + "=" +
                                                encodeURIComponent(data[d]));
                          return str.join("&");
                        }

                      }
                    }
      ),

      refreshAuth: $resource(appSettings.serverPath + "/Token", null,
                    {
                      'refreshAuth': {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        transformRequest: function (data, headersGetter) {
                          var str = [];
                          for (var d in data)
                            str.push(encodeURIComponent(d) + "=" +
                                                encodeURIComponent(data[d]));
                          return str.join("&");
                        }

                      }
                    }
      ),

      //refreshLogin: function refeshUserServices() {
      //  var deferred = $q.defer();

      //  var authData = currentUser.getProfile();

      //  if (authData) {

      //    var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken;

      //    $http.post(appSettings.serverPath + "/Token", data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

      //      currentUser.setProfile(response.userName, response.access_token, response.refresh_token);
      //      deferred.resolve(response);

      //    }).error(function (err, status) {
      //      currentUser.setProfile("", "", "");
      //      deferred.reject(err);
      //    });
      //  }
      //},

      logout: $resource(appSettings.serverPath + "/api/Account/Logout", null,
                    {
                      'logoutUser': {
                        method: 'POST'
                      }
                    }
      )
    }
  }

  //function refeshUserServices($resource, appSettings, currentUser) {
  //  var deferred = $q.defer();

  //  var authData = currentUser.getProfile();

  //  if (authData) {

  //      var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken;

  //    $http.post(appSettings.serverPath + "/Token", data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

  //        currentUser.setProfile(response.userName, response.access_token, response.refresh_token);
  //        deferred.resolve(response);

  //      }).error(function (err, status) {
  //        currentUser.setProfile("", "", "");
  //        deferred.reject(err);
  //      });
  //  }
  //}
})();