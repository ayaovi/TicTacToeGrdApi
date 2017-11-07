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
      mockDatabase.GetAgniKaiByTicketAsync(Arg.Any<string>()).Returns(new AgniKai { Ticket = string.Empty });
      var gamerRepo = new GamerRepository(mockDatabase);

      //Act
      await gamerRepo.CreateGamerAsync(string.Empty);

      //Assert
      await mockDatabase.Received(1).GetAgniKaiByTicketAsync(string.Empty);
    }

    [Test]
    public async Task CreateGamerWithNameAsync_GivenTicketAndName_ExpectGamerCreated()
    {
      //Arrange
      var agnikai = new AgniKai { Ticket = "Ticket" };
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetAgniKaiByTicketAsync(Arg.Any<string>()).Returns(Task.FromResult(agnikai));
      var gamerRepo = new GamerRepository(mockDatabase);

      //Act
      await gamerRepo.CreateGamerWithNameAsync("Ticket", "Gamer-1");
      var gamer = agnikai.GetGamerByName("Gamer-1");

      //Assert
      await mockDatabase.Received(1).GetAgniKaiByTicketAsync("Ticket");
      Assert.IsInstanceOf<AI>(gamer);
    }
  }
}