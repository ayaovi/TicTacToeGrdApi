using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class DatabaseRepository : IDatabaseRepository
  {
    private readonly List<AgniKai> _agniKais = new List<AgniKai>();
    
    public Task AddAgniKaiAsync(AgniKai agniKai)
    {
      _agniKais.Add(agniKai);
      return Task.CompletedTask;
    }

    public Task<AgniKai> GetAgniKaiByTicket(string ticket)
    {
      return Task.FromResult(_agniKais.Single(agniKai => agniKai.Ticket == ticket));
    }
  }
}