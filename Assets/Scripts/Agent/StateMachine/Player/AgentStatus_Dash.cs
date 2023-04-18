using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Dash : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.DASH;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Dash(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        StartAnimQueue();

        mAgent.MoveControl.Dash(mAgent.GetDashDistance(), GamePlayDefine.DashDuration);
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetAnimQueue();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        if (cmd.CmdType == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.ATTACK_HARD)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.RUN)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.DASH)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.IDLE)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else
        {
            Log.Error(LogLevel.Info, "AgentStatus_Dash - undefined cmd handle:{0}", cmd);
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        if (_cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.RUN:
                    ExcuteCommand(cmdType, towards);
                    return;
                case AgentCommandDefine.DASH:
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            AnimQueueMoveOn();
        }
    }
}
