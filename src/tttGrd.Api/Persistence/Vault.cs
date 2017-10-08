using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public class Vault : IVault
  {
    public IList<string> GamerKeys { get; } = Enumerable.Empty<string>().ToList();
    public IList<Token> GameTokens { get; } = Enumerable.Empty<Token>().ToList();

    public Task AddAgniKaiTicket(string key)
    {
      GamerKeys.Add(key);
      return Task.CompletedTask;
    }

    public Task AddGameTokenAsync(Token gameToken)
    {
      GameTokens.Add(gameToken);
      return Task.CompletedTask;
    }
  }

  public class Token
  {
    public string Value { get; set; }
    public DateTime ExpirationDate { get; set; }
  }
}