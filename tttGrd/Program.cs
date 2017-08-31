using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tttGrd
{
  class Program
  {
    static void Main(string[] args)
    {
      var ai = new Gamer { Indicator = Field.X, Name = "AI", Oponent = Field.O };
      var player = new Gamer { Indicator = Field.O, Name = "PLAYER", Oponent = Field.X };

      var i = 0;

      while (i < 10)
      {
        ai.History.Add(new Play());
        var state = new State();
        var currentPlayer = new Random().Next(2);

        var move = (0, 0);
        var pastFirstPlay = false;

        while (!IsWin(state) && !IsFull(state))
        {
          Console.Clear();
          DisplayBoard(state);
          var name = currentPlayer == 0 ? player.Name : ai.Name;

          if (pastFirstPlay) Console.WriteLine($"Openent's move was: {move}\n");

          var message = $"{name}'s turn: ";
          Console.Write(message);

          ai.GameState = state;

          if (!pastFirstPlay) move = currentPlayer == 0 ? MakeMove() : ai.MakeMove();
          else move = currentPlayer == 0 ? MakeMove() : ai.MakeMove(move);

          if (currentPlayer == 1)
          {
            Console.WriteLine(move);
            Console.ReadLine();
          }

          var indicator = currentPlayer == 0 ? player.Indicator : ai.Indicator;

          try
          {
            state = Play(state, move, indicator);
            ai.History[ai.History.Count - 1].Turns.Add(new Turn { GameState = ai.GameState, Move = move });
            currentPlayer = (currentPlayer + 1) % 2;
          }
          catch (Exception e)
          {
            continue;
          }

          pastFirstPlay = true;
        }

        Console.Clear();
        DisplayBoard(state);

        if (!IsFull(state))
        {
          if (currentPlayer == 0)
          {
          }
          else
          {
            ai.History[ai.History.Count - 1].Outcome = 1;
          }
          Console.WriteLine(currentPlayer == 0 ? "PLAYER Won." : "AI Won.");
        }
        else
        {
          Console.WriteLine("It is a Tie");
        }

        Console.ReadLine();
        ++i;
      }
    }

    private static (int Grid, int Cell) MakeMove()
    {
      var move = Console.ReadLine()?.Trim().Split(',').Select(int.Parse).ToArray();
      return (Grid: move.First(), Cell: move.Last());
    }

    private static void DisplayBoard(State state)
    {
      var board = new StringBuilder();

      board.AppendLine("+--- --- ---+--- --- ---+--- --- ---+");

      var strState = state.ToString().ToUpper().Split('@').Select(str => string.Concat(str.Split('|')).ToCharArray()).ToArray();

      for (var i = 0; i < strState.Length; i += 3)
      {
        var s = strState.Select((grid, k) => new { Index = k, Grid = grid })
            .Where(elmt => elmt.Index >= i && elmt.Index < (i + 3))
            .Select(elmt => elmt.Grid)
            .ToArray();

        for (var j = 0; j < 9; j += 3)
        {
          board.AppendLine("| " + string.Join(" | ", s.Select(grid => grid[j] + " | " + grid[j + 1] + " | " + grid[j + 2])) + " |");
          if (j <= 3) board.AppendLine("|--- --- ---|--- --- ---|--- --- ---|");
        }
        board.Append("+--- --- ---+--- --- ---+--- --- ---+\n");
      }

      Console.WriteLine(board.ToString());
    }

    public static State Play(State currentState, (int Grid, int Cell) move, Field tile)
    {
      if (move.Grid < 0 || move.Grid > 8 || move.Cell < 0 || move.Cell > 8) throw new ArgumentException("Invalid Move");

      if (currentState.Fields[move.Grid][move.Cell] != Field.Empty) throw new ArgumentException("Move Already Made");

      var nextState = new State(currentState);
      nextState.Fields[move.Grid][move.Cell] = tile;

      return nextState;
    }

    public static bool IsWin(State state) => state.Fields.All(grid => IsWin(grid));

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
        new[] { 0, 4, 8},   // Backward Diagonal [(0,0) (1,1) (2,2)]
        new[] { 2, 4, 6 },  // Forward Diagonal [(0,2) (1,1) (2,0)]
        new[] { 0, 1, 2 },  // First Horizontal [(0,0) (0,1) (0,2)]
        new[] { 3, 4, 5 },  // Second Horizontal [(1,0) (1,1) (1,2)]
        new[] { 6, 7, 8 },  // Third Horizontal [(2,0) (2,1) (2,2)]
        new[] { 0, 3, 6 },  // First Vertical [(0,0) (1,0) (2,0)]
        new[] { 1, 4, 7 },  // Second Vertical [(0,1) (1,1) (2,1)]
        new[] { 2, 5, 8 }   // Third Vertical [(0,2) (1,2) (2,2)]
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
