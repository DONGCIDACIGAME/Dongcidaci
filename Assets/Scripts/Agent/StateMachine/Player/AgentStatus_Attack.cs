using System.Collections.Generic;
/// <summary>
/// ÖØ»÷×´Ì¬
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
        //Log.Logic(LogLevel.Info, "AgentStatus_AttackHard OnCommands:{0}", cmds.GetBuffer());
        //cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.ATTACK_HARD);
        //cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.RUN);
        //cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.IDLE);

        byte cmd = cmds.PeekCommand();
        if(cmd == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else
        {
            DelayToMeterExcuteCommand(cmd);
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
                ExcuteCommand(cmd);
                return;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
            case AgentCommandDefine.EMPTY:
            default:
                break;
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
