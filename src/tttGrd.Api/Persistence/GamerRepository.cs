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
      var agniKai = await _databaseRepository.GetAgniKaiByTicketAsync(agniKaiTicket);
      if (!agniKai.CanAccommodateGamer()) throw new Exception($"AgniKai with ticket {agniKaiTicket} is full.");
      var indicator = new[] { Field.O, Field.X }[new Random().Next(2)];
      var gamer = new AI
      {
        Name = $"Gamer_{agniKai.GetNextGamerId()}",
        AgniKaiTicket = agniKaiTicket,
        Indicator = indicator
      };
      agniKai.AddGamer(gamer);
      return indicator.ToString().ToLowerInvariant();
    }

    public async Task CreateGamerWithNameAsync(string agniKaiTicket, string name)
    {
      var agniKai = await _databaseRepository.GetAgniKaiByTicketAsync(agniKaiTicket);
      if (!agniKai.CanAccommodateGamer()) throw new Exception($"AgniKai with ticket {agniKaiTicket} is full.");
      var gamer = new AI { Name = name };
      agniKai.AddGamer(gamer);
    }
  }
}