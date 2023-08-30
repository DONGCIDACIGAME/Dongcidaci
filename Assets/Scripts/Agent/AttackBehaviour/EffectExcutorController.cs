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
    private List<GameEffectExcutor> mEffectExcutors;
    private HitPointInfo[] mHitPoints;
    SkEftDataCollection[] mEffectCollictions;
    private bool mEnable;

    private int mMeterLen;
    private float mCurLoopEndTime;
    private float mTimeRecord;
    private int mLoopRecord;
    private int totalLoopTime;

    public void Initialize(Agent agt)
    {
        //Changed by weng 0704
        mEffectExcutors = new List<GameEffectExcutor>();
        mAgt = agt;
        mEnable = false;
    }

    
    public void Start(string statusName, string stateName, float timeLost, SkEftDataCollection[] effectCollictions)
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

        if (effectCollictions == null || effectCollictions.Length == 0)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, effectCollictions is null or empty!");
            return;
        }

        mEffectCollictions = effectCollictions;

        AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgt, statusName, stateName);
        if(stateInfo == null)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed,agentId:{0}, status:{1} can not find state:{2}!", mAgt.GetAgentId(), statusName, stateName);
            return;
        }

        mHitPoints = stateInfo.hitPoints;
        if(mHitPoints == null || mHitPoints.Length == 0)
        {
            //Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hitPoints is null or empty, agentId:{0}, status:{1}, state:{2}!", mAgt.GetAgentId(), comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
            return;
        }

        if(mHitPoints.Length != effectCollictions.Length)
        {
            Log.Error(LogLevel.Normal, "Start Combo Action Excutor Failed, hitPoints.Length != actionData.effects.Length, ");
            return;
        }

        Reset();
        mMeterLen = stateInfo.meterLen;
        totalLoopTime = stateInfo.loopTime;
        mEnable = true;

        GenerateOnce(timeLost);
    }


    private void GenerateOnce(float timeLost)
    {
        // 当前循环还有多长时间结束
        mCurLoopEndTime = MeterManager.Ins.GetTimeToMeterWithOffset(mMeterLen);

        for (int i = 0; i < mHitPoints.Length; i++)
        {
            // 第i个hit点的信息
            HitPointInfo hitpoint = mHitPoints[i];

            float excuteTime = 0;
            if (hitpoint.timeType == TimeDefine.TimeType_MeterRelated)// 关联节拍的
            {
                // 总时长
                float totalTime = timeLost + mCurLoopEndTime;
                if (timeLost / totalTime > hitpoint.progress)
                    continue;

                excuteTime = hitpoint.progress * totalTime - timeLost;
            }
            else if (hitpoint.timeType == TimeDefine.TimeType_AbsoluteTime)// 绝对时间的
            {
                if (timeLost > hitpoint.progress)
                    continue;

                excuteTime = hitpoint.progress - timeLost;
            }

            // 第i个hit点的效果
            // changed by weng 0708
            SkEftDataCollection hitEffects = mEffectCollictions[i];
            var rlsEffects = new List<SkillEffect>();
            if (hitEffects.effects != null)
            {
                foreach (var hitEftData in hitEffects.effects)
                {
                    var rlsSkEft = GameSkEftPool.Ins.PopWithInit(mAgt, hitEftData);
                    if (rlsSkEft != null) rlsEffects.Add(rlsSkEft);
                }
            }
            // 生成这个combo hit 的数据
            var gameEffect = new GameEffectsData(rlsEffects, hitEffects.rlsFxCfg, hitEffects.hitFxCfg);
            //Changed by weng 0704
            //此处应该是个错误，对应的ComboEffectExcutor执行的应该是SkEftDataCollection
            GameEffectExcutor excutor = GamePoolCenter.Ins.ComboEffectExcutorPool.Pop();

            excutor.Initialize(mAgt, excuteTime, gameEffect);

            mEffectExcutors.Add(excutor);
        }
    }


    public void Reset()
    {
        foreach (GameEffectExcutor excutor in mEffectExcutors)
        {
            excutor.Recycle();
        }


        mCurLoopEndTime = 0;
        mTimeRecord = 0;
        totalLoopTime = 0;
        mLoopRecord = 0;
        mMeterLen = 0;

        mEffectExcutors.Clear();
        mEnable = false;
    }

    public void Dispose()
    {
        Reset();
        mEffectExcutors = null;
        mAgt = null;
    }

    public void OnGameUpdate(float deltaTime)
    {
        if (!mEnable)
            return;


        if (mTimeRecord <= mCurLoopEndTime)
            return;

        if (totalLoopTime > 0 && mLoopRecord >= totalLoopTime)
        {
            Reset();
            return;
        }

        GenerateOnce(0);


        //int totalActiveExcutor = 0;
        //for(int i = 0;i<mEffectExcutors.Count;i++)
        //{
        //    GameEffectExcutor excutor = mEffectExcutors[i];

        //    excutor.OnGameUpdate(deltaTime);

        //    if (excutor.active)
        //    {
        //        totalActiveExcutor++;
        //    }
        //}

        //if (totalActiveExcutor == 0)
        //{
        //    Reset();
        //}
    }

    public void OnMeterEnter(int meterIndex)
    {
        
    }

    public void OnMeterEnd(int meterIndex)
    {
        
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        
    }
}
