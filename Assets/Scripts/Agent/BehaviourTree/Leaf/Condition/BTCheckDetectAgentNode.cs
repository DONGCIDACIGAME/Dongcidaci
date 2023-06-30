using System.Collections.Generic;

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
        BTNodeArg arg1 = new BTNodeArg_int("TargetAgentType", mTargetAgentType);
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

    private bool CheckMatchType(Agent agt, int targetAgentType)
    {
        switch (targetAgentType)
        {
            case AgentDefine.AgentType_Hero:
                return agt is Hero;
            case AgentDefine.AgentType_Monster:
                return agt is Monster;
            case AgentDefine.AgentType_NPC:
                return agt is NPC;
            default:
                return false;
        }
    }


    public override int Excute(float deltaTime)
    {
        if(mExcutor == null)
        {
            Log.Error(LogLevel.Info, "BTCheckDetectAgentNode Exucte Failed, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if(mTargetAgentType == AgentDefine.AgentType_NotDefine)
        {
            Log.Error(LogLevel.Info, "BTCheckDetectAgentNode Exucte Failed, target agent type not defined!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        List<Agent> detectedAgents = mExcutor.DetectAgentInVision();
        foreach(Agent agent in detectedAgents)
        {
            if (CheckMatchType(agent, mTargetAgentType))
                return BTDefine.BT_ExcuteResult_Succeed;
        }

        return BTDefine.BT_ExcuteResult_Failed;   
    }

    public override void Reset()
    {
        
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
