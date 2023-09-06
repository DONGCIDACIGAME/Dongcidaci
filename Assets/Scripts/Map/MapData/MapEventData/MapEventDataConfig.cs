using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEventDataConfig : MonoBehaviour
{
    public MapEventData configData;

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
