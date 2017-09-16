using System.Collections.Generic;
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
      /** 
       * the goes as follow:
       * gamer1 makes move (0,4)
       * What would be their respective cell probabilities.
       **/
      var gameState = new State(new[]
      {
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });

      var gamer1 = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_1",
        Oponent = Field.O
      };

      var gamer2 = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        Oponent = Field.X
      };

      var move = (0, 4);

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
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gameState, move, gamer2.Indicator);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }

    [Test]
    public void UpdateCellsProbabilities_GivenPreviousZeroZero_ExpectUpdate()
    {
      //Arrange
      /**
       * the play goes as follow:
       * - gamer1 made move (0,4)
       * - gamer2 replied with move (4,4)
       **/
      (var prob1, var prob2) = GetCellsProbabilities(new[] { new Move { Value = (0, 4), Indicator = Field.X } });

      var gameState = new State(new[]
      {
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|.o.|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });

      var gamer1 = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_1",
        CellProbabilities = prob1,
        Oponent = Field.O
      };

      var gamer2 = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        CellProbabilities = prob2,
        Oponent = Field.X
      };

      var move = (4, 4);

      var expected1 = gamer1.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(0, 9)
                .ToList()
                .ForEach(x => expected1[4][x] += 2f / 9f);  /* increasing my so I can stop the oponent when I get a chance to play in grid 4. */
      Enumerable.Range(1, 8)
                .ToList()
                .ForEach(x => expected1[x][4] -= 1f / 9f);  /* if you can help it, avoid sending the opponent back in grid 0 until you have played there yourself. */
      expected1[4][4] = 0.0f; /* middle one should not be taken again. */

      var expected2 = gamer2.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(0, 9)
                .ToList()
                .ForEach(x => expected2[4][x] += 2f / 9f);
      expected2[4][4] = 0.0f; /* middle one should not be taken again. */

      //Act
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gameState, move, gamer2.Indicator);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }

    private static (float[][], float[][]) GetCellsProbabilities(IEnumerable<Move> moves)
    {
      var prob1 = GetDefaultCellsProbabilitiesForTest();
      var prob2 = GetDefaultCellsProbabilitiesForTest();
      var state = new State();

      moves.ToList().ForEach(move => 
      {
        state.Fields[move.Value.Grid][move.Value.Cell] = move.Indicator;
        var oponent = move.Indicator == Field.O ? Field.X : Field.O;
        prob1 = Program.UpdateCellsProbabilities(prob1, state, move.Value, move.Indicator);
        prob2 = Program.UpdateCellsProbabilities(prob2, state, move.Value, oponent);
      });
      return (prob1, prob2);
    }

    private static float[][] GetDefaultCellsProbabilitiesForTest()
    {
      return new[]
      {
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 0f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f}
      };
    }
  }

  public class Move
  {
    public (int Grid, int Cell) Value { get; set; }
    public Field Indicator { get; set; }
  }
}