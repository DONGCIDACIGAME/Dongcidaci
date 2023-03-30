using GameEngine;

public class GameEventArgs_Object: GameEventArgs
{
    public object Value;
    public GameEventArgs_Object(object arg)
    {
        this.Value = arg;
    }
}
