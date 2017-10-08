using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IKeyGenerator
  {
    Task<string> GenerateKey();
    Task<string> GenerateGameTokenAsync(string username);
  }
}