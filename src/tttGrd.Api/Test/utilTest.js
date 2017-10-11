QUnit.test("Field To Indicator Given 0", function (assert) {
  var util = new Util();
  var indicator = util.fieldToIndicator(0);
  assert.equal(indicator, ".", "should return \".\"");
});

QUnit.test("Field To Indicator Given 1", function (assert) {
  var util = new Util();
  var indicator = util.fieldToIndicator(1);
  assert.equal(indicator, "x", "should return \"x\"");
});

QUnit.test("Field To Indicator Given 2", function (assert) {
  var util = new Util();
  var indicator = util.fieldToIndicator(2);
  assert.equal(indicator, "o", "should return \"o\"");
});

QUnit.test("Indicator To Field Given \".\"", function (assert) {
  var util = new Util();
  var field = util.indicatorTofield(".");
  assert.equal(field, 0, "should return 0");
});

QUnit.test("Indicator To Field Given \"x\"", function (assert) {
  var util = new Util();
  var field = util.indicatorTofield("x");
  assert.equal(field, 1, "should return 1");
});

QUnit.test("Indicator To Field Given \"o\"", function (assert) {
  var util = new Util();
  var field = util.indicatorTofield("o");
  assert.equal(field, 2, "should return 2");
});

QUnit.test("State Compare Given \"undefined\" Old State ", function (assert) {
  var util = new Util();
  var oldState = undefined;
  var newState = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
  var move = util.compareStates(oldState, newState);
  assert.equal(move.Grid, 1, "Grid component of move equal 1");
  assert.equal(move.Cell, 1, "Cell component of move equal 1");
});

QUnit.test("State Compare Given \"non-undefined\" Old State", function (assert) {
  var util = new Util();
  var oldState = [[0, 0, 0], [0, 1, 0], [1, 0, 0]];
  var newState = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
  var move = util.compareStates(oldState, newState);
  assert.equal(move.Grid, 2, "Grid component of move equal 2");
  assert.equal(move.Cell, 0, "Cell component of move equal 0");
});

QUnit.test("Extract Move Given 0", function (assert) {
  var util = new Util();
  var move = util.extractMove(0);
  assert.equal(move[1], 0, "should result in Grid 0");
  assert.equal(move[2], 0, "should result in Cell 0");
});

QUnit.test("Extract Move Given 20", function (assert) {
  var util = new Util();
  var move = util.extractMove(20);
  assert.equal(move[1], 0, "should result in Grid 0");
  assert.equal(move[2], 8, "should result in Cell 8");
});