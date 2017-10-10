QUnit.test("State Compare with Old State Undefined", function (assert) {
  var util = new Util();
  var oldState = undefined;
  var newState = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
  var move = util.compareStates(oldState, newState);
  assert.equal(move.Grid, 1, "Grid component of move equal 1");
  assert.equal(move.Cell, 1, "Cell component of move equal 1");
});

QUnit.test("State Compare with proper Old State", function (assert) {
  var util = new Util();
  var oldState = [[0, 0, 0], [0, 1, 0], [1, 0, 0]];
  var newState = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
  var move = util.compareStates(oldState, newState);
  assert.equal(move.Grid, 2, "Grid component of move equal 2");
  assert.equal(move.Cell, 0, "Cell component of move equal 0");
});