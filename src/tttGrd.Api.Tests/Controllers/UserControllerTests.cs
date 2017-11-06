using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FluentAssertions;
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

    [Test]
    public async Task All_GivenNoPlayers_ExpectEmptyList()
    {
      //Arrange
      var vault = Substitute.For<IVault>();
      var keyGenerator = Substitute.For<IKeyGenerator>();
      var database = new DatabaseRepository(vault, keyGenerator);
      var controller = new UserController(database, vault);

      //Act
      var action = await controller.All();
      var allResponse = action as OkNegotiatedContentResult<List<Player>>;

      //Assert
      Assert.NotNull(allResponse);
      allResponse.Content.ShouldBeEquivalentTo(new List<Player>());
    }
    
    [Test]
    public async Task All_GivenPlayers_ExpectListOfPlayers()
    {
      //Arrange
      var vault = Substitute.For<IVault>();
      vault.GetGameTokenAsync("Token").Returns(Task.FromResult(new Token { Value = "Token" }));
      vault.AddGameTokenAsync(Arg.Any<Token>()).Returns(Task.CompletedTask);
      var keyGenerator = Substitute.For<IKeyGenerator>();
      keyGenerator.GenerateGameTokenAsync(Arg.Any<string>()).Returns(Task.FromResult("Token"));
      var database = new DatabaseRepository(vault, keyGenerator);
      var controller = new UserController(database, vault);
      var expected = new List<Player>
      {
        new Player
        {
          Name = "Player-1",
          Status = PlayerStatus.Online,
          GameToken = "Token"
        },
        new Player
        {
          Name = "Player-2",
          Status = PlayerStatus.Online,
          GameToken = "Token"
        }
      };

      //Act
      await database.AddPlayerAsync("Player-1");
      await database.AddPlayerAsync("Player-2");
      var action = await controller.All();
      var allResponse = action as OkNegotiatedContentResult<List<Player>>;

      //Assert
      Assert.NotNull(allResponse);
      allResponse.Content.ShouldBeEquivalentTo(expected);
    }

    //[Test]
    //public async Task Submit_GivenValidTicketAndToken_Expect200Ok()
    //{
    //  //Arrange
    //  var vault = Substitute.For<IVault>();
    //  vault.GetGameTokenAsync("Token").Returns(Task.FromResult(new Token { Value = "Token" }));
    //  vault.AddGameTokenAsync(Arg.Any<Token>()).Returns(Task.CompletedTask);
    //  var keyGenerator = Substitute.For<IKeyGenerator>();
    //  keyGenerator.GenerateKey().Returns("Ticket");
    //  keyGenerator.GenerateGameTokenAsync(Arg.Any<string>()).Returns(Task.FromResult("Token"));
    //  var database = new DatabaseRepository(vault, keyGenerator);
    //  var agniKaiRepo = new AgniKaiRepository(vault, keyGenerator, database);
    //  var controller = new UserController(database, vault);
    //  var request = new SubmissionRequest
    //  {
    //    Ticket = "Ticket",
    //    Token = "Token"
    //  };

    //  //Act
    //  var action = await controller.SubmitAgniKaiTicket(request);
    //  var allResponse = action as OkResult;

    //  //Assert
    //}
  }
}
