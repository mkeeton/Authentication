(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("userAccountServices",
              ["$resource",
               "appSettings",
               "currentUser",
                 userAccountServices])

  function userAccountServices($resource, appSettings, currentUser) {
    return {

      changePassword: $resource(appSettings.serverPath + "/api/Account/ChangePassword", null,
              {
                'changePassword': { method: 'POST' }
              }
            ),

      loadAccount: $resource(appSettings.serverPath + "/api/account/UserAccountSummary", null,
        {
          query: { method: 'GET' }
        }),

      updateAccount: $resource(appSettings.serverPath + "/api/Account/UpdateAccount", null,
              {
                'updateAccount': { method: 'POST' }
              }
            )
    }
  }
})();