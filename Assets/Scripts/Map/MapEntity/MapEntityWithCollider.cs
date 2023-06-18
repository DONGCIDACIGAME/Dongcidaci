using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapEntityWithCollider : MapEntity, ICollideProcessor
{
    /// <summary>
    /// 碰撞体
    /// </summary>
    protected GameCollider2D mCollider;

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
            var anchorPos = GetWorld2DPosition();
            var anchorRotateAngle = GetWorldRotationByYAxis();
            var scaleV3 = GetLocalScale();
            // 生成碰撞体
            mCollider = new GameCollider2D(
                colliderData,
                this,
                anchorPos,
                anchorRotateAngle,
                scaleV3.x,
                scaleV3.z
                );
            // 注册碰撞
            GameColliderManager.Ins.RegisterGameCollider(mCollider);
        }
    }

    public abstract void HandleCollideTo(ICollideProcessor tgtColliderProcessor);

    public GameEntity GetProcessorEntity()
    {
        return this;
    }


}
