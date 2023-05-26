using GameEngine;
using System.Collections.Generic;

/// <summary>
/// 效果执行器
/// </summary>
public class EffectsExcutor : IGameUpdate, IMeterHandler
{
    private Agent mAgt;
    private HashSet<RectEffectExcutor> effectExcutors;
    private Stack<RectEffectExcutor> toRemoves;

    public void Initialize(Agent agt)
    {
        effectExcutors = new HashSet<RectEffectExcutor>();
        toRemoves = new Stack<RectEffectExcutor>();
        mAgt = agt;
    }


    public void Start(TriggeredComboStep triggeredComboStep)
    {
        if(triggeredComboStep == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, combo is null!");
            return;
        }

        ComboStep comboStep = triggeredComboStep.comboStep;
        if (comboStep == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, ComboActionData is null!");
            return;
        }

        if (mAgt == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, mAgt is null!");
            return;
        }

        AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgt, comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
        if(stateInfo == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed,agentId:{0}, status:{1} can not find state:{2}!", mAgt.GetAgentId(), comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
            return;
        }

        // 总时长
        float totalTime = MeterManager.Ins.GetTimeToMeter(stateInfo.stateMeterLen);

        HitPointInfo[] hitPoints = stateInfo.hitPoints;
        if(hitPoints == null || hitPoints.Length == 0)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hitPoints is null or empty, agentId:{0}, status:{1}, state:{2}!", mAgt.GetAgentId(), comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
            return;
        }

        if(comboStep.agentActionData.effectCollictions == null || comboStep.agentActionData.effectCollictions.Length == 0)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hit point effects is null or empty, comboName:{0}, action Index:{1}!", triggeredComboStep.comboData.comboName, triggeredComboStep.stepIndex);
            return;
        }

        if(hitPoints.Length != comboStep.agentActionData.effectCollictions.Length)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hitPoints.Length != actionData.effects.Length, ");
            return;
        }

        for(int i = 0; i < hitPoints.Length; i++)
        {
            // 第i个hit点的信息
            HitPointInfo hitpoint = hitPoints[i];
            // 第i个hit点的效果
            EffectCollection hitEffect = comboStep.agentActionData.effectCollictions[i];

            for(int j = 0; j < hitEffect.effects.Length; j++)
            {
                RectEffectExcutor excutor = GamePoolCenter.Ins.RectEffectExcutorPool.Pop();
                excutor.Initialize(mAgt, hitpoint.progress * totalTime, hitEffect.effects[j]);
                effectExcutors.Add(excutor);
            }
        }
    }

    public void Reset()
    {
        foreach (RectEffectExcutor excutor in effectExcutors)
        {
            excutor.Recycle();
        }

        effectExcutors.Clear();
    }

    public void Dispose()
    {
        Reset();
        toRemoves = null;
        effectExcutors = null;
        mAgt = null;
    }

    public void OnUpdate(float deltaTime)
    {
        toRemoves.Clear();

        foreach(RectEffectExcutor excutor in effectExcutors)
        {
            excutor.OnUpdate(deltaTime);

            if(!excutor.Active)
            {
                toRemoves.Push(excutor);
            }
        }

        while(toRemoves.TryPop(out RectEffectExcutor excutor))
        {
            effectExcutors.Remove(excutor);
        }
    }

    public void OnMeterEnter(int meterIndex)
    {
        
    }

    public void OnMeterEnd(int meterIndex)
    {
        
    }
}