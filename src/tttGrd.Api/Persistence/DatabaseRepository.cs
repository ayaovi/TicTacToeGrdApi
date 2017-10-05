using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class DatabaseRepository : IDatabaseRepository
  {
    private readonly List<AgniKai> _agniKais = new List<AgniKai>();
    private readonly List<User> _users = new List<User>();
    private readonly IDictionary<string, State> _ongoingGameStates = new Dictionary<string, State>();
    
    public Task AddAgniKaiAsync(AgniKai agniKai)
    {
      _agniKais.Add(agniKai);
      _ongoingGameStates.Add(agniKai.Ticket, new State());
      return Task.CompletedTask;
    }

    public Task<AgniKai> GetAgniKaiByTicket(string ticket)
    {
      return Task.FromResult(_agniKais.Single(agniKai => agniKai.Ticket == ticket));
    }

    public Task AddUserAsync(string username)
    {
      _users.Add(new User {Username = username});
      return Task.CompletedTask;
    }

    public Task<User> GetUserByNameAsync(string username)
    {
      return Task.FromResult(_users.Single(user => user.Username == username));
    }

    public Task<List<User>> GetUsersAsync()
    {
      return Task.FromResult(_users);
    }

    public Task<State> GetStateAsync(string agniKaiTicket)
    {
      return Task.FromResult(_ongoingGameStates[agniKaiTicket]);
    }

    public Task RecordMove(string agniKaiTicket, (int Grid, int Cell) move, Field indicator)
    {
      _ongoingGameStates[agniKaiTicket].Fields[move.Grid][move.Cell] = indicator;
      return Task.CompletedTask;
    }
  }

  public class User
  {
    public string Username { get; set; }
  }
}