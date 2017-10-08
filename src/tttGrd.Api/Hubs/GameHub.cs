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
      var indicator = IndicatorFromTile(tile);
      _database.RecordMoveAsync(agniKaiTicket, (grid, cell), indicator);
      var state = _database.GetStateAsync(agniKaiTicket);
      
      Clients.Group(agniKaiTicket).broadcastState(state); /* we might want to encrypt the state being published and save it with a time stamp. */
    }
  }
}