using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Hubs
{
  [HubName("gameHub")]
  public class GameHub : Hub
  {
    private readonly IDatabaseRepository _database;

    public GameHub(IDatabaseRepository database)
    {
      _database = database;
    }

    public void Announce(string username)
    {
      _database.AddUserAsync(username);
    }

    public async Task JoinAgniKai(string agniKaiTicket)
    {
      await Groups.Add(Context.ConnectionId, agniKaiTicket);
      //Clients.Group(agniKaiTicket).addChatMessage(Context.User.Identity.Name + " joined.");
    }

    public Task LeaveAgniKai(string agniKaiTicket)
    {
      return Groups.Remove(Context.ConnectionId, agniKaiTicket);
    }

    public void SendMove(string agniKaiTicket, (int Grid, int Cell) move)
    {
      //TODO perform some computation, then broadcast the state back to all clients in the group.
      //Clients.All.broadcastMessage(agniKaiTicket, move);
      //We might want to encrypt the state being published a save it with a time stamp.
      Clients.Group(agniKaiTicket).broadcastState(agniKaiTicket, move);
    }
  }
}