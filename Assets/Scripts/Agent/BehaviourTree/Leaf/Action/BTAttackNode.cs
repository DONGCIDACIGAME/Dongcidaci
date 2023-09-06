using UnityEngine;

public class BTAttackNode : BTLeafNode
{
    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }


    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTAttackNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

       Vector3 towards = mExcutor.GetTowards();

        // 发送attack指令
        AgentCommand attackCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        attackCmd.Initialize(AgentCommandDefine.METER_ATTACK, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
        mExcutor.OnCommand(attackCmd);


        if (mLogEnable)
        {
            PrintLog("Attack...");
        }

        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override int GetNodeArgNum()
    {
        return 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_Attack;
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
