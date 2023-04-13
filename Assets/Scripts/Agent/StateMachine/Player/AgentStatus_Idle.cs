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
        byte cmd = cmds.PeekCommand();

        if (cmd == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.ATTACK_HARD)
        {
            ProgressWaitOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd);
        }
        else if (cmd == AgentCommandDefine.RUN)
        {
            ExcuteCommand(cmd);
        }
        else if(cmd == AgentCommandDefine.IDLE)
        {
            DelayToMeterExcuteCommand(cmd);
        }

        //if (CommonHandleOnCmd(cmds, AgentCommandDefine.ATTACK_HARD, AgentStatusDefine.ATTACK))
        //    return;

        //if (cmds.HasCommand(AgentCommandDefine.RUN))
        //    ChangeStatus(AgentStatusDefine.RUN);
        //if (CommonHandleOnCmd(cmds, AgentCommandDefine.RUN, AgentStatusDefine.RUN))
        //    return;
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        byte cmd = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmd);
        switch (cmd)
        {
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.ATTACK_HARD:
                ExcuteCommand(cmd);
                return;
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }

        if (meterIndex < mCurAnimStateEndMeter)
            return;

        AnimQueueMoveOn();
    }
}
