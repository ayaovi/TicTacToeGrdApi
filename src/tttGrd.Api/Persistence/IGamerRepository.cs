using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IGamerRepository
  {
    Task<string> CreateGamerAsync();
  }
}