using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommanHelper
{
    public static Transform FindChildNode(Transform rootNode, string name)
    {
        if (rootNode == null)
        {
            Log.Error(LogLevel.Critical, "FindChildNode Error,rootNode is null!");
            return null;
        }

        Transform target = null;
        target = rootNode.Find(name);
        if (target != null)
        {
            return target;
        }

        for (int i = 0; i < rootNode.transform.childCount; i++)
        {
            Transform child = rootNode.transform.GetChild(i);
            if (child.name.Equals(name))
            {
                target = child;
            }
            else
            {
                target = FindChildNode(child, name);
            }

            if (target != null)
                return target;
        }

        return null;
    }

}
