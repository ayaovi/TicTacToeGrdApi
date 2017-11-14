using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Web.Http.Results;
using tttGrd.Api.Controllers;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Controllers
{
  [TestFixture]
  internal class GamerControllerTests
  {
    [Test]
    public async Task Create_GivenTicket_ExpectAICreated()
    {
      //Arrange
      var gamerRepo = Substitute.For<IGamerRepository>();
      gamerRepo.CreateGamerAsync(Arg.Any<string>()).Returns(Task.FromResult("x"));
      var vault = Substitute.For<IVault>();
      var controller = new GamerController(gamerRepo);
      var request = new SubmissionRequest
      {
        Ticket = "Ticket",
        Token = "Token"
      };

      //Act
      var action = await controller.Create(request);
      var createResponse = action as OkNegotiatedContentResult<string>;

      //Assert
      Assert.IsNotNull(createResponse);
      Assert.AreEqual(createResponse.Content, "x");
      await gamerRepo.Received(1).CreateGamerAsync("Ticket");
    }
  }
}
