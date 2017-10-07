using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using tttGrd.Api.Models;
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
      _database.AddPlayerAsync(username);
    }

    public async Task JoinAgniKai(string agniKaiTicket)
    {
      await Groups.Add(Context.ConnectionId, agniKaiTicket);
      //Clients.Group(agniKaiTicket).addChatMessage(Context.Player.Identity.Name + " joined.");
    }

    public Task LeaveAgniKai(string agniKaiTicket)
    {
      return Groups.Remove(Context.ConnectionId, agniKaiTicket);
    }

    public void SendMove(string agniKaiTicket, int grid, int cell, char tile)
    {
      Field IndicatorFromTile(char t)
      {
        switch (t)
        {
          case 'x':
            return Field.X;
          case 'o':
            return Field.O;
          default:
            return Field.Empty;
        }
      }
      //TODO perform some computation, then broadcast the state back to all clients in the group.
      //Clients.All.broadcastMessage(agniKaiTicket, move);
      var indicator = IndicatorFromTile(tile);
      _database.RecordMove(agniKaiTicket, (grid, cell), indicator);
      var state = _database.GetStateAsync(agniKaiTicket);
      //We might want to encrypt the state being published and save it with a time stamp.
      Clients.Group(agniKaiTicket).broadcastState(state);
    }
  }
}