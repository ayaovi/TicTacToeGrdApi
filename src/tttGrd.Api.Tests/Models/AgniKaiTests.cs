using System;
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
    public void AddGamer_GivenPreviousGamerWithIndicatorO_ShouldAddGamerWithIndicatorX()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = string.Empty };
      var gamer = new Player { AgniKaiTicket = string.Empty, Name = "gamer_2" };

      //Act
      agniKai.AddGamer(new Player { AgniKaiTicket = string.Empty, Name = "gamer_1", Indicator = Field.O });
      agniKai.AddGamer(gamer);

      //Assert
      Assert.AreEqual(gamer.Indicator, Field.X);
    }

    [Test]
    public void AddGamer_GivenPreviousGamerWithIndicatorX_ShouldAddGamerWithIndicatorO()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = string.Empty };
      var gamer = new Player { AgniKaiTicket = string.Empty, Name = "gamer_2" };

      //Act
      agniKai.AddGamer(new Player { AgniKaiTicket = string.Empty, Name = "gamer_1", Indicator = Field.X });
      agniKai.AddGamer(gamer);

      //Assert
      Assert.AreEqual(gamer.Indicator, Field.O);
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

    [Test]
    public void GetGamerByIndicator_GivenNoGamer_ExpectException()
    {
      //Arrange
      var agniKai = new AgniKai { Ticket = "Ticket" };

      //Act && Assert
      Assert.Throws<InvalidOperationException>(() => agniKai.GetGamerByIndicator(Field.O));
    }

    [Test]
    public void GetGamerByIndicator_GivenGamerExists_ExpectGamer()
    {
      //Arrange
      var gamer = new AI { Indicator = Field.O };
      var agniKai = new AgniKai { Ticket = "Ticket" };

      //Act
      agniKai.AddGamer(gamer);

      //Assert
      agniKai.GetGamerByIndicator(Field.O).ShouldBeEquivalentTo(gamer);
    }
  }
}