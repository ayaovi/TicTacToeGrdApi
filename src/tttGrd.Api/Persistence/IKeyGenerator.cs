using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public interface IKeyGenerator
  {
    Task<string> GenerateKey();
  }
}