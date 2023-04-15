using System.Collections.Generic;
/// <summary>
/// 重击状态
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

    public override void OnCommands(AgentCommandBuffer cmds)
    {
        byte cmd = cmds.PeekCommand();
        if (cmd == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.DASH)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.ATTACK_HARD)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.RUN)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.IDLE)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if(cmd == AgentCommandDefine.EMPTY)
        {
            // EMTPY 里什么都不做
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

        byte cmd = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmd);
        switch(cmd)
        {
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
                ExcuteCommand(cmd);
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
