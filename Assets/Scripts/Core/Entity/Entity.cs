public abstract class Entity : IEntity
{
    protected int mEntityId;

    public int GetEntityId()
    {
        return mEntityId;
    }

    public void SetEntityId(int entityId)
    {
        mEntityId = entityId;
    }
}
