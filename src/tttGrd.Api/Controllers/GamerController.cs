using System.Threading.Tasks;
using System.Web.Http;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Controllers
{
  [RoutePrefix("gamer")]
  public class GamerController : ApiController
  {
    private readonly IGamerRepository _gamerRepository;

    public GamerController(IGamerRepository gamerRepository)
    {
      _gamerRepository = gamerRepository;
    }

    [HttpPost]
    [Route("create/ai")]
    public async Task<IHttpActionResult> Create([FromBody] SubmissionRequest request)
    {
      var ticket = request.Ticket;
      await _gamerRepository.CreateGamerAsync(ticket);
      return Ok();
    }
  }
}