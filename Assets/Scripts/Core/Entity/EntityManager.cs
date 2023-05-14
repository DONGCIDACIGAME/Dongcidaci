using GameEngine;
using System.Collections.Generic;

public class EntityManager : Singleton<EntityManager>
{
    private List<IEntity[]> mEntityMap;
    private int mIndexer;
    private Queue<int> validEntityId;

    public EntityManager()
    {
        mEntityMap = new List<IEntity[]>();
        validEntityId = new Queue<int>();
    }

    public void AddEntity(IEntity entity)
    {
        if(entity == null)
        {
            return;
        }

        if(entity.GetEntityId() > 0)
        {
            return;
        }

        int entityId = 0;
        if (validEntityId.Count > 0)
        {
            entityId = validEntityId.Dequeue();
        }
        else
        {
            mIndexer++;
            entityId = mIndexer;
        }

        int outerIndex = entityId / EntityDefine.ENTITY_COLUME;
        int innerIndex = entityId % EntityDefine.ENTITY_COLUME;

        if (outerIndex >= mEntityMap.Count)
        {
            mEntityMap.Add(new IEntity[EntityDefine.ENTITY_COLUME]);
        }

        IEntity[] innerMap = mEntityMap[outerIndex];
        innerMap[innerIndex] = entity;
        entity.SetEntityId(entityId);
    }

    public IEntity GetEntity(int entityId)
    {
        int outerIndex = entityId / EntityDefine.ENTITY_COLUME;
        int innerIndex = entityId % EntityDefine.ENTITY_COLUME;
        if (outerIndex < 0 || outerIndex >= mEntityMap.Count)
        {
            return null;
        }
        IEntity[] innerMap = mEntityMap[outerIndex];
        return innerMap[innerIndex];
    }

    public void RemoveEntity(IEntity entity)
    {
        if (entity == null)
            return;

        int entityId = entity.GetEntityId();

        int outerIndex = entityId / EntityDefine.ENTITY_COLUME;
        int innerIndex = entityId % EntityDefine.ENTITY_COLUME;

        if(outerIndex < mEntityMap.Count)
        {
            mEntityMap[outerIndex][innerIndex] = null;
        }

        validEntityId.Enqueue(entityId);
        entity.SetEntityId(0);
    }
}
