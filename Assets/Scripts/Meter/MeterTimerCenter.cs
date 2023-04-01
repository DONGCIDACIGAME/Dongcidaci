using GameEngine;
using System;
using System.Collections.Generic;

public class MeterTimerCenter : MeterModuleManager<MeterTimerCenter>, IMeterHandler
{
    private Stack<MeterTimer> mTimerPool;
    private HashSet<MeterTimer> mWorkingTimers;
    private List<MeterTimer> toPushTimers;
    private List<MeterTimer> toWorkTimers;

    public override void Initialize()
    {
        mTimerPool = new Stack<MeterTimer>();
        mWorkingTimers = new HashSet<MeterTimer>();
        toPushTimers = new List<MeterTimer>();
        toWorkTimers = new List<MeterTimer>();

        MeterManager.Ins.RegisterBaseMeterHandler(this);
    }

    private MeterTimer PopTimer()
    {
        MeterTimer timer;
        if (mTimerPool.Count == 0)
        {
            timer = new MeterTimer();
        }
        else
        {
            timer = mTimerPool.Pop();
        }

        toWorkTimers.Add(timer);
        return timer;
    }

    public MeterTimer SetTimer(int meterOffset, int loopTime)
    {
        MeterTimer timer = PopTimer();
        timer.SetTimer(meterOffset, loopTime);
        return timer;
    }

    public MeterTimer SetTimer(int meterOffset, int loopTime, Action cb)
    {
        MeterTimer timer = PopTimer();
        timer.SetTimer(meterOffset, loopTime, cb);
        return timer;
    }

    public void RemoveTimer(MeterTimer timer)
    {
        if (timer == null)
            return;

        mWorkingTimers.Remove(timer);
        mTimerPool.Push(timer);

        Log.Error(LogLevel.Info, "push timer:{0}, working Timer Count:{1}", timer, mWorkingTimers.Count);
    }


    public override void Dispose()
    {
        mTimerPool = null;
        mWorkingTimers = null;
        toPushTimers = null;
        toWorkTimers = null;
    }

    public uint GetMeterOffset()
    {
        return 1;
    }

    public override void OnMeter(int meterIndex)
    {
        foreach (MeterTimer timer in mWorkingTimers)
        {
            timer.OnMeter(meterIndex);

            // ��ʱ������ʱ�������б��еȴ�����
            if (timer.CheckEnd())
            {
                toPushTimers.Add(timer);
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(toWorkTimers.Count > 0)
        {
            foreach (MeterTimer timer in toWorkTimers)
            {
                mWorkingTimers.Add(timer);
            }
            toWorkTimers.Clear();
        }

        if(toPushTimers.Count > 0)
        {
            // ���ѽ����Ľ��Ķ�ʱ�����뻺���
            foreach (MeterTimer timer in toPushTimers)
            {
                RemoveTimer(timer);
            }
            toPushTimers.Clear();
        }

    }
}
