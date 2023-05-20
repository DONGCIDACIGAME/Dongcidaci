using GameEngine;
using System;
using System.Collections.Generic;

public class MeterTimerCenter : MeterModuleManager<MeterTimerCenter>
{
    private Stack<MeterTimer> mTimerPool;
    private HashSet<MeterTimer> mWorkingTimers;
    private HashSet<MeterTimer> toPushTimers;
    private HashSet<MeterTimer> toWorkTimers;

    public override void Initialize()
    {
        mTimerPool = new Stack<MeterTimer>();
        mWorkingTimers = new HashSet<MeterTimer>();
        toPushTimers = new HashSet<MeterTimer>();
        toWorkTimers = new HashSet<MeterTimer>();

        MeterManager.Ins.RegisterMeterHandler(this);
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

        if (!mWorkingTimers.Contains(timer))
            return;

        mWorkingTimers.Remove(timer);
        mTimerPool.Push(timer);
    }


    public override void Dispose()
    {
        MeterManager.Ins.UnregiseterMeterHandler(this);
        mTimerPool = null;
        mWorkingTimers = null;
        toPushTimers = null;
        toWorkTimers = null;
    }

    public override void OnMeterEnter(int meterIndex)
    {
        foreach (MeterTimer timer in mWorkingTimers)
        {
            timer.OnMeterEnter(meterIndex);

            // 定时器结束时，放入列表中等待回收
            if (timer.CheckEnd())
            {
                toPushTimers.Add(timer);
            }
        }
    }

    public override void OnMeterEnd(int meterIndex)
    {

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
            // 把已结束的节拍定时器放入缓存池
            foreach (MeterTimer timer in toPushTimers)
            {
                RemoveTimer(timer);
            }
            toPushTimers.Clear();
        }

    }
}
