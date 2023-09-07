using System.Collections.Generic;
using UnityEngine;

public enum MapEventType
{
    PlayerInitPoint,
    MonsterInitPoint,

}



[System.Serializable]
public class MapEventData 
{
    public MapEventType eventType;
    public string eventPrefabName;

    [HideInInspector]
    public float initPosX;
    [HideInInspector]
    public float initPosY;
    [HideInInspector]
    public float initPosZ;
    [HideInInspector]
    public float initRotateX;
    [HideInInspector]
    public float initRotateY;
    [HideInInspector]
    public float initRotateZ;
    [HideInInspector]
    public float initLocalScaleX;
    [HideInInspector]
    public float initLocalScaleY;
    [HideInInspector]
    public float initLocalScaleZ;

    /// <summary>
    /// 自定义字符串参数
    /// </summary>
    public Dictionary<string, string> customStrParams;
    /// <summary>
    /// 自定义浮点参数
    /// </summary>
    public Dictionary<string, float> customFloatParams;
    /// <summary>
    /// 自定义整形参数
    /// </summary>
    public Dictionary<string, int> customIntParams;

}
