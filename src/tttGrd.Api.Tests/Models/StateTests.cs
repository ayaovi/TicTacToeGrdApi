﻿using FluentAssertions;
using NUnit.Framework;
using tttGrd.Api.Models;

namespace tttGrd.Api.Tests.Models
{
  [TestFixture]
  internal class StateTests
  {
    [Test]
    public void DefaultConstructorTest()
    {
      //Arrange
      var state = new State();
      const string visual = "...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...@...|...|...";
      
      //Act && Assert
      Assert.True(visual == state.ToString());
    }

    [Test]
    public void ConstructWithArgumentTest()
    {
      //Arrange
      var state = new State(new[]{
        ".o.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|..."
        });
      const string visual = ".o.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...@.x.|...|...";
      
      //Act && Assert
      Assert.True(visual == state.ToString());
    }

    [Test]
    public void CopyCobstructorTest()
    {
      //Arrange
      var state1 = new State(new[]{
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|...",
        ".x.|...|...", ".x.|...|...", ".x.|...|..."
      });
      var state2 = new State(state1);
      
      //Act && Assert
      state2.ShouldBeEquivalentTo(state1);
    }
  }
}