using System;
using System.Linq;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Models
{
  public class Gamer
  {
    public Field Indicator { get; set; }
    public string Name { get; set; }
    public string AgniKaiTicket { get; set; }
  }

  public class Player : Gamer
  {
    public PlayerStatus Status { get; set; }
    public string GameToken { get; set; }
  }

  // ReSharper disable once InconsistentNaming
  public class AI : Gamer
  {
    public State GameState { get; set; } = new State();
    public Field Opponent { get; set; }
    public float[][] CellProbabilities { get; set; } = Utilities.GetDefaultCellsProbabilities();

    public (int Grid, int Cell) MakeProbabilityBasedMove((int Grid, int Cell) oponentMove) => SelectProbabilityBasedOptimalMove(oponentMove);

    private (int Grid, int Cell) SelectProbabilityBasedOptimalMove((int Grid, int Cell) oponentMove)
    {
      var gridIndex = oponentMove.Cell;
      if (Program.IsWin(GameState.Fields[gridIndex]))
      {
        gridIndex = GameState.Fields.Select((field, i) => new { Field = field, Index = i })
                                    .Where(x => !Program.IsWin(x.Field))
                                    .Select(x => x.Index)
                                    .Random();
      }
      var highestProbCell = GetHighestProbabilityCellIndex(gridIndex);
      return (gridIndex, highestProbCell);
    }

    private int GetHighestProbabilityCellIndex(int gridIndex) => CellProbabilities[gridIndex].Select((x, i) => new Cell { Index = i, Probability = x })
                                                                                             .Maxs((x, y) => x.Probability.Equals(y.Probability))
                                                                                             .Random()
                                                                                             .Index;

    public (int Grid, int Cell) MakeMove()
    {
      var gridIndex = new Random().Next(0, 9);
      var cellIndex = GetHighestProbabilityCellIndex(gridIndex);
      return (gridIndex, cellIndex);
    }
  }

  public class Cell : IComparable<Cell>
  {
    public int Index { get; set; }
    public float Probability { get; set; }
    public int CompareTo(Cell obj)
    {
      if (Probability > obj.Probability) return 1;
      return Probability.Equals(obj.Probability) ? 0 : -1;
    }
  }
}