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
    return $resource(appSettings.serverPath + "/api/account/UserAccountSummary", null,
        {
          query : {
            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
          }
        });
  }
}());