using GameEngine;
using System.Collections.Generic;

/// <summary>
/// 注册进CommonUpdateCenter得updater，更新顺序和注册顺序无关
/// TODO:
/// 1.这里需要搞这么复杂么，是否用个hashset就可以了
/// 2.其他模块的update是否也放在这里驱动？有个问题就是这里的update目前是无序的，可能会有问题
/// </summary>
public class UpdateCenter : ModuleManager<UpdateCenter>
{
    //private HashSet<IGameUpdate> mUpdates;
    private List<IGameUpdate> mUpdates;

    public override void Initialize()
    {
        Log.Logic(LogLevel.Critical, "UpdateCenter Initialized...");
        //mUpdates = new HashSet<IGameUpdate>();
        mUpdates = new List<IGameUpdate>();
    }

    public void RegisterUpdater(IGameUpdate updater)
    {
        if (updater == null)
            return;

        if(!mUpdates.Contains(updater))
            mUpdates.Add(updater);

    }

    public void UnregisterUpdater(IGameUpdate updater)
    {
        if (mUpdates.Contains(updater))
            mUpdates.Remove(updater);
    }

    public override void OnUpdate(float deltaTime)
    {
        // modified by weng 0707
        // 这个地方需要从后往前遍历，因为如果更新者在update中执行了注销的逻辑
        // 会导致索引越界，foreach 性能偏低

        if (mUpdates == null || mUpdates.Count == 0) return;
        for (int i=mUpdates.Count-1; i>=0; i--)
        {
            if (mUpdates[i] != null)
            {
                mUpdates[i].OnUpdate(deltaTime);
            }
        }

        /**
        foreach(IGameUpdate updater in mUpdates)
        {
            if(updater != null)
            {
                updater.OnUpdate(deltaTime);
            }
        }
        */

    }

    public override void Dispose()
    {
        mUpdates = null;
    }
}
