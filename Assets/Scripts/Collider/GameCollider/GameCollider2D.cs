using UnityEngine;

/// <summary>
/// 用于描述碰撞体区域的位置信息
/// </summary>
public struct ColliderVector3
{
    public float x;
    public float y;
    public float rotateAngle;
    public ColliderVector3(float posX, float posY, float angle)
    {
        this.x = posX;
        this.y = posY;
        this.rotateAngle = angle;
    }
}


public abstract class GameCollider2D : IGameCollider2D
{
    
    private GameColliderData2D _colliderData;
    /// <summary>
    /// 该碰撞体的配置数据
    /// </summary>
    public GameColliderData2D ColliderData => _colliderData;

    /// <summary>
    /// 这个碰撞体绑定的游戏体transform信息
    /// </summary>
    private Transform _bindTransform;

    /// <summary>
    /// 用于描述这个碰撞体的位置信息
    /// X和Y信息以及旋转的信息
    /// </summary>
    public ColliderVector3 posVector3
    {
        get
        {
            
            // 计算距离
            float disToTgtPoint = _colliderData.offset.magnitude;
            // 计算偏转角度
            var initOffsetAngle = Vector2.Angle(Vector2.up,_colliderData.offset);
            // 结合当前对象的旋转角度计算实际的偏移x和y


            return new ColliderVector3();
        }
    }

    private Vector2 _pos;

    public void SetPos(Vector2 pos)
    {
        this._pos = pos;
    }

    /// <summary>
    /// 获取该碰撞体当前的位置信息
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPos()
    {
        return _pos;
    }
    

    protected GameCollider2D(GameColliderData2D colliderData,Transform tgtTransform)
    {
        this._colliderData = colliderData;
        this._bindTransform = tgtTransform;
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

    
    public GameColliderType GetColliderType()
    {
        return _colliderData.colliderType;
    }

    public abstract void OnColliderEnter(IGameCollider other);
}
