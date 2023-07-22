public abstract class HeroStatus : AgentStatus
{
    /// <summary>
    /// 根据指令获取要切换到的状态
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public override string GetChangeToStatus(byte cmdType)
    {
        switch(cmdType)
        {
            case AgentCommandDefine.IDLE:
                return AgentStatusDefine.IDLE;
            case AgentCommandDefine.RUN:
                return AgentStatusDefine.RUN;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                if(mAgent.Agent_View.InstantAttack)
                {
                    return AgentStatusDefine.INSTANT_ATTACK;
                }
                else
                {
                    return AgentStatusDefine.ATTACK;
                }
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
}
