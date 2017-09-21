﻿using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  class GamerRepositoryTests
  {
    [Test]
    public async Task CreateGamerTest()
    {
      //Arrange
      var mockKeyGenerator = Substitute.For<IKeyGenerator>();
      mockKeyGenerator.GenerateKey().Returns(string.Empty);
      var mockVault = Substitute.For<IVault>();
      var mockDatabase = Substitute.For<IDatabaseRepository>();
      mockDatabase.GetAgniKaiByTicket(Arg.Any<string>()).Returns(new AgniKai {Ticket = string.Empty});
      var gamerRepo = new GamerRepository(mockDatabase);

      //Act
      await gamerRepo.CreateGamerAsync(string.Empty);

      //Assert
      //await mockKeyGenerator.Received(1).GenerateKey();
      //await mockVault.Received(1).AddAgniKaiTicket(string.Empty);
      await mockDatabase.Received(1).GetAgniKaiByTicket(string.Empty);
    }
  }
}