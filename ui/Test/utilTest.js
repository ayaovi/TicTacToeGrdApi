QUnit.test("Field To Indicator Given 0", function (assert) {
  const util = new Util();
  const indicator = util.fieldToIndicator(0);
  assert.equal(indicator, ".", "should return \".\".");
});

QUnit.test("Field To Indicator Given 1", function (assert) {
  const util = new Util();
  const indicator = util.fieldToIndicator(1);
  assert.equal(indicator, "x", "should return \"x\".");
});

QUnit.test("Field To Indicator Given 2", function (assert) {
  const util = new Util();
  const indicator = util.fieldToIndicator(2);
  assert.equal(indicator, "o", "should return \"o\".");
});

QUnit.test("Indicator To Field Given \".\"", function (assert) {
  const util = new Util();
  const field = util.indicatorTofield(".");
  assert.equal(field, 0, "should return 0.");
});

QUnit.test("Indicator To Field Given \"x\"", function (assert) {
  const util = new Util();
  const field = util.indicatorTofield("x");
  assert.equal(field, 1, "should return 1.");
});

QUnit.test("Indicator To Field Given \"o\"", function (assert) {
  const util = new Util();
  const field = util.indicatorTofield("o");
  assert.equal(field, 2, "should return 2.");
});

QUnit.test("State Compare Given \"undefined\" Old State ", function (assert) {
  const util = new Util();
  const oldState = undefined;
  const newState = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
  const move = util.compareStates(oldState, newState);
  assert.equal(move.Grid, 1, "Grid component of move equal 1.");
  assert.equal(move.Cell, 1, "Cell component of move equal 1.");
});

QUnit.test("State Compare Given \"non-undefined\" Old State", function (assert) {
  const util = new Util();
  const oldState = [[0, 0, 0], [0, 1, 0], [1, 0, 0]];
  const newState = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
  const move = util.compareStates(oldState, newState);
  assert.equal(move.Grid, 2, "Grid component of move equal 2.");
  assert.equal(move.Cell, 0, "Cell component of move equal 0.");
});

QUnit.test("CellIdToMove Given 0", function (assert) {
  const util = new Util();
  const move = util.cellIdToMove(0);
  assert.equal(move.Grid, 0, "should result in Grid 0.");
  assert.equal(move.Cell, 0, "should result in Cell 0.");
});

QUnit.test("CellIdToMove Given 20", function (assert) {
  const util = new Util();
  const move = util.cellIdToMove(20);
  assert.equal(move.Grid, 0, "should result in Grid 0.");
  assert.equal(move.Cell, 8, "should result in Cell 8.");
});

QUnit.test("MoveToCellId Given Move (0, 0)", function (assert) {
  const util = new Util();
  const cellId = util.moveToCellId(new Move(0, 0, "."));
  assert.equal(cellId, 0, "should return 0.");
});

QUnit.test("MoveToCellId Given Move (0, 2)", function (assert) {
  const util = new Util();
  const cellId = util.moveToCellId(new Move(0, 2, "."));
  assert.equal(cellId, 2, "should return 2.");
});

QUnit.test("MoveToCellId Given Move (4, 1)", function (assert) {
  const util = new Util();
  const cellId = util.moveToCellId(new Move(4, 1, "."));
  assert.equal(cellId, 31, "should return 31.");
});

QUnit.test("MoveToCellId Given Move (8, 8)", function (assert) {
  const util = new Util();
  const cellId = util.moveToCellId(new Move(8, 8, "."));
  assert.equal(cellId, 80, "should return 80.");
});

QUnit.test("Have Same Content, Given Different size lists", function (assert) {
  const util = new Util();
  const list1 = [0];
  const list2 = [0, 1];
  const result = util.haveSameContent(list1, list2);
  assert.notOk(result, "should return false.");
});

QUnit.test("Have Same Content, Given string and integer lists", function (assert) {
  const util = new Util();
  const list1 = ["."];
  const list2 = [0, 1];
  const result = util.haveSameContent(list1, list2);
  assert.notOk(result, "should return false.");
});

QUnit.test("Have Same Content, Given same size and content lists", function (assert) {
  const util = new Util();
  const list1 = [0, 1];
  const list2 = [0, 1];
  const result = util.haveSameContent(list1, list2);
  assert.ok(result, "should return false.");
});

QUnit.test("Get Enabled Cell Given Move (0, 0)", function (assert) {
  const util = new Util();
  const expected = [0, 1, 2, 9, 10, 11, 18, 19, 20];
  const cells = util.getEnabledCells(new Move(0, 0, "."));
  assert.ok(util.haveSameContent(cells, expected), "should return Grid 0.");
});

QUnit.test("Get Enabled Cell Given Move (0, 1)", function (assert) {
  const util = new Util();
  const expected = [3, 4, 5, 12, 13, 14, 21, 22, 23];
  const cells = util.getEnabledCells(new Move(0, 1, "."));
  assert.ok(util.haveSameContent(cells, expected), "should return Grid 1.");
});

QUnit.test("Get Enabled Cell Given Move (0, 2)", function (assert) {
  const util = new Util();
  const expected = [6, 7, 8, 15, 16, 17, 24, 25, 26];
  const cells = util.getEnabledCells(new Move(0, 2, "."));
  assert.ok(util.haveSameContent(cells, expected), "should return Grid 2.");
});
