using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  class AgniKaiRepositoryTests
  {
    [Test]
    public async Task InitiateAgniKaiAsync_Test()
    {
      //Arrange
      var mockKeyGenerator = Substitute.For<IKeyGenerator>();
      mockKeyGenerator.GenerateKey().Returns(string.Empty);
      var mockVault = Substitute.For<IVault>();
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      var agniKaiRepo = new AgniKaiRepository(mockVault, mockKeyGenerator, mockDatabase);

      //Act
      await agniKaiRepo.InitiateAgniKaiAsync();

      //Assert
      await mockKeyGenerator.Received(1).GenerateKey();
      await mockVault.Received(1).AddAgniKaiTicket(Arg.Any<string>());
      await mockDatabase.Received(1).AddAgniKaiAsync(Arg.Any<AgniKai>());
    } 
  }
}
