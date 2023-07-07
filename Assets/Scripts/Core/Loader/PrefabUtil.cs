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

    public static GameObject LoadPrefab(string path, GameObject parent, string reason)
    {
        GameObject go = LoadPrefab(path, reason);
        if(go != null && parent != null)
        {
            go.transform.parent = parent.transform;
            go.transform.localPosition = Vector3.zero;
            // changed by weng 
            //go.transform.localScale = Vector3.one;
            go.transform.rotation = Quaternion.identity;
        }

        return go;
    }

}
