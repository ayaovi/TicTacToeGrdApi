(function () {
  var app = angular.module("myApp", []);
  var agniKaiUri = "agnikai";
  var gameUri = "game";
  var errorMessage = function (data, status) {
    return "Error: " + status + (data.Message !== undefined ? (" " + data.Message) : "");
  };
  var hub = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.getAgniKaiTicket = function () {
      $http.get(agniKaiUri + "/initiate")
        .success(function (data, status) {
          $scope.agnikaiTicket = data; // retrieve agnikai ticket.
          var encodedName = $("<div />").text($("#gamerName").val()).html();
          var encodedTicket = $("<div />").text(data).html();
          $("#playerOnline").append("<li><strong>" + encodedName + "</strong>:&nbsp;&nbsp;" + encodedTicket + "</li>");
        })
        .error(function (data, status) {
          $scope.agnikaiTicket = "";
          $scope.errorToSearch = errorMessage(data, status);
        });
    };
    $("#gamerName").val(prompt("Enter your name:", ""));
    $.connection.hub.start(); // connect to signalr hub
  }]);
})();