using System.Threading.Tasks;
using System.Web.Http;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Controllers
{
  [RoutePrefix("gamer/")]
  public class GamerController : ApiController
  {
    private readonly IGamerRepository _gamerRepository;

    public GamerController(IGamerRepository gamerRepository)
    {
      _gamerRepository = gamerRepository;
    }

    [HttpGet]
    [Route("/create")]
    public async Task<IHttpActionResult> Create()
    {
      var result = await _gamerRepository.CreateGamerAsync();
      return Ok(result);
    }
  }
}
