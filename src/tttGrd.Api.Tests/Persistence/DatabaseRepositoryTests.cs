using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  class DatabaseRepositoryTests
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

      //Assert
      result.ShouldBeEquivalentTo(agniKai);
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