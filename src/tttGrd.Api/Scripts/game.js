(function () {
  var app = angular.module("myApp", []);
  var agniKaiUri = "agnikai";
  var gamerUri = "gamer";
  var usersUri = "users";
  var errorMessage = function (data, status) {
    return "Error: " + status + (data.Message !== undefined ? (" " + data.Message) : "");
  };
  var gameHubProxy = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.cellIds = [];
    $scope.cellContents = [];
    $scope.indicators = [".", "x", "o"];
    
    $scope.getActivePlayers = function () {
      $http.get(usersUri + "/all")
        .success(function (data, _) {
          $scope.playersOnline = data;
          if (data.length > 0) {
            data.forEach(user => {
              var encodedUser = $("<div />").text(user.Name).html();
              $("#activeplayers").append("<li><strong>" + encodedUser + "</strong></li>");
            });
          }
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    };

    $scope.recordMove = function (cellId) {
      var move = $scope.extractMove(cellId);
      var encodeMove = $("<div />").text(move[0] + ": (" + move[1] + "," + move[2] + ")").html();
      $("#playerOnline").append("<li>" + encodeMove + "</li>");
      document.getElementById(cellId).disabled = true;
      //document.getElementById(cellId).style.background = "#778899";
      gameHubProxy.server.sendMove($scope.agnikaiTicket, move[1], move[2], "x");
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
      return [cellId, grid, cell];
    };
    
    $scope.setupPvA = function () {
      $http.get(agniKaiUri + "/initiate").then(response => {
        $scope.agnikaiTicket = response.data;
        $http.get(gamerUri + "/create/ai?agnikaiTicket=" + $scope.agnikaiTicket).then((_) => {
          //TODO I don't know yet.
          /* one possibility would be getting the game name. */
        });
        gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
      });
    }

    $scope.updateCellContents = function (fields) {
      var fieldToIndicator = function(field) {
        if (field === 0) return ".";
        if (field === 1) return "x";
        return "o";
      };
      for (var k = 0; k < $scope.cellContents.length; k++) {
        var m = $scope.extractMove(k);
        $scope.cellContents[k] = fieldToIndicator(fields[m[1]][m[2]]);
      }
    }

    $scope.reloadBoard = function() {
      for (var j = 0; j < $scope.cellContents.length; j++) {
        document.getElementById(j).innerHTML = $scope.cellContents[j];
      }
    }

    $scope.challengeAI = function () {
      $scope.setupPvA();
      document.getElementById("challenge-ai-btn").disabled = true;
    }

    gameHubProxy.client.broadcastState = function (state) {
      var fields = state.Result.Fields;
      $scope.updateCellContents(fields);
      $scope.reloadBoard();
      console.log(fields);
    }
    
    //$("#gamerName").val(prompt("Enter your name:", ""));

    /* print welcome message. */
    var divMsg = document.getElementById("welcomeMsg");
    var h1Msg = document.createElement("H1");
    var msg = document.createTextNode("Welcome to the game " + $("#gamerName").val());
    h1Msg.appendChild(msg);
    divMsg.appendChild(h1Msg);

    /* initialise cell ids and contents. */
    for (var i = 0; i < 81; i++) {
      $scope.cellIds.push(i);
      $scope.cellContents.push(".");  /* all cells are empty when the game starts. */
    }

    $.connection.hub.start().done(() => {
      $http.get(usersUri + "login?name=" + $("#gamerName").val());  /* log player in. */
      gameHubProxy.server.announce($("#gamerName").val());
    }); /* connect to signalr hub */
  }]);
})();