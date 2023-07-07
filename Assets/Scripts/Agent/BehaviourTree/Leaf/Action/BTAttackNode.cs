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
        attackCmd.Initialize(AgentCommandDefine.ATTACK_SHORT, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
        mExcutor.OnCommand(attackCmd);


        // 发送behit指令
        if (mContext.TryGetValue("TargetEntity", out object obj))
        {
            Agent targetEntity = obj as Agent;
            AgentCommand beHitCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            beHitCmd.AddArg("moveMove", 1);
            beHitCmd.Initialize(AgentCommandDefine.BE_HIT, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
            targetEntity.OnCommand(attackCmd);
        }

        PrintLog("Attack...");
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
