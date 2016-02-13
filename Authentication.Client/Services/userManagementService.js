(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("userManagementService",
              ["$resource",
               "appSettings",
                  userManagementService])

  angular
      .module("CommonServices")
      .factory("roleManagementService",
              ["$resource",
               "appSettings",
                  roleManagementService])

  function userManagementService($resource, appSettings) {
    return $resource(appSettings.serverPath + "/api/User/:id", null,
        {
          'get': {
          },

          'save': { method: 'POST' }
        });
  }

  function roleManagementService($resource, appSettings) {
    return $resource(appSettings.serverPath + "/api/Roles/:id", null,
        {
          'get': {
          },

          'save': { method: 'POST' }
        });
  }
}());