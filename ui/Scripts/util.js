let Move = function (grid, cell, player) {
  this.Grid = grid;
  this.Cell = cell;
  this.Player = player;
}

let Cell = function (id) {
  this.Id = id;
  this.Disabled = false;
  this.Content = ".";
}

let Vector = function (x, y) {
  this.X = x;
  this.Y = y;
}

let Border = function (left, top, width, height, id) {
  this.Left = left;
  this.Top = top;
  this.Height = height;
  this.Width = width;
  this.Id = id;
}

let Util = function () {
  this.haveSameContent = function (list1, list2) {
    if (list1.length !== list2.length) return false;
    for (let i = 0; i < list1.length; i++) {
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
    const ids = [];
    const enabled = [];
    for (let i = 0; i < 81; i++) {
      ids.push(i);
    }
    ids.forEach(id => {
      if (this.cellIdToMove(id).Grid === move.Cell) {
        enabled.push(id);
      }
    });
    return enabled;
  }

  this.compareStates = function (oldState, newState) {
    let grid = undefined;
    let cell = undefined;
    if (oldState === undefined) {
      /* iterate through the new state. the first field that is not empty is the new move. */
      for (let j = 0; j < newState.length; j++) {
        for (let k = 0; k < newState[j].length; k++) {
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
      for (let j = 0; j < newState.length; j++) {
        for (let k = 0; k < newState[j].length; k++) {
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
    return grid === undefined ? undefined : new Move(grid, cell, this.fieldToIndicator(newState[grid][cell]));
  }

  this.cellIdToMove = function (cellId) {
    const classify = function (index) {
      if (index < 3) return 1;
      if (index < 6) return 2;
      if (index < 9) return 3;
      return 0;
    };
    const n = Math.floor(cellId / 27);
    const r = cellId % 27;
    const s = Math.floor(r / 9);
    const t = r % 9;
    const c = classify(t);
    const grid = (n * 3) - 1 + c;
    const cell = (s * 3) + (t % 3);
    return new Move(grid, cell, ".");
  };

  this.moveToCellId = function (move) {
    const n = Math.floor(move.Grid / 3);
    const m = move.Grid % 3;
    const x = Math.floor(move.Cell / 3);
    const y = move.Cell % 3;
    return n * 27 + x * 9 + m * 3 + y;
  }
}