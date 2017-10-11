var Move = function (grid, cell, player) {
  this.Grid = grid;
  this.Cell = cell;
  this.Player = player;
}

var Border = function (left, top, width, height) {
  this.Left = left;
  this.Top = top;
  this.Height = height;
  this.Width = width;
}

var Util = function () {

  this.getDefaultBorders = function () {
    return [
      new Border(0, 0, 110, 5), new Border(0, 0, 5, 110), new Border(105, 0, 5, 110), new Border(0, 105, 110, 5),
      new Border(105, 0, 110, 5), new Border(210, 0, 5, 110), new Border(105, 105, 110, 5), 
      new Border(210, 0, 110, 5), new Border(315, 0, 5, 110), new Border(210, 105, 110, 5), 
      new Border(0, 105, 5, 110), new Border(105, 105, 5, 110), new Border(0, 210, 110, 5), 
      new Border(210, 105, 5, 110), new Border(105, 105, 110, 5), 

    ];
  }

  this.haveSameContent = function (list1, list2) {
    if (list1.length !== list2.length) return false;
    for (var i = 0; i < list1.length; i++) {
      if (list1[i] !== list2[i]) return false;
    }
    return true;
  }

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

  this.getEnabledCells = function (move) {
    var ids = [];
    var enabled = [];
    for (var i = 0; i < 81; i++) {
      ids.push(i);
    }
    ids.forEach(id => {
      if (this.extractMove(id).Grid === move.Cell) {
        enabled.push(id);
      }
    });
    return enabled;
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
    return new Move(grid, cell, ".");
  };
}