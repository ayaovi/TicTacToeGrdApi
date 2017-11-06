using System;
using System.Threading.Tasks;
using System.Web.Http;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Controllers
{
  [RoutePrefix("users")]
  public class UserController : ApiController
  {
    private readonly IDatabaseRepository _database;
    private readonly IVault _vault;

    public UserController(IDatabaseRepository database, IVault vault)
    {
      _database = database;
      _vault = vault;
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
      var player = await _database.GetPlayerByNameAsync(name);
      if (player == null)
      {
        var token = await _database.AddPlayerAsync(name);
        return Ok(token);
      }
      var oldToken = await _vault.GetGameTokenAsync(player.GameToken);
      return Ok(oldToken);
    }

    [HttpPost]
    [Route("submit")]
    public async Task<IHttpActionResult> SubmitAgniKaiTicket([FromBody] SubmissionRequest request)
    {
      var token = request.Token;
      var ticket = request.Ticket;
      await ValidateToken(token);
      await _database.SubmitTicketAsync(token, ticket);
      return Ok();
    }

    private async Task ValidateToken(string tokenValue)
    {
      var token = await _vault.GetGameTokenAsync(tokenValue);
      if (token.ExpirationDate < DateTime.UtcNow) throw new Exception("Game Token Expired");
    }
  }

  public class SubmissionRequest
  {
    public string Token { get; set; }
    public string Ticket { get; set; }
  }
}
