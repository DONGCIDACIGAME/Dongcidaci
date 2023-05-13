using System.Collections.Generic;

public class AgentStatus_Transfer : AgentStatus
{
    /// <summary>
    /// 自定义动画驱动器
    /// </summary>
    protected CustomAnimDriver mCustomAnimDriver;

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
        mCustomAnimDriver = new CustomAnimDriver(mAgent, GetStatusName());
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        if(mCustomAnimDriver != null)
        {
            mCustomAnimDriver.Dispose();
            mCustomAnimDriver = null;
        }
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);
        mStateDuration = 0;
        mTime = 0;
        if (context != null)
        {
            if (context.TryGetValue("stateName", out object obj1))
            {
                string stateName = obj1 as string;
                mCustomAnimDriver.PlayAnimState(stateName);
            }

            if (context.TryGetValue("duration", out object obj2))
            {
                float duration = (float)obj2;
                mStateDuration = MeterManager.Ins.GetCurrentMeterTime() * duration;
            }
        }

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CommandHandleOnMeter(int meterIndex)
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
