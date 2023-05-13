using UnityEngine;

public interface IGameCollider
{
    int GetColliderType();
    void OnColliderEnter(IGameCollider other);
}


public interface IGameCollider2D : IGameCollider
{
    bool CheckPosInCollider(Vector2 pos);
    bool CheckCollapse(Rect rect);
}






