using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public class Vault : IVault
  {
    public IList<string> GamerKeys { get; } = Enumerable.Empty<string>().ToList();
    public Task AddGamerKey(string key)
    {
      GamerKeys.Add(key);
      return Task.CompletedTask;
    }
  }
}