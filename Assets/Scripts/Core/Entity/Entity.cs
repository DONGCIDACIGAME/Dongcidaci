namespace GameEngine
{
    public abstract class Entity
    {
        // entityID
        public int EntityId { get; private set; }

        public Entity(int entityId)
        {
            EntityId = entityId;
        }

        public virtual bool Equals(Entity other)
        {
            if (other == null)
                return false;

            return other.EntityId == EntityId;
        }
    }
}


