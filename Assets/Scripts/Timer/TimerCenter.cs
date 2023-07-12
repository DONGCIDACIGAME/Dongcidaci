using GameEngine;
using System;
using System.Collections.Generic;

public class TimerCenter : ModuleManager<TimerCenter>
{
    private Stack<GameTimer> mTimerPool;
    private HashSet<GameTimer> mWorkingTimers;
    private List<GameTimer> toPushTimers;

    public override void Initialize()
    {
        mTimerPool = new Stack<GameTimer>();
        mWorkingTimers = new HashSet<GameTimer>();
        toPushTimers = new List<GameTimer>();
    }

    private GameTimer PopTimer()
    {
        GameTimer timer;
        if(mTimerPool.Count == 0)
        {
            timer = new GameTimer();
        }
        else
        {
            timer = mTimerPool.Pop();
        }

        mWorkingTimers.Add(timer);
        return timer;
    }

    public GameTimer SetTimer(float time, int loopTime)
    {
        GameTimer timer = PopTimer();
        timer.SetTimer(time, loopTime);
        return timer;
    }

    public GameTimer SetTimer(float time, int loopTime, Action cb)
    {
        GameTimer timer = SetTimer(time, loopTime);
        timer.BindAction(cb);
        return timer;
    }

    public void RemoveTimer(GameTimer timer)
    {
        if (timer == null)
            return;

        mWorkingTimers.Remove(timer);
        mTimerPool.Push(timer);
    }


    public override void Dispose()
    {
        mTimerPool = null;
        mWorkingTimers = null;
        toPushTimers = null;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        foreach(GameTimer timer in mWorkingTimers)
        {
            timer.OnUpdate(deltaTime);

            // 定时器结束时，放入列表中等待回收
            if(timer.CheckEnd())
            {
                toPushTimers.Add(timer);
            }
        }
    }


    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);

        // 回收已结束的定时器
        foreach (GameTimer timer in toPushTimers)
        {
            RemoveTimer(timer);
        }

        toPushTimers.Clear();
    }

}
