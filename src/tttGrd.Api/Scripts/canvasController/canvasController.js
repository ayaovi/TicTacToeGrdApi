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

    console.log(`xx: ${x} yy: ${y}`)
    //drawX(cellTopLeftX * 60 + 5, cellTopLeftY * 60 + 5);
    drawCircle(cellTopLeftX + 30, cellTopLeftY + 30, 20, 18);
    console.log(`x: ${Math.floor(x / 60)} and y: ${Math.floor(y / 60)}`)
    const pos = Math.floor(x / 60) + Math.floor(y / 60) * 9;
    console.log(pos);
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

  function drawCell(x, y, size) {
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
        drawCell(boxXpos + i * cellSize, boxYpos + j * cellSize, cellSize);
      }
    }
  }


  // setup
  canvas.width = 540;
  canvas.height = 540;
  context.globalAlpha = 1.0;
  draw();
}