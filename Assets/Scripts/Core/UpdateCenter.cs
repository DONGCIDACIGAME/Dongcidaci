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
    private List<IGameUpdate> mToUpdates;
    private SimpleQueue<int> mValidIndexs;

    // ��������ʱ���β����������С
    private int Capacity = 1024;

    public override void Initialize()
    {
        Log.Logic(LogLevel.Critical, "UpdateCenter Initialized...");
        mToUpdates = new List<IGameUpdate>(Capacity);
        mValidIndexs = new SimpleQueue<int>();
    }

    public int RegisterUpdater(IGameUpdate updater)
    {
        // ���û�п��õĿ�λ����һ���Բ���Capacity����λ����
        if (mValidIndexs.Count <= 0)
        {
            int curCapacity = mToUpdates.Count;

            for (int i = 0; i < Capacity; i++)
            {
                int indexAdd = curCapacity + i;
                mValidIndexs.Enqueue(indexAdd);
                mToUpdates.Add(null);
            }
        }

        // ��updater�����׸����ӵĿ�λ��
        int index = mValidIndexs.Dequeue();
        mToUpdates[index] = updater;
        return index;
    }

    public void UnregisterUpdater(int index)
    {
        mToUpdates[index] = null;
        mValidIndexs.Enqueue(index);
    }

    public override void OnUpdate(float deltaTime)
    {
        for(int i=0;i<mToUpdates.Count;i++)
        {
            if(mToUpdates[i] != null)
            {
                mToUpdates[i].OnUpdate(deltaTime);
            }
        }
    }

    public override void Dispose()
    {
        mToUpdates = null;
        mValidIndexs = null;
    }
}
