using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Web.Http.Results;
using tttGrd.Api.Controllers;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Controllers
{
  [TestFixture]
  internal class AgniKaiControllerTests
  {
    [Test]
    public async Task Initiate_GivenTicket_ExpectAICreated()
    {
      //Arrange
      var vault = Substitute.For<IVault>();
      vault.AddAgniKaiTicket(Arg.Any<string>()).Returns(Task.CompletedTask);
      var keyGenerator = Substitute.For<IKeyGenerator>();
      keyGenerator.GenerateKey().Returns(Task.FromResult("Ticket"));
      var database = Substitute.For<IDatabaseRepository>();
      database.AddAgniKaiAsync(Arg.Any<AgniKai>()).Returns(Task.CompletedTask);
      var agniKaiRepo = new AgniKaiRepository(vault, keyGenerator, database);
      var controller = new AgniKaiController(agniKaiRepo);

      //Act
      var action = await controller.Initiate();
      var initiateResponse = action as OkNegotiatedContentResult<string>;

      //Assert
      Assert.IsNotNull(initiateResponse);
      Assert.AreEqual(initiateResponse.Content, "Ticket");
      await keyGenerator.Received(1).GenerateKey();
      await vault.Received(1).AddAgniKaiTicket("Ticket");
      //await database.Received(1).AddAgniKaiAsync(new AgniKai { Ticket = "Ticket" });
      await database.Received(1).AddAgniKaiAsync(Arg.Any<AgniKai>());
    }
  }
}
