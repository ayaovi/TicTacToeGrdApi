using System.Collections.Generic;
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
  }
}
