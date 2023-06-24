using UnityEngine;

public abstract class MapEntityWithCollider : MapEntity
{
    /// <summary>
    /// 碰撞体
    /// </summary>
    protected GameCollider2D mCollider;

    public GameCollider2D GetCollider()
    {
        return mCollider;
    }

    protected abstract IGameColliderHandler GetColliderHanlder();

    protected abstract int GetColliderType();

    /// <summary>
    /// 需要上层绑定view是主动调用一下
    /// </summary>
    /// <param name="mapEntityView"></param>
    protected override void BindMapEntityView(MapEntityView mapEntityView)
    {
        BindGameEntityView(mapEntityView);

        this.mMapEntiyView = mapEntityView;

        if (mMapEntiyView != null && mapEntityView is MapEntityViewWithCollider)
        {
            var mMapEntiyViewCollider = mapEntityView as MapEntityViewWithCollider;
            // 获取碰撞数据
            GameColliderData2D colliderData = mMapEntiyViewCollider.GetColliderData();

            mCollider = GamePoolCenter.Ins.GameCollider2DPool.Pop();
            mCollider.Initialize(GetColliderType(), mEntityId, colliderData, GetColliderHanlder());
            Log.Logic(LogLevel.Info,"注册碰撞");
        }
    }

    public override void SetPosition(Vector3 position)
    {
        base.SetPosition(position);
        mCollider.UpdateColliderPos(position);
        GameColliderManager.Ins.UpdateColliderIndexInMap(mCollider);
    }

    public override void SetRotation(Vector3 rotation)
    {
        base.SetRotation(rotation);
        mCollider.UpdateColliderRot(rotation.y);
        GameColliderManager.Ins.UpdateColliderIndexInMap(mCollider);
    }

    public override void SetScale(Vector3 scale)
    {
        base.SetScale(scale);
        mCollider.UpdateColliderScale(scale);
        GameColliderManager.Ins.UpdateColliderIndexInMap(mCollider);
    }

    public override void Dispose()
    {
        base.Dispose();
        mCollider.Recycle();
    }
}
