using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd
{
  public class Gamer
  {
    public Field Indicator { get; set; }
    public string Name { get; set; }
    public State GameState { get; set; }
    public Field Oponent { get; set; }
    public List<Play> History { get; set; } = new List<Play>();

    private (int Grid, int Cell) SelectOptimalMove((int Grid, int Cell) oponentMove)
    {
      if (Program.IsWin(GameState.Fields[oponentMove.Cell]))
      {
        // play anywhere else.
        var possibleGridIndices = Enumerable.Range(0, 9)
          .Where(x => x != oponentMove.Cell && !Program.IsWin(GameState.Fields[x])).ToArray();
        var selectedGridIndex = possibleGridIndices[new Random().Next(possibleGridIndices.Length)];
        var possibleCellIndices = GetOptimalCellIndices(Program.GetPossibleMoves(GameState.Fields[selectedGridIndex])).ToArray();
        return (selectedGridIndex, possibleCellIndices[new Random().Next(possibleCellIndices.Length)]);
      }
      else
      {
        // must play in grid corresponding to oponentMove.Cell.
        var currentGrid = GameState.Fields[oponentMove.Cell];
        var opponentWinningPaths = Program.GetWinningPaths(currentGrid, Oponent); // check for oponent's winning paths.
        var oponentEminentWinningPaths = opponentWinningPaths.Where(path => path.Length == 1).ToList();  // check oponent's winning paths.

        if (oponentEminentWinningPaths.Any()) return (oponentMove.Cell, oponentEminentWinningPaths.First().FirstOrDefault());

        var myWinningPaths = Program.GetWinningPaths(currentGrid, Indicator);  // check for my winning paths.
        var myEminentWinningPaths = myWinningPaths.Where(path => path.Length == 1).ToList();  // check eminent winning paths.

        if (myEminentWinningPaths.Any()) return (oponentMove.Cell, myEminentWinningPaths.First().FirstOrDefault());

        if (myWinningPaths.Any()) return (oponentMove.Cell, myWinningPaths.First().FirstOrDefault()); // is the first always best? or should be assigning scores here?

        var possibleMoves = Program.GetPossibleMoves(currentGrid).ToArray();
        var optimalMoves = GetOptimalCellIndices(possibleMoves).ToArray();
        return optimalMoves.Any() ? (oponentMove.Cell, optimalMoves[new Random().Next(optimalMoves.Length)]) :
          (oponentMove.Cell, possibleMoves[new Random().Next(possibleMoves.Length)]);
      }
    }

    private IEnumerable<int> GetOptimalCellIndices(IEnumerable<int> cellIndices)
    {
      return cellIndices.Where(x => !Program.IsWin(GameState.Fields[x]))  // strip away already won grid.
                        .Where(x => Program.GetWinningPaths(GameState.Fields[x], Oponent).All(path => path.Length != 1))  // strip away grids where openent is about to win.
                        .Where(x => IHaveWinningChance(x, (a, b) => a >= b)) // strip away grids where openent's winning chances are higher than mine.
                        .Where(x => !HasEminentWinning(GameState.Fields[x], Indicator)); // strip away grids wher my winning is eminent.
    }

    private bool HasEminentWinning(IEnumerable<Field> grid, Field indicator) => Program.GetWinningPaths(grid, indicator).Where(path => path.Length == 1).Any();

    private bool IHaveWinningChance(int gridIndex, Func<int, int, bool> comparator)
    {
      return comparator(Program.GetWinningPaths(GameState.Fields[gridIndex], Indicator).Count, Program.GetWinningPaths(GameState.Fields[gridIndex], Oponent).Count);
    }

    public (int Grid, int Cell) MakeMove((int Grid, int Cell) oponentMove) => SelectOptimalMove(oponentMove);
    public (int Grid, int Cell) MakeMove() => (0, 0);
  }
}