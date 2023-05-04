using UnityEngine;

public class GameColliderData2D
{
    public Vector2 size { get; private set; }
    public Vector2 offset { get; private set; }

    public GameColliderData2D(Vector2 size, Vector2 offset)
    {
        this.size = size;
        this.offset = offset;
    }
}
