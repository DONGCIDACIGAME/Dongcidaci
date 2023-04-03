public class AgentStatus_AttackHard : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK_HARD;
    }

    public override void OnAction(byte command)
    {
        switch (command)
        {
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.ATTACK_HARD:
                AddCommand(command);
                break;
            default:
                break;
        }
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        byte command = PeekCommand();
        switch(command)
        {
            case AgentCommandDefine.IDLE:
                ChangeStatus(AgentStatusDefine.IDLE);
                return;
            case AgentCommandDefine.RUN:
                ChangeStatus(AgentStatusDefine.RUN);
                return;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

        AnimQueueMoveOn();
    }
}
