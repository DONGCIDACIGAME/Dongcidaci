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
        else if (cmd == AgentCommandDefine.ATTACK_HARD || cmd == AgentCommandDefine.ATTACK_LIGHT)
        {
            ProgressWaitOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd);
        }
        else if (cmd == AgentCommandDefine.RUN)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.DASH)
        {
            ExcuteCommand(cmd);
        }
        else if(cmd == AgentCommandDefine.IDLE)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else
        {
            Log.Error(LogLevel.Info, "AgentStatus_Idle - undefined cmd handle:{0}", cmd);
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        byte cmd = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmd);
        switch (cmd)
        {
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.BE_HIT:
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
