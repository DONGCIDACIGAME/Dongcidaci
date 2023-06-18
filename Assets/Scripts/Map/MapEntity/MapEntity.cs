using GameEngine;

public abstract class MapEntity : GameEntity
{
    /// <summary>
    /// 用于更新MapEntity相关的基础表现逻辑，对外，对上层都不开放
    /// </summary>
    protected MapEntityView mMapEntiyView;

    protected virtual void BindMapEntityView(MapEntityView mapEntityView)
    {
        BindGameEntityView(mapEntityView);
        this.mMapEntiyView = mapEntityView;

    }



}

