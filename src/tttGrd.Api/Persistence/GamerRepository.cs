using System.Threading.Tasks;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
{
  public class GamerRepository : IGamerRepository
  {
    private readonly IKeyGenerator _keyGenerator;
    private readonly IVault _vault;
    private readonly IDatabaseRepository _databaseRepository;

    public GamerRepository(IKeyGenerator keyGenerator, IVault vault, IDatabaseRepository databaseRepository)
    {
      _keyGenerator = keyGenerator;
      _vault = vault;
      _databaseRepository = databaseRepository;
    }

    public async Task<string> CreateGamerAsync()
    {
      var key = await _keyGenerator.GenerateKey();
      await _vault.AddGamerKey(key);
      var gamer = new Gamer();
      await _databaseRepository.AddGamerAsync(gamer);
      return key;
    }
  }
}