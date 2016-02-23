var authenticationApp = angular.module('authenticationApp', [
  'ngRoute',
  'ngCookies',
  'ui.bootstrap',
  'dndLists',
  'mainController',
  'authUserControllers',
  'roleControllers',
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
        templateUrl: 'Views/Partials/UserManagement/users.html',
        controller: 'UserListController'
      }).
      when('/Roles', {
        templateUrl: 'Views/Partials/UserManagement/roles.html',
        controller: 'roleListController',
        controllerAs: 'vm'
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

  .factory('APIInterceptor', ['$q', '$location', 'currentUser', '$injector', 'appSettings', function ($q, $location, currentUser, $injector, appSettings) {
    return {
      responseError: responseError,
      request: request,
    };

    function request(config) {
      var d = $q.defer();
      if (config.url.toLowerCase().indexOf(appSettings.serverPath.toLowerCase()) == 0)
      {     
        var user = currentUser.getProfile(),

        refresh_token = user.refreshToken != "" ? user.refreshToken : null;
        current_UserName = user.userName != "" ? user.userName : null;
        var authUserServices = $injector.get('authUserServices');
        //access_token = user.token != "" ? user.token : null;
        if (refresh_token) {
          var refresh = {};
          refresh.grant_type = "refresh_token";
          refresh.refresh_token = refresh_token;
          currentUser.setProfile(current_UserName, "", "");
          authUserServices.login.loginUser(refresh,
              function (data) {
                access_token = data.access_token;
                refresh_token = data.refresh_token;
                currentUser.setProfile(current_UserName, access_token, refresh_token);
                if (access_token) {
                  config.headers.authorization = 'Bearer ' + access_token;
                }
                d.resolve(config);
              },
              function (response) {
                d.resolve(config);
              }
          );
        }
        else
        {
          d.resolve(config);
        }
      }
      else
      {
        d.resolve(config);
      }
      return d.promise;
    };

    function responseError(response) {
      if (response.status === 401) {
        currentUser.setProfile("", "", "");
        $location.url("/");
      }
      return $q.reject(response);
    };
  }])

  //.service('APIInterceptor', ['$q', '$location', 'currentUser', function ($q, $location, currentUser) {
  //  var service = this;

  //  service.request = function (config) {
  //    var user = currentUser.getProfile(),

  //    refresh_token = user.refreshToken != "" ? user.refreshToken : null;
  //    access_token = user.token != "" ? user.token : null;
  //    if (refresh_token)
  //    {
  //      console.log(refresh_token);
  //    }
  //    if (access_token) {
  //      config.headers.authorization = 'Bearer ' + access_token;
  //    }
  //    return config;
  //  };

  //  service.responseError = function (response) {
  //    if (response.status === 401) {
  //      currentUser.setProfile("", "", "");
  //      $location.url("/");
  //    }
  //    return $q.reject(response);
  //  };
  //}])
;