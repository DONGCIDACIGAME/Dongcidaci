public class MapWall : MapEntityWithCollider
{
    public override int GetEntityType()
    {
        return EntityTypeDefine.Wall;
    }

    public override void HandleCollideTo(ICollideProcessor tgtColliderProcessor)
    {
        throw new System.NotImplementedException();
    }


}
