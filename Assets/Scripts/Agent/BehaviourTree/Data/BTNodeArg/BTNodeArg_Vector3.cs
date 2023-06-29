using UnityEngine;

[System.Serializable]
public class BTNodeArg_Vector3 : BTNodeArg
{
    public BTNodeArg_Vector3(string argName, Vector3 value)
    {
        ArgName = argName;
        ArgType = BTDefine.BT_ArgType_Vector3;
        ArgContent = string.Format("{0}_{1}_{2}", value.x, value.y, value.z);
    }
}
