using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public class Vault : IVault
  {
    private readonly IList<string> _gamerKeys = Enumerable.Empty<string>().ToList();
    private readonly IList<Token> _gameTokens = Enumerable.Empty<Token>().ToList();

    public Task AddAgniKaiTicket(string key)
    {
      _gamerKeys.Add(key);
      return Task.CompletedTask;
    }

    public Task AddGameTokenAsync(Token gameToken)
    {
      _gameTokens.Add(gameToken);
      return Task.CompletedTask;
    }

    public Task<Token> GetGameTokenAsync(string value)
    {
      return Task.FromResult(_gameTokens.Single(token => token.Value == value));
    }

    public Task<IList<string>> GetGamerKeysAsync()
    {
      return Task.FromResult(_gamerKeys);
    }
  }

  public class Token
  {
    public string Value { get; set; }
    public DateTime ExpirationDate { get; set; }
  }
}