[System.Serializable]
public class BTNodeArg_float : BTNodeArg
{
    public BTNodeArg_float(string argName, float value)
    {
        ArgName = argName;
        ArgType = BTDefine.BT_ArgType_float;
        ArgContent = value.ToString();
    }
}
