namespace GameEngine
{
    public abstract class Entity : IGameDisposable
    {
        // entityID
        protected int mEntityId;

        public Entity()
        {
            EntityManager.Ins.AddEntity(this);
        }

        public virtual void Dispose()
        {
            EntityManager.Ins.RemoveEntity(this);
        }

        public int GetEntityId()
        {
            return mEntityId;
        }

        public void SetEntityId(int entityId)
        {
            mEntityId = entityId;
        }
    }
}


