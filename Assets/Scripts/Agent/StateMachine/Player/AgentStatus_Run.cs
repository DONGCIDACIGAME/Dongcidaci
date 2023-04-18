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

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if(_cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmdType, meterIndex);
            switch (cmdType)
            {
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.DASH:
                    ExcuteCommand(cmdType, towards);
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


    }


    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }
}
