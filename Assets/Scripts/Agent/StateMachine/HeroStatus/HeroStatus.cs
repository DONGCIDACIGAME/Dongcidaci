using System.Collections.Generic;
using UnityEngine;

public abstract class HeroStatus : AgentStatus
{

    /// <summary>
    /// 输入处理器
    /// </summary>
    protected Stack<IInputHandle> mInputHandles;
    protected IInputHandle mInputHandle;

    /// <summary>
    /// 根据指令获取要切换到的状态
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public override string GetChangeToStatus(int cmdType)
    {
        switch(cmdType)
        {
            case AgentCommandDefine.IDLE:
                return AgentStatusDefine.IDLE;
            case AgentCommandDefine.RUN:
                return AgentStatusDefine.RUN;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                return AgentStatusDefine.ATTACK;
            case AgentCommandDefine.ATTACK_SHORT_INSTANT:
            case AgentCommandDefine.ATTACK_LONG_INSTANT:
                return AgentStatusDefine.INSTANT_ATTACK;
            case AgentCommandDefine.BE_HIT:
                return AgentStatusDefine.BEHIT;
            case AgentCommandDefine.DASH:
                return AgentStatusDefine.DASH;
            case AgentCommandDefine.DEAD:
                return AgentStatusDefine.DEAD;
            default:
                return string.Empty;
        }
    }

    public abstract void RegisterInputHandle();
    public abstract void UnregisterInputHandle();

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        RegisterInputHandle();
        mInputHandle.SetEnable(false);
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        mInputHandle.SetEnable(true);
    }

    public override void OnExit()
    {
        base.OnExit();

        mInputHandle.SetEnable(false);
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();

        UnregisterInputHandle();
        mInputHandles = null;

        foreach(InputHandle inputHandle in mInputHandles)
        {

        }
    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);
    }
}
