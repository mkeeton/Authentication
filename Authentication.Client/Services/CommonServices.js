(function () {
  "use strict";

  angular
      .module("CommonServices",
                  ["ngResource"])
    .constant("appSettings",
      {
        serverPath: "http://localhost:50378"
      });
}());