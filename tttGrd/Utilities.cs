using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd
{
  public static class Utilities
  {
    public static IEnumerable<T> Copy<T>(this IEnumerable<T> input, Func<T, T> duplicator) => input.Select(duplicator);

    public static IEnumerable<T> Maxs<T>(this IEnumerable<T> input, Func<T, T, bool> comparator)
    {
      var enumerable = input as IList<T> ?? input.ToList();
      if (!enumerable.Any()) return Enumerable.Empty<T>();
      if (enumerable.Count == 1) return enumerable;
      var max = enumerable.Max();
      return enumerable.Where(x => comparator(x, max));
    }

    public static T Random<T>(this IEnumerable<T> input)
    {
      var enumerable = input as IList<T> ?? input.ToList();
      if (!enumerable.Any()) return default(T);
      if (enumerable.Count == 1) return enumerable.FirstOrDefault();
      var index = new Random().Next(enumerable.Count);
      return enumerable[index];
    }

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

    public static float[][] GetCellsProbabilities(IEnumerable<Move> moves, State state)
    {
      var prob = GetDefaultCellsProbabilities();

      moves.ToList().ForEach(move =>
      {
        if (state.Fields[move.Value.Grid][move.Value.Cell] == Field.Empty)
          state.Fields[move.Value.Grid][move.Value.Cell] = move.Indicator;
        prob = Program.UpdateCellsProbabilities(prob, state, move.Value);
      });
      return prob;
    }
  }
}