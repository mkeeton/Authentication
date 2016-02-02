(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("currentUser",
                ['$cookies',currentUser])

  function currentUser($cookies) {

    var profile = {
      isLoggedIn: false,
      username: "",
      token: ""
    };

    var setProfile = function (username, token) {
      profile.username = username;
      profile.token = token;
      if (username == "") {
        profile.isLoggedIn = false;
      }
      else {
        profile.isLoggedIn = true;
      }
      $cookies.putObject('userProfile', profile);
    };

    var getProfile = function () {
      var storedProfile = $cookies.getObject('userProfile');
      if (storedProfile != undefined)
      {
        profile = storedProfile;
      }
      return profile;
    }

    return {
      setProfile: setProfile,
      getProfile: getProfile
    }
  }
})();