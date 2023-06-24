public class MapBlock : MapEntityWithCollider
{
    public MapBlock(MapBlockView mapBlockView)
    {
        BindMapEntityView(mapBlockView);
        SyncAllTansformInfoFromView();
        
    }


    public override int GetEntityType()
    {
        return EntityTypeDefine.Block;
    }

    protected override IGameColliderHandler GetColliderHanlder()
    {
        return new BlockColliderHandler(this);
    }

    protected override int GetColliderType()
    {
        return GameColliderDefine.ColliderType_Block;
    }
}
