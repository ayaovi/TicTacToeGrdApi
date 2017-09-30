(function () {
  var app = angular.module("myApp", []);
  var agniKaiUri = "agnikai";
  var gameUri = "game";
  var errorMessage = function (data, status) {
    return "Error: " + status + (data.Message !== undefined ? (" " + data.Message) : "");
  };
  var hub = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.cellIds = [];
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

    $scope.recordMove = function (cellId) {
      var move = $scope.extractMove(cellId);
      var encodeMove = $("<div />").text(move).html();
      $("#playerOnline").append("<li>" + encodeMove + "</li>");
    };

    $scope.extractMove = function (cellId) {
      var n = Math.floor(cellId / 27);
      var r = cellId % 27;
      var x = r % 3;
      var grid = (n * 3) + 3 - x % 3;
      var cell = r % 3;
      return  cellId + ": " + "(" + grid + ", " + cell + ")";
    };

    $("#gamerName").val(prompt("Enter your name:", ""));
    for (var i = 0; i < 81; i++) {
      $scope.cellIds.push(i);
    }

    $.connection.hub.start(); // connect to signalr hub
  }]);
})();