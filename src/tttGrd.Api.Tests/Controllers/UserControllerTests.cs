using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Controllers;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;
using System;

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

    [Test]
    public async Task Submit_GivenValidTicketAndToken_Expect200Ok()
    {
      //Arrange
      var mockVault = Substitute.For<IVault>();
      mockVault.GetGameTokenAsync("Token").Returns(Task.FromResult(new Token
      {
        Value = "Token",
        ExpirationDate = DateTime.UtcNow.Add(new TimeSpan(0, 0, 0, 10)) /* 10s in the future. */
      }));
      mockVault.AddGameTokenAsync(Arg.Any<Token>()).Returns(Task.CompletedTask);
      var mockKeyGenerator = Substitute.For<IKeyGenerator>();
      mockKeyGenerator.GenerateKey().Returns("Ticket");
      mockKeyGenerator.GenerateGameTokenAsync(Arg.Any<string>()).Returns(Task.FromResult("Token"));
      var database = new DatabaseRepository(mockVault, mockKeyGenerator);
      var agniKaiRepo = new AgniKaiRepository(mockVault, mockKeyGenerator, database);
      var controller = new UserController(database, mockVault);

      //Act
      var ticket = await agniKaiRepo.InitiateAgniKaiAsync();
      var token = await database.AddPlayerAsync("Player-1");
      var request = new SubmissionRequest
      {
        Ticket = ticket,
        Token = token.Value
      };
      var action = await controller.SubmitAgniKaiTicket(request);
      var submitResponse = action as OkNegotiatedContentResult<Field>;

      //Assert
      Assert.NotNull(submitResponse);
      Assert.AreNotEqual(submitResponse.Content, Field.Empty);
      Assert.AreEqual(await database.GetPlayerCountAsync(), 1);
      await mockKeyGenerator.Received(1).GenerateKey();
      await mockVault.Received(1).AddAgniKaiTicket("Ticket");
      await mockVault.Received(1).GetGameTokenAsync("Token");
    }

    [Test]
    public async Task Submit_GivenTwoGamersInAgniKai_Expect200OkAndDifferentGamerIndicators()
    {
      //Arrange
      var mockVault = Substitute.For<IVault>();
      mockVault.GetGameTokenAsync("Token-1").Returns(Task.FromResult(new Token
      {
        Value = "Token-1",
        ExpirationDate = DateTime.UtcNow.Add(new TimeSpan(0, 0, 0, 10)) /* 10s in the future. */
      }));
      mockVault.GetGameTokenAsync("Token-2").Returns(Task.FromResult(new Token
      {
        Value = "Token-2",
        ExpirationDate = DateTime.UtcNow.Add(new TimeSpan(0, 0, 0, 10)) /* 10s in the future. */
      }));
      mockVault.AddGameTokenAsync(Arg.Any<Token>()).Returns(Task.CompletedTask);
      var mockKeyGenerator = Substitute.For<IKeyGenerator>();
      mockKeyGenerator.GenerateKey().Returns("Ticket");
      mockKeyGenerator.GenerateGameTokenAsync("Player-1").Returns(Task.FromResult("Token-1"));
      mockKeyGenerator.GenerateGameTokenAsync("Player-2").Returns(Task.FromResult("Token-2"));
      var database = new DatabaseRepository(mockVault, mockKeyGenerator);
      var agniKaiRepo = new AgniKaiRepository(mockVault, mockKeyGenerator, database);
      var controller = new UserController(database, mockVault);

      //Act
      var ticket = await agniKaiRepo.InitiateAgniKaiAsync();
      var token1 = await database.AddPlayerAsync("Player-1");
      var token2 = await database.AddPlayerAsync("Player-2");
      var request1 = new SubmissionRequest
      {
        Ticket = ticket,
        Token = token1.Value
      };
      var request2 = new SubmissionRequest
      {
        Ticket = ticket,
        Token = token2.Value
      };
      var action1 = await controller.SubmitAgniKaiTicket(request1);
      var submitResponse1 = action1 as OkNegotiatedContentResult<Field>;
      var action2 = await controller.SubmitAgniKaiTicket(request2);
      var submitResponse2 = action2 as OkNegotiatedContentResult<Field>;

      //Assert
      Assert.NotNull(submitResponse1);
      Assert.NotNull(submitResponse2);
      Assert.AreNotEqual(submitResponse1.Content, submitResponse2.Content);
      Assert.AreEqual(await database.GetPlayerCountAsync(), 2);
      await mockKeyGenerator.Received(1).GenerateKey();
      await mockVault.Received(1).AddAgniKaiTicket("Ticket");
      await mockVault.Received(1).GetGameTokenAsync("Token-1");
      await mockVault.Received(1).GetGameTokenAsync("Token-2");
    }
  }
}
