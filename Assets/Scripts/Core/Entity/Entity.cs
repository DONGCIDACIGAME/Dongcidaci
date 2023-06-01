namespace GameEngine
{
    public abstract class Entity
    {
        // entityID
        private int mEntityId;

        public Entity()
        {
            EntityManager.Ins.AddEntity(this);
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


