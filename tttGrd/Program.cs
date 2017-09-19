using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tttGrd
{
  public class Program
  {
    private static void Main(string[] args)
    {
      var ais = new[]
      {
        new Gamer { Indicator = Field.X, Name = "AI 1", Oponent = Field.O },
        new Gamer { Indicator = Field.O, Name = "AI 2", Oponent = Field.X }

      };

      var i = 0;

      while (i < 10)
      {
        var state = new State();
        var probabilities = Utilities.GetDefaultCellsProbabilities();
        var currentGamer = new Random().Next(2);

        var move = (0, 0);
        var pastFirstPlay = false;

        while (!IsWin(state) && !IsFull(state))
        {
          Console.Clear();
          DisplayBoard(state);
          var name = ais[currentGamer].Name;

          if (pastFirstPlay) Console.WriteLine($"Openent's move was: {move}\n");

          var message = $"{name}'s turn: ";
          Console.Write(message);

          ais[currentGamer].GameState = state;

          move = !pastFirstPlay ? ais[currentGamer].MakeMove() : ais[currentGamer].MakeProbabilityBasedMove(move);

          Console.WriteLine(move);
          Console.ReadLine();

          var indicator = ais[currentGamer].Indicator;

          try
          {
            state = Play(state, move, indicator);
            currentGamer = (currentGamer + 1) % 2;
            probabilities = UpdateCellsProbabilities(probabilities, state, move);
            ais[0].CellProbabilities = probabilities;
            ais[1].CellProbabilities = probabilities;
          }
          catch (Exception)
          {
            continue;
          }

          pastFirstPlay = true;
        }

        Console.Clear();
        DisplayBoard(state);

        if (!IsFull(state))
        {
          Console.WriteLine(currentGamer == 0 ? "PLAYER Won." : "AI Won.");
        }
        else
        {
          Console.WriteLine("It is a Tie");
        }

        Console.ReadLine();
        ++i;
      }
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
          var j1 = j;
          board.AppendLine("| " + string.Join(" | ", s.Select(grid => grid[j1] + " | " + grid[j1 + 1] + " | " + grid[j1 + 2])) + " |");
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

    public static bool IsWin(State state) => state.Fields.All(IsWin);

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

    public static bool IsEmpty(IEnumerable<Field> grid) => grid.All(cell => cell == Field.Empty);

    public static bool IsFull(State state) => state.Fields.SelectMany(grid => grid).All(cell => cell != Field.Empty);

    public static List<int[]> GetWinningPaths(IEnumerable<Field> grid, Field tile)
    {
      var winningPaths = new List<int[]>();
      var allWinningPaths = new[]
      {
        new[] { 0, 4, 8 },  // Backward Diagonal [(0,0) (1,1) (2,2)]
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

    public static float[][] UpdateCellsProbabilities(float[][] probs, State state, (int Grid, int Cell) move, Field indicator = Field.Empty)
    {
      var copy = probs.Select(x => x.Select(x1 => x1).ToArray()).ToArray(); /* make a duplicate of cells probabilities. */
      copy[move.Grid][move.Cell] = 0.0f;  /* cell you just played cannot be played again. */
      var winPaths = GetWinningPaths(state.Fields[move.Grid], state.Fields[move.Grid][move.Cell]);
      var eminentWinIndices = winPaths.Where(path => path.Length == 1)
                                      .SelectMany(x => x) /* flatten it out. */
                                      .ToList();
      winPaths.SelectMany(x => x) /* flatten it out. */
              .Where(x => !eminentWinIndices.Contains(x)) /* remove all eniment win indices (because they will be handled seperately). */
              .ToList()
              .ForEach(x => copy[move.Grid][x] += 2f / 9f);

      eminentWinIndices.ForEach(x => copy[move.Grid][x] = 2.0f); /* eminent win index should have 100% probability. */
      
      if (IsWin(state.Fields[move.Grid]) || eminentWinIndices.Any())
      {
        Enumerable.Range(0, 9)
                  .Where(x => x != move.Grid && copy[x][move.Grid] > 0.0f)
                  .ToList()
                  .ForEach(x => copy[x][move.Grid] = 0f);
      }
      else
      {
        Enumerable.Range(0, 9)
                  .Where(x => x != move.Grid && copy[x][move.Grid] > 0.0f)
                  .ToList()
                  .ForEach(x => copy[x][move.Grid] -= 1f / 9f);
      }

      return copy;
    }
  }
}