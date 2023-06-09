public class BTMoveOneFrameNode : BTMoveNode
{
    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTMoveOneFramerNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        MoveAndStop();
        if (mLogEnable)
        {
            PrintLog("move one frame...");
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }



    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_MoveOneFrame;
    }

    public override void Reset()
    {

    }

    protected override void CustomDispose()
    {
        
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
}
