namespace tttGrd.Api.Models
{
  public class Turn
  {
    public State GameState { get; set; }
    public (int, int) Move { get; set; }
  }
}
