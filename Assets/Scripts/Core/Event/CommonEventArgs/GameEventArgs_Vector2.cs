using GameEngine;
using UnityEngine;

public class GameEventArgs_Vector2 : GameEventArgs
{
    public Vector2 Value;
    public GameEventArgs_Vector2(Vector2 value)
    {
        this.Value = value;
    }
}
