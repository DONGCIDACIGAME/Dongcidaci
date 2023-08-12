using System.Collections.Generic;

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
        { AgentCommandDefine.INSTANT_ATTACK, AgentStatusDefine.ATTACK},
        { AgentCommandDefine.ACCUMULATING_ATTACK_START, AgentStatusDefine.ATTACK},
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
}
