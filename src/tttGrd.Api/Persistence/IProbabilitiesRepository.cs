using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public interface IProbabilitiesRepository
  {
    Task<float[][]> GetCellProbabilitiesAsync(State state);
  }
}