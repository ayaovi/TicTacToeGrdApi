using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IVault
  {
    Task AddAgniKaiTicket(string key);
  }
}