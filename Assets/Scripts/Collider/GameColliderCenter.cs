using System.Collections;
using System.Collections.Generic;
using GameEngine;

/// <summary>
/// TODO: ����Ҫ�Ż�һ�£����ݵ�ͼ���ֿ飬������ʱ��ֻ���ĳ�������ڵ�
/// ��������Ҫ��һ��
/// ��������Ӧ���Ǹ���λ�úͰ�Χ�д�С
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
