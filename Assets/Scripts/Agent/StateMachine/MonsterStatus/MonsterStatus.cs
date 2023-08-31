using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStatus : AgentStatus
{
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

    /// <summary>
    /// 根据节拍进度执行
    /// 如果本拍的剩余时间占比=waitMeterProgress,就直接执指令，否则等下拍执行指令
    /// 其他情况等待下一拍执行
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboStep"></param>
    protected bool ConditionalExcute(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        if (triggerMeter <= mCurLogicStateEndMeter)
            return false;

        // 当前节拍的进度
        float progress = MeterManager.Ins.GetCurrentMeterProgress();
        if (progress <= GamePlayDefine.AttackMeterProgressWait)
        {
            float timeLost = MeterManager.Ins.GetTimePassed(triggerMeter);
            ExcuteCombo(triggerMeter, timeLost, triggeredComboStep);
            return true;
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
            return false;
        }
    }
}
