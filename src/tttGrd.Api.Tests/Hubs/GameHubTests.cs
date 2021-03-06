﻿using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Hubs;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;
// ReSharper disable InconsistentNaming

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
      var clients = Substitute.For<IHubCallerConnectionContext<object>>();
      var uiMock = Substitute.For<IMockClient>();
      uiMock.When(x => x.notifyOfChallenge(Arg.Any<string>())).Do(x => { });
      clients.Client(Arg.Any<string>()).Returns(uiMock);
      var hub = new GameHub(mockDatabase)
      {
        Clients = clients
      };

      //Act
      await hub.NotifyPlayerAsync("Challengee", "Challenger");

      //Assert
      await mockDatabase.Received(1).GetConnectionAsync("Challengee");
      uiMock.Received(1).notifyOfChallenge("Challenger");
    }

    [Test]
    public async Task NotifyOfChallengeAcceptedAsync_GivenParameters_ExpectPlayerBeNotified()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetConnectionAsync(Arg.Any<string>()).Returns(Task.FromResult("1234"));
      var clients = Substitute.For<IHubCallerConnectionContext<object>>();
      var uiMock = Substitute.For<IMockClient>();
      uiMock.When(x => x.notifyOfChallengeAccpeted(Arg.Any<string>())).Do(x => { });
      clients.Client(Arg.Any<string>()).Returns(uiMock);
      var hub = new GameHub(mockDatabase)
      {
        Clients = clients
      };

      //Act
      await hub.NotifyOfChallengeAcceptedAsync("Challengee", "Challenger");

      //Assert
      await mockDatabase.Received(1).GetConnectionAsync("Challenger");
      uiMock.Received(1).notifyOfChallengeAccpeted("Challengee");
    }

    [Test]
    public async Task AgniKaiStartNotification_GivenParameters_ExpectPlayerBeNotified()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetConnectionAsync(Arg.Any<string>()).Returns(Task.FromResult("1234"));
      var clients = Substitute.For<IHubCallerConnectionContext<object>>();
      var uiMock = Substitute.For<IMockClient>();
      uiMock.When(x => x.notifyOfAgniKaiStart(Arg.Any<string>())).Do(x => { });
      clients.Client(Arg.Any<string>()).Returns(uiMock);
      var hub = new GameHub(mockDatabase)
      {
        Clients = clients
      };

      //Act
      await hub.AgniKaiStartNotification("Ticket", "challengee");

      //Assert
      await mockDatabase.Received(1).GetConnectionAsync("challengee");
      uiMock.Received(1).notifyOfAgniKaiStart("Ticket");
    }

    [Test]
    public async Task JoinAgniKai_GivenTicket_ExpectPlayerAddedToAgnikai()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      var context = Substitute.For<HubCallerContext>();
      context.ConnectionId.Returns("1234");
      var groups = Substitute.For<IGroupManager>();
      groups.When(x => x.Add(Arg.Any<string>(), Arg.Any<string>())).Do(x => { });
      var hub = new GameHub(mockDatabase)
      {
        Context = context,
        Groups = groups
      };

      //Act
      await hub.JoinAgniKai("Ticket");

      //Assert
      await mockDatabase.Received(0).GetConnectionAsync(Arg.Any<string>());
      await groups.Received(1).Add("1234", "Ticket");
    }

    [Test]
    public async Task LeaveAgniKai_GivenTicket_ExpectPlayerBeRemovedFromAgnikai()
    {
      //Arrange
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      var context = Substitute.For<HubCallerContext>();
      context.ConnectionId.Returns("1234");
      var groups = Substitute.For<IGroupManager>();
      groups.When(x => x.Remove(Arg.Any<string>(), Arg.Any<string>())).Do(x => { });
      var hub = new GameHub(mockDatabase)
      {
        Context = context,
        Groups = groups
      };

      //Act
      await hub.LeaveAgniKai("Ticket");

      //Assert
      await mockDatabase.Received(0).GetConnectionAsync(Arg.Any<string>());
      await groups.Received(1).Remove("1234", "Ticket");
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
      var clients = Substitute.For<IHubCallerConnectionContext<object>>();
      var uiMock = Substitute.For<IMockClient>();
      uiMock.When(x => x.broadcastState(Arg.Any<State>())).Do(x => { });
      clients.Group(Arg.Any<string>()).Returns(uiMock);
      var hub = new GameHub(mockDatabase)
      {
        Clients = clients 
      };

      //Act
      await hub.SendMoveAI("Ticket", 0, 4, 'x');

      //Assert
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", (0, 4), Field.X);
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", Arg.Any<(int, int)>(), Field.O);
      await mockDatabase.Received(2).GetStateAsync("Ticket");
      await mockDatabase.Received(1).GetAgniKaiByTicketAsync("Ticket");
      uiMock.Received(1).broadcastState(state);
    }

    [Test]
    public async Task SendMovePlayer_GivenParameters_ExpectSomething()
    {
      //Arrange
      var state = new State(new[]{
        "...|.x.|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|...",
        "...|...|...", "...|...|...", "...|...|..."
      });
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.RecordMoveAsync(Arg.Any<string>(), Arg.Any<(int, int)>(), Arg.Any<Field>()).Returns(Task.CompletedTask);
      mockDatabase.GetStateAsync(Arg.Any<string>()).Returns(Task.FromResult(state));
      var clients = Substitute.For<IHubCallerConnectionContext<object>>();
      var uiMock = Substitute.For<IMockClient>();
      uiMock.When(x => x.broadcastState(Arg.Any<State>())).Do(x => { });
      clients.Group(Arg.Any<string>()).Returns(uiMock);
      var hub = new GameHub(mockDatabase)
      {
        Clients = clients
      };

      //Act
      await hub.SendMovePlayer("Ticket", 0, 4, 'x');

      //Assert
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", (0, 4), Field.X);
      await mockDatabase.Received(1).GetStateAsync("Ticket");
      uiMock.Received(1).broadcastState(state);
    }
  }

  public interface IMockClient
  {
    void broadcastState(State state);
    void notifyOfChallenge(string challengerId);
    void notifyOfChallengeAccpeted(string challengerId);
    void notifyOfAgniKaiStart(string agnikaiTicket);
  }
}
