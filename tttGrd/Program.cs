using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }

    public static State Play(State currentState, int move, Field tile)
    {
      if (move < 0 || move > 8) throw new ArgumentException("Invalid Move");

      if (currentState.Fields[move / 3][move % 3] != Field.Empty) throw new ArgumentException("Move Already Made");

      var nextState = new State(currentState);
      nextState.Fields[move / 3][move % 3] = tile;

      return nextState;
    }

    public static bool IsWin(IEnumerable<Field> grid)
    {
      var enumerable = grid as Field[] ?? grid.ToArray();

      var backwardDiagonal = enumerable[0] != Field.Empty && enumerable[0] == enumerable[4] && enumerable[4] == enumerable[8];
      var forwardDiagonal = enumerable[2] != Field.Empty && enumerable[2] == enumerable[4] && enumerable[4] == enumerable[6];
      var firstHorizontal = enumerable[0] != Field.Empty && enumerable[0] == enumerable[1] && enumerable[1] == enumerable[2];
      var secondHorizontal = enumerable[3] != Field.Empty && enumerable[3] == enumerable[4] && enumerable[4] == enumerable[5];
      var thirdHorizontal = enumerable[6] != Field.Empty && enumerable[6] == enumerable[7] && enumerable[7] == enumerable[8];
      var firstVertical = enumerable[0] != Field.Empty && enumerable[0] == enumerable[3] && enumerable[3] == enumerable[6];
      var secondVertical = enumerable[1] != Field.Empty && enumerable[1] == enumerable[4] && enumerable[4] == enumerable[7];
      var thirdVertical = enumerable[2] != Field.Empty && enumerable[2] == enumerable[5] && enumerable[5] == enumerable[8];

      return backwardDiagonal || forwardDiagonal ||
        firstHorizontal || secondHorizontal || thirdHorizontal ||
        firstVertical || secondVertical || thirdVertical;
    }

    public static bool IsEmpty(State state) => state.Fields.SelectMany(grid => grid).All(cell => cell == Field.Empty);

    public static bool IsFull(State state) => state.Fields.SelectMany(grid => grid).All(cell => cell != Field.Empty);

    public static List<int[]> GetWinningPaths(IEnumerable<Field> grid, Field tile)
    {
      var winningPaths = new List<int[]>();
      var allWinningPaths = new[]
      {
        //Backward Diagonal [(0,0) (1,1) (2,2)]
        new[] { 0, 4, 8},
        //Forward Diagonal [(0,2) (1,1) (2,0)]
        new[] { 2, 4, 6 },
        // First Horizontal [(0,0) (0,1) (0,2)]
        new[] { 0, 1, 2 },
        // Second Horizontal [(1,0) (1,1) (1,2)]
        new[] { 3, 4, 5 },
        // Third Horizontal [(2,0) (2,1) (2,2)]
        new[] { 6, 7, 8 },
        // First Vertical [(0,0) (1,0) (2,0)]
        new[] { 0, 3, 6 },
        // Second Vertical [(0,1) (1,1) (2,1)]
        new[] { 1, 4, 7 },
        // Third Vertical [(0,2) (1,2) (2,2)]
        new[] { 2, 5, 8 }
      };

      var enumerable = grid as Field[] ?? grid.ToArray();

      for (var i = 0; i < enumerable.Length; i++)
      {
        if (enumerable[i] == Field.Empty) continue;

        winningPaths.AddRange(from path in allWinningPaths
                              where path.Contains(i)
                              let isWinningPath =
                              (enumerable[path[0]] == Field.Empty || enumerable[path[0]] == tile) &&
                              (enumerable[path[1]] == Field.Empty || enumerable[path[1]] == tile) &&
                              (enumerable[path[2]] == Field.Empty || enumerable[path[2]] == tile)
                              where isWinningPath
                              select path.Where(x => enumerable[x] == Field.Empty && x != i).ToArray());
      }

      return winningPaths;
    }

    public static IEnumerable<int> GetPossibleMoves(IEnumerable<Field> grid) =>
      grid.Select((cell, index) => new { cell, index })
           .Where(tile => tile.cell == Field.Empty)
           .Select(tile => tile.index);
  }
}
