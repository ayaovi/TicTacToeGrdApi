using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace tttGrd.Api.Hubs
{
  public class GameHub : Hub
  {
    public Task JoinAgniKai(string agniKaiTicket)
    {
      return Groups.Add(Context.ConnectionId, agniKaiTicket);
    }

    public Task LeaveAgniKai(string agniKaiTicket)
    {
      return Groups.Remove(Context.ConnectionId, agniKaiTicket);
    }
  }
}