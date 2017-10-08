using System.Threading.Tasks;
using System.Web.Http;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Controllers
{
  [RoutePrefix("users")]
  public class UserController : ApiController
  {
    private readonly IDatabaseRepository _database;

    public UserController(IDatabaseRepository database)
    {
      _database = database;
    }

    [HttpGet]
    [Route("all")]
    public async Task<IHttpActionResult> All()
    {
      var result = await _database.GetPlayersAsync();
      return Ok(result);
    }

    [HttpGet]
    [Route("login")]
    public async Task<IHttpActionResult> Login(string name)
    {
      var token = await _database.AddPlayerAsync(name);
      return Ok(token);
    }

    [HttpPost]
    [Route("submit")]
    public async Task<IHttpActionResult> SubmitAgniKaiTicket(string token, string ticket)
    {
      Validate(token);
      await _database.SubmitTicketAsync(token, ticket);
      return Ok();
    }
  }
}
