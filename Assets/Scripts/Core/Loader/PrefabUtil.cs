using GameEngine;
using UnityEngine;

public static class PrefabUtil
{
    public static GameObject LoadPrefab(string path,string reason)
    {
        GameObject newGo = null;
        GameObject go = ResourceMgr.Ins.Load<GameObject>(path, reason);
        if(go != null)
        {
            newGo = GameObject.Instantiate(go);
        }

        return newGo;
    }

    /// <summary>
    /// 加载prefab到指定go下
    /// 不保持世界坐标
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static GameObject LoadPrefab(string path, GameObject parent, string reason)
    {
        GameObject go = LoadPrefab(path, reason);
        if(go != null && parent != null)
        {
            go.transform.SetParent(parent.transform, false);
        }

        return go;
    }

    /// <summary>
    /// 加载prefab到指定go下
    /// 不保持世界坐标
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <param name="localPos"></param>
    /// <param name="localScale"></param>
    /// <param name="localRot"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static GameObject LoadPrefab(string path, GameObject parent, Vector3 localPos, Vector3 localScale, Quaternion localRot, string reason)
    {
        GameObject go = LoadPrefab(path, reason);
        if (go != null && parent != null)
        {
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = localPos;
            go.transform.localScale = localScale;
            go.transform.localRotation = localRot;
        }

        return go;
    }

}
