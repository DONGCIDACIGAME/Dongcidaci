[System.Serializable]
public class BTNodeArg_int : BTNodeArg
{
    public BTNodeArg_int(string argName, int value) 
    {
        ArgName = argName;
        ArgType = BTDefine.BT_ArgType_int;
        ArgContent = value.ToString();
    }
}
