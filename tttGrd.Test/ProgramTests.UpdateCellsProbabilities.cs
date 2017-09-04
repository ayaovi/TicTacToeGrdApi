using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace tttGrd.Test
{
  [TestFixture]
  internal class ProgramTests
  {
    [Test]
    public void UpdateCellsProbabilities_GivenDefaultStateAndOpponentMove_ExpectUpdate()
    {
      //Arrange
      var gamer1 = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_1",
        GameState = new State(new[]{
          "...|.x.|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|..."
        }),
        Oponent = Field.O
      };

      var gamer2 = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "...|.x.|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|..."
        }),
        Oponent = Field.X
      };

      var move = (Grid: 0, Cell: 4);

      var expected1 = new[]
      {
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f },
        new[] { 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f }
      };
      Enumerable.Range(0, 9)
        .ToList()
        .ForEach(x => expected1[0][x] = 1f / 3f);

      expected1[0][4] = 0.0f; /* middle one should not be taken again. */

      var expected2 = expected1.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(1, 8)
        .ToList()
        .ForEach(x => expected2[x][0] -= 1f / 9f);

      //Act
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gamer1.GameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gamer2.GameState, move, gamer2.Indicator);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }
  }
}