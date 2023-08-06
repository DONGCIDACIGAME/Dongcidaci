using System.Collections.Generic;
using UnityEngine;

public class GeneralGameObjectPool
{
    private GameObject mPoolRoot;
    private Dictionary<string, Stack<GameObject>> mPool;

    public GeneralGameObjectPool(GameObject poolRoot)
    {
        mPoolRoot = poolRoot;
        // 这里不要做非空保护，如果初始化的缓存池根节点是空，保证第一时间报错，防止后续导致的其他错乱
        mPoolRoot.SetActive(false);
        mPool = new Dictionary<string, Stack<GameObject>>();
    }


    private Stack<GameObject> GetPrefabStack(string prefabPath)
    {
        if (!mPool.TryGetValue(prefabPath, out Stack<GameObject> stack))
        {
            stack = new Stack<GameObject>();
            mPool.Add(prefabPath, stack);
        }

        return stack;
    }

    public GameObject Pop(string prefabPath, GameObject parent, string loadReason)
    {
        Stack<GameObject> stack = GetPrefabStack(prefabPath);

        if(stack.Count == 0)
        {
            return PrefabUtil.LoadPrefab(prefabPath, parent, loadReason);
        }

        GameObject go = stack.Pop();
        if(parent != null)
        {
            go.transform.SetParent(parent.transform, false);
        }

        return go;
    }

    public void Push(string prefabPath, GameObject obj)
    {
        if (obj == null)
        {
            Log.Error(LogLevel.Normal, "GeneralGameObjectPool pushed gameobject is null!");
            return;
        }

        Stack<GameObject> stack = GetPrefabStack(prefabPath);
        obj.transform.SetParent(mPoolRoot.transform);
        stack.Push(obj);
    }

    public void Dispose()
    {
        foreach(Stack<GameObject> pools in mPool.Values)
        {
            while (pools.Count > 0)
            {
                GameObject.Destroy(pools.Pop());
            }
        }

        mPool = null;
    }
}
