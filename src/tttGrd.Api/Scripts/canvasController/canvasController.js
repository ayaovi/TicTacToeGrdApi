var counter = 0;
let CanvasController = function () {
  var canvas = document.getElementById('canvas');
  var context = canvas.getContext('2d');

  this.initCanvas = function () {
    draw();
  }

  this.handledMouseClicked = function (e) {
    const x = e.x - canvas.offsetLeft;
    const y = e.y - canvas.offsetTop;

    const cellTopLeftX = Math.floor(x / 60) * 60;
    const cellTopLeftY = Math.floor(y / 60) * 60;

    //console.log(`xx: ${x} yy: ${y}`);
    //drawX(cellTopLeftX * 60 + 5, cellTopLeftY * 60 + 5);
    //drawCircle(cellTopLeftX + 30, cellTopLeftY + 30, 20, 18);
    //console.log(`x: ${Math.floor(x / 60)} and y: ${Math.floor(y / 60)}`);
    return Math.floor(x / 60) + Math.floor(y / 60) * 9;
    //console.log(pos);
  }

  this.reloadBoard = function (cells) {
    cells.foreach(cell => {
      if (cell.Content == "x") {
        //drawX.
        const cellTopLeft = getCellTopLeftPosition(cell.Id);
        drawX(cellTopLeft.X * 60 + 5, cellTopLeft.Y * 60 + 5);
      }
      else if (cell.Content == "o") {
        //draw circle.
      }
    });
  }

  function getCellTopLeftPosition(id) {
    const boxXId = id % 3;
    const boxYId = Math.floor(id / 3);
    const boxXpos = boxXId * 3 * cellSize;
    const boxYpos = boxYId * 3 * cellSize;
    return new Vector();
  }

  function drawX(x, y) {
    drawLine(x, y, x + 45, y + 45);
    drawLine(x + 45, y, x, y + 45);
  }

  function drawCircle(x, y, r1, r2) {
    context.beginPath();
    context.ellipse(x, y, r1, r2, 90 * Math.PI / 180, 0, 2 * Math.PI);
    context.lineWidth = 3;
    context.stroke();
  }

  function drawLine(x1, y1, x2, y2) {
    context.beginPath();    // Staring point (10,45).
    context.moveTo(x1, y1); // End point (180,47).
    context.lineTo(x2, y2); // Make the line visible.
    context.lineWidth = 3;
    context.stroke();
  }

  function draw() {
    console.log("Called");
    for (let i = 0; i < 9; i++) {
      draw3x3Box(i, 60);
    }

  }

  function drawCell(id, size) {
    const cellXId = id % 9;
    const cellYId = Math.floor(id / 9);
    const x = cellXId * size;
    const y = cellYId * size;
    context.strokeRect(x, y, size, size);
  }

  function draw3x3Box(id, cellSize) {
    const boxXId = id % 3;
    const boxYId = Math.floor(id / 3);
    const boxXpos = boxXId * 3 * cellSize;
    const boxYpos = boxYId * 3 * cellSize;
    const currentWidth = context.lineWidth;
    context.lineWidth = 6;
    context.strokeRect(boxXId * 3 * cellSize, boxYId * 3 * cellSize, 3 * cellSize, 3 * cellSize);
    context.lineWidth = currentWidth;
    for (let i = 0; i < 3; i++) {
      for (let j = 0; j < 3; j++) {
        console.log(`Drawing cell ${boxYId * 27 + i * 9 + j + boxXId * 3}`);
        drawCell(boxYId * 27 + i * 9 + j + boxXId * 3, cellSize);
      }
    }
  }


  // setup
  canvas.width = 540;
  canvas.height = 540;
  context.globalAlpha = 1.0;
  //draw();
}