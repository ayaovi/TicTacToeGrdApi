using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd.Test
{
  [TestFixture]
  public class GamerTests
  {
    [Test]
    public void MakeMove_GivenNoGamerHistoryAndDefaultState_ExpectRandomMoveWithingConstraint()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_1",
        GameState = new State(),
        Oponent = Field.O
      };

      var indices = Enumerable.Range(0, 9).ToList();

      //Act 
      var move = gamer.MakeMove();

      //Assert
      Assert.True(indices.Contains(move.Grid));
      Assert.True(indices.Contains(move.Cell));
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
    public void MakeMove_GivenPreviousMove_ShouldTryAvoidSendingOpponentToAlreadyWonGridIfPossible()
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
    public void MakeMove_GivenPreviousMove_ShouldOnlySendOpponentToAlreadyWonGridIfNoWayToAvoidIt()
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
    public void MakeMove_GivenOpportunityForOpponentToWinGrid_ShouldStopOpponent()
    {
      //Arrange
      var gameState = new State();
      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (0, 0), Indicator = Field.X },
        new Move { Value = (0, 8), Indicator = Field.O },
        new Move { Value = (8, 4), Indicator = Field.X },
        new Move { Value = (4, 5), Indicator = Field.O },
        new Move { Value = (5, 6), Indicator = Field.X },
        new Move { Value = (6, 0), Indicator = Field.O },
        new Move { Value = (0, 2), Indicator = Field.X },
        new Move { Value = (2, 1), Indicator = Field.O },
        new Move { Value = (1, 8), Indicator = Field.X },
        new Move { Value = (8, 0), Indicator = Field.O },
      }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        GameState = gameState,
        CellProbabilities = prob,
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((8, 0));

      //Assert
      Assert.AreEqual(0, move.Grid);
      Assert.AreEqual(1, move.Cell);
    }

    [Test]
    public void MakeMove_GivenOpportunity_ShouldNotSendOpponentWhereHeIsAboutToWin_Prob()
    {
      //Arrange
      var gameState = new State();
      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (0, 1), Indicator = Field.X },
        new Move { Value = (1, 2), Indicator = Field.O },
        new Move { Value = (2, 4), Indicator = Field.X },
        new Move { Value = (4, 3), Indicator = Field.O },
        new Move { Value = (3, 1), Indicator = Field.X },
        new Move { Value = (1, 4), Indicator = Field.O },
        new Move { Value = (4, 4), Indicator = Field.X },
        new Move { Value = (4, 6), Indicator = Field.O },
        new Move { Value = (6, 8), Indicator = Field.X },
        new Move { Value = (8, 0), Indicator = Field.O },
      }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = gameState,
        CellProbabilities = prob,
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((8, 0));
      var possibleCellIndices = new List<int> { 0, 2, 3, 5, 6, 7, 8 };

      //Assert
      Assert.AreEqual(0, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenOpportunity_ShouldMaximiseMyWinningChances_Prob()
    {
      //Arrange
      var gameState = new State();
      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (0, 4), Indicator = Field.X },
        new Move { Value = (4, 4), Indicator = Field.O },
        new Move { Value = (4, 1), Indicator = Field.X },
        new Move { Value = (1, 7), Indicator = Field.O },
        new Move { Value = (7, 3), Indicator = Field.X },
        new Move { Value = (3, 4), Indicator = Field.O }
      }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = gameState,
        CellProbabilities = prob,
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((3, 4));
      var possibleCellIndices = new List<int> { 0, 2 };

      //Assert
      Assert.AreEqual(4, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenOpportunity_ShouldAvoidSendingOpponentWhereHeHasHigherChanceOfWinning_Prob()
    {
      //Arrange
      var gameState = new State();
      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (0, 0), Indicator = Field.X },
        new Move { Value = (0, 4), Indicator = Field.O },
        new Move { Value = (4, 1), Indicator = Field.X },
        new Move { Value = (1, 4), Indicator = Field.O },
        new Move { Value = (4, 0), Indicator = Field.X },
        new Move { Value = (0, 2), Indicator = Field.O }
      }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = gameState,
        CellProbabilities = prob,
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((0, 2));
      var possibleCellIndices = new List<int> { 2, 3, 4, 5, 6, 7, 8 };

      //Assert
      Assert.AreEqual(2, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenOpportunity_ShouldAvoidSendingOpponentWhereMyWinIsEminent_Prob()
    {
      //Arrange
      var gameState = new State();
      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (4, 0), Indicator = Field.X },
        new Move { Value = (4, 1), Indicator = Field.X },
        new Move { Value = (0, 2), Indicator = Field.O }
      }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = gameState,
        CellProbabilities = prob,
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((0, 2));
      var possibleCellIndices = new List<int> { 2, 3, 5, 6, 7, 8 };

      //Assert
      Assert.AreEqual(2, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenPlay_ShouldNotDirectOpponentToGridFour_Prob()
    {
      //Arrange
      var gameState = new State();
      var prob = Utilities.GetCellsProbabilities(new[]
      {
        new Move { Value = (4, 6), Indicator = Field.X },
        new Move { Value = (4, 7), Indicator = Field.X },
        new Move { Value = (4, 8), Indicator = Field.X }
      }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.O,
        Name = "Gamer_2",
        CellProbabilities = prob,
        GameState = gameState,
        Oponent = Field.X
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((4, 8));
      var possibleCellIndices = new List<int> { 0, 1, 2, 3, 5, 6, 7, 8 };

      //Assert
      Assert.IsTrue(move.Grid == 8);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenPlay_ShouldNotDirectOpponentToGridZero_Prob()
    {
      //Arrange
      var gameState = new State(new[]
      {
        "...|.o.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });
      var prob = Utilities.GetCellsProbabilities(new[] { new Move { Value = (0, 4), Indicator = Field.O } }, gameState);
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = gameState,
        CellProbabilities = prob,
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeProbabilityBasedMove((0, 4));
      var possibleCellIndices = new List<int> { 1, 2, 3, 5, 6, 7, 8 };

      //Assert
      Assert.AreEqual(4, move.Grid);
      Assert.IsTrue(possibleCellIndices.Contains(move.Cell));
    }

    [Test]
    public void MakeMove_GivenOpportunityToPlayInEmptyGrid_ShouldPreferablyGoCenter()
    {
      //Arrange
      var gamer = new Gamer
      {
        Indicator = Field.X,
        Name = "Gamer_2",
        GameState = new State(),
        Oponent = Field.O
      };

      //Act
      var move = gamer.MakeMove();
      var possibleGridIndices = Enumerable.Range(0, 9).ToList();

      //Assert
      Assert.IsTrue(possibleGridIndices.Contains(move.Grid));
      Assert.AreEqual(4, move.Cell);
    }
  }
}