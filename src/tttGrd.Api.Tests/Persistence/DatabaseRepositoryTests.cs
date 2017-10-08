using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  internal class DatabaseRepositoryTests
  {
    [Test]
    public async Task AddAgniKaiAsync_GivenAgniKai_ExpectAgniKaiBeAdded()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = "12345" };
      var mockVault = Substitute.For<IVault>();
      var mockKeyGen = Substitute.For<IKeyGenerator>();
      var database = new DatabaseRepository(mockVault, mockKeyGen);

      //Act
      await database.AddAgniKaiAsync(agniKai);
      var result = await database.GetAgniKaiByTicket("12345");
      var state = await database.GetStateAsync(agniKai.Ticket);

      //Assert
      result.ShouldBeEquivalentTo(agniKai);
      state.ShouldBeEquivalentTo(new State());
    }

    [Test]
    public async Task RecordMove_GivenTicketAndMove_ExpectMoveBeRecorded()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = "12345" };
      var mockVault = Substitute.For<IVault>();
      var mockKeyGen = Substitute.For<IKeyGenerator>();
      var database = new DatabaseRepository(mockVault, mockKeyGen);
      var move = (2, 3);
      var expectedState = new State();
      const Field indicator = Field.X;
      expectedState.Fields[2][3] = indicator;

      //Act
      await database.AddAgniKaiAsync(agniKai);
      await database.RecordMoveAsync(agniKai.Ticket, move, indicator);
      var state = await database.GetStateAsync(agniKai.Ticket);

      //Assert
      state.ShouldBeEquivalentTo(expectedState);
    }

    [Test]
    public async Task AddUserAsync_GivenUsername_ExpectUserBeAdded()
    {
      //Arrange
      var expected = new Player { Name = "Player-1", GameToken = "token" };
      var mockVault = Substitute.For<IVault>();
      var mockKeyGen = Substitute.For<IKeyGenerator>();
      mockKeyGen.GenerateGameTokenAsync("Player-1").Returns("token");
      var database = new DatabaseRepository(mockVault, mockKeyGen);

      //Act
      await database.AddPlayerAsync("Player-1");
      var result = await database.GetPlayerByNameAsync("Player-1");

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUsersAsync_GivenNoUsers_ExpectEmptyCollection()
    {
      //Arrange
      var expected = new List<Player>();
      var mockVault = Substitute.For<IVault>();
      var mockKeyGen = Substitute.For<IKeyGenerator>();
      var database = new DatabaseRepository(mockVault, mockKeyGen);

      //Act
      var result = await database.GetPlayersAsync();

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUsersAsync_GivenOneUser_ExpectCollectionWithOneUser()
    {
      //Arrange
      var expected = new List<Player>
      {
        new Player{Name = "Player-1", GameToken = "token"}
      };
      var mockVault = Substitute.For<IVault>();
      var mockKeyGen = Substitute.For<IKeyGenerator>();
      mockKeyGen.GenerateGameTokenAsync("Player-1").Returns("token");
      var database = new DatabaseRepository(mockVault, mockKeyGen);

      //Act
      await database.AddPlayerAsync("Player-1");
      var result = await database.GetPlayersAsync();

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task SubmitTicketAsync_GivenTokenAndTicket_ExpectTicketBeAdded()
    {
      //Arrange
      const string token = "token";
      const string ticket = "ticket";
      const string name = "Player-1";
      var expected = new Player
      {
        Name = name,
        GameToken = token,
        AgniKaiTicket = ticket,
        Status = PlayerStatus.Online
      };
      var mockVault = Substitute.For<IVault>();
      var mockKeyGen = Substitute.For<IKeyGenerator>();
      mockKeyGen.GenerateGameTokenAsync(name).Returns("token");
      var database = new DatabaseRepository(mockVault, mockKeyGen);

      //Act
      await database.AddAgniKaiAsync(new AgniKai { Ticket = ticket });
      await database.AddPlayerAsync(name);
      await database.SubmitTicketAsync(token, ticket);
      var result = await database.GetPlayerByNameAsync(name);

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }
  }
}