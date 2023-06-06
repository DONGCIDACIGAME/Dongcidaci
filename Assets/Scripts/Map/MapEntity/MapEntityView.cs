using UnityEngine;

[RequireComponent(typeof(GameColliderView))]
public abstract class MapEntityView : GameEntityView
{
    // 碰撞配置（挂载在预制件上）
    protected GameColliderView mColliderView;

    public GameColliderData2D GetColliderData()
    {
        // 拿到colliderview组件
        mColliderView = GetComponent<GameColliderView>();
        // 获取colliderView组件上的碰撞配置，生成碰撞数据
        GameColliderData2D colliderData = new GameColliderData2D(mColliderView.size, mColliderView.offset);
        return colliderData;
    }

    public override void Dispose()
    {
        base.Dispose();
        mColliderView = null;
    }

}

