const cellSize = 50;

var Cell = ng.Class({
    constructor: function (x, y, value = 0) {
        this.x = x;
        this.y = y;
        this.value = value;
    },

    draw: function (context) {
        context.strokeRect(this.x * cellSize, this.y * cellSize, cellSize, cellSize);
    }
});