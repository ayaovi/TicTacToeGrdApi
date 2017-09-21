using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public interface IDatabaseRepository
  {
    Task AddGamerAsync(Gamer gamer);
    Task<Gamer> GetGamerByTokenAsync(string gamerToken);
  }
}