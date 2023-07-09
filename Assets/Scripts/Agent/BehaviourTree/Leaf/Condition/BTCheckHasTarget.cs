public class BTCheckHasTarget : BTLeafNode
{
    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (!mContext.ContainsKey("TargetEntity"))
            return BTDefine.BT_ExcuteResult_Failed;

        if(mLogEnable)
        {
            PrintLog("Has Target Entity...");
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override int GetNodeArgNum()
    {
        return 0;   
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        return null;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckHasTarget;
    }
}
