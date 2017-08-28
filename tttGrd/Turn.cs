namespace tttGrd
{
  public class Turn
  {
    public State GameState { get; set; }
    public (int, int) Move { get; set; }

    public override bool Equals(object obj)
    {
      var turn = obj as Turn;
      if (turn == null) return false;
      return GameState.Equals(turn.GameState) && Move.Equals(turn.Move);
    }
  }
}
