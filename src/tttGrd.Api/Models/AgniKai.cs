using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd.Api.Models
{
  public class AgniKai
  {
    public string Ticket { get; set; }
    private readonly IList<AI> _gamers = new List<AI>();

    public bool AddGamer(AI gamer)
    {
      if (!CanAccommodateGamer()) return false;
      _gamers.Add(gamer);
      return true;
    }

    public AI GetGamerByName(string name) =>_gamers.FirstOrDefault(gamer => string.Equals(gamer.Name, name, StringComparison.InvariantCultureIgnoreCase));

    public bool CanAccommodateGamer() => _gamers.Count < 2;

    public int GetNextGamerId() =>_gamers.Count + 1;
  }
}