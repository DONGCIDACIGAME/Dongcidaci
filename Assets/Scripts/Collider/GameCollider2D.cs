using UnityEngine;

public abstract class GameCollider2D : IGameCollider2D
{
    private GameColliderData2D _colliderData;
    private Vector2 _pos;

    protected GameCollider2D(GameColliderData2D colliderData)
    {
        this._colliderData = colliderData;
    }

    public void SetPos(Vector2 pos)
    {
        this._pos = pos;
    }

    public Vector2 GetPos()
    {
        return _pos;
    }

    public GameColliderData2D GetColliderData()
    {
        return _colliderData;
    }

    public bool CheckCollapse(Vector2 pos, Vector2 size, Vector2 offset)
    {
        float offsetX = Mathf.Abs((_pos.x + _colliderData.offset.x) - (pos.x + offset.x));
        float offsetY = Mathf.Abs((_pos.y + _colliderData.offset.y) - (pos.y + offset.y));
        return offsetX <= size.x / 2 + _colliderData.size.x / 2
            && offsetY <= size.y / 2 + _colliderData.size.y / 2;
    }

    public bool CheckCollapse(Rect area)
    {
        Vector2 pos = new Vector2(area.x, area.y);
        Vector2 size = new Vector2(area.width, area.height);
        return CheckCollapse(pos, size, Vector2.zero);
    }

    public bool CheckCollapse(GameCollider2D other)
    {
        return CheckCollapse(other._pos, other._colliderData.size, other._colliderData.offset);
    }

    public bool CheckPosInCollider(Vector2 pos)
    {
        return pos.x >= _pos.x + _colliderData.offset.x - _colliderData.size.x / 2 && pos.x <= _pos.x + _colliderData.offset.x + _colliderData.size.x / 2
            && pos.y >= _pos.y + _colliderData.offset.y - _colliderData.size.y / 2 && pos.y <= _pos.y + _colliderData.offset.y + _colliderData.size.y / 2;
    }

    public abstract int GetColliderType();

    public abstract void OnColliderEnter(IGameCollider other);
}
