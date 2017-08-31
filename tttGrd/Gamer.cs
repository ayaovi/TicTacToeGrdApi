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
        .Where(x => x != oponentMove.Cell && !Program.IsWin(GameState.Fields[x]))
        .ToArray();
        var selectedGridIndex = possibleGridIndices[new Random().Next(possibleGridIndices.Length)];
        var possibleMoves = Program.GetPossibleMoves(GameState.Fields[selectedGridIndex])
        .ToArray();
        return (selectedGridIndex, possibleMoves[new Random().Next(possibleMoves.Length)]);
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

        var possibleMoves = Program.GetPossibleMoves(currentGrid).ToArray();
        var optimalMoves = possibleMoves.Where(x => !Program.IsWin(GameState.Fields[x])).ToArray();
        return optimalMoves.Any() ? (oponentMove.Cell, optimalMoves[new Random().Next(optimalMoves.Length)]) :
          (oponentMove.Cell, possibleMoves[new Random().Next(possibleMoves.Length)]);
      }
    }

    public (int Grid, int Cell) MakeMove((int Grid, int Cell) oponentMove) => SelectOptimalMove(oponentMove);
    public (int Grid, int Cell) MakeMove() => (0, 0);
  }
}