using System.Collections.Generic;

public class AgentStatus_Run : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
    }

    public override void OnAction(byte command)
    {
        switch(command)
        {
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.RUN:
                AddCommand(command);
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

        byte command = PeekCommand();
        if(command == AgentCommandDefine.IDLE)
        {
            ChangeStatus(AgentStatusDefine.IDLE);
            return;
        }

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ���ļ�¼����
        mCurAnimStateMeterRecord = 0;

        AnimQueueMoveOn();
    }

}
