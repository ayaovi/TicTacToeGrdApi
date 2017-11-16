let CanvasController = function () {
  var canvas = document.getElementById('canvas');
  var context = canvas.getContext('2d');

  this.initCanvas = function () {
    draw();
  }

  this.handledMouseClicked = function (e) {
    const x = e.x - canvas.offsetLeft;
    const y = e.y - canvas.offsetTop;
    return Math.floor(x / 60) + Math.floor(y / 60) * 9;
  }

  this.reloadBoard = function (cells, gridToHighlight, cellToHighlight) {
    draw(gridToHighlight);
    highlightCell(cellToHighlight, "#FFD700");  /* higlight cell in gold. */
    cells.forEach(cell => {
      if (cell.Content == "x") {
        //drawX.
        const pos = getCellPosition(cell.Id, 60);
        drawX(pos.X + 7, pos.Y + 7);
      }
      else if (cell.Content == "o") {
        //drawO.
        const pos = getCellPosition(cell.Id, 60);
        drawO(pos.X + 30, pos.Y + 30, 20, 18);
      }
    });
  }

  function highlightCell(cellId, border) {
    if (cellId === undefined) return;
    const pos = getCellPosition(cellId, 60);
    context.clearRect(pos.X, pos.Y, 60, 60);
    const currentStyle = context.strokeStyle;
    context.strokeStyle = border;
    drawCell(cellId, 60);
    context.strokeStyle = currentStyle;
  }

  function drawX(x, y) {
    drawLine(x, y, x + 45, y + 45);
    drawLine(x + 45, y, x, y + 45);
  }

  function drawO(x, y, r1, r2) {
    const currentWidth = context.lineWidth; /* save initial line width setting. */
    context.beginPath();  /* always beginPath() */
    context.ellipse(x, y, r1, r2, 90 * Math.PI / 180, 0, 2 * Math.PI);
    context.lineWidth = 3;
    context.stroke();   /* stroke() ends the path. */
    context.lineWidth = currentWidth; /* restore initial line width setting. */
  }

  function drawLine(x1, y1, x2, y2) {
    const currentWidth = context.lineWidth;
    context.beginPath();
    context.moveTo(x1, y1);
    context.lineTo(x2, y2);
    context.lineWidth = 3;
    context.stroke();
    context.lineWidth = currentWidth;
  }

  function getCellPosition(id, size) {
    const cellXId = id % 9;
    const cellYId = Math.floor(id / 9);
    const x = cellXId * size;
    const y = cellYId * size;
    return new Vector(x, y);
  }

  function draw(gridToHighlight) {
    context.clearRect(canvas.offsetLeft, canvas.offsetTop, 540, 540); /* clear the board. */
    for (let i = 0; i < 9; i++) {
      if (i === gridToHighlight) continue;
      draw3x3Box(i, 60);
    }
    if (gridToHighlight != undefined) {
      draw3x3Box(gridToHighlight, 60, "#F00");
    }
  }

  function drawCell(id, size) {
    const cellPosition = getCellPosition(id, size);
    context.strokeRect(cellPosition.X, cellPosition.Y, size, size);
  }

  function draw3x3Box(id, cellSize, border = "#000000") {
    const boxXId = id % 3;
    const boxYId = Math.floor(id / 3);
    const boxXpos = boxXId * 3 * cellSize;
    const boxYpos = boxYId * 3 * cellSize;
    const currentWidth = context.lineWidth;
    const currentStyle = context.strokeStyle;
    context.lineWidth = 3;
    context.strokeStyle = border;
    context.clearRect(boxXId * 3 * cellSize, boxYId * 3 * cellSize, 3 * cellSize, 3 * cellSize);
    context.strokeRect(boxXId * 3 * cellSize, boxYId * 3 * cellSize, 3 * cellSize, 3 * cellSize);
    context.lineWidth = currentWidth;
    context.strokeStyle = currentStyle;
    for (let i = 0; i < 3; i++) {
      for (let j = 0; j < 3; j++) {
        drawCell(boxYId * 27 + i * 9 + j + boxXId * 3, cellSize);
      }
    }
  }

  // setup
  canvas.width = 540;
  canvas.height = 540;
  context.globalAlpha = 1.0;
}