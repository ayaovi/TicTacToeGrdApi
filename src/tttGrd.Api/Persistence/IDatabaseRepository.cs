﻿using System.Collections.Generic;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public interface IDatabaseRepository
  {
    Task AddAgniKaiAsync(AgniKai agniKai);
    Task<AgniKai> GetAgniKaiByTicketAsync(string ticket);
    Task<Token> AddPlayerAsync(string username);
    Task<Player> GetPlayerByNameAsync(string playerName);
    Task<List<Player>> GetPlayersAsync();
    Task<State> GetStateAsync(string agniKaiTicket);
    Task AddConnectionAsync(string username, string connectionId);
    Task<string> GetConnectionAsync(string username);
    Task RecordMoveAsync(string agniKaiTicket, (int Grid, int Cell) move, Field indicator);
    Task<Field> SubmitTicketAsync(string token, string ticket);
    Task<int> GetPlayerCountAsync();
  }
}