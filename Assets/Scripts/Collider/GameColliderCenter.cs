using System.Collections;
using System.Collections.Generic;
using GameEngine;

/// <summary>
/// TODO: 这里要优化一下，根据地图划分块，做检测的时候只检测某个区域内的
/// 区块粒度要想一下
/// 划分依据应该是根据位置和包围盒大小
/// </summary>
public class GameColliderCenter : ModuleManager<GameColliderCenter>
{
    private HashSet<GameCollider> mAllGameColliders;

    public override void Initialize()
    {
        mAllGameColliders = new HashSet<GameCollider>();
    }

    public void RegisterGameCollider(GameCollider collider)
    {
        if(!mAllGameColliders.Contains(collider))
        {
            mAllGameColliders.Add(collider);
        }
    }

    public void UnRegisterGameCollider(GameCollider collider)
    {
        if (mAllGameColliders.Contains(collider))
        {
            mAllGameColliders.Remove(collider);
        }
    }
    
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);


    }

    public override void Dispose()
    {
        mAllGameColliders = null;
    }
}
