using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class ProbabilitiesRepository : IProbabilitiesRepository
  {
    public Task<float[][]> GetCellProbabilitiesAsync(State state)
    {
      throw new System.NotImplementedException();
    }
  }
}