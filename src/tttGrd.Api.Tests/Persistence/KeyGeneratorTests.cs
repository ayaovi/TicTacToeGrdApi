using System.Threading.Tasks;
using NUnit.Framework;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  internal class KeyGeneratorTests
  {
    [Test]
    public async Task GenerateGameTokenAsync_GivenUsername_ExpectGameToken()
    {
      //Arrange
      var generator = new KeyGenerator();

      //Act
      var token = await generator.GenerateGameTokenAsync("Player-1");

      //Assert
      Assert.NotNull(token);
    }
  }
}
