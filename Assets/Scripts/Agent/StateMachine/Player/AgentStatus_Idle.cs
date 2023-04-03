using System.Collections.Generic;

public class AgentStatus_Idle : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void OnAction(byte command)
    {
        switch (command)
        {
            case AgentCommandDefine.IDLE:
                AddCommand(command);
                break;
            case AgentCommandDefine.RUN:
                ChangeStatus(AgentStatusDefine.RUN);
                break;
            case AgentCommandDefine.ATTACK_HARD:
                ChangeStatus(AgentStatusDefine.ATTACK_HARD);
                break;
            default:
                break;
        }
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

        if (HasCommand(AgentCommandDefine.IDLE))
        {
            AnimQueueMoveOn();
        }
    }
}
