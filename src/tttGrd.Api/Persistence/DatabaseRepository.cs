using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public enum PlayerStatus
  {
    Online, Offline, Playing
  }

  public class DatabaseRepository : IDatabaseRepository
  {
    private readonly IVault _vault;
    private readonly IKeyGenerator _keyGenerator;
    private readonly List<AgniKai> _agniKais = new List<AgniKai>();
    private readonly List<Player> _players = new List<Player>();
    private readonly IDictionary<string, State> _ongoingGameStates = new Dictionary<string, State>();
    private readonly IDictionary<string, string> _connections = new Dictionary<string, string>();

    public DatabaseRepository(IVault vault, IKeyGenerator keyGenerator)
    {
      _vault = vault;
      _keyGenerator = keyGenerator;
    }

    public Task AddAgniKaiAsync(AgniKai agniKai)
    {
      _agniKais.Add(agniKai);
      _ongoingGameStates.Add(agniKai.Ticket, new State());
      return Task.CompletedTask;
    }

    public Task<AgniKai> GetAgniKaiByTicketAsync(string ticket)
    {
      return Task.FromResult(_agniKais.Single(agniKai => agniKai.Ticket == ticket));
    }

    public async Task<Token> AddPlayerAsync(string username)
    {
      var gameTokenValue = await _keyGenerator.GenerateGameTokenAsync(username);
      var token = new Token
      {
        Value = gameTokenValue,
        ExpirationDate = DateTime.UtcNow.Add(new TimeSpan(0, 0, 30, 0)) /* 30 min in the future. */
      };
      await _vault.AddGameTokenAsync(token);
      _players.Add(new Player
      {
        Name = username,
        Status = PlayerStatus.Online,
        GameToken = gameTokenValue
      });
      return token;
    }

    public Task<Player> GetPlayerByNameAsync(string playerName)
    {
      return Task.FromResult(_players.SingleOrDefault(player => player.Name == playerName));
    }

    public Task<List<Player>> GetPlayersAsync()
    {
      return Task.FromResult(_players);
    }

    public Task<State> GetStateAsync(string agniKaiTicket)
    {
      return Task.FromResult(_ongoingGameStates[agniKaiTicket]);
    }

    public Task RecordMoveAsync(string agniKaiTicket, (int Grid, int Cell) move, Field indicator)
    {
      _ongoingGameStates[agniKaiTicket].Fields[move.Grid][move.Cell] = indicator;
      return Task.CompletedTask;
    }

    public Task<Field> SubmitTicketAsync(string token, string ticket)
    {
      var player = _players.Single(p => p.GameToken == token);
      player.AgniKaiTicket = ticket;
      var agnikai = _agniKais.Single(a => a.Ticket == ticket);
      agnikai.AddGamer(player);
      return Task.FromResult(player.Indicator);
    }

    public Task<int> GetPlayerCountAsync()
    {
      return Task.FromResult(_players.Count);
    }

    public Task AddConnectionAsync(string username, string connectionId)
    {
      _connections.Add(username, connectionId);
      return Task.CompletedTask;
    }

    public Task<string> GetConnectionAsync(string username)
    {
      _connections.TryGetValue(username, out var connectionId);
      return Task.FromResult(connectionId);
    }
  }
}