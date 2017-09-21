using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class DatabaseRepository : IDatabaseRepository
  {
    private readonly List<Gamer> _gamers = new List<Gamer>();

    public Task AddGamerAsync(Gamer gamer)
    {
      _gamers.Add(gamer);
      return Task.CompletedTask;
    }

    public Task<Gamer> GetGamerByTokenAsync(string gamerToken)
    {
      return Task.FromResult(_gamers.Single(gamer => gamer.Token == gamerToken));
    }
  }
}