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
  }
}