using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IGamerRepository
  {
    Task CreateGamerAsync(string agniKaiTicket);
    Task CreateGamerWithNameAsync(string agniKaiTicket, string name);
  }
}