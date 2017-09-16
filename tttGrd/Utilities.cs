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
  }
}