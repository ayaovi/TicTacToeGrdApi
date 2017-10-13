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
  var move = util.cellIdToMove(0);
  assert.equal(move.Grid, 0, "should result in Grid 0");
  assert.equal(move.Cell, 0, "should result in Cell 0");
});

QUnit.test("Extract Move Given 20", function (assert) {
  var util = new Util();
  var move = util.cellIdToMove(20);
  assert.equal(move.Grid, 0, "should result in Grid 0");
  assert.equal(move.Cell, 8, "should result in Cell 8");
});


QUnit.test("Have Same Content, Given Different size lists", function (assert) {
  var util = new Util();
  var list1 = [0];
  var list2 = [0, 1];
  var result = util.haveSameContent(list1, list2);
  assert.notOk(result, "should return false");
});

QUnit.test("Have Same Content, Given string and integer lists", function (assert) {
  var util = new Util();
  var list1 = ["."];
  var list2 = [0, 1];
  var result = util.haveSameContent(list1, list2);
  assert.notOk(result, "should return false");
});

QUnit.test("Have Same Content, Given same size and content lists", function (assert) {
  var util = new Util();
  var list1 = [0, 1];
  var list2 = [0, 1];
  var result = util.haveSameContent(list1, list2);
  assert.ok(result, "should return false");
});

QUnit.test("Get Enabled Cell Given Move (0, 0)", function (assert) {
  var util = new Util();
  var expected = [0, 1, 2, 9, 10, 11, 18, 19, 20];
  var cells = util.getEnabledCells(new Move(0, 0, "."));
  assert.ok(util.haveSameContent(cells, expected), "should return Grid 0");
});

QUnit.test("Get Enabled Cell Given Move (0, 1)", function (assert) {
  var util = new Util();
  var expected = [3, 4, 5, 12, 13, 14, 21, 22, 23];
  var cells = util.getEnabledCells(new Move(0, 1, "."));
  assert.ok(util.haveSameContent(cells, expected), "should return Grid 1");
});

QUnit.test("Get Enabled Cell Given Move (0, 2)", function (assert) {
  var util = new Util();
  var expected = [6, 7, 8, 15, 16, 17, 24, 25, 26];
  var cells = util.getEnabledCells(new Move(0, 2, "."));
  assert.ok(util.haveSameContent(cells, expected), "should return Grid 2");
});