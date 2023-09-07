using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MapEventDataConfig : MonoBehaviour
{
    public MapEventData configData;

    [System.Serializable]
    public struct StrDict
    {
        public string keyStr;
        public string valueStr;
    }

    [System.Serializable]
    public struct FloatDict
    {
        public string keyStr;
        public float valueFloat;
    }

    [System.Serializable]
    public struct IntDict
    {
        public string keyStr;
        public int valueInt;
    }

    [SerializeField]
    public List<StrDict> customStrDictList;
    [SerializeField]
    public List<FloatDict> customFloatDictList;
    [SerializeField]
    public List<IntDict> customIntDictList;


    public void SyncPositionInfo()
    {
        configData.initPosX = transform.position.x;
        configData.initPosY = transform.position.y;
        configData.initPosZ = transform.position.z;

        configData.initRotateX = transform.eulerAngles.x;
        configData.initRotateY = transform.eulerAngles.y;
        configData.initRotateZ = transform.eulerAngles.z;

        configData.initLocalScaleX = transform.localScale.x;
        configData.initLocalScaleY = transform.localScale.y;
        configData.initLocalScaleZ = transform.localScale.z;
    }


    public void SyncCustomDictData()
    {
        if (configData.customStrParams == null) configData.customStrParams = new Dictionary<string, string>();
        if (configData.customFloatParams == null) configData.customFloatParams = new Dictionary<string, float>();
        if (configData.customIntParams == null) configData.customIntParams = new Dictionary<string, int>();

        configData.customStrParams.Clear();
        configData.customFloatParams.Clear();
        configData.customIntParams.Clear();

        foreach (var strPair in customStrDictList)
        {
            Log.Logic(LogLevel.Normal,"添加自定义字符串参数");
            configData.customStrParams.Add(strPair.keyStr,strPair.valueStr);
        }

        foreach (var floatPair in customFloatDictList)
        {
            Log.Logic(LogLevel.Normal, "添加自定义浮点参数");
            configData.customFloatParams.Add(floatPair.keyStr, floatPair.valueFloat);
        }

        foreach (var intPair in customIntDictList)
        {
            Log.Logic(LogLevel.Normal, "添加自定义整形参数");
            configData.customIntParams.Add(intPair.keyStr, intPair.valueInt);
        }

    }


#if UNITY_EDITOR

    public bool drawLabel = true;
    
    private void OnDrawGizmos()
    {
        if (drawLabel == false) return;


        switch (configData.eventType)
        {
            case MapEventType.PlayerInitPoint:
                Handles.Label(transform.position, new GUIContent("主角出生"));
                break;
            case MapEventType.MonsterInitPoint:
                Handles.Label(transform.position, new GUIContent("怪物出生"));
                break;

        }

        
    }


#endif

}
