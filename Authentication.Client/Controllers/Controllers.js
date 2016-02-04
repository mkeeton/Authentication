var authUserControllers = angular.module('authUserControllers', []);

authUserControllers.controller('AuthenticationController', ["$location","authUserServices","currentUser",function ($location,authUserServices,currentUser) {
  var vm = this;
  vm.user = {};

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
    vm.user.grant_type = "password";
    vm.user.UserName = vm.user.Email;

    authUserServices.login.login(vm.user,
        function (data) {
          vm.message = "";
          vm.password = "";
          currentUser.setProfile(vm.user.UserName, data.access_token);
          $location.url("/AccountSummary");
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

  vm.changePassword = function () {
    vm.message = '';
    authUserServices.changePassword.changePassword(vm.user,
        function (data) {
          vm.originalUser = angular.copy(data);
          vm.user.OldPassword = "";
          vm.user.NewPassword = "";
          vm.user.ConfirmPassword = "";
          vm.message = "Password Changed";
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

  vm.loadAccount = function () {
    authUserServices.loadAccount.query(function (data) {
      vm.user = data;
      vm.user.originalEmail = vm.user.Email;
    });
  };

  vm.updateAccount = function () {
    vm.message = '';
    authUserServices.updateAccount.updateAccount(vm.user,
        function (data) {
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