public abstract class MapEntity : GameEntity
{
    /// <summary>
    /// 用于更新MapEntity相关的基础表现逻辑
    /// </summary>
    protected MapEntityView _mMapEntiyView;

    protected virtual void BindMapEntityView(MapEntityView mapEntityView)
    {
        BindGameEntityView(mapEntityView);
        this._mMapEntiyView = mapEntityView;
    }

    public MapEntityView GetView()
    {
        return _mMapEntiyView;
    }
}

