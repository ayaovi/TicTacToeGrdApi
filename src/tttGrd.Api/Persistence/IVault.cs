using System.Collections.Generic;
using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IVault
  {
    Task AddAgniKaiTicket(string key);
    Task AddGameTokenAsync(Token gameToken);
    Task<Token> GetGameTokenAsync(string value);
    Task<IList<string>> GetGamerKeysAsync();
  }
}