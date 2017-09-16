using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd
{
  public static class Utilities
  {
    public static IEnumerable<T> Copy<T>(this IEnumerable<T> input, Func<T, T> duplicator) => input.Select(duplicator);
  }
}