﻿(function () {
  const app = angular.module("myApp", []);
  const util = new Util();
  const canvasController = new CanvasController();
  const apiBaseUrl = 'http://localhost:50032/'
  const agniKaiUri = "agnikai";
  const gamerUri = "gamer";
  const usersUri = "users";
  const errorMessage = function (data, status) {
    return `Error: ${status}${data.Message !== undefined ? (` ${data.Message}`) : ""}`;
  };
  $.connection.hub.url = `${apiBaseUrl}signalr`;
  const gameHubProxy = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.cellIds = [];
    $scope.cells = [];
    $scope.cellContents = [];
    $scope.indicators = [".", "x", "o"];
    $scope.history = [];
    $scope.previousState = [];
    $scope.defaultBorders = util.getDefaultBorders();
    $scope.gridBorders = util.getGridBorders();
    $scope.selectedPlayer = undefined;

    $scope.getActivePlayers = function () {
      $http.get(`${apiBaseUrl}${usersUri}/all`)
        .success(function (data, _) {
          $scope.playersOnline = data;
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
      $("#playerOnline").append(`<li>${encodeMove}</li>`);
      $scope.disableAllCells();
      /* un-highlight the highlighted mini-grid if any. */
      if ($scope.history.length > 2) {
        const moveBeforeLast = $scope.history[$scope.history.length - 2];
        const border = $scope.gridBorders[moveBeforeLast.Cell].map(id => $scope.defaultBorders[id]);
        //util.editBorders(border, "#000000");
        //util.colourCell(moveBeforeLast, "#D3D3D3"); /* change cell border back to normal (i.e. lightgray). */
      }
      gameHubProxy.server.sendMoveAI($scope.agnikaiTicket, move.Grid, move.Cell, move.Player);
    }

    $scope.recordMove = function (cellId) {
      const move = util.cellIdToMove(cellId);
      move.Player = $scope.indicator;
      $scope.history.push(move);  /* add move to history. */
      $scope.previousState[move.Grid][move.Cell] = util.indicatorTofield(move.Player);
      const encodeMove = $("<div />").text(`${cellId}: (${move.Grid},${move.Cell})`).html();
      $("#playerOnline").append(`<li>${encodeMove}</li>`);
      $scope.disableAllCells();
      /* un-highlight the highlighted mini-grid if any. */
      if ($scope.history.length > 2) {
        //const moveBeforeLast = $scope.history[$scope.history.length - 2];
        //const border = $scope.gridBorders[moveBeforeLast.Cell].map(id => $scope.defaultBorders[id]);
        //util.editBorders(border, "#000000");
        canvasController.highlightCell(cellId);
        //util.colourCell(moveBeforeLast, "#D3D3D3"); /* change cell border back to normal (i.e. lightgray). */
      }
      gameHubProxy.server.sendMoveAI($scope.agnikaiTicket, move.Grid, move.Cell, move.Player);
    };

    $scope.setupPvA = function () {
      $http.get(`${apiBaseUrl}${agniKaiUri}/initiate`).then(response => {
        $scope.agnikaiTicket = response.data;
        const req1 = {
          method: "POST",
          url: `${apiBaseUrl}${gamerUri}/create/ai`,
          data: { ticket: $scope.agnikaiTicket }
        };
        $http(req1).then((resp) => {
          const aiIndicator = resp.data;
          if (aiIndicator === $scope.indicators[1]) $scope.indicator = $scope.indicators[2];
          else $scope.indicator = $scope.indicators[1];
        });
        const req2 = {
          method: "POST",
          url: `${apiBaseUrl}${usersUri}/submit`,
          data: {
            token: $scope.gameToken.Value,
            ticket: $scope.agnikaiTicket
          }
        }
        $http(req2).then();
        gameHubProxy.server.joinAgniKai($scope.agnikaiTicket);
      });
    }

    $scope.setupPvP = function (player) {
      $http.get(`${apiBaseUrl}${agniKaiUri}/initiate`).then(response => {
        $scope.agnikaiTicket = response.data;
        gameHubProxy.server.callAgniKai($scope.agnikaiTicket);
      });
    }

    $scope.updateCellContents = function (fields) {
      $scope.cells.forEach(cell => {
        const move = util.cellIdToMove(cell.Id);
        cell.Content = util.fieldToIndicator(fields[move.Grid][move.Cell]);
      });
    }

    $scope.challengeAI = function () {
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
      console.log(`view ${player.Name} information`);
    }

    $scope.enableCells = function (ids) {
      ids.forEach(id => {
        if ($scope.cellContents[id] === ".") {
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
      canvasController.reloadBoard($scope.cells, move.Cell);
      $scope.enableCells(util.getEnabledCells(move));
      const border = $scope.gridBorders[move.Cell].map(id => $scope.defaultBorders[id]);
    }

    //$("#gamerName").val(prompt("Enter your name:", ""));

    /* print welcome message. */
    const divMsg = document.getElementById("welcomeMsg");
    const h1Msg = document.createElement("H1");
    const msg = document.createTextNode(`Welcome to the game ${$("#gamerName").val()}`);
    h1Msg.appendChild(msg);
    divMsg.appendChild(h1Msg);

    /* initialise cell ids and contents. */
    for (let i = 0; i < 81; i++) {
      $scope.cellIds.push(i);
      $scope.cells.push(new Cell(i));
      $scope.cellContents.push(".");  /* all cells are empty when the game starts. */
    }

    for (let i = 0; i < 9; i++) {
      $scope.previousState.push([0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }

    //$scope.drawBorders();

    $.connection.hub.start().done(() => {
      $http.get(`${apiBaseUrl}${usersUri}/login?name=${$("#gamerName").val()}`).then(response => {
        $scope.gameToken = response.data;
        canvasController.initCanvas();
      });  /* log player in. */
    }); /* connect to signalr hub */
  }]);
})();