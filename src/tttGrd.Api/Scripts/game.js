(function () {
  var app = angular.module("myApp", []);
  var agniKaiUri = "agnikai";
  var gameUri = "game";
  var usersUri = "users";
  var errorMessage = function (data, status) {
    return "Error: " + status + (data.Message !== undefined ? (" " + data.Message) : "");
  };
  var gameHubProxy = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.cellIds = [];

    $scope.getAgniKaiTicket = function () {
      $http.get(agniKaiUri + "/initiate")
        .success(function (data, _) {
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

    $scope.getActivePlayers = function () {
      gameHubProxy.server.announce($("#gamerName").val());
      $http.get(usersUri + "/all")
        .success(function (data, _) {
          $scope.playersOnline = data;
          if (data.length > 0) {
            data.forEach(user => {
              var encodedUser = $("<div />").text(user.Username).html();
              $("#activeplayers").append("<li><strong>" + encodedUser + "</strong></li>");
            });
          }
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
      document.getElementById(cellId).disabled = true;
      document.getElementById(cellId).style.background = "#778899";
    };

    $scope.extractMove = function (cellId) {
      var classify = function (index) {
        if (index < 3) return 1;
        if (index < 6) return 2;
        if (index < 9) return 3;
        return 0;
      };
      var n = Math.floor(cellId / 27);
      var r = cellId % 27;
      var s = Math.floor(r / 9);
      var t = r % 9;
      var c = classify(t);
      var grid = (n * 3) - 1 + c;
      var cell = (s * 3) + (t % 3);
      return cellId + ": " + "(" + grid + ", " + cell + ")";
    };

    $scope.challengeAI = function () {
      //Create the AI
    }

    gameHubProxy.client.broadcastState = function (state) {

    }

    //$("#gamerName").val(prompt("Enter your name:", ""));

    // print welcome message.
    var divMsg = document.getElementById("welcomeMsg");
    var h1Msg = document.createElement("H1");
    var msg = document.createTextNode("Welcome to the game " + $("#gamerName").val());
    h1Msg.appendChild(msg);
    divMsg.appendChild(h1Msg);

    for (var i = 0; i < 81; i++) {
      $scope.cellIds.push(i);
    }

    $.connection.hub.start(); // connect to signalr hub
    //gameHubProxy.server.announce($("#gamerName").val());
  }]);
})();