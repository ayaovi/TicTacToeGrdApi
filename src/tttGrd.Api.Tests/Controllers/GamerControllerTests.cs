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
      var mockGamerRepo = Substitute.For<IGamerRepository>();
      mockGamerRepo.CreateGamerAsync(Arg.Any<string>()).Returns(Task.CompletedTask);
      var vault = Substitute.For<IVault>();
      var controller = new GamerController(mockGamerRepo);
      var request = new SubmissionRequest
      {
        Ticket = "Ticket",
        Token = "Token"
      };

      //Act
      var action = await controller.Create(request);
      var createResponse = action as OkResult;

      //Assert
      Assert.IsNotNull(createResponse);
      await mockGamerRepo.Received(1).CreateGamerAsync("Ticket");
    }
  }
}
