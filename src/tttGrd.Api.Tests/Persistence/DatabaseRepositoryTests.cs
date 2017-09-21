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
    public async Task AddGamer_GivenGamer_ExpectGamerBeAdded()
    {
      //Arrange
      var gamer = new Gamer { Token = "12345" };
      var database = new DatabaseRepository();

      //Act
      await database.AddGamerAsync(gamer);
      var result = await database.GetGamerByTokenAsync("12345");

      //Assert
      result.ShouldBeEquivalentTo(gamer);
    }
  }
}
