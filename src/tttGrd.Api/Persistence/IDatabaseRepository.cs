﻿using System.Collections.Generic;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public interface IDatabaseRepository
  {
    Task AddAgniKaiAsync(AgniKai agniKai);
    Task<AgniKai> GetAgniKaiByTicket(string ticket);
    Task AddUserAsync(string username);
    Task<User> GetUserByNameAsync(string username);
    Task<List<User>> GetUsersAsync();
    Task<State> GetStateAsync(string agniKaiTicket);
    Task RecordMove(string agniKaiTicket, (int Grid, int Cell) move, Field indicator);
  }
}