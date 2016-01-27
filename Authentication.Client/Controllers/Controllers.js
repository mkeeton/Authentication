var authUserControllers = angular.module('authUserControllers', []);

authUserControllers.controller('LoginController', function ($scope, $http) {

});

authUserControllers.controller('RegisterController', function ($scope, $http) {

});

authUserControllers.controller('ForgottenPasswordController', function ($scope, $http) {

});

authUserControllers.controller('UserListController', ['$scope', 'authUserServices', function ($scope, authUserServices) {
  authUserServices.query(function (data) {
    $scope.users = data;
  });
}]);