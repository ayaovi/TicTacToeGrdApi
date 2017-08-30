var btns = [];

function setup() {
	createCanvas(400, 400);
	
	var side = 30;
	
	for (var i = 0; i < 9; i++) {
		var grid = [];
		for (var j = 0; j < 9; j++) {
			grid.push(new btn(createVector((i % 3) * 100 + (j % 3) * 35, floor(i / 3) * 100 + floor(j / 3) * 35), side, side, i, j));
		}
		btns.push(grid);
	}
}

function mousePressed() {
	btns.forEach(b => b.forEach(b1 => {
		if (b1.position.x <= mouseX && (b1.position.x + b1.width) > mouseX && 
		b1.position.y <= mouseY && (b1.position.y + b1.height) > mouseY) b1.colour = 100;
	}));
}

function btn(position, width, height, grid, cell) {
	this.position = position;
	this.width = width;
	this.height = height;
	this.grid = grid;
	this.cell = cell;
	this.colour = 50;
	
	this.show = function() {
		fill(this.colour);
		rect(this.position.x, this.position.y, this.width, this.height);
	}
}

function draw() {
	background(200);
	btns.forEach(b => b.forEach(b1 => b1.show()));
}