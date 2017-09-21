using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Models;
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
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      var gamerRepo = new GamerRepository(mockKeyGenerator, mockVault, mockDatabase);

      //Act
      await gamerRepo.CreateGamerAsync();

      //Assert
      await mockKeyGenerator.Received(1).GenerateKey();
      await mockVault.Received(1).AddGamerKey(Arg.Any<string>());
      await mockDatabase.Received(1).AddGamerAsync(Arg.Any<Gamer>());
    }
  }
}