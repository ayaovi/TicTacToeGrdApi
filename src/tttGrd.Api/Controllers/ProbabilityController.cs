using System.Threading.Tasks;
using System.Web.Http;

namespace tttGrd.Api.Controllers
{
  [RoutePrefix("getCellProbabilities/")]
  public class ProbabilityController : ApiController
  {
    public ProbabilityController()
    {
      
    }
    // GET: api/Probability
    public async Task<IHttpActionResult> Get()
    {
      return Ok();
    }
  }
}