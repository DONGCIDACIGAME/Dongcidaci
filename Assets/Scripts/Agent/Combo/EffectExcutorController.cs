using GameEngine;
using GameSkillEffect;
using System.Collections.Generic;

/// <summary>
/// 效果执行器
/// </summary>
public class EffectExcutorController : IGameUpdate, IMeterHandler
{
    private Agent mAgt;
    //Changed by weng 0704
    private List<ComboEffectExcutor> effectExcutors;
    private bool mEnable;

    public void Initialize(Agent agt)
    {
        //Changed by weng 0704
        effectExcutors = new List<ComboEffectExcutor>();
        mAgt = agt;
        mEnable = false;
    }

    
    public void Start(string statusName, string stateName, int comboUID, ComboType comboType, bool isLastStep ,SkEftDataCollection[] effectCollictions)
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
            // changed by weng 0708
            SkEftDataCollection hitEffects = effectCollictions[i];
            var rlsEffects = new List<SkillEffect>();
            if(hitEffects.effects != null)
            {
                foreach (var hitEftData in hitEffects.effects)
                {
                    var rlsSkEft = GameSkEftPool.Ins.PopWithInit(mAgt,hitEftData);
                    if (rlsSkEft != null) rlsEffects.Add(rlsSkEft);
                }
            }
            // 生成这个combo hit 的数据
            var comboHitData = new ComboHitEffectsData(comboUID,comboType,isLastStep,rlsEffects,hitEffects.rlsFxCfg,hitEffects.hitFxCfg);
            //Changed by weng 0704
            //此处应该是个错误，对应的ComboEffectExcutor执行的应该是SkEftDataCollection
            ComboEffectExcutor excutor = GamePoolCenter.Ins.ComboEffectExcutorPool.Pop();
            
            if(hitpoint.timeType == TimeDefine.TimeType_MeterRelated)// 关联节拍的
            {
                // 总时长
                float totalTime = MeterManager.Ins.GetTimeToMeterWithOffset(stateInfo.meterLen);
                excutor.Initialize(mAgt, hitpoint.progress * totalTime, comboHitData);
            }
            else if(hitpoint.timeType == TimeDefine.TimeType_AbsoluteTime)// 绝对时间的
            {
                excutor.Initialize(mAgt, hitpoint.progress, comboHitData);
            }
            effectExcutors.Add(excutor);

            /** old code
            for(int j = 0; j < hitEffect.effects.Length; j++)
            {
                ComboEffectExcutor excutor = GamePoolCenter.Ins.ComboEffectExcutorPool.Pop();
                // modified by weng 0704 
                //excutor.Initialize(mAgt, hitpoint.progress * totalTime, hitEffect.effects[j]);
                excutor.Initialize(mAgt, hitpoint.progress * totalTime, hitEffect.effects[j]);
                effectExcutors.Add(excutor);
            }
            */
        }

        mEnable = true;
    }

    public void Reset()
    {
        foreach (ComboEffectExcutor excutor in effectExcutors)
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
        for(int i = 0;i<effectExcutors.Count;i++)
        {
            ComboEffectExcutor excutor = effectExcutors[i];

            excutor.OnUpdate(deltaTime);

            if (excutor.active)
            {
                totalActiveExcutor++;
            }
        }

        if (totalActiveExcutor == 0)
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
