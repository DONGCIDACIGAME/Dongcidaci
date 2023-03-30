using GameEngine;
using System.Collections.Generic;

public class EntityManager : Singleton<EntityManager>
{
    private List<IEntity[]> mEntityMap;
    private int mIndexer;
    private HashSet<int> mEntityRecord;
    private Queue<int> validEntityId;

    public EntityManager()
    {
        mEntityMap = new List<IEntity[]>();
        mEntityRecord = new HashSet<int>();
        validEntityId = new Queue<int>();
    }

    public int  AddEntity(IEntity entity)
    {
        if(entity == null)
        {
            return -1;
        }

        int uniqueId = entity.GetHashCode();
        if(mEntityRecord.Contains(uniqueId))
        {
            return -1;
        }

        int entityId = -1;
        if (validEntityId.Count > 0)
        {
            entityId = validEntityId.Dequeue();
        }
        else
        {
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
        mEntityRecord.Add(uniqueId);
        mIndexer++;
        return entityId;
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

        int uniqueId = entity.GetHashCode();
        int entityId = entity.GetEntityId();

        mEntityRecord.Remove(uniqueId);

        int outerIndex = entityId / EntityDefine.ENTITY_COLUME;
        int innerIndex = entityId % EntityDefine.ENTITY_COLUME;

        if(outerIndex < mEntityMap.Count)
        {
            mEntityMap[outerIndex][innerIndex] = null;
        }

        validEntityId.Enqueue(entityId);
    }
}
