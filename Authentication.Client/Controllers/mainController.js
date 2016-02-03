var mainController = angular.module('mainController', []);

mainController.controller('mainNavController', ['$scope','$location','authUserServices', 'currentUser', function ($scope,$location,authUserServices,currentUser) {
  var vmNav = this;
  vmNav.user = currentUser.getProfile();

  $scope.$watch(currentUser.userName, function (userName) {
    vmNav.user = currentUser.getProfile();
    //console.log("Username changed to " + vmNav.user.userName);
  });

  vmNav.logout = function () {
    authUserServices.logout.logout(null,
        function (data) {
          currentUser.setProfile("", "");
          $location.url("/");
        },
        function (response) {
          vmNav.message = response.statusText + "\r\n";
          if (response.data.exceptionMessage)
            vmNav.message += response.data.exceptionMessage;

          if (response.data.error) {
            vmNav.message += response.data.error;
          }
        });
  }
}]);