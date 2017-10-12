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
    $scope.defaultBorders = util.getDefaultBorders();

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
      move.Player = $scope.indicator;
      $scope.history.push(move);  /* add move to history. */
      $scope.previousState[move.Grid][move.Cell] = util.indicatorTofield(move.Player);
      var encodeMove = $("<div />").text(cellId + ": (" + move.Grid + "," + move.Cell + ")").html();
      $("#playerOnline").append("<li>" + encodeMove + "</li>");
      $scope.disableAllCells();
      //document.getElementById(cellId).style.background = "#778899";
      gameHubProxy.server.sendMoveAI($scope.agnikaiTicket, move.Grid, move.Cell, move.Player);
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
        var move = util.extractMove(k);
        $scope.cellContents[k] = util.fieldToIndicator(fields[move.Grid][move.Cell]);
      }
    }

    $scope.reloadBoard = function () {
      for (var j = 0; j < $scope.cellContents.length; j++) {
        document.getElementById(j).innerHTML = $scope.cellContents[j];
      }
    }

    $scope.challengeAI = function () {
      $scope.setupPvA();
      document.getElementById("challenge-ai-btn").disabled = true;
    }

    $scope.enableCells = function (ids) {
      ids.forEach(id => {
        if ($scope.cellContents[id] === ".") {
          document.getElementById(id).disabled = false;
        }
      });
    }

    gameHubProxy.client.broadcastState = function (state) {
      var fields = state.Fields;
      var move = util.compareStates($scope.previousState, fields);
      $scope.history.push(move); /* the state difference is the new move. */
      $scope.previousState = state.Fields;
      $scope.updateCellContents(fields);
      $scope.reloadBoard();
      $scope.enableCells(util.getEnabledCells(move));
    }

    $scope.drawBorders = function () {
      $scope.defaultBorders.forEach(border => {
        $('#grid').append('<div style="position:absolute;left:' + border.Left + 'px;top:'
          + border.Top + 'px;height:' + border.Height + 'px;width:' + border.Width +'px;background:#000000;z-index:1;"></div>');
      });
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

    $scope.drawBorders();

    $.connection.hub.start().done(() => {
      $http.get(usersUri + "/login?name=" + $("#gamerName").val()).then(response => {
        $scope.gameToken = response.data;
      });  /* log player in. */
      gameHubProxy.server.announce($("#gamerName").val());
    }); /* connect to signalr hub */
  }]);
})();