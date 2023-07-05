using GameEngine;
using System.Collections.Generic;

/// <summary>
/// 动画移动执行控制器
/// </summary>
public class AnimMovementExcutorController : IGameUpdate, IMeterHandler
{
    private Agent mAgt;
    private HashSet<AnimMovementExcutor> mMovementExcutors;
    private bool mEnable;

    public void Initialize(Agent agt)
    {
        mMovementExcutors = new HashSet<AnimMovementExcutor>();
        mAgt = agt;
        mEnable = false;
    }

    public void Start(string statusName, string stateName, Movement[] movements)
    {
        if (mAgt == null)
        {
            Log.Error(LogLevel.Normal, "Start Movements Excutor Failed, mAgt is null!");
            return;
        }

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "Start Movements Excutor Failed, statusName is null!");
            return;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "Start Movements Excutor Failed, stateName is null!");
            return;
        }

        AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgt, statusName, stateName);
        if (stateInfo == null)
        {
            Log.Error(LogLevel.Normal, "Start Movements Excutor Failed,agentId:{0}, status:{1} can not find state:{2}!", mAgt.GetAgentId(), statusName, stateName);
            return;
        }

        HitPointInfo[] hitPoints = stateInfo.hitPoints;
        if (hitPoints == null || hitPoints.Length == 0)
        {
            //Log.Error(LogLevel.Normal, "Start Movements Excutor Failed, hitPoints is null or empty, agentId:{0}, status:{1}, state:{2}!", mAgt.GetAgentId(), comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
            return;
        }

        if (movements == null || movements.Length == 0)
        {
            //Log.Error(LogLevel.Normal, "Start Movements Excutor Failed, effectCollictions is null or empty!");
            return;
        }

        // 总时长
        float totalTime = MeterManager.Ins.GetTimeToMeter(stateInfo.stateMeterLen);
        if (hitPoints.Length != movements.Length)
        {
            Log.Error(LogLevel.Normal, "Start Movements Excutor Failed, hitPoints.Length != actionData.effects.Length, ");
            return;
        }

        for (int i = 0; i < hitPoints.Length; i++)
        {
            // 第i个hit点的信息
            HitPointInfo hitpoint = hitPoints[i];
            // 第i个hit点的效果
            Movement movement = movements[i];

            if (movement != null)
            {
                AnimMovementExcutor excutor = GamePoolCenter.Ins.MovementExcutorPool.Pop();

                excutor.Initialize(mAgt, hitpoint.progress * totalTime, movement.distance, totalTime * movement.duration);
                mMovementExcutors.Add(excutor);
            }
        }

        mEnable = true;
    }

    public void Reset()
    {
        foreach (AnimMovementExcutor excutor in mMovementExcutors)
        {
            excutor.Recycle();
        }

        mMovementExcutors.Clear();
        mEnable = false;
    }

    public void Dispose()
    {
        Reset();
        mMovementExcutors = null;
        mAgt = null;
    }

    public void OnUpdate(float deltaTime)
    {
        if (!mEnable)
            return;

        int totalActiveExcutor = 0;
        foreach (AnimMovementExcutor excutor in mMovementExcutors)
        {
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
