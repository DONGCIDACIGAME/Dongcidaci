using GameEngine;
using System.Collections.Generic;

/// <summary>
/// ע���CommonUpdateCenter��updater������˳���ע��˳���޹�
/// TODO:
/// 1.������Ҫ����ô����ô���Ƿ��ø�hashset�Ϳ�����
/// 2.����ģ���update�Ƿ�Ҳ���������������и�������������updateĿǰ������ģ����ܻ�������
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
