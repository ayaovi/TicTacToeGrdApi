using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using tttGrd.Api.Models;

namespace tttGrd.Api.Persistence
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

    public static string GetMd5Hash(this string input)
    {
      string hash;
      using (var md5Hash = MD5.Create())
      {
        var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        hash = string.Concat(data.Select(x => x.ToString("x2")));
      }
      return hash;
    }

    public static Field IndicatorFromTile(char t)
    {
      switch (t)
      {
        case 'x':
          return Field.X;
        case 'o':
          return Field.O;
        default:
          return Field.Empty;
      }
    }

    public static T Random<T>(this IEnumerable<T> input)
    {
      var enumerable = input as IList<T> ?? input.ToList();
      if (!enumerable.Any()) return default(T);
      if (enumerable.Count == 1) return enumerable.FirstOrDefault();
      var index = new Random().Next(enumerable.Count);
      return enumerable[index];
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> input)
    {
      var list = input.ToList();
      var rand = new Random();
      for (var i = 0; i < list.Count; i++)
      {
        var j = rand.Next(i);
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
      }
      return list;
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