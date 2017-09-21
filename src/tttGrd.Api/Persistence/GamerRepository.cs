using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public class GamerRepository : IGamerRepository
  {
    private readonly IKeyGenerator _keyGenerator;
    private readonly IVault _vault;

    public GamerRepository(IKeyGenerator keyGenerator, IVault vault)
    {
      _keyGenerator = keyGenerator;
      _vault = vault;
    }

    public async Task<string> CreateGamerAsync()
    {
      var key = await _keyGenerator.GenerateKey();
      await _vault.AddGamerKey(key);
      //TODO create the gamer.
      return key;
    }
  }
}