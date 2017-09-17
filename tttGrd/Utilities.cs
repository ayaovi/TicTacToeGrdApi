using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd
{
  public static class Utilities
  {
    public static IEnumerable<T> Copy<T>(this IEnumerable<T> input, Func<T, T> duplicator) => input.Select(duplicator);
    public static float[][] GetDefaultCellsProbabilities()
    {
      return new[]
      {
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 0f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f},
        new[] {1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f, 1f, 1f / 9f, 1f / 9f, 1f / 9f, 1f / 9f}
      };
    }

    public static (float[][], float[][]) GetCellsProbabilities(IEnumerable<Move> moves, State state)
    {
      var prob1 = GetDefaultCellsProbabilities();
      var prob2 = GetDefaultCellsProbabilities();

      moves.ToList().ForEach(move =>
      {
        var oponent = move.Indicator == Field.O ? Field.X : Field.O;
        prob1 = Program.UpdateCellsProbabilities(prob1, state, move.Value, move.Indicator);
        prob2 = Program.UpdateCellsProbabilities(prob2, state, move.Value, oponent);
      });
      return (prob1, prob2);
    }
  }
}