using System.Collections.Generic;
using UnityEngine;

public abstract class HeroStatus : AgentStatus
{

    /// <summary>
    /// 输入处理器
    /// </summary>
    protected IInputHandle mInputHandle;

    /// <summary>
    /// 指令到角色状态间的切换关系
    /// </summary>
    private Dictionary<int, string> CmdToStatusMap = new Dictionary<int, string>
    {
        { AgentCommandDefine.IDLE, AgentStatusDefine.IDLE},
        { AgentCommandDefine.RUN, AgentStatusDefine.RUN},
        { AgentCommandDefine.DASH, AgentStatusDefine.DASH},
        { AgentCommandDefine.INSTANT_ATTACK, AgentStatusDefine.INSTANT_ATTACK},
        { AgentCommandDefine.METER_ATTACK, AgentStatusDefine.METER_ATTACK},
        { AgentCommandDefine.CHARGING, AgentStatusDefine.CHARGING},
        { AgentCommandDefine.CHARGING_ATTACK, AgentStatusDefine.CHARGING_ATTACK},
        { AgentCommandDefine.BE_HIT, AgentStatusDefine.BEHIT},
        { AgentCommandDefine.BE_HIT_BREAK, AgentStatusDefine.BEHIT},
        { AgentCommandDefine.DEAD, AgentStatusDefine.DEAD},
    };

    /// <summary>
    /// 根据指令获取要切换到的状态
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public override string GetChangeToStatus(int cmdType)
    {
        if (CmdToStatusMap.TryGetValue(cmdType, out string statusName))
        {
            return statusName;
        }

        return string.Empty;
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
        mInputHandle = null;
    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);
    }
}
