using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjTransformData
{
    public float posX = 0;
    public float posY = 0;
    public float posZ = 0;
    public float angleX = 0;
    public float angleY = 0;
    public float angleZ = 0;
    public float scaleX = 1;
    public float scaleY = 1;
    public float scaleZ = 1;
}


[System.Serializable]
public class FXConfigData
{
    public string fxName;

    public ObjTransformData transInfo;




}
