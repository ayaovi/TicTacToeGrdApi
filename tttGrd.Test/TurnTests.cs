using NUnit.Framework;

namespace tttGrd.Test
{
  [TestFixture]
  class TurnTests
  {
    [Test]
    public void Equals_GivenIdenticalTurns_ExpectTrue()
    {
      //Arrange
      var turn1 = new Turn { GameState = new State() };
      var turn2 = new Turn { GameState = new State() };

      //Act && Assert
      Assert.True(turn1.Equals(turn2));
    }

    [Test]
    public void Equals_GivenNonIdenticalTurns_ExpectFalse()
    {
      //Arrange
      var turn1 = new Turn
      {
        GameState = new State(new[]{
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|..."
        })
      };
      var turn2 = new Turn { GameState = new State() };

      //Act && Assert
      Assert.False(turn1.Equals(turn2));
    }
  }
}