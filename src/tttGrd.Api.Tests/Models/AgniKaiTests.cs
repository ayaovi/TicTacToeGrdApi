using FluentAssertions;
using NUnit.Framework;
using tttGrd.Api.Models;

namespace tttGrd.Api.Tests.Models
{
  [TestFixture]
  internal class AgniKaiTests
  {
    [Test]
    public void AddGamer_GivenGamer_ShouldAddGamer()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = string.Empty };
      var gamer = new AI { AgniKaiTicket = string.Empty, Name = "gamer_1" };

      //Act
      agniKai.AddGamer(gamer);
      var result = agniKai.GetGamerByName("gamer_1");

      //Assert
      result.ShouldBeEquivalentTo(gamer);
    }

    [Test]
    public void CanAccommodate_Test()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = string.Empty };

      //Act && Assert
      Assert.IsTrue(agniKai.CanAccommodateGamer());
    }

    [Test]
    public void GetNextGamerId_GivenEmptyAgniKai_ShouldReturnOne()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = string.Empty };

      //Act && Assert
      Assert.AreEqual(1, agniKai.GetNextGamerId());
    }

    [Test]
    public void GetNextGamerId_GivenAgniKaiWithOneGamer_ShouldReturnTwo()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = string.Empty };

      //Act 
      agniKai.AddGamer(new AI { AgniKaiTicket = string.Empty, Name = "gamer_1" });

      //Assert
      Assert.AreEqual(2, agniKai.GetNextGamerId());
    }
  }
}