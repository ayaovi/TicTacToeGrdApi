using FluentAssertions;
using NUnit.Framework;
using tttGrd.Api.Models;

namespace tttGrd.Api.Tests.Models
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
      turn2.ShouldBeEquivalentTo(turn1);
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