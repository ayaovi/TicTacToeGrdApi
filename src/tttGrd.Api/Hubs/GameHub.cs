using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace tttGrd.Api.Hubs
{
  public class GameHub : Hub
  {
    public async Task JoinAgniKai(string agniKaiTicket)
    {
      await Groups.Add(Context.ConnectionId, agniKaiTicket);
      Clients.Group(agniKaiTicket).addChatMessage(Context.User.Identity.Name + " joined.");
    }

    public Task LeaveAgniKai(string agniKaiTicket)
    {
      return Groups.Remove(Context.ConnectionId, agniKaiTicket);
    }

    public void Send(string agniKaiTicket, (int Grid, int Cell) move)
    {
      //TODO perform some computation.
      //Clients.All.broadcastMessage(agniKaiTicket, move);
      Clients.Group(agniKaiTicket).broadcastState(agniKaiTicket, move);
    }
  }
}