using System;
using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class GamerRepository : IGamerRepository
  {
    private readonly IDatabaseRepository _databaseRepository;

    public GamerRepository(IDatabaseRepository databaseRepository)
    {
      _databaseRepository = databaseRepository;
    }

    public async Task<string> CreateGamerAsync(string agniKaiTicket)
    {
      var agniKai = await _databaseRepository.GetAgniKaiByTicket(agniKaiTicket);
      if (!agniKai.CanAccommodateGamer()) throw new Exception($"AgniKai with ticket {agniKaiTicket} is full.");
      var gamer = new AI
      {
        Name = $"Gamer_{agniKai.GetNextGamerId()}",
        AgniKaiTicket = agniKaiTicket
      };
      agniKai.AddGamer(gamer);
      return gamer.Name;
    }

    public async Task CreateGamerWithNameAsync(string agniKaiTicket, string name)
    {
      var agniKai = await _databaseRepository.GetAgniKaiByTicket(agniKaiTicket);
      if (!agniKai.CanAccommodateGamer()) throw new Exception($"AgniKai with ticket {agniKaiTicket} is full.");
      var gamer = new AI { Name = name };
      agniKai.AddGamer(gamer);
    }
  }
}