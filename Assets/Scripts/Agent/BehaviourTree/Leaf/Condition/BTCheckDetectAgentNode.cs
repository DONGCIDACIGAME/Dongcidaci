public class BTCheckDetectAgentNode : BTLeafNode
{
    private int mTargetAgentType;

    public void SetTargetAgentType(int targetAgentType)
    {
        mTargetAgentType = targetAgentType;
    }

    public int GetTargetAgentType()
    {
        return mTargetAgentType;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetTargetAgentType(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg();
        arg1.ArgName = "TargetAgentType";
        arg1.ArgType = BTDefine.BT_ArgType_int;
        arg1.ArgContent = mTargetAgentType.ToString();
        return new BTNodeArg[] { arg1 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if(mTargetAgentType != AgentDefine.AgentType_Hero
            && mTargetAgentType != AgentDefine.AgentType_Monster 
            && mTargetAgentType != AgentDefine.AgentType_NPC)
        {
            info = string.Format("Node {0} has Invalid Agent Target Type:{1}", NodeName, mTargetAgentType); 
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        // TODO:需要角色侦察区域支持
        return 0;   
    }

    public override void Reset()
    {
        mTargetAgentType = AgentDefine.AgentType_NotDefine;
    }

    protected override void CustomDispose()
    {
        mTargetAgentType = AgentDefine.AgentType_NotDefine;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckDetectAgent;
    }
}
