var Move = function (grid, cell, player) {
  this.Grid = grid;
  this.Cell = cell;
  this.Player = player;
}

var Util = function () {

  this.fieldToIndicator = function (field) {
    if (field === 0) return ".";
    if (field === 1) return "x";
    return "o";
  }

  this.indicatorTofield = function (indicator) {
    if (indicator === ".") return 0;
    if (indicator === "x") return 1;
    return 2;
  }

  this.compareStates = function (oldState, newState) {
    var grid = undefined;
    var cell = undefined;
    if (oldState === undefined) {
      /* iterate through the new state. the first field that is not empty is the new move. */
      for (var j = 0; j < newState.length; j++) {
        for (var k = 0; k < newState[j].length; k++) {
          if (newState[j][k] !== 0) {
            cell = k;
            break;
          }
        }
        if (cell !== undefined) {
          grid = j;
          break;
        }
      }
    }
    else {
      /* iterate through both old and new state. the first field where they do not have the same value is the new move. */
      for (var j = 0; j < newState.length; j++) {
        for (var k = 0; k < newState[j].length; k++) {
          if (newState[j][k] !== oldState[j][k]) {
            cell = k;
            break;
          }
        }
        if (cell !== undefined) {
          grid = j;
          break;
        }
      }
    }
    return new Move(grid, cell, this.fieldToIndicator(newState[grid][cell]));
  }

  this.extractMove = function (cellId) {
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
}