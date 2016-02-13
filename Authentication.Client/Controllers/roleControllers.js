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

  vm.addRole = function (size) {
    var modalInstance = $uibModal.open({
      animation: 'true',
      templateUrl: 'Views/Partials/UserManagement/roleDetails.html',
      controller: 'roleDetailsController',
      controllerAs: 'vm',
      size: size,
      resolve: {
        roleId: function () {
          return '0';
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

roleControllers.controller('roleDetailsController', ['$scope', '$route', '$filter', '$uibModalInstance', 'roleManagementService', 'roleId', function ($scope, $route, $filter, $uibModalInstance, roleManagementService, roleId) {
  var vm = this;
  vm.role = "";

  vm.orderList = function (list) {
    var orderBy = $filter('orderBy');
    list = orderBy(list, 'Path');
    return list;
  }

  vm.loadRole = function () {
    roleManagementService.get({ id: roleId },
        function (data) {
          vm.role = data;
          var _routes = $route.routes;
          angular.forEach(_routes, function (value) {
            if ((value.redirectTo == null) && (!((value.universalLink == 'true') || (value.externalLink == 'true') || (value.generalInternalLink == 'true')))) {
              var _newRoute = new function () {
                this.Client = "";
                this.Path = value.originalPath;
              }
              vm.role.AvailableClientPaths.push(_newRoute);
            }
          });
          vm.role.AvailableClientPaths = vm.orderList(vm.role.AvailableClientPaths);
          vm.role.AssignedClientPaths = vm.orderList(vm.role.AssignedClientPaths);
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