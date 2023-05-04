using UnityEngine;

public interface IGameCollider2D : IGameCollider
{
    bool CheckPosInCollider(Vector2 pos);
    bool CheckCollapse(Rect rect);
}
