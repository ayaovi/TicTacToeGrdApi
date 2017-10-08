using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class AgniKaiRepository : IAgniKaiRepository
  {
    private readonly IVault _vault;
    private readonly IKeyGenerator _keyGenerator;
    private readonly IDatabaseRepository _database;

    public AgniKaiRepository(IVault vault, IKeyGenerator keyGenerator, IDatabaseRepository database)
    {
      _vault = vault;
      _keyGenerator = keyGenerator;
      _database = database;
    }

    public async Task<string> InitiateAgniKaiAsync()
    {
      var ticket = await _keyGenerator.GenerateKey();
      await _vault.AddAgniKaiTicket(ticket);
      await _database.AddAgniKaiAsync(new AgniKai { Ticket = ticket });
      return ticket;
    }
  }
}