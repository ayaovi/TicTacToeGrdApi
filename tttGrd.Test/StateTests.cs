using NUnit.Framework;

namespace tttGrd.Test
{
  [TestFixture]
  class StateTests
  {
    [Test]
    public void DefaultConstructorTest()
    {
      //Arrange
      var state = new State();
      var visual = "...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...";
      
      //Act && Assert
      Assert.True(visual == state.ToString());
    }

    [Test]
    public void ConstructWithArgumentTest()
    {
      //Arrange
      var state = new State(new[]{
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|..."
        });
      var visual = ".x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...";
      
      //Act && Assert
      Assert.True(visual == state.ToString());
    }
  }
}