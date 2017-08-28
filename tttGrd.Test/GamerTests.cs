using NUnit.Framework;
using System.Collections.Generic;

namespace tttGrd.Test
{
  [TestFixture]
  public class GamerTests
  {
    [Test]
    public void MakeMove_GivenNoHistoryGamerAndDefaultState_ExpectTupleZeroZero()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_1",
        GameState = new State(),
        Oponent = Field.O
      };

      //Act && Assert
      Assert.True(gamer.MakeMove().Equals((0, 0)));
    }

    [Test]
    public void MakeMove_GivenPreviousMoveToGridZeroCellZero_ExpectMoveInGridZeroAnywhereButCellZero()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "x..|...|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|...",
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((0, 0));
      var possibleCellIndices = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

      //Assert
      Assert.True(move.Item1 == 0);
      Assert.True(possibleCellIndices.Contains(move.Item2));
    }

    [Test]
    public void MakeMove_GivenPreviousMoveToGridZeroCellZeroWithGridWon_ExpectMoveAnywhereElseButGridZero()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "x.o|x..|xo.", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|...",
          "...|...|...", "...|...|...", "...|...|...",
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((0, 0));
      var possibleGridIndices = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
      var possibleCellIndices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

      //Assert
      Assert.True(possibleGridIndices.Contains(move.Item1));
      Assert.True(possibleCellIndices.Contains(move.Item2));
    }

    [Test]
    public void MakeMove_GivenPreviousMoveToGridNineCellNineWithEveryButGridFourWon_ExpectMoveInGridFour()
    {
      //MakeMove_GivenNextMoveInGridGamerIsAboutToWin_ExpectWinningOfGrid also passing by vertue.
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "xxx|o..|xoo", "xxx|o..|xoo", "xxx|o..|xoo",
          "xxx|o..|xoo", "x..|...|..o", "xxx|o..|xoo",
          "xxx|o..|xoo", "xxx|o..|xoo", "xxx|o..|xoo",
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((8, 8));
      var possibleCellIndices = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

      //Assert
      Assert.True(move.Item1 == 4);
      Assert.True(possibleCellIndices.Contains(move.Item2));
    }
  }
}