using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  internal class GamerRepositoryTests
  {
    [Test]
    public async Task CreateGamerTest()
    {
      //Arrange
      var mockKeyGenerator = Substitute.For<IKeyGenerator>();
      mockKeyGenerator.GenerateKey().Returns(string.Empty);
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetAgniKaiByTicket(Arg.Any<string>()).Returns(new AgniKai {Ticket = string.Empty});
      var gamerRepo = new GamerRepository(mockDatabase);

      //Act
      await gamerRepo.CreateGamerAsync(string.Empty);

      //Assert
      await mockDatabase.Received(1).GetAgniKaiByTicket(string.Empty);
    }
  }
}