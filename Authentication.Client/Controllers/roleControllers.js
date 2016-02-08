var roleControllers = angular.module('roleControllers', []);

roleControllers.controller('roleListController', ['$scope', '$uibModal', 'roleManagementService', function ($scope, $uibModal, roleManagementService) {
  var vm = this;

  vm.LoadRoleList = function () {
    roleManagementService.query(function (data) {
      vm.roles = data;
    });
  }

  vm.open = function (roleId, size) {
    var modalInstance = $uibModal.open({
      animation: 'true',
      templateUrl: 'Views/Partials/UserManagement/roleDetails.html',
      controller: 'roleDetailsController',
      controllerAs: 'vm',
      size: size,
      resolve: {
        roleId: function () {
          return roleId;
        }
      }
    });

    modalInstance.result.then(function () {
      vm.LoadRoleList();
    }, function () {
      vm.LoadRoleList();
    });
  }
}]);

roleControllers.controller('roleDetailsController', ['$scope', '$uibModalInstance','roleManagementService', 'roleId', function ($scope, $uibModalInstance, roleManagementService, roleId) {
  var vm = this;

  vm.loadRole = function () {
    roleManagementService.get({ id: roleId },
        function (data) {
          vm.role = data;
          console.log(vm.role.AvailableApis);
        },
        function (response) {
          vm.message = response.statusText + "\r\n";
          if (response.data.exceptionMessage)
            vm.message += response.data.exceptionMessage;
        });
  }

  vm.updateRole = function () {
    roleManagementService.save(vm.role,
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