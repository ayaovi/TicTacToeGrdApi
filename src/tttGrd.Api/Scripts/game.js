(function () {
  var app = angular.module("myApp", []);
  var util = new Util();
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
    $scope.history = [];
    $scope.previousState = [];
    $scope.disable = [];

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

    $scope.disableAllCells = function () {
      for (var j = 0; j < $scope.disable.length; j++) {
        //$scope.disable[j] = "true";
        document.getElementById(j).disabled = true;
      }
    }

    $scope.recordMove = function (cellId) {
      var move = util.extractMove(cellId);
      $scope.history.push(new Move(move[1], move[2], $scope.indicator));  /* add move to history. */
      $scope.previousState[move[1]][move[2]] = util.indicatorTofield($scope.indicator);
      var encodeMove = $("<div />").text(move[0] + ": (" + move[1] + "," + move[2] + ")").html();
      $("#playerOnline").append("<li>" + encodeMove + "</li>");
      $scope.disableAllCells();
      //document.getElementById(cellId).style.background = "#778899";
      gameHubProxy.server.sendMoveAI($scope.agnikaiTicket, move[1], move[2], $scope.indicator);
    };

    $scope.setupPvA = function () {
      $http.get(agniKaiUri + "/initiate").then(response => {
        $scope.agnikaiTicket = response.data;
        var req1 = {
          method: "POST",
          url: gamerUri + "/create/ai",
          data: { ticket: $scope.agnikaiTicket }
        };
        $http(req1).then((resp) => {
          var aiIndicator = resp.data;
          if (aiIndicator === $scope.indicators[1]) $scope.indicator = $scope.indicators[2];
          else $scope.indicator = $scope.indicators[1];
        });
        var req2 = {
          method: "POST",
          url: usersUri + "/submit",
          data: {
            token: $scope.gameToken.Value,
            ticket: $scope.agnikaiTicket
          }
        }
        $http(req2).then();
        gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
      });
    }

    $scope.updateCellContents = function (fields) {
      for (var k = 0; k < $scope.cellContents.length; k++) {
        var m = util.extractMove(k);
        $scope.cellContents[k] = util.fieldToIndicator(fields[m[1]][m[2]]);
      }
    }

    $scope.reloadBoard = function () {
      for (var j = 0; j < $scope.cellContents.length; j++) {
        document.getElementById(j).innerHTML = $scope.cellContents[j];
        //if ($scope.cellContents[j] !== ".") document.getElementById(j).disabled = true;
      }
    }

    $scope.challengeAI = function () {
      $scope.setupPvA();
      document.getElementById("challenge-ai-btn").disabled = true;
    }

    gameHubProxy.client.broadcastState = function (state) {
      var fields = state.Fields;
      $scope.history.push(util.compareStates($scope.previousState, fields)); /* the state difference is the new move. */
      $scope.previousState = state.Fields;
      $scope.updateCellContents(fields);
      $scope.reloadBoard();
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
      $scope.disable.push("false");
    }

    for (var i = 0; i < 9; i++) {
      $scope.previousState.push([0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }

    $.connection.hub.start().done(() => {
      $http.get(usersUri + "/login?name=" + $("#gamerName").val()).then(response => {
        $scope.gameToken = response.data;
      });  /* log player in. */
      gameHubProxy.server.announce($("#gamerName").val());
    }); /* connect to signalr hub */
  }]);
})();