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

      var expected1 = GetDefaultCellsProbabilitiesForTest();
      Enumerable.Range(0, 9)
                .ToList()
                .ForEach(x => expected1[0][x] = 1f / 3f);

      expected1[0][4] = 0.0f; /* cell 4 in grid 0 cannot be played again. */

      var expected2 = expected1.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(1, 8)
                .ToList()
                .ForEach(x => expected2[x][0] -= 1f / 9f);  /* if you can help it, avoid sending the opponent back in grid 0 until you have played there yourself. */

      //Act
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gamer1.GameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gamer2.GameState, move, gamer2.Indicator);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }

    [Test]
    public void UpdateCellsProbabilities_GivenPreviousZeroZero_ExpectUpdate()
    {
      //Arrange
      /*
       the play was going as follow:
       - gamer1 made move (0,4)
       - gamer2 replied with move (4,4)
       */
      var gamer1 = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_1",
        GameState = new State(new[]{
          "...|.x.|...", "...|...|...", "...|...|...",
          "...|...|...", "...|.o.|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|..."
        }),
        Oponent = Field.O
      };
      gamer1.CellProbabilities = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gamer1.GameState, (0, 4), gamer1.Indicator);

      var gamer2 = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "...|.x.|...", "...|...|...", "...|...|...",
          "...|...|...", "...|.o.|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|..."
        }),
        Oponent = Field.X
      };
      gamer2.CellProbabilities = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gamer2.GameState, (0, 0), gamer2.Indicator);

      var move = (Grid: 4, Cell: 4);

      var expected1 = gamer1.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(1, 9)
                .ToList()
                .ForEach(x => expected1[x][0] -= 1f / 9f);  /* if you can help it, avoid sending the opponent back in grid 0 until you have played there yourself. */
      expected1[4][4] = 0.0f; /* middle one should not be taken again. */

      var expected2 = GetDefaultCellsProbabilitiesForTest();
      Enumerable.Range(0, 9)
                .ToList()
                .ForEach(x => expected2[4][x] += 2f / 9f);
      expected2[4][4] = 0.0f; /* middle one should not be taken again. */
      
      //Act
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gamer1.GameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gamer2.GameState, move, gamer2.Indicator);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }

    private static float[][] GetDefaultCellsProbabilitiesForTest()
    {
      return new[]
      {
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f}
      };
    }
  }
}