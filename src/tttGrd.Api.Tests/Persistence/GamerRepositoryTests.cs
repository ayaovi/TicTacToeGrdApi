using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  class GamerRepositoryTests
  {
    [Test]
    public async Task CreateGamerTest()
    {
      //Arrange
      var mockKeyGenerator = Substitute.For<IKeyGenerator>();
      mockKeyGenerator.GenerateKey().Returns(string.Empty);
      var mockVault = Substitute.For<IVault>();
      var gamerRepo = new GamerRepository(mockKeyGenerator, mockVault);

      //Act
      await gamerRepo.CreateGamerAsync();

      //Assert
      await mockKeyGenerator.Received(1).GenerateKey();
      await mockVault.Received(1).AddGamerKey(Arg.Any<string>());
    }
  }
}