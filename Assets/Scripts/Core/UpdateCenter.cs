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
    private HashSet<IGameUpdate> mUpdates;
    public override void Initialize()
    {
        Log.Logic(LogLevel.Critical, "UpdateCenter Initialized...");
        mUpdates = new HashSet<IGameUpdate>();
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
        foreach(IGameUpdate updater in mUpdates)
        {
            if(updater != null)
            {
                updater.OnUpdate(deltaTime);
            }
        }
    }

    public override void Dispose()
    {
        mUpdates = null;
    }
}
