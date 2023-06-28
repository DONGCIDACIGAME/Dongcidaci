using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class ConvexCollider2D : IConvex2DCollider, IRecycle
{
    /// <summary>
    /// UID的自增计数器
    /// </summary>
    protected class AutoIndexCounter
    {
        private int _index;
        public int GetIndex()
        {
            _index += 1;
            return _index;
        }
    }

    /// <summary>
    /// 自增型id生成器
    /// </summary>
    protected static readonly AutoIndexCounter _indexCounter = new AutoIndexCounter();

    /// <summary>
    /// 碰撞体的自增UID
    /// </summary>
    protected int _colliderUID;
    public int GetColliderUID()
    {
        return _colliderUID;
    }

    /// <summary>
    /// 在碰撞体生成实例时赋予UID
    /// </summary>
    public ConvexCollider2D()
    {
        // 新建实例时，动态生成唯一自增的UID
        this._colliderUID = _indexCounter.GetIndex();
    }

    /// <summary>
    /// 绑定的实体ID
    /// </summary>
    protected int _entityId;
    public int GetBindEntityId()
    {
        return _entityId;
    }

    /// <summary>
    /// 碰撞类型
    /// </summary>
    protected MyColliderType _colliderType;
    public MyColliderType GetColliderType()
    {
        return _colliderType;
    }

    /// <summary>
    /// 凸面多边形形状
    /// </summary>
    protected IConvex2DShape _convex2DShape;
    public IConvex2DShape Convex2DShape => _convex2DShape;

    protected Vector3 _scale = Vector3.one;
    public Vector3 Scale => _scale;

    protected bool _needRegister = false;

    public void Initialize(MyColliderType colliderType, int bindEntityID, GameColliderData2D initColliderData, Vector3 anchorPos, float anchorAngle, Vector3 scale, bool needRegister)
    {
        this._colliderType = colliderType;
        this._entityId = bindEntityID;

        // 碰撞的大小和偏移
        switch (initColliderData.shapeType)
        {
            case Convex2DShapeType.Rect:
                this._convex2DShape = new Rect2DShape(anchorPos,anchorAngle,initColliderData.offset,initColliderData.size);
                break;
            case Convex2DShapeType.Circle:
                this._convex2DShape = new Circle2DShape(anchorPos, anchorAngle, initColliderData.offset, initColliderData.size.x);
                break;
            case Convex2DShapeType.Ellipse:
                this._convex2DShape = new Ellipse2DShape(anchorPos, anchorAngle, initColliderData.offset, initColliderData.size);
                break;

            default:
                Log.Error(LogLevel.Critical, "init convex collider error, undefined shape");
                this._convex2DShape = new Rect2DShape(anchorPos, anchorAngle, initColliderData.offset, initColliderData.size);
                break;
        }

        if (scale.x == 0 || scale.y == 0 || scale.z == 0)
        {
            Log.Error(LogLevel.Normal, "InitWithRegister error ---- scale is zero ");
            this._scale = Vector3.one;
        }
        else
        {
            this._scale = scale;
        }

        this._needRegister = needRegister;

        if (_needRegister)
        {
            // 注册碰撞
            GameColliderManager.Ins.RegisterGameCollider(this);
        }

    }

    /// <summary>
    /// 更新碰撞体的位置
    /// </summary>
    /// <param name="colliderSetter"></param>
    /// <param name="newAnchorPos"></param>
    /// <returns></returns>
    public bool UpdateColliderPos(IColliderSetter colliderSetter, Vector3 newAnchorPos)
    {
        if (this._convex2DShape == null) return false;

        if (_needRegister)
        {
            if (colliderSetter is GameColliderManager)
            {
                this._convex2DShape.AnchorPos = newAnchorPos;
            }
            else { return false; }
        }
        else
        {
            this._convex2DShape.AnchorPos = newAnchorPos;
        }

        return true;

    }

    /// <summary>
    /// 设置碰撞体的旋转角度
    /// </summary>
    /// <param name="colliderSetter"></param>
    /// <param name="newAnchorRotateAngle"></param>
    /// <returns></returns>
    public bool UpdateColliderRotateAngle(IColliderSetter colliderSetter, float newAnchorRotateAngle)
    {
        if (this._convex2DShape == null) return false;
        if (_needRegister)
        {
            if (colliderSetter is GameColliderManager)
            {
                this._convex2DShape.AnchorAngle = newAnchorRotateAngle;
            }
            else { return false; }
        }
        else
        {
            this._convex2DShape.AnchorAngle = newAnchorRotateAngle;
        }

        return true;
    }

    public bool UpdateColliderScale(IColliderSetter colliderSetter, Vector3 newScale)
    {
        if (this._convex2DShape == null) return false;
        if (_needRegister)
        {
            if (colliderSetter is GameColliderManager)
            {
                // 更新scale需要同时更新 size 和 offset
                var changeRatioX = newScale.x / _scale.x;
                var changeRatioZ = newScale.z / _scale.z;
                this._convex2DShape.Offset = new Vector2(this._convex2DShape.Offset.x * changeRatioX, this._convex2DShape.Offset.y * changeRatioZ);
                this._convex2DShape.Size = new Vector2(this._convex2DShape.Size.x * changeRatioX, this._convex2DShape.Size.y * changeRatioZ);
                this._scale = newScale;
            }
            else { return false; }
        }
        else
        {
            // 更新scale需要同时更新 size 和 offset
            var changeRatioX = newScale.x / _scale.x;
            var changeRatioZ = newScale.z / _scale.z;
            this._convex2DShape.Offset = new Vector2(this._convex2DShape.Offset.x * changeRatioX, this._convex2DShape.Offset.y * changeRatioZ);
            this._convex2DShape.Size = new Vector2(this._convex2DShape.Size.x * changeRatioX, this._convex2DShape.Size.y * changeRatioZ);
            this._scale = newScale;
        }

        return true;
    }



    public void Dispose()
    {
        _convex2DShape = null;

        _scale = Vector3.zero;
        // 清除碰撞类型
        _colliderType = MyColliderType.Collider_None;
        // 解除绑定的entity
        _entityId = 0;

        if (_needRegister)
        {
            //游戏体被销毁
            GameColliderManager.Ins.UnRegisterGameCollider(this);
            _needRegister = false;
        }
    }

    public void Recycle()
    {
        Dispose();
        GamePoolCenter.Ins.ConvexCollider2DPool.Push(this);
    }

    public void RecycleReset()
    {
        Dispose();
    }
}
