using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Tests.Persistence
{
  [TestFixture]
  internal class VaultTests
  {
    [Test]
    public async Task AddGamerKey_GivenKey_ShouldAddKey()
    {
      //Arrange
      var generator = new KeyGenerator();
      var key = await generator.GenerateKey();
      var vault = new Vault();
      var expected = new List<string> { key };

      //Act
      await vault.AddAgniKaiTicket(key);
      var keys = await vault.GetGamerKeysAsync();

      //Assert
      keys.ShouldAllBeEquivalentTo(expected);
    }

    [Test]
    public async Task AddGameTokenAsync_GivenToken_ExpectTokenBeAdded()
    {
      //Arrange
      var token = new Token
      {
        Value = "Token",
        ExpirationDate = DateTime.UtcNow
      };
      var vault = new Vault();
      var expected = new List<Token> { token };

      //Act
      await vault.AddGameTokenAsync(token);
      var token1 = await vault.GetGameTokenAsync("Token");
      var tokens = await vault.GetGameTokensAsync();

      //Assert
      tokens.ShouldBeEquivalentTo(expected);
      token1.ShouldBeEquivalentTo(token);
    }
  }
}
