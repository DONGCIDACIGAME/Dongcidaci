using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ػ�״̬
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    private ComboHandler mComboHandler;

    public override void CustomInitialize()
    {
        mComboHandler = new ComboHandler();
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
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
    }

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        if (cmd.CmdType == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.DASH)
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
        else if (cmd.CmdType == AgentCommandDefine.IDLE)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.EMPTY)
        {
            // EMTPY ��ʲô������
        }
        else
        {
            Log.Error(LogLevel.Info, "AgentStatus_Attack - undefined cmd handle:{0}", cmd);
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        if (_cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ExcuteCommand(cmdType, towards);
                    return;
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.ATTACK_LIGHT:
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    return;
            }

            AnimQueueMoveOn();
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mComboHandler.OnUpdate(deltaTime);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }
}
