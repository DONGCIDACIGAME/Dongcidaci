using System.Collections.Generic;

public class AgentStatus_Idle : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Idle(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        StartAnimQueue();
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetAnimQueue();
    }

    public override void OnCommands(AgentCommandBuffer cmds)
    {
        if (CommonHandleOnCmd(cmds, AgentCommandDefine.ATTACK_HARD, AgentStatusDefine.ATTACK))
            return;

        if (CommonHandleOnCmd(cmds, AgentCommandDefine.RUN, AgentStatusDefine.RUN))
            return;
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        byte command = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}", command);
        switch (command)
        {
            case AgentCommandDefine.RUN:
                ChangeStatus(AgentStatusDefine.RUN);
                return;
            case AgentCommandDefine.ATTACK_HARD:
                ChangeStatus(AgentStatusDefine.ATTACK);
                return;
            case AgentCommandDefine.IDLE:
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
