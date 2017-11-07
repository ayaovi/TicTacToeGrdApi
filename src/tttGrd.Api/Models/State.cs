using System;
using System.Collections.Generic;
using System.Linq;

namespace tttGrd.Api.Models
{
  public class State
  {
    public Field[][] Fields { get; set; } = {
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty},
        new[] {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty}
      };

    public State() { }

    public State(State state)
    {
      for (var i = 0; i < 9; i++)
      {
        for (var j = 0; j < 9; j++)
        {
          Fields[i][j] = state.Fields[i][j];
        }
      }
    }

    public State(IEnumerable<string> strState)
    {
      var enumerable = strState as string[] ?? strState.ToArray();

      for (var i = 0; i < enumerable.Length; i++)
      {
        var miniGrid = enumerable[i].Split('|');

        for (var j = 0; j < miniGrid.Length; j++)
        {
          for (var k = 0; k < miniGrid[j].Length; k++)
          {
            switch (miniGrid[j][k])
            {
              case 'x':
                Fields[i][j * 3 + k] = Field.X;
                break;
              case 'o':
                Fields[i][j * 3 + k] = Field.O;
                break;
            }
          }
        }
      }
    }

    public IEnumerable<Field> FlattenedFields() => Fields.SelectMany(x => x);

    public static string FieldToString(Field field)
    {
      switch (field)
      {
        case Field.X:
          return "x";
        case Field.O:
          return "o";
        default:
          return ".";
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is State state)) return false;
      return state.FlattenedFields()
                  .Zip(FlattenedFields(), Tuple.Create)
                  .All(x => x.Item1 == x.Item2);
    }

    public override string ToString()
    {
      return string.Join("@", Fields.Select(row => string.Concat(row.Select((x, i) => i == 2 || i == 5 ? FieldToString(x) + "|" : FieldToString(x)))));
    }
  }
}