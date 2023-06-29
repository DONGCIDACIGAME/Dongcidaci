[System.Serializable]
public class BTNodeArg_string : BTNodeArg
{
    public BTNodeArg_string(string argName, string value)
    {
        ArgName = argName;
        ArgType = BTDefine.BT_ArgType_string;
        ArgContent = value;
    }
}
