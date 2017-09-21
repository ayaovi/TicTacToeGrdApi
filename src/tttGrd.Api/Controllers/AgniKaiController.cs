using System.Threading.Tasks;
using System.Web.Http;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Controllers
{
  [RoutePrefix("agniKai")]
  public class AgniKaiController : ApiController
  {
    private readonly IAgniKaiRepository _agniKaiRepository;

    public AgniKaiController(IAgniKaiRepository agniKaiRepository)
    {
      _agniKaiRepository = agniKaiRepository;
    }

    [HttpGet]
    [Route("initiate")]
    public async Task<IHttpActionResult> Initiate()
    {
      var ticket = await _agniKaiRepository.InitiateAgniKaiAsync();
      return Ok(ticket);
    }
  }
}