var Move = function (grid, cell, player) {
  this.Grid = grid;
  this.Cell = cell;
  this.Player = player;
}

var Border = function (left, top, width, height, id) {
  this.Left = left;
  this.Top = top;
  this.Height = height;
  this.Width = width;
  this.Id = id;
}

var Util = function () {

  this.getDefaultBorders = function () {
    return [
      new Border(0, 0, 110, 5, "border-0"), new Border(0, 0, 5, 110, "border-1"), new Border(105, 0, 5, 110, "border-2"), new Border(0, 105, 110, 5, "border-3"),
      new Border(105, 0, 110, 5, "border-4"), new Border(210, 0, 5, 110, "border-5"), new Border(105, 105, 110, 5, "border-6"), 
      new Border(210, 0, 110, 5, "border-7"), new Border(315, 0, 5, 110, "border-8"), new Border(210, 105, 110, 5, "border-9"), 
      new Border(0, 105, 5, 110, "border-10"), new Border(105, 105, 5, 110, "border-11"), new Border(0, 210, 110, 5, "border-12"), 
      new Border(210, 105, 5, 110, "border-13"), new Border(105, 210, 110, 5, "border-14"), 
      new Border(315, 105, 5, 110, "border-15"), new Border(210, 210, 110, 5, "border-16"), 
      new Border(0, 210, 5, 110, "border-17"), new Border(105, 210, 5, 110, "border-18"), new Border(0, 315, 110, 5, "border-19"), 
      new Border(210, 210, 5, 110, "border-20"), new Border(105, 315, 110, 5, "border-21"),
      new Border(315, 210, 5, 110, "border-22"), new Border(210, 315, 110, 5, "border-23")
    ];
  }

  this.getGridBorders = function() {
    return [[0, 1, 2, 3], [2, 4, 5, 6], [5, 7, 8, 9],
            [10, 3, 11, 12], [11, 6, 13, 14], [13, 9, 15, 16],
            [17, 12, 18, 19], [18, 14, 20, 21], [20, 16, 22, 23]];
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