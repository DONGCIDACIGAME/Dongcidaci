public class MapBlock : MapEntityWithCollider
{
    public MapBlock(MapBlockView mapBlockView)
    {
        BindMapEntityView(mapBlockView);
        
    }

    protected override MyColliderType ColliderType => MyColliderType.Collider_Block;

    public override int GetEntityType()
    {
        return EntityTypeDefine.Block;
    }

    
    


}
