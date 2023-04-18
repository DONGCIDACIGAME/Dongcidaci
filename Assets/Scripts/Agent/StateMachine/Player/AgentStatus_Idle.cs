using System.Collections.Generic;
using UnityEngine;

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

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        if (cmd.CmdType == AgentCommandDefine.BE_HIT)
        {
            
        }
        else if (cmd.CmdType == AgentCommandDefine.ATTACK_HARD || cmd.CmdType == AgentCommandDefine.ATTACK_LIGHT)
        {
            
        }
        else if (cmd.CmdType == AgentCommandDefine.RUN)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.DASH)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.IDLE)
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
        if (_cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ExcuteCommand(cmdType, towards);
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
}
