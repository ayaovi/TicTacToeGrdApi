namespace tttGrd.Api.Models
{
  public class Move
  {
    public (int Grid, int Cell) Value { get; set; }
    public Field Indicator { get; set; }
  }
}