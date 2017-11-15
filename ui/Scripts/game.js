(function () {
  const app = angular.module("myApp", []);
  const util = new Util();
  const canvasController = new CanvasController();
  const apiBaseUrl = 'http://localhost:50032'
  const agniKaiUri = "agnikai";
  const gamerUri = "gamer";
  const usersUri = "users";
  const errorMessage = function (data, status) {
    return `Error: ${status}${data.Message !== undefined ? (` ${data.Message}`) : ""}`;
  };
  $.connection.hub.url = `${apiBaseUrl}/signalr`;
  const gameHubProxy = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.cells = [];
    $scope.indicators = [".", "x", "o"];
    $scope.history = [];
    $scope.previousState = [];
    $scope.selectedPlayer = undefined;
    $scope.challenger = undefined;
    $scope.game = undefined;

    $scope.getActivePlayers = function () {
      $http.get(`${apiBaseUrl}/${usersUri}/all`)
        .success(function (data, _) {
          $scope.playersOnline = data.filter(player => player.Name !== $("#gamer-name").val());
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    };

    $scope.disableAllCells = function () {
      $scope.cells.forEach(cell => {
        cell.Disabled = true;
      });
    }

    $scope.recordAction = function (event) {
      const cellId = canvasController.handledMouseClicked(event);
      if ($scope.cells[cellId].Disabled) return;
      const move = util.cellIdToMove(cellId);
      move.Player = $scope.indicator;
      $scope.history.push(move);  /* add move to history. */
      $scope.previousState[move.Grid][move.Cell] = util.indicatorTofield(move.Player);
      const encodeMove = $("<div />").text(`${cellId}: (${move.Grid},${move.Cell})`).html();
      $("#player-move").append(`<li>${encodeMove}</li>`);
      $scope.disableAllCells();
      if ($scope.game = "PvA") {
        gameHubProxy.server.sendMoveAI($scope.agnikaiTicket, move.Grid, move.Cell, move.Player);
      }
      else if ($scope.game = "PvP") {
        gameHubProxy.server.sendMovePlayer($scope.agnikaiTicket, move.Grid, move.Cell, move.Player);
      }
    }

    $scope.setupPvA = function () {
      $http.get(`${apiBaseUrl}/${agniKaiUri}/initiate`).then(response => {
        $scope.agnikaiTicket = response.data;
        const req1 = {
          method: "POST",
          url: `${apiBaseUrl}/${gamerUri}/create/ai`,
          data: { ticket: $scope.agnikaiTicket }
        };
        $http(req1).then();
        const req2 = {
          method: "POST",
          url: `${apiBaseUrl}/${usersUri}/submit`,
          data: {
            token: $scope.gameToken.Value,
            ticket: $scope.agnikaiTicket
          }
        }
        $http(req2).then(response => {
          $scope.indicator = util.fieldToIndicator(response.data);
        });
        gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
      });
    }

    $scope.setupPvP = function () {
      $http.get(`${apiBaseUrl}/${agniKaiUri}/initiate`).then(response => {
        $scope.agnikaiTicket = response.data;
        gameHubProxy.server.agniKaiStartNotification($scope.agnikaiTicket, $scope.selectedPlayer);
        const req = {
          method: "POST",
          url: `${apiBaseUrl}/${usersUri}/submit`,
          data: {
            token: $scope.gameToken.Value,
            ticket: $scope.agnikaiTicket
          }
        }
        $http(req).then();
        gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
      });
    }

    $scope.notifyOfAgniKaiStart = function (agnikaiTicket) {
      $scope.agnikaiTicket = agnikaiTicket;
      const req = {
        method: "POST",
        url: `${apiBaseUrl}/${usersUri}/submit`,
        data: {
          token: $scope.gameToken.Value,
          ticket: $scope.agnikaiTicket
        }
      }
      $http(req).then();
      gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
    }

    $scope.updateCellContents = function (fields) {
      $scope.cells.forEach(cell => {
        const move = util.cellIdToMove(cell.Id);
        cell.Content = util.fieldToIndicator(fields[move.Grid][move.Cell]);
      });
    }

    $scope.challengeAI = function () {
      $scope.game = "PvA";
      $scope.setupPvA();
      document.getElementById("challenge-ai-btn").disabled = true;
      document.getElementById("agnikai-btn").disabled = true;
    }

    $scope.viewPlayerInfo = function (player) {
      $scope.selectedPlayer = player;
      console.log(`view ${player.Name} information`);
    }

    $scope.challengeSelectedPlayer = function () {
      // should have a game routine that both challengeAI and challengeSelectedPlayer could use.
      $scope.game = "PvP";
      console.log(`view ${$scope.selectedPlayer.Name} information`);
      gameHubProxy.server.notifyPlayerAsync($scope.selectedPlayer.Name, $("#gamer-name").val());
    }

    $scope.enableCells = function (ids) {
      ids.forEach(id => {
        if ($scope.cells[id].Content === ".") {
          $scope.cells[id].Disabled = false;
        }
      });
    }

    gameHubProxy.client.broadcastState = function (state) {
      const fields = state.Fields;
      const move = util.compareStates($scope.previousState, fields);
      $scope.history.push(move); /* the state difference is the new move. */
      $scope.previousState = state.Fields;
      $scope.updateCellContents(fields);
      canvasController.reloadBoard($scope.cells, move.Cell, util.moveToCellId(move));
      $scope.enableCells(util.getEnabledCells(move));
      if ($scope.game === "PvA") {
      }
      else if ($scope.game === "PvP") {

      }
    }

    $scope.commenceAgniKai = function () {
      console.log("commence agnikai.");
      $scope.setupPvP();
      // get agnikai token and send it to opponent.
    }

    $scope.acceptChallenge = function () {
      $scope.game = "PvP";
      gameHubProxy.server.notifyOfChallengeAcceptedAsync($("#gamer-name").val(), $scope.challenger);
    }

    gameHubProxy.client.notifyOfChallengeAccpeted = function (challengeeId) {
      if ($scope.selectedPlayer !== challengeeId) console.log("challengee mismatch.");
      document.getElementById("challenge-notification-box").style.display = "block";
      $("#challenge-msg").append(`${$scope.selectedPlayer.Name} accepted your challenge`);
      document.getElementById("accept-challenge-btn").style.display = "none";
      document.getElementById("commence-agnikai-btn").style.display = "block";
      document.getElementById("commence-agnikai-btn").innerHTML = "Commence AgniKai";
    }

    gameHubProxy.client.notifyOfChallenge = function (challengerId) {
      $scope.challenger = challengerId;
      document.getElementById("challenge-notification-box").style.display = "block";
      $("#challenge-msg").append(`${challengerId} would like to challenge you`);
      document.getElementById("accept-challenge-btn").innerHTML = "Accept challenge";
    }

    $("#gamer-name").val(prompt("Enter Your Identifier:", ""));

    /* print welcome message. */
    const divMsg = document.getElementById("welcome-msg");
    const h1Msg = document.createElement("H1");
    const msg = document.createTextNode(`Welcome to the game ${$("#gamer-name").val()}`);
    h1Msg.appendChild(msg);
    divMsg.appendChild(h1Msg);

    /* initialise cell ids and contents. */
    for (let i = 0; i < 81; i++) {
      $scope.cells.push(new Cell(i));
    }

    for (let i = 0; i < 9; i++) {
      $scope.previousState.push([0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }

    $.connection.hub.start().done(() => {
      $http.get(`${apiBaseUrl}/${usersUri}/login?name=${$("#gamer-name").val()}`).then(response => {
        $scope.gameToken = response.data;
        gameHubProxy.server.announceAsync($("#gamer-name").val());
        canvasController.initCanvas();
      });  /* log player in. */
    }); /* connect to signalr hub */
  }]);
})();