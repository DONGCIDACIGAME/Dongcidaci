using UnityEngine;

[System.Serializable]
public class BTNodeArg_Vector2 : BTNodeArg
{
    public BTNodeArg_Vector2(string argName, Vector2 value)
    {
        ArgName = argName;
        ArgType = BTDefine.BT_ArgType_Vector2;
        ArgContent = string.Format("{0}_{1}",value.x, value.y);
    }
}
