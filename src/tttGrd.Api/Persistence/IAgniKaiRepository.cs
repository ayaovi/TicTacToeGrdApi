using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IAgniKaiRepository
  {
    Task<string> InitiateAgniKaiAsync();
  }
}