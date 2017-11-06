using System.Threading.Tasks;
using System.Web.Http.Results;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Controllers;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Controllers
{
  [TestFixture]
  internal class UserControllerTests
  {
    [Test]
    public async Task Login_GivenName_ExpectPlayerloggedIn()
    {
      //Arrange
      var database = Substitute.For<IDatabaseRepository>();
      database.GetPlayerByNameAsync(Arg.Any<string>()).Returns(Task.FromResult(default(Player)));
      database.AddPlayerAsync(Arg.Any<string>()).Returns(Task.FromResult(default(Token)));
      var vault = Substitute.For<IVault>();
      var controller = new UserController(database, vault);

      //Act
      var action = await controller.Login("Player-1");
      var loginResponse = action as OkNegotiatedContentResult<Token>;

      //Assert
      Assert.IsNotNull(loginResponse);
      await database.Received(1).GetPlayerByNameAsync("Player-1");
      await database.Received(1).AddPlayerAsync("Player-1");
    }

    [Test]
    public async Task Login_GivenPlayerAlreadyloggedIn_ExpectPlayerOldToken()
    {
      //Arrange
      var vault = Substitute.For<IVault>();
      vault.GetGameTokenAsync("Token-1").Returns(Task.FromResult(new Token { Value = "Token-1" }));
      var keyGenerator = Substitute.For<IKeyGenerator>();
      keyGenerator.GenerateGameTokenAsync("Player-1").Returns(Task.FromResult("Token-1"));
      var database = new DatabaseRepository(vault, keyGenerator);
      var controller = new UserController(database, vault);

      //Act
      await database.AddPlayerAsync("Player-1");
      var action = await controller.Login("Player-1");
      var loginResponse = action as OkNegotiatedContentResult<Token>;

      //Assert
      Assert.IsNotNull(loginResponse);
      await keyGenerator.Received(1).GenerateGameTokenAsync("Player-1");
      await vault.Received(1).GetGameTokenAsync("Token-1");
      Assert.AreEqual(loginResponse.Content.Value, "Token-1");
      Assert.AreEqual(await database.GetPlayerCountAsync(), 1);
    }
  }
}
