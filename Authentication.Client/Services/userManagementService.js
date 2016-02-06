(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("userManagementService",
              ["$resource",
               "appSettings",
                  userManagementService])

  function userManagementService($resource, appSettings) {
    return $resource(appSettings.serverPath + "/api/User/:id", null,
        {
          'get': {
          },

          'save': {
          },

          'update': {
            method: 'PUT',
          }
        });
  }
}());