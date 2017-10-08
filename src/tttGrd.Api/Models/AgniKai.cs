using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd.Api.Models
{
  public class AgniKai
  {
    public string Ticket { get; set; }
    private readonly IList<Gamer> _gamers = new List<Gamer>();

    public bool AddGamer(Gamer gamer)
    {
      if (!CanAccommodateGamer()) return false;
      _gamers.Add(gamer);
      return true;
    }

    public Gamer GetGamerByName(string name) =>_gamers.FirstOrDefault(gamer => string.Equals(gamer.Name, name, StringComparison.InvariantCultureIgnoreCase));

    public bool CanAccommodateGamer() => _gamers.Count < 2;

    public int GetNextGamerId() =>_gamers.Count + 1;
  }
}