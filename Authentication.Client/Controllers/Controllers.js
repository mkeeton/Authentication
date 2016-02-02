var authUserControllers = angular.module('authUserControllers', []);

authUserControllers.controller('navigationController', ['currentUser', function (currentUser) {
  var vm = this;
  vm.user = currentUser.getProfile();
}]);

authUserControllers.controller('AuthenticationController', ["authUserServices","currentUser",function (authUserServices,currentUser) {
  var vm = this;
  vm.userData = {};

  vm.register = function () {
    vm.message = '';
    vm.user.ConfirmPassword = vm.user.Password;
    authUserServices.registration.register(vm.user,
        function (data) {
          vm.originalUser = angular.copy(data);

          vm.message = "Registration Complete";
          vm.login();
        },
        function (response) {
          vm.message = response.statusText + "\r\n";
          if (response.data.modelState) {
            for (var key in response.data.modelState) {
              vm.message += response.data.modelState[key] + "\r\n";
            }
          }
          if (response.data.exceptionMessage)
            vm.message += response.data.exceptionMessage;
        });
  };

  vm.login = function () {
    vm.userData.grant_type = "password";
    vm.userData.userName = vm.userData.email;

    authUserServices.login.login(vm.userData,
        function (data) {
          vm.message = "";
          vm.password = "";
          currentUser.setProfile(vm.userData.userName, data.access_token);
        },
        function (response) {
          vm.password = "";
          vm.message = response.statusText + "\r\n";
          if (response.data.exceptionMessage)
            vm.message += response.data.exceptionMessage;

          if (response.data.error) {
            vm.message += response.data.error;
          }
        });
  }

  vm.forgottenPassword = function () {

  }
}]);

authUserControllers.controller('AccountController', ['$scope', 'userAccountServices','currentUser', function ($scope, userAccountServices,currentUser) {
  var vm = this;
  vm.user = currentUser.getProfile();
  vm.userData = {};
  userAccountServices.query(function (data) {
    vm.userData = data;
  });
}]);

authUserControllers.controller('UserListController', ['$scope', 'userAccountServices', function ($scope, userAccountServices) {
  userAccountServices.query(function (data) {
    $scope.users = data;
  });
}]);

var compareTo = function () {
  return {
    require: "ngModel",
    scope: {
      otherModelValue: "=compareTo"
    },
    link: function (scope, element, attributes, ngModel) {

      ngModel.$validators.compareTo = function (modelValue) {
        return modelValue == scope.otherModelValue;
      };

      scope.$watch("otherModelValue", function () {
        ngModel.$validate();
      });
    }
  };
};

authUserControllers.directive("compareTo", compareTo);