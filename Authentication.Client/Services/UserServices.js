var authUserServices = angular.module('authUserServices', ['ngResource']);

authUserServices.factory('User', ['$resource',
  function ($resource) {
    return $resource('phones/:phoneId.json', {}, {
      query: { method: 'GET', params: { emailAddress: 'users' }, isArray: true }
    });
  }]);

authUserServices.service('userService', ['$http', function ($http) {
  this.getUsers = function ($scope) {
    return $http({
      method: "GET",
      url: "http://localhost:50378/api/values",
      headers: { 'Content-Type': 'application/json' }
    });
  };
}]);