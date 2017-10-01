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
      var result = await _database.GetUsersAsync();
      return Ok(result);
    }
  }
}
