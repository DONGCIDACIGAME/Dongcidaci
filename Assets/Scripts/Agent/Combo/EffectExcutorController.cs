using GameEngine;
using System.Collections.Generic;

/// <summary>
/// 效果执行器
/// </summary>
public class EffectExcutorController : IGameUpdate, IMeterHandler
{
    private Agent mAgt;
    private HashSet<RectEffectExcutor> effectExcutors;
    private bool mEnable;

    public void Initialize(Agent agt)
    {
        effectExcutors = new HashSet<RectEffectExcutor>();
        mAgt = agt;
        mEnable = false;
    }

    
    public void Start(string statusName, string stateName, EffectCollection[] effectCollictions)
    {
        if (mAgt == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, mAgt is null!");
            return;
        }

        if(string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, statusName is null!");
            return;
        }

        if(string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, stateName is null!");
            return;
        }

        AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgt, statusName, stateName);
        if(stateInfo == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed,agentId:{0}, status:{1} can not find state:{2}!", mAgt.GetAgentId(), statusName, stateName);
            return;
        }

        // 总时长
        float totalTime = MeterManager.Ins.GetTimeToMeter(stateInfo.stateMeterLen);

        HitPointInfo[] hitPoints = stateInfo.hitPoints;
        if(hitPoints == null || hitPoints.Length == 0)
        {
            //Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hitPoints is null or empty, agentId:{0}, status:{1}, state:{2}!", mAgt.GetAgentId(), comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
            return;
        }

        if(effectCollictions == null || effectCollictions.Length == 0)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, effectCollictions is null or empty!");
            return;
        }

        if(hitPoints.Length != effectCollictions.Length)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hitPoints.Length != actionData.effects.Length, ");
            return;
        }

        for(int i = 0; i < hitPoints.Length; i++)
        {
            // 第i个hit点的信息
            HitPointInfo hitpoint = hitPoints[i];
            // 第i个hit点的效果
            EffectCollection hitEffect = effectCollictions[i];

            for(int j = 0; j < hitEffect.effects.Length; j++)
            {
                RectEffectExcutor excutor = GamePoolCenter.Ins.RectEffectExcutorPool.Pop();
                // modified by weng 0704 
                //excutor.Initialize(mAgt, hitpoint.progress * totalTime, hitEffect.effects[j]);
                excutor.Initialize(mAgt, hitpoint.progress * totalTime, hitEffect.effects[j]);
                effectExcutors.Add(excutor);
            }
        }

        mEnable = true;
    }

    public void Reset()
    {
        foreach (RectEffectExcutor excutor in effectExcutors)
        {
            excutor.Recycle();
        }

        effectExcutors.Clear();
        mEnable = false;
    }

    public void Dispose()
    {
        Reset();
        effectExcutors = null;
        mAgt = null;
    }

    public void OnUpdate(float deltaTime)
    {
        if (!mEnable)
            return;

        int totalActiveExcutor = 0;
        foreach(RectEffectExcutor excutor in effectExcutors)
        {
            excutor.OnUpdate(deltaTime);

            if (excutor.active)
            {
                totalActiveExcutor++;
            }
        }

        if(totalActiveExcutor == 0)
        {
            Reset();
        }
    }

    public void OnMeterEnter(int meterIndex)
    {
        
    }

    public void OnMeterEnd(int meterIndex)
    {
        
    }
}
