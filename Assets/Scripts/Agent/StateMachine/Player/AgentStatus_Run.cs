using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Run : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Run(mAgent);
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
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.DASH)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.IDLE)
        {
            ExcuteCommand(cmd);
        }
        else
        {
            Log.Error(LogLevel.Info, "AgentStatus_Run - undefined cmd handle:{0}", cmd);
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        byte cmd = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmd, meterIndex);

        switch (cmd)
        {
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.DASH:
                ExcuteCommand(cmd);
                return;
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }

        if (meterIndex < mCurAnimStateEndMeter)
            return;

        AnimQueueMoveOn();
    }


    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }
}
