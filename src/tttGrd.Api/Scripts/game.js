(function () {
  const app = angular.module("myApp", []);
  const util = new Util();
  const agniKaiUri = "agnikai";
  const gamerUri = "gamer";
  const usersUri = "users";
  const errorMessage = function (data, status) {
    return `Error: ${status}${data.Message !== undefined ? (` ${data.Message}`) : ""}`;
  };
  const gameHubProxy = $.connection.gameHub; // create a proxy to signalr hub on web server

  app.controller("myCtrl", ["$http", "$scope", function ($http, $scope) {
    $scope.cellIds = [];
    $scope.cellContents = [];
    $scope.indicators = [".", "x", "o"];
    $scope.history = [];
    $scope.previousState = [];
    $scope.defaultBorders = util.getDefaultBorders();
    $scope.gridBorders = util.getGridBorders();
    $scope.selectedPlayer = undefined;

    $scope.getActivePlayers = function () {
      $http.get(`${usersUri}/all`)
        .success(function (data, _) {
          $scope.playersOnline = data;
        })
        .error(function (data, status) {
          $scope.errorToSearch = errorMessage(data, status);
        });
    };

    $scope.disableAllCells = function () {
      $scope.cellIds.forEach(id => {
        document.getElementById(id).disabled = true;
      });
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
        const moveBeforeLast = $scope.history[$scope.history.length - 2];
        const border = $scope.gridBorders[moveBeforeLast.Cell].map(id => $scope.defaultBorders[id]);
        util.editBorders(border, "#000000");
        util.colourCell(moveBeforeLast, "#D3D3D3"); /* change cell border back to normal (i.e. lightgray). */
      }
      gameHubProxy.server.sendMoveAI($scope.agnikaiTicket, move.Grid, move.Cell, move.Player);
    };

    $scope.setupPvA = function () {
      $http.get(`${agniKaiUri}/initiate`).then(response => {
        $scope.agnikaiTicket = response.data;
        const req1 = {
          method: "POST",
          url: `${gamerUri}/create/ai`,
          data: { ticket: $scope.agnikaiTicket }
        };
        $http(req1).then((resp) => {
          const aiIndicator = resp.data;
          if (aiIndicator === $scope.indicators[1]) $scope.indicator = $scope.indicators[2];
          else $scope.indicator = $scope.indicators[1];
        });
        const req2 = {
          method: "POST",
          url: `${usersUri}/submit`,
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
      for (let k = 0; k < $scope.cellContents.length; k++) {
        const move = util.cellIdToMove(k);
        $scope.cellContents[k] = util.fieldToIndicator(fields[move.Grid][move.Cell]);
      }
    }

    $scope.reloadBoard = function () {
      for (let j = 0; j < $scope.cellContents.length; j++) {
        document.getElementById(j).innerHTML = $scope.cellContents[j];
      }
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

    $scope.enableCells = function (ids) {
      ids.forEach(id => {
        if ($scope.cellContents[id] === ".") {
          document.getElementById(id).disabled = false;
        }
      });
    }

    gameHubProxy.client.broadcastState = function (state) {
      const fields = state.Fields;
      const move = util.compareStates($scope.previousState, fields);
      $scope.history.push(move); /* the state difference is the new move. */
      $scope.previousState = state.Fields;
      $scope.updateCellContents(fields);
      $scope.reloadBoard();
      $scope.enableCells(util.getEnabledCells(move));
      const border = $scope.gridBorders[move.Cell].map(id => $scope.defaultBorders[id]);
      util.editBorders(border, "#FF0000");  /* highlight the grid to play in as red. */
      util.colourCell(move, "#FFD700"); /* highlight oponent cell in gold. */
    }

    $scope.drawBorders = function () {
      $scope.defaultBorders.forEach(border => {
        $("#grid").append(`<div style="position:absolute;left:${border.Left}px;top:${border.Top}px;height:${border.Height}px;width:${border.Width}px;background:#000000;z-index:1;" id="${border.Id}"></div>`);
      });
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
      $scope.cellContents.push(".");  /* all cells are empty when the game starts. */
    }

    for (let i = 0; i < 9; i++) {
      $scope.previousState.push([0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }

    $scope.drawBorders();

    $.connection.hub.start().done(() => {
      $http.get(`${usersUri}/login?name=${$("#gamerName").val()}`).then(response => {
        $scope.gameToken = response.data;
      });  /* log player in. */
    }); /* connect to signalr hub */
  }]);
})();