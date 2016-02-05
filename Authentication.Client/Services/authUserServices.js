(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("authUserServices",
              ["$resource",
               "appSettings",
                 authUserServices])


  function authUserServices($resource, appSettings) {
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

      logout: $resource(appSettings.serverPath + "/api/Account/Logout", null,
                    {
                      'logoutUser': {
                        method: 'POST'
                      }
                    }
      )
    }
  }
})();