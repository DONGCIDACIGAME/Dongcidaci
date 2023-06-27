using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public abstract class ConvexCollider2D : IConvex2DCollider, IRecycle
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
    /// 获取从左上开始顺时针旋转的顶点信息
    /// </summary>
    /// <returns></returns>
    public abstract Vector2[] GetSortedVertexs();

    /// <summary>
    /// 获取多边形碰撞体的所有边
    /// </summary>
    /// <returns></returns>
    public Vector2[,] GetEdges()
    {
        var vertexs = GetSortedVertexs();
        if (vertexs == null || vertexs.Length == 0) return null;

        var retEdges = new Vector2[vertexs.Length,2];

        for (int i=0; i<vertexs.Length;i++)
        {
            int nextIndex = i + 1;
            if(nextIndex >= vertexs.Length)
            {
                //last point
                nextIndex = 0;
            }
            retEdges[i, 0] = vertexs[i];
            retEdges[i, 1] = vertexs[nextIndex];
        }

        return retEdges;
    }

    /// <summary>
    /// 获取多边形所有边朝外的归一化法向
    /// </summary>
    /// <returns></returns>
    public Vector2[] GetEdgeOutsideNormals()
    {
        var edges = GetEdges();
        if (edges==null||edges.Length == 0)
        {
            return null;
        }

        var retNormals = new Vector2[edges.GetLength(0)];

        for (int i=0;i<retNormals.Length;i++)
        {
            var edgeVector = edges[i, 1] - edges[i,0];
            edgeVector = edgeVector.normalized;
            var normalV3  =Quaternion.AngleAxis(-90f,Vector3.up)*new Vector3(edgeVector.x,0,edgeVector.y);
            retNormals[i] = new Vector2(normalV3.x,normalV3.z);
        }

        return retNormals;
    }

    protected Vector3 _anchorPos;
    public Vector3 AnchorPos => _anchorPos;

    protected float _anchorAngle;
    public float AnchorAngle => _anchorAngle;

    protected Vector3 _scale = Vector3.one;
    public Vector3 Scale => _scale;

    protected Vector2 _size;
    public Vector2 Size => _size;

    protected Vector2 _offset;
    public Vector2 Offset => _offset;

    protected bool _needRegister = false;

    public void Initialize(MyColliderType colliderType, int bindEntityID, GameColliderData2D initialColliderData, Vector3 anchorPos, float anchorAngle, Vector3 scale, bool needRegister)
    {
        this._colliderType = colliderType;
        this._entityId = bindEntityID;

        // 碰撞的大小和偏移
        this._size = initialColliderData.size;
        this._offset = initialColliderData.offset;
        this._anchorPos = anchorPos;
        this._anchorAngle = anchorAngle;

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
            //GameColliderManager.Ins.RegisterGameCollider(this);
        }

    }


    public bool UpdateColliderPos(IColliderSetter colliderSetter, Vector3 newAnchorPos)
    {
        if (_needRegister)
        {
            if (colliderSetter is GameColliderManager)
            {
                this._anchorPos = newAnchorPos;
            }
            else { return false; }
        }
        else
        {
            this._anchorPos = newAnchorPos;
        }

        return true;

    }

    public bool UpdateColliderRotateAngle(IColliderSetter colliderSetter, float newAnchorRotateAngle)
    {
        if (_needRegister)
        {
            if (colliderSetter is GameColliderManager)
            {
                this._anchorAngle = newAnchorRotateAngle;
            }
            else { return false; }
        }
        else
        {
            this._anchorAngle = newAnchorRotateAngle;
        }

        return true;
    }

    public bool UpdateColliderScale(IColliderSetter colliderSetter, Vector3 newScale)
    {
        if (_needRegister)
        {
            if (colliderSetter is GameColliderManager)
            {
                // 更新scale需要同时更新 size 和 offset
                var changeRatioX = newScale.x / _scale.x;
                var changeRatioZ = newScale.z / _scale.z;
                this._offset = new Vector2(_offset.x * changeRatioX, _offset.y * changeRatioZ);
                this._size = new Vector2(_size.x * changeRatioX, _size.y * changeRatioZ);
                this._scale = newScale;
            }
            else { return false; }
        }
        else
        {
            // 更新scale需要同时更新 size 和 offset
            var changeRatioX = newScale.x / _scale.x;
            var changeRatioZ = newScale.z / _scale.z;
            this._offset = new Vector2(_offset.x * changeRatioX, _offset.y * changeRatioZ);
            this._size = new Vector2(_size.x * changeRatioX, _size.y * changeRatioZ);
            this._scale = newScale;
        }

        return true;
    }



    public void Dispose()
    {
        _size = Vector2.zero;
        _offset = Vector2.zero;
        _scale = Vector3.zero;
        _anchorPos = Vector3.zero;
        _anchorAngle = 0;

        // 清除碰撞类型
        _colliderType = MyColliderType.Collider_None;
        // 解除绑定的entity
        _entityId = 0;

        if (_needRegister)
        {
            //游戏体被销毁
            //GameColliderManager.Ins.UnRegisterGameCollider(this);
            _needRegister = false;
        }
    }

    public abstract void Recycle();

    public void RecycleReset()
    {
        Dispose();
    }
}
