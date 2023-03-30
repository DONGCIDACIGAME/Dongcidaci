using GameEngine;

public class GameEventArgs_Bool : GameEventArgs
{
    public bool Value;
    public GameEventArgs_Bool(bool value)
    {
        this.Value = value;
    }
}
