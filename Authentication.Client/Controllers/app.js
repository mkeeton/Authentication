var authenticationApp = angular.module('authenticationApp', [
  'ngRoute',
  'ngCookies',
  'mainController',
  'authUserControllers',
  'CommonServices'
]);

authenticationApp.config(['$routeProvider', '$locationProvider', '$httpProvider',
  function ($routeProvider, $locationProvider,$httpProvider) {
    // use the HTML5 History API
    //$locationProvider.html5Mode(true);

    $routeProvider.
      when('/Register', {
        templateUrl: 'Views/Partials/register.html',
        //controller: 'AuthenticationController'
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
        templateUrl: 'Views/Partials/accountSummary.html'
      }).
      when('/ChangePassword', {
        templateUrl: 'Views/Partials/changePassword.html',
        //controller: 'PasswordController'
      }).
      otherwise({
        templateUrl: 'Views/Partials/login.html',
        //controller: 'AuthenticationController'
      });

    $httpProvider.interceptors.push('APIInterceptor');
  }])

  .service('APIInterceptor', ['$location', 'currentUser', function ($location, currentUser) {
    var service = this;

    service.request = function (config) {
      var user = currentUser.getProfile(),
          access_token = user.username!="" ? user.token : null;

      if (access_token) {
        config.headers.authorization = 'Bearer ' + access_token;
      }
      return config;
    };

    //service.responseError = function (response) {
    //  if (response.status === 401) {
    //    $location.url("/");
    //  }
    //  //return response;
    //};
  }])
;