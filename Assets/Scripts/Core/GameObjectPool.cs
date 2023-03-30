using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject Pool 
/// 1. 常规的pool操作
/// 2. gameobject 回收到pool的默认节点下
/// </summary>
public class GameObjectPool
{
    private GameObject mGoPoolRoot;
    private Stack<GameObject> mPool;

    private void SetGoPoolRoot(GameObject root)
    {
        if (mGoPoolRoot == null)
        {
            Log.Error(LogLevel.Critical, "GameObjectPool Push go Failed,mGoPoolRoot is null!");
            return;
        }

        mGoPoolRoot = root;
        mGoPoolRoot.SetActive(false);
    }

    private Stack<GameObject> pool;

    public GameObjectPool(GameObject root)
    {
        SetGoPoolRoot(root);
        this.mPool = new Stack<GameObject>();
    }

    public void PushObj(GameObject go)
    {
        if(go == null)
        {
            Log.Error(LogLevel.Normal, "GameObjectPool Push go Failed,go is null!");
            return;
        }

        if(mGoPoolRoot == null)
        {
            Log.Error(LogLevel.Critical, "GameObjectPool Push go Failed,mGoPoolRoot is null!");
            return;
        }

        mPool.Push(go);
        go.transform.SetParent(mGoPoolRoot.transform);
    }

    public GameObject PopObj()
    {
        if (pool.Count == 0)
        {
            return null;
        }

        return pool.Pop();
    }

    public void Clear()
    {
        pool.Clear();
        if(mGoPoolRoot != null)
        {
            GameObject.Destroy(mGoPoolRoot);
        }
    }
}
