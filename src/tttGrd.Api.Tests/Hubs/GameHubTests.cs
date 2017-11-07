using System;
using System.Threading.Tasks;
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
      mockDatabase.AddPlayerAsync(Arg.Any<string>()).Returns(Task.FromResult(default(Token)));
      var hub = new GameHub(mockDatabase);

      //Act
      hub.Announce("Player-1");

      //Assert
      await mockDatabase.Received(1).AddPlayerAsync("Player-1");
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
      var hub = new GameHub(mockDatabase);

      //Act
      Assert.ThrowsAsync<NullReferenceException>(() => hub.SendMoveAI("Ticket", 0, 4, 'x'));

      //Assert
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", (0, 4), Field.X);
      await mockDatabase.Received(1).RecordMoveAsync("Ticket", Arg.Any<(int, int)>(), Field.O);
      await mockDatabase.Received(2).GetStateAsync("Ticket");
      await mockDatabase.Received(1).GetAgniKaiByTicketAsync("Ticket");
    }
  }
}
