using System;
using System.Collections.Generic;
using System.Linq;
using tttGrd.Api.Persistence;

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
      if (gamer.Indicator == Field.Empty)
      {
        AssignIndicator(gamer);
      }
      return true;
    }

    private void AssignIndicator(Gamer gamer)
    {
      if (_gamers.Count == 0)
      {
        gamer.Indicator = new[] { Field.O, Field.X }.Random();
      }
      else
      {
        gamer.Indicator = _gamers[0].Indicator == Field.O ? Field.X : Field.O;
      }
    }

    public Gamer GetGamerByName(string name) => _gamers.FirstOrDefault(gamer => string.Equals(gamer.Name, name, StringComparison.InvariantCultureIgnoreCase));

    public bool CanAccommodateGamer() => _gamers.Count < 2;

    public int GetNextGamerId() => _gamers.Count + 1;

    // ReSharper disable once InconsistentNaming
    public Gamer GetGamerByIndicator(Field indicator)
    {
      return _gamers.Single(gamer => gamer.Indicator == indicator);
    }
  }
}