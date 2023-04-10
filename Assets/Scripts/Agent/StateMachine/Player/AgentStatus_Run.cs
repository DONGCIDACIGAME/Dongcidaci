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
        if (CommonHandleOnCmd(cmds, AgentCommandDefine.ATTACK_HARD, AgentStatusDefine.ATTACK))
            return;

        cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.IDLE);
        //if (CommonHandleOnCmd(cmds, AgentCommandDefine.IDLE, AgentStatusDefine.IDLE))
            //return;

        //if(cmds.HasCommand(AgentCommandDefine.IDLE))
        //{
        //    ChangeStatus(AgentStatusDefine.IDLE);
        //    return;
        //}    

        //cmdBuffer.MergeCommand(cmds, AgentCommandDefine.RUN);
        //cmdBuffer.MergeCommand(cmds, AgentCommandDefine.IDLE);
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        byte command = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", command, meterIndex);

        if (command == AgentCommandDefine.ATTACK_HARD)
        {
            ChangeStatus(AgentStatusDefine.ATTACK);
            return;
        }

        //switch (command)
        //{
        //    case AgentCommandDefine.ATTACK_HARD:
        //        ChangeStatus(AgentStatusDefine.ATTACK);
        //        return;
        //    case AgentCommandDefine.IDLE:
        //        ChangeStatus(AgentStatusDefine.IDLE);
        //        return;
        //    case AgentCommandDefine.RUN:
        //    case AgentCommandDefine.EMPTY:
        //    default:
        //        break;
        //}

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

        if (command == AgentCommandDefine.IDLE)
        {
            ChangeStatus(AgentStatusDefine.IDLE);
            return;
        }

        AnimQueueMoveOn();
    }


    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }
}
