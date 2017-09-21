using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd.Api.Models
{
  public class AgniKai
  {
    public string Ticket { get; set; }
    private readonly IEnumerable<Gamer> _gamers = new List<Gamer>();

    public bool AddGamer(Gamer gamer)
    {
      return true;
    }

    public Gamer GetGamerByName(string gamerId) =>_gamers.FirstOrDefault(gamer => string.Equals(gamer.Name, gamerId, StringComparison.InvariantCultureIgnoreCase));

    public bool CanAccommodateGamer() => _gamers.Count() < 2;

    public int GetNextGamerId() =>_gamers.Count() + 1;
  }
}