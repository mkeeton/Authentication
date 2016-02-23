var authUserControllers = angular.module('authUserControllers', []);

authUserControllers.controller('AuthenticationController', ["$location","authUserServices","userAccountServices","currentUser",function ($location,authUserServices,userAccountServices,currentUser) {
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
  }

  vm.login = function () {
    vm.user.grant_type = "password";
    vm.user.UserName = vm.user.Email;
    authUserServices.login.loginUser(vm.user,
        function (data) {
          vm.message = "";
          vm.user.Password = "";
          currentUser.setProfile(vm.user.UserName, data.access_token,data.refresh_token);
          $location.url("/AccountSummary");
        },
        function (response) {
          vm.user.Password = "";
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
    userAccountServices.changePassword.changePassword(vm.user,
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
  }

  vm.loadAccount = function () {
    userAccountServices.loadAccount.query(function (data) {
      vm.user.Email = data.Email;
      vm.user.FirstName = data.FirstName;
      vm.user.LastName = data.LastName;
      vm.user.originalEmail = vm.user.Email;
    });
  }

  vm.updateAccount = function () {
    vm.message = '';
    vm.user.UserName = vm.user.Email;
    userAccountServices.updateAccount.updateAccount(vm.user,
        function (data) {
          if (vm.user.Email != vm.user.originalEmail) {

            authUserServices.logout.logoutUser(null,
                function (data) {
                  currentUser.setProfile("", "");
                  vm.user.originalEmail = vm.user.Email;
                  vm.login();
                },
                function (response) {
                  vm.message = response.statusText + "\r\n";
                  if (response.data.exceptionMessage)
                    vm.message += response.data.exceptionMessage;

                  if (response.data.error) {
                    vm.message += response.data.error;
                  }
                });
            
          }
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
  }
}]);

authUserControllers.controller('UserListController', ['$scope','$uibModal', 'userManagementService', function ($scope, $uibModal, userManagementService) {
  var vm = this;

  vm.LoadUserList = function () {
    userManagementService.query(function (data) {
      vm.users = data;
    });
  }

  vm.open = function (userId,size) {
    var modalInstance = $uibModal.open({
      animation: 'true',
      templateUrl: 'Views/Partials/UserManagement/userDetails.html',
      controller: 'UserDetailsController',
      controllerAs: 'vm',
      size: size,
      resolve: {
        userId: function () {
          return userId;
        }
      }
    });

    modalInstance.result.then(function () {
      vm.LoadUserList();
    }, function () {
      vm.LoadUserList();
    });
  }
}]);

authUserControllers.controller('UserDetailsController', ['$scope', '$uibModalInstance', 'userManagementService', 'roleManagementService', 'userId', function ($scope, $uibModalInstance, userManagementService, roleManagementService, userId) {
  var vm = this;

  vm.loadUser = function () {
    userManagementService.get({ id: userId },
        function (data) {
          vm.user = data;
        },
        function (response) {
          vm.message = response.statusText + "\r\n";
          if (response.data.exceptionMessage)
            vm.message += response.data.exceptionMessage;
        });
  }

  vm.updateAccount = function () {
    userManagementService.save(vm.user,
        function (data) {
          $uibModalInstance.close();
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
  }

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