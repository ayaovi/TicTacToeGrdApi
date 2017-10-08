using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace tttGrd.Api.Persistence
{
  public class KeyGenerator : IKeyGenerator
  {
    public Task<string> GenerateKey()
    {
      var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
      var key = Guid.NewGuid().ToByteArray();
      return Task.FromResult(Convert.ToBase64String(time.Concat(key).ToArray()));
    }

    public Task<string> GenerateGameTokenAsync(string username)
    {
      return Task.FromResult(string.Concat(username.ToCharArray()
                                    .Concat(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                                                           .ToCharArray())
                                    .Shuffle())
                                    .GetMd5Hash());
    }
  }
}