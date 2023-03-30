using GameEngine;

public class GameEventArgs_String : GameEventArgs
{
    public string Value;
    public GameEventArgs_String(string value)
    {
        this.Value = value;
    }
}