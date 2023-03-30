using GameEngine;

public class GameEventArgs_KV_String_Int : GameEventArgs
{
    public string Key;
    public int Value;

    public GameEventArgs_KV_String_Int(string key,int value)
    {
        this.Key = key;
        this.Value = value;
    }
}
