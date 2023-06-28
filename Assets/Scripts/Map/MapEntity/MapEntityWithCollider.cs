using UnityEngine;

public abstract class MapEntityWithCollider : MapEntity
{
    /// <summary>
    /// 碰撞体
    /// </summary>
    protected ConvexCollider2D mCollider;

    public ConvexCollider2D GetCollider()
    {
        return mCollider;
    }

    protected abstract MyColliderType ColliderType { get; }

    /// <summary>
    /// 需要上层绑定view是主动调用一下
    /// </summary>
    /// <param name="mapEntityView"></param>
    protected override void BindMapEntityView(MapEntityView mapEntityView)
    {
        BindGameEntityView(mapEntityView);
        this._mMapEntiyView = mapEntityView;
        SyncAllTansformInfoFromView();
        InitCollider();
    }

    protected virtual void InitCollider()
    {
        if (_mMapEntiyView != null && _mMapEntiyView is MapEntityViewWithCollider)
        {
            var mMapEntiyViewCollider = _mMapEntiyView as MapEntityViewWithCollider;
            // 获取碰撞数据
            GameColliderData2D colliderData = mMapEntiyViewCollider.GetColliderData();
            mCollider = GamePoolCenter.Ins.ConvexCollider2DPool.Pop();
            mCollider.Initialize(
                this.ColliderType, 
                mEntityId, 
                colliderData, 
                GetPosition(),
                GetRotation().y,
                GetLocalScale(), 
                true
                );

            Log.Logic(LogLevel.Info, "MapEntityWithCollider -- InitCollider -- register successful");
        }
    }

    public override void SetPosition(Vector3 position)
    {
        base.SetPosition(position);
        GameColliderManager.Ins.UpdateColliderPos(mCollider,position);
    }

    protected virtual bool CheckUpdateColliderRotate()
    {
        return true;
    }

    public override void SetRotation(Vector3 rotation)
    {
        base.SetRotation(rotation);

        if(CheckUpdateColliderRotate())
            GameColliderManager.Ins.UpdateColliderRotateAngle(mCollider,rotation.y);
    }

    public override void SetScale(Vector3 scale)
    {
        base.SetScale(scale);
        GameColliderManager.Ins.UpdateColliderScale(mCollider,scale);
    }

    public override void Dispose()
    {
        base.Dispose();
        mCollider.Recycle();
    }



}
