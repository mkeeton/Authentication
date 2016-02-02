var authenticationApp = angular.module('authenticationApp', [
  'ngRoute',
  'ngCookies',
  'mainController',
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
        controller: 'AuthenticationController'
      }).
      when('/ForgottenPassword', {
        templateUrl: 'Views/Partials/forgottenPassword.html',
        controller: 'AuthenticationController'
      }).
      when('/Users', {
        templateUrl: 'Views/Partials/users.html',
        controller: 'UserListController'
      }).
      when('/AccountSummary', {
        templateUrl: 'Views/Partials/accountSummary.html',
        controller: 'AccountController'
      }).
      otherwise({
        templateUrl: 'Views/Partials/login.html',
        controller: 'AuthenticationController'
      });

  }]);