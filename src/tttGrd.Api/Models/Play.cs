using System.Collections.Generic;
using System.Linq;

namespace tttGrd.Api.Models
{
  public class Play
  {
    public List<Turn> Turns { get; set; }
    public int Outcome { get; set; } = 0;

    public Play()
    {
      Turns = new List<Turn>();
    }

    public Play(IEnumerable<string[]> states)
    {
      Turns = states.Select(state => new Turn { GameState = new State(state) }).ToList();
    }

    public bool Has(State state) => Turns.Any(turn => turn.GameState.Equals(state));
  }
}
