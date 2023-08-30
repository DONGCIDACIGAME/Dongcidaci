using System.Collections.Generic;
/// <summary>
/// 蓄力攻击触发器
/// </summary>
public class ChargeAttackTrigger
{
    private AgentActionData mChargeActionData;
    private List<ChargeAttackStep> mSortChargeAttackSteps;

    public void Initialize(Agent agent, string chargeStateName)
    {
        if (agent == null)
        {
            Log.Error(LogLevel.Normal, "ChargeAttackTrigger Initialize Error, agent is null!");
            return;
        }


        AttackBehaviourData attackBehaviourDatas = DataCenter.Ins.AgentAtkBehaviourDataCenter.GetAgentAtkBehaviourData(agent.GetAgentId());
        if (attackBehaviourDatas == null)
        {
            Log.Error(LogLevel.Normal, "ChargeAttackTrigger Initialize Error, AttackBehaviourData is null! " + agent.GetName());
            return;
        }

        if (attackBehaviourDatas.chargeAttackDatas == null)
        {
            Log.Error(LogLevel.Normal, "ChargeAttackTrigger Initialize Error, chargeAttackDatas is null, agentId:{0},  agentName:{1}", attackBehaviourDatas.agentId, attackBehaviourDatas.agentName);
            return;
        }

        mSortChargeAttackSteps = new List<ChargeAttackStep>();

        for (int i = 0; i < attackBehaviourDatas.chargeAttackDatas.Length; i++)
        {
            ChargeAttackData chargeAttackData = attackBehaviourDatas.chargeAttackDatas[i];
            if (chargeAttackData == null)
            {
                Log.Error(LogLevel.Normal, "ChargeAttackTrigger Initialize Error, null chargeAttackData at index {0}, agent id:{1}, agent Name:{2}", i, attackBehaviourDatas.agentId, attackBehaviourDatas.agentName);
                continue;
            }

            if (!chargeAttackData.chargeActionData.stateName.Equals(chargeStateName))
                continue;

            if (chargeAttackData.ChargeAttackSteps == null || chargeAttackData.ChargeAttackSteps.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ChargeAttackTrigger Initialize Error, chargeAttackData [{0}] ChargeAttackSteps is null or emtpty, agent id:{1}, agent Name:{2}", chargeAttackData.chargeActionData.stateName, attackBehaviourDatas.agentId, attackBehaviourDatas.agentName);
                continue;
            }

            mChargeActionData = chargeAttackData.chargeActionData;
            for (int j = 0; j < chargeAttackData.ChargeAttackSteps.Length; j++)
            {
                mSortChargeAttackSteps.Add(chargeAttackData.ChargeAttackSteps[j]);
            }
        }

        for(int i = 0; i < mSortChargeAttackSteps.Count-1; i++)
        {
            for(int j = i+1;j<mSortChargeAttackSteps.Count;j++)
            {
                ChargeAttackStep step1 = mSortChargeAttackSteps[i];
                ChargeAttackStep step2 = mSortChargeAttackSteps[j];

                if(step1.chargeMeterLen > step2.chargeMeterLen)
                {
                    mSortChargeAttackSteps[i] = step2;
                    mSortChargeAttackSteps[j] = step1;
                }
            }
        }
    }

    public AgentActionData GetChargeActionData()
    {
        return mChargeActionData;
    }

    public ChargeAttackStep Trigger(int meterLen)
    {
        for(int i = mSortChargeAttackSteps.Count-1; i >= 0; i--)
        {
            ChargeAttackStep attackStep = mSortChargeAttackSteps[i];
            if(meterLen >= attackStep.chargeMeterLen)
            {
                return attackStep;
            }
        }

        return null;
    }

    public ChargeAttackStep GetMaxChargeStep()
    {
        if (mSortChargeAttackSteps == null || mSortChargeAttackSteps.Count == 0)
            return null;

        return mSortChargeAttackSteps[mSortChargeAttackSteps.Count - 1];
    }

}