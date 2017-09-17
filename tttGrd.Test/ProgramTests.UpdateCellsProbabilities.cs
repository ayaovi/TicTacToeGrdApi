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
       * - gamer1 makes move (0,4)
       * - What would be their respective cell probabilities.
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

      var expected1 = Utilities.GetDefaultCellsProbabilities();
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
    public void UpdateCellsProbabilities_GivenPreviousStateAndOpenentMove_ExpectUpdate()
    {
      //Arrange
      /**
       * the play goes as follow:
       * - gamer1 made move (0,4)
       * - gamer2 replied with move (4,4)
       **/

      var gameState = new State(new[]
      {
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|.o.|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });
      (var prob1, var prob2) = GetCellsProbabilities(new[] { new Move { Value = (0, 4), Indicator = Field.X } }, gameState);
      
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

    [Test]
    public void UpdateCellsProbabilities_GivenPreviousStateAndOpenentMove1_ExpectUpdate()
    {
      //Arrange
      /**
       * the play goes as follow:
       * - gamer1 moves (0,4)
       * - gamer2 moves (4,4)
       * - gamer1 moves (4,8)
       **/
      var gameState = new State(new[]
      {
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|.o.|..x", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });

      (var prob1, var prob2) = GetCellsProbabilities(new[] { new Move { Value = (0, 4), Indicator = Field.X }, new Move { Value = (4, 4), Indicator = Field.O } }, gameState);
      
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

      var move = (4, 8);

      var expected1 = gamer1.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      expected1[4][3] += 2f / 9f; /* for me and the oponent. */
      expected1[4][5] += 2f / 9f; /* for me and the oponent. */
      expected1[4][6] += 2f / 9f; /* for me and the oponent. */
      expected1[4][7] += 2f / 9f; /* for me and the oponent. */
      expected1[4][8] = 0.0f; /* middle one should not be taken again. */

      var expected2 = gamer2.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(0, 9)
                .Where(x => expected2[x][4] > 0)
                .ToList()
                .ForEach(x => expected2[x][4] -= 1f / 9f);
      expected2[4][3] += 2f / 9f; /* for me and the oponent. */
      expected2[4][5] += 2f / 9f; /* for me and the oponent. */
      expected2[4][6] += 2f / 9f; /* for me and the oponent. */
      expected2[4][7] += 2f / 9f; /* for me and the oponent. */
      expected2[4][8] = 0.0f; /* middle one should not be taken again. */

      //Act
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gameState, move, gamer2.Indicator);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }

    [Test]
    public void UpdateCellsProbabilities_GivenPreviousStateAndOpenentMove2_ExpectUpdate()
    {
      //Arrange
      /**
       * the play goes as follow:
       * - gamer1 moves (0,4)
       * - gamer2 moves (4,4)
       * - gamer1 moves (4,8)
       * - gamer1 moves (8,1)
       **/
      var gameState = new State(new[]
      {
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|.o.|..x", "...|...|...",
        "...|...|...", "...|...|...", ".o.|...|..."
      });

      (var prob1, var prob2) = GetCellsProbabilities(new[]
                                {
                                  new Move { Value = (0, 4), Indicator = Field.X },
                                  new Move { Value = (4, 4), Indicator = Field.O },
                                  new Move { Value = (4, 8), Indicator = Field.X }
                                }, gameState);

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

      var move = (8, 1);

      var expected1 = gamer1.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      /* because oponent made move (8,1), we need to block cells {c_0, c_2, c_4, c_7} in g_8. */
      expected1[8][0] += 2f / 9f;
      expected1[8][2] += 2f / 9f;
      expected1[8][4] += 2f / 9f;
      expected1[8][7] += 2f / 9f;
      /* furthermore, I need to discourage sending oponent to g_8 (i.e. update c_8 in all grids) */
      Enumerable.Range(0, 8)
                .Where(x => expected1[x][8] > 0f)
                .ToList()
                .ForEach(x => expected1[x][8] -= 1f / 9f);
      /* finally move (8,1) can no longer be made. */
      expected1[8][1] = 0.0f;

      var expected2 = gamer2.CellProbabilities.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      /* because I made move (8,1), I need to further pick some of {c_0, c_2, c_4, c_7} in g_8 in order to win the grid. */
      expected2[8][0] += 2f / 9f;
      expected2[8][2] += 2f / 9f;
      expected2[8][4] += 2f / 9f;
      expected2[8][7] += 2f / 9f;
      /* furthermore, I need to discourage sending oponent to g_8 (i.e. update c_8 in all grids) otherwise (s)he might spoil my party. */
      //Enumerable.Range(0, 8)
      //          .Where(x => expected2[x][8] > 0f)
      //          .ToList()
      //          .ForEach(x => expected2[x][8] -= 1f / 9f);
      /* finally move (8,1) can no longer be made. */
      expected2[8][1] = 0.0f;

      //Act
      var cells1 = Program.UpdateCellsProbabilities(gamer1.CellProbabilities, gameState, move, gamer1.Indicator);
      var cells2 = Program.UpdateCellsProbabilities(gamer2.CellProbabilities, gameState, move, gamer2.Indicator);

      //var v1 = Flatten(expected2);
      //var v2 = Flatten(cells2);

      //Assert
      cells1.ShouldAllBeEquivalentTo(expected1);
      cells2.ShouldAllBeEquivalentTo(expected2);
    }

    private static string Flatten(IEnumerable<float[]> f) => string.Join("\n", f.Select(f1 => string.Join(", ", f1)));

    private static (float[][], float[][]) GetCellsProbabilities(IEnumerable<Move> moves, State state)
    {
      var prob1 = Utilities.GetDefaultCellsProbabilities();
      var prob2 = Utilities.GetDefaultCellsProbabilities();

      moves.ToList().ForEach(move => 
      {
        var oponent = move.Indicator == Field.O ? Field.X : Field.O;
        prob1 = Program.UpdateCellsProbabilities(prob1, state, move.Value, move.Indicator);
        prob2 = Program.UpdateCellsProbabilities(prob2, state, move.Value, oponent);
      });
      return (prob1, prob2);
    }
  }

  public class Move
  {
    public (int Grid, int Cell) Value { get; set; }
    public Field Indicator { get; set; }
  }
}