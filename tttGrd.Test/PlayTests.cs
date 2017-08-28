using NUnit.Framework;

namespace tttGrd.Test
{
  [TestFixture]
  class PlayTests
  {
    [Test]
    public void DefaultConstructorTest()
    {
      //Arrange
      var play = new Play();

      //Act && Assert
      Assert.True(play.Turns.Count == 0);
      Assert.True(play.Outcome == 0);
    }

    [Test]
    public void ConstructorWithArgumentTest()
    {
      //Arrange
      var play = new Play(new[]{
        new []{
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|..."
          }
        });

      //Act && Assert
      Assert.True(play.Turns.Count == 1);
      Assert.True(play.Outcome == 0);
    }

    [Test]
    public void Has_GivenBelongingState_ExpectTrue()
    {
      //Arrange
      var play = new Play(new[]{
        new []{
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|..."
        }
      });
      var state = new State(new[]{
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|..."
        });

      //Act && Assert
      Assert.True(play.Has(state));
    }

    [Test]
    public void Has_GivenNonBelongingState_ExpectFalse()
    {
      //Arrange
      var play = new Play(new[]{
        new []{
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|..."
        }
      });
      var state = new State(new[]{
          ".x.|...|...", ".x.|...|...", "...|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|...",
          ".x.|...|...", ".x.|...|...", ".x.|...|..."
        });

      //Act && Assert
      Assert.False(play.Has(state));
    }
  }
}