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
                      'login': {
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
                    }),

      logout: $resource(appSettings.serverPath + "/api/Account/Logout", null,
                    {
                      'logout': {
                        method: 'POST'
                      }
                    }),

      changePassword: $resource(appSettings.serverPath + "/api/Account/ChangePassword", null,
              {
                'changePassword': { method: 'POST' }
              }
            ),

      loadAccount: $resource(appSettings.serverPath + "/api/account/UserAccountSummary", null,
        {
          query : { method: 'GET' }
        }),

      updateAccount: $resource(appSettings.serverPath + "/api/Account/UpdateAccount", null,
              {
                'updateAccount': { method: 'POST' }
              }
            )
    }
  }
}());