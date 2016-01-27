var authenticationApp = angular.module('authenticationApp', [
  'ngRoute',
  'authUserControllers',
  'CommonServices'
]);

authenticationApp.config(['$routeProvider',
  function ($routeProvider, $locationProvider) {
    // use the HTML5 History API
    //$locationProvider.html5Mode(true);

    $routeProvider.
      when('/Register', {
        templateUrl: 'Views/Partials/register.html',
        controller: 'RegisterController'
      }).
      when('/ForgottenPassword', {
        templateUrl: 'Views/Partials/forgottenPassword.html',
        controller: 'ForgottePasswordController'
      }).
      when('/Users', {
        templateUrl: 'Views/Partials/users.html',
        controller: 'UserListController'
      }).
      otherwise({
        templateUrl: 'Views/Partials/login.html',
        controller: 'LoginController'
      });

  }]);