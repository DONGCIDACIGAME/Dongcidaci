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
        cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.ATTACK_HARD);
        cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.RUN);
        cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.IDLE);
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        byte command = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}", command);
        switch(command)
        {
            case AgentCommandDefine.IDLE:
                ChangeStatus(AgentStatusDefine.IDLE);
                return;
            case AgentCommandDefine.RUN:
                ChangeStatus(AgentStatusDefine.RUN);
                return;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

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
