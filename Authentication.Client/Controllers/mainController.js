var mainController = angular.module('mainController', []);

mainController.controller('mainController', ['authUserServices', 'currentUser', function (authUserServices,currentUser) {
  var vm = this;
  vm.user = currentUser.getProfile();

  vm.logout = function () {
    authUserServices.logout.logout(null,
        function (data) {
          currentUser.setProfile("", "");
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
}]);