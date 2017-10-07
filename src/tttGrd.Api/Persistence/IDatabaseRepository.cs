using System.Collections.Generic;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public interface IDatabaseRepository
  {
    Task AddAgniKaiAsync(AgniKai agniKai);
    Task<AgniKai> GetAgniKaiByTicket(string ticket);
    Task AddPlayerAsync(string username);
    Task<Player> GetPlayerByNameAsync(string playerName);
    Task<List<Player>> GetUsersAsync();
    Task<State> GetStateAsync(string agniKaiTicket);
    Task RecordMove(string agniKaiTicket, (int Grid, int Cell) move, Field indicator);
  }
}