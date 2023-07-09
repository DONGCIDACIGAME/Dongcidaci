using System.Collections.Generic;

public class BTDetectAgentInAreaNode : BTLeafNode
{
    private int mTargetAgentType;
    private int mTargetAreaType;

    public void SetTargetAgentType(int targetAgentType)
    {
        mTargetAgentType = targetAgentType;
    }

    public int GetTargetAgentType()
    {
        return mTargetAgentType;
    }

    public void SetTargetAreaType(int targetAreaType)
    {
        mTargetAreaType = targetAreaType;
    }

    public int GetTargetAreaType()
    {
        return mTargetAreaType;
    }

    public override int GetNodeArgNum()
    {
        return 2;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value1);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        result = BehaviourTreeHelper.ParseInt(args[1], out int value2);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetTargetAgentType(value1);
        SetTargetAreaType(value2);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("TargetAgentType", mTargetAgentType);
        BTNodeArg arg2 = new BTNodeArg_int("TargetAreaType", mTargetAgentType);
        return new BTNodeArg[] { arg1, arg2 };
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

        bool hasAgentType = false;
        for(int i = 0;i<AgentDefine.ALL_AGENT_TYPES.Length;i++)
        {
            if(AgentDefine.ALL_AGENT_TYPES[i].Type == mTargetAgentType)
            {
                hasAgentType = true;
                break;
            }
        }

        if (!hasAgentType)
        {
            info = string.Format("Node {0} has Invalid Agent Target Type:{1}", NodeName, mTargetAgentType);
            return false;
        }

        if(mTargetAgentType == AgentDefine.AgentType_NotDefine)
        {
            info = string.Format("Node {0} Agent Target Type undefine is invalid", NodeName);
            return false;
        }


        bool hasAreaType = false;
        for (int i = 0; i < BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length; i++)
        {
            if (BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Type == mTargetAreaType)
            {
                hasAreaType = true;
                break;
            }
        }

        if (!hasAreaType)
        {
            info = string.Format("Node {0} has Invalid Target area Type:{1}", NodeName, mTargetAreaType);
            return false;
        }

        if (mTargetAreaType == BTDefine.BT_LogicArea_Type_Undefine)
        {
            info = string.Format("Node {0} Target area Type undefine is invalid", NodeName);
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
            {
                mContext["TargetEntity"] = agent;
                if (mLogEnable)
                {
                    PrintLog("Detect Target Agent in vision Succeed~");
                }
                return BTDefine.BT_ExcuteResult_Succeed;
            }
        }
        if (mLogEnable)
        {
            PrintLog("Detect Target in vision Agent Failed.");
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
        return BTDefine.BT_Node_Type_Leaf_DetectAgentInArea;
    }
}
