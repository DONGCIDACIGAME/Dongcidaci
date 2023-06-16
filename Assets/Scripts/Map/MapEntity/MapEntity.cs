public abstract class MapEntity : GameEntity
{
    /// <summary>
    /// 用于更新MapEntity相关的基础表现逻辑，对外，对上层都不开放
    /// </summary>
    private MapEntityView mMapEntiyView;

    /// <summary>
    /// 碰撞体
    /// </summary>
    protected GameCollider2D mCollider;

    /// <summary>
    /// 需要上层绑定view是主动调用一下
    /// </summary>
    /// <param name="mapEntityView"></param>
    protected void BindMapEntityView(MapEntityView mapEntityView)
    {
        BindGameEntityView(mapEntityView);

        mMapEntiyView = mapEntityView;

        if(mMapEntiyView != null)
        {
            // 获取碰撞数据
            GameColliderData2D colliderData = mMapEntiyView.GetColliderData();
            // 生成碰撞体
            mCollider = new GameCollider2D(colliderData, null, GetPosition());
            // 注册碰撞
            //GameColliderManager.Ins.RegisterGameCollider(mCollider);
        }
    }
}

