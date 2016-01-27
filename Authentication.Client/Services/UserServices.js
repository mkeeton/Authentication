(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("authUserServices",
              ["$resource",
               "appSettings",
                 authUserServices])

  function authUserServices($resource, appSettings) {
    return $resource(appSettings.serverPath + "/api/values");
  }
}());