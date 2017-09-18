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
       * - x makes move (0,4)
       * - What would be their respective cell probabilities.
       **/
      var gameState = new State();
      var probs = Utilities.GetDefaultCellsProbabilities();

      var move = (0, 4);
      gameState.Fields[0][4] = Field.X;

      var expected = Utilities.GetDefaultCellsProbabilities();
      Enumerable.Range(0, 9)
                .ToList()
                .ForEach(x =>
                {
                  if (x != 0) expected[x][0] -= 1f / 9f;
                  expected[0][x] = 1f / 3f;
                });

      expected[0][4] = 0.0f; /* cell 4 in grid 0 cannot be played again. */
      
      //Act
      var cells = Program.UpdateCellsProbabilities(probs, gameState, move);

      //Assert
      cells.ShouldAllBeEquivalentTo(expected);
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
      var prob = Utilities.GetCellsProbabilities(new[] { new Move { Value = (0, 4), Indicator = Field.X } }, gameState);
      
      var move = (4, 4);

      var expected = prob.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(0, 9)
                .ToList()
                .ForEach(x =>
                {
                  if (x != 0) expected[x][4] -= 1f / 9f;
                  expected[4][x] += 2f / 9f;
                });  /* increasing my so I can stop the oponent when I get a chance to play in grid 4. */
      expected[4][4] = 0.0f; /* middle one should not be taken again. */
      
      //Act
      var cells = Program.UpdateCellsProbabilities(prob, gameState, move);

      //Assert
      cells.ShouldAllBeEquivalentTo(expected);
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

      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (0, 4), Indicator = Field.X }, new Move { Value = (4, 4), Indicator = Field.O }
      }, gameState);

      var move = (4, 8);

      var expected = prob.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      Enumerable.Range(0, 9)
                .Where(x => expected[x][4] > 0)
                .ToList()
                .ForEach(x => expected[x][4] -= 1f / 9f);
      expected[4][3] += 2f / 9f; /* for me and the oponent. */
      expected[4][5] += 2f / 9f; /* for me and the oponent. */
      expected[4][6] += 2f / 9f; /* for me and the oponent. */
      expected[4][7] += 2f / 9f; /* for me and the oponent. */
      expected[4][8] = 0.0f; /* middle one should not be taken again. */

      //Act
      var cells = Program.UpdateCellsProbabilities(prob, gameState, move);

      //Assert
      cells.ShouldAllBeEquivalentTo(expected);
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

      var prob = Utilities.GetCellsProbabilities(new[]
                                {
                                  new Move { Value = (0, 4), Indicator = Field.X },
                                  new Move { Value = (4, 4), Indicator = Field.O },
                                  new Move { Value = (4, 8), Indicator = Field.X }
                                }, gameState);
      
      var move = (8, 1);

      var expected = prob.Copy(x => x.Select(x1 => x1).ToArray()).ToArray();
      /* because oponent made move (8,1), we need to block cells {c_0, c_2, c_4, c_7} in g_8. */
      expected[8][0] += 2f / 9f;
      expected[8][2] += 2f / 9f;
      expected[8][4] += 2f / 9f;
      expected[8][7] += 2f / 9f;
      /* furthermore, I need to discourage sending oponent to g_8 (i.e. update c_8 in all grids) */
      Enumerable.Range(0, 8)
                .Where(x => expected[x][8] > 0f)
                .ToList()
                .ForEach(x => expected[x][8] -= 1f / 9f);
      /* finally move (8,1) can no longer be made. */
      expected[8][1] = 0.0f;

      //Act
      var cells = Program.UpdateCellsProbabilities(prob, gameState, move);
      
      //Assert
      cells.ShouldAllBeEquivalentTo(expected);
    }
  }
}