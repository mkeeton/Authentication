﻿(function () {
  "use strict";

  angular
      .module("CommonServices")
      .factory("currentUser",
                ['$cookies',currentUser])

  function currentUser($cookies) {

    var profile = {
      isLoggedIn: false,
      userName: "",
      token: "",
      refreshToken: ""
    };

    var setProfile = function (username, token, refreshToken) {
      profile.userName = username;
      profile.token = token;
      profile.refreshToken = refreshToken;
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
      getProfile: getProfile,
      isLoggedIn: function () { return profile.isLoggedIn; },
      userName: function () { return profile.userName;}
    }
  }
})();