using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
      var database = new DatabaseRepository();

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
      var database = new DatabaseRepository();
      var move = (2, 3);
      var expectedState = new State();
      const Field indicator = Field.X;
      expectedState.Fields[2][3] = indicator;

      //Act
      await database.AddAgniKaiAsync(agniKai);
      await database.RecordMove(agniKai.Ticket, move, indicator);
      var state = await database.GetStateAsync(agniKai.Ticket);

      //Assert
      state.ShouldBeEquivalentTo(expectedState);
    }

    [Test]
    public async Task AddUserAsync_GivenUsername_ExpectUserBeAdded()
    {
      //Arrange
      var expected = new User { Username = "User-1" };
      var database = new DatabaseRepository();

      //Act
      await database.AddUserAsync("User-1");
      var result = await database.GetUserByNameAsync("User-1");

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUsersAsync_GivenNoUsers_ExpectEmptyCollection()
    {
      //Arrange
      var expected = new List<User>();
      var database = new DatabaseRepository();

      //Act
      var result = await database.GetUsersAsync();

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUsersAsync_GivenOneUser_ExpectCollectionWithOneUser()
    {
      //Arrange
      var expected = new List<User>
      {
        new User{Username = "User-1"}
      };
      var database = new DatabaseRepository();

      //Act
      await database.AddUserAsync("User-1");
      var result = await database.GetUsersAsync();

      //Assert
      result.ShouldBeEquivalentTo(expected);
    }
  }
}