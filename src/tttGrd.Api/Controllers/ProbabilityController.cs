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

    public async Task<IHttpActionResult> Get()
    {
      return Ok();
    }
  }
}