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
        // must play in mini-board oponentMove.Cell.
        var possibleMoves = Program.GetPossibleMoves(GameState.Fields[oponentMove.Cell]).ToArray();
        var optimalMoves = possibleMoves.Where(x => !Program.IsWin(GameState.Fields[x])).ToArray();
        return optimalMoves.Any() ? (oponentMove.Cell, optimalMoves[new Random().Next(optimalMoves.Length)]) :
          (oponentMove.Cell, possibleMoves[new Random().Next(possibleMoves.Length)]);
      }
      //var historicalMoves = History.Where(play => play.Outcome == 1 && play.Has(GameState)).ToList();

      //if (historicalMoves.Any())
      //{
      //  return historicalMoves.FirstOrDefault().Turns.Find(turn => turn.GameState.Equals(GameState)).Move;
      //}

      //var opponentWinningPaths = TicTacToe.GetWinningPaths(GameState, Oponent);

      //if (opponentWinningPaths.Count != 0)
      //{
      //  var oponentEminentWiningPaths = opponentWinningPaths.Where(path => path.Length == 1).ToList();
      //  var myWinningPaths = TicTacToe.GetWinningPaths(GameState, Indicator);
      //  var myEminentWinningPaths = myWinningPaths.Where(path => path.Length == 1).ToList();

      //  if (myEminentWinningPaths.Count != 0) return myEminentWinningPaths[0][0];

      //  if (oponentEminentWiningPaths.Count != 0) return oponentEminentWiningPaths[0][0];
      //  {
      //    if (myWinningPaths.Count != 0) return myWinningPaths[0][new Random().Next(myWinningPaths[0].Length)];
      //  }
      //}
      //var moves = TicTacToe.GetPossibleMoves(GameState).ToList();
      //return moves[new Random().Next(moves.Count)];
      //return (0, 0);
    }

    public (int Grid, int Cell) MakeMove((int Grid, int Cell) oponentMove) => SelectOptimalMove(oponentMove);
    public (int Grid, int Cell) MakeMove() => (0, 0);
  }
}