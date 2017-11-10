using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Hubs;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Hubs
{
  [TestFixture]
  internal class GameHubTests
  {
    [Test]
    public async Task Announce_GivenUsername_ExpectPlayerBeAdded()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.AddConnectionAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
      var context = Substitute.For<HubCallerContext>();
      context.ConnectionId.Returns("1234");
      var hub = new GameHub(mockDatabase)
      {
        Context = context
      };

      //Act
      await hub.AnnounceAsync("Player-1");

      //Assert
      await mockDatabase.Received(1).AddConnectionAsync("Player-1", "1234");
    }

    [Test]
    public async Task NotifyPlayerAsync_GivenParameters_ExpectPlayerBeNotified()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetConnectionAsync(Arg.Any<string>()).Returns(Task.FromResult("1234"));
      var hub = new GameHub(mockDatabase);

      //Act
      Assert.ThrowsAsync<NullReferenceException>(() => hub.NotifyPlayerAsync("Player-1", "Player-2"));

      //Assert
      await mockDatabase.Received(1).GetConnectionAsync("Player-1");
    }

    [Test]
    public async Task NotifyOfChallengeAcceptedAsync_GivenParameters_ExpectPlayerBeNotified()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetConnectionAsync(Arg.Any<string>()).Returns(Task.FromResult("1234"));
      var hub = new GameHub(mockDatabase);

      //Act
      Assert.ThrowsAsync<NullReferenceException>(() => hub.NotifyPlayerAsync("Player-1", "Player-2"));

      //Assert
      await mockDatabase.Received(1).GetConnectionAsync("Player-1");
    }

    [Test]
    public async Task SendMoveAI_GivenParameters_ExpectSomething()
    {
      //Arrange
      var state = new State(new[]{
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });
      var agniKai = new AgniKai
      {
        Ticket = "Ticket"
      };
      agniKai.AddGamer(new Player
      {
        AgniKaiTicket = "Ticket",
        Name = "Player-1",
        Indicator = Field.X
      });
      agniKai.AddGamer(new AI
      {
        AgniKaiTicket = "Ticket",
        Name = "Gamer-1",
        Indicator = Field.O
      });
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.RecordMoveAsync(Arg.Any<string>(), Arg.Any<(int, int)>(), Arg.Any<Field>()).Returns(Task.CompletedTask);
      mockDatabase.GetStateAsync(Arg.Any<string>()).Returns(Task.FromResult(state));
      mockDatabase.GetAgniKaiByTicketAsync(Arg.Any<string>()).Returns(Task.FromResult(agniKai));
      //var clients = Substitute.For<IHubCallerConnectionContext<object>>();
      //clients.Group(Arg.Any<string>()).broadcastState(Arg.Any<State>()).Returns();
      var hub = new GameHub(mockDatabase);
      //var hub = new GameHub(mockDatabase)
      //{
      //  Clients = clients,
      //};

      //Act
      Assert.ThrowsAsync<NullReferenceException>(() => hub.SendMoveAI("Ticket", 0, 4, 'x'));
      //await hub.SendMoveAI("Ticket", 0, 4, 'x');

      //Assert
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", (0, 4), Field.X);
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", Arg.Any<(int, int)>(), Field.O);
      await mockDatabase.Received(2).GetStateAsync("Ticket");
      await mockDatabase.Received(1).GetAgniKaiByTicketAsync("Ticket");
    }
  }
}
