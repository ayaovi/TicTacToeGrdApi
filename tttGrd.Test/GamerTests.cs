using NUnit.Framework;
using System.Collections.Generic;

namespace tttGrd.Test
{
  [TestFixture]
  public class GamerTests
  {
    [Test]
    public void MakeMove_GivenNoGamerHistoryAndDefaultState_ExpectTupleZeroZero()
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
          "...|...|...", "...|...|...", "...|...|..."
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((0, 0));
      var possibleCellIndices = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

      //Assert
      Assert.True(move.Grid == 0);
      Assert.True(possibleCellIndices.Contains(move.Cell));
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
      Assert.True(possibleGridIndices.Contains(move.Grid));
      Assert.True(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenPreviousMoveToGridNineCellNineWithEveryButGridFiveWon_ExpectMoveInGridFive()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "xxx|o..|xoo", "xxx|o..|xoo", "xxx|o..|xoo",
          "xxx|o..|xoo", "x..|...|..o", "xxx|o..|xoo",
          "xxx|o..|xoo", "xxx|o..|xoo", "xxx|o..|xoo"
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((8, 8));
      var possibleCellIndices = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

      //Assert
      Assert.True(move.Grid == 4);
      Assert.True(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenPreviousMove_ShouldTryAvoidSendingOponentToAlreadyWonGridIfPossible()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "xxx|o..|xoo", "xxx|o..|xoo", "xxo|o..|xox",
          "xxx|o..|xoo", "x..|...|..o", "xxx|o..|xoo",
          "xxx|o..|xoo", "xxx|o..|xoo", "xxo|o..|oox"
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((2, 8));

      //Assert
      Assert.AreEqual(8, move.Grid);
      Assert.AreEqual(4, move.Cell);
    }

    [Test]
    public void MakeMove_GivenPreviousMove_ShouldSendOponentToAlreadyWonGridIfNoWayToAvoidIt()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "xxx|o..|xoo", "xxx|o..|xoo", "xxo|o..|xox",
          "xxx|o..|xoo", "xxx|o..|xoo", "xxx|o..|xoo",
          "xxx|o..|xoo", "xxx|o..|xoo", "xxo|o..|xoo"
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((2, 8));
      var possibleCellIndices = new List<int> { 4, 5 };

      //Assert
      Assert.AreEqual(8, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenOportunityToWinGrid_ShouldWin()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "x.x|...|..o", "...|...|..x", ".o.|...|...",
          "...|...|x..", "...|..o|...", "...|...|x..",
          "o..|o..|...", "...|...|...", "...|.x.|..."
        }),
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeMove((8, 0));

      //Assert
      Assert.AreEqual(0, move.Grid);
      Assert.AreEqual(1, move.Cell);
    }

    [Test]
    public void MakeMove_GivenOpportunityForOponentToWinGrid_ShouldStopOponent()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "x.x|...|..o", "...|...|..x", ".o.|...|...",
          "...|...|x..", "...|..o|...", "...|...|x..",
          "o..|o..|...", "...|...|...", "...|.x.|..."
        }),
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeMove((8, 0));

      //Assert
      Assert.AreEqual(0, move.Grid);
      Assert.AreEqual(1, move.Cell);
    }

    [Test]
    public void MakeMove_GivenOpportunity_ShouldNotSendOponentWhereHeIsAboutToWin()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = new State(new[]{
          ".x.|...|...", "..o|.o.|...", "...|.x.|...",
          ".x.|...|...", "...|ox.|o..", "...|...|...",
          "...|...|..x", "...|...|...", "o..|...|..."
        }),
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeMove((8, 0));
      var possibleCellIndices = new List<int> { 0, 2, 3, 5, 6, 7, 8 };

      //Assert
      Assert.AreEqual(0, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenOpportunity_ShouldMaximiseMyWinningChances()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = new State(new[]{
          "...|.x.|...", "...|...|.o.", "...|x..|...",
          "o..|.o.|...", ".x.|.o.|...", "..o|...|...",
          "...|...|...", "...|x..|...", "...|...|..."
        }),
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeMove((3, 4));
      var possibleCellIndices = new List<int> { 0, 2 };

      //Assert
      Assert.AreEqual(4, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }
  }
}