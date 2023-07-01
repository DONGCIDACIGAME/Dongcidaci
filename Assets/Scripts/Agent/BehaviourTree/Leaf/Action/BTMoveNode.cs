using UnityEngine;

public abstract class BTMoveNode : BTLeafNode
{
    protected Vector3 GetTargetTowards()
    {
        if (mContext == null)
        {
            Log.Error(LogLevel.Normal, "GetTargetTowards Error, mContext is null!");
            return DirectionDef.none;
        }

        if (mContext.TryGetValue("TargetTowards", out object obj))
        {
            Vector3 towards = (Vector3)obj;

            if (towards != null)
            {
                return towards;
            }
        }

        Log.Error(LogLevel.Normal, "GetTargetTowards Error,Fail to get target towards!");
        return DirectionDef.none;
    }

    protected void Move()
    {
        // 获取运动方向
        Vector3 towards = GetTargetTowards();
        // 发送run指令
        AgentInputCommand runCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        runCmd.Initialize(AgentCommandDefine.RUN, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
        mExcutor.OnCommand(runCmd);
    }

    //protected void StopMove()
    //{
    //    // 发送idle指令
    //    AgentInputCommand idleCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
    //    idleCmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, DirectionDef.none);
    //    mExcutor.OnCommand(idleCmd);
    //}
}
