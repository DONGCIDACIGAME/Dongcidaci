using System.Collections.Generic;

public class AgentStatus_Transfer : AgentStatus
{
    /// <summary>
    /// 状态持续时间
    /// </summary>
    private float mStateDuration;

    /// <summary>
    /// 计时
    /// </summary>
    private float mTime;

    public override string GetStatusName()
    {
        return AgentStatusDefine.TRANSFER;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Transfer(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);
        mStateDuration = 0;
        mTime = 0;
        if (context != null)
        {
            if (context.TryGetValue("duration", out object obj))
            {
                float duration = (float)obj;
                mStateDuration = MeterManager.Ins.GetCurrentMeterTime() * duration;
            }
        }

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mTime += deltaTime;

        if(mTime >= mStateDuration)
        {
            ChangeStatus(AgentStatusDefine.IDLE, null);
        }
    }

}
